
using Android.App;
using Android.Content.PM;
using Android.Hardware;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Counter.Droid;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency ( typeof ( PlaySound ) )]
namespace Counter.Droid {
        public class PlaySound : IPlaySoundService {
                public async void AlertSound ( ) {
                        ToneGenerator tone = new ToneGenerator ( Stream.Notification , 100 );
                        for ( int i = 0 ; i < 3 ; i++ ) {
                                tone.StartTone ( Tone.CdmaOneMinBeep , 300 );
                                await Task.Delay ( 800 );
                        }
                        tone.Release ( );
                }
                public async void NotifySound ( ) {
                        ToneGenerator tone = new ToneGenerator ( Stream.Notification , 100 );
                        tone.StartTone ( Tone.CdmaNetworkBusy , 300 );
                        await Task.Delay ( 500 );
                        tone.Release ( );
                }
        }
        [Activity ( Label = "血汗計算機" , Icon = "@mipmap/icon" , Theme = "@style/MainTheme" , MainLauncher = true , ScreenOrientation = ScreenOrientation.Portrait , ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
        public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ISensorEventListener {
                public static MainActivity Instance { get; set; }
                protected override void OnCreate ( Bundle savedInstanceState ) {
                        base.OnCreate ( savedInstanceState );

                        Xamarin.Essentials.Platform.Init ( this , savedInstanceState );
                        Forms.Init ( this , savedInstanceState );
                        LoadApplication ( new App ( ) );

                        CallSensorManager ( true );

                        Instance = this;
                }

                public void OnSensorChanged ( SensorEvent e ) {
                        if ( e.Values [ 0 ] == 0 ) {
                                if ( e.Sensor.Type == SensorType.Proximity ) {
                                        MessagingCenter.Send ( Xamarin.Forms.Application.Current , "prox" );
                                }
                        }
                }
                void ISensorEventListener.OnAccuracyChanged ( Sensor sensor , SensorStatus accuracy ) {
                        return;
                }
                public override void OnRequestPermissionsResult ( int requestCode , string [ ] permissions , [GeneratedEnum] Permission [ ] grantResults ) {
                        Xamarin.Essentials.Platform.OnRequestPermissionsResult ( requestCode , permissions , grantResults );
                        base.OnRequestPermissionsResult ( requestCode , permissions , grantResults );
                }
                public void CallSensorManager ( bool reg ) {
                        SensorManager sm;
                        sm = ( SensorManager ) GetSystemService ( SensorService );
                        if ( sm.GetSensorList ( SensorType.Proximity ) != null ) {
                                if ( reg ) {
                                        Sensor prox = sm.GetDefaultSensor ( SensorType.Proximity );
                                        sm.RegisterListener ( this , prox , SensorDelay.Normal );
                                } else {
                                        sm.UnregisterListener ( this );
                                }
                        }
                }
                protected override void OnResume ( ) {
                        base.OnResume ( );
                        CallSensorManager ( true );
                        MessagingCenter.Send ( Xamarin.Forms.Application.Current , "timer start" );
                }
                protected override void OnUserLeaveHint ( ) {
                        base.OnUserLeaveHint ( );
                        CallSensorManager ( false );
                }
                public override void OnBackPressed ( ) {
                        base.OnBackPressed ( );
                        CallSensorManager ( false );
                        MessagingCenter.Send ( Xamarin.Forms.Application.Current , "back pressed" );
                }
        }
}