using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace thegame
{
    class Button
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;

        Color colour = new Color(255, 255, 255, 255);

        bool down;
        public bool isClicked;

        public Button()
        {
    
        }

        public void Load(Texture2D newTexture, Vector2 newPosition)
        { 
            texture = newTexture;
            position = newPosition;
        }

        public void Update(MouseState mouse)
        {
            mouse = Mouse.GetState();

            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                if (colour.A == 255) down = false;
                if (colour.A == 0) down = true;
                if (down) colour.A += 3; else colour.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                    colour.A = 255;
                }
                
            }
            else if (colour.A < 255)
                    colour.A += 3;

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, colour);
        }
    }
}
