using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ACADprogram
{
    public class Crossbar
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public int FullWidth { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length1 { get; set; }
        public int Length2 { get; set; }
        public int Boltdistance { get; set; }
        public int holediameter { get; set; }
        public int boltdiameter { get; set; }
        public int boltlength { get; set; }
        public int beamlength { get; set; }

        public enum CrossbarType
        {
            R1n, R2n, R1, R1_A, AR5, AR6, AR6_1, AR7, AR7_1, AR8, No
        }
        public class CrossbarName
        {
            public CrossbarType Type { get; set; }
            public string Name { get; set; }
        }
        public static List<CrossbarName> crossbarnames { get; set; } = new List<CrossbarName>()
        {
            new CrossbarName() { Type = CrossbarType.No, Name = "нет" },
            new CrossbarName() { Type = CrossbarType.R1n, Name = "Ригель Р1н" },
            new CrossbarName() { Type = CrossbarType.R2n, Name = "Ригель Р2н" },
            new CrossbarName() { Type = CrossbarType.R1, Name = "Ригель Р1" },
            new CrossbarName() { Type = CrossbarType.R1_A, Name = "Ригель Р1-А" },
            new CrossbarName() { Type = CrossbarType.AR5, Name = "Ригель АР 5" },
            new CrossbarName() { Type = CrossbarType.AR6, Name = "Ригель АР 6" },
            new CrossbarName() { Type = CrossbarType.AR6_1, Name = "Ригель АР 6-1" },
            new CrossbarName() { Type = CrossbarType.AR7, Name = "Ригель АР 7" },
            new CrossbarName() { Type = CrossbarType.AR7_1, Name = "Ригель АР 7-1" },
            new CrossbarName() { Type = CrossbarType.AR8, Name = "Ригель АР 8" }
        };
        public static readonly Dictionary<CrossbarType, Crossbar> AllCrossbar = new Dictionary<CrossbarType, Crossbar>()
        {
            {
                CrossbarType.No, new Crossbar()
                {
                    Length = 0,
                    FullWidth = 0,
                    Height = 0,
                    Width = 0,
                    Length1 = 0,
                    Length2 = 0,
                    Boltdistance = 0,
                    holediameter = 0,
                    boltdiameter= 0,
                    boltlength=0,
                    beamlength = 0
                }
            },
            {
                CrossbarType.R1n, new Crossbar()
                {
                    Length = 3000,
                    FullWidth = 200,
                    Height = 400,
                    Width = 70,
                    Length1 = 900,
                    Length2 = 1200,
                    Boltdistance = 620,
                    holediameter = 40,
                    boltdiameter= 30,
                    boltlength=1000,
                    beamlength = 750
                }
            },
            {
                CrossbarType.R2n, new Crossbar()
                {
                    Length = 1500,
                    FullWidth = 140,
                    Height = 500,
                    Width = 70,
                    Length1 = 650,
                    Length2 = 120,
                    Boltdistance = 620,
                    holediameter = 40,
                    boltdiameter= 27,
                    boltlength=640,
                    beamlength = 740
                }
            },
            {
                CrossbarType.R1, new Crossbar()
                {
                    Length = 1500,
                    FullWidth = 140,
                    Height = 500,
                    Width = 70,
                    Length1 = 650,
                    Length2 = 120,
                    Boltdistance = 620,
                    holediameter = 40,
                    boltdiameter= 27,
                    boltlength=640,
                    beamlength = 740
                }
            },
            {
                CrossbarType.R1_A, new Crossbar()
                {
                    Length = 3000,
                    FullWidth = 200,
                    Height = 400,
                    Width = 70,
                    Length1 = 900,
                    Length2 = 1200,
                    Boltdistance = 620,
                    holediameter = 40,
                    boltdiameter= 27,
                    boltlength=1000,
                    beamlength = 740
                }
            },
            {
                CrossbarType.AR5, new Crossbar()
                {
                    Length = 3000,
                    FullWidth = 200,
                    Height = 400,
                    Width = 70,
                    Length1 = 900,
                    Length2 = 1200,
                    Boltdistance = 620,
                    holediameter = 40,
                    boltdiameter= 30,
                    boltlength=1000,
                    beamlength = 750 
                }
            },
            {
                CrossbarType.AR6, new Crossbar()
                {
                    Length = 3500,
                    FullWidth = 200,
                    Height = 500,
                    Width = 70,
                    Length1 = 1150,
                    Length2 = 1200,
                    Boltdistance = 700,
                    holediameter = 40,
                    boltdiameter= 30,
                    boltlength=1000,
                    beamlength = 830
                }
            },
            {
                CrossbarType.AR6_1, new Crossbar()
                {
                    Length = 3500,
                    FullWidth = 200,
                    Height = 500,
                    Width = 70,
                    Length1 = 1150,
                    Length2 = 1200,
                    Boltdistance = 810,
                    holediameter = 50,
                    boltdiameter= 40,
                    boltlength=1000,
                    beamlength = 940
                }
            },
            {
                CrossbarType.AR7, new Crossbar()
                {
                    Length = 2000,
                    FullWidth = 200,
                    Height = 300,
                    Width = 70,
                    Length1 = 750,
                    Length2 = 500,
                    Boltdistance = 400,
                    holediameter = 40,
                    boltdiameter= 30,
                    boltlength=700,
                    beamlength = 550
                }
            },
            {
                CrossbarType.AR7_1, new Crossbar()
                {
                    Length = 2000,
                    FullWidth = 200,
                    Height = 300,
                    Width = 70,
                    Length1 = 750,
                    Length2 = 500,
                    Boltdistance = 430,
                    holediameter = 40,
                    boltdiameter= 30,
                    boltlength=700,
                    beamlength = 550
                }
            },
            {
                CrossbarType.AR8, new Crossbar()
                {
                    Length = 6000,
                    FullWidth = 350,
                    Height = 640,
                    Width = 80,
                    Length1 = 1750,
                    Length2 = 2500,
                    Boltdistance = 810,
                    holediameter = 50,
                    boltdiameter= 40,
                    boltlength=1000,
                    beamlength = 940
                }
            },
        };
        public static bool TryBuildCrossbar(CrossbarType CrossbarTypeCri, Vector3d vector_ponizh, MainWindowContext Context,
            out Solid3d Crossbar_Solid3d, FoundationType FoundationTypei, int ai, int ri, out double alpha, out double betta)
        {
            alpha = 0;
            betta = 0;
            Crossbar_Solid3d = null;
            if (Crossbar.AllCrossbar[CrossbarTypeCri] == Crossbar.AllCrossbar[CrossbarType.No]) return false;
            Crossbar_Solid3d = new Solid3d();
            var length = Crossbar.AllCrossbar[CrossbarTypeCri].Length;
            var fullwidth = Crossbar.AllCrossbar[CrossbarTypeCri].FullWidth;
            var height = Crossbar.AllCrossbar[CrossbarTypeCri].Height;
            var width = Crossbar.AllCrossbar[CrossbarTypeCri].Width;
            var length1 = Crossbar.AllCrossbar[CrossbarTypeCri].Length1;
            var length2 = Crossbar.AllCrossbar[CrossbarTypeCri].Length2;
            var boltdistance = Crossbar.AllCrossbar[CrossbarTypeCri].Boltdistance;

            //Угол наклона ригеля на анкерных фундаментах 
            var ll = Foundation.AllFoundations[FoundationTypei].e + Foundation.AllFoundations[FoundationTypei].ak * 0.5 - Foundation.AllFoundations[FoundationTypei].ak1 * 0.5;
            var Hkolfoud = Foundation.AllFoundations[FoundationTypei].hf - Foundation.AllFoundations[FoundationTypei].h1 - Foundation.AllFoundations[FoundationTypei].h2 - Foundation.AllFoundations[FoundationTypei].h3 - Foundation.AllFoundations[FoundationTypei].h4 - Foundation.AllFoundations[FoundationTypei].hk1;
            alpha = Math.Atan(ll / Hkolfoud);
            //Угол наклона ригеля на анкерных фундаментах 
            //Угол наклона элемента крепления ригеля
            var mm = (Foundation.AllFoundations[FoundationTypei].e - Foundation.AllFoundations[FoundationTypei].ak * 0.5) + Foundation.AllFoundations[FoundationTypei].ak1 * 0.5;
            betta = Math.Atan(mm / Hkolfoud);
            //Угол наклона элемента крепления ригеля
            //Построение ригеля
            Point3d[] ptCr;
            ptCr = new Point3d[8]
            {
                new Point3d(-length2*0.5, 0, 0),
                new Point3d(-length*0.5, 0, 0),
                new Point3d(-length*0.5, width, 0),
                new Point3d(-length2*0.5, fullwidth, 0),
                new Point3d(length2*0.5, fullwidth, 0),
                new Point3d(length*0.5, width, 0),
                new Point3d(length*0.5, 0, 0),
                new Point3d(length2*0.5, 0, 0)
            };
            var plCrDown = new VirtualPolyline3d8pt(ptCr[0], ptCr[1], ptCr[2], ptCr[3], ptCr[4], ptCr[5], ptCr[6], ptCr[7]);
            VirtualPolyline3d8pt plCrUp = new VirtualPolyline3d8pt(ptCr[0], ptCr[1], ptCr[2], ptCr[3], ptCr[4], ptCr[5], ptCr[6], ptCr[7]);
            plCrUp.Move(new Vector3d(0, 0, height));
            LoftOptionsBuilder loftOptionsBuilder = new LoftOptionsBuilder()
            {
                Ruled = true
            };
            
                Crossbar_Solid3d.CreateLoftedSolid(new LoftProfile[]
                            {
                                 new LoftProfile(plCrDown.Gets()),
                                 new LoftProfile(plCrUp.Gets()) 
                            },
                          new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
                Circle bolt_Circle = new Circle(new Point3d(0, 0, 0), new Vector3d(0, AllCrossbar[CrossbarTypeCri].FullWidth, 0), AllCrossbar[CrossbarTypeCri].holediameter * 0.5);
                Solid3d Hole_bolt = new Solid3d();
                DBObjectCollection CrossbarHoleBolt = new DBObjectCollection();
                CrossbarHoleBolt.Add(bolt_Circle);
                DBObjectCollection Region_CrossbarHoleBolt = new DBObjectCollection();
                Region_CrossbarHoleBolt = Region.CreateFromCurves(CrossbarHoleBolt);
                Region acRegion_CRHB = Region_CrossbarHoleBolt[0] as Region;
                Hole_bolt.Extrude(acRegion_CRHB, 5000, 0);
                Hole_bolt.TransformBy(Matrix3d.Displacement(new Vector3d(-AllCrossbar[CrossbarTypeCri].Boltdistance * 0.5, 0, AllCrossbar[CrossbarTypeCri].Height * 0.5)));
                Solid3d Hole_bolt2 = new Solid3d();
                Hole_bolt2.CopyFrom(Hole_bolt);
                Hole_bolt2.TransformBy(Matrix3d.Displacement(new Vector3d(Crossbar.AllCrossbar[CrossbarTypeCri].Boltdistance, 0, 0)));
                Crossbar_Solid3d.BooleanOperation(BooleanOperationType.BoolSubtract, Hole_bolt);
                Crossbar_Solid3d.BooleanOperation(BooleanOperationType.BoolSubtract, Hole_bolt2);
            //Детали крепления ригеля    
            //Уголок L125x8
            Point3d[] pt_d13 = new Point3d[8]
            {
                new Point3d(0, 0, 0),
                new Point3d(0, 125*0.5, 0),
                new Point3d(0, 125, 0),
                new Point3d(8, 125, 0),
                new Point3d(8, 8, 0),
                new Point3d(125, 8, 0),
                new Point3d(125, 0, 0),
                new Point3d(125*0.5, 0, 0)
            };
            var pl_d13 = new VirtualPolyline3d8pt(pt_d13[0], pt_d13[1], pt_d13[2], pt_d13[3], pt_d13[4], pt_d13[5], pt_d13[6], pt_d13[7]);
            DBObjectCollection d13_BD = new DBObjectCollection();
            d13_BD.Add(pl_d13.Gets());
            DBObjectCollection RegionBD_d13=new DBObjectCollection();
            RegionBD_d13=Region.CreateFromCurves(d13_BD);
            Region Region_d13= RegionBD_d13[0] as Region;
            Solid3d D13 = new Solid3d();
            D13.Extrude(Region_d13, AllCrossbar[CrossbarTypeCri].beamlength, 0);
            D13.TransformBy(Matrix3d.Rotation(90 * Math.PI / 180, new Vector3d(0, 1, 0), new Point3d(0, 0, 0)));
            D13.TransformBy(Matrix3d.Displacement(new Vector3d(AllCrossbar[CrossbarTypeCri].beamlength / 2, 0, (AllCrossbar[CrossbarTypeCri].Height * 0.5 + 125 * 0.5))));
            D13.BooleanOperation(BooleanOperationType.BoolSubtract, Hole_bolt);
            D13.BooleanOperation(BooleanOperationType.BoolSubtract, Hole_bolt2);
            //Уголок L125x8
            //болты
            Circle bolt_C = new Circle(new Point3d(0, 0, 0), new Vector3d(0, AllCrossbar[CrossbarTypeCri].FullWidth, 0), AllCrossbar[CrossbarTypeCri].boltdiameter * 0.5);
            Solid3d Bolt_Srossbar = new Solid3d();
            DBObjectCollection Bolt_SrossbarBD = new DBObjectCollection();
            Bolt_SrossbarBD.Add(bolt_C);
            DBObjectCollection Region_Bolt_SrossbarBD = new DBObjectCollection();
            Region_Bolt_SrossbarBD = Region.CreateFromCurves(Bolt_SrossbarBD);
            Region BoltCR_Region = Region_Bolt_SrossbarBD[0] as Region;
            Bolt_Srossbar.Extrude(BoltCR_Region, AllCrossbar[CrossbarTypeCri].boltlength, 0);
            Bolt_Srossbar.TransformBy(Matrix3d.Displacement(new Vector3d(-AllCrossbar[CrossbarTypeCri].Boltdistance * 0.5, -(AllCrossbar[CrossbarTypeCri].boltlength * 0.5 + 300), AllCrossbar[CrossbarTypeCri].Height * 0.5)));
            Solid3d Bolt_Srossbar2 = new Solid3d();
            Bolt_Srossbar2.CopyFrom(Bolt_Srossbar);
            Bolt_Srossbar2.TransformBy(Matrix3d.Displacement(new Vector3d(Crossbar.AllCrossbar[CrossbarTypeCri].Boltdistance, 0, 0)));
            //болты
            D13.BooleanOperation(BooleanOperationType.BoolUnite, Bolt_Srossbar);
            D13.BooleanOperation(BooleanOperationType.BoolUnite, Bolt_Srossbar2);
            //Детали крепления ригеля 
            D13.TransformBy(Matrix3d.Displacement(vector_ponizh));
            Crossbar_Solid3d.TransformBy(Matrix3d.Displacement(vector_ponizh));

            int[] m = new int[] { Context.crossbarquantityType1, Context.crossbarquantityType2, Context.crossbarquantityType3, Context.crossbarquantityType4 };
            int[] n = new int[] { Context.crossbarquantityType1A, Context.crossbarquantityType2A, Context.crossbarquantityType3A, Context.crossbarquantityType4A };
            if (m[ai] > 0 && n[ai] == 0)
            {
                if (Foundation.AllFoundations[FoundationTypei].e == 0)
                {
                    D13.TransformBy(Matrix3d.Rotation(270 * Math.PI / 180, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    D13.TransformBy(Matrix3d.Displacement(VectorBeamCrossbarStraight.VectorStraight(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri)));
                    Crossbar_Solid3d.TransformBy(Matrix3d.Rotation(270 * Math.PI / 180, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    Crossbar_Solid3d.TransformBy(Matrix3d.Displacement(VectorCrossbarStraight.VectorStraight(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri)));
                }
                else
                {
                    D13.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    D13.TransformBy(Matrix3d.Displacement(VectorBeamCrossbarAnker.VectorAnker(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri, alpha, betta)));
                    Crossbar_Solid3d.TransformBy(Matrix3d.Displacement(VectorCrossbarAnker.VectorAnker(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri, alpha)));
                }
            }
            else if (m[ai] == 0 && n[ai] > 0)
            {
                if (Foundation.AllFoundations[FoundationTypei].e == 0)
                {
                    //D13.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    D13.TransformBy(Matrix3d.Displacement(VectorBeamCrossbarStraight.VectorStraight(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri)));
                    //Crossbar_Solid3d.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    Crossbar_Solid3d.TransformBy(Matrix3d.Displacement(VectorCrossbarStraight.VectorStraight(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri)));
                }
                else
                {
                    D13.TransformBy(Matrix3d.Rotation(-betta, new Vector3d(1, 0, 0), new Point3d(0, 0, (AllCrossbar[CrossbarTypeCri].Height * 0.5 + 125 * 0.5))));
                    D13.TransformBy(Matrix3d.Rotation(270 * Math.PI / 180, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    Crossbar_Solid3d.TransformBy(Matrix3d.Rotation(alpha, new Vector3d(1, 0, 0), new Point3d(0, 0, 0)));
                    Crossbar_Solid3d.TransformBy(Matrix3d.Rotation(90 * Math.PI / 180, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    D13.TransformBy(Matrix3d.Displacement(VectorBeamCrossbarAnker.VectorAnker(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri, alpha, betta)));
                    Crossbar_Solid3d.TransformBy(Matrix3d.Displacement(VectorCrossbarAnker.VectorAnker(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri, alpha)));
                }
            }
            else
            {
                if ((ri % 2) == 0)
                {
                    if (Foundation.AllFoundations[FoundationTypei].e == 0)
                    {
                        //D13.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        D13.TransformBy(Matrix3d.Displacement(VectorBeamCrossbarStraight.VectorStraight(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri)));
                        //Crossbar_Solid3d.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        Crossbar_Solid3d.TransformBy(Matrix3d.Displacement(VectorCrossbarStraight.VectorStraight(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri)));
                    }
                    else
                    {
                        D13.TransformBy(Matrix3d.Rotation(-betta, new Vector3d(1, 0, 0), new Point3d(0, 0, (AllCrossbar[CrossbarTypeCri].Height * 0.5 + 125 * 0.5))));
                        D13.TransformBy(Matrix3d.Rotation(270 * Math.PI / 180, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        Crossbar_Solid3d.TransformBy(Matrix3d.Rotation(alpha, new Vector3d(1, 0, 0), new Point3d(0, 0, 0)));
                        Crossbar_Solid3d.TransformBy(Matrix3d.Rotation(90 * Math.PI / 180, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        D13.TransformBy(Matrix3d.Displacement(VectorBeamCrossbarAnker.VectorAnker(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri, alpha, betta)));
                        Crossbar_Solid3d.TransformBy(Matrix3d.Displacement(VectorCrossbarAnker.VectorAnker(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri, alpha)));
                    }
                }
                else
                {
                    if (Foundation.AllFoundations[FoundationTypei].e == 0)
                    {
                        D13.TransformBy(Matrix3d.Rotation(270 * Math.PI / 180, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        D13.TransformBy(Matrix3d.Displacement(VectorBeamCrossbarStraight.VectorStraight(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri)));
                        Crossbar_Solid3d.TransformBy(Matrix3d.Rotation(270 * Math.PI / 180, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        Crossbar_Solid3d.TransformBy(Matrix3d.Displacement(VectorCrossbarStraight.VectorStraight(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri)));
                    }
                    else
                    {
                        D13.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                        D13.TransformBy(Matrix3d.Displacement(VectorBeamCrossbarAnker.VectorAnker(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri, alpha, betta)));
                        Crossbar_Solid3d.TransformBy(Matrix3d.Displacement(VectorCrossbarAnker.VectorAnker(FoundationTypei, CrossbarTypeCri, Context.crossbarlocation, Context, ai, ri, alpha)));
                    }
                }
            }
            Crossbar_Solid3d.BooleanOperation(BooleanOperationType.BoolUnite, D13);
            return true;
        }
    }
}
