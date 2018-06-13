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

namespace Minsk
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class DeveloperActivity : Activity
    {
        ImageButton btnVK;
        ImageButton btnBack;
        Button btnDrawMessage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DeveloperLayout);
            // Create your application here

            btnVK = FindViewById<ImageButton>(Resource.Id.btnVK);
            btnVK.Click += BtnVK_Click;

            btnBack = FindViewById<ImageButton>(Resource.Id.btnBackDeveloper);
            btnBack.Click += BtnBack_Click;

            btnDrawMessage = FindViewById<Button>(Resource.Id.btnDraw);
            btnDrawMessage.Click += BtnDrawMessage_Click;
        }

        private void BtnDrawMessage_Click(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("https://mail.ru/");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(OptionsActivity));
            StartActivity(nextActivity);
        }

        private void BtnVK_Click(object sender, EventArgs e)
        {
                var uri = Android.Net.Uri.Parse("https://vk.com/id117061006");
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
        }
    }
}