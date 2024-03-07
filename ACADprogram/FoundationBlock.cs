using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;

namespace ACADprogram
{
    public class SolidInfo
    {
        public Solid3d Solid { get; set; } = new Solid3d();
        public double Length { get; set; }
        public double FullLength { get; set; }
        public double AngleRotation { get; set; }
        public double Eccentricity { get; set; }
    }

    public abstract class FoundationBlock
    {
        public virtual SolidInfo Blockcreation(MainWindowContext Context, FoundationType SelectedType, Vector3d vector, B5n SelectedBeam, B5n SelectedMainBeam) =>
            new SolidInfo();
    }
    public class OneFoundation : FoundationBlock
    {
        public virtual double lebel { get; set; } = 1;
        public virtual SolidInfo BuildOneFoundation(MainWindowContext Context, FoundationType SelectedType, Vector3d vector, out Solid3d OneFound)
        {
            return new SolidInfo()
            {
                Solid = OneFound = FoundationBuild.Build(SelectedType, vector),
            };
        }
        public override SolidInfo Blockcreation(MainWindowContext Context, FoundationType SelectedType, Vector3d vector, B5n SelectedBeam, B5n SelectedMainBeam)
        {
            BuildOneFoundation(Context, SelectedType, vector, out Solid3d OneFound);
            return new SolidInfo() 
            {
                Solid = OneFound,
                Length = Foundation.AllFoundations[SelectedType].a,
                FullLength = Foundation.AllFoundations[SelectedType].OverallSize,
                AngleRotation = Foundation.AllFoundations[SelectedType].Ugol_povorota,
                Eccentricity = Foundation.AllFoundations[SelectedType].e
            };
        }
    }
    public class TwoFoundation : OneFoundation
    {
        public override double lebel { get; set; } = 2;
        public virtual SolidInfo BuildTwoFoundation(MainWindowContext Context, FoundationType SelectedType, Vector3d vector, B5n SelectedBeam, out Solid3d TwoFound, Solid3d OneFound)
        {
            OneFound.TransformBy(Matrix3d.Displacement(new Vector3d(-SelectedBeam.LB * 0.5, 0, 0)));
            OneFound.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
            Solid3d Beamone = new Solid3d();
            Beamone = SelectedBeam.BuildBeam();
            Beamone.TransformBy(Matrix3d.Displacement(new Vector3d(0, 0, Foundation.AllFoundations[SelectedType].hf)));
            TwoFound = new Solid3d();
            TwoFound.CopyFrom(OneFound);
            TwoFound.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
            Solid3d[] TwoSi = new Solid3d[2] { OneFound, Beamone };
            foreach (var s in TwoSi)
            {
                TwoFound.BooleanOperation(BooleanOperationType.BoolUnite, s);
            }
            TwoFound.TransformBy(Matrix3d.Displacement(new Vector3d(0, 102, 0)));
            TwoFound.TransformBy(Matrix3d.Rotation(90 * Math.PI / 180, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
            return new SolidInfo()
            {
                Solid = TwoFound
            };
        }
        public override SolidInfo Blockcreation(MainWindowContext Context, FoundationType SelectedType, Vector3d vector, B5n SelectedBeam, B5n SelectedMainBeam)
        {
            BuildOneFoundation(Context, SelectedType, vector, out Solid3d OneFound);
            BuildTwoFoundation(Context, SelectedType, vector, SelectedBeam, out Solid3d TwoFound, OneFound);
            return new SolidInfo()
            {
                Solid = TwoFound,
                Length = Foundation.AllFoundations[SelectedType].a + SelectedBeam.LB + 2 * Foundation.AllFoundations[SelectedType].e,
                FullLength = Foundation.AllFoundations[SelectedType].OverallSize,
                AngleRotation = Foundation.AllFoundations[SelectedType].Ugol_povorota,
                Eccentricity = 102
            };
        }
    }
    public class FourFoundation : TwoFoundation
    {
        public override double lebel { get; set; } = 4;
        protected virtual SolidInfo BuildFourFoundation(MainWindowContext Context, FoundationType SelectedType, Vector3d vector, B5n SelectedBeam, B5n SelectedMainBeam, out Solid3d FourFound, Solid3d TwoFound)
        {
            TwoFound.TransformBy(Matrix3d.Displacement(new Vector3d(-SelectedMainBeam.LB * 0.5, 0, 0)));
            Solid3d TwoFound2 = new Solid3d();
            TwoFound2.CopyFrom(TwoFound);
            TwoFound2.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
            Solid3d MainBeam = new Solid3d();
            MainBeam = SelectedMainBeam.BuildBeam();
            MainBeam.TransformBy(Matrix3d.Displacement(new Vector3d(0,0, (Foundation.AllFoundations[SelectedType].hf) + SelectedBeam.Hbeam)));
            Solid3d[] solid3Ds = new Solid3d[3] { MainBeam, TwoFound, TwoFound2 };
            FourFound = new Solid3d();
            foreach (var i in solid3Ds) 
            {
                FourFound.BooleanOperation(BooleanOperationType.BoolUnite, i);  
            }
            return new SolidInfo()
            {
                Solid = FourFound
            };
        }
        public override SolidInfo Blockcreation(MainWindowContext Context, FoundationType SelectedType, Vector3d vector, B5n SelectedBeam, B5n SelectedMainBeam)
        {
            BuildOneFoundation(Context, SelectedType, vector, out Solid3d OneFound);
            BuildTwoFoundation( Context,  SelectedType,  vector,  SelectedBeam, out Solid3d TwoFound,  OneFound);
            BuildFourFoundation(Context, SelectedType, vector, SelectedBeam, SelectedMainBeam, out Solid3d FourFound, TwoFound);
            return new SolidInfo()
            {
                Solid = FourFound,
                Length = Foundation.AllFoundations[SelectedType].a + SelectedMainBeam.LB,
                FullLength = Foundation.AllFoundations[SelectedType].OverallSize + Foundation.AllFoundations[SelectedType].e * 2 + SelectedBeam.LB,
                AngleRotation = 0,
                Eccentricity = 0
            };
        }
    }
}
