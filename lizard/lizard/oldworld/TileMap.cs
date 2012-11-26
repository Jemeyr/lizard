using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lizard.oldworld
{
    class TileMap
    {
        public const int TILESIZE = 64;
        public static int worldSize;

        public World world;

        public Vector2 vpSize;
        public static Tile[,] tiles;
        public static Texture2D[] textures = new Texture2D[16];
        private static int texCount = 0;

        public TileMap(int size, Vector2 vpSize, World world)
        {
            this.world = world;

            this.vpSize = vpSize;
            worldSize = size;

            tiles = new Tile[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    tiles[i, j] =  new Tile((i+j)%8 <4?3:2, new Vector2(i*TILESIZE, j*TILESIZE), world);
                }
            }
        }

        public static Tile getTileAtPos(Vector2 position)
        {
            Vector2 pos = position;
            pos /= TileMap.TILESIZE;

            pos.X = MathHelper.Clamp(pos.X, 0, TileMap.TILESIZE * (TileMap.worldSize - 1));
            pos.Y = MathHelper.Clamp(pos.Y, 0, TileMap.TILESIZE * (TileMap.worldSize - 1));

            return TileMap.tiles[(int)pos.X, (int)pos.Y];
        }

        public static void addTexture(Texture2D tex)
        {
            textures[texCount++] = tex;
        }

        public void render(Camera camera, SpriteBatch spriteBatch)
        {
            Vector2 camstart = new Vector2(camera.Position.X, camera.Position.Y);

            for (int i = ((int)camstart.X / TILESIZE); i < ((int)camstart.X / TILESIZE) + 1 + ((int)vpSize.X / TILESIZE); i++)
            {
                if (i < 0 || i > worldSize - 1)
                {
                    continue;
                }
                for (int j = ((int)camstart.Y / TILESIZE); j < ((int)camstart.Y / TILESIZE) + 1 + ((int)vpSize.Y / TILESIZE); j++)
                {
                    if (j < 0 || j > worldSize - 1)
                    {
                        continue;
                    }
                    tiles[i, j].Render(spriteBatch);
                    
                }
            }
        }




    }
}
