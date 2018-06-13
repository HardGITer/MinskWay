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
    class Vertices
    {
        public int value;
        public bool fill;

        public Vertices(int value, bool fill)
        {
            this.value = value;
            this.fill = fill;
        }

        public Vertices()
        {
            value = 0;
            fill = false;
        }
    }
}