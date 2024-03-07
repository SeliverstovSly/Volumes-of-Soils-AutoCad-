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
    public class B5n
    {
        public virtual string lebel { get; set; } = "Б5н";
        public virtual double LB { get; set; } = 4010; //фундаментная база балки - расстояние между фундаментами
        public virtual double d1a { get; set; } = 850; // Длина детали 1
        public virtual double d1b { get; set; } = 650; // Ширина детали 1
        public virtual double d1a1 { get; set; } = 550; // Длина ступени детали 1
        public virtual double d1t { get; set; } = 50; //толщина детали 1
        public virtual double d2a { get; set; } = 700; //длина детали 2
        public virtual double d2b { get; set; } = 650; //Ширина детали 2
        public virtual double d2a1 { get; set; } = 600; // Длина ступени детали 2
        public virtual double d2t { get; set; } = 40; // Толщина детали 2
        public virtual double l { get; set; } = -2005;  //Плечо перемещения детали 2
        public virtual double l1 { get; set; } = -1365; //Плечо перемещения детали 3
        public virtual double a { get; set; } = 1880;   //Длина детели 3
        public virtual double d3b { get; set; } = 650; //Ширина детали 3
        public virtual double d3t { get; set; } = 28; //толщина детали 3
        public virtual double a1 { get; set; } = 3210;  //Длина детали 4
        public virtual double d4b { get; set; } = 650; //ширина детали 4
        public virtual double d4t { get; set; } = 28; //толщина детали 4
        public virtual double a2 { get; set; } = 4570;  //Длина детали 5
        public virtual double d5H { get; set; } = 645; //высота детали 5
        public virtual double T { get; set; } = 20;  //Толщина детали 5
        public virtual double d6H { get; set; } = 645; //Высота детали 6 и 7(Б1н-Б3н)
        public virtual double d6t { get; set; } = 12; //Толщина детали 6 и 7(Б1н-Б3н)
        public virtual double d6nb { get; set; } = 310; //Ширина нижней части детали 6
        public virtual double d6vb { get; set; } = 310; //Ширина нижней части детали 6
        public virtual double d7nb { get; set; } = 310; //Ширина нижней части детали 6
        public virtual double d7vb { get; set; } = 310; //Ширина нижней части детали 6
        public virtual double diametr_hole { get; set; } = 58;  //Диаметр отверстий в детали 1
        public virtual double diametrD2_hole { get; set; } = 58;  //Диаметр отверстий в детали 2
        public virtual double H { get; set; } = 685; //отметка детали 1
        public virtual double d6_rebro { get; set; } = 455; //расстояние между крайними рёбрами
        public virtual double Hbeam { get; set; } = 735; //высота балки

        protected virtual void BuildDetal1(out Solid3d d1_solid, double d1a, double d1b, double d1a1, double d1t, double d3t)
        {
            //Деталь 1  
            Point3d[] d1_pt;
            d1_pt = new Point3d[8]
            {
                new Point3d(-d1a * 0.5, -d1b * 0.5, 0),
                new Point3d(-d1a * 0.5, -d1b * 0.5, d3t),
                new Point3d(-d1a1 * 0.5, -d1b * 0.5, d1t),
                new Point3d(0, -d1b * 0.5, d1t),
                new Point3d(d1a1 * 0.5, -d1b * 0.5, d1t),
                new Point3d(d1a * 0.5, -d1b * 0.5, d3t),
                new Point3d(d1a * 0.5, -d1b * 0.5, 0),
                new Point3d(0, -d1b * 0.5, 0)
            };
            VirtualPolyline3d8pt d1_pl1 = new VirtualPolyline3d8pt(d1_pt[0], d1_pt[1], d1_pt[2], d1_pt[3], d1_pt[4], d1_pt[5], d1_pt[6], d1_pt[7]);
            VirtualPolyline3d8pt d1_pl2 = new VirtualPolyline3d8pt(d1_pt[0], d1_pt[1], d1_pt[2], d1_pt[3], d1_pt[4], d1_pt[5], d1_pt[6], d1_pt[7]);
            d1_pl2.Move(new Vector3d(0, d1b, 0));
            d1_solid = VirtualLoftSolid.loftSolidBuild(d1_pl1.Gets(), d1_pl2.Gets());
        }
        protected virtual void BuildDetal1_vectorHole(in Solid3d d1_solid, double diametr_hole, double H, out Solid3d d1_S)
        {            
            //отверстия в детале 1
            Circle d1_hole = new Circle(new Point3d(0,0,0), new Vector3d(0,0,1), diametr_hole * 0.5);
            Region d1_region = VirtualRegion.RegionBuild(d1_hole);
            var hole_solid = new Solid3d();
            hole_solid.Extrude(d1_region, 100, 0);
            Vector3d[] hole_pt;
            hole_pt = new Vector3d[4] 
            {
                new Vector3d(-175, -175, 0),
                new Vector3d(-175, 175, 0),
                new Vector3d(175, 175, 0),
                new Vector3d(175, -175, 0)
            };
            Solid3d hole_solidi = new Solid3d();
            foreach (var i in hole_pt)            
            {
                var newhole = hole_solidi;
                hole_solidi.CopyFrom(hole_solid);
                hole_solidi.TransformBy(Matrix3d.Displacement(i));
                d1_solid.BooleanOperation(BooleanOperationType.BoolSubtract, newhole);
                hole_solidi = newhole;
            }
            d1_solid.TransformBy(Matrix3d.Displacement(new Vector3d(0, 0, H)));
            d1_S = new Solid3d();
            d1_S.CopyFrom(d1_solid);
        }
        protected virtual void BuildDetal2(double l, out Solid3d d2_solid, double d2a, double d2b, double d2a1, double d2t, double d4t)
        {
            //Деталь 2
            //var l = -2005;

            Point3d[] d2_pt;
            d2_pt = new Point3d[8]
            {
                new Point3d(0, -d2b * 0.5, 0),
                new Point3d(-d2a1*0.5, -d2b * 0.5, 0),
                new Point3d(-d2a1*0.5, -d2b * 0.5, d2t),
                new Point3d(0, -d2b * 0.5,d2t),
                new Point3d(d2a1*0.5,-d2b * 0.5,d2t),
                new Point3d(d2a1*0.5+100,-d2b * 0.5,d2t),
                new Point3d(d2a1*0.5+100,-d2b * 0.5,d2t-d4t),
                new Point3d(d2a1*0.5,-d2b * 0.5,0)
            };
            VirtualPolyline3d8pt d2_pl1 = new VirtualPolyline3d8pt(d2_pt[0], d2_pt[1], d2_pt[2], d2_pt[3], d2_pt[4], d2_pt[5], d2_pt[6], d2_pt[7]);
            VirtualPolyline3d8pt d2_pl2 = new VirtualPolyline3d8pt(d2_pt[0], d2_pt[1], d2_pt[2], d2_pt[3], d2_pt[4], d2_pt[5], d2_pt[6], d2_pt[7]);
            d2_pl2.Move(new Vector3d(0, d2b, 0));
            d2_solid = VirtualLoftSolid.loftSolidBuild(d2_pl1.Gets(), d2_pl2.Gets());
        }
        protected virtual void BuildDetal2_vectorHole(in Solid3d d2_solid, double diametrD2_hole, out Solid3d d2_S, out Solid3d d2_S2, double l)
        {
            //Отверстия в детале 2
            Circle d2_hole = new Circle(new Point3d(0,0,0), new Vector3d(0,0,1), diametrD2_hole * 0.5);
            Region d2_region = VirtualRegion.RegionBuild(d2_hole);
            var hole_solid = new Solid3d();
            hole_solid.Extrude(d2_region, 100, 0);
            Vector3d[] hole_pt;
            hole_pt = new Vector3d[4]
            {
                new Vector3d(-175, -175, 0),
                new Vector3d(-175, 175, 0),
                new Vector3d(175, 175, 0),
                new Vector3d(175, -175, 0)
            };
            Solid3d hole_solidi = new Solid3d();
            foreach (var i in hole_pt)
            {
                var newhole = hole_solidi;
                hole_solidi.CopyFrom(hole_solid);
                hole_solidi.TransformBy(Matrix3d.Displacement(i));
                d2_solid.BooleanOperation(BooleanOperationType.BoolSubtract, newhole);
                hole_solidi = newhole;
            }
            d2_solid.TransformBy(Matrix3d.Displacement(new Vector3d(l, 0, 0))); //перемещение на заданную величину l
            d2_S = new Solid3d();
            d2_S.CopyFrom(d2_solid);
            d2_S2 = new Solid3d();
            d2_S2.CopyFrom(d2_S);
            d2_S2.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
        }
        protected virtual void BuildDetal3(double l1, double a, out Solid3d d3_solid, out Solid3d d3_solid2, double H, double d3b, double d3t) 
        {
            //Деталь 3
            Point3d[] d3_pt;
            d3_pt = new Point3d[4] 
            {
                new Point3d(-a*0.5,-d3b * 0.5,0),
                new Point3d(-a*0.5, -d3b * 0.5, d3t),
                new Point3d(a*0.5,-d3b * 0.5,d3t),
                new Point3d(a*0.5,-d3b * 0.5,0)
            };
            VirtualPolyline3d d3_pl1 = new VirtualPolyline3d(d3_pt[0], d3_pt[1], d3_pt[2], d3_pt[3]);
            VirtualPolyline3d d3_pl2 = new VirtualPolyline3d(d3_pt[0], d3_pt[1], d3_pt[2], d3_pt[3]);
            d3_pl2.Move(new Vector3d(0, d3b, 0));
            d3_solid = VirtualLoftSolid.loftSolidBuild(d3_pl1.Gets(), d3_pl2.Gets());
            d3_solid.TransformBy(Matrix3d.Displacement(new Vector3d(0, 0, H)));
            d3_solid.TransformBy(Matrix3d.Displacement(new Vector3d(l1, 0, 0)));
            d3_solid2 = new Solid3d();
            d3_solid2.CopyFrom(d3_solid);
            d3_solid2.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
        }
        protected virtual void BuildDetal4(double a1, out Solid3d d4_solid, double d4b, double d4t, double d2t)
        {
            //Деталь 4
            Point3d[] d4_pt;
            d4_pt = new Point3d[4]
            {
                new Point3d(-a1 * 0.5, -d4b * 0.5, 0),
                new Point3d(-a1 * 0.5, -d4b * 0.5, d4t),
                new Point3d(a1 * 0.5, -d4b * 0.5, d4t),
                new Point3d(a1 * 0.5, -d4b * 0.5, 0)
            };
            VirtualPolyline3d d4_pl1 = new VirtualPolyline3d(d4_pt[0], d4_pt[1], d4_pt[2], d4_pt[3]);
            VirtualPolyline3d d4_pl2 = new VirtualPolyline3d(d4_pt[0], d4_pt[1], d4_pt[2], d4_pt[3]);
            d4_pl2.Move(new Vector3d(0, d4b, 0));
            d4_solid = VirtualLoftSolid.loftSolidBuild(d4_pl1.Gets(), d4_pl2.Gets());
            d4_solid.TransformBy(Matrix3d.Displacement(new Vector3d(0, 0, d2t-d4t)));
            //d4_solid.ToBD();
        }
        protected virtual void BuildDetal5(double a2, double T, out Solid3d d5_solid, double d2t, double d5H)
        {
            //Деталь 5
            Point3d[] d5_pt;
            d5_pt = new Point3d[4]
            {
                new Point3d(-a2 * 0.5, -T * 0.5, d2t),
                new Point3d(-a2 * 0.5, -T * 0.5, d2t + d5H),
                new Point3d(a2 * 0.5, -T * 0.5, d2t + d5H),
                new Point3d(a2 * 0.5, -T * 0.5, d2t)
            };
            VirtualPolyline3d d5_pl1 = new VirtualPolyline3d(d5_pt[0], d5_pt[1], d5_pt[2], d5_pt[3]);
            VirtualPolyline3d d5_pl2 = new VirtualPolyline3d(d5_pt[0], d5_pt[1], d5_pt[2], d5_pt[3]);
            d5_pl2.Move(new Vector3d(0, T, 0));
            d5_solid = VirtualLoftSolid.loftSolidBuild(d5_pl1.Gets(), d5_pl2.Gets());
            //d5_solid.ToBD();
        }
        protected virtual void BuildDetal6(double T, out Solid3d d6_solid, double d2t, double d6H, double d6t, double d6nb, double d6vb, double d7nb, double d7vb)
        {
            //Деталь 6
            var b1 = 25;
            Point3d[] d6_pt;
            d6_pt = new Point3d[8]
            {
                new Point3d(-d6t * 0.5, T * 0.5 + d6nb, d2t),
                new Point3d(-d6t * 0.5, T * 0.5 + d6nb * 0.5, d2t),
                new Point3d(-d6t * 0.5, T * 0.5 + b1, d2t),
                new Point3d(-d6t * 0.5, T * 0.5, d2t + b1),
                new Point3d(-d6t * 0.5, T * 0.5, d2t + (d6H - b1)),
                new Point3d(-d6t * 0.5, T * 0.5 + b1, d2t+d6H),
                new Point3d(-d6t * 0.5, T * 0.5 + d6vb * 0.5, d2t + d6H),
                new Point3d(-d6t * 0.5, T * 0.5 + d6vb, d2t + d6H),
            };
            VirtualPolyline3d8pt d6_pl1 = new VirtualPolyline3d8pt(d6_pt[0], d6_pt[1], d6_pt[2], d6_pt[3], d6_pt[4], d6_pt[5], d6_pt[6], d6_pt[7]);
            VirtualPolyline3d8pt d6_pl2 = new VirtualPolyline3d8pt(d6_pt[0], d6_pt[1], d6_pt[2], d6_pt[3], d6_pt[4], d6_pt[5], d6_pt[6], d6_pt[7]);
            d6_pl2.Move(new Vector3d(d6t, 0, 0));
            d6_solid = VirtualLoftSolid.loftSolidBuild(d6_pl1.Gets(), d6_pl2.Gets());
            Solid3d d6_solid2 = new Solid3d();

            Point3d[] d6_pt2;
            d6_pt2 = new Point3d[8]
            {
                new Point3d(-d6t * 0.5, -T * 0.5 - d7nb, d2t),
                new Point3d(-d6t * 0.5, -T * 0.5 - d7nb * 0.5, d2t),
                new Point3d(-d6t * 0.5, -T * 0.5 - b1, d2t),
                new Point3d(-d6t * 0.5, -T * 0.5, d2t + b1),
                new Point3d(-d6t * 0.5, -T * 0.5, d2t + (d6H - b1)),
                new Point3d(-d6t * 0.5, -T * 0.5 - b1, d2t+d6H),
                new Point3d(-d6t * 0.5, -T * 0.5 - d7vb * 0.5, d2t + d6H),
                new Point3d(-d6t * 0.5, -T * 0.5 - d7vb, d2t + d6H),
            };
            VirtualPolyline3d8pt d6_pl3 = new VirtualPolyline3d8pt(d6_pt2[0], d6_pt2[1], d6_pt2[2], d6_pt2[3], d6_pt2[4], d6_pt2[5], d6_pt2[6], d6_pt2[7]);
            VirtualPolyline3d8pt d6_pl4 = new VirtualPolyline3d8pt(d6_pt2[0], d6_pt2[1], d6_pt2[2], d6_pt2[3], d6_pt2[4], d6_pt2[5], d6_pt2[6], d6_pt2[7]);
            d6_pl4.Move(new Vector3d(d6t, 0, 0));
            d6_solid2 = VirtualLoftSolid.loftSolidBuild(d6_pl3.Gets(), d6_pl4.Gets());
            d6_solid.BooleanOperation(BooleanOperationType.BoolUnite, d6_solid2);
        }
        protected virtual void BuildDetal6_vector(in Solid3d d6_solid, out Solid3d d6_solid2, double d6_rebro)
        {
            //Копирование детали 6 по балке
            Vector3d[] d6_vector;
            d6_vector = new Vector3d[10]
            {
                new Vector3d(-275-728-728-275-275, 0, 0),
                new Vector3d(-275-728-728-275, 0, 0),
                new Vector3d(-275-728-728, 0, 0),
                new Vector3d(-275-728, 0, 0),
                new Vector3d(-275, 0, 0),
                new Vector3d(275, 0, 0),
                new Vector3d(275+728, 0, 0),
                new Vector3d(275+728+728, 0, 0),
                new Vector3d(275+728+728+275, 0, 0),
                new Vector3d(275+728+728+275+275, 0, 0),
            };
            Solid3d[] d6_solidj = new Solid3d[10] 
            {
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d()
            };            
            for (var j=0; j < d6_vector.Length; j++)
            {
                var e = d6_vector[j];
                var newsolid = d6_solidj[j];
                d6_solidj[j].CopyFrom(d6_solid);
                d6_solidj[j].TransformBy(Matrix3d.Displacement(e));
                d6_solidj[j] = newsolid;
            }
            Solid3d[] d6_solidJ = new Solid3d[10] 
            {
                d6_solidj[0],
                d6_solidj[1],
                d6_solidj[2],
                d6_solidj[3],
                d6_solidj[4],
                d6_solidj[5],
                d6_solidj[6],
                d6_solidj[7],
                d6_solidj[8],
                d6_solidj[9],
            };
            foreach (var k in d6_solidJ)
            {
                d6_solid.BooleanOperation(BooleanOperationType.BoolUnite, k);
            }
            d6_solid2 = new Solid3d();
            d6_solid2.CopyFrom(d6_solid);
            
        }
        protected virtual void BeamUnite(in Solid3d d1_S, in Solid3d d2_S, in Solid3d d2_S2, in Solid3d d3_solid, Solid3d d3_solid2, in Solid3d d4_solid, in Solid3d d5_solid, in Solid3d d6_solid2,out Solid3d Beam)
        {
            Solid3d[] Solid_unid = new Solid3d[8] { d1_S, d2_S, d2_S2, d3_solid, d3_solid2, d4_solid, d5_solid, d6_solid2 };
            Beam = new Solid3d();
            foreach (var i in Solid_unid)
            {
                Beam.BooleanOperation(BooleanOperationType.BoolUnite, i);
            }
            Beam.ToBD();
        }
        public virtual Solid3d BuildBeam()
        {
            BuildDetal1(out Solid3d d1_solid, d1a, d1b, d1a1, d1t, d3t);
            BuildDetal1_vectorHole(d1_solid, diametr_hole, H, out Solid3d d1_S);
            BuildDetal2(l, out Solid3d d2_solid, d2a, d2b, d2a1, d2t, d4t);
            BuildDetal2_vectorHole(d2_solid, diametrD2_hole, out Solid3d d2_S, out Solid3d d2_S2, l);
            BuildDetal3(l1, a, out Solid3d d3_solid, out Solid3d d3_solid2, H, d3b, d3t);
            BuildDetal4(a1, out Solid3d d4_solid, d4b, d4t, d2t);
            BuildDetal5(a2, T, out Solid3d d5_solid, d2t, d5H);
            BuildDetal6(T, out Solid3d d6_solid, d2t, d6H,d6t, d6nb, d6vb, d7nb, d7vb);
            BuildDetal6_vector(d6_solid, out Solid3d d6_solid2, d6_rebro);
            BeamUnite(d1_S, d2_S, d2_S2, d3_solid, d3_solid2, d4_solid, d5_solid, d6_solid2,out Solid3d Beam);
            return Beam;
        }
    }
    public class B4n_350:B5n
    {
        public override string lebel { get; set; } = "Б4н-350";
        public override double LB { get; set; } = 2510; //фундаментная база балки - расстояние между фундаментами
        public override double l { get; set; } = -1255;  //Плечо перемещения детали 2
        public override double l1 { get; set; } = -990;  //Плечо перемещения детали 3
        public override double a { get; set; } = 1130;  // длина детали 3
        public override double a1 { get; set; } = 1710; // длина детали 4
        public override double a2 { get; set; } = 3070; // длина детали 5
        public override double T { get; set; } = 25; // толщина детали 5
        protected override void BuildDetal6_vector(in Solid3d d6_solid, out Solid3d d6_solid2, double d6_rebro)
        {
            //Копирование детали 6 по балке
            Vector3d[] d6_vector;
            d6_vector = new Vector3d[8]
            {
                new Vector3d(-275-705-275-275, 0, 0),
                new Vector3d(-275-705-275, 0, 0),
                new Vector3d(-275-705, 0, 0),
                new Vector3d(-275, 0, 0),
                new Vector3d(275, 0, 0),
                new Vector3d(275+705, 0, 0),                
                new Vector3d(275+705+275, 0, 0),
                new Vector3d(275+705+275+275, 0, 0),
            };
            Solid3d[] d6_solidj = new Solid3d[8]
            {
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d()
            };
            for (var j = 0; j < d6_vector.Length; j++)
            {
                var e = d6_vector[j];
                var newsolid = d6_solidj[j];
                d6_solidj[j].CopyFrom(d6_solid);
                d6_solidj[j].TransformBy(Matrix3d.Displacement(e));
                d6_solidj[j] = newsolid;
            }
            Solid3d[] d6_solidJ = new Solid3d[8]
            {
                d6_solidj[0],
                d6_solidj[1],
                d6_solidj[2],
                d6_solidj[3],
                d6_solidj[4],
                d6_solidj[5],
                d6_solidj[6],
                d6_solidj[7]
            };
            foreach (var k in d6_solidJ)
            {
                d6_solid.BooleanOperation(BooleanOperationType.BoolUnite, k);
            }
            d6_solid2 = new Solid3d();
            d6_solid2.CopyFrom(d6_solid);
            //d6_solid.ToBD();
        }
    }
    public class B4n_250:B4n_350
    {
        public override string lebel { get; set; } = "Б4н-250";
        public override double LB { get; set; } = 2510; //фундаментная база балки - расстояние между фундаментами
        public override double H { get; set; } = 685; //отметка детали 1
        protected override void BuildDetal1_vectorHole(in Solid3d d1_solid, double diametr_hole, double H, out Solid3d d1_S)
        {
            //отверстия в детале 1
            Circle d1_hole = new Circle(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), diametr_hole * 0.5);
            Region d1_region = VirtualRegion.RegionBuild(d1_hole);
            var hole_solid = new Solid3d();
            hole_solid.Extrude(d1_region, 100, 0);
            Vector3d[] hole_pt;
            hole_pt = new Vector3d[4]
            {
                new Vector3d(-125, -125, 0),
                new Vector3d(-125, 125, 0),
                new Vector3d(125, 125, 0),
                new Vector3d(125, -125, 0)
            };
            Solid3d hole_solidi = new Solid3d();
            foreach (var i in hole_pt)
            {
                var newhole = hole_solidi;
                hole_solidi.CopyFrom(hole_solid);
                hole_solidi.TransformBy(Matrix3d.Displacement(i));
                d1_solid.BooleanOperation(BooleanOperationType.BoolSubtract, newhole);
                hole_solidi = newhole;
            }
            d1_solid.TransformBy(Matrix3d.Displacement(new Vector3d(0, 0, H)));
            d1_S = new Solid3d();
            d1_S.CopyFrom(d1_solid);
        }
    }
    public class B3n : B5n
    {
        public override string lebel { get; set; } = "Б3н";
        public override double LB { get; set; } = 1700; //фундаментная база балки - расстояние между фундаментами
        public override double d1a { get; set; } = 740; // Длина детали 1
        public override double d1b { get; set; } = 530; // Ширина детали 1
        public override double d1a1 { get; set; } = 540; // Длина ступени детали 1
        public override double d1t { get; set; } = 40; //толщина детали 1
        public override double d2a { get; set; } = 550; //длина детали 2
        public override double d2b { get; set; } = 490; //Ширина детали 2
        public override double d2a1 { get; set; } = 450; // Длина ступени детали 2
        public override double d2t { get; set; } = 40; // Толщина детали 2
        public override double l { get; set; } = -850;  //Плечо перемещения детали 2
        public override double a { get; set; } = 650;   //Длина детели 3
        public override double d3b { get; set; } = 530; //Ширина детали 3
        public override double d3t { get; set; } = 20; //толщина детали 3
        public override double l1 { get; set; } = -695; //Плечо перемещения детали 3
        public override double H { get; set; } = 460; //отметка детали 1,3
        public override double a1 { get; set; } = 1090;  //Длина детали 4
        public override double d4b { get; set; } = 490; //ширина детали 4
        public override double d4t { get; set; } = 20; //толщина детали 4
        public override double a2 { get; set; } = 1695;  //Длина детали 5
        public override double d5H { get; set; } = 420; //высота детали 5
        public override double T { get; set; } = 16;  //Толщина детали 5
        public override double d6H { get; set; } = 420; //Высота детали 6 и 7(Б1н-Б3н)
        public override double d6t { get; set; } = 12; //Толщина детали 6 и 7(Б1н-Б3н)
        public override double d6nb { get; set; } = 235; //Ширина нижней части детали 6
        public override double d6vb { get; set; } = 155; //Ширина нижней части детали 6
        public override double d7nb { get; set; } = 235; //Ширина нижней части детали 6
        public override double d7vb { get; set; } = 360; //Ширина нижней части детали 6
        public override double diametrD2_hole { get; set; } = 44;  //Диаметр отверстий в детали 2
        public virtual double d8_vect { get; set; } = 847.5; //плечо перемещения детали 8
        public override double d6_rebro { get; set; } = 455; //расстояние между крайними рёбрами деталь 6 и 7
        public override double Hbeam { get; set; } = 500; //высота балки
        protected virtual void BuildDetal1_vectorHole2(in Solid3d d1_S, double H, out Solid3d d1_S2, double diametr_hole)
        {
            //отверстия в детале 1
            Circle d1_hole2 = new Circle(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), diametr_hole * 0.5);
            Region d1_region = VirtualRegion.RegionBuild(d1_hole2);
            var hole_solid = new Solid3d();
            hole_solid.Extrude(d1_region, 100, 0);
            Vector3d[] hole2_pt;
            hole2_pt = new Vector3d[4]
            {
                new Vector3d(-177, 0, H),
                new Vector3d(0, 177, H),
                new Vector3d(177, 0, H),
                new Vector3d(0, -177, H)
            };
            Solid3d hole2_solidi = new Solid3d();
            foreach (var i in hole2_pt)
            {
                var newhole = hole2_solidi;
                hole2_solidi.CopyFrom(hole_solid);
                hole2_solidi.TransformBy(Matrix3d.Displacement(i));
                d1_S.BooleanOperation(BooleanOperationType.BoolSubtract, newhole);
                hole2_solidi = newhole;
            }
            d1_S2 = new Solid3d();
            d1_S2.CopyFrom(d1_S);
            d1_S2.TransformBy(Matrix3d.Displacement(new Vector3d(0, -102, 0)));
        }
        protected override void BuildDetal2_vectorHole(in Solid3d d2_solid, double diametrD2_hole, out Solid3d d2_S, out Solid3d d2_S2, double l)
        {
            //Отверстия в детале 2
            Circle d2_hole = new Circle(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), diametrD2_hole * 0.5);
            Region d2_region = VirtualRegion.RegionBuild(d2_hole);
            var hole_solid = new Solid3d();
            hole_solid.Extrude(d2_region, 100, 0);
            Vector3d[] hole_pt;
            d2_solid.TransformBy(Matrix3d.Displacement(new Vector3d(-20, 0, 0)));
            hole_pt = new Vector3d[3]
            {
                new Vector3d(-177, -0, 0),
                new Vector3d(0, 177, 0),
                new Vector3d(0, -177, 0)
            };
            Solid3d hole_solidi = new Solid3d();
            foreach (var i in hole_pt)
            {
                var newhole = hole_solidi;
                hole_solidi.CopyFrom(hole_solid);
                hole_solidi.TransformBy(Matrix3d.Displacement(i));
                d2_solid.BooleanOperation(BooleanOperationType.BoolSubtract, newhole);
                hole_solidi = newhole;
            }
            d2_solid.TransformBy(Matrix3d.Displacement(new Vector3d(l, 0, 0))); //перемещение на заданную величину l
            d2_S = new Solid3d();
            d2_S.CopyFrom(d2_solid);
            d2_S2 = new Solid3d();
            d2_S2.CopyFrom(d2_solid);
            d2_S2.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
        }
        protected virtual void BuildDetal3_vectorE(in Solid3d d3_solid, in Solid3d d3_solid2, out Solid3d d3_S, out Solid3d d3_S2)
        {
            d3_S = new Solid3d();
            d3_S2 = new Solid3d();
            d3_S.CopyFrom(d3_solid);
            d3_S2.CopyFrom(d3_solid2);
            d3_S.TransformBy(Matrix3d.Displacement(new Vector3d(0, -102, 0)));
            d3_S2.TransformBy(Matrix3d.Displacement(new Vector3d(0, -102, 0)));
        }
        protected override void BuildDetal6_vector(in Solid3d d6_solid, out Solid3d d6_solid2, double d6_rebro)
        {
            //Копирование детали 6 по балке
            Vector3d[] d6_vector;
            d6_vector = new Vector3d[6]
            {
                new Vector3d(-d6_rebro-180-180 * 0.5, 0, 0),
                new Vector3d(-180-180 * 0.5, 0, 0),
                new Vector3d(-180 * 0.5, 0, 0),
                new Vector3d(180 * 0.5, 0, 0),
                new Vector3d(180+180 * 0.5, 0, 0),
                new Vector3d(d6_rebro+180+180 * 0.5, 0, 0)
            };
            Solid3d[] d6_solidj = new Solid3d[6]
            {
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d(),
                new Solid3d()
            };
            for (var j = 0; j < d6_vector.Length; j++)
            {
                var e = d6_vector[j];
                var newsolid = d6_solidj[j];
                d6_solidj[j].CopyFrom(d6_solid);
                d6_solidj[j].TransformBy(Matrix3d.Displacement(e));
                d6_solidj[j] = newsolid;
            }
            Solid3d[] d6_solidJ = new Solid3d[6]
            {
                d6_solidj[0],
                d6_solidj[1],
                d6_solidj[2],
                d6_solidj[3],
                d6_solidj[4],
                d6_solidj[5]
            };
            Solid3d d6_s = new Solid3d();
            foreach (var k in d6_solidJ)
            {
                d6_s.BooleanOperation(BooleanOperationType.BoolUnite, k);
            }
            d6_solid2 = new Solid3d();
            d6_solid2.CopyFrom(d6_s);
        }
        protected virtual void BuildDetal8(double d2t, out Solid3d d8_solid, double d8_vect)
        {
            var d8b = 235;
            var d8h = 420;
            var d8t = 12;
            Point3d[] d8_pt = new Point3d[8] 
            {
                new Point3d(0, 0, d2t),
                new Point3d(0, -d8b * 0.5, d2t),
                new Point3d(0, -d8b, d2t),
                new Point3d(d8t, -d8b, d2t),
                new Point3d(d8t, -d8t, d2t),
                new Point3d(d8b, -d8t, d2t),
                new Point3d(d8b, 0, d2t),
                new Point3d(d8b * 0.5, 0, d2t),
            };
            var d8_pl1 = new VirtualPolyline3d8pt(d8_pt[0], d8_pt[1], d8_pt[2], d8_pt[3], d8_pt[4], d8_pt[5], d8_pt[6], d8_pt[7]);
            var d8_pl2 = new VirtualPolyline3d8pt(d8_pt[0], d8_pt[1], d8_pt[2], d8_pt[3], d8_pt[4], d8_pt[5], d8_pt[6], d8_pt[7]);
            d8_pl2.Move(new Vector3d(0, 0, d8h));
            d8_solid = new Solid3d();
            d8_solid = VirtualLoftSolid.loftSolidBuild(d8_pl1.Gets(), d8_pl2.Gets());
            d8_solid.TransformBy(Matrix3d.Rotation(225 * Math.PI / 180, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
            d8_solid.TransformBy(Matrix3d.Displacement(new Vector3d(-d8_vect, 0, 0)));
            Solid3d d8_solid2 = new Solid3d();
            d8_solid2.CopyFrom(d8_solid);
            d8_solid2.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
            d8_solid.BooleanOperation(BooleanOperationType.BoolUnite, d8_solid2);
        }
        protected virtual void BeamUnite(in Solid3d d1_S2, in Solid3d d2_S, in Solid3d d2_S2, in Solid3d d3_S, Solid3d d3_S2, in Solid3d d4_solid, in Solid3d d5_solid, in Solid3d d6_solid2, in Solid3d d8_solid, out Solid3d Beam)
        {
            Solid3d[] Solid_unid = new Solid3d[9] { d1_S2, d2_S, d2_S2, d3_S, d3_S2, d4_solid, d5_solid, d6_solid2, d8_solid };
            Beam = new Solid3d();
            foreach (var i in Solid_unid)
            {
                Beam.BooleanOperation(BooleanOperationType.BoolUnite, i);
            }
            Beam.ToBD();
        }
        public override Solid3d BuildBeam()
        {
            BuildDetal1(out Solid3d d1_solid, d1a, d1b, d1a1, d1t, d3t);
            BuildDetal1_vectorHole(d1_solid, diametr_hole, H, out Solid3d d1_S);
            BuildDetal1_vectorHole2(d1_S, H, out Solid3d d1_S2, diametr_hole);
            BuildDetal2(l, out Solid3d d2_solid, d2a, d2b, d2a1, d2t, d4t);
            BuildDetal2_vectorHole(d2_solid, diametrD2_hole, out Solid3d d2_S, out Solid3d d2_S2, l);
            BuildDetal3(l1, a, out Solid3d d3_solid, out Solid3d d3_solid2, H, d3b, d3t);
            BuildDetal3_vectorE(d3_solid, d3_solid2, out Solid3d d3_S, out Solid3d d3_S2);
            BuildDetal4(a1, out Solid3d d4_solid, d4b, d4t, d2t);
            BuildDetal5(a2, T, out Solid3d d5_solid, d2t, d5H);
            BuildDetal6(T, out Solid3d d6_solid, d2t, d6H, d6t, d6nb, d6vb, d7nb, d7vb);
            BuildDetal6_vector(d6_solid, out Solid3d d6_solid2, d6_rebro);
            BuildDetal8(d2t, out Solid3d d8_solid, d8_vect);
            BeamUnite(d1_S2, d2_S, d2_S2, d3_S, d3_S2, d4_solid, d5_solid, d6_solid2, d8_solid, out Solid3d Beam);
            return Beam;
        }
    }
    public class B2n : B3n
    {
        public override string lebel { get; set; } = "Б2н";
        public override double LB { get; set; } = 1400; //фундаментная база балки - расстояние между фундаментами
        public override double l { get; set; } = -700;  //Плечо перемещения детали 2
        public override double a { get; set; } = 500;   //Длина детели 3
        public override double l1 { get; set; } = -620; //Плечо перемещения детали 3
        public override double a1 { get; set; } = 790;  //Длина детали 4
        public override double a2 { get; set; } = 1395;  //Длина детали 5
        public override double d6_rebro { get; set; } = 305; //расстояние между крайними рёбрами деталь 6 и 7
        public override double d8_vect { get; set; } = 697.5; //плечо перемещения детали 8
    }
    public class B1n : B3n
    {
        public override string lebel { get; set; } = "Б1н";
        public override double LB { get; set; } = 560; //фундаментная база балки - расстояние между фундаментами
        public override double l { get; set; } = -280;  //Плечо перемещения детали 2
        public override double a1 { get; set; } = 70;  //Длина детали 4
        public override double d4b { get; set; } = 490; //ширина детали 4
        public override double d4t { get; set; } = 40; //толщина детали 4
        public override double a2 { get; set; } = 555;  //Длина детали 5
        public override double d6_rebro { get; set; } = 0; //расстояние между крайними рёбрами деталь 6 и 7
        public override double d8_vect { get; set; } = 277.5; //плечо перемещения детали 8
        protected virtual Solid3d BuildDetal1(double d1t, double d1b, out Solid3d d1_solid) 
        {
            d1_solid = new Solid3d();
            var l = 900;
            Point3d[] d1_pt;
            d1_pt = new Point3d[8]
            {
                new Point3d(-l*0.5, d1b*0.5, 0),
                new Point3d(-l*0.5, -65, 0),
                new Point3d(-250, -d1b*0.5, 0),
                new Point3d(0, -d1b*0.5, 0),
                new Point3d(250, -d1b*0.5, 0),
                new Point3d(l*0.5, -65, 0),
                new Point3d(l*0.5, d1b*0.5, 0),
                new Point3d(0, d1b*0.5, 0),
            };
            var d1_pl1 = new VirtualPolyline3d8pt(d1_pt[0], d1_pt[1], d1_pt[2], d1_pt[3], d1_pt[4], d1_pt[5], d1_pt[6], d1_pt[7] );
            var d1_pl2 = new VirtualPolyline3d8pt(d1_pt[0], d1_pt[1], d1_pt[2], d1_pt[3], d1_pt[4], d1_pt[5], d1_pt[6], d1_pt[7]);
            d1_pl2.Move(new Vector3d(0, 0, d1t));
            d1_solid = VirtualLoftSolid.loftSolidBuild(d1_pl1.Gets(), d1_pl2.Gets());
            return d1_solid;
        }
        protected virtual Solid3d BuildDetal2(double d2t, double d2b, out Solid3d d2_solid)
        {
            d2_solid = new Solid3d();
            var l = 490;
            Point3d[] d2_pt;
            d2_pt = new Point3d[8]
            {
                new Point3d(0, d2b*0.5, 0),
                new Point3d(-l*0.5+130, d2b*0.5, 0),
                new Point3d(-l*0.5, d2b*0.5-130, 0),
                new Point3d(-l*0.5, -d2b*0.5+130, 0),
                new Point3d(-l*0.5+130, -d2b*0.5, 0),
                new Point3d(0, -d2b*0.5, 0),
                new Point3d(l*0.5, -d2b*0.5, 0),
                new Point3d(l*0.5, d2b*0.5, 0)
            };
            var d2_pl1 = new VirtualPolyline3d8pt(d2_pt[0], d2_pt[1], d2_pt[2], d2_pt[3], d2_pt[4], d2_pt[5], d2_pt[6], d2_pt[7]);
            var d2_pl2 = new VirtualPolyline3d8pt(d2_pt[0], d2_pt[1], d2_pt[2], d2_pt[3], d2_pt[4], d2_pt[5], d2_pt[6], d2_pt[7]);
            d2_pl2.Move(new Vector3d(0, 0, d2t));
            d2_solid = VirtualLoftSolid.loftSolidBuild(d2_pl1.Gets(), d2_pl2.Gets());
            d2_solid.TransformBy(Matrix3d.Displacement(new Vector3d(20, 0, 0)));
            return d2_solid;
        }
        protected override void BuildDetal6_vector(in Solid3d d6_solid, out Solid3d d6_solid2, double d6_rebro)
        {
            //Копирование детали 6 по балке
            Vector3d[] d6_vector;
            d6_vector = new Vector3d[2]
            {
                new Vector3d(-180 * 0.5, 0, 0),
                new Vector3d(180 * 0.5, 0, 0)
            };
            Solid3d[] d6_solidj = new Solid3d[2]
            {
                new Solid3d(),
                new Solid3d()
            };
            for (var j = 0; j < d6_vector.Length; j++)
            {
                var e = d6_vector[j];
                var newsolid = d6_solidj[j];
                d6_solidj[j].CopyFrom(d6_solid);
                d6_solidj[j].TransformBy(Matrix3d.Displacement(e));
                d6_solidj[j] = newsolid;
            }
            Solid3d[] d6_solidJ = new Solid3d[2]
            {
                d6_solidj[0],
                d6_solidj[1]
            };
            Solid3d d6_s = new Solid3d();
            foreach (var k in d6_solidJ)
            {
                d6_s.BooleanOperation(BooleanOperationType.BoolUnite, k);
            }
            d6_solid2 = new Solid3d();
            d6_solid2.CopyFrom(d6_s);
        }
        protected virtual void BeamUnite(Solid3d d1_S2, Solid3d d2_S, Solid3d d2_S2, Solid3d d4_solid,  Solid3d d5_solid, Solid3d d6_solid2, Solid3d d8_solid, out Solid3d Beam)
        {
            Solid3d[] Solid_unid = new Solid3d[7] { d1_S2, d2_S, d2_S2, d4_solid, d5_solid, d6_solid2, d8_solid };
            Beam = new Solid3d();
            foreach (var i in Solid_unid)
            {
                Beam.BooleanOperation(BooleanOperationType.BoolUnite, i);
            }
            Beam.ToBD();
        }
        public override Solid3d BuildBeam()
        {
            BuildDetal1(d1t, d1b, out Solid3d d1_solid);
            BuildDetal1_vectorHole( d1_solid, diametr_hole, H, out Solid3d d1_S);
            BuildDetal1_vectorHole2(d1_S, H, out Solid3d d1_S2, diametr_hole);
            BuildDetal2(d2t, d2b, out Solid3d d2_solid);
            BuildDetal2_vectorHole(d2_solid, diametrD2_hole, out Solid3d d2_S, out Solid3d d2_S2, l);
            BuildDetal4(a1, out Solid3d d4_solid, d4b, d4t, d2t);
            BuildDetal5(a2, T, out Solid3d d5_solid, d2t, d5H);
            BuildDetal6(T, out Solid3d d6_solid, d2t, d6H, d6t, d6nb, d6vb, d7nb, d7vb);
            BuildDetal6_vector(d6_solid, out Solid3d d6_solid2, d6_rebro);
            BuildDetal8(d2t, out Solid3d d8_solid, d8_vect);
            BeamUnite(d1_S2, d2_S, d2_S2, d4_solid, d5_solid, d6_solid2, d8_solid, out Solid3d Beam);
            return Beam;
        }
    }
}
