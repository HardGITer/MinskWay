using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsk.Resources.DataBase.DataHelper;

namespace Minsk
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class TimeForSingleStopActivity : Activity
    {
        StringGridAdapter adapter;
        List<string> stopping = new List<string>();
        GridView gridView;
        ImageButton btnBack;
        TextView txtNumber;

        DBHelper dbNew;
        SQLiteDatabase sqlitedb;
        allBuses_ bus;

        DBHelper dbTroll;
        SQLiteDatabase sqlitedbTroll;

        int itemNumber;

        string transportSelectedType;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TimeForSingleStop);
            // Create your application here
            transportSelectedType = Intent.GetStringExtra("Type" ?? "Not recv");

            dbNew = new DBHelper(this, "BusDBN.db");
            sqlitedb = dbNew.WritableDatabase;

            dbTroll = new DBHelper(this, "TrollbusDB.db");
            sqlitedbTroll = dbTroll.WritableDatabase;

            txtNumber = FindViewById<TextView>(Resource.Id.txtNumberTimeStop);
            txtNumber.Text = Intent.GetStringExtra("Number" ?? "Not recv");

            btnBack = FindViewById<ImageButton>(Resource.Id.btnBackTimeStop);
            btnBack.Click += BtnBack_Click;

            itemNumber = Convert.ToInt32(Intent.GetStringExtra("ItemNumber" ?? "Not recv"));

            if (transportSelectedType == "bus")
            {
                AddData();
            }
            if (transportSelectedType == "troll")
            {
                AddDataTroll();
            }

            stopping = bus.fullTiming.Split('!').ToList();

            List<string> nlist = stopping[itemNumber+1].Split('_').ToList();

            for (int i = 0; i < nlist.Count; i++)
            {
                nlist[i] = RemoveU(nlist[i]);
            }
            nlist.RemoveRange(0, 1);

            adapter = new StringGridAdapter(this, nlist,"left");
            gridView = FindViewById<GridView>(Resource.Id.gridViewTimeStop);
            gridView.Adapter = adapter;
        }

        private string RemoveU(string str)
        {
            return str.Replace("<u>", "").Replace("</u>", "");
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(DirectionSelectActivitycs));
            nextActivity.PutExtra("Number", txtNumber.Text);
            nextActivity.PutExtra("Type", transportSelectedType);
            StartActivity(nextActivity);
        }

        private void AddData()
        {
            ICursor selectData = sqlitedb.RawQuery("select * from allBuses_ where number ='" + txtNumber.Text + "'", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                do
                {
                    bus = new allBuses_();
                    bus.number = selectData.GetString(selectData.GetColumnIndex("number"));
                    bus.wayTo = selectData.GetString(selectData.GetColumnIndex("wayTo"));
                    bus.wayFrom = selectData.GetString(selectData.GetColumnIndex("wayFrom"));
                    bus.allStopID = selectData.GetString(selectData.GetColumnIndex("allStopID"));
                    bus.fullTiming = selectData.GetString(selectData.GetColumnIndex("fullTiming"));
                } while (selectData.MoveToNext());
                selectData.Close();
            }
        }

        private void AddDataTroll()
        {
            ICursor selectData = sqlitedbTroll.RawQuery("select * from allBuses where number ='" + txtNumber.Text + "'", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                do
                {
                    bus = new allBuses_();
                    bus.number = selectData.GetString(selectData.GetColumnIndex("number"));
                    bus.wayTo = selectData.GetString(selectData.GetColumnIndex("wayTo"));
                    bus.wayFrom = selectData.GetString(selectData.GetColumnIndex("wayFrom"));
                    bus.allStopID = selectData.GetString(selectData.GetColumnIndex("allStopID"));
                    bus.fullTiming = selectData.GetString(selectData.GetColumnIndex("fullTiming"));
                } while (selectData.MoveToNext());
                selectData.Close();
            }
        }
    }
}