using System;
using Android.Content;
using Android.Support.Design.BottomNavigation;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Views;
using ExhibitorModule.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("CIC")]
[assembly: ExportEffect(typeof(TabShiftEffect), nameof(TabShiftEffect))]
namespace ExhibitorModule.Droid.Effects
{
    public class TabShiftEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (!(Container.GetChildAt(0) is ViewGroup layout))
                return;

            if (!(layout.GetChildAt(1) is BottomNavigationView bottomNavigationView))
                return;

            // This is what we set to adjust if the shifting happens
            bottomNavigationView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityLabeled;
        }

        protected override void OnDetached()
        {
        }
    }
}
