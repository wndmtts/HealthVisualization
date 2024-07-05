using Android.App;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.Locations;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Runtime;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Content;
using Java.Security;
using Android.Content.PM;

namespace HealthVisualization.Activities
{
    [Activity(Label = "LocalizacaoActivity", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class LocalizacaoActivity : AppCompatActivity, IOnMapReadyCallback, Android.Locations.ILocationListener
    {
        private GoogleMap _map;
        private LocationManager _locationManager;
        private string _locationProvider;
        private TextView _locationTextView;
        private Location _initialLocation;
        const int RequestLocationId = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_localizacao);

            _locationTextView = FindViewById<TextView>(Resource.Id.locationTextView);


            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new[] { Android.Manifest.Permission.AccessFineLocation }, RequestLocationId);
            }
            else
            {
                InitializeMap();
            }
        }

        private void InitializeMap()
        {
            var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;
            _map.MyLocationEnabled = true;

            _locationManager = (LocationManager)GetSystemService(LocationService);
            _locationProvider = LocationManager.GpsProvider;
            _initialLocation = new Location(_locationProvider);
            _initialLocation.Latitude = -25.0884;
            _initialLocation.Longitude = -50.0978;

            if (_locationManager.IsProviderEnabled(_locationProvider))
            {
                //_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
                StartLocationUpdates();
                OnLocationChanged(_initialLocation);
            }
            else
            {
                Toast.MakeText(this, "GPS is not enabled", ToastLength.Short).Show();
            }
        }


        private void StartLocationUpdates()
        {
            _locationManager.RequestLocationUpdates(_locationProvider, 500, 1, this);
        }

        public void OnLocationChanged(Location location)
        {
            if (location != null && _map != null)
            {
                LatLng userLocation = new LatLng(location.Latitude, location.Longitude);
                _map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(userLocation, 15));
                _map.AddMarker(new MarkerOptions().SetPosition(userLocation).SetTitle("You are here"));

                // Atualiza o texto com a latitude e longitude atual
                _locationTextView.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";
            }
        }

        public void OnProviderDisabled(string provider) { }
        public void OnProviderEnabled(string provider) { }
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }
    }
    }

