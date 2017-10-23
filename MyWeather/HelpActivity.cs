
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace MyWeather
{
    [Activity(Label = "Weather Help", ScreenOrientation = ScreenOrientation.Portrait)]
    public class HelpActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_help);
            string helpContent = "Weather\n"
                                  + "\tWeather app is used to know the climate details of various geographical regions."
                                  + "\n\tBy default this app shows four city climate details. "
                                  + "Tap on the weather icons displayed bottom of app to view details about particular city. "
                                  + "\nThe default cities are \n\t1. Badagara,\n\t2. Sharjah,\n\t3. Mecca\n\t4. London"
                                  + "\n\n\tThese default cities can be changed according to user preferences."
                                  + "\n\nHow to change city?"
                                  + "\n\t Go to options button, select the city which you want to change, then a popup will appear, enter desired city name then click on 'Change' button. "
                                 // + "This is an auto complete text box, if you are patient enough to wait for a while City names will be shown as auto complete. These kind of selection will leads to proper retrieval of climate details. "
                                  + "Changed city details will be displayed."
                                  + "\n\nNote - This app required internet connection to view the climate details. "
                                  + "If server is down or updating itself, then 'Please Try later' message will be displayed."
                                  + "\n\nDisclaimer - Author of this app is not responsible for the accuracy of the climate details shown.";
            ((TextView)FindViewById(Resource.Id.helpcontent)).SetText(helpContent, TextView.BufferType.Normal);
        }
    }
}