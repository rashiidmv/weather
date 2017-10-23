using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace MyWeather
{
    //Default four cities
    public class CityPreference
    {
        ISharedPreferences prefs;
        public string City1
        {
            get { return prefs.GetString("city1", "Badagara"); ; }
            set { prefs.Edit().PutString("city1", value).Commit(); }
        }

        public string City2
        {
            get { return prefs.GetString("city2", "Sharjah"); ; }
            set { prefs.Edit().PutString("city2", value).Commit(); }
        }
        public string City3
        {
            get { return prefs.GetString("city3", "Mecca"); ; }
            set { prefs.Edit().PutString("city3", value).Commit(); }
        }

        public string City4
        {
            get { return prefs.GetString("city4", "London"); ; }
            set { prefs.Edit().PutString("city4", value).Commit(); }
        }
        public CityPreference(Activity activity)
        {
            prefs = activity.GetPreferences(Android.Content.FileCreationMode.Private);
        }

    }
}