using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
namespace Counter {
        public class CounterVM : ContentPage {
                public CounterVM ( ) {
                        LockStatus = "OFF";
                        Reset_enable = false;
                        LockColor = Color.Red;
                }
                private string _Setting;
                public string Setting {
                        get => _Setting;
                        set {
                                _Setting = value;
                                OnPropertyChanged ( );
                        }
                }
                private int _Counter;
                public int Counter {
                        get => _Counter;
                        set {
                                _Counter = value;
                                OnPropertyChanged ( );
                        }
                }
                private int _Total;
                public int Total {
                        get => _Total;
                        set {
                                _Total = value;
                                OnPropertyChanged ( );
                        }
                }
                private Color _SettingColor;
                public Color SettingColor {
                        get => _SettingColor;
                        set {
                                _SettingColor = value;
                                OnPropertyChanged ( );
                        }
                }
                private Color _LockColor;
                public Color LockColor {
                        get => _LockColor;
                        set {
                                _LockColor = value;
                                OnPropertyChanged ( );
                        }
                }

                private Color _BG_color;
                public Color BG_color {
                        get => _BG_color;
                        set {
                                _BG_color = value;
                                OnPropertyChanged ( );
                        }
                }
                private bool _Add_enable;
                public bool Add_enable {
                        get => _Add_enable;
                        set {
                                _Add_enable = value;
                                OnPropertyChanged ( );
                        }
                }
                private bool _Setting_enable;
                public bool Setting_enable {
                        get => _Setting_enable;
                        set {
                                _Setting_enable = value;
                                OnPropertyChanged ( );
                        }
                }

                private bool _Reset_enable;
                public bool Reset_enable {
                        get => _Reset_enable;
                        set {
                                _Reset_enable = value;
                                OnPropertyChanged ( );
                        }
                }
                private bool _SettingLock;
                public bool SettingLock {
                        get => _SettingLock;
                        set {
                                _SettingLock = value;
                                if ( SettingLock ) {
                                        SettingStatus = "ON";
                                        SettingColor = Color.Green;
                                        Setting_enable = true;
                                } else {
                                        SettingStatus = "OFF";
                                        SettingColor = Color.Red;
                                        Setting_enable = false;
                                }
                                OnPropertyChanged ( );
                        }
                }
                private bool _ResetLock;
                public bool ResetLock {
                        get => _ResetLock;
                        set {
                                _ResetLock = value;
                                if ( ResetLock ) {
                                        LockStatus = "ON";
                                        LockColor = Color.Green;
                                        Reset_enable = true;
                                } else {
                                        LockStatus = "OFF";
                                        LockColor = Color.Red;
                                        Reset_enable = false;
                                }
                                OnPropertyChanged ( );
                        }
                }
                private string _SettingStatus;
                public string SettingStatus {
                        get => _SettingStatus;
                        set {
                                _SettingStatus = value;
                                OnPropertyChanged ( );
                        }
                }
                private string _LockStatus;
                public string LockStatus {
                        get => _LockStatus;
                        set {
                                _LockStatus = value;
                                OnPropertyChanged ( );
                        }
                }
                public int Delay_Time {
                        get => Preferences.Get ( "Delay_Time" , 300 );
                        set {
                                if ( value < 0 ) {
                                        value = 0;
                                }
                                Preferences.Set ( "Delay_Time" , value );
                                OnPropertyChanged ( );
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
                                for ( int i = 0 ; i < 3 ; i++ ) {
                                        BG_color = Color.OrangeRed;
                                        await Task.Delay ( Delay_Time );
                                        BG_color = Color.White;
                                        await Task.Delay ( Delay_Time );
                                }
                        } else if ( Counter > int.Parse ( Setting ) ) {
                                Counter = 1;
                                await Task.Delay ( Delay_Time );
                        } else {
                                await Task.Delay ( Delay_Time );
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
                        Application.Current.MainPage.Navigation.PushAsync ( new Setting ( ) );
                } );
                public ICommand SaveSetting => new Command ( ( ) => {
                        Preferences.Set ( "Delay_Time" , Delay_Time );
                } );
        }
}