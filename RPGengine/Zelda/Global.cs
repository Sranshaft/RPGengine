using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda
{
    public class Global
    {
        public static float DEBUG_FPS = 0f;

        public const int GAME_WIDTH = 640;
        public const int GAME_HEIGHT = 512;

        public const float PLAYER_MOVEMENT = 8f;

        public static Vector2 MAP_POSITION = Vector2.Zero;

        public const float NPC_MOVEMENT = 1f;

        public static Texture2D SHADOW_TEXTURE;
        public static Rectangle SHADOW_RECTANGLE;

        public const int TILE_SIZE = 64;

        public static string WINDOW_TITLE = "RPG Engine";
        public static int WINDOW_WIDTH = 640;
        public static int WINDOW_HEIGHT = 600;
    }
}
