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
        KeyboardState oldkey;
        public instances_type type { get; private set; }
        public object execute { get; private set; }
        public int selected { get; private set; }
        public SoundEffect sound { get; private set; }
        private List<string> Text_Game;
        private SoundEffectInstance instancesound;

        private int[,] tilemap;
        public List<Rectangle> blocks;
        public List<Rectangle> blocksTop;
        public List<Rectangle> blocksLeft;
        public List<Rectangle> blocksRight;
        public List<Rectangle> tile;

        private bool movedown;
        private bool moveleft;
        private bool moveright;
        private Drawable tree;

        private Drawable debug;

         private List<Texture2D>   texlis;
            private int mapSizeX;
           private int mapSizeY;

        private void GetText(string language)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (language == "english")
            {
                Text_Game = new List<string> { "Play", "Options", "Exit", "Language", "Full screen", "Back", "English", "French" };
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

            movedown = true;
            moveleft = true;
            moveright = true;
        }

    

        public void UpdateByKey(GameTime gametime)
        {
            oldkey = keyboardState;
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape)) /* Exit the game */
            {
                game.Exit();
            }

            if (type == instances_type.Menu)
            {
                (execute as Menu).Update(gametime, keyboardState, oldkey); /* For the design */
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
                movedown = true;
                moveleft = true;
                moveright = true;
                (execute as Perso).gravity = 5f;
                foreach (Rectangle top in blocksTop)
                {
                    if (top.Intersects((execute as Perso).hitBoxPerso))
                    {
                        movedown = false;
                        (execute as Perso).gravity = 0f;
                    }
                }

                foreach (Rectangle left in blocksLeft)
                {
                    if (left.Intersects((execute as Perso).hitBoxPerso))
                    {
                        moveleft = false;
                    }
                }

                foreach (Rectangle right in blocksRight)
                {
                    if (right.Intersects((execute as Perso).hitBoxPerso))
                    {
                        moveright = false;
                    }
                }

                
                    (execute as Perso).Update(gametime, keyboardState, oldkey, movedown, moveleft, moveright);

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
                    tilemap = new int[,]
                        {
                            {0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,1,1,1,0,0,1,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,1,1,1,1,1,0,0,1,0,0,1,0,1,1,0,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        };

              
            texlis = new List<Texture2D>();
            mapSizeX = tilemap.GetLength(1);
            mapSizeY = tilemap.GetLength(0);
                    blocks = new List<Rectangle>();
            blocksTop = new List<Rectangle>();
            blocksLeft = new List<Rectangle>();
            blocksRight = new List<Rectangle>();
                    tile = new List<Rectangle>();

                    for (int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
                {
                    if (tilemap[y, x] == 1)
                    {
                        blocks.Add(new Rectangle(x * 65, y * 8 + 330, 65, 8));

                    }
                }
            }

            foreach (Rectangle block in blocks)
            {
                blocksTop.Add(new Rectangle(block.X , block.Y , 65 , 8));
            }
            foreach (Rectangle block in blocks)
            {
                blocksRight.Add(new Rectangle(block.X, block.Y + 3, 1  , 8));
            }
            foreach (Rectangle block in blocks)
            {
                blocksLeft.Add(new Rectangle(block.X + 65, block.Y + 3, 1, 8));
            }


       

                   
                    execute = new Perso(new Vector2(90, 280));

                    tree = new Drawable(drawable_type.tree);


                    movedown = true;
                    moveright = true;
                    moveleft = true;

                    debug = new Drawable(drawable_type.font);
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

               /* for (int i = 0; i < mapSizeX; i++)
                {
                    for (int j = 0; j < mapSizeY; j++)
                    {
                        if (tilemap[j, i] == 1)
                            sb.Draw(Textures.plateform_texture, new Rectangle(i * 65, j * 8 + 260, 65, 8), Color.White);

                    }

                    
                }*/

                foreach (Rectangle top in blocks)
                {
                    sb.Draw(Textures.plateform_texture, top , Color.White);
                }

               /*  foreach (Rectangle top in blocksRight)
                {
                    sb.Draw(Textures.hitbox, top, Color.White);
                } // debug right collision 
                
                 foreach (Rectangle top in blocksLeft)
                 {
                     sb.Draw(Textures.hitbox, top, Color.Red);
                 } // debug left collision 
                */
                debug.Draw(sb, "Down : " + movedown.ToString() + " left " + moveleft.ToString() + " right " + moveright.ToString(), new Vector2(300, 50), Color.White, "normal");
               
                (execute as Perso).Draw(sb); /* Should be execute in the Drawable class */
                tree.Draw(sb, new Vector2(-100, 50));
            }
        }

    }
}
