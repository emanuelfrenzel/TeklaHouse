using System.Collections;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace TeklaHouse
{
    public static class InsertElements
    {
        public static Beam InsertBeam(Point startPoint, Point endPoint,
            string profileString, string materialString, Position.RotationEnum rotation = Position.RotationEnum.TOP)
        {
            var beam = new Beam();
            beam.StartPoint = startPoint;
            beam.EndPoint = endPoint;
            beam.Profile.ProfileString = profileString;
            beam.Material.MaterialString = materialString;
            beam.Position.Rotation = rotation;
            beam.Insert();
            return beam;
        }

        public static ContourPlate InsertPlate(ArrayList points,
            string profileString, string materialString, Chamfer chamfer = null)
        {
            var cp = new ContourPlate();
            foreach (Point point in points)
            {
                cp.AddContourPoint(point, chamfer);
            }
            cp.Profile.ProfileString = profileString;
            cp.Material.MaterialString = materialString;
            cp.Insert();
            return cp;
        }

        public static ContourPlate InsertPlate(Point A, Point B, Point C, Point D,
            string profileString, string materialString, Chamfer chamfer = null)
        {
            var cp = new ContourPlate();
            cp.AddContourPoint(A, chamfer);
            cp.AddContourPoint(B, chamfer);
            cp.AddContourPoint(C, chamfer);
            cp.AddContourPoint(D, chamfer);
            cp.Profile.ProfileString = profileString;
            cp.Material.MaterialString = materialString;
            cp.Insert();
            return cp;
        }

        public static ContourPlate InsertPlate(Point A, Point B, Point C,
            string profileString, string materialString, Chamfer chamfer = null)
        {
            var cp = new ContourPlate();
            cp.AddContourPoint(A, chamfer);
            cp.AddContourPoint(B, chamfer);
            cp.AddContourPoint(C, chamfer);
            cp.Profile.ProfileString = profileString;
            cp.Material.MaterialString = materialString;
            cp.Insert();
            return cp;
        }

        public static ContourPlate InsertCutPlate(ContourPlate cuttedPlate, ArrayList points,
            string profileString, string materialString, Chamfer chamfer = null)
        {
            var cp = new ContourPlate();
            foreach (Point point in points)
            {
                cp.AddContourPoint(point, chamfer);
            }
            cp.Profile.ProfileString = profileString;
            cp.Material.MaterialString = materialString;
            cp.Class = BooleanPart.BooleanOperativeClassName;
            cp.Insert();
            var bp = new BooleanPart { Type = BooleanPart.BooleanTypeEnum.BOOLEAN_CUT, Father = cuttedPlate };
            bp.SetOperativePart(cp);
            bp.Insert();
            cp.Delete();
            return cp;
        }
    }
}
