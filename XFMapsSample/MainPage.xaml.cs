using System;
using System.Xml.Serialization;
using TcxTools;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XFMapsSample
{
    public partial class MainPage : ContentPage
    {
        private readonly Trackpoint[] pointsRoute1;
        private readonly Trackpoint[] pointsRoute2;

        private Pin pinRoute1 = new Pin
        {
            Label = "Other users location"
        };

        private Pin pinRoute2 = new Pin
        {
            Label = "Other users location"
        };

        public MainPage()
        {
            InitializeComponent();

            pointsRoute1 = GetTrackingRoute("route.tcx");
            pointsRoute2 = GetTrackingRoute("route1.tcx");

            myMap.Pins.Add(pinRoute1);
            myMap.Pins.Add(pinRoute2);
            var index = 0;

            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                pinRoute1.Position = new Xamarin.Forms.Maps.Position(pointsRoute1[index].Position.LatitudeDegrees, pointsRoute1[index].Position.LongitudeDegrees);

                pinRoute2.Position = new Xamarin.Forms.Maps.Position(pointsRoute2[index].Position.LatitudeDegrees, pointsRoute2[index].Position.LongitudeDegrees);

                if (index == 0)
                {
                    myMap.MoveToRegion(MapSpan.FromCenterAndRadius(pinRoute1.Position, Xamarin.Forms.Maps.Distance.FromKilometers(1)));
                }

                index += 5;

                return pointsRoute1.Length >= index;
            });
        }

        private Trackpoint[] GetTrackingRoute(string filename)
        {
            using (var stream = Embedded.Load(filename))
            {
                var serializer = new XmlSerializer(typeof(TrainingCenterDatabase));
                var @object = serializer.Deserialize(stream);

                var database = (TrainingCenterDatabase)@object;
                return database.Activities.Activity[0].Lap[0].Track.ToArray();
            }
        }
    }
}
