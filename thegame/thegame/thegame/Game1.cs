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
using System.Windows.Forms;
using X2DPE;
using X2DPE.Helpers;
using System.Globalization;

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

            Map.particleComponent = new ParticleComponent(this);
            MapMulti.particleComponent = new ParticleComponent(this);
            this.Components.Add(Map.particleComponent);
            this.Components.Add(MapMulti.particleComponent);

            this.IsMouseVisible = true;
            IntPtr hWnd = this.Window.Handle;
            var control = System.Windows.Forms.Control.FromHandle(hWnd);
            var form = control.FindForm();
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Content.RootDirectory = "Content";

            
            
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (CultureInfo.InstalledUICulture.ToString() == "fr-FR")
            {
                Language.change("french");
                Instances.language = "french";
            }
            else
            {
                Language.change("english");
                Instances.language = "english";
            }
        }

        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.load(Content);
            instancesobject = new Instances(this);
        }

        protected override void UnloadContent()
        {
            //TODO: Unload any non ContentManager content here
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
