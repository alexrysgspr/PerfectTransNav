using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Prism;
using Prism.Ioc;
using AColor = Android.Graphics.Color;

namespace TransNav.Droid
{
    [Activity(Label = "TransNav", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            MakeStatusBarTranslucent(false);
            LoadApplication(new App(new AndroidInitializer()));
        }

        internal void MakeStatusBarTranslucent(bool makeTranslucent)
        {
            if (makeTranslucent)
            {
                SetStatusBarColor(AColor.Transparent);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LayoutFullscreen | SystemUiFlags.LayoutStable);
                }
            }
            else
            {
                SetStatusBarColor(GetColorPrimaryDark());
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Window.DecorView.SystemUiVisibility = StatusBarVisibility.Visible;
                }
            }
        }

        private AColor GetColorPrimaryDark()
        {
            using (var value = new TypedValue())
            {
                if (Theme.ResolveAttribute(Resource.Attribute.colorPrimaryDark, value, true))
                {
                    var color = new AColor(value.Data);
                    return color;
                }

                return AColor.Transparent;
            }
        }

        internal AColor GetColorPrimary()
        {
            using (var value = new TypedValue())
            {
                if (Theme.ResolveAttribute(Resource.Attribute.colorPrimary, value, true))
                {
                    var color = new AColor(value.Data);
                    return color;
                }

                return AColor.Transparent;
            }
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

