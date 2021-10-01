using System;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Counter {
        public class BaseVM : ContentPage {

                public BaseVM ( ) {

                }

                public DateTime Proc_start { get; set; }
                public DateTime Proc_end { get; set; }
                public bool WorkMode { get; set; }

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
                                        WorkMode = false;
                                        Add_enable = false;
                                } else {
                                        SettingStatus = "OFF";
                                        SettingColor = Color.Red;
                                        Setting_enable = false;
                                        WorkMode = true;
                                        if ( Setting != null ) {
                                                Add_enable = true;
                                        }
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
                public int DelayTime {
                        get => Preferences.Get ( "DelayTime" , 300 );
                        set {
                                if ( value < 0 ) {
                                        value = 0;
                                }
                                Preferences.Set ( "DelayTime" , value );
                                OnPropertyChanged ( );
                        }
                }
                public int SwipeInterval {
                        get => Preferences.Get ( "SwipeInterval" , 2000 );
                        set {
                                if ( value < 0 ) {
                                        value = 0;
                                }
                                Preferences.Set ( "SwipeInterval" , value );
                                OnPropertyChanged ( );
                        }
                }
                private TimeSpan _Gap;
                public TimeSpan Gap {
                        get => _Gap;
                        set {
                                _Gap = value;
                                OnPropertyChanged ( );
                        }
                }
                private TimeSpan _Total_time;
                public TimeSpan Total_time {
                        get => _Total_time;
                        set {
                                _Total_time = value;
                                OnPropertyChanged ( );
                        }
                }

                private DateTime _Click_time;
                public DateTime Click_time {
                        get => _Click_time;
                        set {
                                _Click_time = value;
                                OnPropertyChanged ( );
                        }
                }

        }
}