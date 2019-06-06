using System;
using System.Windows;
using Tekla.Structures.Model;

namespace TeklaHouse
{
    public partial class MainWindow : Window
    {
        private static readonly Model model = new Model();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var startPoint = ExtendedPicker.PickPoint();
            var house = new House(Convert.ToInt16(NumberOfRooms.Text), Convert.ToInt16(NumberOfFloors.Text),
                Lenght.Text.ToDouble(), Width.Text.ToDouble(), FloorHeight.Text.ToDouble(),
                RoofHeight.Text.ToDouble(), startPoint);
            house.Generate();
            model.CommitChanges();
        }

        private void PlaceDoor_Click(object sender, RoutedEventArgs e)
        {
            House.InsertDoor(DoorWidth.Text.ToDouble(), DoorHeight.Text.ToDouble());
            model.CommitChanges();
        }

        private void PLaceWindows_Click(object sender, RoutedEventArgs e)
        {
            House.InsertWindows(WindowLenght.Text.ToDouble(),
                WindowHeight.Text.ToDouble(), WindowHeightFloor.Text.ToDouble());
            model.CommitChanges();
        }
    }
}
