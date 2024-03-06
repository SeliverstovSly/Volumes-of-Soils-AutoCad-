using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices.Filters;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using Spire.Xls;
using static ACADprogram.Crossbar;
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
            using (var PdTrans = Ext.tr = PdDb.TransactionManager.StartTransaction())
            //открыти таблицы блоков для чтения
            using (var PdBlk = PdTrans.GetObject(PdDb.BlockTableId, OpenMode.ForRead) as BlockTable)
            //Открытие пространство модели таблицы блоков для записи.
            using (var PdBlkRec = Ext.rec = PdTrans.GetObject(PdBlk[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord)
            {
                //Определение размеров дна котлована взависимости от типа опоры и типа фундамента

                double UgolProekc;
                double[] Lk_dopA;
                Lk_dopA = new double[4]
                {
                    new double(),
                    new double(),
                    new double(),
                    new double()
                };
                double[] Lk_dopB;
                Lk_dopB = new double[4]
                {
                    new double(),
                    new double(),
                    new double(),
                    new double()
                };
                int[] FQuantity = new int[] { Context.FoundationQuantity1, Context.FoundationQuantity2, Context.FoundationQuantity3, Context.FoundationQuantity4 };
                FoundationType[] typei = new FoundationType[] { Context.SelectedType1, Context.SelectedType2, Context.SelectedType3, Context.SelectedType4 };
                B5n[] Beamtype = new B5n[] { Context.SelectedBeam1, Context.SelectedBeam2, Context.SelectedBeam3, Context.SelectedBeam4 };
                for (int i = 0; i < typei.Length; i++)
                {
                    var e = typei[i];
                    var g = FQuantity[i];
                    switch (g)
                        {
                            case 1:
                            if (e == FoundationType.FS1n_A || e == FoundationType.FSP1n_A)
                            {
                                Foundation.AllFoundations[e].OverallSize = 4200;
                                UgolProekc = 9.4623 * Math.PI / 180;
                            }
                            else if (e == FoundationType.FS2n_A || e == FoundationType.FSP2n_A)
                            {
                                Foundation.AllFoundations[e].OverallSize = 5200;
                                UgolProekc = 15.0202 * Math.PI / 180;
                            }
                            else
                            {
                                UgolProekc = 0 * Math.PI / 180;
                            }
                            Lk_dopA[i] = Foundation.AllFoundations[e].a * 0.5 + Context.SvesaBetonPod + Context.SvesaShebenPod + Context.HShebenPodgotovki * 1 + Context.DopRazmerNizhaKotlovana;
                            Lk_dopB[i] = Foundation.AllFoundations[e].OverallSize * 0.5 + Context.SvesaBetonPod + Context.SvesaShebenPod + Context.HShebenPodgotovki * 1 + Context.DopRazmerNizhaKotlovana;

                            if (Foundation.AllFoundations[e].Ugol_povorota > 0)
                            {
                                Lk_dopA[i] = 0.5 * Math.Sqrt(Math.Pow(Foundation.AllFoundations[e].a, 2) + Math.Pow(Foundation.AllFoundations[e].OverallSize, 2)) * Math.Cos(UgolProekc) + Math.Sqrt(Math.Pow(Foundation.AllFoundations[e].e, 2) * 0.5) + Context.SvesaBetonPod / Math.Cos(Foundation.AllFoundations[e].Ugol_povorota * Math.PI / 180) + Context.SvesaShebenPod / Math.Cos(Foundation.AllFoundations[e].Ugol_povorota * Math.PI / 180) + Context.HShebenPodgotovki * 1 / Math.Cos(Foundation.AllFoundations[e].Ugol_povorota * Math.PI / 180) + Context.DopRazmerNizhaKotlovana;
                                Lk_dopB[i] = 0.5 * Math.Sqrt(Math.Pow(Foundation.AllFoundations[e].a, 2) + Math.Pow(Foundation.AllFoundations[e].OverallSize, 2)) * Math.Cos(UgolProekc) + Math.Sqrt(Math.Pow(Foundation.AllFoundations[e].e, 2) * 0.5) + Context.SvesaBetonPod / Math.Cos(Foundation.AllFoundations[e].Ugol_povorota * Math.PI / 180) + Context.SvesaShebenPod / Math.Cos(Foundation.AllFoundations[e].Ugol_povorota * Math.PI / 180) + Context.HShebenPodgotovki * 1 / Math.Cos(Foundation.AllFoundations[e].Ugol_povorota * Math.PI / 180) + Context.DopRazmerNizhaKotlovana;
                            }
                            break;
                            case 2:
                            var f = Beamtype[i];
                            var L2foundation = (Foundation.AllFoundations[e].a * 0.5 + Foundation.AllFoundations[e].e) * 2 + f.LB;
                            Lk_dopA[i] = L2foundation * 0.5 + Context.SvesaBetonPod + Context.SvesaShebenPod + Context.HShebenPodgotovki * 1 + Context.DopRazmerNizhaKotlovana;
                            Lk_dopB[i] = Foundation.AllFoundations[e].OverallSize * 0.5 + Context.SvesaBetonPod + Context.SvesaShebenPod + Context.HShebenPodgotovki * 1 + Context.DopRazmerNizhaKotlovana;
                            if (Foundation.AllFoundations[e].Ugol_povorota > 0)
                            {

                            }

                            break;

                        }
                    
                }

                var lk_ALeft0 = Math.Max(Lk_dopA[0], Lk_dopA[3]) + Opora.AllOpora[Context.SelectedType].Baza_A * 0.5;
                var lk_ARight0 = Math.Max(Lk_dopA[1], Lk_dopA[2]) + Opora.AllOpora[Context.SelectedType].Baza_A * 0.5;
                var lk_BUp0 = Math.Max(Lk_dopB[0], Lk_dopB[1]) + Opora.AllOpora[Context.SelectedType].Baza_B * 0.5;
                var lk_BDown0 = Math.Max(Lk_dopB[2], Lk_dopB[3]) + Opora.AllOpora[Context.SelectedType].Baza_B * 0.5;
                //Глубина Котлована
                var hk = Foundation.AllFoundations[Context.SelectedType1].Glubina_Zalozhenia + Context.HBetonPodgotovki + Context.HShebenPodgotovki;
                //Определение размеров дна котлована взависимости от типа опоры и типа фундамента
                //Округление значения верха котлована с расчётом новоо значения низа котлована
                var lk_Aleftk0 = RoundingSizePitWidth.RoundSize(lk_ALeft0, hk, Context.OtkosKotlovana, 100);
                var lk_ARightk0 = RoundingSizePitWidth.RoundSize(lk_ARight0, hk, Context.OtkosKotlovana, 100);
                var lk_BUpk0 = RoundingSizePitWidth.RoundSize(lk_BUp0, hk, Context.OtkosKotlovana, 100);
                var lk_BDownk0 = RoundingSizePitWidth.RoundSize(lk_BDown0, hk, Context.OtkosKotlovana, 100);

                var lk_ALeft = lk_Aleftk0;
                var lk_ARight = lk_ARightk0;
                var lk_BUp = lk_BUpk0;
                var lk_BDown = lk_BDownk0;
                //Округление значения верха котлована с расчётом новоо значения низа котлована
                //Настройка визуального стиля
                ViewportTable ModSpace = (ViewportTable)PdTrans.GetObject(PdDb.ViewportTableId, OpenMode.ForRead);
                ViewportTableRecord ModSpaceRec = (ViewportTableRecord)PdTrans.GetObject(ModSpace["*Active"], OpenMode.ForWrite);
                DBDictionary Style = (DBDictionary)PdTrans.GetObject(PdDb.VisualStyleDictionaryId, OpenMode.ForRead);
                ModSpaceRec.VisualStyleId = Style.GetAt("Conceptual");

                //Исходные данные для отрезков(длина и координаты) 
                var d = hk; //размер по Z
                var j = Context.OtkosKotlovana;    //Уклон откосов котлована
                var SR_otkos = Context.OtkosSrezki; //Уклон откосов срезки
                var SN_otkos = Context.OtkosNasypi; //Уклон откосов насыпи
                var SN_dop = Context.LdopNasypi;   //Запас к размеру длины и ширины основания насыпи
                var d1 = Context.IGE1;    //Отметка первого (верхнего) слоя ИГЭ, мм
                var d2 = Context.IGE2;    //Отметка второго слоя ИГЭ, мм
                var d3 = Context.IGE3;    //Отметка третьего слоя ИГЭ, мм
                var d4 = Context.IGE4;    //Отметка четвёптого слоя ИГЭ, мм
                var d5 = Context.IGE5;    //Отметка пятого слоя ИГЭ, мм
                var d6 = Context.IGE6;    //Отметка шестого слоя ИГЭ, мм
                var h_ponizh = Context.OtmetkaPonizh; //Величина изменения отметки положения 3D фигур (понижение или повышения центра опоры)

                var h_leftA = Context.HReliefPoperekVLeft;  //Высота от поверхности котлована до рельефа
                var h_rightA = Context.HReliefPoperekVRight;  //Высота от поверхности котлована до рельефа
                var h_downB = Context.HReliefVdolVLDown;    //Высота от поверхности котлована до рельефа
                var h_UpB = Context.HReliefVdolVLUp;    //Высота от поверхности котлована до рельефа

                //Вектор изменения отметки положения цетнра опоры
                var Vector_ponizh = new Vector3d(0, 0, h_ponizh);

                var VerhKotleft = lk_ALeft + d * j;
                var VerhKotRight = lk_ARight + d * j;
                var VerhKotUp = lk_BUp + d * j;
                var VerhKotDown = lk_BDown + d * j;

                var h1 = h_leftA * VerhKotleft / (Opora.AllOpora[Context.SelectedType].Baza_A * 0.5);
                var h2 = h_rightA * VerhKotRight / (Opora.AllOpora[Context.SelectedType].Baza_A * 0.5);
                var h3 = h_UpB * VerhKotUp / (Opora.AllOpora[Context.SelectedType].Baza_B * 0.5);
                var h4 = h_downB * VerhKotDown / (Opora.AllOpora[Context.SelectedType].Baza_B * 0.5);
                //MessageBox.Show($"{lk_ALeft}----{VerhKotleft}----{lk_ARight}----{VerhKotRight}----{lk_BUp}----{VerhKotUp}----{lk_BDown}----{VerhKotDown}----");
                //return;

                Point3d[] ptm;

                // Раздел определения координат точек определяющих рельеф
                //Расстояние от цетра котлована до точке 9-12
                var lt09 = 0.5 * Math.Sqrt(Math.Pow(VerhKotleft, 2) + Math.Pow(VerhKotUp, 2));
                var lt010 = 0.5 * Math.Sqrt(Math.Pow(VerhKotRight, 2) + Math.Pow(VerhKotUp, 2));
                var lt011 = 0.5 * Math.Sqrt(Math.Pow(VerhKotRight, 2) + Math.Pow(VerhKotDown, 2));
                var lt012 = 0.5 * Math.Sqrt(Math.Pow(VerhKotleft, 2) + Math.Pow(VerhKotDown, 2));
                //Расстояние от центра котлована до точек в углах котлована (5-8)
                var ly05 = Math.Sqrt(Math.Pow(VerhKotleft, 2) + Math.Pow(VerhKotUp, 2));
                var ly06 = Math.Sqrt(Math.Pow(VerhKotRight, 2) + Math.Pow(VerhKotUp, 2));
                var ly07 = Math.Sqrt(Math.Pow(VerhKotRight, 2) + Math.Pow(VerhKotDown, 2));
                var ly08 = Math.Sqrt(Math.Pow(VerhKotleft, 2) + Math.Pow(VerhKotDown, 2));
                //Высота точек (9-12) между главными точками 1-4
                var h9 = 0.5 * (h1 + h3);
                var h10 = 0.5 * (h3 + h2);
                var h11 = 0.5 * (h2 + h4);
                var h12 = 0.5 * (h4 + h1);

                //Высота точек в углах котлована
                var h5 = (h9 * ly05) / lt09;
                var h6 = (h10 * ly06) / lt010;
                var h7 = (h11 * ly07) / lt011;
                var h8 = (h12 * ly08) / lt012;

                //3D точки вершин верха котлована

                ptm = new Point3d[8]
                    {
                        new Point3d(-VerhKotleft, 0, (d + h1)),
                        new Point3d(VerhKotRight, 0, (d + h2)),
                        new Point3d(0, -VerhKotDown, (d + h4)),
                        new Point3d(0, VerhKotUp, (d + h3)),
                        new Point3d(-VerhKotleft, VerhKotUp, (d + h5)),
                        new Point3d(VerhKotRight, VerhKotUp, (d + h6)),
                        new Point3d(VerhKotRight, -VerhKotDown, (d + h7)),
                        new Point3d(-VerhKotleft, -VerhKotDown, (d + h8)),
                    };

                //Координаты точек для построения котлована
                var pt0 = new Point3d(0, 0, 0);
                var ptd = new Point3d(0, 0, d);
                var ptd1 = new Point3d(-lk_ALeft, 0, d);
                var ptd2 = new Point3d(lk_ARight, 0, d);
                var ptd3 = new Point3d(0, lk_BUp, d);
                var ptd4 = new Point3d(0, -lk_BDown, d);

                var pt5 = new Point3d(-lk_ALeft, -lk_BDown, 0);
                var pt6 = new Point3d(-lk_ALeft, lk_BUp, 0);
                var pt7 = new Point3d(lk_ARight, lk_BUp, 0);
                var pt8 = new Point3d(lk_ARight, -lk_BDown, 0);

                var pt9 = new Point3d(-VerhKotleft, -VerhKotDown, d);
                var pt10 = new Point3d(-VerhKotleft, VerhKotUp, d);
                var pt11 = new Point3d(VerhKotRight, VerhKotUp, d);
                var pt12 = new Point3d(VerhKotRight, -VerhKotDown, d);

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
                        newSlise.ToBD();
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
                var lineSRLeft = VerhKotleft + SR_h * SR_otkos;  //Размер половины верхнего основания тела "срезки" поперек оси ВЛ
                var lineSRUp = VerhKotUp + SR_h * SR_otkos;  //Размер половины верхнего основания тела "срезки" вдоль оси ВЛ
                var LineSRright = VerhKotRight + SR_h * SR_otkos;
                var lineSRDown = VerhKotDown + SR_h * SR_otkos;
                //Координаты точек верхнего основания усечёной пирамиды тела "срезки"
                var SR_pt8 = new Point3d(-lineSRLeft, -lineSRDown, SR_h);
                var SR_pt5 = new Point3d(-lineSRLeft, lineSRUp, SR_h);
                var SR_pt6 = new Point3d(LineSRright, lineSRUp, SR_h);
                var SR_pt7 = new Point3d(LineSRright, -lineSRDown, SR_h);
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
                var ptinter1 = new Point3d(-VerhKotleft, -VerhKotDown, (d + h_ponizh));
                var ptinter2 = new Point3d(-VerhKotleft, VerhKotUp, (d + h_ponizh));
                var ptinter3 = new Point3d(VerhKotRight, VerhKotUp, (d + h_ponizh));
                var ptinter4 = new Point3d(VerhKotRight, -VerhKotDown, (d + h_ponizh));
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

                        {
                            try
                            {
                                var SRSlise = SRV_Reliefdi.Slice(SRReliefdi, true);
                                SRSlise.ToBD();
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

                var SN_lineX1 = VerhKotleft + SN_dop;
                var SN_lineX2 = VerhKotRight + SN_dop;
                var SN_lineY1 = VerhKotUp + SN_dop;
                var SN_lineY2 = VerhKotDown + SN_dop;
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
                SN_Volume.ToBD();

                //Обрезка объёмного тела насыпи по плоскости рельефа
                var SNV_Relief = SN_Volume.Slice(NewSurf, true);
                SNV_Relief.ToBD();
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
                var AOp2 = Opora.AllOpora[Context.SelectedType].Baza_A * 0.5;
                var BOp2 = Opora.AllOpora[Context.SelectedType].Baza_B * 0.5;
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
                    Fund_Type1i.ToBD();


                }
                //ФУНДАМЕНТЫ. ТИП 1
                //ФУНДАМЕНТЫ. ТИП 2
                Solid3d Fund_Type2i = FoundationBuild.Build(Context.SelectedType2, Vector_ponizh);
                Fund_Type2i.TransformBy(Matrix3d.Rotation((Math.PI + (Foundation.AllFoundations[Context.SelectedType2].Ugol_povorota * Math.PI / 180)), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                Fund_Type2i.TransformBy(Matrix3d.Displacement(PtType2));
                Fund_Type2i.ToBD();
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
                Fund_Type3i.ToBD();
                //ФУНДАМЕНТЫ. ТИП 3
                //ФУНДАМЕНТЫ. ТИП 4
                Solid3d Fund_Type4i = FoundationBuild.Build(Context.SelectedType4, Vector_ponizh);
                Fund_Type4i.TransformBy(Matrix3d.Rotation((Foundation.AllFoundations[Context.SelectedType4].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                Fund_Type4i.TransformBy(Matrix3d.Displacement(PtType4));
                Fund_Type4i.ToBD();
                //ФУНДАМЕНТЫ. ТИП 4


                //Щебёночная подготовка
                //Отметки установки щебёночной подготовки
                var Pt_SPF1 = new Vector3d(-AOp2, BOp2, 0);
                var Pt_SPF2 = new Vector3d(AOp2, BOp2, 0);
                var Pt_SPF3 = new Vector3d(AOp2, -BOp2, 0);
                var Pt_SPF4 = new Vector3d(-AOp2, -BOp2, 0);
                //Щебёночная подготовка под ф-т 1
                double VolShebPodgot1 = 0;
                if (Builder.TryBuildSheben(Context.SelectedType1, Vector_ponizh, Context, out var ShebPodgot1))
                {
                    ShebPodgot1.TransformBy(Matrix3d.Rotation((-Foundation.AllFoundations[Context.SelectedType1].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    ShebPodgot1.TransformBy(Matrix3d.Displacement(Pt_SPF1));
                    ShebPodgot1.ToBD();
                    VolShebPodgot1 = Math.Round(ShebPodgot1.MassProperties.Volume * Math.Pow(10, -9), 3);
                }
                //Щебёночная подготовка под ф-т 1
                //Щебёночная подготовка под ф-т 2
                double VolShebPodgot2 = 0;
                if (Builder.TryBuildSheben(Context.SelectedType2, Vector_ponizh, Context, out var ShebPodgot2))
                {
                    ShebPodgot2.TransformBy(Matrix3d.Rotation((Math.PI + (Foundation.AllFoundations[Context.SelectedType2].Ugol_povorota * Math.PI / 180)), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    ShebPodgot2.TransformBy(Matrix3d.Displacement(Pt_SPF2));
                    ShebPodgot2.ToBD();
                    VolShebPodgot2 = Math.Round(ShebPodgot2.MassProperties.Volume * Math.Pow(10, -9), 3);
                }
                //Щебёночная подготовка под ф-т 2
                //Щебёночная подготовка под ф-т 3
                double VolShebPodgot3 = 0;
                if (Builder.TryBuildSheben(Context.SelectedType3, Vector_ponizh, Context, out var ShebPodgot3))
                {
                    ShebPodgot3.TransformBy(Matrix3d.Rotation(UgP, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    ShebPodgot3.TransformBy(Matrix3d.Displacement(Pt_SPF3));
                    ShebPodgot3.ToBD();
                    VolShebPodgot3 = Math.Round(ShebPodgot3.MassProperties.Volume * Math.Pow(10, -9), 3);
                }
                //Щебёночная подготовка под ф-т 3
                //Щебёночная подготовка под ф-т 4
                double VolShebPodgot4 = 0;
                if (Builder.TryBuildSheben(Context.SelectedType4, Vector_ponizh, Context, out var ShebPodgot4))
                {
                    ShebPodgot4.TransformBy(Matrix3d.Rotation((Foundation.AllFoundations[Context.SelectedType4].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    ShebPodgot4.TransformBy(Matrix3d.Displacement(Pt_SPF4));
                    ShebPodgot4.ToBD();
                    VolShebPodgot4 = Math.Round(ShebPodgot4.MassProperties.Volume * Math.Pow(10, -9), 3);
                }
                Context.VolumeShebenPodg = VolShebPodgot1 + VolShebPodgot2 + VolShebPodgot3 + VolShebPodgot4;
                //Щебёночная подготовка под ф-т 4
                //БЕТОННАЯ ПОДГОТОВКА
                //Отметки установки щебёночной подготовки
                var Pt_BPF1 = new Vector3d(-AOp2, BOp2, Context.HShebenPodgotovki);
                var Pt_BPF2 = new Vector3d(AOp2, BOp2, Context.HShebenPodgotovki);
                var Pt_BPF3 = new Vector3d(AOp2, -BOp2, Context.HShebenPodgotovki);
                var Pt_BPF4 = new Vector3d(-AOp2, -BOp2, Context.HShebenPodgotovki);
                //Бетонная подготовка ф-т 1
                double VolPodBeton1 = 0;
                if (Builder.TryBuildBeton(Context.SelectedType1, Vector_ponizh, Context, out var PodBeton1))
                {
                    PodBeton1.TransformBy(Matrix3d.Rotation((-Foundation.AllFoundations[Context.SelectedType1].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    PodBeton1.TransformBy(Matrix3d.Displacement(Pt_BPF1));
                    PodBeton1.ToBD();
                    VolPodBeton1 = Math.Round(PodBeton1.MassProperties.Volume * Math.Pow(10, -9), 3);
                }
                //Бетонная подготовка ф-т 1
                //Бетонная подготовка ф-т 2
                double VolPodBeton2 = 0;
                if (Builder.TryBuildBeton(Context.SelectedType2, Vector_ponizh, Context, out var PodBeton2))
                {
                    PodBeton2.TransformBy(Matrix3d.Rotation((Math.PI + (Foundation.AllFoundations[Context.SelectedType2].Ugol_povorota * Math.PI / 180)), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    PodBeton2.TransformBy(Matrix3d.Displacement(Pt_BPF2));
                    PodBeton2.ToBD();
                    VolPodBeton2 = Math.Round(PodBeton2.MassProperties.Volume * Math.Pow(10, -9), 3);
                }
                //Бетонная подготовка ф-т 2
                //Бетонная подготовка ф-т 3
                double VolPodBeton3 = 0;
                if ((Builder.TryBuildBeton(Context.SelectedType3, Vector_ponizh, Context, out var PodBeton3)))
                {
                    PodBeton3.TransformBy(Matrix3d.Rotation(UgP, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    PodBeton3.TransformBy(Matrix3d.Displacement(Pt_BPF3));
                    PodBeton3.ToBD();
                    VolPodBeton3 = Math.Round(PodBeton3.MassProperties.Volume * Math.Pow(10, -9), 3);
                }
                //Бетонная подготовка ф-т 3
                //Бетонная подготовка ф-т 4
                double VolPodBeton4 = 0;
                if ((Builder.TryBuildBeton(Context.SelectedType4, Vector_ponizh, Context, out var BetonPod4)))
                {
                    BetonPod4.TransformBy(Matrix3d.Rotation((Foundation.AllFoundations[Context.SelectedType4].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    BetonPod4.TransformBy(Matrix3d.Displacement(Pt_BPF4));
                    BetonPod4.ToBD();
                    VolPodBeton4 = Math.Round(BetonPod4.MassProperties.Volume * Math.Pow(10, -9), 3);
                }
                Context.VolumeBetonPodg = VolPodBeton1 + VolPodBeton2 + VolPodBeton3 + VolPodBeton4;
                //Бетонная подготовка ф-т 4
                //Ригель в модели
                for (int ri = 1; ri <= Context.crossbarquantityType1 + Context.crossbarquantityType1A; ri++)
                {
                    if (Crossbar.TryBuildCrossbar(Context.SelectedTypeCr1, Vector_ponizh, Context, out var Crossbar3DType1, Context.SelectedType1, 0, ri, out var alpha, out var betta))
                    {
                        Crossbar3DType1.ToBD();
                        Crossbar3DType1.TransformBy(Matrix3d.Rotation((-Foundation.AllFoundations[Context.SelectedType1].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        Crossbar3DType1.TransformBy(Matrix3d.Displacement(Pt_SPF1));
                    }
                }
                for (int ri = 1; ri <= Context.crossbarquantityType2 + Context.crossbarquantityType2A; ri++)
                {
                    if (Crossbar.TryBuildCrossbar(Context.SelectedTypeCr2, Vector_ponizh, Context, out var Crossbar3DType2, Context.SelectedType2, 1, ri, out var alpha, out var betta))
                    {
                        Crossbar3DType2.ToBD();
                        Crossbar3DType2.TransformBy(Matrix3d.Rotation((Math.PI + (Foundation.AllFoundations[Context.SelectedType2].Ugol_povorota * Math.PI / 180)), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        Crossbar3DType2.TransformBy(Matrix3d.Displacement(Pt_SPF2));
                    }
                }
                for (int ri = 1; ri <= Context.crossbarquantityType3 + Context.crossbarquantityType3A; ri++)
                {
                    if (Crossbar.TryBuildCrossbar(Context.SelectedTypeCr3, Vector_ponizh, Context, out var Crossbar3DType3, Context.SelectedType3, 2, ri, out var alpha, out var betta))
                    {
                        Crossbar3DType3.ToBD();
                        if (Foundation.AllFoundations[Context.SelectedType3].Ugol_povorota == 0)
                        {
                            Crossbar3DType3.TransformBy(Matrix3d.Rotation((Math.PI), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        }
                        Crossbar3DType3.TransformBy(Matrix3d.Rotation(UgP, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        Crossbar3DType3.TransformBy(Matrix3d.Displacement(Pt_SPF3));
                    }
                }
                for (int ri = 1; ri <= Context.crossbarquantityType4 + Context.crossbarquantityType4A; ri++)
                {
                    if (Crossbar.TryBuildCrossbar(Context.SelectedTypeCr4, Vector_ponizh, Context, out var Crossbar3DType4, Context.SelectedType4, 3, ri, out var alpha, out var betta))
                    {
                        Crossbar3DType4.ToBD();
                        Crossbar3DType4.TransformBy(Matrix3d.Rotation((Foundation.AllFoundations[Context.SelectedType4].Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        Crossbar3DType4.TransformBy(Matrix3d.Displacement(Pt_SPF4));
                    }
                }
                //Ригель в модели
                //var ffff = new NextBeam();
                //if (ffff.BuildBeam(Context, out var B2n, out Solid3d Hole_d1,out Vector3d[] pt_holed1, out Vector3d[] pt_hole2, out Solid3d B1n_d4, out Solid3d B1n_d5, out Solid3d B1n_d6))
                //{
                //    B2n.ToBD();
                //}

                ////Управление Балками в модели
                //Solid3d B1 = new Solid3d();
                //B1 = Context.SelectedBeam1.BuildBeam();
                //B1.TransformBy(Matrix3d.Rotation(-45 * Math.PI / 180, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                ////Высота установки ригеля
                //var Hb1 = h_Fi + Foundation.AllFoundations[Context.SelectedType1].hf;
                //B1.TransformBy(Matrix3d.Displacement(new Vector3d(-AOp2+197.9899, BOp2-197.9899, Hb1)));



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

            //Excel
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Range[1, 1].Value = "Номер опоры";
            worksheet.Range[1, 2].Value = "Объёмы срезаемого грунта, м3";
            worksheet.Range[1, 9].Value = "Объёмы разрабатываемого грунта в котловане, м3";
            worksheet.Range[1, 16].Value = "Объёмы насыпи, м3";
            worksheet.Range[1, 17].Value = "Площадь планировки насыпи, м3";
            worksheet.Range[1, 18].Value = "Объём бетонной подготовки, м3";
            worksheet.Range[1, 19].Value = "Объём щебёночной подготовки, м3";
            for (var b = 0; b < Context.Volumes.Count; b++)
            {
                worksheet.Range[2, b + 2].Value = $"Объёмы ИГЭ-№{b + 1}";
                worksheet.Range[3, b + 2].Value = $"{Context.VolumesSR[b].Value}";
                worksheet.Range[2, b + 9].Value = $"Объёмы ИГЭ-№{b + 1}";
                worksheet.Range[3, b + 9].Value = $"{Context.Volumes[b].Value}";
            }
            worksheet.Range[3, 16].Value = $"{Context.VolumesN}";
            worksheet.Range[3, 17].Value = $"{Context.SQN}";
            worksheet.Range[3, 18].Value = $"{Context.VolumeBetonPodg}";
            worksheet.Range[3, 19].Value = $"{Context.VolumeShebenPodg}";
            worksheet.Range[3, 1].Value = $"{NumberOP}";
            worksheet.Merge(worksheet.Range[1, 1], worksheet.Range[2, 1]);
            worksheet["B1:H1"].Merge();
            worksheet["I1:O1"].Merge();
            worksheet["P1:P2"].Merge();
            worksheet["Q1:Q2"].Merge();
            worksheet["R1:R2"].Merge();
            worksheet["S1:S2"].Merge();
            worksheet.AllocatedRange.ColumnWidth = 17;
            worksheet.AllocatedRange.RowHeight = 25;
            CellStyle style = workbook.Styles.Add("new style");
            style.Font.FontName = "ISOCPEUR";
            style.Font.Size= 12;
            style.VerticalAlignment = VerticalAlignType.Center;
            style.HorizontalAlignment = HorizontalAlignType.Center;
            style.WrapText = true;
            worksheet.AllocatedRange.Style = style;
            workbook.SaveToFile($"C:\\Users\\{Environment.UserName}\\Desktop\\{NumberOP}.xlsx", ExcelVersion.Version2013);
            //Excel
            Context.ExceptionMessage = "EndCommand";


            //var beams = new List<BeamBase>()
            //{
            //    new TestBeam() { xxx = 22 },
            //    new TestBeam(),
            //    new TestBeam(),
            //    new TestBeam(),
            //    new TestBeam(),
            //    new TestBeam2() { zzz = 23 },
            //    new TestBeam2(),
            //    new TestBeam2(),
            //    new TestBeam2(),
            //};


            //foreach (var b in beams)
            //{
            //    b.BuildBeam();
            //}

            //var beam = new BeamBase();


        }
    }

    public static class Ext
    {
        public static BlockTableRecord rec;
        public static Transaction tr;

        public static void ToBD(this Entity ent)
        {
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
            var Poliline3D = new Polyline3d(Poly3dType.SimplePoly, pts, true);

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
        public void Move(Vector3d vect)
        {
            for (int i = 0; i < points.Length; i++)
                points[i] = points[i] + vect;
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
    public class Round
    {
        public static double RoundVerh(double number, int accuracy)
        {
            var e = number / accuracy;
            var t = (Math.Round(e) + 1) * accuracy;
            return t;
        }
    }
    public static class RoundingSizePitWidth
    {
        public static double RoundSize(this double width, double depth, double slope, int accuracy)
        {
            var Upperbase = width + depth * slope;
            var Upperbase_accuracy = Upperbase / accuracy;
            var Round_Upperbase = (Math.Round(Upperbase_accuracy)) * accuracy;
            var Bottombase = Round_Upperbase - depth * slope;
            return Bottombase;
        }
    }
    public class DisplacementCrossbar
    {
        public static double Displacement(FoundationType foundationTypei, CrossbarType crossbarTypeCri, double distance)
        {
            var Hf = Foundation.AllFoundations[foundationTypei].hf;
            var H1 = Foundation.AllFoundations[foundationTypei].h1;
            var H2 = Foundation.AllFoundations[foundationTypei].h2;
            var H3 = Foundation.AllFoundations[foundationTypei].h3;
            var H4 = Foundation.AllFoundations[foundationTypei].h4;
            var Hk = Foundation.AllFoundations[foundationTypei].hk1;

            var hkol = Hf - (H1 + H2 + H3 + H4 + Hk);
            var delta = (Foundation.AllFoundations[foundationTypei].ak - Foundation.AllFoundations[foundationTypei].ak1) * 0.5;
            var level_Crossbar = distance + Crossbar.AllCrossbar[crossbarTypeCri].Height;
            var delta_Crossbar = level_Crossbar * delta/ hkol;
            var a_Crossbar = Foundation.AllFoundations[foundationTypei].ak1 * 0.5 + delta_Crossbar;
            return a_Crossbar;
        }
    }
    public class VectorCrossbarAnker
    {
        public static Vector3d VectorAnker(FoundationType foundationTypei, CrossbarType crossbarTypeCri, double distance, MainWindowContext Context, int ai, int ri, double alpha)
        {
            int[] m = new int[] { Context.crossbarquantityType1, Context.crossbarquantityType2, Context.crossbarquantityType3, Context.crossbarquantityType4 };
            int[] n = new int[] { Context.crossbarquantityType1A, Context.crossbarquantityType2A, Context.crossbarquantityType3A, Context.crossbarquantityType4A };
            var Hf = Foundation.AllFoundations[foundationTypei].hf;
            var H1 = Foundation.AllFoundations[foundationTypei].h1;
            var H2 = Foundation.AllFoundations[foundationTypei].h2;
            var H3 = Foundation.AllFoundations[foundationTypei].h3;
            var H4 = Foundation.AllFoundations[foundationTypei].h4;
            var Hk = Foundation.AllFoundations[foundationTypei].hk1;
            var e = Foundation.AllFoundations[foundationTypei].e;
            var Hb = Context.HBetonPodgotovki;
            var Hsh = Context.HShebenPodgotovki;
            var hkol = Hf - (H1 + H2 + H3 + H4 + Hk);
            //Вектор по стороне "В"
            Vector3d Vector_AnkerB = new Vector3d();
            var delta_h = Crossbar.AllCrossbar[crossbarTypeCri].FullWidth * Math.Sin(alpha);
            double delta = (Foundation.AllFoundations[foundationTypei].bk - Foundation.AllFoundations[foundationTypei].bk1) * 0.5;
            double Height_Crossbar = new double();
            double level_Crossbar = new double();
            double delta_Crossbar = new double();
            double b_Crossbar = new double();
            double a_Crossbar = new double();
            Height_Crossbar = distance + Crossbar.AllCrossbar[crossbarTypeCri].Height * 0.5 + Crossbar.AllCrossbar[crossbarTypeCri].Height * (ri - 1);
            level_Crossbar = distance + Crossbar.AllCrossbar[crossbarTypeCri].Height * ri;
            delta_Crossbar = level_Crossbar * delta / hkol;
            b_Crossbar = Foundation.AllFoundations[foundationTypei].bk1 * 0.5 + delta_Crossbar;
            a_Crossbar = Height_Crossbar * e / hkol;
            Vector_AnkerB = new Vector3d(-a_Crossbar, (b_Crossbar), (Hf + Hb + Hsh - level_Crossbar));
            //var Vector_Anker = new Vector3d(-a_Crossbar, -(b_Crossbar + Crossbar.AllCrossbar[crossbarTypeCri].FullWidth), (Hf + Hb + Hsh - level_Crossbar));
            //var Vector_AnkerBi = new Vector3d(-a_Crossbar, (b_Crossbar), (Hf + Hb + Hsh - level_Crossbar));
            //Вектор по стороне "В"
            //Вектор по стороне "А"
            double deltaA = new double();
            double Height_CrossbarA = new double();
            double level_CrossbarA = new double();
            double delta_CrossbarA = new double();
            double a_CrossbarA = new double();
            Vector3d Vector_AnkerA = new Vector3d();
            deltaA = (Foundation.AllFoundations[foundationTypei].ak - Foundation.AllFoundations[foundationTypei].ak1) * 0.5;
            Height_CrossbarA = distance + (Crossbar.AllCrossbar[crossbarTypeCri].Height * 0.5) + (Crossbar.AllCrossbar[crossbarTypeCri].Height) * (ri - 1);
            level_CrossbarA = distance + (Crossbar.AllCrossbar[crossbarTypeCri].Height) * ri;
            delta_CrossbarA = level_CrossbarA * deltaA / hkol;
            a_CrossbarA = level_CrossbarA * e / hkol + Foundation.AllFoundations[foundationTypei].ak1 * 0.5 + delta_CrossbarA;
            Vector_AnkerA = new Vector3d(-a_CrossbarA, (0), (Hf + Hb + Hsh - level_CrossbarA));
            //Вектор по стороне "А"
            Vector3d Vector_Anker = new Vector3d();
            if (m[ai] > 0 && n[ai] == 0){ Vector_Anker = Vector_AnkerB; }
            else if (m[ai] == 0 && n[ai] > 0){ Vector_Anker = Vector_AnkerA; }
            else
            {
                if ((ri % 2) == 0) { Vector_Anker = Vector_AnkerA; }
                else { Vector_Anker = Vector_AnkerB; }
            }
            return Vector_Anker;
        }
    }
    public class VectorCrossbarStraight
    {
        public static Vector3d VectorStraight(FoundationType foundationTypei, CrossbarType crossbarTypeCri, double distance, MainWindowContext Context, int ai, int ri)
        {
            int[] m = new int[] { Context.crossbarquantityType1, Context.crossbarquantityType2, Context.crossbarquantityType3, Context.crossbarquantityType4 };
            int[] n = new int[] { Context.crossbarquantityType1A, Context.crossbarquantityType2A, Context.crossbarquantityType3A, Context.crossbarquantityType4A };
            var Hf = Foundation.AllFoundations[foundationTypei].hf;
            var H1 = Foundation.AllFoundations[foundationTypei].h1;
            var H2 = Foundation.AllFoundations[foundationTypei].h2;
            var H3 = Foundation.AllFoundations[foundationTypei].h3;
            var H4 = Foundation.AllFoundations[foundationTypei].h4;
            var Hk = Foundation.AllFoundations[foundationTypei].hk1;
            var e = Foundation.AllFoundations[foundationTypei].e;
            var Hb = Context.HBetonPodgotovki;
            var Hsh = Context.HShebenPodgotovki;
            
            var hkol = Hf - (H1 + H2 + H3 + H4 + Hk);
            //Вектор по стороне "В"
            var deltaB = (Foundation.AllFoundations[foundationTypei].ak - Foundation.AllFoundations[foundationTypei].ak1) * 0.5;
            var level_CrossbarB = distance + Crossbar.AllCrossbar[crossbarTypeCri].Height*ri;
            var delta_CrossbarB = level_CrossbarB * deltaB / hkol;
            var a_Crossbar = Foundation.AllFoundations[foundationTypei].ak1 * 0.5 + delta_CrossbarB;
            //Vector3d Vector_Straight = new Vector3d((a_Crossbar + Crossbar.AllCrossbar[crossbarTypeCri].FullWidth), 0, (Hf + Hb + Hsh - level_Crossbar));
            Vector3d Vector_StraightB = new Vector3d(-(a_Crossbar + Crossbar.AllCrossbar[crossbarTypeCri].FullWidth), 0, (Hf + Hb + Hsh - level_CrossbarB));
            //Вектор по стороне "В"
            //Вектор по стороне "А"
            var deltaA = (Foundation.AllFoundations[foundationTypei].bk - Foundation.AllFoundations[foundationTypei].bk1) * 0.5;
            var level_CrossbarA = distance + Crossbar.AllCrossbar[crossbarTypeCri].Height * ri;
            var delta_CrossbarA = level_CrossbarA * deltaA / hkol;
            var b_Crossbar = Foundation.AllFoundations[foundationTypei].bk1 * 0.5 + delta_CrossbarA;
            Vector3d Vector_StraightA = new Vector3d(0, -(b_Crossbar + Crossbar.AllCrossbar[crossbarTypeCri].FullWidth), (Hf + Hb + Hsh - level_CrossbarA));
            //Вектор по стороне "А"
            Vector3d Vector_Straight = new Vector3d();
            
            if (m[ai] > 0 && n[ai] == 0)
            {
                Vector_Straight = Vector_StraightB;
            }
            else if (m[ai] == 0 && n[ai] > 0)
            {
                Vector_Straight = Vector_StraightA;
            }
            else 
            {
                if ((ri % 2) == 0)
                {
                    Vector_Straight = Vector_StraightA;
                }
                else
                {
                    Vector_Straight = Vector_StraightB;
                }
            }
            return Vector_Straight;
        }
    }
    public class VectorBeamCrossbarAnker
    {
        public static Vector3d VectorAnker(FoundationType foundationTypei, CrossbarType crossbarTypeCri, double distance, MainWindowContext Context, int ai, int ri, double alpha, double betta)
        {
            int[] m = new int[] { Context.crossbarquantityType1, Context.crossbarquantityType2, Context.crossbarquantityType3, Context.crossbarquantityType4 };
            int[] n = new int[] { Context.crossbarquantityType1A, Context.crossbarquantityType2A, Context.crossbarquantityType3A, Context.crossbarquantityType4A };
            var Hf = Foundation.AllFoundations[foundationTypei].hf;
            var H1 = Foundation.AllFoundations[foundationTypei].h1;
            var H2 = Foundation.AllFoundations[foundationTypei].h2;
            var H3 = Foundation.AllFoundations[foundationTypei].h3;
            var H4 = Foundation.AllFoundations[foundationTypei].h4;
            var Hk = Foundation.AllFoundations[foundationTypei].hk1;
            var e = Foundation.AllFoundations[foundationTypei].e;
            var Hb = Context.HBetonPodgotovki;
            var Hsh = Context.HShebenPodgotovki;
            var hkol = Hf - (H1 + H2 + H3 + H4 + Hk);
            //Вектор по стороне "В"
            var delta = (Foundation.AllFoundations[foundationTypei].bk - Foundation.AllFoundations[foundationTypei].bk1) * 0.5;
            var Height_Crossbar = distance + Crossbar.AllCrossbar[crossbarTypeCri].Height * 0.5 + Crossbar.AllCrossbar[crossbarTypeCri].Height * (ri - 1);
            var level_Crossbar = distance + Crossbar.AllCrossbar[crossbarTypeCri].Height * ri;
            var delta_Crossbar = level_Crossbar * delta / hkol;
            var b_Crossbar = Foundation.AllFoundations[foundationTypei].bk1 * 0.5 + delta_Crossbar;
            var a_Crossbar = Height_Crossbar * e / hkol;
            //var Vector_Anker = new Vector3d(-a_Crossbar, -(b_Crossbar + Crossbar.AllCrossbar[crossbarTypeCri].FullWidth), (Hf + Hb + Hsh - level_Crossbar));
            var Vector_AnkerB = new Vector3d(-a_Crossbar, -(b_Crossbar), (Hf + Hb + Hsh - level_Crossbar));
            //Вектор по стороне "В"
            //Вектор по стороне "А"
            var delta_h = Crossbar.AllCrossbar[crossbarTypeCri].FullWidth * Math.Sin(alpha);
            var deltaA = (Foundation.AllFoundations[foundationTypei].ak - Foundation.AllFoundations[foundationTypei].ak1) * 0.5;
            var hsech = deltaA * 2 + Foundation.AllFoundations[foundationTypei].ak;
            var delta_hsech = hsech * Math.Sin(betta) * 0.65;
            var Height_CrossbarA = distance + Crossbar.AllCrossbar[crossbarTypeCri].Height * 0.5 + (Crossbar.AllCrossbar[crossbarTypeCri].Height) * (ri - 1) - 125 * 0.5 + delta_hsech;
            var level_CrossbarA = distance + Crossbar.AllCrossbar[crossbarTypeCri].Height * ri + delta_hsech;
            var delta_CrossbarA = Height_CrossbarA * deltaA / hkol;
            var delta_e = Height_CrossbarA * e / hkol;
            var a_CrossbarA = delta_e - delta_CrossbarA - Foundation.AllFoundations[foundationTypei].ak1 * 0.5;
            //var a_CrossbarA = level_Crossbar * e / hkol + Foundation.AllFoundations[foundationTypei].ak1 * 0.5 + delta_CrossbarA;
            var Vector_AnkerA = new Vector3d(-a_CrossbarA, (0), (Hf + Hb + Hsh - level_CrossbarA));
            //Вектор по стороне "А"
            Vector3d Vector_Anker = new Vector3d();
            if (m[ai] > 0 && n[ai] == 0) { Vector_Anker = Vector_AnkerB; }
            else if (m[ai] == 0 && n[ai] > 0) { Vector_Anker = Vector_AnkerA; }
            else
            {
                if ((ri % 2) == 0) { Vector_Anker = Vector_AnkerA; }
                else { Vector_Anker = Vector_AnkerB; }
            }
            return Vector_Anker;
        }
    }
    public class VectorBeamCrossbarStraight
    {
        public static Vector3d VectorStraight(FoundationType foundationTypei, CrossbarType crossbarTypeCri, double distance, MainWindowContext Context, int ai, int ri)
        {
            int[] m = new int[] { Context.crossbarquantityType1, Context.crossbarquantityType2, Context.crossbarquantityType3, Context.crossbarquantityType4 };
            int[] n = new int[] { Context.crossbarquantityType1A, Context.crossbarquantityType2A, Context.crossbarquantityType3A, Context.crossbarquantityType4A };
            var Hf = Foundation.AllFoundations[foundationTypei].hf;
            var H1 = Foundation.AllFoundations[foundationTypei].h1;
            var H2 = Foundation.AllFoundations[foundationTypei].h2;
            var H3 = Foundation.AllFoundations[foundationTypei].h3;
            var H4 = Foundation.AllFoundations[foundationTypei].h4;
            var Hk = Foundation.AllFoundations[foundationTypei].hk1;
            var e = Foundation.AllFoundations[foundationTypei].e;
            var Hb = Context.HBetonPodgotovki;
            var Hsh = Context.HShebenPodgotovki;

            var hkol = Hf - (H1 + H2 + H3 + H4 + Hk);
            //Вектор по стороне "В"
            var deltaB = (Foundation.AllFoundations[foundationTypei].ak - Foundation.AllFoundations[foundationTypei].ak1) * 0.5;
            var level_CrossbarB = distance + Crossbar.AllCrossbar[crossbarTypeCri].Height * ri;
            var delta_CrossbarB = level_CrossbarB * deltaB / hkol;
            var a_Crossbar = Foundation.AllFoundations[foundationTypei].ak1 * 0.5 + delta_CrossbarB;
            //Vector3d Vector_Straight = new Vector3d((a_Crossbar + Crossbar.AllCrossbar[crossbarTypeCri].FullWidth), 0, (Hf + Hb + Hsh - level_Crossbar));
            Vector3d Vector_StraightB = new Vector3d((a_Crossbar), 0, (Hf + Hb + Hsh - level_CrossbarB));
            //Вектор по стороне "В"
            //Вектор по стороне "А"
            var deltaA = (Foundation.AllFoundations[foundationTypei].bk - Foundation.AllFoundations[foundationTypei].bk1) * 0.5;
            var level_CrossbarA = distance + Crossbar.AllCrossbar[crossbarTypeCri].Height * ri;
            var delta_CrossbarA = level_CrossbarA * deltaA / hkol;
            var b_Crossbar = Foundation.AllFoundations[foundationTypei].bk1 * 0.5 + delta_CrossbarA;
            Vector3d Vector_StraightA = new Vector3d(0, (b_Crossbar), (Hf + Hb + Hsh - level_CrossbarB));
            //Вектор по стороне "А"
            Vector3d Vector_Straight = new Vector3d();
            if (m[ai] > 0 && n[ai] == 0)
            {
                Vector_Straight = Vector_StraightB;
            }
            else if (m[ai] == 0 && n[ai] > 0)
            {
                Vector_Straight = Vector_StraightA;
            }
            else
            {
                if ((ri % 2) == 0)
                {
                    Vector_Straight = Vector_StraightA;
                }
                else
                {
                    Vector_Straight = Vector_StraightB;
                }
            }
            return Vector_Straight;
        }
    }
}


