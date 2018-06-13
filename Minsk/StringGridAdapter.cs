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
using Java.Lang;
using Android.Graphics;

namespace Minsk
{
    class StringGridAdapter : BaseAdapter
    {
        List<string> items;
        Activity context;
        string position;

        public StringGridAdapter(Activity context, List<string> items):base()
        {
            this.context = context;
            this.items = items;
        }

        public StringGridAdapter(Activity context, List<string> items, string position) : base()
        {
            this.context = context;
            this.items = items;
            this.position = position;
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }
        public string GetNumber(int position)
        {
            return items[position];
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null)
            {
                if (this.position == null)
                {
                    view = context.LayoutInflater.Inflate(Resource.Layout.gridView_Layout, null);
                }
                else
                    view = context.LayoutInflater.Inflate(Resource.Layout.GridViewTimingLayout, null);
            }
            view.FindViewById<TextView>(Resource.Id.textView1).Text = item;
            return view;
        }
    }
}