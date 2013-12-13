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
    class Perso : Sprite
    {
        public Rectangle hitBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }
        public override void Update(KeyboardState keyboardState, MouseState mouseState)
        {
            speed = 0.5f;
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                position.X--;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                position.X++;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                position.Y--;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                position.Y++;
            }
        }
    }
}
