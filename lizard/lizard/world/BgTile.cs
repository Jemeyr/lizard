using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using lizard.player;

namespace lizard.world
{
    class BgTile
    {
        private static Texture2D[] textures = new Texture2D[16];
        private static int texCount = 0;



        public Vector2 index;
        public Vector2 position;
        
        public int texID;
        
        private bool passable;

        public World world;


        public BgTile(int texID, Vector2 index, World world)
        {
            this.world = world;

            this.texID = texID;
            
            this.UpdatePassability();

            this.index = index;
            this.position = index * BgTileMap.BGTILESIZE;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BgTile.textures[texID], position, Color.White);
        }


        public static void addTexture(Texture2D tex)
        {
            textures[texCount++] = tex;
        }


        public void UpdatePassability()
        {
            if (this.texID == 3)
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
