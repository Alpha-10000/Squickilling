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

namespace thegame
{

    public enum instances_type
    {
        Game,
        Menu
    }

    public class Instances
    {
        KeyboardState keyboardState;
        public instances_type type { get; private set; }
        public object execute { get; private set; }
        public int selected { get; private set; }
        public SoundEffect sound { get; private set; }

        public Instances()
        {
            this.type = instances_type.Menu;
            this.selected = 0;

            /* Execute by default when loading for the first time */
            execute = new Menu(3);
            (execute as Menu).AddElements("Play Game");
            (execute as Menu).AddElements("Options");
            (execute as Menu).AddElements("Quit");
            MediaPlayer.Play(Textures.openingSound_Effect);

        }

    

        public void UpdateByKey(GameTime gametime)
        {
            keyboardState = Keyboard.GetState();

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
                        default:
                            break;
                    }
                }
                else if (selected == 1)
                {
                    switch ((execute as Menu).selected) /* Go back to main Menu */
                    {
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
               
            }
            else
            {
                (execute as Perso).Update(gametime);
            }

        }


        public void Execute()
        {
            switch (this.selected)
            {
                case 0: /* Create main menu */
                    Textures.buttonSound_Effect.Play();
                    execute = new Menu(3);
            (execute as Menu).AddElements("Play Game");
            (execute as Menu).AddElements("Options");
            (execute as Menu).AddElements("Quit");
            
                    break;
                case 1: /* Create option menu */
                    Textures.buttonSound_Effect.Play();
                    execute = new Menu(3);
                    (execute as Menu).AddElements("Language");
                    (execute as Menu).AddElements("Full screen");
                    (execute as Menu).AddElements("Back");
   
                    break;
                case 2: /* Start the game */
                    Textures.buttonSound_Effect.Play();
                    execute = new Perso(new Vector2(0, 300));
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
                (execute as Perso).Draw(sb); /* Should be execute in the Drawable class */
            }
        }

    }
}
