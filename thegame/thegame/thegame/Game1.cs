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
using System.Threading;
using X2DPE;
using X2DPE.Helpers;

namespace thegame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Instances instancesobject;

        public int width;
        public int height;

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 520;
            //this.graphics.PreferredBackBufferWidth = 800;

            width = graphics.PreferredBackBufferWidth;
            height = graphics.PreferredBackBufferHeight;

            Instances.particleComponent = new ParticleComponent(this);
            this.Components.Add(Instances.particleComponent);
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
            GraphicsDevice.Clear(Color.Beige);
                instancesobject.Display(spriteBatch, gameTime);
            
            base.Draw(gameTime);
        }

   


    }
}
