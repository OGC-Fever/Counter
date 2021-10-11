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
                public CounterVM ( ) {
                        SettingLock = false;
                        ResetLock = false;
                }
                public async Task SpeakNow ( string text ) {
                        SpeechOptions settings = new SpeechOptions ( ) {
                                Volume = .1f ,
                                Pitch = 1.0f
                        };
                        await TextToSpeech.SpeakAsync ( text , settings );
                }

                public void Listen ( ) {
                        Proc_start = DateTime.Now;
                        MessagingCenter.Subscribe<Application> ( Application.Current , "timer start" , ( sender ) => {
                                Proc_start = DateTime.Now;
                                Click_time = DateTime.Now;
                        } );
                        MessagingCenter.Subscribe<Application> ( Application.Current , "prox" , ( sender ) => {
                                if ( WorkMode && Add_enable ) {
                                        Proc_end = DateTime.Now;
                                        if ( ( Proc_end - Proc_start ).TotalMilliseconds >= SwipeInterval ) {
                                                Add.Execute ( null );
                                                Proc_start = DateTime.Now;
                                        }
                                }
                        } );
                }
                public void UnListen ( ) {
                        MessagingCenter.Unsubscribe<Application> ( Application.Current , "prox" );
                }

                public ICommand Set => new Command ( ( ) => {
                        Click_time = DateTime.Now;
                        try {
                                if ( Counter > Total || Counter > int.Parse ( Setting ) ) {
                                        return;
                                }
                                if ( int.Parse ( Setting ) > 0 ) {
                                        Add_enable = true;
                                        SettingLock = false;
                                        Reset_enable = false;
                                        Setting = int.Parse ( Setting ).ToString ( );
                                }
                        } catch ( Exception ) {
                                Add_enable = false;
                                Setting_enable = true;
                        }
                } );
                public ICommand Add => new Command ( async ( ) => {
                        Add_enable = false;
                        Counter++;
                        Total++;
                        Total_time += DateTime.Now - Click_time;
                        Gap = TimeSpan.FromSeconds ( Math.Round ( Total_time.TotalSeconds / Total ) );
                        Click_time = DateTime.Now;
                        try {
                                if ( Counter == int.Parse ( Setting ) ) {
                                        await SpeakNow ( $"{Counter} set" );
                                        DependencyService.Get<IPlaySoundService> ( ).AlertSound ( );
                                        for ( int i = 0 ; i < 3 ; i++ ) {
                                                await Task.Delay ( DelayTime );
                                                BG_color = Color.OrangeRed;
                                                await Task.Delay ( DelayTime );
                                                BG_color = Color.White;
                                                await Task.Delay ( DelayTime );
                                        }
                                } else {
                                        if ( Counter > int.Parse ( Setting ) ) {
                                                Counter = 1;
                                        }
                                        await SpeakNow ( $"{Counter} set" );
                                        DependencyService.Get<IPlaySoundService> ( ).NotifySound ( );
                                        await Task.Delay ( DelayTime );
                                }
                        } catch ( Exception ) {
                                return;
                        }
                        Add_enable = true;
                } );
                public ICommand Reset => new Command ( ( ) => {
                        Counter = 0;
                        Total = 0;
                        Reset_enable = false;
                        LockColor = Color.Red;
                        LockStatus = "OFF";
                        ResetLock = false;
                        Gap = TimeSpan.FromSeconds ( 0 );
                        Total_time = TimeSpan.FromSeconds ( 0 );
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
                public ICommand Edit_Counter => new Command ( async ( ) => {
                        if ( SettingLock == false ) {
                                return;
                        }
                        WorkMode = false;
                        string result = await Application.Current.MainPage.DisplayPromptAsync ( "自訂數量" , null , keyboard: Keyboard.Numeric );
                        try {
                                if ( int.Parse ( result ) <= int.Parse ( Setting ) ) {
                                        Counter = int.Parse ( result );
                                }
                        } catch ( Exception ) {
                                return;
                        }
                        WorkMode = true;
                } );
                public ICommand Edit_Total => new Command ( async ( ) => {
                        if ( SettingLock == false ) {
                                return;
                        }
                        WorkMode = false;
                        string result = await Application.Current.MainPage.DisplayPromptAsync ( "自訂總數" , null , keyboard: Keyboard.Numeric );
                        try {
                                if ( int.Parse ( result ) >= Counter ) {
                                        Total = int.Parse ( result );
                                }
                        } catch ( Exception ) {
                                return;
                        }
                        WorkMode = true;
                } );

                public ICommand GoToSetting => new Command ( ( ) => {
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