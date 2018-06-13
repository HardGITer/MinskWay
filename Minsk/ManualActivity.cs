using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace Minsk
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class ManualActivity : Activity
    {
        ImageButton btnBack;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ManualLayout);

            btnBack = FindViewById<ImageButton>(Resource.Id.btnBackManual);
            btnBack.Click += BtnBack_Click;

            var viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            ImageAdapter adapter = new ImageAdapter(this);
            viewPager.Adapter = adapter;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(OptionsActivity));
            StartActivity(nextActivity);
        }
    }
}