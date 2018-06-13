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

namespace Minsk.ParsingFromWeb
{
    public class TransportUnit
    {
        public TransportUnit(string number, string wayTo, string wayFrom)
        {
            this.number = number;
            this.wayTo = wayTo;
            this.wayFrom = wayFrom;
        }

        public TransportUnit()
        {

        }

        public string number { get; set; }
        public string wayTo { get; set; }
        public string wayFrom { get; set; }

    }
}