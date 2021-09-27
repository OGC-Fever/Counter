using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Counter {
        [XamlCompilation ( XamlCompilationOptions.Compile )]
        public partial class Counter : ContentPage {
                public Counter ( ) {
                        InitializeComponent ( );
                }

                protected override void OnAppearing ( ) {
                        base.OnAppearing ( );
                        CounterVM vm = ( CounterVM ) BindingContext;
                        vm.WorkMode = true;
                        vm.Listen ( );
                }
                protected override void OnDisappearing ( ) {
                        base.OnDisappearing ( );
                        CounterVM vm = ( CounterVM ) BindingContext;
                        vm.WorkMode = false;
                }
        }
}
