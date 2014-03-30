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
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Instances instancesobject;

        Camera camera = new Camera();
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //this.graphics.PreferredBackBufferHeight = 500;
            //this.graphics.PreferredBackBufferWidth = 800;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.load(Content);
            instancesobject = new Instances(this);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            instancesobject.UpdateByKey(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            camera.Position = instancesobject.cameraPos;
            GraphicsDevice.Clear(Color.Beige);
            if (instancesobject.selected == 2)//for the fixed background. I had to do that
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Textures.background, Vector2.Zero, Color.White * 0.9f);
                spriteBatch.End();
            }

            if (instancesobject.pause)
            {
                spriteBatch.Begin();
                instancesobject.Display(spriteBatch);
                spriteBatch.End();
            }
            else
            {

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null,
                       camera.TransformMatrix);
                instancesobject.Display(spriteBatch);
                spriteBatch.End();

            }
            base.Draw(gameTime);
        }

   


    }
}
