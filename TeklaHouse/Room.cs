using System;
using System.Collections;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace TeklaHouse
{
    class Room
    {
        private static readonly int minRoomLenght = 2000;
        private static readonly int gridSize = 100;
        private Point A;
        private Point B => new Point(A.X + lenght, A.Y, A.Z);
        private Point C => new Point(A.X + lenght, A.Y + width, A.Z);
        private Point D => new Point(A.X, A.Y + width, A.Z);
        private Point A1 => new Point(A.X, A.Y, A.Z + height);
        private Point B1 => new Point(A.X + lenght, A.Y, A.Z + height);
        private Point C1 => new Point(A.X + lenght, A.Y + width, A.Z + height);
        private Point D1 => new Point(A.X, A.Y + width, A.Z + height);

        private double lenght { get; set; }
        private double width { get; set; }
        private double height { get; set; }

        public double Area => lenght * width;

        public Room(Point startPoint, double lenght, double width, double height, double groundHeight)
        {
            A = new Point(startPoint.X, startPoint.Y, startPoint.Z + groundHeight);
            this.lenght = lenght;
            this.width = width;
            this.height = height;
        }

        public Room SplitRoom()
        {
            if (lenght > width)
            {
                var randomSplit = RandomExtended.getRandom(minRoomLenght,
                    Convert.ToInt32(lenght) - minRoomLenght) / gridSize * gridSize;
                Room newRoom = new Room(new Point(A.X + randomSplit, A.Y, A.Z),
                    lenght - randomSplit, width, height, 0);
                lenght = randomSplit;
                return newRoom;
            }
            else
            {
                var randomSplit = RandomExtended.getRandom(minRoomLenght,
                    Convert.ToInt32(width) - minRoomLenght) / gridSize * gridSize;
                Room newRoom = new Room(new Point(A.X, A.Y + randomSplit, A.Z),
                    lenght, width - randomSplit, height, 0);
                width = randomSplit;
                return newRoom;
            }
        }

        public void InsertWalls()
        {
            ArrayList basementPoints = new ArrayList
            {
                new Point(A.X - 50, A.Y - 50, A.Z - 50),
                new Point(B.X + 50, B.Y - 50, B.Z - 50),
                new Point(C.X + 50, C.Y + 50, C.Z - 50),
                new Point(D.X - 50, D.Y + 50, D.Z - 50),
            };
            InsertElements.InsertPlate(basementPoints, "PL100", "A36");
            InsertElements.InsertPlate(A, B, B1, A1, "PL100", "A36");
            InsertElements.InsertPlate(B, C, C1, B1, "PL100", "A36");
            InsertElements.InsertPlate(D, C, C1, D1, "PL100", "A36");
            InsertElements.InsertPlate(A, D, D1, A1, "PL100", "A36");
            InsertElements.InsertBeam(new Point(A.X+150,A.Y+150,A.Z), new Point(A1.X+150, A1.Y+150, A1.Z),
                "L203X203X28.6", "350W", Position.RotationEnum.BELOW);
            InsertElements.InsertBeam(new Point(B.X-150, B.Y+150, B.Z), new Point(B1.X-150, B1.Y+150, B1.Z),
                "L203X203X28.6", "350W", Position.RotationEnum.FRONT);
            InsertElements.InsertBeam(new Point(C.X-150, C.Y-150, C.Z), new Point(C1.X-150, C1.Y-150, C1.Z),
                "L203X203X28.6", "350W", Position.RotationEnum.TOP);
            InsertElements.InsertBeam(new Point(D.X+150, D.Y-150, D.Z), new Point(D1.X+150, D1.Y-150, D1.Z),
            "L203X203X28.6", "350W", Position.RotationEnum.BACK);
        }
    }
}
