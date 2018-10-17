using Xamarin.Forms;

namespace TransNav.Views
{
    public partial class MyDetailPage : ContentPage, ITransparentActionBarPage
    {
        public MyDetailPage()
        {
            InitializeComponent();
            IsTransparentActionBar = true;
        }

        public bool IsTransparentActionBar { get; }
    }
}
