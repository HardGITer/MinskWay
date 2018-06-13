using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Timers;
using System.Threading.Tasks;

namespace Minsk
{
    [Activity(Theme = "@style/SplashTheme", MainLauncher = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.AutoReset = false;
            timer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                    StartActivity(typeof(MainActivity));
            };
            timer.Start();
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() =>
            {
                Task.Delay(3000);
            });
            startupWork.ContinueWith(t =>
            {
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }, TaskScheduler.FromCurrentSynchronizationContext());
            startupWork.Start();
        }
    }
}