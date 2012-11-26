using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using lizard.world;
using lizard.world.block;


namespace lizard.oldworld
{
    class Block
    {
        public Vector2 position;

        public Texture2D tex;
        public int texID;

        public Block(Texture2D texture)
        {
            this.tex = texture;
        }

        public Block(int texID, Vector2 position) : this (BlockTexture.textures[texID])
        {
            this.texID = texID;
            this.position = position;


        }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.tex, this.position, Color.White);   
        }

        public virtual void HandleInteraction()
        {}

    }
}
