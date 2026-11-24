//using Android.App;
//using Android.Content.PM;
//using Android.OS;

//namespace CalorieTracker
//{
//    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
//    public class MainActivity : MauiAppCompatActivity
//    {
//    }
//}
using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.View;

namespace CalorieTracker
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Window != null)
            {
                var controller = WindowCompat.GetInsetsController(Window, Window.DecorView);
                if (controller != null)
                {
                    // true = тъмни икони (за светъл/бял фон най-горе)
                    // false = светли/бели икони (за тъмен фон най-горе)
                    controller.AppearanceLightStatusBars = true;
                }
            }
        }
    }
}