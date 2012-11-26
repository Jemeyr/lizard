using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using lizard.player;

namespace lizard.world
{
    class World
    {
        //the player, they get their lizards
        public Player player;

        //thie tilemap, which is used to keep track of blocks and lizards
        public TileMap tileMap;

        //the background tilemap, which is for background tiles
        public BgTileMap bgTileMap;

        //all our blocks
        private List<Block> blocks;



        public World(Player p)
        {

            this.player = p;
            
            
            bgTileMap = new BgTileMap(64, player.vpSize, this);
            tileMap = new TileMap(64, this);
            
            blocks = new List<Block>();


            LoadLevel("level.txt");

            Lizard.addLizard(TileMap.tiles[82, 72]);
            Lizard.addLizard(TileMap.tiles[84, 72]);
           // Lizard.addLizard(TileMap.tiles[82, 74]).block = new world.block.TreeBlock(TileMap.tiles[41, 37].position);

        }

        public void Render(SpriteBatch spriteBatch)
        {
            bgTileMap.Render(player.camera, spriteBatch);


            foreach (Block block in blocks)
            {
                if (block.Position.X +32 > player.camera.Position.X && block.Position.X < player.camera.Position.X + player.vpSize.X &&
                    block.Position.Y +32 > player.camera.Position.Y && block.Position.Y < player.camera.Position.Y + player.vpSize.Y)
                {
                    block.Render(spriteBatch);
                }
            }

            foreach (Lizard lizard in player.lizards)
            {
                lizard.Render(spriteBatch);
            }


          //  TileMap.Render(player.camera, spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            foreach (Lizard lizard in player.lizards)
            {
                lizard.Update(gameTime);
            }
        }


        public bool AddBlock(Block block)
        {
            if (block != null)
            {
                this.blocks.Add(block);
                TileMap.getTileByIndex((int)block.Index.X, (int)block.Index.Y).AddBlock(block);

                block.HandleInteraction();

                return true;
            }
            return false;
        }

        public bool AddBlock(Block block, bool handleInteraction)
        {
            if (block != null)
            {
                this.blocks.Add(block);

                TileMap.getTileByIndex((int)block.Index.X, (int)block.Index.Y).AddBlock(block);

                if (handleInteraction)
                {
                    block.HandleInteraction();
                }
                return true;
            }
            return false;
        }



        public Block RemoveBlock(Block block)
        {
            
            if (block != null)
            {
                this.blocks.Remove(block);
            }

            return block;
        }

        public void SaveLevel(string fileName)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);

            file.WriteLine("#bgTiles");

            bgTileMap.Save(file);

            file.Close();
            
        }


        public void LoadLevel(string fileName)
        {
            System.IO.StreamReader fileReader = new System.IO.StreamReader(fileName);
            
            if (fileReader.ReadLine() != "#bgTiles")
            {
                Console.Out.WriteLine("FAILURE");
                return;
            }

            bgTileMap.Load(fileReader);

            fileReader.Close();

        }


    }
}
