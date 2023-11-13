using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ACADprogram
{
    public class ACAD
    {
        private static Dispatcher AutocadDispatcher;

        [CommandMethod("SVS")] public static void InvokeWithWindow() =>
            TryProgramm(() =>
            {
                AutocadDispatcher = Dispatcher.CurrentDispatcher;
                Thread thread = new Thread(() => AutocadDispatcher.Invoke(() => AcadApp.ShowModelessWindow(new Window1(AutocadDispatcher))));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            },
            "new.txt");
        [CommandMethod("SVSWW")] public static void InvokeWithoutWindow() =>
            TryProgramm(() => Program(new MainWindowContext()), "new.txt");

        public static void TryProgramm(Action program, string ExcepionInfoFileName)
        {
            try
            {
                program();
            }
            catch (System.Exception ex)
            {
                File.WriteAllText($@"C:\Users\{Environment.UserName}\Desktop\{ExcepionInfoFileName}", ex.MessageExpress());
                System.Diagnostics.Process.Start("explorer", $@"C:\Users\{Environment.UserName}\Desktop\{ExcepionInfoFileName}");
                throw ex;
            }
        }

        public static void Program(MainWindowContext Context)
        {
            //Добавления документа и его базы данных
            var Pdoc = AcadApp.DocumentManager.MdiActiveDocument;
            var EdDoc = Pdoc.Editor;
            var PdDb = Pdoc.Database;

            string NumberOP = Context.NumberOpory;  //Номер опоры

            using (var ld = Pdoc.LockDocument())
            //запуск транзакции
            using (var PdTrans = PdDb.TransactionManager.StartTransaction())
            //открыти таблицы блоков для чтения
            using (var PdBlk = PdTrans.GetObject(PdDb.BlockTableId, OpenMode.ForRead) as BlockTable)
            //Открытие пространство модели таблицы блоков для записи.
            using (var PdBlkRec = PdTrans.GetObject(PdBlk[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord)
            {

                
                //Настройка визуального стиля
                ViewportTable ModSpace = (ViewportTable)PdTrans.GetObject(PdDb.ViewportTableId, OpenMode.ForRead);
                ViewportTableRecord ModSpaceRec = (ViewportTableRecord)PdTrans.GetObject(ModSpace["*Active"], OpenMode.ForWrite);
                DBDictionary Style = (DBDictionary)PdTrans.GetObject(PdDb.VisualStyleDictionaryId, OpenMode.ForRead);
                ModSpaceRec.VisualStyleId = Style.GetAt("Conceptual");
                

                //Исходные данные для отрезков(длина и координаты) 
                var Baza_Op_A=6700;
                var Baza_Op_B = 6700;


                var A = Context.LpoperekVL; //размер по Х
                var B = Context.LvdolVL; //размер по У
                var d = Context.GlubinaKotlovana; //размер по Z
                var j = Context.OtkosKotlovana;    //Уклон откосов котлована
                var SR_otkos = Context.OtkosSrezki; //Уклон откосов срезки
                var SN_otkos = Context.OtkosNasypi; //Уклон откосов насыпи
                var SN_dop = Context.LdopNasypi;   //Запас к размеру длины и ширины основания насыпи
                var h1 = Context.HReliefPoperekVLeft;  //Высота от поверхности котлована до рельефа - срезка
                var h2 = Context.HReliefPoperekVRight;  //Высота от поверхности котлована до рельефа - насыпь
                var h3 = Context.HReliefVdolVLDown;    //Высота от поверхности котлована до рельефа - срезка
                var h4 = Context.HReliefVdolVLUp;    //Высота от поверхности котлована до рельефа - насыпь
                var d1 = Context.IGE1;    //Отметка первого (верхнего) слоя ИГЭ, мм
                var d2 = Context.IGE2;    //Отметка второго слоя ИГЭ, мм
                var d3 = Context.IGE3;    //Отметка третьего слоя ИГЭ, мм
                var d4 = Context.IGE4;    //Отметка четвёптого слоя ИГЭ, мм
                var d5 = Context.IGE5;    //Отметка пятого слоя ИГЭ, мм
                var d6 = Context.IGE6;    //Отметка шестого слоя ИГЭ, мм
                var h_ponizh = Context.OtmetkaPonizh; //Величина изменения отметки положения 3D фигур (понижение или повышения центра опоры)


                

                //Вектор изменения отметки положения цетнра опоры
                var Vector_ponizh = new Vector3d(0, 0, h_ponizh);

                var linePL1 = A / 2;
                var linePL2 = B / 2;

                var linePL3 = linePL1 + d * j;
                var linePL4 = linePL2 + d * j;

                Point3d[] ptm;

                // Раздел определения координат точек определяющих рельеф
                //Расстояние от цетра котлована до точке 9-12
                var lt9101112 = 0.5 * Math.Sqrt(Math.Pow(linePL3, 2) + Math.Pow(linePL4, 2));

                //Расстояние от центра котлована до точек в углах котлована (5-8)
                var ly5678 = Math.Sqrt(Math.Pow(linePL3, 2) + Math.Pow(linePL4, 2));

                //Высота точек (9-12) между главными точками 1-4
                var h9 = 0.5 * (h1 + h3);
                var h10 = 0.5 * (h3 + h2);
                var h11 = 0.5 * (h2 + h4);
                var h12 = 0.5 * (h4 + h1);

                //Высота точек в углах котлована
                var h5 = (h9 * ly5678) / lt9101112;
                var h6 = (h10 * ly5678) / lt9101112;
                var h7 = (h11 * ly5678) / lt9101112;
                var h8 = (h12 * ly5678) / lt9101112;

                //3D точки вершин верха котлована
                
                ptm = new Point3d[8]
                    {
                        new Point3d(-linePL3, 0, (d + h1)),
                        new Point3d(linePL3, 0, (d + h2)),
                        new Point3d(0, -linePL4, (d + h4)),
                        new Point3d(0, linePL4, (d + h3)),
                        new Point3d(-linePL3, linePL4, (d + h5)),
                        new Point3d(linePL3, linePL4, (d + h6)),
                        new Point3d(linePL3, -linePL4, (d + h7)),
                        new Point3d(-linePL3, -linePL4, (d + h8)),
                    };

                //Координаты точек для построения котлована
                var pt0 = new Point3d(0, 0, 0);
                var ptd = new Point3d(0, 0, d);
                var ptd1 = new Point3d(-linePL3, 0, d);
                var ptd2 = new Point3d(linePL3, 0, d);
                var ptd3 = new Point3d(0, linePL4, d);
                var ptd4 = new Point3d(0, -linePL4, d);

                var pt5 = new Point3d(-linePL1, -linePL2, 0);
                var pt6 = new Point3d(-linePL1, linePL2, 0);
                var pt7 = new Point3d(linePL1, linePL2, 0);
                var pt8 = new Point3d(linePL1, -linePL2, 0);

                var pt9 = new Point3d(-linePL3, -linePL4, d);
                var pt10 = new Point3d(-linePL3, linePL4, d);
                var pt11 = new Point3d(linePL3, linePL4, d);
                var pt12 = new Point3d(linePL3, -linePL4, d);

                var pl2 = new VirtualPolyline3d(pt5, pt6, pt7, pt8);
                var pl3 = new VirtualPolyline3d(pt9, pt10, pt11, pt12);





                LoftOptionsBuilder loftOptionsBuilder = new LoftOptionsBuilder()
                {
                    Ruled = true
                };
                ////отрисовка твердотелого объекта
                Solid3d UP3D = new Autodesk.AutoCAD.DatabaseServices.Solid3d();
                UP3D.CreateLoftedSolid(new LoftProfile[]
                    {
                        new LoftProfile(pl2.Gets()),
                        new LoftProfile(pl3.Gets())
                    },
                new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
                UP3D.TransformBy(Matrix3d.Displacement(Vector_ponizh));
                PdBlkRec.AppendEntity(UP3D);
                PdTrans.AddNewlyCreatedDBObject(UP3D, true);

                //Отрисовка контура поверхности рельефа
                
                var pl_435 = new VirtualPolyline3d3pt(ptm[4], ptm[3], ptm[5]);
                var pl_0d1 = new VirtualPolyline3d3pt(ptm[0], ptd, ptm[1]);
                var pl_726 = new VirtualPolyline3d3pt(ptm[7], ptm[2], ptm[6]);

                LoftOptionsBuilder LoftOptionsB = new LoftOptionsBuilder()
                {
                    Ruled = true
                };

                var NewSurf = Autodesk.AutoCAD.DatabaseServices.Surface.CreateLoftedSurface(new LoftProfile[]
                {
                            new LoftProfile(pl_435.Gets()),
                            new LoftProfile(pl_0d1.Gets()),
                            new LoftProfile(pl_726.Gets()),
                },
                null, null, LoftOptionsB.ToLoftOptions());

                var pl_pts = new VirtualPolyline3d8pt(ptm[0], ptm[4], ptm[3], ptm[5], ptm[1], ptm[6], ptm[2], ptm[7]);

                ////отрисовка поверхности рельефа
                NewSurf.TransformBy(Matrix3d.Scaling(20, ptd));

                var UP3D_Relief = UP3D.Slice(NewSurf, true);
                UP3D.Erase();
                PdBlkRec.AppendEntity(UP3D_Relief);
                PdTrans.AddNewlyCreatedDBObject(UP3D_Relief, true);

                int[] Igd = new int[6] { -d1, -d2, -d3, -d4, -d5, -d6 };

                var ll = Igd.Where(e => e < 0).Select(e => new Vector3d(0, 0, e)).ToList();
                Context.Volumes.Clear();
                var counter = 0;
                Context.Volumes.Add(new VolumeInfo($"Объем ИГЭ-№{++counter} м3:", UP3D_Relief.MassProperties.Volume));
                Solid3d UP3D_Reliefd_iter = UP3D_Relief;
                foreach (var p in ll)
                {
                    var Reliefd_iter = Autodesk.AutoCAD.DatabaseServices.Surface.CreateLoftedSurface(new LoftProfile[]
                    {
                            new LoftProfile(pl_435.Gets()),
                            new LoftProfile(pl_0d1.Gets()),
                            new LoftProfile(pl_726.Gets()),
                    },
                    null, null, LoftOptionsB.ToLoftOptions()); ;

                    Reliefd_iter.TransformBy(Matrix3d.Scaling(20, ptd));
                    Reliefd_iter.TransformBy(Matrix3d.Displacement(p));
                    {
                        var newSlise = UP3D_Reliefd_iter.Slice(Reliefd_iter, true);
                        newSlise.ToBD(PdBlkRec, PdTrans);
                        UP3D_Reliefd_iter = newSlise;

                        Context.Volumes.Add(new VolumeInfo($"Объем ИГЭ-№{++counter} м3:", newSlise.MassProperties.Volume));
                    }
                }

                for (int i = 0; i < Context.Volumes.Count - 1; i++)
                    Context.Volumes[i].Value -= Context.Volumes[i + 1].Value;

                foreach (var VOL in Context.Volumes)
                    VOL.Value *= Math.Pow(10, -9);
                                
                //Разбивка тела "срезки"
                var SR_h = 65000;                         //Высота усечённой пирамиды для отрисовки тела "срезки"
                var lineSRX = linePL3 + SR_h * SR_otkos;  //Размер половины верхнего основания тела "срезки" поперек оси ВЛ
                var lineSRY = linePL4 + SR_h * SR_otkos;  //Размер половины верхнего основания тела "срезки" вдоль оси ВЛ
                                                          //Координаты точек верхнего основания усечёной пирамиды тела "срезки"
                var SR_pt8 = new Point3d(-lineSRX, -lineSRY, SR_h);
                var SR_pt5 = new Point3d(-lineSRX, lineSRY, SR_h);
                var SR_pt6 = new Point3d(lineSRX, lineSRY, SR_h);
                var SR_pt7 = new Point3d(lineSRX, -lineSRY, SR_h);
                var SR_pl = new VirtualPolyline3d(SR_pt8, SR_pt5, SR_pt6, SR_pt7);
                //Отрисовка объёмного тела "Срезки"
                Solid3d SR_Volume = new Autodesk.AutoCAD.DatabaseServices.Solid3d();
                SR_Volume.CreateLoftedSolid(new LoftProfile[]
                    {
                             new LoftProfile(pl3.Gets()),
                             new LoftProfile(SR_pl.Gets())
                    },
                      new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
                SR_Volume.TransformBy(Matrix3d.Displacement(Vector_ponizh));
                PdBlkRec.AppendEntity(SR_Volume);
                PdTrans.AddNewlyCreatedDBObject(SR_Volume, true);

                //проверка пересечения рельефа с плоскостью усечённой пирамиды
                var cols = new Point3dCollection();
                var ptinter1 = new Point3d(-linePL3, -linePL4, (d + h_ponizh));
                var ptinter2 = new Point3d(-linePL3, linePL4, (d + h_ponizh));
                var ptinter3 = new Point3d(linePL3, linePL4, (d + h_ponizh));
                var ptinter4 = new Point3d(linePL3, -linePL4, (d + h_ponizh));
                var pllv = new VirtualPolyline3d(ptinter1, ptinter2, ptinter3, ptinter4);

                pllv.Gets().IntersectWith(pl_pts.Gets(), Intersect.OnBothOperands, cols, IntPtr.Zero, IntPtr.Zero);
                //throw new System.Exception(cols.Count.ToString());

                if (cols.Count > 0)
                {
                    //Обрезка объёмного тела "Срезки" плоскостью рельефа
                    var SRV_Relief = SR_Volume.Slice(NewSurf, true);
                    SR_Volume.Erase();
                    PdBlkRec.AppendEntity(SRV_Relief);
                    PdTrans.AddNewlyCreatedDBObject(SRV_Relief, true);

                    Context.VolumesSR.Clear();
                    var poryadok = 0;
                    Context.VolumesSR.Add(new VolumeInfo($"Объем ИГЭ-№{++poryadok} м3:", SRV_Relief.MassProperties.Volume));

                    //Отрисовка объёмного тела "срезки" с учётом слоев ИГЭ
                    Solid3d SRV_Reliefdi = SRV_Relief;
                    foreach (var sr in ll)
                    {
                        var SRReliefdi = Autodesk.AutoCAD.DatabaseServices.Surface.CreateLoftedSurface(new LoftProfile[]
                        {
                                new LoftProfile(pl_435.Gets()),
                                new LoftProfile(pl_0d1.Gets()),
                                new LoftProfile(pl_726.Gets()),
                        },
                        null, null, LoftOptionsB.ToLoftOptions());

                        SRReliefdi.TransformBy(Matrix3d.Scaling(20, ptd));
                        SRReliefdi.TransformBy(Matrix3d.Displacement(sr));

                        //var colss = new Point3dCollection();
                        //pl_pts.Gets().TransformBy(Matrix3d.Displacement(sr));
                        //{
                        //    VirtualPolyline3d8pt Newpts = pl_pts;
                        //    pl_pts = Newpts;
                        //    pllv.Gets().IntersectWith(Newpts.Gets(), Intersect.OnBothOperands, colss, IntPtr.Zero, IntPtr.Zero); 
                        //}



                        {
                            try
                            {
                                var SRSlise = SRV_Reliefdi.Slice(SRReliefdi, true);
                                SRSlise.ToBD(PdBlkRec, PdTrans);
                                SRV_Reliefdi = SRSlise;

                                Context.VolumesSR.Add(new VolumeInfo($"Объем ИГЭ-№{++poryadok} м3:", SRSlise.MassProperties.Volume));
                            }
                            catch
                            {
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < Context.VolumesSR.Count - 1; i++)
                        Context.VolumesSR[i].Value -= Context.VolumesSR[i + 1].Value;

                    foreach (var VOL in Context.VolumesSR)
                        VOL.Value *= Math.Pow(10, -9);
                }
                else
                {
                    SR_Volume.Erase();
                }


                //throw new System.Exception("dfssdf");


                //Орисовка Объёмного тела насыпи
                //разбивка размеров усечённой пирамиды "Насыпи"
                var SN_h = SR_h; //Высота усечённой пирамиды для отрисовки тела "насыпи"
                Point3d[] SN_t;
                Point3d[] SNN_t;

                var SN_lineX1 = linePL3 + SN_dop;
                var SN_lineX2 = linePL3 + SN_dop;
                var SN_lineY1 = linePL4 + SN_dop;
                var SN_lineY2 = linePL4 + SN_dop;
                SN_t = new Point3d[4]
                {
                        new Point3d(-SN_lineX1, -SN_lineY2, d),
                        new Point3d(-SN_lineX1, SN_lineY1, d),
                        new Point3d(SN_lineX2, SN_lineY1, d),
                        new Point3d(SN_lineX2, -SN_lineY2, d),
                };

                var SNN_lineX1 = SN_lineX1 + SN_h * SN_otkos;
                var SNN_lineX2 = SN_lineX2 + SN_h * SN_otkos;
                var SNN_lineY1 = SN_lineY1 + SN_h * SN_otkos;
                var SNN_lineY2 = SN_lineY2 + SN_h * SN_otkos;
                SNN_t = new Point3d[4]
                {
                        new Point3d(-SNN_lineX1, -SNN_lineY2, -SN_h),
                        new Point3d(-SNN_lineX1, SNN_lineY1, -SN_h),
                        new Point3d(SNN_lineX2, SNN_lineY1, -SN_h),
                        new Point3d(SNN_lineX2, -SNN_lineY2, -SN_h),
                };

                //Отрисовка полилийний основания усечённой пирамиды "насыпи"
                var SN_pl1 = new VirtualPolyline3d(SN_t[0], SN_t[1], SN_t[2], SN_t[3]);
                var SN_pl2 = new VirtualPolyline3d(SNN_t[0], SNN_t[1], SNN_t[2], SNN_t[3]);

                //Отрисовка Объёмного тела "Насыпи"
                Solid3d SN_Volume = new Autodesk.AutoCAD.DatabaseServices.Solid3d();
                SN_Volume.CreateLoftedSolid(new LoftProfile[]
                    {
                             new LoftProfile(SN_pl1.Gets()),
                             new LoftProfile(SN_pl2.Gets())
                    },
                      new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
                SN_Volume.TransformBy(Matrix3d.Displacement(Vector_ponizh));
                SN_Volume.ToBD(PdBlkRec, PdTrans);

                //Обрезка объёмного тела насыпи по плоскости рельефа
                var SNV_Relief = SN_Volume.Slice(NewSurf, true);
                SNV_Relief.ToBD(PdBlkRec, PdTrans);
                SNV_Relief.Erase();
                var VolumesN1 = Math.Round(SN_Volume.MassProperties.Volume * Math.Pow(10, -9), 3);
                Context.VolumesN = VolumesN1;

                //Определение планировки площади насыпи
                var NasS_loft = Autodesk.AutoCAD.DatabaseServices.LoftedSurface.CreateLoftedSurface(new LoftProfile[]
                    {
                             new LoftProfile(SN_pl1.Gets()),
                             new LoftProfile(SN_pl2.Gets()),
                    },
                      null, null, new LoftOptionsBuilder()
                      {
                          Ruled = true
                      }
                      .ToLoftOptions());
                var Nas_F = Autodesk.AutoCAD.DatabaseServices.Surface.CreateFrom(SN_pl1.Gets());
                var areaNas_GetFrom = Nas_F.GetArea();
                //Перемещение поверхностей на вектор понижения

                NasS_loft.TransformBy(Matrix3d.Displacement(Vector_ponizh));
                Nas_F.TransformBy(Matrix3d.Displacement(Vector_ponizh));
                //Сечение поверхности 
                var SliceSLoft = NasS_loft.SliceBySurface(NewSurf);
                var SliceSN = Nas_F.SliceBySurface(NewSurf);
                //Получение площиди поверхностей
                
                var areaN1 = SliceSLoft.NewSurface.GetArea();
                var areaN2 = SliceSN.NegativeHalfSurface.GetArea();
                Context.SQN = Math.Round((areaN1 + (areaNas_GetFrom - areaN2)) * Math.Pow(10, -6), 3);

                ////Координаты расположения фундаментов в плане базы опоры
                var AOp2 = Baza_Op_A * 0.5;
                var BOp2 = Baza_Op_B * 0.5;
                ////Местополжение тип 1-4
                //Высота установки фундаментов
                var h_Fi = Context.HShebenPodgotovki + Context.HBetonPodgotovki;
                //Высота установки фундаментов
                var PtType1 = new Vector3d(-AOp2, BOp2, h_Fi);
                var PtType2 = new Vector3d(AOp2, BOp2, h_Fi);
                var PtType3 = new Vector3d(AOp2, -BOp2, h_Fi);
                var PtType4 = new Vector3d(-AOp2, -BOp2, h_Fi);

                //ФУНДАМЕНТЫ. ТИП 1
                {
                    Solid3d Fund_Type1i = FoundationBuild.Build(Context.SelectedType1, Vector_ponizh);
                    Fund_Type1i.TransformBy(Matrix3d.Rotation((-Foundation.AllFoundations[Context.SelectedType1].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));


                    //var ggg=new Point3d(0, 0, 0);
                    //var ggg2 = new Point3d(0, 1, 1);
                    ////ModSpaceRec.SetViewDirection(OrthographicView.TopView); ////Вид сверху
                    //Line3d line3D = new Line3d(ggg, ggg2);
                    
                    //if (Foundation.AllFoundations[Context.SelectedType1].n_bolt == 2)
                    //{
                    //    Fund_Type1i.TransformBy(Matrix3d.Mirroring(line3D));
                    //}
                    Fund_Type1i.TransformBy(Matrix3d.Displacement(PtType1));
                    Fund_Type1i.ToBD(PdBlkRec, PdTrans);
                    
                    
                }
                //ФУНДАМЕНТЫ. ТИП 1
                //ФУНДАМЕНТЫ. ТИП 2
                Solid3d Fund_Type2i = FoundationBuild.Build(Context.SelectedType2, Vector_ponizh);
                Fund_Type2i.TransformBy(Matrix3d.Rotation((Math.PI + (Foundation.AllFoundations[Context.SelectedType2].Ugol_povorota * Math.PI / 180)), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                Fund_Type2i.TransformBy(Matrix3d.Displacement(PtType2));
                Fund_Type2i.ToBD(PdBlkRec, PdTrans);
                //ФУНДАМЕНТЫ. ТИП 2
                //ФУНДАМЕНТЫ. ТИП 3
                var UgP = new double();
                if (Foundation.AllFoundations[Context.SelectedType3].Ugol_povorota == 0)
                {
                    UgP = 0 * Math.PI / 180;
                }
                else
                {
                    UgP = Math.PI * 0.5 + (Foundation.AllFoundations[Context.SelectedType3].Ugol_povorota * Math.PI / 180);
                }
                Solid3d Fund_Type3i = FoundationBuild.Build(Context.SelectedType3, Vector_ponizh);
                Fund_Type3i.TransformBy(Matrix3d.Rotation(UgP, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                Fund_Type3i.TransformBy(Matrix3d.Displacement(PtType3));
                Fund_Type3i.ToBD(PdBlkRec, PdTrans);
                //ФУНДАМЕНТЫ. ТИП 3
                //ФУНДАМЕНТЫ. ТИП 4
                Solid3d Fund_Type4i = FoundationBuild.Build(Context.SelectedType4, Vector_ponizh);
                Fund_Type4i.TransformBy(Matrix3d.Rotation((Foundation.AllFoundations[Context.SelectedType4].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                Fund_Type4i.TransformBy(Matrix3d.Displacement(PtType4));
                Fund_Type4i.ToBD(PdBlkRec, PdTrans);
                //ФУНДАМЕНТЫ. ТИП 4

                
                //Щебёночная подготовка
                //Отметки установки щебёночной подготовки
                var Pt_SPF1 = new Vector3d(-AOp2, BOp2, 0);
                var Pt_SPF2 = new Vector3d(AOp2, BOp2, 0);
                var Pt_SPF3 = new Vector3d(AOp2, -BOp2, 0);
                var Pt_SPF4 = new Vector3d(-AOp2, -BOp2, 0);
                //Щебёночная подготовка под ф-т 1
                if (Builder.TryBuildSheben(Context.SelectedType1, Vector_ponizh, Context, out var ShebPodgot1))
                {
                    ShebPodgot1.TransformBy(Matrix3d.Rotation((-Foundation.AllFoundations[Context.SelectedType1].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    ShebPodgot1.TransformBy(Matrix3d.Displacement(Pt_SPF1));
                    ShebPodgot1.ToBD(PdBlkRec, PdTrans);
                }
                //Щебёночная подготовка под ф-т 1
                //Щебёночная подготовка под ф-т 2
                if (Builder.TryBuildSheben(Context.SelectedType2, Vector_ponizh, Context, out var ShebPodgot2))
                {
                    ShebPodgot2.TransformBy(Matrix3d.Rotation((Math.PI + (Foundation.AllFoundations[Context.SelectedType2].Ugol_povorota * Math.PI / 180)), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    ShebPodgot2.TransformBy(Matrix3d.Displacement(Pt_SPF2));
                    ShebPodgot2.ToBD(PdBlkRec, PdTrans);
                }
                //Щебёночная подготовка под ф-т 2
                //Щебёночная подготовка под ф-т 3
                if (Builder.TryBuildSheben(Context.SelectedType3, Vector_ponizh, Context, out var ShebPodgot3))
                {
                    ShebPodgot3.TransformBy(Matrix3d.Rotation(UgP, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    ShebPodgot3.TransformBy(Matrix3d.Displacement(Pt_SPF3));
                    ShebPodgot3.ToBD(PdBlkRec, PdTrans);
                }
                //Щебёночная подготовка под ф-т 3
                //Щебёночная подготовка под ф-т 4
                if (Builder.TryBuildSheben(Context.SelectedType4, Vector_ponizh, Context, out var ShebPodgot4))
                {
                    ShebPodgot4.TransformBy(Matrix3d.Rotation((Foundation.AllFoundations[Context.SelectedType4].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    ShebPodgot4.TransformBy(Matrix3d.Displacement(Pt_SPF4));
                    ShebPodgot4.ToBD(PdBlkRec, PdTrans);
                }
                //Щебёночная подготовка под ф-т 4
                //БЕТОННАЯ ПОДГОТОВКА
                //Отметки установки щебёночной подготовки
                var Pt_BPF1 = new Vector3d(-AOp2, BOp2, Context.HShebenPodgotovki);
                var Pt_BPF2 = new Vector3d(AOp2, BOp2, Context.HShebenPodgotovki);
                var Pt_BPF3 = new Vector3d(AOp2, -BOp2, Context.HShebenPodgotovki);
                var Pt_BPF4 = new Vector3d(-AOp2, -BOp2, Context.HShebenPodgotovki);
                //Бетонная подготовка ф-т 1
                if (Builder.TryBuildBeton(Context.SelectedType1, Vector_ponizh, Context, out var PodBeton1))
                {
                    PodBeton1.TransformBy(Matrix3d.Rotation((-Foundation.AllFoundations[Context.SelectedType1].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    PodBeton1.TransformBy(Matrix3d.Displacement(Pt_BPF1));
                    PodBeton1.ToBD(PdBlkRec, PdTrans);
                }
                //Бетонная подготовка ф-т 1
                //Бетонная подготовка ф-т 2
                if (Builder.TryBuildBeton(Context.SelectedType2, Vector_ponizh, Context, out var PodBeton2))
                {
                    PodBeton2.TransformBy(Matrix3d.Rotation((Math.PI + (Foundation.AllFoundations[Context.SelectedType2].Ugol_povorota * Math.PI / 180)), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    PodBeton2.TransformBy(Matrix3d.Displacement(Pt_BPF2));
                    PodBeton2.ToBD(PdBlkRec, PdTrans);
                }
                //Бетонная подготовка ф-т 2
                //Бетонная подготовка ф-т 3
                if((Builder.TryBuildBeton(Context.SelectedType3, Vector_ponizh, Context, out var PodBeton3)))
                { 
                
                    PodBeton3.TransformBy(Matrix3d.Rotation(UgP, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    PodBeton3.TransformBy(Matrix3d.Displacement(Pt_BPF3));
                    PodBeton3.ToBD(PdBlkRec, PdTrans);
                }
                //Бетонная подготовка ф-т 3
                //Бетонная подготовка ф-т 4
                if ((Builder.TryBuildBeton(Context.SelectedType4, Vector_ponizh, Context, out var BetonPod4)))
                {
                    BetonPod4.TransformBy(Matrix3d.Rotation((Foundation.AllFoundations[Context.SelectedType4].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    BetonPod4.TransformBy(Matrix3d.Displacement(Pt_BPF4));
                    BetonPod4.ToBD(PdBlkRec, PdTrans);
                }
                //Бетонная подготовка ф-т 4




                //Сохранение объекта в базе данных
                PdTrans.Commit();
            }
            EdDoc.UpdateTiledViewportsFromDatabase();

            //Сохранение файла
            string strDWGname = Pdoc.Name;
            Object Format = AcadApp.GetSystemVariable("DWGTITLED");
            if (System.Convert.ToInt16(Format) == 0)
            {
                strDWGname = $"C:\\Users\\{Environment.UserName}\\Desktop\\{NumberOP}.dwg";

            }
            Pdoc.Database.SaveAs(strDWGname, DwgVersion.Current);

            Context.ExceptionMessage = "EndCommand";
        }
    }

    public static class Ext
    {
        public static void ToBD(this Entity ent, BlockTableRecord rec, Transaction tr)
        {



            using (BlockTableRecord sdsd = new BlockTableRecord())
            {
                
            }

                rec.AppendEntity(ent);
            tr.AddNewlyCreatedDBObject(ent, true);
        }
        
    }

    public class VirtualPolyline3d
    {
        public VirtualPolyline3d(Point3d ps1, Point3d ps2, Point3d ps3, Point3d ps4) 
        {
            points[0] = ps1;
            points[1] = ps2;
            points[2] = ps3;
            points[3] = ps4;
        }
        public readonly Point3d[] points = new Point3d[4];

        public VirtualPolyline3d Move(Vector3d vect)
        {
            for (int i = 0; i < points.Length; i++)
                points[i] = points[i] + vect;
            return this;
        }

        public Polyline3d Gets()
        {
            var pts = new Point3dCollection
            {
                points[0],
                points[1],
                points[2],
                points[3],
                points[0]
            };
            var Poliline3D = new Polyline3d(Poly3dType.SimplePoly, pts, true);

            return Poliline3D;
        }
        public void Draw(BlockTableRecord PdBlkRec, Transaction PdTrans)
        {
            var pts = new Point3dCollection
            {
                points[0],
                points[1],
                points[2],
                points[3],
                points[0]
            };
            var Poliline3D = new Polyline3d(Poly3dType.SimplePoly,pts,true);

            PdBlkRec.AppendEntity(Poliline3D);
            PdTrans.AddNewlyCreatedDBObject(Poliline3D, true);
        }
    }
    public class VirtualPolyline
    {
        public VirtualPolyline(Point2d p1, Point2d p2, Point2d p3, Point2d p4)
        {
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            points[3] = p4;
        }

        public readonly Point2d[] points = new Point2d[4];

        public void Move(Vector2d vect)
        {
            for (int i = 0; i < points.Length; i++)
                points[i] = points[i] + vect;
        }


        public void Draw(BlockTableRecord PdBlkRec, Transaction PdTrans)
        {

            var PoliLyne1 = new Autodesk.AutoCAD.DatabaseServices.Polyline();

            PoliLyne1.AddVertexAt(0, points[0], 0, 0, 0);
            PoliLyne1.AddVertexAt(1, points[1], 0, 0, 0);
            PoliLyne1.AddVertexAt(2, points[2], 0, 0, 0);
            PoliLyne1.AddVertexAt(3, points[3], 0, 0, 0);
            PoliLyne1.AddVertexAt(4, points[0], 0, 0, 0);


            PdBlkRec.AppendEntity(PoliLyne1);
            PdTrans.AddNewlyCreatedDBObject(PoliLyne1, true);
        }
    }
    public class VirtualPolyline3d8pt
    {
        public VirtualPolyline3d8pt(Point3d ps1, Point3d ps2, Point3d ps3, Point3d ps4, Point3d ps5, Point3d ps6, Point3d ps7, Point3d ps8)
        {
            points[0] = ps1;
            points[1] = ps2;
            points[2] = ps3;
            points[3] = ps4;
            points[4] = ps5;
            points[5] = ps6;
            points[6] = ps7;
            points[7] = ps8;
        }
        public readonly Point3d[] points = new Point3d[8];
        public Polyline3d Gets()
        {
            var pts = new Point3dCollection
            {
                points[0],
                points[1],
                points[2],
                points[3],
                points[4],
                points[5],
                points[6],
                points[7],
                points[0]

            };
            var Poliline3D = new Polyline3d(Poly3dType.SimplePoly, pts, true);

            return Poliline3D;
        }
        public void Draw(BlockTableRecord PdBlkRec, Transaction PdTrans)
        {
            var pts = new Point3dCollection
            {
                points[0],
                points[1],
                points[2],
                points[3],
                points[4],
                points[5],
                points[6],
                points[7],
                points[0]
            };
            var Poliline3D = new Polyline3d(Poly3dType.SimplePoly, pts, true);

            PdBlkRec.AppendEntity(Poliline3D);
            PdTrans.AddNewlyCreatedDBObject(Poliline3D, true);
        }
     }


    public class VirtualPolyline3d3pt
    {
        public VirtualPolyline3d3pt(Point3d ps1, Point3d ps2, Point3d ps3)
        {
            points[0] = ps1;
            points[1] = ps2;
            points[2] = ps3;  
        }
        public readonly Point3d[] points = new Point3d[3];
        public Polyline3d Gets()
        {
            var pts = new Point3dCollection
            {
                points[0],
                points[1],
                points[2],
            };
            var Poliline3D = new Polyline3d(Poly3dType.SimplePoly, pts, true);
            Poliline3D.Closed = false;
            return Poliline3D;
        }
        public void Draw(BlockTableRecord PdBlkRec, Transaction PdTrans)
        {
            var pts = new Point3dCollection
            {
                points[0],
                points[1],
                points[2],   
            };
            var Poliline3D = new Polyline3d(Poly3dType.SimplePoly, pts, true);
            Poliline3D.Closed = false;

            PdBlkRec.AppendEntity(Poliline3D);
            PdTrans.AddNewlyCreatedDBObject(Poliline3D, true);
        }
    }
    public class VirtualPolyline3d12pt
    {
        public VirtualPolyline3d12pt(Point3d ps1, Point3d ps2, Point3d ps3, Point3d ps4, Point3d ps5, Point3d ps6, Point3d ps7, Point3d ps8, Point3d ps9, Point3d ps10, Point3d ps11, Point3d ps12)
        {
            points[0] = ps1;
            points[1] = ps2;
            points[2] = ps3;
            points[3] = ps4;
            points[4] = ps5;
            points[5] = ps6;
            points[6] = ps7;
            points[7] = ps8;
            points[8] = ps9;
            points[9] = ps10;
            points[10] = ps11;
            points[11] = ps12;
            
        }
        public readonly Point3d[] points = new Point3d[12];
        public Polyline3d Gets()
        {
            var pts = new Point3dCollection
            {
                points[0],
                points[1],
                points[2],
                points[3],
                points[4],
                points[5],
                points[6],
                points[7],
                points[8],
                points[9],
                points[10],
                points[11],
                points[0]
            };
            var Poliline3D = new Polyline3d(Poly3dType.SimplePoly, pts, true);

            return Poliline3D;
        }
        public void Draw(BlockTableRecord PdBlkRec, Transaction PdTrans)
        {
            var pts = new Point3dCollection
            {
                points[0],
                points[1],
                points[2],
                points[3],
                points[4],
                points[5],
                points[6],
                points[7],
                points[8],
                points[9],
                points[10],
                points[11],
                points[0]
            };
            var Poliline3D = new Polyline3d(Poly3dType.SimplePoly, pts, true);

            PdBlkRec.AppendEntity(Poliline3D);
            PdTrans.AddNewlyCreatedDBObject(Poliline3D, true);
        }
        public VirtualPolyline3d12pt Move(Vector3d vect)
        {
            for (int i = 0; i < points.Length; i++)
                points[i] = points[i] + vect;
            return this;
        }
    }

}


