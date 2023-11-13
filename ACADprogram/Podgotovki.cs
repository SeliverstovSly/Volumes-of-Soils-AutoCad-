using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace ACADprogram
{

    public class Builder
    {
        
        public static bool TryBuildSheben(FoundationType FoundationTypei, Vector3d vector_ponizh, MainWindowContext Context, out Solid3d solid) 
        {
            solid = null;
            if (Context.HShebenPodgotovki == 0) return false;
            solid = new Solid3d();
            //свесы подготовки
            var svesupod = 300;
            var svesupodBeton = 300;
            if (Context.HBetonPodgotovki==0)
            {
                svesupodBeton = 0;
            }
            var Lp1y = Foundation.AllFoundations[FoundationTypei].b + svesupod + svesupodBeton;
            if (FoundationTypei == FoundationType.FS1n_A || FoundationTypei == FoundationType.FSP1n_A)
            {
               Lp1y = 4200 + svesupod + svesupodBeton;
            }
            else if (FoundationTypei == FoundationType.FS2n_A || FoundationTypei == FoundationType.FSP2n_A)
            {
                Lp1y = 5200 + svesupod + svesupodBeton;
            }
            else
            {
                Lp1y = Foundation.AllFoundations[FoundationTypei].b + svesupod + svesupodBeton;
            }
            var Lp1x = Foundation.AllFoundations[FoundationTypei].a + svesupod + svesupodBeton;
            var hpodg = Context.HShebenPodgotovki;
            var ip = 1;
            if (hpodg <= 100)
            {
                ip = 0;
            }
            var Lp2x = Lp1x + 2 * ip * Context.HShebenPodgotovki;
            var Lp2y = Lp1y + 2 * ip * Context.HShebenPodgotovki;
            Point3d[] pt_pod1;
            pt_pod1 = new Point3d[4]
            {
                new Point3d(-Lp1x / 2, Lp1y / 2, hpodg),
                new Point3d(Lp1x / 2, Lp1y / 2, hpodg),
                new Point3d(Lp1x / 2, -Lp1y / 2, hpodg),
                new Point3d(-Lp1x / 2, -Lp1y / 2, hpodg)
            };
            var pl_pod1 = new VirtualPolyline3d(pt_pod1[0], pt_pod1[1], pt_pod1[2], pt_pod1[3]);
            Point3d[] pt_pod2;
            pt_pod2 = new Point3d[4]
            {
                new Point3d(-Lp2x / 2, Lp2y / 2, 0),
                new Point3d(Lp2x / 2, Lp2y / 2, 0),
                new Point3d(Lp2x / 2, -Lp2y / 2, 0),
                new Point3d(-Lp2x / 2, -Lp2y / 2, 0)
            };
            var pl_pod2 = new VirtualPolyline3d(pt_pod2[0], pt_pod2[1], pt_pod2[2], pt_pod2[3]);
            //Построение 3Dтела щебёночной подготовки. Тип.
            LoftOptionsBuilder loftOptionsBuilder = new LoftOptionsBuilder()
            {
                Ruled = true
            };
            solid.CreateLoftedSolid(new LoftProfile[]
                {
                        new LoftProfile(pl_pod1.Gets()),
                        new LoftProfile(pl_pod2.Gets())
                },
                new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
            solid.TransformBy(Matrix3d.Displacement(new Vector3d(-Foundation.AllFoundations[FoundationTypei].e, 0, 0)));
            solid.TransformBy(Matrix3d.Displacement(vector_ponizh));
            return true;
        }
   
        public static bool TryBuildBeton(FoundationType FoundationTypei, Vector3d vector_ponizh, MainWindowContext Context, out Solid3d solid) 
        {
            solid = null;
            if (Context.HBetonPodgotovki == 0) return false;
            solid = new Solid3d();
            var svesupodBeton = 300;
            var Lp1y = Foundation.AllFoundations[FoundationTypei].b + svesupodBeton;
            if (FoundationTypei == FoundationType.FS1n_A || FoundationTypei == FoundationType.FSP1n_A)
            {
                Lp1y = 4200 + svesupodBeton;
            }
            else if (FoundationTypei == FoundationType.FS2n_A || FoundationTypei == FoundationType.FSP2n_A)
            {
                Lp1y = 5200 + svesupodBeton;
            }
            else
            {
                Lp1y = Foundation.AllFoundations[FoundationTypei].b + svesupodBeton;
            }
            var Lp1x = Foundation.AllFoundations[FoundationTypei].a + svesupodBeton;
            var hpodg = Context.HBetonPodgotovki;
            var ip = 0;
            var Lp2x = Lp1x + 2 * ip * Context.HBetonPodgotovki;
            var Lp2y = Lp1y + 2 * ip * Context.HBetonPodgotovki;
            Point3d[] pt_pod1;
            pt_pod1 = new Point3d[4]
            {
                new Point3d(-Lp1x / 2, Lp1y / 2, hpodg),
                new Point3d(Lp1x / 2, Lp1y / 2, hpodg),
                new Point3d(Lp1x / 2, -Lp1y / 2, hpodg),
                new Point3d(-Lp1x / 2, -Lp1y / 2, hpodg)
            };
            var pl_pod1 = new VirtualPolyline3d(pt_pod1[0], pt_pod1[1], pt_pod1[2], pt_pod1[3]);
            Point3d[] pt_pod2;
            pt_pod2 = new Point3d[4]
            {
                new Point3d(-Lp2x / 2, Lp2y / 2, 0),
                new Point3d(Lp2x / 2, Lp2y / 2, 0),
                new Point3d(Lp2x / 2, -Lp2y / 2, 0),
                new Point3d(-Lp2x / 2, -Lp2y / 2, 0)
            };
            var pl_pod2 = new VirtualPolyline3d(pt_pod2[0], pt_pod2[1], pt_pod2[2], pt_pod2[3]);
            //Построение 3Dтела бетонной подготовки
            LoftOptionsBuilder loftOptionsBuilder = new LoftOptionsBuilder()
            {
                Ruled = true
            };
            
            solid.CreateLoftedSolid(new LoftProfile[]
                {
                        new LoftProfile(pl_pod1.Gets()),
                        new LoftProfile(pl_pod2.Gets())
                },
                new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
            solid.TransformBy(Matrix3d.Displacement(new Vector3d(-Foundation.AllFoundations[FoundationTypei].e, 0, 0)));
            solid.TransformBy(Matrix3d.Displacement(vector_ponizh));
            return true;
        }
    }
}
