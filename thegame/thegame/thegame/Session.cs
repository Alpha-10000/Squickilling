using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace thegame
{
    public static class Session
    {

        public static string session_password = "";
        public static string session_email = "";
        public static string session_id = "";
        public static string session_name = "";
        public static bool session_isset = false;


        public static void NewSession(string session_id2, string session_email2, string session_password2, string session_name2)
        {
            session_password = session_password2;
            session_email = session_email2;
            session_id = session_id2;
            session_name = session_name2;
            session_isset = true;
        }
    }
}
