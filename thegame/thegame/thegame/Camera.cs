using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace thegame
{
    class Camera
    {
        public Camera()
        {
            Position = Vector2.Zero;
            Zoom = 1f;
        }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }

        public Random random = new Random();

        public bool shake = false;

        public Matrix Shake()
        {
            if (shake)
            {
                return Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom) *
                       Matrix.CreateTranslation(Position.X + (float)(random.Next(-18, 18)), Position.Y + (float)(random.Next(-18, 18)), 0);
            }
            else
            {
                return Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom) *
                       Matrix.CreateTranslation(Position.X, Position.Y, 0);
            }

        }

        public Matrix TransformMatrix
        {
            get
            {
                return Shake();
            }
        }
    }
}
