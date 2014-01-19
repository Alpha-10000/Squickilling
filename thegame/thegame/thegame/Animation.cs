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
    class Animation
    {
        int frameCounter;
        int switchFrames;
        bool actif;

        private Vector2 position, nbFrames, currentFrame;
        Rectangle SpriteSheet;

        Texture2D Sprite;
        public Texture2D AnimationSprite
        {
            set { Sprite = value; }
        }


        public Vector2 CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool Actif
        {
            get { return actif; }
            set { actif = value; }
        }
        public int FrameWidth
        {
            get { return Sprite.Width / (int)nbFrames.X; }
        
        }
        public int FrameHeight
        {
            get { return Sprite.Height / (int)nbFrames.Y; }
        }


        public void Initialize(Vector2 position, Vector2 Frames)
        {
            actif = false;
            switchFrames = 70;
            this.position = position;
            nbFrames = Frames;
        }

        public void Update(GameTime gameTime)
        {
            if (actif)
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            else
            {
                currentFrame.X = 0;
                frameCounter = 0;
            }

            if (frameCounter >= switchFrames)
            {
                frameCounter = 0;
                currentFrame.X += FrameWidth;
                if (currentFrame.X >= Sprite.Width)
                {
                    currentFrame.X = 0;
                }
                SpriteSheet = new Rectangle((int)currentFrame.X, (int)currentFrame.Y * FrameHeight , FrameWidth, FrameHeight);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, position, SpriteSheet, Color.White);
        }
    }
}
