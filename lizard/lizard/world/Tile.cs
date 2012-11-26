using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using lizard.player;
using lizard.world.block;

namespace lizard.world
{
    class Tile
    {
        //this is where blocks and lizards live
        public Vector2 position;
        public Vector2 index;

        public Block block;
        public Lizard lizard;
        
        private bool passable;

        public World world;

        public Tile(Vector2 index, World world)
        {
            this.world = world;
            this.index = index;
            this.position = index * TileMap.TILESIZE;
        
            this.UpdatePassability();
        }

        
        public bool AddBlock(Block block)
        {
            
            if (this.block == null)
            {
                this.block = block;

                UpdatePassability();

                block.Position = this.position;

                return true;
            }
            return false;
        }

        public bool AddLizard(Lizard lizard)
        {
            if (this.lizard == null)
            {
                this.lizard = lizard;
                lizard.tile = this;
                UpdatePassability();
                return true;
            }

            return false;
        }

        public bool RemoveLizard(Lizard lizard)
        {
            if (this.lizard != null)
            {
                this.lizard = null;
                UpdatePassability();
                return true;
            }

            return false;
        }


        public Block RemoveBlock()
        {
            if (this.block == null)
            {
                return null;
            }

            Block block = this.block;
            this.block = null;

            world.RemoveBlock(block);

            UpdatePassability();

            return block;
        }

        public void UpdatePassability()
        {
            if (this.block != null || this.lizard != null || !BgTileMap.bgtiles[(int)index.X / 2, (int)index.Y / 2].Passable)
            {
                this.passable = false;
                return;
            }
            

            this.passable = true;
        }

        public bool Passable
        {
            get
            {
                UpdatePassability();
                return this.passable;
            }
        }

    }
}
