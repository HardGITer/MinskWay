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
    public class StopSelectActivity : Activity
    {
        StringGridAdapter adapter;
        List<string> stopping = new List<string>();
        GridView gridView;
        ImageButton btnBack;
        TextView txtNumber;

        string zarabotaiPojalusta = "help";

        string transportSelectedType;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.StopSelectLayout);
            // Create your application here
            stopping = Intent.GetStringExtra("WayToStr" ?? "Not recv").Split('_').ToList();
            transportSelectedType = Intent.GetStringExtra("Type" ?? "Not recv");

            adapter = new StringGridAdapter(this, stopping,"left");
            gridView = FindViewById<GridView>(Resource.Id.gridViewStopSelect);
            gridView.Adapter = adapter;
            gridView.ItemClick += GridView_ItemClick;

            txtNumber = FindViewById<TextView>(Resource.Id.txtNumberStopSelect);
            txtNumber.Text = Intent.GetStringExtra("Number" ?? "Not recv");

            btnBack = FindViewById<ImageButton>(Resource.Id.btnBackStopSelect);
            btnBack.Click += BtnBack_Click;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
                Intent nextActivity = new Intent(this, typeof(DirectionSelectActivitycs));
                nextActivity.PutExtra("Number", txtNumber.Text);
                nextActivity.PutExtra("Type", transportSelectedType);
                StartActivity(nextActivity);
        }

        private void GridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (transportSelectedType != "metro")
            {
                int position = e.Position;
                Intent nextActivity = new Intent(this, typeof(TimeForSingleStopActivity));
                nextActivity.PutExtra("Number", txtNumber.Text);
                nextActivity.PutExtra("Type", transportSelectedType);
                txtNumber.Text = position.ToString();
                nextActivity.PutExtra("ItemNumber", txtNumber.Text);
                StartActivity(nextActivity);
            }
            else
            {
                Toast.MakeText(this, "Поезд ходит каждые 2 - 4 минуты", ToastLength.Short).Show();
            }
        }
    }
}