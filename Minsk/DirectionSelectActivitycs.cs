
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

namespace Minsk
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class DirectionSelectActivitycs : Activity 
    {
        Button btnAddInLove;

        ImageButton btnBack;
        TextView number;
        TextView txtWayTo;
        TextView txtWayFrom;

        DBHelper dbNew;
        SQLiteDatabase sqlitedb;

        DBHelper dbTroll;
        SQLiteDatabase sqlitedbTroll;

        List<allBuses_> trollUnitList = new List<allBuses_>();
        List<string> allWayListTroll = new List<string>();
        List<string> wayToTroll = new List<string>();
        List<string> wayFromTroll = new List<string>();

        List<allBuses_> busUnitList = new List<allBuses_>();
        List<string> allWayList = new List<string>();
        List<string> wayTo = new List<string>();
        List<string> wayFrom = new List<string>();

        string transportSelectedType;

        List<string> metroWayM1List = new List<string>()
        {
            "ст.м. Малиновка","ст.м. Петровщина","ст.м. Михалово","ст.м. Грушевка","ст.м. Институт Культуры","ст.м. Площадь Ленина","ст.м. Октябрьская","ст.м. Площадь Победы","ст.м. Площадь Якуба Коласа","ст.м. Академия Наук","ст.м. Парк Челюскинцев","ст.м. Московская","ст.м. Восток","ст.м. Борисовский Тракт","ст.м. Уручье"
        };

        List<string> metroWayM2List = new List<string>()
        {
            "ст.м. Каменная Горка","ст.м. Кунцевщина","ст.м. Спортивная","ст.м. Пушкинская","ст.м. Молодежная","ст.м. Фрунзенская","ст.м. Немига","ст.м. Купаловская","ст.м. Первомайская","ст.м. Пролетарская","ст.м. Тракторный Завод","ст.м. Партизанская","ст.м. Автозаводская","ст.м. Могилевская"
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.DirectionSelectLayout);
            // проблема скорее всего в ап компате

            dbNew = new DBHelper(this,"BusDBN.db");
            sqlitedb = dbNew.WritableDatabase;

            dbTroll = new DBHelper(this, "TrollbusDB.db");
            sqlitedbTroll = dbTroll.WritableDatabase;

            btnBack = FindViewById<ImageButton>(Resource.Id.btnBack);
            number = FindViewById<TextView>(Resource.Id.txtNumber);
            txtWayTo = FindViewById<TextView>(Resource.Id.txtWayTo);
            txtWayFrom = FindViewById<TextView>(Resource.Id.txtWayFrom);
            txtWayTo.Click += WayTo_Click;
            txtWayFrom.Click += WayFrom_Click;

            number.TextAlignment = Android.Views.TextAlignment.Center;
            number.Text = Intent.GetStringExtra("Number" ?? "Not recv");

            btnAddInLove = FindViewById<Button>(Resource.Id.btnLove);
            btnAddInLove.Click += BtnAddInLove_Click;

            //AddData(number.Text);
            if (Intent.GetStringExtra("Type" ?? "Not recv") == "bus")
            {
                transportSelectedType = "bus";
                AddData(Intent.GetStringExtra("Number" ?? "Not recv"));
                CutWay(allWayList, ref wayTo, ref wayFrom);
                wayTo.RemoveAt(0);
                txtWayTo.Text = wayTo[0] + " - " + wayTo[wayTo.Count - 1];
                txtWayFrom.Text = wayFrom[0] + " - " + wayFrom[wayFrom.Count - 1];
            }
            if (Intent.GetStringExtra("Type" ?? "Not recv") == "troll")
            {
                transportSelectedType = "troll";
                AddDataTroll(Intent.GetStringExtra("Number" ?? "Not recv"));
                CutWay(allWayListTroll, ref wayToTroll, ref wayFromTroll);
                wayToTroll.RemoveAt(0);
                txtWayTo.Text = wayToTroll[0] + " - " + wayToTroll[wayToTroll.Count - 1];
                txtWayFrom.Text = wayFromTroll[0] + " - " + wayFromTroll[wayFromTroll.Count - 1];
            }
            if (Intent.GetStringExtra("Type" ?? "Not recv") == "metro")
            {
                transportSelectedType = "metro";
                if (number.Text == "M1")
                {
                    txtWayTo.Text = "Малиновка - Уручье";
                    txtWayFrom.Text = "Уручье - Малиновка";
                }
                if (number.Text == "M2")
                {
                    txtWayTo.Text = "Каменная Горка - Могилевская";
                    txtWayFrom.Text = "Могилевская - Каменная Горка";
                }
            }

            btnBack.Click += BtnBack_Click;
        }

        private void BtnAddInLove_Click(object sender, EventArgs e)
        {
            DBHelper helper = new DBHelper(this, "LoveDB.db");
            helper.Add(number.Text, "bus", this);
            helper.Close();
        }

        private void WayTo_Click(object sender, EventArgs e)
        {
            if (transportSelectedType == "bus")
            {
                Intent nextActivity = new Intent(this, typeof(StopSelectActivity));
                nextActivity.PutExtra("Number", number.Text);
                string waytoStr = null;
                foreach (var item in wayTo)
                {
                    waytoStr += item;
                    waytoStr += "_";
                }
                nextActivity.PutExtra("WayToStr", waytoStr);
                nextActivity.PutExtra("Type", "bus");
                StartActivity(nextActivity);
            }

            if (transportSelectedType == "troll")
            {
                Intent nextActivity = new Intent(this, typeof(StopSelectActivity));
                nextActivity.PutExtra("Number", number.Text);
                string waytoStrTroll = null;
                foreach (var item in wayToTroll)
                {
                    waytoStrTroll += item;
                    waytoStrTroll += "_";
                }
                nextActivity.PutExtra("WayToStr", waytoStrTroll);
                nextActivity.PutExtra("Type", "troll");
                StartActivity(nextActivity);
            }

            if (transportSelectedType == "metro")
            {
                Intent nextActivity = new Intent(this, typeof(StopSelectActivity));
                nextActivity.PutExtra("Number", number.Text);
                string waytoStrMetro = null;
                if (number.Text == "M1")
                {
                    foreach (var item in metroWayM1List)
                    {
                        waytoStrMetro += item;
                        waytoStrMetro += "_";
                    }
                }
                if (number.Text == "M2")
                {
                    foreach (var item in metroWayM2List)
                    {
                        waytoStrMetro += item;
                        waytoStrMetro += "_";
                    }
                }
                nextActivity.PutExtra("WayToStr", waytoStrMetro);
                nextActivity.PutExtra("Type", "metro");
                StartActivity(nextActivity);
            }
        }

        private void WayFrom_Click(object sender, EventArgs e)
        {
            if (transportSelectedType == "bus")
            {
                Intent nextActivity = new Intent(this, typeof(StopSelectActivity));
                nextActivity.PutExtra("Number", number.Text);
                string wayfromStr = null;
                foreach (var item in wayFrom)
                {
                    wayfromStr += item;
                    wayfromStr += "_";
                }
                nextActivity.PutExtra("WayToStr", wayfromStr);
                StartActivity(nextActivity);
            }

            if (transportSelectedType == "troll")
            {
                Intent nextActivity = new Intent(this, typeof(StopSelectActivity));
                nextActivity.PutExtra("Number", number.Text);
                string wayfromStrTroll = null;
                foreach (var item in wayFromTroll)
                {
                    wayfromStrTroll += item;
                    wayfromStrTroll += "_";
                }
                nextActivity.PutExtra("WayToStr", wayfromStrTroll);
                StartActivity(nextActivity);
            }
            if (transportSelectedType == "metro")
            {
                Intent nextActivity = new Intent(this, typeof(StopSelectActivity));
                nextActivity.PutExtra("Number", number.Text);
                string waytoStrMetro = null;
                if (number.Text == "M1")
                {
                    metroWayM1List.Reverse();
                    foreach (var item in metroWayM1List)
                    {
                        waytoStrMetro += item;
                        waytoStrMetro += "_";
                    }
                }
                if (number.Text == "M2")
                {
                    metroWayM2List.Reverse();
                    foreach (var item in metroWayM2List)
                    {
                        waytoStrMetro += item;
                        waytoStrMetro += "_";
                    }
                }
                nextActivity.PutExtra("WayToStr", waytoStrMetro);
                nextActivity.PutExtra("Type", "metro");
                StartActivity(nextActivity);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(MainActivity));
            StartActivity(nextActivity);
        }

        private void AddData(string number)
        {
            ICursor selectData = sqlitedb.RawQuery("select * from allBuses_ where number ='"+ number+"'", new string[] { });
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
                string[] wayto = item.wayTo.Split('_');
                foreach (var i in item.wayTo.Split('_'))
                {
                    allWayList.Add(i);
                }
            }
        }

        private void AddDataTroll(string number)
        {
            ICursor selectData = sqlitedbTroll.RawQuery("select * from allBuses where number ='" + number + "'", new string[] { });
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
                string[] waytoTroll = item.wayTo.Split('_');
                foreach (var i in item.wayTo.Split('_'))
                {
                    allWayListTroll.Add(i);
                }
            }
        }

        public void CutWay(List<string> way, ref List<string> wayTo, ref List<string> wayFrom)
        {
            int endPoint = 0;
            for (int i = 0; i < way.Count; i++)
            {
                if (way[i] != way[i + 1])
                {
                    wayTo.Add(way[i]);
                }
                else
                {
                    wayTo.Add(way[i]);
                    endPoint = i + 1;
                    break;
                }
            }
            for (int i = endPoint; i < way.Count; i++)
            {
                wayFrom.Add(way[i]);
            }
        }
    }
}