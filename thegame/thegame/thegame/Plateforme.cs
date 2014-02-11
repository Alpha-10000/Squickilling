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
    class Plateforme : Drawable
    {
        Vector2 positionPlatform;
       // Rectangle hitBoxPlatform;

        protected Texture2D imagePlatform;
        public Texture2D ImagePlatform { get; private set; }
        public Vector2 Position { get; private set; }
        public Plateforme(drawable_type drawable_t ,Vector2 positionPlateform)
            : base(drawable_t)
        {
            this.positionPlatform = positionPlateform;
        }

        public void LoadContent(ContentManager Content, string assetName) /* I didnt know what to do with this so I let this that way. */
        {

            imagePlatform = Content.Load<Texture2D>(assetName);
            Rectangle hitBoxPerso = new Rectangle((int)(positionPlatform.X - imagePlatform.Width / 2), (int)(positionPlatform.Y - imagePlatform.Height / 2), imagePlatform.Width, imagePlatform.Height);
        
        }

     
    }
}
