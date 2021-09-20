
using Android.App;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Xamarin.Forms;

namespace Counter.Droid {
        [Activity ( Label = "血汗計算機" , Icon = "@mipmap/icon" , Theme = "@style/MainTheme" , MainLauncher = true , ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
        public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ISensorEventListener {
                protected override void OnCreate ( Bundle savedInstanceState ) {
                        base.OnCreate ( savedInstanceState );

                        Xamarin.Essentials.Platform.Init ( this , savedInstanceState );
                        Forms.Init ( this , savedInstanceState );
                        LoadApplication ( new App ( ) );

                        //proximity sensor
                        SensorManager sm;
                        sm = ( SensorManager ) GetSystemService ( SensorService );
                        if ( sm.GetSensorList ( SensorType.Proximity ) != null ) {
                                Sensor Prox = sm.GetDefaultSensor ( SensorType.Proximity );
                                sm.RegisterListener ( this , Prox , SensorDelay.Normal );
                        }
                }
                public void OnSensorChanged ( SensorEvent e ) {
                        MessagingCenter.Send ( Xamarin.Forms.Application.Current , "prox" , e.Values [ 0 ].ToString ( ) );
                }
                void ISensorEventListener.OnAccuracyChanged ( Sensor sensor , SensorStatus accuracy ) {
                        return;
                }
                public override void OnRequestPermissionsResult ( int requestCode , string [ ] permissions , [GeneratedEnum] Permission [ ] grantResults ) {
                        Xamarin.Essentials.Platform.OnRequestPermissionsResult ( requestCode , permissions , grantResults );

                        base.OnRequestPermissionsResult ( requestCode , permissions , grantResults );
                }
        }
}