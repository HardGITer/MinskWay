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
using Minsk.Resources.DataBase.DataHelper;

namespace Minsk
{
    class WayMatrixUnit
    {
        public allBuses_ unit;
        public int index;

        public WayMatrixUnit(allBuses_ unit, int index)
        {
            this.unit = unit;
            this.index = index;
        }
    }
}