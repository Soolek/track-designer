using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace track_designer
{
    public static class Extensions
    {
        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}
