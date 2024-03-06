using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace ACADprogram
{
    public abstract class BeamBase
    {
        public abstract bool BuildBeam(MainWindowContext context, out Solid3d Solid, out Solid3d Hole_d1, out Vector3d[] pt_holed1, out Vector3d[] pt_hole2, out Solid3d B1n_d4, out Solid3d B1n_d5, out Solid3d B1n_d6);
    }

    public class MainBeam : BeamBase
    {
        public override bool BuildBeam(MainWindowContext context, out Solid3d B1n, out Solid3d Hole_d1, out Vector3d[] pt_holed1, out Vector3d[] pt_hole2, out Solid3d B1n_d4, out Solid3d B1n_d5, out Solid3d B1n_d6) =>
            BuildMainBeam(context, out B1n, out Hole_d1, out pt_holed1, out pt_hole2, out  B1n_d4, out B1n_d5, out  B1n_d6);

        protected bool BuildMainBeam(MainWindowContext context, out Solid3d B1n, out Solid3d Hole_d1,  out Vector3d[] pt_holed1, out Vector3d[] pt_hole2, out Solid3d B1n_d4, out Solid3d B1n_d5, out Solid3d B1n_d6)
        {
            B1n_d4 = null;
            B1n_d5 = null;
            B1n_d6 = null;
            Hole_d1 = null;
            pt_hole2 = null;
            pt_holed1 = null;
            B1n = null;
            if (context.Beamlocation == 0) return false;
            B1n = new Solid3d();
            var d1_length_1 = 130;
            var d1_length_2 = 790;
            var d1_width_1 = 130;
            var d1_width_2 = 230;
            var d1_height = 40;
            Point3d[] pt_d1;
            pt_d1 = new Point3d[8]
            {
                new Point3d(-d1_length_2 * 0.5, -(d1_width_1 + d1_width_2  *0.5), 0),
                new Point3d(-(d1_length_1 + d1_length_2 * 0.5), -d1_width_2 * 0.5, 0),
                new Point3d(-(d1_length_1 + d1_length_2 * 0.5), d1_width_2 * 0.5, 0),
                new Point3d(-d1_length_2 * 0.5, (d1_width_1 + d1_width_2 * 0.5), 0),
                new Point3d(d1_length_2*0.5, (d1_width_1 + d1_width_2 * 0.5), 0),
                new Point3d(d1_length_1 + d1_length_2 * 0.5, d1_width_2 * 0.5, 0),
                new Point3d(d1_length_1 + d1_length_2 * 0.5, -d1_width_2 * 0.5, 0),
                new Point3d(d1_length_2 * 0.5, -(d1_width_1 + d1_width_2  *0.5), 0),
            };
            var pl_d1 = new VirtualPolyline3d8pt(pt_d1[0], pt_d1[1], pt_d1[2], pt_d1[3], pt_d1[4], pt_d1[5], pt_d1[6], pt_d1[7]);
            var pl2_d1 = new VirtualPolyline3d8pt(pt_d1[0], pt_d1[1], pt_d1[2], pt_d1[3], pt_d1[4], pt_d1[5], pt_d1[6], pt_d1[7]);
            pl2_d1.Move(new Vector3d(0, 0, d1_height));
            B1n = VirtualLoftSolid.loftSolidBuild(pl_d1.Gets(), pl2_d1.Gets());
            var cpl_d1 = new Circle(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), 22);
            Region acRegion_hole_d1 = VirtualRegion.RegionBuild(cpl_d1);
            Hole_d1 = new Solid3d();
            Hole_d1.Extrude(acRegion_hole_d1, 100, 0);
            pt_holed1 = new Vector3d[6]
            {
                new Vector3d(-560 * 0.5, - 177, 0),
                new Vector3d(-(560 * 0.5 + 177), 0, 0),
                new Vector3d(-560 * 0.5, 177, 0),
                new Vector3d(560 * 0.5, -177, 0),
                new Vector3d((560 * 0.5 + 177), 0, 0),
                new Vector3d(560 * 0.5, 177, 0),
            };
            Solid3d hole_d1j = new Solid3d();
            foreach (var i in pt_holed1)
            {
                var newhole = hole_d1j;
                hole_d1j.CopyFrom(Hole_d1);
                hole_d1j.TransformBy(Matrix3d.Displacement(i));
                B1n.BooleanOperation(BooleanOperationType.BoolSubtract, newhole);
                hole_d1j = newhole;
            }
            Point3d[] pt_d2;
            pt_d2 = new Point3d[8]
            {
                new Point3d(-500*0.5, -(175+90), 460),
                new Point3d(-(500*0.5+200), -65, 460),
                new Point3d(-(500*0.5+200), 530*0.5, 460),
                new Point3d(0, 530*0.5, 460),
                new Point3d((500*0.5+200), 530*0.5, 460),
                new Point3d((500*0.5+200), -65, 460),
                new Point3d(500*0.5, -(175+90), 460),
                new Point3d(0 , -(175+90), 460),
            };
            var pl_d2 = new VirtualPolyline3d8pt(pt_d2[0], pt_d2[1], pt_d2[2], pt_d2[3], pt_d2[4], pt_d2[5], pt_d2[6], pt_d2[7]);
            var pl2_d2 = new VirtualPolyline3d8pt(pt_d2[0], pt_d2[1], pt_d2[2], pt_d2[3], pt_d2[4], pt_d2[5], pt_d2[6], pt_d2[7]);
            pl2_d2.Move(new Vector3d(0, 0, 40));
            Solid3d B1n_d2 = VirtualLoftSolid.loftSolidBuild(pl_d2.Gets(), pl2_d2.Gets());
            var cpl_d2 = new Circle(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), 29);
            Region acRegion_hole_d2 = VirtualRegion.RegionBuild(cpl_d2);
            Solid3d Hole_d2 = new Solid3d();
            Hole_d2.Extrude(acRegion_hole_d2, 1000, 0);
            
            pt_hole2 = new Vector3d[8] 
            {
                new Vector3d(-175, -175, 0),
                new Vector3d(-177, 0, 0),
                new Vector3d(-175, 175, 0),
                new Vector3d(0, 177, 0),
                new Vector3d(175, 175, 0),
                new Vector3d(177, 0, 0),
                new Vector3d(175, -175, 0),
                new Vector3d(0, -177, 0),
            };
            Solid3d hole_d2j = new Solid3d();
            foreach (var j in pt_hole2)
            {
                var newhole2 = hole_d2j;
                hole_d2j.CopyFrom(Hole_d2);
                hole_d2j.TransformBy(Matrix3d.Displacement(j));
                B1n_d2.BooleanOperation(BooleanOperationType.BoolSubtract, newhole2);
                hole_d2j = newhole2;
            }
            B1n_d2.TransformBy(Matrix3d.Displacement(new Vector3d(0, -102, 0)));
            Point3d[] pt_d3;
            pt_d3 = new Point3d[4]
            {
                new Point3d(-555*0.5,-8,40),
                new Point3d(-555*0.5,8,40),
                new Point3d(555*0.5,8,40),
                new Point3d(555*0.5,-8,40),
            };
            var pl_d3 = new VirtualPolyline3d(pt_d3[0], pt_d3[1], pt_d3[2], pt_d3[3]);
            var pl2_d3 = new VirtualPolyline3d(pt_d3[0], pt_d3[1], pt_d3[2], pt_d3[3]);
            pl2_d3.Move(new Vector3d(0, 0, 460));
            Solid3d B1n_d3= VirtualLoftSolid.loftSolidBuild(pl_d3.Gets(), pl2_d3.Gets()); 
            Point3d[] pt_d4;
            pt_d4 = new Point3d[4] 
            {
                new Point3d(-96, 8, 40),
                new Point3d(-96, 243, 40),
                new Point3d(-96, 163, 460),
                new Point3d(-96, 8, 460),
            };
            var pl_d4 = new VirtualPolyline3d(pt_d4[0], pt_d4[1], pt_d4[2], pt_d4[3]);
            var pl2_d4 = new VirtualPolyline3d(pt_d4[0], pt_d4[1], pt_d4[2], pt_d4[3]);
            pl2_d4.Move(new Vector3d(12, 0, 0));
            B1n_d4 = new Solid3d();
            B1n_d4 = VirtualLoftSolid.loftSolidBuild(pl_d4.Gets(), pl2_d4.Gets());
            Solid3d B1n2_d4=new Solid3d();
            B1n2_d4.CopyFrom(B1n_d4);
            B1n2_d4.TransformBy(Matrix3d.Displacement(new Vector3d(180, 0, 0)));
            Point3d[] pt_d5;
            pt_d5 = new Point3d[4] 
            {
                new Point3d(-96, -8, 40),
                new Point3d(-96, -243, 40),
                new Point3d(-96, -368, 460),
                new Point3d(-96, -8, 460),
            };
            var pl_d5 = new VirtualPolyline3d(pt_d5[0], pt_d5[1], pt_d5[2], pt_d5[3]);
            var pl2_d5 = new VirtualPolyline3d(pt_d5[0], pt_d5[1], pt_d5[2], pt_d5[3]);
            pl2_d5.Move(new Vector3d(12, 0, 0));
            B1n_d5 = new Solid3d();
            B1n_d5 = VirtualLoftSolid.loftSolidBuild(pl_d5.Gets(), pl2_d5.Gets());
            Solid3d B1n2_d5=new Solid3d();
            B1n2_d5.CopyFrom(B1n_d5);
            B1n2_d5.TransformBy(Matrix3d.Displacement(new Vector3d(180, 0, 0)));
            Point3d[] pt_d6;
            pt_d6 = new Point3d[8]
            {
                new Point3d(-278, -8, 40),
                new Point3d(-444, -175, 40),
                new Point3d(-452, -166, 40),
                new Point3d(-286, 0, 40),
                new Point3d(-452, 166, 40),
                new Point3d(-444, 175, 40),
                new Point3d(-361, 95, 40),
                new Point3d(-278, 8, 40),
            };
            var pl_d6 = new VirtualPolyline3d8pt(pt_d6[0], pt_d6[1], pt_d6[2], pt_d6[3], pt_d6[4], pt_d6[5], pt_d6[6], pt_d6[7]);
            var pl2_d6 = new VirtualPolyline3d8pt(pt_d6[0], pt_d6[1], pt_d6[2], pt_d6[3], pt_d6[4], pt_d6[5], pt_d6[6], pt_d6[7]);
            pl2_d6.Move(new Vector3d(0, 0, 420));
            B1n_d6 = new Solid3d();
            B1n_d6 = VirtualLoftSolid.loftSolidBuild(pl_d6.Gets(), pl2_d6.Gets());
            Solid3d B1n2_d6 = new Solid3d();
            B1n2_d6.CopyFrom(B1n_d6);
            B1n2_d6.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 40)));

            Solid3d[] B1n_di = new Solid3d[8] { B1n_d2, B1n_d3, B1n_d4, B1n2_d4, B1n_d5, B1n2_d5, B1n_d6, B1n2_d6 };
            foreach (var k in B1n_di)
            {
                B1n.BooleanOperation(BooleanOperationType.BoolUnite, k);
            }
            return true;
        }  
    }
    public class NextBeam : MainBeam
    {
        public override bool BuildBeam(MainWindowContext context, out Solid3d B1n, out Solid3d Hole_d1, out Vector3d[] pt_holed1, out Vector3d[] pt_hole2, out Solid3d B1n_d4, out Solid3d B1n_d5, out Solid3d B1n_d6)
        {
            BuildMainBeam(context, out _, out var MainHole_d1, out var Mainpt_holed1, out var Mainpt_hole2, out var MainB1n_d4, out var MainB1n_d5, out var MainB1n_d6);
            return BuildNext(context, out B1n, out Hole_d1, out pt_holed1, out pt_hole2, out B1n_d4, out B1n_d5, out B1n_d6, MainHole_d1, Mainpt_holed1, Mainpt_hole2, MainB1n_d4, MainB1n_d5, MainB1n_d6);
        }

        protected bool BuildNext(MainWindowContext context, out Solid3d B2n, out Solid3d Hole_d1, out Vector3d[] pt_holed1, out Vector3d[] pt_hole2, out Solid3d B1n_d4, 
            out Solid3d B1n_d5, out Solid3d B1n_d6, Solid3d MainHole_d1, Vector3d[] Mainpt_holed1, Vector3d[] Mainpt_hole2, Solid3d MainB1n_d4, 
            Solid3d MainB1n_d5, Solid3d MainB1n_d6)
        {
            Hole_d1 = null;
            pt_holed1 = null;
            pt_hole2 = null;
            B2n = null;
            B1n_d4 = null;
            B1n_d5 = null;
            B1n_d6 = null;
            if (context.Beamlocation == 0) return false;
            B2n = new Solid3d();
            var d1_distance = 790 * 0.5;
            Point3d[] pt_d1;
            pt_d1 = new Point3d[8] 
            {
                new Point3d(-100, 0, 0),
                new Point3d(-100, -245, 0),
                new Point3d(-420, -245, 0),
                new Point3d(-550, -230 * 0.5, 0),
                new Point3d(-550, 0, 0),
                new Point3d(-550, 230 * 0.5, 0),
                new Point3d(-420, 245, 0),
                new Point3d(-100, 245, 0)
            };
            var pl_d1 = new VirtualPolyline3d8pt(pt_d1[0], pt_d1[1], pt_d1[2], pt_d1[3], pt_d1[4], pt_d1[5], pt_d1[6], pt_d1[7]);
            var pl2_d1 = new VirtualPolyline3d8pt(pt_d1[0], pt_d1[1], pt_d1[2], pt_d1[3], pt_d1[4], pt_d1[5], pt_d1[6], pt_d1[7]);
            pl2_d1.Move(new Vector3d(0, 0, 40));
            Solid3d B2n_d1 = VirtualLoftSolid.loftSolidBuild(pl_d1.Gets(), pl2_d1.Gets());
            Point3d[] pt3_d1;
            pt3_d1 = new Point3d[4]
            {
                new Point3d(0, -245, 40),
                new Point3d(0, -245, 20),
                new Point3d(-100, -245, 0),
                new Point3d(-100, -245, 40),
            };
            var pl3_d1 = new VirtualPolyline3d(pt3_d1[0], pt3_d1[1], pt3_d1[2], pt3_d1[3]);
            var pl4_d1 = new VirtualPolyline3d(pt3_d1[0], pt3_d1[1], pt3_d1[2], pt3_d1[3]);
            pl4_d1.Move(new Vector3d(0, 490, 0));
            Solid3d B2n2_d1 = VirtualLoftSolid.loftSolidBuild(pl3_d1.Gets(), pl4_d1.Gets());
            Solid3d hole_d1j = new Solid3d();
            Solid3d HoleB2n_d1 = MainHole_d1;
            Vector3d[] pt_hole_d1 = Mainpt_holed1;
            foreach (var i in pt_hole_d1)
            {
                var newhole = hole_d1j;
                hole_d1j.CopyFrom(HoleB2n_d1);
                hole_d1j.TransformBy(Matrix3d.Displacement(i));
                B2n_d1.BooleanOperation(BooleanOperationType.BoolSubtract, newhole);
                hole_d1j = newhole;
            }
            Solid3d[] B2n_d1i = new Solid3d[2] { B2n_d1, B2n2_d1 };
            Solid3d B2n_d1_all = new Solid3d();
            foreach (var i in B2n_d1i) 
            {
                B2n_d1_all.BooleanOperation(BooleanOperationType.BoolUnite, i);
            }
            B2n_d1_all.TransformBy(Matrix3d.Displacement(new Vector3d(-d1_distance, 0, 0)));
            Solid3d B2n_d1Right = new Solid3d();
            B2n_d1Right.CopyFrom(B2n_d1_all);
            B2n_d1Right.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
            Point3d[] pt_d3;
            pt_d3 = new Point3d[4]
            {
                new Point3d(d1_distance, -245, 20),
                new Point3d(-d1_distance, -245, 20),
                new Point3d(-d1_distance, 245, 20),
                new Point3d(d1_distance, 245, 20)
            };
            var pl_d3 = new VirtualPolyline3d(pt_d3[0], pt_d3[1], pt_d3[2], pt_d3[3]);
            var pl2_d3 = new VirtualPolyline3d(pt_d3[0], pt_d3[1], pt_d3[2], pt_d3[3]);
            pl2_d3.Move(new Vector3d(0, 0, 20));
            Solid3d B2n_d3 = VirtualLoftSolid.loftSolidBuild(pl_d3.Gets(), pl2_d3.Gets());
            //Деталь 2
            Point3d[] pt_d2;
            pt_d2 = new Point3d[4]
            {
                new Point3d(-540 * 0.5, 530 * 0.5, 460),
                new Point3d(540 * 0.5, 530 * 0.5, 460),
                new Point3d(540 * 0.5, -530 * 0.5, 460),
                new Point3d(-540 * 0.5, -530 * 0.5, 460)
            };
            var pl_d2 = new VirtualPolyline3d(pt_d2[0], pt_d2[1], pt_d2[2], pt_d2[3]);
            var pl2_d2 = new VirtualPolyline3d(pt_d2[0], pt_d2[1], pt_d2[2], pt_d2[3]);
            pl2_d2.Move(new Vector3d(0, 0, 40));
            Solid3d B2n_d2 = VirtualLoftSolid.loftSolidBuild(pl_d2.Gets(), pl2_d2.Gets());
            Point3d[] pt2_d2;
            pt2_d2 = new Point3d[4] 
            {
                new Point3d(-540 * 0.5, -530 * 0.5, 460),
                new Point3d((-540 * 0.5) - 100, -530 * 0.5, 460),
                new Point3d((-540 * 0.5) - 100, -530 * 0.5, 480),
                new Point3d(-540 * 0.5, -530 * 0.5, 500)
            };
            var pl3_d2 = new VirtualPolyline3d(pt2_d2[0], pt2_d2[1], pt2_d2[2], pt2_d2[3]);
            var pl4_d2 = new VirtualPolyline3d(pt2_d2[0], pt2_d2[1], pt2_d2[2], pt2_d2[3]);
            pl4_d2.Move(new Vector3d(0, 530, 0));
            Solid3d B2n2_d2 = VirtualLoftSolid.loftSolidBuild(pl3_d2.Gets(), pl4_d2.Gets());
            Solid3d B2n3_d2 = new Solid3d(); ;
            B2n3_d2.CopyFrom(B2n2_d2);
            B2n3_d2.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
            Solid3d[] B2n2_d2i = new Solid3d[2] { B2n2_d2, B2n3_d2 };
            foreach (var j in B2n2_d2i)
            {
                B2n_d2.BooleanOperation(BooleanOperationType.BoolUnite, j);
            }
            //Болтовые отверстия
            Circle d2_hole= new Circle(new Point3d(0,0,0), new Vector3d(0,0,1), 29);
            Region regiond2_hole = VirtualRegion.RegionBuild(d2_hole);
            Solid3d Hole_d2 = new Solid3d();
            Hole_d2.Extrude(regiond2_hole, 1000, 0);
            Vector3d[] vectd2_hole = Mainpt_hole2;
            Solid3d Hole_d2h = new Solid3d();
            foreach (var h in vectd2_hole)
            {
                var newholed2 = Hole_d2h;
                Hole_d2h.CopyFrom(Hole_d2);
                Hole_d2h.TransformBy(Matrix3d.Displacement(h));
                B2n_d2.BooleanOperation(BooleanOperationType.BoolSubtract, newholed2);
                Hole_d2h = newholed2;
            }
            B2n_d2.TransformBy(Matrix3d.Displacement(new Vector3d(0, -102, 0)));
            //Болтовые отверстия
            //Деталь 2
            //деталь 4
            Point3d[] pt_d4;
            pt_d4 = new Point3d[8] 
            {
                new Point3d(-740*0.5, -530*0.5,460),
                new Point3d((-740 * 0.5) - 150, -530 * 0.5, 460),
                new Point3d((-740 * 0.5) - 300, -530 * 0.5, 460),
                new Point3d((-740 * 0.5) - 500, -65, 460),
                new Point3d((-740 * 0.5) - 500, 530 * 0.5, 460),
                new Point3d((-740 * 0.5) - 300, 530 * 0.5, 460),
                new Point3d((-740 * 0.5), 530 * 0.5, 460),
                new Point3d((-740 * 0.5), 0, 460)
            };
            var pl_d4 = new VirtualPolyline3d8pt(pt_d4[0], pt_d4[1], pt_d4[2], pt_d4[3], pt_d4[4], pt_d4[5], pt_d4[6], pt_d4[7]);
            var pl2_d4 = new VirtualPolyline3d8pt(pt_d4[0], pt_d4[1], pt_d4[2], pt_d4[3], pt_d4[4], pt_d4[5], pt_d4[6], pt_d4[7]);
            pl2_d4.Move(new Vector3d(0, 0, 20));
            Solid3d B2n_d4 = VirtualLoftSolid.loftSolidBuild(pl_d4.Gets(), pl2_d4.Gets());
            Solid3d B2n2_d4 = new Solid3d();
            B2n2_d4.CopyFrom(B2n_d4);
            B2n_d4.TransformBy(Matrix3d.Displacement(new Vector3d(0, -102, 0)));
            B2n2_d4.TransformBy(Matrix3d.Displacement(new Vector3d(0, -102, 0)));
            B2n2_d4.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 1, 0), new Point3d(0, 0, 470)));
            //деталь 4
            //Деталь 6 и 7
            Solid3d B2n_d6 = MainB1n_d4;
            Solid3d B2n_d7 = MainB1n_d5;
            Solid3d B2n_d8 = MainB1n_d6;
            int[] id6d7 = new int[5]{ -485, -180, 180, 360, 665};
            var vect_d6d7 = id6d7.Where(e => e != 0).Select(e => new Vector3d(e, 0, 0)).ToList();
            Solid3d B2n_d6i = new Solid3d();
            Solid3d B2n_d7i = new Solid3d();
            //foreach (var p in vect_d6d7)
            //{
            //    {
            //        var B2n_d6ii = B2n_d6i;
            //        B2n_d6ii.CopyFrom(B2n_d6);
            //        B2n_d6ii.TransformBy(Matrix3d.Displacement(p));
            //        B2n_d6.BooleanOperation(BooleanOperationType.BoolUnite, B2n_d6ii);
            //        B2n_d6i = B2n_d6ii;
            //    }
            //    {
            //        var B2n_d7ii = B2n_d7i;
            //        B2n_d7ii.CopyFrom(B2n_d7);
            //        B2n_d7ii.TransformBy(Matrix3d.Displacement(p));
            //        B2n_d7.BooleanOperation(BooleanOperationType.BoolUnite, B2n_d7ii);
            //        B2n_d7i = B2n_d7ii;
            //    }
            //}

            //Деталь 6 и 7


            Solid3d[] B2n_di = new Solid3d[8] { B2n_d1_all, B2n_d1Right, B2n_d3, B2n_d2, B2n_d4, B2n2_d4, B2n_d6, B2n_d7 };
            foreach (var k in B2n_di) { B2n.BooleanOperation(BooleanOperationType.BoolUnite, k); }


            return true;
        }
    }
    //public class Next3Beam : MainBeam
    //{
    //    public override bool BuildBeam(MainWindowContext context, out Solid3d B1n, out Solid3d Hole_d1, out Vector3d[] pt_holed1)
    //    {
    //        BuildMainBeam(context, out _, out var MainHole_d1, out var Mainpt_holed1);
    //        BuildNext(context, out _, out Hole_d1, out pt_holed1, MainHole_d1, Mainpt_holed1, out xxx, out yyy);
    //        return Build3Next(context, out B1n, out Hole_d1, out pt_holed1, xxx, yyy);
    //    }

    //    protected bool Build3Next(MainWindowContext context, out Solid3d B2n, out Solid3d Hole_d1,
    //        out Vector3d[] pt_holed1, Solid3d MainHole_d1, Vector3d[] Mainpt_holed1)
    //    {
    //        Hole_d1 = null;
    //        pt_holed1 = null;
    //        B2n = null;
    //        if (context.Beamlocation == 0) return false;
    //        B2n = new Solid3d();
    //        var d1_distance = 790 * 0.5;
    //        Point3d[] pt_d1;
    //        pt_d1 = new Point3d[8]
    //        {
    //            new Point3d(-100, 0, 0),
    //            new Point3d(-100, -245, 0),
    //            new Point3d(-420, -245, 0),
    //            new Point3d(-550, -230 * 0.5, 0),
    //            new Point3d(-550, 0, 0),
    //            new Point3d(-550, 230 * 0.5, 0),
    //            new Point3d(-420, 245, 0),
    //            new Point3d(-100, 245, 0)
    //        };
    //        var pl_d1 = new VirtualPolyline3d8pt(pt_d1[0], pt_d1[1], pt_d1[2], pt_d1[3], pt_d1[4], pt_d1[5], pt_d1[6], pt_d1[7]);
    //        var pl2_d1 = new VirtualPolyline3d8pt(pt_d1[0], pt_d1[1], pt_d1[2], pt_d1[3], pt_d1[4], pt_d1[5], pt_d1[6], pt_d1[7]);
    //        pl2_d1.Move(new Vector3d(0, 0, 40));
    //        Solid3d B2n_d1 = new Solid3d();
    //        LoftOptionsBuilder loftOptionsBuilder = new LoftOptionsBuilder()
    //        {
    //            Ruled = true
    //        };

    //        B2n_d1.CreateLoftedSolid(new LoftProfile[]
    //            {
    //               new LoftProfile(pl_d1.Gets()),
    //               new LoftProfile(pl2_d1.Gets())
    //            },
    //            new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
    //        Point3d[] pt3_d1;
    //        pt3_d1 = new Point3d[4]
    //        {
    //            new Point3d(0, -245, 40),
    //            new Point3d(0, -245, 20),
    //            new Point3d(-100, -245, 0),
    //            new Point3d(-100, -245, 40),
    //        };
    //        var pl3_d1 = new VirtualPolyline3d(pt3_d1[0], pt3_d1[1], pt3_d1[2], pt3_d1[3]);
    //        var pl4_d1 = new VirtualPolyline3d(pt3_d1[0], pt3_d1[1], pt3_d1[2], pt3_d1[3]);
    //        pl4_d1.Move(new Vector3d(0, 490, 0));
    //        Solid3d B2n2_d1 = new Solid3d();
    //        B2n2_d1.CreateLoftedSolid(new LoftProfile[]
    //            {
    //               new LoftProfile(pl3_d1.Gets()),
    //               new LoftProfile(pl4_d1.Gets())
    //            },
    //            new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
    //        Solid3d hole_d1j = new Solid3d();


    //        Solid3d HoleB2n_d1 = Hole_d1;
    //        Vector3d[] pt_hole_d1 = pt_holed1;
    //        foreach (var i in pt_hole_d1)
    //        {
    //            var newhole = hole_d1j;
    //            hole_d1j.CopyFrom(HoleB2n_d1);
    //            hole_d1j.TransformBy(Matrix3d.Displacement(i));
    //            B2n_d1.BooleanOperation(BooleanOperationType.BoolSubtract, newhole);
    //            hole_d1j = newhole;
    //        }
    //        Solid3d[] B2n_d1i = new Solid3d[2] { B2n_d1, B2n2_d1 };
    //        foreach (var i in B2n_d1i)
    //        {
    //            B2n.BooleanOperation(BooleanOperationType.BoolUnite, i);
    //        }





    //        return true;
    //    }
    //}
    public static class VirtualLoftSolid
    {
        public static Solid3d loftSolidBuild(Polyline3d polyline1, Polyline3d polyline2)
        {
            Solid3d solid = new Solid3d();
            LoftOptionsBuilder loftOptionsBuilder = new LoftOptionsBuilder()
            {
                Ruled = true
            };
            solid.CreateLoftedSolid(new LoftProfile[]
                {
                   new LoftProfile(polyline1),
                   new LoftProfile(polyline2)
                },
                new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
            return solid;
        }
    }
    public static class VirtualRegion 
    {
        public static Region RegionBuild(Circle circle) 
        {
            DBObjectCollection DB_Object = new DBObjectCollection();
            DB_Object.Add(circle);
            DBObjectCollection Region_DB = new DBObjectCollection();
            Region_DB = Region.CreateFromCurves(DB_Object);
            Region region = Region_DB[0] as Region;
            return region;
        }
    }
}
