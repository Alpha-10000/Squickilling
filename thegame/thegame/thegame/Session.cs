using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Net;
using System.Collections.Specialized;

namespace thegame
{
    public static class Session
    {

        public static string session_password = "";
        public static string session_email = "";
        public static string session_id = "";
        public static string session_name = "";
        public static bool session_isset = false;

        private static float TimeIntervalRequest = 0;
        private static float TimeInterval = 10000;//10 seconds
        private static bool finish = true;


        public static void NewSession(string session_id2, string session_email2, string session_password2, string session_name2)
        {
            session_password = session_password2;
            session_email = session_email2;
            session_id = session_id2;
            session_name = session_name2;
            session_isset = true;
            TimeIntervalRequest = TimeInterval;
        }

        public static void update(GameTime gametime)
         {

            TimeIntervalRequest += gametime.ElapsedGameTime.Milliseconds;

            if (finish && TimeIntervalRequest >= TimeInterval)
            {
                try
                {
                    finish = false;
                    TimeIntervalRequest = 0;
                    WebClient wb = new WebClient();
                    var data = new NameValueCollection();
                    wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted);
                    data["pwd"] = Session.session_password;
                    data["id"] = Session.session_id;
                    wb.UploadValuesAsync(new Uri("http://squickilling.com/json/update.php"), "POST", data);
                }
                catch
                {
                    //TODO
                }
            }
        }

        static void client_UploadFileCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {

                if (e.Result != null)
                    finish = true;
            }
            catch
            {

            }
        
         }
    }
}
