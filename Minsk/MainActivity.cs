using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System;
using System.Collections.Generic;
using Minsk.Resources.DataBase.DataHelper;
using Android.Content;
using Minsk.ParsingFromWeb;
using Android.Database.Sqlite;
using Android.Database;
using SQLite;
using System.Drawing;

namespace Minsk
{
    [Activity(Label = "MinskWay", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        string transpotrTypeSelected = "bus";

        GridView gridView;
        ImageButton btnTroll;
        ImageButton btnBus;
        ImageButton btnOptions;
        ImageButton btnShortestWay;
        ImageButton btnMap;
        ImageButton btnLove;
        ImageButton btnMetro;

        StringGridAdapter adapter;

        List<string> gridViewStringTroll = new List<string>();
        List<string> busList = new List<string>();
        List<string> metroList = new List<string>()
        {
            "M1", "M2"
        };

        //for new local db
        DBHelper dbNew;
        SQLiteDatabase sqlitedb;

        List<allBuses_> busUnitList = new List<allBuses_>();

        DBHelper dbTroll;
        SQLiteDatabase sqlitedbTroll;
        List<allBuses_> trollUnitList = new List<allBuses_>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
           base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            dbNew = new DBHelper(this, "BusDBN.db");
            sqlitedb = dbNew.WritableDatabase;

            dbTroll = new DBHelper(this, "TrollbusDB.db");
            sqlitedbTroll = dbTroll.WritableDatabase;

            btnTroll = FindViewById<ImageButton>(Resource.Id.imgBtnTroll);
            btnTroll.Click += BtnTroll_Click;

            btnBus = FindViewById<ImageButton>(Resource.Id.imgBtnBus);
            btnBus.Click += BtnBus_Click;

            btnOptions = FindViewById<ImageButton>(Resource.Id.imgBtnOptions);
            btnOptions.Click += BtnOptions_Click;

            btnShortestWay = FindViewById<ImageButton>(Resource.Id.imgBtnShortestWay);
            btnShortestWay.Click += BtnShortestWay_Click;

            btnMap = FindViewById<ImageButton>(Resource.Id.imgBtnMap);
            btnMap.Click += BtnMap_Click;

            btnLove = FindViewById<ImageButton>(Resource.Id.imgBtnBest);
            btnLove.Click += BtnLove_Click;

            btnMetro = FindViewById<ImageButton>(Resource.Id.imgBtnMetro);
            btnMetro.Click += BtnMetro_Click;

            AddData();
            AddDataTroll();

            adapter = new StringGridAdapter(this, busList);
            gridView = FindViewById<GridView>(Resource.Id.gridView1);
            gridView.Adapter = adapter;
            gridView.ItemClick += GridView_ItemClick;
        }

        private void BtnMetro_Click(object sender, EventArgs e)
        {
            transpotrTypeSelected = "metro";
            adapter = new StringGridAdapter(this, metroList);
            gridView.Adapter = adapter;
        }

        private void BtnLove_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(LoveActivity));
            StartActivity(nextActivity);
        }

        private void GridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(DirectionSelectActivitycs));
            nextActivity.PutExtra("Number", adapter.GetNumber(e.Position));
            nextActivity.PutExtra("Type", transpotrTypeSelected);
            StartActivity(nextActivity);
        }

        private void BtnTroll_Click(object sender, EventArgs e)
        {
            transpotrTypeSelected = "troll";
            adapter = new StringGridAdapter(this, gridViewStringTroll);
            gridView.Adapter = adapter;
        }

        private void BtnMap_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(MapActivity));
            StartActivity(nextActivity);
        }

        private void BtnBus_Click(object sender, EventArgs e)
        {
            transpotrTypeSelected = "bus";
            adapter = new StringGridAdapter(this, busList);
            gridView.Adapter = adapter;
        }

        private void BtnOptions_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(OptionsActivity));
            StartActivity(nextActivity);
        }

        private void BtnShortestWay_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(ShortestWayActivity));
            StartActivity(nextActivity);
        }

        private void DeactivateAllBtn()
        {
            Android.Graphics.Drawables.Drawable colorr = btnLove.Background;
            btnTroll.Background = btnTroll.Background;
            btnBus.Background = btnTroll.Background;
            btnMetro.Background = btnTroll.Background;
        }

        //private void LoadData()
        //{

        //}

        private void AddData()
        {
            ICursor selectData = sqlitedb.RawQuery("select * from allBuses_", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                do
                {
                    allBuses_ bus = new allBuses_();
                    bus.number = selectData.GetString(selectData.GetColumnIndex("number"));
                    bus.wayTo = selectData.GetString(selectData.GetColumnIndex("wayTo"));
                    bus.wayFrom = selectData.GetString(selectData.GetColumnIndex("wayFrom"));

                    busUnitList.Add(bus);
                } while (selectData.MoveToNext());
                selectData.Close();
            }
            foreach (var item in busUnitList)
            {
                busList.Add(item.number);
            }
        }

        private void AddDataTroll()
        {
            ICursor selectData = sqlitedbTroll.RawQuery("select * from allBuses", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                do
                {
                    allBuses_ bus = new allBuses_();
                    bus.number = selectData.GetString(selectData.GetColumnIndex("number"));
                    bus.wayTo = selectData.GetString(selectData.GetColumnIndex("wayTo"));
                    bus.wayFrom = selectData.GetString(selectData.GetColumnIndex("wayFrom"));

                    trollUnitList.Add(bus);
                } while (selectData.MoveToNext());
                selectData.Close();
            }
            foreach (var item in trollUnitList)
            {
                gridViewStringTroll.Add(item.number);
            }
        }

        private void InsertIntoLove(string number, string type)
        {
            DBHelper dbNew = new DBHelper(this, "LoveDB.db");
            SQLiteDatabase sqlitedb = dbNew.WritableDatabase; ;
            ContentValues contentValues = new ContentValues();
            contentValues.Put("number", number);
            contentValues.Put("type", type);
            sqlitedb.Insert("Love", null, contentValues);
        }

        private void InsertIntoTable(Love unit)
        {
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "LoveDB.db")))
                {
                    connection.Insert(unit);
                }
        }
    }
}

