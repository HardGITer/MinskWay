using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps.Model;
using Android.Locations;

namespace Minsk
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MapActivity : Activity,IOnMapReadyCallback,ILocationListener
    {

        GoogleMap map;
        Spinner spinner;
        LocationManager locationManager;
        string provider;

        ImageButton btnBack;

        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(provider, 400, 1, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            map.Clear();

            double lat, lng;
            lat = location.Latitude;
            lng = location.Longitude;

            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(new LatLng(lat, lng));
            markerOptions.SetTitle("Your position");
            map.AddMarker(markerOptions);

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(lat, lng));
            builder.Zoom(15f);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            map.MoveCamera(cameraUpdate);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;
            googleMap.MoveCamera(CameraUpdateFactory.ZoomIn());
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.MapLayout);

            spinner = FindViewById<Spinner>(Resource.Id.spinner);
            spinner.ItemSelected += Spinner_ItemSelected;

            btnBack = FindViewById<ImageButton>(Resource.Id.btnBackMap);
            btnBack.Click += BtnBack_Click;

            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);

            locationManager = (LocationManager)GetSystemService(Context.LocationService);
            provider = locationManager.GetBestProvider(new Criteria(), false);

            Location location = locationManager.GetLastKnownLocation(provider);
            if (location == null)
            {
                Toast.MakeText(this, "No Location" , ToastLength.Short).Show();
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(MainActivity));
            StartActivity(nextActivity);
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            switch (e.Position)
            {
                case 0:
                    map.MapType = GoogleMap.MapTypeHybrid;
                    break;
                case 1:
                    map.MapType = GoogleMap.MapTypeNone;
                    break;
                case 2:
                    map.MapType = GoogleMap.MapTypeNormal;
                    break;
                case 3:
                    map.MapType = GoogleMap.MapTypeSatellite;
                    break;
                case 4:
                    map.MapType = GoogleMap.MapTypeTerrain;
                    break;
                default:
                    map.MapType = GoogleMap.MapTypeNormal;
                    break;
            }
        }
    }
}