using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Counter {
        public interface IPlaySoundService {
                void AlertSound ( );
                void NotifySound ( );
        }

        public class CounterVM : BaseVM {
                public void KeepScreenOn ( ) {
                        DeviceDisplay.KeepScreenOn = true;
                }
  
                public CounterVM ( ) {
                        Listen ( );
                        KeepScreenOn ( );
                        SettingLock = false;
                        ResetLock = false;
                }

                public void Listen ( ) {
                        Proc_start = DateTime.Now;
                        MessagingCenter.Subscribe<Application , string> ( Application.Current , "prox" , ( sender , args ) => {
                                if ( args == "0" ) {
                                        UnListen ( );
                                }
                        } );
                }
                public void UnListen ( ) {
                        Proc_end = DateTime.Now;
                        MessagingCenter.Unsubscribe<Application , string> ( Application.Current , "prox" );
                        if ( ( Proc_end - Proc_start ).TotalMilliseconds >= SwipeInterval && Add_enable && WorkMode ) {
                                Add.Execute ( null );
                        }
                        Listen ( );
                }

                private void Click_Gap ( ) {
                        if ( Total <= 1 ) {
                                Click_time = DateTime.Now;
                                return;
                        }
                        Total_time += ( float ) ( DateTime.Now - Click_time ).TotalSeconds;
                        Total_time_xaml = TimeSpan.FromSeconds ( Total_time );
                        Click_time = DateTime.Now;
                        try {
                                Gap = TimeSpan.FromSeconds ( Math.Round ( Total_time / ( Total - 1 ) ) );
                        } catch ( Exception ) {
                                Gap = TimeSpan.FromSeconds ( 0 );
                        }
                }
                public ICommand Set => new Command ( ( ) => {
                        Counter = 0;
                        Total = 0;
                        try {
                                if ( int.Parse ( Setting ) > 0 ) {
                                        Add_enable = true;
                                        SettingLock = false;
                                        Reset_enable = false;
                                }
                        } catch ( Exception ) {
                                Add_enable = false;
                                Setting_enable = true;
                        }
                } );
                public ICommand Add => new Command ( async ( ) => {
                        Add_enable = false;
                        if ( int.Parse ( Setting ) == 0 ) {
                                return;
                        }
                        Counter++;
                        Total++;
                        if ( Counter == int.Parse ( Setting ) ) {
                                DependencyService.Get<IPlaySoundService> ( ).AlertSound ( );
                                for ( int i = 0 ; i < 3 ; i++ ) {
                                        await Task.Delay ( DelayTime );
                                        BG_color = Color.OrangeRed;
                                        await Task.Delay ( DelayTime );
                                        BG_color = Color.White;
                                        await Task.Delay ( DelayTime );
                                }
                        } else {
                                DependencyService.Get<IPlaySoundService> ( ).NotifySound ( );
                                if ( Counter > int.Parse ( Setting ) ) {
                                        Counter = 1;
                                }
                                await Task.Delay ( DelayTime );
                        }
                        Add_enable = true;
                        Click_Gap ( );
                } );
                public ICommand Reset => new Command ( ( ) => {
                        Counter = 0;
                        Total = 0;
                        Reset_enable = false;
                        LockColor = Color.Red;
                        LockStatus = "OFF";
                        ResetLock = false;
                        Gap = TimeSpan.FromSeconds ( 0 );
                        Total_time = 0;
                } );
                public ICommand Lock => new Command ( ( ) => {
                        Reset_enable = !Reset_enable;
                        if ( Reset_enable ) {
                                LockStatus = "ON";
                                LockColor = Color.Green;
                        } else {
                                LockStatus = "OFF";
                                LockColor = Color.Red;
                        }
                } );

                public ICommand GoToSetting => new Command ( ( ) => {
                        WorkMode = true;
                        Application.Current.MainPage.Navigation.PushAsync ( new Setting ( ) );
                } );
                public ICommand SaveDelayTime => new Command ( ( ) => {
                        Preferences.Set ( "DelayTime" , DelayTime );
                } );
                public ICommand SaveSwipeInterval => new Command ( ( ) => {
                        Preferences.Set ( "SwipeInterval" , SwipeInterval );
                } );
        }
}