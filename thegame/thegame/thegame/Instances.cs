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

        public Instances(instances_type type)
        {
            this.type = type;
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
                        if (keyboardState.IsKeyDown(Keys.Enter))
                        {
                            (execute as Menu).MenuBool = false;
                            Thread.Sleep(100);
                        }
                        break;
                    case 1:
                        if (keyboardState.IsKeyDown(Keys.Enter))
                        {
                            this.selected++;
                            Execute();
                            Thread.Sleep(100);
                        }
                        break;
                    default:
                        break;
                }

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
            else
            {
          
            switch ((execute as Menu).selected)
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
                case 0:
                    execute = new Menu(3);
            (execute as Menu).AddElements("Play Game");
            (execute as Menu).AddElements("Options");
            (execute as Menu).AddElements("Quit");
            
                    break;
                case 1:
                    execute = new Menu(3);
                    (execute as Menu).AddElements("Language");
                    (execute as Menu).AddElements("Full screen");
                    (execute as Menu).AddElements("Back");
   
                    break;
                default:
                    break;
            }
        }

        public void Display(SpriteBatch sb)
        {
            (execute as Menu).Display(sb);
        }

    }
}
