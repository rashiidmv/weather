using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Text;
using Java.Util;
using Org.Json;
using Android.Gms.Ads;

namespace MyWeather
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class WeatherActivity : Activity
    {
        private string CitiName = null;
        private AlertDialog.Builder builder;
        private bool IsCitiChanged = false;

        TextView cityField;
        TextView updatedField;
        Switch fahrenheitSwitch;
        TextView descriptionField;
        TextView currentTemperatureField;
        TextView weatherIcon;
        TextView humidityField;
        TextView pressureField;
        TextView windField;
        TextView sunsetField;
        TextView sunriseField;


        //   Handler handler;
        string whichCity;
        JSONObject currentSelection;
        Typeface weatherFont;
        CityPreference cp;
        JSONObject[] allCityData;

        LinearLayout citi1_layout;
        LinearLayout citi2_layout;
        LinearLayout citi3_layout;
        LinearLayout citi4_layout;
        TextView weather_icon1;
        TextView weather_icon2;
        TextView weather_icon3;
        TextView weather_icon4;
        private AdView mAdView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_weather);

            //  handler = new Handler(Looper.MainLooper);
            whichCity = string.Empty;
            //   SetEmpty();
            weatherFont = Typeface.CreateFromAsset(this.Assets, "fonts/weather.ttf");

            allCityData = new JSONObject[4];

            cityField = (TextView)FindViewById(Resource.Id.city_field);
            updatedField = (TextView)FindViewById(Resource.Id.updated_field);
            fahrenheitSwitch = (Switch)FindViewById(Resource.Id.fahrenheit_switch);
            fahrenheitSwitch.CheckedChange += FahrenheitSwitch_CheckedChange;
            descriptionField = (TextView)FindViewById(Resource.Id.description_field);
            humidityField = (TextView)FindViewById(Resource.Id.humidity_field);
            pressureField = (TextView)FindViewById(Resource.Id.pressure_field);
            currentTemperatureField = (TextView)FindViewById(Resource.Id.current_temperature_field);
            weatherIcon = (TextView)FindViewById(Resource.Id.weather_icon);
            weatherIcon.SetTypeface(weatherFont, TypefaceStyle.Normal);

            windField = (TextView)FindViewById(Resource.Id.wind_field);
            sunsetField = (TextView)FindViewById(Resource.Id.sunset_field);
            sunriseField = (TextView)FindViewById(Resource.Id.sunrise_field);

            citi1_layout = (LinearLayout)FindViewById(Resource.Id.citi1_layout);
            citi1_layout.Click += Citi1_layout_Click;
            citi2_layout = (LinearLayout)FindViewById(Resource.Id.citi2_layout);
            citi2_layout.Click += Citi2_layout_Click;
            citi3_layout = (LinearLayout)FindViewById(Resource.Id.citi3_layout);
            citi3_layout.Click += Citi3_layout_Click;
            citi4_layout = (LinearLayout)FindViewById(Resource.Id.citi4_layout);
            citi4_layout.Click += Citi4_layout_Click;

            weather_icon1 = (TextView)FindViewById(Resource.Id.weather_icon1);
            weather_icon1.SetTypeface(weatherFont, TypefaceStyle.Normal);
            weather_icon2 = (TextView)FindViewById(Resource.Id.weather_icon2);
            weather_icon2.SetTypeface(weatherFont, TypefaceStyle.Normal);
            weather_icon3 = (TextView)FindViewById(Resource.Id.weather_icon3);
            weather_icon3.SetTypeface(weatherFont, TypefaceStyle.Normal);
            weather_icon4 = (TextView)FindViewById(Resource.Id.weather_icon4);
            weather_icon4.SetTypeface(weatherFont, TypefaceStyle.Normal);
            //if (isNetworkAvailable())
            //{
            FindViewById(Resource.Id.main_content_buzy).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.city1_content_buzy).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.city2_content_buzy).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.city3_content_buzy).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.city4_content_buzy).Visibility = ViewStates.Visible;

            cp = new CityPreference(this);
            if (isNetworkAvailable())
                updateWeatherData();
            else
            {
                //Toast.MakeText(this, "\t\t\tNo Internet\nPlease turn on data connection", ToastLength.Long).Show();
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage("No Internet\nPlease turn on data connection").SetPositiveButton(Resource.String.ok, OnNetworkUpdation);
                builder.Show();
            }

            //  CITIES = new string[] { "Delhi","Bangalore", "Bangalore1", "Bangalore2", "Bangalore3" };
            //Thread t = new Thread(() =>
            //{
            //    System.IO.Stream input = Assets.Open("city.list.json");
            //    BufferedReader reader = new BufferedReader(new InputStreamReader(input));
            //    StringBuffer json = new StringBuffer(1024);
            //    string tmp = "";
            //    while ((tmp = reader.ReadLine()) != null)
            //        json.Append(tmp).Append("\n");
            //    reader.Close();
            //    string[] allobjects = json.ToString().Split('\n');
            //    CITIES = new string[allobjects.Length - 2];

            //    int j = 0;
            //    for (int i = 1; i < (allobjects.Length - 1); i++)
            //    {
            //        JSONObject obj = new JSONObject(allobjects[i]);
            //        CITIES[j++] = obj.GetString("name");
            //    }

            //    RunOnUiThread(() =>
            //        {
            //            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line, CITIES);
            //        });

            //});
            //t.Start();
            //   MobileAds.Initialize(this, "ca-app-pub-4243626382965279~1983949294");
            //MobileAds.Initialize(this, "ca-app-pub-3940256099942544~3347511713");
            
            mAdView = (AdView)FindViewById(Resource.Id.adView);
            AdRequest adRequest = new AdRequest.Builder().Build();
            mAdView.LoadAd(adRequest);

        }

        private void FahrenheitSwitch_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            TextView current_temperature_field = ((TextView)FindViewById(Resource.Id.current_temperature_field));
            TextView temp_field1 = ((TextView)FindViewById(Resource.Id.temp_field1));
            TextView temp_field2 = ((TextView)FindViewById(Resource.Id.temp_field2));
            TextView temp_field3 = ((TextView)FindViewById(Resource.Id.temp_field3));
            TextView temp_field4 = ((TextView)FindViewById(Resource.Id.temp_field4));

            TextView current_temperature_field_celcius = ((TextView)FindViewById(Resource.Id.current_temperature_field_celcius));
            TextView temp_field1_celcius = ((TextView)FindViewById(Resource.Id.temp_field1_celcius));
            TextView temp_field2_celcius = ((TextView)FindViewById(Resource.Id.temp_field2_celcius));
            TextView temp_field3_celcius = ((TextView)FindViewById(Resource.Id.temp_field3_celcius));
            TextView temp_field4_celcius = ((TextView)FindViewById(Resource.Id.temp_field4_celcius));

            if (e.IsChecked)
            {
                fahrenheitSwitch.SetText(Resource.String.offfahrenheit);
                if (currentSelection != null)
                {
                    JSONObject main = currentSelection.GetJSONObject("main");
                    current_temperature_field.SetText(string.Format("{0:0.00}", ((main.GetDouble("temp") * 1.8 )+ 32)), TextView.BufferType.Normal);
                    current_temperature_field_celcius.SetText("F", TextView.BufferType.Normal);
                }
                if (allCityData[0] != null)
                {
                    JSONObject json = allCityData[0];
                    JSONObject main = json.GetJSONObject("main");
                    temp_field1.SetText(string.Format("{0:0.00}", ((main.GetDouble("temp") * 1.8) + 32)), TextView.BufferType.Normal);
                    temp_field1_celcius.SetText("F", TextView.BufferType.Normal);
                }
                if (allCityData[1] != null)
                {
                    JSONObject json = allCityData[1];
                    JSONObject main = json.GetJSONObject("main");
                    temp_field2.SetText(string.Format("{0:0.00}", ((main.GetDouble("temp") * 1.8) + 32)), TextView.BufferType.Normal);
                    temp_field2_celcius.SetText("F", TextView.BufferType.Normal);
                }
                if (allCityData[2] != null)
                {
                    JSONObject json = allCityData[2];
                    JSONObject main = json.GetJSONObject("main");
                    temp_field3.SetText(string.Format("{0:0.00}", ((main.GetDouble("temp") * 1.8) + 32)), TextView.BufferType.Normal);
                    temp_field3_celcius.SetText("F", TextView.BufferType.Normal);
                }
                if (allCityData[3] != null)
                {
                    JSONObject json = allCityData[3];
                    JSONObject main = json.GetJSONObject("main");
                    temp_field4.SetText(string.Format("{0:0.00}", ((main.GetDouble("temp") * 1.8) + 32)), TextView.BufferType.Normal);
                    temp_field4_celcius.SetText("F", TextView.BufferType.Normal);
                }
            }
            else
            {
                fahrenheitSwitch.SetText(Resource.String.onfahrenheit);
                if (currentSelection != null)
                {
                    JSONObject main = currentSelection.GetJSONObject("main");
                    current_temperature_field.SetText(string.Format("{0:0.00}", main.GetDouble("temp")), TextView.BufferType.Normal);
                    current_temperature_field_celcius.SetText("C", TextView.BufferType.Normal);
                }
                if (allCityData[0] != null)
                {
                    JSONObject json = allCityData[0];
                    JSONObject main = json.GetJSONObject("main");
                    temp_field1.SetText(string.Format("{0:0.00}", main.GetDouble("temp")), TextView.BufferType.Normal);
                    temp_field1_celcius.SetText("C", TextView.BufferType.Normal);
                }
                if (allCityData[1] != null)
                {
                    JSONObject json = allCityData[1];
                    JSONObject main = json.GetJSONObject("main");
                    temp_field2.SetText(string.Format("{0:0.00}", main.GetDouble("temp")), TextView.BufferType.Normal);
                    temp_field2_celcius.SetText("C", TextView.BufferType.Normal);
                }
                if (allCityData[2] != null)
                {
                    JSONObject json = allCityData[2];
                    JSONObject main = json.GetJSONObject("main");
                    temp_field3.SetText(string.Format("{0:0.00}", main.GetDouble("temp")), TextView.BufferType.Normal);
                    temp_field3_celcius.SetText("C", TextView.BufferType.Normal);
                }
                if (allCityData[3] != null)
                {
                    JSONObject json = allCityData[3];
                    JSONObject main = json.GetJSONObject("main");
                    temp_field4.SetText(string.Format("{0:0.00}", main.GetDouble("temp")), TextView.BufferType.Normal);
                    temp_field4_celcius.SetText("C", TextView.BufferType.Normal);
                }
            }
        }

        private void OnNetworkUpdation(object sender, DialogClickEventArgs e)
        {
            if (isNetworkAvailable())
                updateWeatherData();
            else
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage("No Internet\nPlease turn on data connection").SetPositiveButton(Resource.String.ok, OnNetworkUpdation);
                builder.Show();
            }
        }

        private void SetEmpty()
        {
            ((TextView)FindViewById(Resource.Id.city_field)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.updated_field)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.current_temperature_field)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.description_field)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.humidity_field)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.pressure_field)).SetText("", TextView.BufferType.Normal);

            ((TextView)FindViewById(Resource.Id.temp_field1)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.city_field1)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.temp_field2)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.city_field2)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.temp_field3)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.city_field3)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.temp_field4)).SetText("", TextView.BufferType.Normal);
            ((TextView)FindViewById(Resource.Id.city_field4)).SetText("", TextView.BufferType.Normal);
        }

        private bool isNetworkAvailable()
        {
            ConnectivityManager connectivityManager
                  = (ConnectivityManager)GetSystemService(Context.ConnectivityService);
            NetworkInfo activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            return activeNetworkInfo != null && activeNetworkInfo.IsConnected;
        }

        private void renderWeather(JSONObject json)
        {
            FindViewById(Resource.Id.main_content_buzy).Visibility = ViewStates.Gone;
            FindViewById(Resource.Id.current_temperature_field_celcius).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.current_temperature_field_degree).Visibility = ViewStates.Visible;
            try
            {
                JSONObject sys = json.GetJSONObject("sys");
                cityField.SetText(json.GetString("name") +
                       ", " +
                       sys.GetString("country"), TextView.BufferType.Normal);

                JSONObject details = json.GetJSONArray("weather").GetJSONObject(0);
                JSONObject main = json.GetJSONObject("main");
                descriptionField.SetText(details.GetString("description"), TextView.BufferType.Normal);
                humidityField.SetText("Humidity: " + main.GetString("humidity"), TextView.BufferType.Normal);
                pressureField.SetText("Pressure: " + main.GetString("pressure") + " hPa", TextView.BufferType.Normal);

                if (fahrenheitSwitch.Checked)
                {
                    currentTemperatureField.SetText(string.Format("{0:0.00}", ((main.GetDouble("temp") * 1.8) + 32)), TextView.BufferType.Normal);
                    ((TextView)FindViewById(Resource.Id.current_temperature_field_celcius)).SetText("F", TextView.BufferType.Normal);
                }
                else
                {
                    currentTemperatureField.SetText(string.Format("{0:0.00}", main.GetDouble("temp")), TextView.BufferType.Normal);
                    ((TextView)FindViewById(Resource.Id.current_temperature_field_celcius)).SetText("C", TextView.BufferType.Normal);
                }

                Date d = new Date(json.GetLong("dt") * 1000);
                //SimpleDateFormat sdf = new SimpleDateFormat("dd/M/yyyy");
                SimpleDateFormat sdf = new SimpleDateFormat("dd-M-yyyy hh:mm:ss a");
                updatedField.SetText("Last update: " + sdf.Format(d), TextView.BufferType.Normal);

                JSONObject wind = json.GetJSONObject("wind");
                windField.SetText("Wind Speed: " + wind.GetString("speed") + "Km/H", TextView.BufferType.Normal);
                Date sunrise = new Date(sys.GetLong("sunrise") * 1000);
                SimpleDateFormat sdfSun = new SimpleDateFormat("hh:mm a");
                sunriseField.SetText("Sunrise: " + sdfSun.Format(sunrise) + ",", TextView.BufferType.Normal);
                Date sunset = new Date(sys.GetLong("sunset") * 1000);
                sunsetField.SetText("Sunset: " + sdfSun.Format(sunset), TextView.BufferType.Normal);

                setWeatherIcon(details.GetInt("id"),
                        json.GetJSONObject("sys").GetLong("sunrise") * 1000,
                        json.GetJSONObject("sys").GetLong("sunset") * 1000, weatherIcon);

            }
            catch (Java.Lang.Exception e)
            {
                //Log.e("SimpleWeather", "One or more fields not found in the JSON data");
            }
        }

        private void RenderCiti1(JSONObject json)
        {
            FindViewById(Resource.Id.city1_content_buzy).Visibility = ViewStates.Gone;
            ((TextView)FindViewById(Resource.Id.city_field1)).SetText(json.GetString("name"), TextView.BufferType.Normal);
            JSONObject main = json.GetJSONObject("main");
            ((TextView)FindViewById(Resource.Id.temp_field1)).SetText(string.Format("{0:0.00}", main.GetDouble("temp")), TextView.BufferType.Normal);
            JSONObject details = json.GetJSONArray("weather").GetJSONObject(0);

            setWeatherIcon(details.GetInt("id"),
                        json.GetJSONObject("sys").GetLong("sunrise") * 1000,
                        json.GetJSONObject("sys").GetLong("sunset") * 1000, weather_icon1);

        }
        private void RenderCiti2(JSONObject json)
        {
            FindViewById(Resource.Id.city2_content_buzy).Visibility = ViewStates.Gone;
            ((TextView)FindViewById(Resource.Id.city_field2)).SetText(json.GetString("name"), TextView.BufferType.Normal);
            JSONObject main = json.GetJSONObject("main");
            ((TextView)FindViewById(Resource.Id.temp_field2)).SetText(string.Format("{0:0.00}", main.GetDouble("temp")), TextView.BufferType.Normal);
            JSONObject details = json.GetJSONArray("weather").GetJSONObject(0);

            setWeatherIcon(details.GetInt("id"),
                        json.GetJSONObject("sys").GetLong("sunrise") * 1000,
                        json.GetJSONObject("sys").GetLong("sunset") * 1000, weather_icon2);

        }
        private void RenderCiti3(JSONObject json)
        {
            FindViewById(Resource.Id.city3_content_buzy).Visibility = ViewStates.Gone;
            ((TextView)FindViewById(Resource.Id.city_field3)).SetText(json.GetString("name"), TextView.BufferType.Normal);
            JSONObject main = json.GetJSONObject("main");
            ((TextView)FindViewById(Resource.Id.temp_field3)).SetText(string.Format("{0:0.00}", main.GetDouble("temp")), TextView.BufferType.Normal);
            JSONObject details = json.GetJSONArray("weather").GetJSONObject(0);

            setWeatherIcon(details.GetInt("id"),
                        json.GetJSONObject("sys").GetLong("sunrise") * 1000,
                        json.GetJSONObject("sys").GetLong("sunset") * 1000, weather_icon3);

        }
        private void RenderCiti4(JSONObject json)
        {
            FindViewById(Resource.Id.city4_content_buzy).Visibility = ViewStates.Gone;
            ((TextView)FindViewById(Resource.Id.city_field4)).SetText(json.GetString("name"), TextView.BufferType.Normal);
            JSONObject main = json.GetJSONObject("main");
            ((TextView)FindViewById(Resource.Id.temp_field4)).SetText(string.Format("{0:0.00}", main.GetDouble("temp")), TextView.BufferType.Normal);
            JSONObject details = json.GetJSONArray("weather").GetJSONObject(0);

            setWeatherIcon(details.GetInt("id"),
                        json.GetJSONObject("sys").GetLong("sunrise") * 1000,
                        json.GetJSONObject("sys").GetLong("sunset") * 1000, weather_icon4);

        }
        private void updateWeatherData()
        {
            GetDataForCiti1();
            GetDataForCiti2();
            GetDataForCiti3();
            GetDataForCiti4();
        }

        private void GetDataForCiti1()
        {
            Thread t = new Thread(() =>
            {
                allCityData[0] = IsCitiChanged ? RemoteFetch.GetJSON(this, CitiName):RemoteFetch.GetJSON(this, cp.City1);
                currentSelection = allCityData[0];
                if (allCityData[0] == null)
                {
                    this.RunOnUiThread(() =>
                    {
                        FindViewById(Resource.Id.main_content_buzy).Visibility = ViewStates.Gone;
                        FindViewById(Resource.Id.city1_content_buzy).Visibility = ViewStates.Gone;
                        Toast.MakeText(this,
                                this.GetString(Resource.String.place_not_found),
                                ToastLength.Short).Show();
                        ((TextView)FindViewById(Resource.Id.city_field)).SetText(cp.City1, TextView.BufferType.Normal);
                        ((TextView)FindViewById(Resource.Id.current_temperature_field)).SetText("Please Try Later", TextView.BufferType.Normal);
                        ((TextView)FindViewById(Resource.Id.current_temperature_field_degree)).Visibility = ViewStates.Gone;
                        ((TextView)FindViewById(Resource.Id.current_temperature_field_celcius)).Visibility = ViewStates.Gone;
                        ((TextView)FindViewById(Resource.Id.temp_field1)).SetText("Try Later", TextView.BufferType.Normal);
                        ((TextView)FindViewById(Resource.Id.city_field1)).SetText(cp.City1, TextView.BufferType.Normal);
                    });
                }
                else
                {
                    this.RunOnUiThread(() =>
                    {
                        renderWeather(allCityData[0]);
                        RenderCiti1(allCityData[0]);
                        cp.City1 = allCityData[0].GetString("name");
                    });
                }
                IsCitiChanged = false;
            });
            t.Start();
        }
        private void GetDataForCiti2()
        {
            Thread t = new Thread(() =>
            {
                allCityData[1] = IsCitiChanged ? RemoteFetch.GetJSON(this, CitiName):RemoteFetch.GetJSON(this, cp.City2);
                if (allCityData[1] == null)
                {
                    this.RunOnUiThread(() =>
                    {
                        FindViewById(Resource.Id.city2_content_buzy).Visibility = ViewStates.Gone;
                        Toast.MakeText(this,
                                this.GetString(Resource.String.place_not_found),
                                ToastLength.Short).Show();
                        ((TextView)FindViewById(Resource.Id.temp_field2)).SetText("Try Later", TextView.BufferType.Normal);
                        if (whichCity.Equals("2"))
                        {
                            ((TextView)FindViewById(Resource.Id.city_field)).SetText(cp.City2, TextView.BufferType.Normal);
                            ((TextView)FindViewById(Resource.Id.current_temperature_field)).SetText("Please Try Later", TextView.BufferType.Normal);
                            ((TextView)FindViewById(Resource.Id.current_temperature_field_degree)).Visibility = ViewStates.Gone;
                            ((TextView)FindViewById(Resource.Id.current_temperature_field_celcius)).Visibility = ViewStates.Gone;
                        }
                        ((TextView)FindViewById(Resource.Id.city_field2)).SetText(cp.City2, TextView.BufferType.Normal);
                    });
                }
                else
                {
                    this.RunOnUiThread(() =>
                    {
                        RenderCiti2(allCityData[1]);
                        if (whichCity.Equals("2"))
                            renderWeather(allCityData[1]);
                    //  if (IsCitiChanged)
                    cp.City2= allCityData[1].GetString("name");
                     //   cp.City2 = CitiName;

                    });
                }
                IsCitiChanged = false;
            });
            t.Start();
        }
        private void GetDataForCiti3()
        {
            Thread t = new Thread(() =>
            {
                allCityData[2] = IsCitiChanged ? RemoteFetch.GetJSON(this, CitiName):RemoteFetch.GetJSON(this, cp.City3);
                if (allCityData[2] == null)
                {
                    this.RunOnUiThread(() =>
                    {
                        FindViewById(Resource.Id.city3_content_buzy).Visibility = ViewStates.Gone;
                        Toast.MakeText(this,
                                this.GetString(Resource.String.place_not_found),
                                ToastLength.Short).Show();
                        ((TextView)FindViewById(Resource.Id.temp_field3)).SetText("Try Later", TextView.BufferType.Normal);
                        if (whichCity.Equals("3"))
                        {
                            ((TextView)FindViewById(Resource.Id.city_field)).SetText(cp.City3, TextView.BufferType.Normal);
                            ((TextView)FindViewById(Resource.Id.current_temperature_field)).SetText("Please Try Later", TextView.BufferType.Normal);
                            ((TextView)FindViewById(Resource.Id.current_temperature_field_degree)).Visibility = ViewStates.Gone;
                            ((TextView)FindViewById(Resource.Id.current_temperature_field_celcius)).Visibility = ViewStates.Gone;
                        }
                        ((TextView)FindViewById(Resource.Id.city_field3)).SetText(cp.City3, TextView.BufferType.Normal);
                    });
                }
                else
                {
                    this.RunOnUiThread(() =>
                    {
                        RenderCiti3(allCityData[2]);
                        if (whichCity.Equals("3"))
                            renderWeather(allCityData[2]);
                        //if (IsCitiChanged)
                        //    cp.City3 = CitiName;
                        cp.City3 = allCityData[2].GetString("name");
                    });
                }
                IsCitiChanged = false;
            });
            t.Start();
        }
        private void GetDataForCiti4()
        {
            Thread t = new Thread(() =>
            {
                allCityData[3] = IsCitiChanged ? RemoteFetch.GetJSON(this, CitiName):RemoteFetch.GetJSON(this, cp.City4);
                if (allCityData[3] == null)
                {
                    this.RunOnUiThread(() =>
                    {
                        FindViewById(Resource.Id.city4_content_buzy).Visibility = ViewStates.Gone;
                        Toast.MakeText(this,
                                this.GetString(Resource.String.place_not_found),
                                ToastLength.Short).Show();
                        ((TextView)FindViewById(Resource.Id.temp_field4)).SetText("Try Later", TextView.BufferType.Normal);
                        if (whichCity.Equals("4"))
                        {
                            ((TextView)FindViewById(Resource.Id.city_field)).SetText(cp.City4, TextView.BufferType.Normal);
                            ((TextView)FindViewById(Resource.Id.current_temperature_field)).SetText("Please Try Later", TextView.BufferType.Normal);
                            ((TextView)FindViewById(Resource.Id.current_temperature_field_degree)).Visibility = ViewStates.Gone;
                            ((TextView)FindViewById(Resource.Id.current_temperature_field_celcius)).Visibility = ViewStates.Gone;
                        }
                        ((TextView)FindViewById(Resource.Id.city_field4)).SetText(cp.City4, TextView.BufferType.Normal);
                    });
                }
                else
                {
                    this.RunOnUiThread(() =>
                    {
                        RenderCiti4(allCityData[3]);
                        if (whichCity.Equals("4"))
                            renderWeather(allCityData[3]);
                        cp.City4 = allCityData[3].GetString("name");
                    });
                }
                IsCitiChanged = false;
            });
            t.Start();
        }
        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            menu.Clear();

            MenuInflater.Inflate(Resource.Menu.menu1, menu);
            IMenuItem city1 = menu.FindItem(Resource.Id.change_city1);
            IMenuItem city2 = menu.FindItem(Resource.Id.change_city2);
            IMenuItem city3 = menu.FindItem(Resource.Id.change_city3);
            IMenuItem city4 = menu.FindItem(Resource.Id.change_city4);
            string tempCitiName1 = cp.City1;
            if (tempCitiName1.Length > 14)
                tempCitiName1 = tempCitiName1.Substring(0, 14) + "...";
            string tempCitiName2 = cp.City2;
            if (tempCitiName2.Length > 14)
                tempCitiName2 = tempCitiName2.Substring(0, 14) + "...";
            string tempCitiName3 = cp.City3;
            if (tempCitiName3.Length > 14)
                tempCitiName3 = tempCitiName3.Substring(0, 14) + "...";
            string tempCitiName4 = cp.City4;
            if (tempCitiName4.Length > 14)
                tempCitiName4 = tempCitiName4.Substring(0, 14) + "...";

            city1.SetTitle("Change " + tempCitiName1);
            city2.SetTitle("Change " + tempCitiName2);
            city3.SetTitle("Change " + tempCitiName3);
            city4.SetTitle("Change " + tempCitiName4);

            return true;
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.change_city1)
            {
                whichCity = "1";
                showInputDialog();
            }
            if (item.ItemId == Resource.Id.change_city2)
            {
                whichCity = "2";
                showInputDialog();
            }
            if (item.ItemId == Resource.Id.change_city3)
            {
                whichCity = "3";
                showInputDialog();
            }
            if (item.ItemId == Resource.Id.change_city4)
            {
                whichCity = "4";
                showInputDialog();
            }
            if (item.ItemId == Resource.Id.about)
            {
                Intent aboutIntent = new Intent(this, typeof(AboutActivity));
                StartActivity(aboutIntent);
            }
            if (item.ItemId == Resource.Id.help)
            {
                Intent helpIntent = new Intent(this, typeof(HelpActivity));
                StartActivity(helpIntent);
            }
            return true;
        }


        //   ArrayAdapter<string> adapter;
        AutoCompleteTextView cities;
        private void showInputDialog()
        {
            builder = new AlertDialog.Builder(this);
            cities = new AutoCompleteTextView(this);

            builder.SetTitle("Select new city");
            cities.InputType = InputTypes.ClassText | InputTypes.TextFlagCapWords;
            cities.SetMaxLines(1);
            builder.SetView(cities);
            //  cities.Adapter = adapter;
            builder.SetNegativeButton("Cancel", HandelNegativeButtonClick);
            builder.SetPositiveButton("Change", HandlePositiveButtonClick);

            builder.Show();
        }


        private string[] CITIES;
        private object listener;

        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            CitiName = cities.Text;
            if (!string.IsNullOrWhiteSpace(CitiName))
                changeCity(CitiName);
        }

        private void HandelNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            builder.Dispose();
        }

        public void changeCity(string city)
        {
            FindViewById(Resource.Id.main_content_buzy).Visibility = ViewStates.Visible;
            IsCitiChanged = true;
            if (whichCity.Equals("1"))
            {
                FindViewById(Resource.Id.city1_content_buzy).Visibility = ViewStates.Visible;
                GetDataForCiti1();
            }
            if (whichCity.Equals("2"))
            {
                FindViewById(Resource.Id.city2_content_buzy).Visibility = ViewStates.Visible;
                GetDataForCiti2();
            }
            if (whichCity.Equals("3"))
            {
                FindViewById(Resource.Id.city3_content_buzy).Visibility = ViewStates.Visible;
                GetDataForCiti3();
            }
            if (whichCity.Equals("4"))
            {
                FindViewById(Resource.Id.city4_content_buzy).Visibility = ViewStates.Visible;
                GetDataForCiti4();
            }
        }
        private void setWeatherIcon(int actualId, long sunrise, long sunset, TextView v)
        {
            int id = actualId / 100;
            string icon = "";
            if (actualId == 800)
            {

                long currentTime = DateTime.Now.Ticks;
                if (currentTime >= sunrise && currentTime < sunset)
                {
                    icon = this.GetString(Resource.String.weather_sunny);
                }
                else
                {
                    icon = this.GetString(Resource.String.weather_clear_night);
                }
            }
            else
            {
                switch (id)
                {
                    case 2:
                        icon = this.GetString(Resource.String.weather_thunder);
                        break;
                    case 3:
                        icon = this.GetString(Resource.String.weather_drizzle);
                        break;
                    case 7:
                        icon = this.GetString(Resource.String.weather_foggy);
                        break;
                    case 8:
                        icon = this.GetString(Resource.String.weather_cloudy);
                        break;
                    case 6:
                        icon = this.GetString(Resource.String.weather_snowy);
                        break;
                    case 5:
                        icon = this.GetString(Resource.String.weather_rainy);
                        break;
                }
            }
            v.SetText(icon, TextView.BufferType.Normal);
        }

        private void Citi1_layout_Click(object sender, EventArgs e)
        {
            currentSelection = allCityData[0];
            if (allCityData[0] != null)
            {
                renderWeather(allCityData[0]);
            }
            else
                GetDataForCiti1();
        }
        private void Citi2_layout_Click(object sender, EventArgs e)
        {
            currentSelection = allCityData[1];
            if (allCityData[1] != null)
            {
                renderWeather(allCityData[1]);
            }
            else
                GetDataForCiti2();
        }
        private void Citi3_layout_Click(object sender, EventArgs e)
        {
            currentSelection = allCityData[2];
            if (allCityData[2] != null)
            {
                renderWeather(allCityData[2]);
            }
            else
                GetDataForCiti3();
        }
        private void Citi4_layout_Click(object sender, EventArgs e)
        {
            currentSelection = allCityData[3];
            if (allCityData[3] != null)
            {
                renderWeather(allCityData[3]);
            }
            else
                GetDataForCiti4();
        }
    }
}

