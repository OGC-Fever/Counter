using Xamarin.Forms;

namespace Counter {
        public partial class App : Application {
                public App ( ) {
                        InitializeComponent ( );

                        MainPage = new NavigationPage ( new Counter ( ) );

                        //On<Android> ( ).SetBarHeight ( 450 );
                }
        }
}
