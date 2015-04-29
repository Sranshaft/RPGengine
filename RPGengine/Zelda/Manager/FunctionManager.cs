using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Manager
{
    public static class FunctionManager
    {
        private static Random _rnd = new Random();

        public static int Random(int min, int max)
        {
            return _rnd.Next(min, max);
        }

        public static double GetDistance(Vector2 originPos, Vector2 destinationPos)
        {
            var x = Math.Pow(originPos.X - destinationPos.X, 2);
            var y = Math.Pow(originPos.Y - destinationPos.Y, 2);

            return Math.Sqrt(x + y);
        }
    }
}
