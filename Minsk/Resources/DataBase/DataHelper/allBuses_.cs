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
using SQLite;

namespace Minsk.Resources.DataBase.DataHelper
{
    public class allBuses_
    {   [PrimaryKey]
        public string number { get; set; }
        public string wayTo { get; set; }
        public string wayFrom { get; set; }
        public string allStopID { get; set; }
        public string fullTiming { get; set; }
    }
}