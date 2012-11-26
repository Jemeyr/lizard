using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lizard.world
{
    class TileMap
    {
        //ok this contains the tiles which we use for units AND worldtiles
        public const int TILESIZE = 32;//size of tile for units
        
        public static int worldSize; //dimensions of the world in worldtilesize unit

        public World world;

        public static Tile[,] tiles;

        public static void Render(Camera camera, SpriteBatch spriteBatch)
        {
            
            Vector2 camstart = new Vector2(camera.Position.X, camera.Position.Y);


            for (int i = ((int)camstart.X / TILESIZE); i < ((int)camstart.X / TILESIZE) + 1 + (35); i++)
            {
                if (i < 0 || i > worldSize - 1)
                {
                    continue;
                }
                for (int j = ((int)camstart.Y / TILESIZE); j < ((int)camstart.Y / TILESIZE) + 1 + (20); j++)
                {
                    if (j < 0 || j > worldSize - 1)
                    {
                        continue;
                    }
                    spriteBatch.Draw(Game1.debugTex, new Rectangle(i*TILESIZE,j*TILESIZE, TILESIZE, TILESIZE), TileMap.getTileByIndex(i,j).Passable? new Color(.1f,.9f,.9f,.01f): new Color(1f,0f,0f,.11f));


                }
            }
        }

        public TileMap(int size, World world)
        {
            this.world = world;

            worldSize = size*2;//Size is double because tiles are smaller than bgtiles by 2

        
            tiles = new Tile[worldSize, worldSize];
            for (int i = 0; i < worldSize; i++)
            {
                for (int j = 0; j < worldSize; j++)
                {
                    tiles[i, j] = new Tile(new Vector2(i, j), world);
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

        public static Tile getTileByIndex(int x, int y)
        {
            int validX = (int)MathHelper.Clamp(x, 0, TileMap.worldSize - 1);
            int validY = (int)MathHelper.Clamp(y, 0, TileMap.worldSize - 1);

            return tiles[validX, validY];
            
        }



    }
}
