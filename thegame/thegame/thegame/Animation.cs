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

        private Vector2 currentFrame;
        public Vector2 Position { get; set; }
        public Vector2 nbFrames { get; set; }
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

        public Animation(Vector2 Position, Vector2 nbframes)
        {
            actif = true;
            switchFrames = 80;
            this.Position = Position;
            this.nbFrames = nbframes;
        }

        public void Update(GameTime gameTime)
        {
            if (actif)
            {
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                currentFrame.X = 0;
                frameCounter = 0;
                SpriteSheet = new Rectangle((int)currentFrame.X, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
            }

            if (frameCounter >= switchFrames)
            {
                frameCounter = 0;
                currentFrame.X += FrameWidth;
                if (currentFrame.X > Sprite.Width - FrameWidth)
                    currentFrame.X = 0;
                SpriteSheet = new Rectangle((int)currentFrame.X, (int)currentFrame.Y * FrameHeight , FrameWidth, FrameHeight);
            }
        }

        public void Draw(SpriteBatch spriteBatch, bool LastMovewasRight)//to place the sprite at the right position when using right/left
        {
            if((int)currentFrame.Y == 0)
                spriteBatch.Draw(Sprite, new Vector2(Position.X - Textures.mario_texture.Width / 16, Position.Y - 5), SpriteSheet, Color.White);
            else
                spriteBatch.Draw(Sprite, new Vector2(Position.X - 5, Position.Y - 5), SpriteSheet, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, SpriteSheet, Color.White);
        }
    }
}
