using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lizard.world
{
    class BgTileMap
    {
        public const int BGTILESIZE = 64;
        public static int worldSize;

        public World world;

        public Vector2 vpSize;
        public static BgTile[,] bgtiles;
        public static List<BgTile> auxBgTiles;
        
        public BgTileMap(int size, Vector2 vpSize, World world)
        {
            this.world = world;

            this.vpSize = vpSize;
            worldSize = size;

            auxBgTiles = new List<BgTile>();
            bgtiles = new BgTile[size, size];
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    bgtiles[i, j] = new BgTile(0, new Vector2(i, j), world);
                }
            }
        }

        public static BgTile getTileAtPos(Vector2 position)
        {
            Vector2 pos = position;
            pos /= TileMap.TILESIZE;

            pos.X = MathHelper.Clamp(pos.X, 0, TileMap.TILESIZE * (TileMap.worldSize - 1));
            pos.Y = MathHelper.Clamp(pos.Y, 0, TileMap.TILESIZE * (TileMap.worldSize - 1));

            return BgTileMap.bgtiles[(int)pos.X, (int)pos.Y];
        }

        public void Render(Camera camera, SpriteBatch spriteBatch)
        {
            Vector2 camstart = new Vector2(camera.Position.X, camera.Position.Y);

            for (int i = ((int)camstart.X / BGTILESIZE); i < ((int)camstart.X / BGTILESIZE) + 1 + ((int)vpSize.X / BGTILESIZE); i++)
            {
                if (i < 0 || i > worldSize - 1)
                {
                    continue;
                }
                for (int j = ((int)camstart.Y / BGTILESIZE); j < ((int)camstart.Y / BGTILESIZE) + 1 + ((int)vpSize.Y / BGTILESIZE); j++)
                {
                    if (j < 0 || j > worldSize - 1)
                    {
                        continue;
                    }
                    bgtiles[i, j].Render(spriteBatch);

                }
            }


            foreach (BgTile bgtile in auxBgTiles)
            {
                if(bgtile.position.X + BgTileMap.BGTILESIZE > camera.Position.X &&
                   bgtile.position.Y + BgTileMap.BGTILESIZE > camera.Position.Y &&
                   bgtile.position.X < camera.Position.X + world.player.vpSize.X &&
                   bgtile.position.Y < camera.Position.Y + world.player.vpSize.Y)
                {
                    bgtile.Render(spriteBatch);
                }
            }
        }

        public void Save(System.IO.StreamWriter file)
        {

            for (int i = 0; i < BgTileMap.worldSize; i++)
            {
                for (int j = 0; j < BgTileMap.worldSize; j++)
                {
                    file.Write(BgTileMap.bgtiles[i, j].texID);
                }
                file.Write("\n");
            }
        }

        public void Load(System.IO.StreamReader file)
        {

            for (int i = 0; i < BgTileMap.worldSize; i++)
            {
                System.IO.StringReader stringReader = new System.IO.StringReader(file.ReadLine());
                for (int j = 0; j < BgTileMap.worldSize; j++)
                {
                    BgTileMap.bgtiles[i, j].texID = stringReader.Read() - (int)'0';
                    BgTileMap.bgtiles[i, j].UpdatePassability();
                }
            }
        }


    }
}
