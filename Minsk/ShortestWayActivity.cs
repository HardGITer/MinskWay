using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class ShortestWayActivity : Activity
    {
        StringGridAdapter adapter;
        List<string> stopping = new List<string>();
        GridView gridView;
        ImageButton btnBack;
        TextView txtNumber;
        EditText editFrom;
        EditText editTo;
        Button btnFindWay;

        DBHelper dbNew;
        SQLiteDatabase sqlitedb;
        List<allBuses_> busList = new List<allBuses_>();

        List<WayMatrixUnit> linearWay = new List<WayMatrixUnit>();
        List<List<WayMatrixUnit>> wayMatrix = new List<List<WayMatrixUnit>>();
        int[,] wayIntMatrix;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ShortestWayLayout);
            // Create your application here
            //for new db
            dbNew = new DBHelper(this, "BusDBN.db");
            sqlitedb = dbNew.WritableDatabase;

            gridView = FindViewById<GridView>(Resource.Id.gridViewShortestWay);

            txtNumber = FindViewById<TextView>(Resource.Id.txtNumberShortestWay);

            btnBack = FindViewById<ImageButton>(Resource.Id.btnBackShortestWay);
            btnBack.Click += BtnBackShortestWay_Click;

            editFrom = FindViewById<EditText>(Resource.Id.editFromShortestWay);

            editTo = FindViewById<EditText>(Resource.Id.editToShortestWay);

            btnFindWay = FindViewById<Button>(Resource.Id.btnFindWay);
            btnFindWay.Click += BtnFindWay_Click;

            AddData();
        }

        private int[,] GenerateWayMatrix(List<WayMatrixUnit> wayList)
        {
            int[,] matrix = new int[wayList.Count + 1, wayList.Count + 1];
            for (int i = 0; i < wayList.Count; i++)
            {
                for (int j = 0; j < wayList.Count; j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 9999;
                    }
                    else
                    {
                        List<string> arr = wayList[j].unit.wayTo.Split('_').ToList();
                        arr.RemoveAt(0);
                        string[] narr = arr.ToArray();
                        foreach (var item in narr)
                        {
                            if (wayList[i].unit.wayTo.Split('_').ToList().Contains(item))
                            {
                                matrix[i, j] = 20;
                            }
                        }
                        if (matrix[i, j] != 9999 && matrix[i, j] != 20)
                        {
                            matrix[i, j] = int.MaxValue;
                        }
                    }
                }
            }
            return matrix;
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
                    if (i!=1)
                    {
                        wayTo.Add(way[i]);
                        endPoint = i + 1;
                        break;
                    }
                }
            }
            for (int i = endPoint; i < way.Count; i++)
            {
                wayFrom.Add(way[i]);
            }
        }

        private void BtnBackShortestWay_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(MainActivity));
            StartActivity(nextActivity);
        }

        private void BtnFindWay_Click(object sender, EventArgs e)
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, "Поиск...", ToastLength.Long).Show();
            });
            Task.Run(() =>
            {
                List<string> checklist = new List<string>();
                foreach (var item in linearWay)
                {
                    checklist.Add(item.unit.number);
                }
                if (editFrom.Text == editTo.Text || checklist.Contains(editFrom.Text) == false || checklist.Contains(editTo.Text) == false ||
                    editTo.Text == "" || editFrom.Text == "")
                {
                    Toast.MakeText(this, "данные введены не верно ", ToastLength.Short).Show();
                }
                else
                {
                    WayMatrixUnit unitFrom = new WayMatrixUnit(new allBuses_(), 0);
                    WayMatrixUnit unitTo = new WayMatrixUnit(new allBuses_(), 0);
                    foreach (var item in linearWay)
                    {
                        if (item.unit.number == editFrom.Text)
                        {
                            unitFrom = item;
                        }
                        if (item.unit.number == editTo.Text)
                        {
                            unitTo = item;
                        }
                    }
                    linearWay[0] = unitFrom;
                    linearWay[linearWay.Count - 1] = unitTo;

                    wayIntMatrix = GenerateWayMatrix(linearWay);
                    List<string> adapterList = FindSimpleWay(wayIntMatrix);
                    if (adapterList == null)
                    {
                        ShortestWayFounder shortestWayFounder = new ShortestWayFounder(wayIntMatrix, Convert.ToInt32(editFrom.Text), Convert.ToInt32(editTo.Text));
                        List<string> ListForadapter = shortestWayFounder.CalculateShortestWay();
                        adapter = new StringGridAdapter(this, ListForadapter);
                    }
                    else
                    {
                        adapter = new StringGridAdapter(this, adapterList);
                    }

                    gridView.Adapter = adapter;
                }
            });
        }

        private void BuildMatrix(string wayfrom, string wayto)
        {

        }

        private void AddData()
        {
            ICursor selectData = sqlitedb.RawQuery("select * from allBuses_ ", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                int i = 0;
                do
                {
                    allBuses_ bus = new allBuses_();
                    bus.number = selectData.GetString(selectData.GetColumnIndex("number"));
                    bus.wayTo = selectData.GetString(selectData.GetColumnIndex("wayTo"));
                    bus.wayFrom = selectData.GetString(selectData.GetColumnIndex("wayFrom"));
                    bus.allStopID = selectData.GetString(selectData.GetColumnIndex("allStopID"));
                    bus.fullTiming = selectData.GetString(selectData.GetColumnIndex("fullTiming"));
                    linearWay.Add(new WayMatrixUnit(bus, i));
                        i++;
                } while (selectData.MoveToNext());
                selectData.Close();
            }
        }

        private List<string> FindSimpleWay(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i,j] !=int.MaxValue && linearWay[i].unit.number == editFrom.Text && linearWay[j].unit.number == editTo.Text)
                    {
                        return new List<string>() { editFrom.Text + " - " + editTo.Text + " (20мин)" };
                    }
                }
            }
            return null;
        }

        private void AddData(string wayFrom, string wayTo, ref List<allBuses_> busHavingCheckedStop)
        {
            ICursor selectData = sqlitedb.RawQuery("select * from allBuses_ ", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                do
                {
                    allBuses_ bus = new allBuses_();
                    bus.number = selectData.GetString(selectData.GetColumnIndex("number"));
                    bus.wayTo = selectData.GetString(selectData.GetColumnIndex("wayTo"));
                    bus.wayFrom = selectData.GetString(selectData.GetColumnIndex("wayFrom"));
                    bus.allStopID = selectData.GetString(selectData.GetColumnIndex("allStopID"));
                    bus.fullTiming = selectData.GetString(selectData.GetColumnIndex("fullTiming"));
                    if (bus.wayTo.Contains(wayTo) && bus.wayTo.Contains(wayFrom))
                    {
                        busHavingCheckedStop.Add(bus);
                    }
                } while (selectData.MoveToNext());
                selectData.Close();
            }
        }
    }
}