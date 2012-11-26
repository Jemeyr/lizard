using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace lizard.world.block
{
    class BlockTexture
    {
        public static Texture2D[] textures = new Texture2D[32];
        private static int textureCount = 0;

        public static void addTexture(Texture2D tex)
        {
            textures[textureCount++] = tex;
        }
    }
}
