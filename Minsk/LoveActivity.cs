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
    public class LoveActivity : Activity
    {
        StringGridAdapter adapter;
        List<Love> loveList = new List<Love>();
        GridView gridView;
        ImageButton btnBack;
        TextView txtNumber;

        DBHelper dbNew;
        SQLiteDatabase sqlitedb;
        Love love;

        List<string> loveStrList = new List<string>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Lovelayout);
            dbNew = new DBHelper(this, "LoveDB.db");
            sqlitedb = dbNew.WritableDatabase;

            txtNumber = FindViewById<TextView>(Resource.Id.txtNumberLove);
            txtNumber.Text = "Избранное";

            btnBack = FindViewById<ImageButton>(Resource.Id.btnBackLove);
            btnBack.Click += BtnBack_Click;

            AddData();

            adapter = new StringGridAdapter(this, loveStrList,"left");
            gridView = FindViewById<GridView>(Resource.Id.gridViewLove);
            gridView.Adapter = adapter;
            gridView.ItemClick += GridView_ItemClick;
        }

        private void GridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(DirectionSelectActivitycs));
            nextActivity.PutExtra("Number", adapter.GetNumber(e.Position).Split(' ')[3]);
            nextActivity.PutExtra("Type", "bus");
            StartActivity(nextActivity);
        }

        private string RemoveU(string str)
        {
            return str.Replace("<u>", "").Replace("</u>", "");
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(MainActivity));
            StartActivity(nextActivity);
        }

        private void AddData()
        {
            ICursor selectData = sqlitedb.RawQuery("select * from Love", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                do
                {
                    love = new Love();
                    love.number = selectData.GetString(selectData.GetColumnIndex("number"));
                    love.type = selectData.GetString(selectData.GetColumnIndex("type"));
                    loveList.Add(love);
                } while (selectData.MoveToNext());
                selectData.Close();
            }
            foreach (var item in loveList)
            {
                loveStrList.Add("(" + item.type + ")   " + item.number);
            }
        }

        private void InsertIntoLove(string number, string type)
        {
            DBHelper dbNew = new DBHelper(this, "LoveDB.db");
            SQLiteDatabase sqlitedb = dbNew.WritableDatabase; ;
            ContentValues contentValues = new ContentValues();
            contentValues.Put("number", number.ToString());
            contentValues.Put("type", type);
            sqlitedb.Insert("Love",null, contentValues);
        }
    }
}