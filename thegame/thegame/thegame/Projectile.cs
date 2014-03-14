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
    class Projectile : Drawable
    {
        int distance = 100;
        bool visible;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        Vector2 positionProjectile;
        Vector2 startPosition;
        float speed;
        Vector2 direction;

        public Projectile(drawable_type drawable_t, Vector2 positionProjectile, Vector2 startPosition, float speed, Vector2 direction)
            : base(drawable_t)
        {
            this.positionProjectile = positionProjectile;
            this.startPosition = startPosition;
            this.speed = speed;
            this.direction = direction;
            visible = true;
        }


        public void Update(GameTime gameTime)
        {
            if (Vector2.Distance(startPosition, positionProjectile) > distance)
            {
                visible = false;
            }

            if (visible == true)
            {
                positionProjectile += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible == true)
            {
                base.Draw(spriteBatch, positionProjectile);
            }
        }



    }
}
