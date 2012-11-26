using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using lizard.player;

namespace lizard.oldworld
{
    class Tile
    {

        public Vector2 position;
        public int texID;
        public Block[] blocks;

        public Lizard[] lizards;

        private bool passable;

        public World world;


        public Tile(int texID, Vector2 position, World world)
        {
            this.world = world;

            this.blocks = new Block[4]; //4 blocks can sit on it
            this.lizards = new Lizard[4];

            this.texID = texID;
           /// this.tex = TileMap.textures[texID];

            this.UpdatePassability();

            this.position = position;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TileMap.textures[texID], position, Color.White);
        }

        public bool AddBlock(int index, Block block)
        {
            if (index < 4 && index > -1)
            {
                if (this.blocks[index] == null)
                {
                    this.blocks[index] = block;
                    
                    UpdatePassability();

                    block.position = this.position + new Vector2(index % 2 == 1 ? TileMap.TILESIZE / 2 : 0, index / 2 == 1 ? TileMap.TILESIZE / 2 : 0);
                     
                    return true;
                }
                return false;

            }
            return false;
        }

        public bool AddLizard(int index, Lizard lizard)
        {
            if (this.lizards[index] == null)
            {
                this.lizards[index] = lizard;
                //lizard.tile = this;
                return true;
            }

            return false;
        }

        public bool RemoveLizard(Lizard lizard)
        {
            for (int i = 0; i < 4; i++)
            {
                if (lizards[i] == lizard)
                {
                    lizards[i] = null;
                    lizard.tile = null;
                    return true;
                }
            }

            return false;
        }

        public Lizard RemoveLizard(int index)
        {
            if (lizards[index] != null)
            {
                Lizard liz = lizards[index];
                liz.tile = null;
                lizards[index] = null;
                return liz;
            }
            return null;
        }


        public Block RemoveBlock(int index)
        {
            Block block = this.blocks[index];
            this.blocks[index] = null;
            
            UpdatePassability();

            return block;
        }

        public void UpdatePassability()
        {
            if (this.texID == 3)
            {
                this.passable = false;
                return;
            }

            if (this.blocks[0] != null &&
                this.blocks[1] != null &&
                this.blocks[2] != null &&
                this.blocks[3] != null)
            {
                this.passable = false;
                return;
            }


            this.passable = true;
        }

        public bool Passable
        {
            get {
                UpdatePassability();
                return this.passable;
            }
        }

    }
}
