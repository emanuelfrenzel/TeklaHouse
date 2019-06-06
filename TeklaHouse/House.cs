using System;
using System.Collections;
using System.Collections.Generic;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using ModelObjectSelector = Tekla.Structures.Model.UI.ModelObjectSelector;

namespace TeklaHouse
{
    class House
    {
        private static readonly int basementHeight = 100;
        private static readonly int plateOffset = 50;
        private static readonly int windowsDistance = 500;
        private readonly int numberOfRooms;
        private readonly int numberOfFloors;
        private readonly double lenght;
        private readonly double width;
        private readonly double floorHeight;
        private readonly double roofHeight;
        private readonly Point startPoint;
        private readonly List<Room> rooms = new List<Room>();
        private Point A => new Point(startPoint.X - plateOffset, startPoint.Y - plateOffset, startPoint.Z + basementHeight / 2);
        private Point B => new Point(startPoint.X + lenght + plateOffset, startPoint.Y - plateOffset, startPoint.Z + basementHeight / 2);
        private Point C => new Point(startPoint.X + lenght + plateOffset, startPoint.Y + width + plateOffset, startPoint.Z + basementHeight / 2);
        private Point D => new Point(startPoint.X - plateOffset, startPoint.Y + width + plateOffset, startPoint.Z + basementHeight / 2);
        private Point A1 => new Point(startPoint.X - plateOffset, startPoint.Y - plateOffset,
            startPoint.Z + floorHeight * numberOfFloors + basementHeight + basementHeight / 2);
        private Point B1 => new Point(startPoint.X + lenght + plateOffset, startPoint.Y - plateOffset,
            startPoint.Z + floorHeight * numberOfFloors + basementHeight + basementHeight / 2);
        private Point C1 => new Point(startPoint.X + lenght + plateOffset, startPoint.Y + width + plateOffset,
            startPoint.Z + floorHeight * numberOfFloors + basementHeight + basementHeight / 2);
        private Point D1 => new Point(startPoint.X - plateOffset, startPoint.Y + width + plateOffset,
            startPoint.Z + floorHeight * numberOfFloors + basementHeight + basementHeight / 2);
        private Point midPoint => new Point(startPoint.X + lenght / 2, startPoint.Y + width / 2,
            startPoint.Z + basementHeight + floorHeight * numberOfFloors + roofHeight + basementHeight / 2);

        public House(int numberOfRooms, int numberOfFloors,
            double lenght, double width, double floorHeight, double roofHeight, Point startPoint)
        {
            this.numberOfRooms = numberOfRooms;
            this.numberOfFloors = numberOfFloors;
            this.lenght = lenght;
            this.width = width;
            this.floorHeight = floorHeight;
            this.roofHeight = roofHeight;
            this.startPoint = startPoint;
        }

        public void Generate()
        {
            for (int i = 0; i < numberOfFloors; ++i)
            {
                var point = new Point(startPoint.X, startPoint.Y, startPoint.Z + i * floorHeight);
                rooms.Add(new Room(point, lenght, width, floorHeight, basementHeight));
            }

            while (rooms.Count < numberOfRooms)
            {
                rooms.Add(rooms[rooms.Count - 1].SplitRoom());
                rooms.Sort((x, y) => x.Area.CompareTo(y.Area));
            }
            InsertElements.InsertPlate(A1, B1, C1, D1, "PL100", "A36");
            if (roofHeight > 0)
            {
                InsertElements.InsertPlate(A1, B1, midPoint, "PL100", "A36");
                InsertElements.InsertPlate(B1, C1, midPoint, "PL100", "A36");
                InsertElements.InsertPlate(C1, D1, midPoint, "PL100", "A36");
                InsertElements.InsertPlate(D1, A1, midPoint, "PL100", "A36");
                InsertElements.InsertBeam(A1, midPoint, "L203X203X28.6", "350W");
                InsertElements.InsertBeam(B1, midPoint, "L203X203X28.6", "350W", Position.RotationEnum.BACK);
                InsertElements.InsertBeam(C1, midPoint, "L203X203X28.6", "350W");
                InsertElements.InsertBeam(D1, midPoint, "L203X203X28.6", "350W", Position.RotationEnum.BACK);
            }
            foreach (Room room in rooms)
            {
                room.InsertWalls();
            }
        }

        public static bool InsertDoor(double doorWidth, double doorHeight)
        {
            var objects = new ModelObjectSelector().GetSelectedObjects();
            while (objects.MoveNext())
            {
                var obj = objects.Current;
                try
                {
                    var midWallPoint = obj.GetCoordinateSystem().Origin;
                    var wallLenght = obj.GetCoordinateSystem().AxisX;
                    midWallPoint.X += wallLenght.X / 2;
                    midWallPoint.Y += wallLenght.Y / 2;
                    ArrayList list;
                    if (wallLenght.X == 0)
                    {
                        list = new ArrayList
                        {
                            new Point(midWallPoint.X, midWallPoint.Y - doorWidth /2, midWallPoint.Z),
                            new Point(midWallPoint.X, midWallPoint.Y + doorWidth / 2, midWallPoint.Z),
                            new Point(midWallPoint.X, midWallPoint.Y + doorWidth / 2, midWallPoint.Z + doorHeight),
                            new Point(midWallPoint.X, midWallPoint.Y - doorWidth / 2, midWallPoint.Z + doorHeight)
                        };
                    }
                    else
                    {
                        list = new ArrayList
                        {
                            new Point(midWallPoint.X - doorWidth /2, midWallPoint.Y, midWallPoint.Z),
                            new Point(midWallPoint.X + doorWidth / 2, midWallPoint.Y, midWallPoint.Z),
                            new Point(midWallPoint.X + doorWidth / 2, midWallPoint.Y, midWallPoint.Z + doorHeight),
                            new Point(midWallPoint.X - doorWidth / 2, midWallPoint.Y, midWallPoint.Z + doorHeight)
                        };
                    }
                    InsertElements.InsertCutPlate(obj as ContourPlate, list, "PL100", "A36");
                }
                catch
                {
                    return true;
                }
            }
            return false;
        }

        public static bool InsertWindows(double lenght, double height, double heightFloor)
        {
            var objects = new ModelObjectSelector().GetSelectedObjects();
            while (objects.MoveNext())
            {
                var obj = objects.Current;
                try
                {
                    var midWallPoint = obj.GetCoordinateSystem().Origin;
                    var wallLenght = obj.GetCoordinateSystem().AxisX;
                    if (wallLenght.X == 0)
                    {
                        int numberOfWindows = Convert.ToInt32(Math.Floor(wallLenght.Y / (lenght + windowsDistance)));
                        midWallPoint.Y += wallLenght.Y / numberOfWindows / 2;
                        for (int i = 0; i < numberOfWindows; ++i)
                        {
                            ArrayList list = new ArrayList
                            {
                                new Point(midWallPoint.X, midWallPoint.Y - lenght /2, midWallPoint.Z + heightFloor),
                                new Point(midWallPoint.X, midWallPoint.Y + lenght / 2, midWallPoint.Z + heightFloor),
                                new Point(midWallPoint.X, midWallPoint.Y + lenght / 2, midWallPoint.Z + heightFloor + height),
                                new Point(midWallPoint.X, midWallPoint.Y - lenght / 2, midWallPoint.Z + heightFloor + height)
                            };
                            InsertElements.InsertCutPlate(obj as ContourPlate, list, "PL100", "A36");
                            midWallPoint.Y += wallLenght.Y / numberOfWindows;
                        }
                    }
                    else
                    {
                        int numberOfWindows = Convert.ToInt32(Math.Floor(wallLenght.X / (lenght + windowsDistance)));
                        midWallPoint.X += wallLenght.X / numberOfWindows / 2;
                        for (int i = 0; i < numberOfWindows; ++i)
                        {
                            ArrayList list = new ArrayList
                            {
                                new Point(midWallPoint.X - lenght /2, midWallPoint.Y, midWallPoint.Z + heightFloor),
                                new Point(midWallPoint.X + lenght / 2, midWallPoint.Y, midWallPoint.Z + heightFloor),
                                new Point(midWallPoint.X + lenght / 2, midWallPoint.Y, midWallPoint.Z + heightFloor + height),
                                new Point(midWallPoint.X - lenght / 2, midWallPoint.Y, midWallPoint.Z + heightFloor + height)
                            };
                            InsertElements.InsertCutPlate(obj as ContourPlate, list, "PL100", "A36");
                            midWallPoint.X += wallLenght.X / numberOfWindows;
                        }
                    }
                }
                catch
                {
                    return true;
                }
            }
            return false;
        }
    }
}
