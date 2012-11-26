using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace lizard.world.block
{
    class TreeBlock : Block
    {

        public TreeBlock(Vector2 position)
            : base(1, position)
        {
            //nothing to do here?
        }

        public override void HandleInteraction()
        {
            Tile t = TileMap.getTileAtPos(this.Position);
            /*
            for (int i = 0; i < 4; i++)
            {
                if (i == self)
                {
                    t.world.RemoveBlock(t, i);
                    continue;
                }
                if (t.lizards[i] == null && (t.blocks[i] == null || t.blocks[i] == this))
                {
                    t.world.AddBlock(i, new WoodBlock(t.position), t);
                }

            }
            */
        }


    }
}
