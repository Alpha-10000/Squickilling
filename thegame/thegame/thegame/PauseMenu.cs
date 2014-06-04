using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace thegame
{
    class PauseMenu : Drawable
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

        private int currentHeight = 0;
        private int currentWidth = 0;

        private Texture2D background;
        private Texture2D ground_texture;

        public PauseMenu(): base(drawable_type.font)
        {
            tab = new string[3];
            this.pos_tab = 0;
            this.selected = 0;
            color_tab = new Color[3];
            this.defaultColor = Color.Black;
            this.Text = Text;
        
            AddElements(Language.Text_Game["_btnPlay"]);
            AddElements(Language.Text_Game["_btnMenu"]);
            AddElements(Language.Text_Game["_btnQuit"]);
        }

        public void Display(SpriteBatch sb, gameState thegamestate)
        {
            switch (thegamestate)
            {
                case gameState.AutumnLevel:
                    background = Textures.autumnBackground;
                    ground_texture = Textures.autumn_ground_texture;
                    break;
                default:
                    background = Textures.winterBackground;
                    ground_texture = Textures.winter_ground_texture;
                    break;

            }
                    sb.Begin();
                    sb.Draw(background, new Vector2(-2, -2), Color.White);
                    sb.Draw(ground_texture, new Vector2(0, 405), Color.White);
                    sb.Draw(ground_texture, new Vector2(790, 405), Color.White);
                    sb.Draw(Textures.pausedTexture, Textures.pausedRectangle, Color.White);

                    foreach (Rectangle dessine in CenterText)
                    {
                        sb.Draw(Textures.hitbox, new Rectangle(dessine.X - currentWidth + 14, dessine.Y + 4, currentWidth * 2 - 8, currentHeight - 8), Color.Beige);
                        sb.Draw(Textures.hitbox, new Rectangle(dessine.X - currentWidth + 10, dessine.Y, 4, currentHeight), Color.Black);//left
                        sb.Draw(Textures.hitbox, new Rectangle(dessine.X + currentWidth + 6, dessine.Y, 4, currentHeight), Color.Black);//right
                        sb.Draw(Textures.hitbox, new Rectangle(dessine.X - currentWidth + 10 , dessine.Y + currentHeight - 4, currentWidth*2, 4), Color.Black);//bottom
                        sb.Draw(Textures.hitbox, new Rectangle(dessine.X - currentWidth + 10, dessine.Y, currentWidth*2, 4), Color.Black);//bottom
                    }

                    int i = 0;
                    int x = 400;//correspond to the center
                    int y = 150;

                    while (i < this.pos_tab && tab[i] != null)
                    {
                        this.Draw(sb, tab[i], new Vector2(x - CenterText[i].Width / 2 + 10, y + 2), color_tab[i], "menu");
                        y = 80 + y;
                        i++;
                    }
                    sb.End();

        }

        public void Update(GameTime gametime, bool SoundIsTrue)
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
                        if (SoundIsTrue)
                            Textures.buttonSound_Effect.Play();
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
                        if (SoundIsTrue)
                            Textures.buttonSound_Effect.Play();
                    }
                }

                if (Inputs.isKeyRelease(Keys.Enter))
                    IChooseSomething = true;

                if (Inputs.isKeyRelease(Keys.Back) && activateBackSpace)
                {
                    IChooseSomething = true;
                    selected = this.color_tab.Length - 1;
                }

            }
        }

        private void AddElements(string Text)
        {
            this.tab[pos_tab] = Text;
            if(pos_tab == 0)
                this.color_tab[pos_tab] = change_Color;
            else
                this.color_tab[pos_tab] = defaultColor;
            this.pos_tab++;

            int width = (int)Textures.font_texture.MeasureString(Text).X + 25;
            int height = Textures.font_texture.LineSpacing + 25;
            AreaListMouse.Add(new Rectangle(0, y - 10, 800, height));
            CenterText.Add(new Rectangle(x - 10, y - 10, width, height));

            foreach (Rectangle dessine in CenterText)
            {
                if (dessine.Width > currentWidth)
                    currentWidth = dessine.Width;
                if (dessine.Height > currentHeight)
                    currentHeight = dessine.Height;
            }

            y = 80 + y;

        }
    
    }
}
