using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Data.SqlClient;
using System.Globalization;

namespace thegame
{

    public enum instances_type
    {
        Game,
        Menu
    }

    public class Instances
    {
        public Game1 game { get; private set; }
        KeyboardState keyboardState;
        public instances_type type { get; private set; }
        public object execute { get; private set; }
        public int selected { get; private set; }
        public SoundEffect sound { get; private set; }
        private List<string> Text_Game;
        private Vector2[] coordPlateforme;
        private string[] typePlateforme;
        private SoundEffectInstance instancesound;

        private void GetText(string language)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (language == "english")
            {
                Text_Game = new List<string> { "Play Game", "Options", "Quit", "Language", "Full screen", "Back", "English", "French" };
            }
            else
            {
                Text_Game = new List<string> { "Jouer", "Options", "Quitter", "Langue", "Plein écran", "Retour", "Anglais", "Français" };
            }
        }

        public Instances(Game1 game)
        {
            this.type = instances_type.Menu;
            this.selected = 0;
            if(CultureInfo.InstalledUICulture.ToString() == "fr-FR")
            {
            GetText("french");
            }
            else{
                GetText("english");
            }
            instancesound = Textures.gameSound_Effect.CreateInstance();
            instancesound.IsLooped = true;
            /* Execute by default when loading for the first time */
            execute = new Menu(3, "Squickilling");
            (execute as Menu).AddElements(Text_Game[0]);
            (execute as Menu).AddElements(Text_Game[1]);
            (execute as Menu).AddElements(Text_Game[2]);
            MediaPlayer.Play(Textures.openingSound_Effect);
            this.game = game;
            
        }

    

        public void UpdateByKey(GameTime gametime)
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape)) /* Exit the game */
            {
                game.Exit();
            }

            if (type == instances_type.Menu)
            {
                (execute as Menu).Update(gametime, keyboardState); /* For the design */
                if (this.selected == 0)
                {
                    

                    switch ((execute as Menu).selected)
                    {
                        case 0:
                            if (keyboardState.IsKeyDown(Keys.Enter)) /* Start the game */
                            {
                                (execute as Menu).MenuBool = false;
                                this.type = instances_type.Game;
                                this.selected = 2;
                                Execute();
                                Thread.Sleep(150);
                            }
                            break;
                        case 1:
                            if (keyboardState.IsKeyDown(Keys.Enter)) /* Go to options settings */
                            {
                                this.selected++;
                                Execute();
                                Thread.Sleep(150);
                            }
                            break;
                        case 2:
                            if (keyboardState.IsKeyDown(Keys.Enter)) /* Exit the game */
                            {
                                game.Exit();
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if (selected == 1)
                {
                    switch ((execute as Menu).selected) /* Go back to main Menu */
                    {
                        case 0:
                            if (keyboardState.IsKeyDown(Keys.Enter))
                            {
                                this.selected = 3;
                                Execute();
                                Thread.Sleep(150);
                            }
                            break;
                        case 2:
                            if (keyboardState.IsKeyDown(Keys.Enter))
                            {
                                this.selected--;
                                Execute();
                                Thread.Sleep(150);
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if (selected == 3)
                {
                    
                    switch ((execute as Menu).selected) /* Go back to main Menu */
                    {
                        case 0:
                            if (keyboardState.IsKeyDown(Keys.Enter))
                            {
                                GetText("english");
                                this.selected = 0;
                                Execute();
                                Thread.Sleep(150);
                            }
                            break;
                        case 1:
                            if (keyboardState.IsKeyDown(Keys.Enter))
                            {
                                GetText("french");
                                this.selected = 0;
                                Execute();
                                Thread.Sleep(150);
                            }
                            break;
                        default:
                            break;
                    }
                    
                }
            }
            else
            {
                (execute as Perso).Update(gametime);
                if (keyboardState.IsKeyDown(Keys.Back)) /* Go to options settings */
                {
                    this.selected = 0;
                    instancesound.Stop();
                    this.type = instances_type.Menu;
                    Execute();
                    Thread.Sleep(150);
                }
       
            }

        }


        public void Execute()
        {
            switch (this.selected)
            {
                case 0: /* Create main menu */
                    Textures.buttonSound_Effect.Play();
                    execute = new Menu(3, "Squickilling");
            (execute as Menu).AddElements(Text_Game[0]);
            (execute as Menu).AddElements(Text_Game[1]);
            (execute as Menu).AddElements(Text_Game[2]);
            
                    break;
                case 1: /* Create option menu */
                    Textures.buttonSound_Effect.Play();
                    execute = new Menu(3, Text_Game[1]);
            (execute as Menu).AddElements(Text_Game[3]);
            (execute as Menu).AddElements(Text_Game[4]);
            (execute as Menu).AddElements(Text_Game[5]);
   
                    break;
                case 2: /* Start the game */
                    Textures.buttonSound_Effect.Play();
                    instancesound.Play();
                    coordPlateforme = new Vector2[4] { new Vector2(300, 300), new Vector2(380, 250), new Vector2(450, 200), new Vector2(20, 350) };
                    typePlateforme = new string[4] { "plateforme", "plateforme", "buche", "buche" };
                    execute = new Perso(new Vector2(0, 300));
                    break;
                case 3: /* Start the game */
                    Textures.buttonSound_Effect.Play();
                    execute = new Menu(2, Text_Game[3]);
            (execute as Menu).AddElements(Text_Game[6]);
            (execute as Menu).AddElements(Text_Game[7]);
                    break;
                default:
                    break;
            }
        }

        public void Display(SpriteBatch sb)
        {
            if (type == instances_type.Menu)
            {
                (execute as Menu).Display(sb);
            }
            else
            {
                for (int i = 0; i < coordPlateforme.Length; i++)
                {
                    if (typePlateforme[i] == "plateforme")
                    {
                        new Plateforme(drawable_type.Plateform_default, coordPlateforme[i]).Draw(sb, coordPlateforme[i]);
                    }
                    else
                    {
                        new Plateforme(drawable_type.buche, coordPlateforme[i]).Draw(sb, coordPlateforme[i]);
                    } 
                }
                (execute as Perso).Draw(sb); /* Should be execute in the Drawable class */
            }
        }

    }
}
