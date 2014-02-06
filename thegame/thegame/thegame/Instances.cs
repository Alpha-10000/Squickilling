using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

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

        public Instances()
        {
            this.type = instances_type.Menu;
            this.selected = 0;

            execute = new Menu(3);
            (execute as Menu).AddElements("Play Game");
            (execute as Menu).AddElements("Options");
            (execute as Menu).AddElements("Quit");
        }

        public void Selected(int i)
        {
            this.selected = this.selected + i;
        }

        public void UpdateByKey(GameTime gametime)
        {
            keyboardState = Keyboard.GetState();

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
                            Thread.Sleep(100);
                        }
                        break;
                    case 1:
                        if (keyboardState.IsKeyDown(Keys.Enter)) /* Go to options settings */
                        {
                            this.selected++;
                            Execute();
                            Thread.Sleep(100);
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
                            Thread.Sleep(100);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                (execute as Perso).Update(gametime);
            }

            if(type == instances_type.Menu)
            {
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    if ((execute as Menu).selected < (execute as Menu).color_tab.Length - 1)
                    {
                        (execute as Menu).selected++;
                        (execute as Menu).color_tab[(execute as Menu).selected] = Color.Blue;
                        (execute as Menu).color_tab[(execute as Menu).selected - 1] = (execute as Menu).defaultColor;
                        Thread.Sleep(100);
                    }

                }
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    if ((execute as Menu).selected >= 1)
                    {
                        (execute as Menu).color_tab[(execute as Menu).selected] = (execute as Menu).defaultColor;
                        (execute as Menu).selected--;
                        (execute as Menu).color_tab[(execute as Menu).selected] = Color.Blue;
                        Thread.Sleep(100);

                    }

                }
            }

        }


        public void Execute()
        {
            switch (this.selected)
            {
                case 0: /* Create main menu */
                    execute = new Menu(3);
            (execute as Menu).AddElements("Play Game");
            (execute as Menu).AddElements("Options");
            (execute as Menu).AddElements("Quit");
            
                    break;
                case 1: /* Create option menu */
                    execute = new Menu(3);
                    (execute as Menu).AddElements("Language");
                    (execute as Menu).AddElements("Full screen");
                    (execute as Menu).AddElements("Back");
   
                    break;
                case 2: /* Start the game */
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
