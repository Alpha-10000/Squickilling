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

namespace thegame
{
    class Bomb
    {
        private Drawable mine_gray = new Drawable(drawable_type.mine_gray);
        private Drawable mine_red = new Drawable(drawable_type.mine_red);
        Animation animationMine;

        public Rectangle Object;
        private int x;
        private int y;
        private int width;
        private int height;

        public bool activateExplosion = false;
        public bool checkIfFinish = false;

        public float elaspedTime = 0;
        public bool checkBlood = true;

        private Vector2 currentFrame = Vector2.Zero;
        public Bomb(Rectangle Object)
        {
            this.Object = Object;
            x = Object.X;
            y = Object.Y;
            width = Object.Width;
            height = Object.Height;
            animationMine = new Animation(new Vector2(x, y - 1), new Vector2(9, 1));
            animationMine.AnimationSprite = Textures.animation_mine;
        }

        // draw only once the blood screen and health - 5 only once
        public void BloodOnce(ref int health)
        {
            if (checkBlood)
            {
                health -= 5;
                checkBlood = false;
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            if (activateExplosion)
            {
                elaspedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (elaspedTime >= 0)
                {
                    elaspedTime = 0;
                    currentFrame.X += 64;
                    if (currentFrame.X >= 320)
                    {
                        currentFrame.X = 0;
                        currentFrame.Y++;
                    }
                    if (currentFrame.Y > 4)
                    {
                        activateExplosion = false;
                        checkIfFinish = true;
                    }
                    else
                        sb.Draw(Textures.explosion, new Vector2(x - 30, y - 30), new Rectangle((int)currentFrame.X, (int)currentFrame.Y * 64, 64, 64), Color.White);
                }
            }
            else
            {
                animationMine.Update(gameTime);
                animationMine.Draw(sb);
            }
        }
    }
}

