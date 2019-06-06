using System.Collections.Generic;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace TeklaHouse
{
    public static class ExtendedContourPlate
    {
        public static void Insert(this ContourPlate cp, List<Point> contourPoints, 
            string profileString, string materialString, Chamfer chamfer = null)
        {
            foreach (var point in contourPoints)
            {
                cp.AddContourPoint(point, chamfer);
            }
            cp.Profile.ProfileString = profileString;
            cp.Material.MaterialString = materialString;
            cp.Insert();
        }

        public static void AddContourPoint(this ContourPlate cp, Point p, Chamfer chamfer = null)
        {
            var cPoint = new ContourPoint(p, chamfer);
            cp.AddContourPoint(cPoint);
        }
    }
}
