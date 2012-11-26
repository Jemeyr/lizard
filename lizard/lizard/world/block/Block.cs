using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using lizard.world;
using lizard.world.block;


namespace lizard.world
{
    class Block
    {
        private Vector2 position;
        private Vector2 index;

        public Texture2D tex;
        public int texID;

        private Block(Texture2D texture)
        {
            this.tex = texture;
        }

        public Block(int texID, Vector2 position)
            : this(BlockTexture.textures[texID])
        {
            this.texID = texID;
            this.Position = position;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.tex, this.position, Color.White);
        }

        public virtual void HandleInteraction()
        { }



        public Vector2 Position { 
            get { return this.position; }
            set
            {
                this.position = value;
                this.index = new Vector2((int)(position.X / TileMap.TILESIZE), (int)(position.Y / TileMap.TILESIZE));
            }
        }
        public Vector2 Index
        {
            get { return this.index; }
            set
            {
                this.index = value;
                this.position = new Vector2(position.X * TileMap.TILESIZE, position.Y * TileMap.TILESIZE);
            }
        }

    }
}
