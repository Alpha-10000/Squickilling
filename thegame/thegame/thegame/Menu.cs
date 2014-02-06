using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace thegame
{
    class Menu : Drawable
    {
        public bool MenuBool = true;
        KeyboardState keyboardState;

        public Menu() : base(drawable_type.font)
        {

        }
        public void Update(GameTime gametime)
        {
            keyboardState = Keyboard.GetState();


                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    MenuBool = false;
                }


            }

     

    }
}
