using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace thegame
{
    class Inputs
    {
        static private MouseState curMouseState = new MouseState();
        static private KeyboardState curKeyBState = new KeyboardState();
        static private MouseState prevMouseState = new MouseState();
        static private KeyboardState prevKeyBState = new KeyboardState();
        static private bool usemouse;
        static bool keypress;
        static private Keys[] lastPressedKeys;
        public static Keys[] pressedKeys;

        public Inputs()
        {
            usemouse = false;
            keypress = false;
        }
        static public void upDate()
        {
            prevMouseState = curMouseState;
            prevKeyBState = curKeyBState;

            curMouseState = Mouse.GetState();
            curKeyBState = Keyboard.GetState();

            lastPressedKeys = prevKeyBState.GetPressedKeys();
            pressedKeys = curKeyBState.GetPressedKeys();

            keypress = false;
            //check if the currently pressed keys were already pressed
            foreach (Keys key in pressedKeys)
                if (!lastPressedKeys.Contains(key))
                    keypress = true;

      

            if (curMouseState != prevMouseState)
                usemouse = true;

            if (curKeyBState != prevKeyBState)
                usemouse = false;
        }

        static public Vector2 getMousePos() {  return new Vector2(curMouseState.X, curMouseState.Y); }

        static public Point getMousePoint() { return new Point(curMouseState.X, curMouseState.Y); }

        static public bool UseMouse(){ return usemouse; }

        static public bool AnyKeyPressed() { return keypress; }

        static public string getInputKey()
        {
            if(pressedKeys.Count() > 0 && AnyKeyPressed())
            {
                if ((pressedKeys.Contains(Keys.LeftShift) || pressedKeys.Contains(Keys.RightShift)) && pressedKeys.Contains(Keys.D2))//english qwerty keyboard
                    return "@";

                    Keys thefirst = pressedKeys[0];
                    switch (thefirst)
                    {
                        case Keys.A:
                            return "a";
                        case Keys.B:
                            return "b";
                        case Keys.C:
                            return "c";
                        case Keys.D:
                            return "d";
                        case Keys.E:
                            return "e";
                        case Keys.F:
                            return "f";
                        case Keys.G:
                            return "g";
                        case Keys.H:
                            return "h";
                        case Keys.I:
                            return "i";
                        case Keys.J:
                            return "j";
                        case Keys.K:
                            return "k";
                        case Keys.L:
                            return "l";
                        case Keys.M:
                            return "m";
                        case Keys.N:
                            return "n";
                        case Keys.O:
                            return "o";
                        case Keys.P:
                            return "p";
                        case Keys.Q:
                            return "q";
                        case Keys.R:
                            return "r";
                        case Keys.S:
                            return "s";
                        case Keys.T:
                            return "t";
                        case Keys.U:
                            return "u";
                        case Keys.V:
                            return "v";
                        case Keys.W:
                            return "w";
                        case Keys.X:
                            return "x";
                        case Keys.Y:
                            return "y";
                        case Keys.Z:
                            return "z";
                        case Keys.OemPeriod:
                            return ".";
                        case Keys.Attn:
                            return "@";
                        default:
                            return "";
                    
                }
        }
            
            return "";
        
        }

        static public bool isKeyDown(Keys k) { return curKeyBState.IsKeyDown(k); }

        static public bool isKeyRelease(Keys k) { return curKeyBState.IsKeyDown(k) && !prevKeyBState.IsKeyDown(k); }

        static public bool isKeysUP(Keys k) { return curKeyBState.IsKeyUp(k); }

        static public bool isLMBClick() { return curMouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed;}

        static public bool isClickPressed() { return curMouseState.LeftButton == ButtonState.Pressed; }

        static public bool isRightClick(){ return curMouseState.RightButton == ButtonState.Released && prevMouseState.RightButton == ButtonState.Pressed; }
    }
}

