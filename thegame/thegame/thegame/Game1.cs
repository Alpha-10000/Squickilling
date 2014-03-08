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
        public Instances instancesobject;
        
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
            if (!Instances.isIntroDone)
            {
                Drawable.vidTexture = Textures.vidPlayer.GetTexture();
                spriteBatch.Begin();
                spriteBatch.Draw(Drawable.vidTexture, Textures.vidRectangle, Color.White);
                spriteBatch.End();
            }

                if (instancesobject.type == instances_type.Game && Instances.isIntroDone)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(Textures.background, Vector2.Zero, Color.White*0.9f);
                    instancesobject.Display(spriteBatch);
                    spriteBatch.End();
                }
                else if (instancesobject.type == instances_type.Menu && Instances.isIntroDone)
                {
                    GraphicsDevice.Clear(Color.Beige);
                    spriteBatch.Begin();
                    instancesobject.Display(spriteBatch);
                    spriteBatch.End();
                }
           
            base.Draw(gameTime);
        }

   


    }
}
