using System;
using System.Collections.Generic;
using System.Text;
using UntitledGame.EntityManagement;
using Microsoft.Xna.Framework;

namespace UntitledGame.World.EntityComponents
{
    struct BoundsComponent : IComponent
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }


        public Vector2 Position
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Vector2 Size
        {
            get => new Vector2(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }


        public BoundsComponent(float x, float y)
            : this(x, y, 1.0F, 1.0F)
        {

        }

        public BoundsComponent(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle GetBoundingRectangle()
        {
            return new Rectangle(Position.ToPoint(), Size.ToPoint());
        }
    }
}
