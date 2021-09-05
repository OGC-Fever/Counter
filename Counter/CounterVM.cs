using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
namespace Counter {
        public class CounterVM : ContentPage {
                public CounterVM ( ) {
                        LockStatus = "OFF";
                        ResetTrigger = false;
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
                private bool _ResetTrigger;
                public bool ResetTrigger {
                        get => _ResetTrigger;
                        set {
                                _ResetTrigger = value;
                                OnPropertyChanged ( );
                        }
                }
                private bool _LockTrigger;
                public bool LockTrigger {
                        get => _LockTrigger;
                        set {
                                _LockTrigger = value;
                                if ( LockTrigger ) {
                                        LockStatus = "ON";
                                        LockColor = Color.Green;
                                        ResetTrigger = true;
                                } else {
                                        LockStatus = "OFF";
                                        LockColor = Color.Red;
                                        ResetTrigger = false;
                                }
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
                        get => Preferences.Get ( "Delay_Time" , 3000 );
                        set {
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
                                }
                        } catch ( Exception ) {
                                Add_enable = false;
                        }
                        ResetTrigger = false;
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
                        ResetTrigger = false;
                        LockColor = Color.Red;
                        LockStatus = "OFF";
                        LockTrigger = false;
                } );
                public ICommand Lock => new Command ( ( ) => {
                        ResetTrigger = !ResetTrigger;
                        if ( ResetTrigger ) {
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