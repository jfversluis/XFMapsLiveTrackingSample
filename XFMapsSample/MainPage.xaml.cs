using System;
using System.Xml.Serialization;
using TcxTools;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XFMapsSample
{
    public partial class MainPage : ContentPage
    {
        private Trackpoint[] points;
        private Trackpoint[] points1;
        private Pin pin = new Pin
        {
            Label = "Other user location"
        };

        private Pin pin1 = new Pin
        {
            Label = "Other user location"
        };

        public MainPage()
        {
            InitializeComponent();

            using (var stream = Embedded.Load("route.tcx"))
            {
                var serializer = new XmlSerializer(typeof(TrainingCenterDatabase));
                var @object = serializer.Deserialize(stream);

                var database = (TrainingCenterDatabase)@object;
                points = database.Activities.Activity[0].Lap[0].Track.ToArray();
            }

            using (var stream = Embedded.Load("route1.tcx"))
            {
                var serializer = new XmlSerializer(typeof(TrainingCenterDatabase));
                var @object = serializer.Deserialize(stream);

                var database = (TrainingCenterDatabase)@object;
                points1 = database.Activities.Activity[0].Lap[0].Track.ToArray();
            }

            var idx = 0;
            myMap.Pins.Add(pin);
            myMap.Pins.Add(pin1);
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                pin.Position = new Xamarin.Forms.Maps.Position(points[idx].Position.LatitudeDegrees, points[idx].Position.LongitudeDegrees);

                pin1.Position = new Xamarin.Forms.Maps.Position(points1[idx].Position.LatitudeDegrees, points1[idx].Position.LongitudeDegrees);

                if (idx == 0)
                    myMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Xamarin.Forms.Maps.Distance.FromKilometers(1)));

                idx+=5;

                return points.Length >= idx;
            });
        }
    }
}
