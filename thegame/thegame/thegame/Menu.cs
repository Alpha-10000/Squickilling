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
using System.Threading;

namespace thegame
{
    public class Menu : Drawable
    {
        public bool MenuBool = true;
     
        public string[] tab { get; private set; }
        public int size { get; private set; }
        public int pos_tab { get; private set; }
        public Color[] color_tab { get; private set; }
        public int selected { get; set; }
        public Color defaultColor { get; private set; }

        public Menu(int size) : base(drawable_type.font)
        {
            this.size = size;
            tab = new string[this.size];
            this.pos_tab = 0;
            this.selected = 0;
            color_tab = new Color[this.size];
            this.defaultColor = Color.Black;
        }

        public void AddElements(string Text)
        {
            
            this.tab[pos_tab] = Text;
            if (selected == pos_tab)
            {
                this.color_tab[pos_tab] = Color.Blue;
            }
            else
            {
                this.color_tab[pos_tab] = defaultColor;
            }
            this.pos_tab++;
        }

        public void Display(SpriteBatch sb)
        {
            int i = 0;
            int x = 0;
            int y = 0;
            while (i < this.pos_tab && tab[i] != null)
            {
                this.Draw(sb, tab[i], new Vector2(100 + x, 200 + y), color_tab[i]);
                x = 70 + x;
                y = 60 + y;
                i++;
            }
        }

        

       
     

    }
}
