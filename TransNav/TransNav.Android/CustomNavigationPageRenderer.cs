using Android.Content;
using Android.Util;
using Android.Views;
using System.ComponentModel;
using TransNav;
using TransNav.Droid;
using TransNav.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using AColor = Android.Graphics.Color;
using AToolbar = Android.Support.V7.Widget.Toolbar;
using AView = Android.Views.View;
[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationPageRenderer))]

namespace TransNav.Droid
{
    public class CustomNavigationPageRenderer : NavigationPageRenderer
    {
        private readonly BarProvider _barProvider;

        public CustomNavigationPageRenderer(Context context) : base(context)
        {
            _barProvider = new BarProvider(context);
        }

        private CustomNavigationPage NavigationPage => Element as CustomNavigationPage;
        private MainActivity MainActivity => Context as MainActivity;

        protected override void OnToolbarItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnToolbarItemPropertyChanged(sender, e);
        }

        protected override void UpdateMenuItemIcon(Context context, IMenuItem menuItem, ToolbarItem toolBarItem)
        {
            base.UpdateMenuItemIcon(context, menuItem, toolBarItem);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Xamarin.Forms.NavigationPage.CurrentPageProperty.PropertyName)
            {
                if (IsNavigationBarTranslucent())
                {
                    NavigationPage.BarBackgroundColor = Color.Transparent;
                    MainActivity.MakeStatusBarTranslucent(true);
                }
                else
                {
                    NavigationPage.BarBackgroundColor = MainActivity.GetColorPrimary().ToColor();
                    MainActivity.MakeStatusBarTranslucent(false);
                }
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (IsNavigationBarTranslucent())
            {
                int containerHeight = b - t;
                NavigationPage.ContainerArea = new Rectangle(0, 0, Context.FromPixels(r - l), Context.FromPixels(containerHeight));

                for (var i = 0; i < ChildCount; i++)
                {
                    AView child = GetChildAt(i);

                    if (child is AToolbar toolbar)
                    {
                        var (barHeight, statusBarHeight) = _barProvider.GetBarHeights();
                        toolbar.Layout(0, statusBarHeight, r - l, barHeight + statusBarHeight);
                        continue;
                    }

                    child.Layout(0, 0, r, b);
                }
            }
        }



        private bool IsNavigationBarTranslucent()
        {
            return Element.CurrentPage is ITransparentActionBarPage transparentPage
                && transparentPage.IsTransparentActionBar;
        }
    }

    internal class ActionBarProvider
    {
        private readonly Context _context;

        public ActionBarProvider(Context context)
        {
            _context = context;
        }

        internal int GetActionBarHeight()
        {
            var attr = Resource.Attribute.actionBarSize;

            int actionBarHeight;
            using (var tv = new TypedValue())
            {
                actionBarHeight = 0;
                if (_context.Theme.ResolveAttribute(attr, tv, true))
                {
                    actionBarHeight = TypedValue.ComplexToDimensionPixelSize(tv.Data, _context.Resources.DisplayMetrics);
                }
            }

            if (actionBarHeight <= 0)
            {
                return IsPortrait() ? (int)_context.ToPixels(56) : (int)_context.ToPixels(48);
            }

            return actionBarHeight;
        }

        private bool IsPortrait()
        {
            return _context.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait;
        }
    }
    public class BarProvider
    {
        private readonly StatusBarProvider _statusBarProvider;
        private readonly ActionBarProvider _actionBarProvider;

        public BarProvider(Context context)
        {
            _statusBarProvider = new StatusBarProvider(context);
            _actionBarProvider = new ActionBarProvider(context);
        }

        internal (int actionBarHeight, int statusBarHeight) GetBarHeights()
        {
            var barHeight = _actionBarProvider.GetActionBarHeight();
            var statusBarHeight = _statusBarProvider.GetStatusBarHeight();
            return (barHeight, statusBarHeight);
        }
    }
    internal class StatusBarProvider
    {
        private readonly Context _context;
        private int _statusBarHeight = -1;

        public StatusBarProvider(Context context)
        {
            _context = context;
        }

        internal int GetStatusBarHeight()
        {
            if (_statusBarHeight >= 0)
            {
                return _statusBarHeight;
            }

            var result = 0;
            var resourceId = _context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                result = _context.Resources.GetDimensionPixelSize(resourceId);
            }

            return _statusBarHeight = result;
        }
    }
}