using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            Xamarin.FormsMaps.Init();

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
            if (currentLocation != null) {
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
    }
}