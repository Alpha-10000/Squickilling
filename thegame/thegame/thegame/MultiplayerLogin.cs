using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace thegame
{
    class MultiplayerLogin
    {

        /* $con=mysqli_connect("db506882976.db.1and1.com","dbo506882976","5:#%8!_0|+%T1e7","db506882976");

if (mysqli_connect_errno())
  {
  echo "Failed to connect to MySQL: " . mysqli_connect_error();
  }
         * */

        private string text = "" ;

        public MultiplayerLogin()
        {
            string cs = "server=db506882976.db.1and1.com;database=db506882976;uid=dbo506882976;pwd=epitapower94";

            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                text = "MySQL version : ca marche ";

            }
            catch (MySqlException ex)
            {
                text = "Error connection";

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }


        public void Update()
        {

        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();
            sb.DrawString(Textures.font_texture, "essai log " + text, new Vector2(20, 20), Color.Black);
            sb.End();
        }

    }
}
