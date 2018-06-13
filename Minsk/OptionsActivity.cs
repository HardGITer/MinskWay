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
using Minsk.ParsingFromWeb;
using Minsk.Resources.DataBase.DataHelper;

namespace Minsk
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class OptionsActivity : Activity
    {
        DataBase db;

        ImageButton btnBack;
        Button btnManual;
        Button btnInfo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.OptionsLayout);

            btnBack = FindViewById<ImageButton>(Resource.Id.btnBackOptions);
            btnBack.Click += BtnBack_Click;

            btnManual = FindViewById<Button>(Resource.Id.btnManual);
            btnManual.Click += BtnManual_Click;

            btnInfo = FindViewById<Button>(Resource.Id.btnInfo);
            btnInfo.Click += BtnInfo_Click;
        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(DeveloperActivity));
            StartActivity(nextActivity);
        }

        private void BtnManual_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(ManualActivity));
            StartActivity(nextActivity);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(MainActivity));
            StartActivity(nextActivity);
        }

        private void BtnUpdateShedule_Click(object sender, EventArgs e)
        {
            db = new DataBase();
            db.CreateDatabase();
            Parsing parsing = new Parsing();
            string strHTMLPage = parsing.SaveHTMLPage();
            parsing.ParseHTMLPage(strHTMLPage, "<a href='.*?RouteNum=.*?&day=.*?&Transport=Autobus'>(.*?)<\\/a>");
            List<TransportUnit> shedule = parsing.Shedule;

            for (int i = 0; i < shedule.Count; i++)
            {
                db.InsertIntoTable(shedule[i]);
            }
        }
    }
}