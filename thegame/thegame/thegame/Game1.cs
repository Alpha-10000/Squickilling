using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace thegame
{
    

   

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        float frameRate;
        private SpriteFont _font;

        Perso mario = new Perso();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
          //  this.graphics.PreferredBackBufferHeight = 500;
          //  this.graphics.PreferredBackBufferWidth = 800;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            mario.Initialize();
            frameRate = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mario.LoadContent(Content, "mario", 0, 300);
            _font = Content.Load<SpriteFont>("FPS");
           /* if (mario.hitBoxPerso.Intersects(blockBounds) || !GraphicsDevice.Viewport.Bounds.Contains(ballBounds))
            {

                //we have a simple collision!
                //if it has hit, swap the direction of the ball, and update it's position
                ballVelocity = -ballVelocity;
                ballPosition += ballVelocity;

            }

            else
            {

                //move the ball a bit
                ballPosition += ballVelocity;

            }

            //update bounding boxes
            ballBounds.X = (int)ballPosition.X;
            ballBounds.Y = (int)ballPosition.Y;

            blockBounds.X = (int)blockPosition.X;
            blockBounds.Y = (int)blockPosition.Y;*/
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            mario.Update(gameTime);
            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            mario.Draw(spriteBatch);
            spriteBatch.DrawString(_font, "FPS : " + frameRate.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
