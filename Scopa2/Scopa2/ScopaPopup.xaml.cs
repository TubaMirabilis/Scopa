using Xamarin.Forms.Xaml;

namespace Scopa2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScopaPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ScopaPopup()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}