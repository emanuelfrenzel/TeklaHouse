using System.Collections;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;

namespace TeklaHouse
{
    class ExtendedPicker
    {
        private static readonly Picker picker = new Picker();

        public static Point PickPoint()
        {
            var point = picker.PickPoints(Picker.PickPointEnum.PICK_ONE_POINT,
                "Pick position");
            return point[0] as Point;
        }

        public static ArrayList PickPolygon()
        {
            var points = picker.PickPoints(Picker.PickPointEnum.PICK_POLYGON,
                "Pick the corner points of the plate. To finish, press the middle mouse button.");
            return points;
        }
    }
}
