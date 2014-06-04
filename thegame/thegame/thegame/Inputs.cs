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

            Keys[] lastPressedKeys = prevKeyBState.GetPressedKeys();
            Keys[] pressedKeys = curKeyBState.GetPressedKeys();

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

        static public bool isKeyDown(Keys k) { return curKeyBState.IsKeyDown(k); }

        static public bool isKeyRelease(Keys k) { return curKeyBState.IsKeyDown(k) && !prevKeyBState.IsKeyDown(k); }

        static public bool isKeysUP(Keys k) { return curKeyBState.IsKeyUp(k); }

        static public bool isLMBClick() { return curMouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed;}

        static public bool isRightClick(){ return curMouseState.RightButton == ButtonState.Released && prevMouseState.RightButton == ButtonState.Pressed; }
    }
}

