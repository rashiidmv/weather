using System.Linq;
using Android.Content;
using Java.IO;
using Java.Lang;
using Java.Net;
using Org.Json;

namespace MyWeather
{
    public class RemoteFetch
    {
        private static string OPEN_WEATHER_MAP_API ="http://api.openweathermap.org/data/2.5/weather?q=%s&units=metric";
        public static JSONObject GetJSON(Context context, string city)
        {
            JSONObject data = null;
            try
            {
                URL url = new URL(OPEN_WEATHER_MAP_API.Replace("%s",city));
                HttpURLConnection connection =
                    (HttpURLConnection)url.OpenConnection();
                string s1 = context.GetString(Resource.String.open_weather_maps_app_id);
                connection.AddRequestProperty("x-api-key", s1);
    
                BufferedReader reader = new BufferedReader(
                     new InputStreamReader(connection.InputStream));
                
                StringBuffer json = new StringBuffer(1024);
                string tmp = "";
                while ((tmp = reader.ReadLine()) != null)
                    json.Append(tmp).Append("\n");
                reader.Close();

                data = new JSONObject(json.ToString());
               // data = new JSONObject(connection.Content.ToString());
                
            }
            catch (Java.Lang.Exception e)
            {
                return data;
            }
            return data;
        }
    }
}