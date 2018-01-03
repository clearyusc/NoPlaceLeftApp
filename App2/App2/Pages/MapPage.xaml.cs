using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using App2.Models;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private Interaction _interaction; //todo: implement an InteractionViewModel or a MapsViewModel, etc.
        private bool prayedForButtonToggle = false;
        private bool testimonyButtonToggle = false;
        private bool gospelButtonToggle = false;

        public MapPage()
        {
            InitializeComponent();
            Xamarin.FormsMaps.Init();
            _interaction = new Interaction();

            InitialMapSetup(MyMap);
        }

        private void InitialMapSetup(Map map)
        {
            MyMap.IsShowingUser = true;
            MyMap.HeightRequest = 320;
            MyMap.WidthRequest = 200;
            MyMap.VerticalOptions = LayoutOptions.FillAndExpand;

            CenterMapOnUsersLocation(map);
        }

        private async void CenterMapOnUsersLocation(Map map)
        {
            var currentLocation = await GetCurrentLocation();
            if (currentLocation != null)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(currentLocation.Latitude, currentLocation.Longitude), Distance.FromMiles(1)).WithZoom(20));
            }
            else
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(55, -122), Distance.FromMiles(0.3)));
                // todo: add error handling / dialogue display to user
            }
        }

        private async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentLocation()
        {
            Plugin.Geolocator.Abstractions.Position position = null;

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                    return position;

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                    return null;

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
            }
            catch (Exception ex)
            {
                //Todo: Display error as we have timed out or can't get location.
            }

            if (position == null)
                return null;

            return position;
        }

        private async void AddPinButtonClicked(object sender, EventArgs e)
        {
            MyLabel.Text = "add pin button clicked!";
            var geoPosition = await GetCurrentLocation();
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(geoPosition.Latitude, geoPosition.Longitude),
                Label = "custom pin",
                Address = "custom detail info"
            };
            MyMap.Pins.Add(pin);
        }

        private void PrayedForButtonClicked(object sender, EventArgs e)
        {
            if (!prayedForButtonToggle)
            {
                PrayedForButton.TextColor = Color.Coral;
            }
            else
            {
                PrayedForButton.TextColor = Color.Gray;
            }
            prayedForButtonToggle = !prayedForButtonToggle;
            // TODO: REFACTOR THIS TO A SIMPLE FLAG FOR THE FRONT-END TRACKING, STORE IT IN THE BACKEND WHEN 'SAVE' 
            //MyLabel.Text = "prayed for clicked!";
            //PrayedForButton.TextColor = Color.Green;

            //bool togglingButtonOff = false;
            //foreach(var ministryAction in _interaction.MinistryActions)
            //{
            //    if (ministryAction.Type == MinistryActionType.PrayedFor)
            //    {
            //        togglingButtonOff = true;
            //        _interaction.MinistryActions.Remove(ministryAction);
            //        break;
            //    }
            //}

            //if (togglingButtonOff) { 
            //    PrayedForButton.TextColor = Color.Gray;
            //}
            //else { 
            //    PrayedForButton.TextColor = Color.Green;
            //    _interaction.MinistryActions.Add(new MinistryAction(MinistryActionType.PrayedFor));
            //}
        }

        private void TestimonyButtonClicked(object sender, EventArgs e)
        {
            if (!testimonyButtonToggle)
                TestimonyButton.TextColor = Color.Azure;
            else
                TestimonyButton.TextColor = Color.Gray;

            testimonyButtonToggle = !testimonyButtonToggle;
        }

        private void GospelButtonClicked(object sender, EventArgs e)
        {
            if (!gospelButtonToggle)
                GospelButton.TextColor = Color.Green;
            else
                GospelButton.TextColor = Color.Gray;

            gospelButtonToggle = !gospelButtonToggle;
        }
    }
}