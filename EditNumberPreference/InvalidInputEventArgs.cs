using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.Content.Res;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messert.Controls.Droid
{
    public class InvalidInputEventArgs : EventArgs
    {
		public string Message { get; private set; }

        public InvalidInputEventArgs(string message)
        {
            Message = message;
        }
    }
}
