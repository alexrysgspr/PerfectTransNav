using Xamarin.Forms;

namespace TransNav.Views
{
    public partial class MyDetailPage : ContentPage, ITransparentActionBarPage
    {
        public MyDetailPage()
        {
            InitializeComponent();
        }

        public bool IsTransparentActionBar { get; }
}
