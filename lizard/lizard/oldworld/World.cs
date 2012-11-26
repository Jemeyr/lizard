using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using lizard.player;

namespace lizard.oldworld
{
    class World
    {
        public Player player;
        public TileMap tileMap;
        public List<Block> blocks;

        public World(Player p)
        {
            tileMap = new TileMap(64, p.vpSize,this);
            blocks = new List<Block>();

            this.player = p;

            ReadIn();
            /*
            Lizard.addLizard(TileMap.tiles[41, 36]);
            Lizard.addLizard(TileMap.tiles[42, 36]);
            Lizard.addLizard(TileMap.tiles[41, 37]).block  = new oldworld.block.WoodBlock(TileMap.tiles[41,37].position);
            */
        }

        public void Render(SpriteBatch spriteBatch)
        {
            tileMap.render(player.camera, spriteBatch);

            foreach (Block block in blocks)
            {
                block.Render(spriteBatch);
            }

            foreach (Lizard lizard in player.lizards)
            {
                lizard.Render(spriteBatch);
            }

        }

        public void Update(GameTime gameTime)
        {
            foreach (Lizard lizard in player.lizards)
            {
                lizard.Update(gameTime);
            }
        }


        public bool AddBlock(int index, Block block, Tile tile)
        {
            if (tile.AddBlock(index, block))
            {
                this.blocks.Add(block);
                block.HandleInteraction();
            
                return true;
            }
            return false;
        }

        public bool AddBlock(int index, Block block, Tile tile, bool handleInteraction)
        {
            if (tile.AddBlock(index, block))
            {
                this.blocks.Add(block);
                if (handleInteraction)
                {
                    block.HandleInteraction();
                }
                return true;
            }
            return false;
        }

        public Block RemoveBlock(Tile tile, int index)
        {

            Block block = tile.RemoveBlock(index);
            if (block != null)
            {
                this.blocks.Remove(block);
            }

            return block;
        
        }


        public void WriteOut()
        {
            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter("level.txt");
            file.WriteLine("64");
            for (int i = 0; i < TileMap.worldSize; i++)
            {
                for (int j = 0; j < TileMap.worldSize; j++)
                {
                    file.Write(TileMap.tiles[i, j].texID);
                }
                file.Write("\n");
            }

            file.Close();
        }

        public void ReadIn()
        {
            System.IO.StreamReader fileReader = new System.IO.StreamReader("level.txt");
            if (fileReader.ReadLine() != "64")
            {
                Console.Out.WriteLine("FAILURE");
                return;
            }
            for (int i = 0; i < TileMap.worldSize; i++)
            {
                System.IO.StringReader stringReader = new System.IO.StringReader(fileReader.ReadLine());
                for (int j = 0; j < TileMap.worldSize; j++)
                {
                    TileMap.tiles[i, j].texID = stringReader.Read() - (int)'0';
                    TileMap.tiles[i, j].UpdatePassability();

                }
            }

            fileReader.Close();

        }


    }
}
