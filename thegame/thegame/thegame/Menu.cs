﻿using System;
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
    public enum menu {
      START_GAME = 0,
      OPTION_SETTINGS,
      EXIT_GAME,
    };

    public class Menu : Drawable
    {
        public bool MenuBool = true;
        public string[] tab { get; private set; } //nom des élements du menu
        public int size { get; private set; }
        public int pos_tab { get; private set; }
        public Color[] color_tab { get; private set; }
        public int selected { get; set; }
        public Color defaultColor { get; private set; }
        public string Text { get; private set; }

        private List<Rectangle> AreaListMouse = new List<Rectangle>();//List that contains rectangle for text element in order to detect the mouse
        private List<Rectangle> CenterText = new List<Rectangle>();
        private int x = 400;
        private int y = 150;

        private int YExcavator = 140;

        private Color change_Color = Color.OrangeRed;


        public bool activateBackSpace = false;

        public bool IChooseSomething = false;
        

        public Menu(int size, string Text)
            : base(drawable_type.font)
        {
            this.size = size;
            tab = new string[this.size];
            this.pos_tab = 0;
            this.selected = 0;
            color_tab = new Color[this.size];
            this.defaultColor = Color.White;
            this.Text = Text;
           
        }

        public void AddElements(string Text)
        {
            this.tab[pos_tab] = Text;
            if (selected == pos_tab)
                this.color_tab[pos_tab] = change_Color;
            else
                this.color_tab[pos_tab] = defaultColor;
            this.pos_tab++;

            int width = (int)Textures.font_texture.MeasureString(Text).X + 25;
            int height = Textures.font_texture.LineSpacing + 25;
            AreaListMouse.Add(new Rectangle(0, y - 10, 800, height));
            CenterText.Add(new Rectangle(x - 10, y - 10, width, height));


            y = 60 + y;
            
        }

        public void Display(SpriteBatch sb, bool developperMode)
        {
            int i = 0;
            int x = 400;    //correspond to the center
            int y = 150;

            // Draw Menu Background Here
            if(!Game1.graphics.IsFullScreen)
                sb.Draw(Textures.menu_main_page, new Vector2(0,0),Color.White);
            else 
                sb.Draw(Textures.menu_main_page, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth+40,Game1.graphics.PreferredBackBufferHeight+5), Color.White);

            sb.Draw(Textures.white_tree, new Vector2(195, 100),null, Color.Black * 0.6f,0f,Vector2.Zero,0.70f,SpriteEffects.None,0f);

            Drawable squirrel = new Drawable(drawable_type.Squirrel);
            squirrel.Draw(sb, new Vector2(630, 120));

            Drawable excator = new Drawable(drawable_type.excavatorArm);
            excator.Draw(sb, new Vector2(0, YExcavator));

            while (i < this.pos_tab && tab[i] != null)
            {
                this.Draw(sb, tab[i], new Vector2(x - CenterText[i].Width / 2, y), color_tab[i], "menu");
                y = 60 + y;
                i++;
            }

            this.Draw(sb, Text, new Vector2(400 - ((int)Textures.fontTitle_texture.MeasureString(Text).X + 25) / 2, 20), Color.Black, "titre");

            if(developperMode)
                foreach (Rectangle dessine in AreaListMouse)
                    sb.Draw(Textures.hitbox, dessine, Color.White * 0.5f);
        }

        public void Update(GameTime gametime, bool SoundIs)
        {
            
            bool MouseOnSOmething = false;
            int index = 0;

            if (Inputs.UseMouse())
            {
                foreach (Rectangle check in AreaListMouse)
                    if (check.Contains(Inputs.getMousePoint()))
                    {
                        index = AreaListMouse.FindIndex(x => x == check);
                        YExcavator = 140 + index * 60;
                        MouseOnSOmething = true;
                        if (Inputs.isLMBClick())
                            IChooseSomething = true;
                        break;
                    }

                if (MouseOnSOmething)
                {
                    for (int i = 0; i < color_tab.GetLength(0); i++)
                        color_tab[i] = defaultColor;
                    color_tab[index] = change_Color;
                    
                    if (SoundIs && this.selected != index)
                    {
                        Textures.buttonSound_Effect.Play();

                    }
                    this.selected = index;
                }
            }
            else
            {
                if (Inputs.isKeyRelease(Keys.Down))
                {
                    if (this.selected < this.color_tab.Length - 1)
                    {
                        this.selected++;
                        this.color_tab[this.selected] = change_Color;
                        YExcavator = 140 + selected * 60;
                        this.color_tab[this.selected - 1] = this.defaultColor;
                        if (SoundIs) Textures.buttonSound_Effect.Play();
                    }
                }
                if (Inputs.isKeyRelease(Keys.Up))
                {
                    if (this.selected >= 1)
                    {
                        this.color_tab[this.selected] = this.defaultColor;
                        this.selected--;
                        this.color_tab[this.selected] = change_Color;
                        YExcavator = 140 + selected * 60;
                        if (SoundIs)
                            Textures.buttonSound_Effect.Play();
                    }
                }

                if (Inputs.isKeyRelease(Keys.Enter)) IChooseSomething = true;

                if (Inputs.isKeyRelease(Keys.Back) && activateBackSpace)
                {
                    IChooseSomething = true;
                    selected = this.color_tab.Length - 1;
                }
            }   
        }
    }
}
