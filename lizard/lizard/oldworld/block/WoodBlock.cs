using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace lizard.oldworld.block
{
    class WoodBlock : Block
    {

        public WoodBlock(Vector2 position)
            : base(0, position)
        {
            //nothing to do here?
        }

        public override void HandleInteraction()
        {
            Tile t = TileMap.getTileAtPos(this.position);

            if (t.blocks[0] != null && t.blocks[0].texID == 0 &&
                t.blocks[1] != null && t.blocks[1].texID == 0 &&
                t.blocks[2] != null && t.blocks[2].texID == 0 &&
                t.blocks[3] != null && t.blocks[3].texID == 0)
            {

                t.world.RemoveBlock(t, 0);
                t.world.RemoveBlock(t, 1);
                t.world.RemoveBlock(t, 2);
                t.world.RemoveBlock(t, 3);
                
                t.texID = 2;

            }
        }
        

    }
}
