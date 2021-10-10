
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Counter {
        [XamlCompilation ( XamlCompilationOptions.Compile )]
        public partial class Setting : ContentPage {
                public Setting ( ) {
                        InitializeComponent ( );
                }
                protected override void OnAppearing ( ) {
                        base.OnAppearing ( );
                        CounterVM vm = ( CounterVM ) BindingContext;
                        vm.WorkMode = false;
                        vm.UnListen ( );
                }
        }
}