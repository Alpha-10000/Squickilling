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

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom) *
                       Matrix.CreateTranslation(Position.X, Position.Y, 0);
            }
        }
    }
}
