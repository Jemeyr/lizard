using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using lizard.world;

namespace lizard.player
{
    class Node
    {
        public Node prev;
        public Tile tile;
        public int dist;

        public int x
        {
            get { return (int)this.tile.index.X; }
        }

        public bool passable
        {
            get { return this.tile.Passable; }
        }

        public int y
        {
            get { return (int)this.tile.index.Y; }
        }

        public Node(Node prev, int x, int y)
        {
            this.prev = prev;
            this.dist = prev == null ? 0 : this.prev.dist + 1;

            this.tile = TileMap.getTileByIndex(x, y);
        }

        public Node(Node prev, Vector2 index)
        {
            this.prev = prev;
            this.tile = TileMap.getTileByIndex((int)index.X, (int)index.Y);
        }

    }

    class Lizard
    {
        public static Texture2D lizTex;
        public static Texture2D targTex;

        public static Player player;

        public Vector2 position;
        public float rotation;

        public Block block;
        
        public Tile tile;
        public Tile targetTile;
        
        public bool pickup;
        public Block pickupBlock;
        public Tile dropTile;

        public Tile nextTile;

        public List<Tile> path;

        public bool isSelected;


        private static Vector2 offset = new Vector2(-16);

        private Lizard(Vector2 position)
        {
            this.tile = TileMap.getTileAtPos(position);

            this.isSelected = false;
            this.position = position;

            pickup = false;
            
            this.path = new List<Tile>();


            this.targetTile = tile;
            this.nextTile = tile;

            Path(targetTile);
            
            this.block = null;

            this.rotation = 0f;
        }

        public static void initLizard(Texture2D lizTex, Texture2D targTex)
        {
            Lizard.lizTex = lizTex;
            Lizard.targTex = targTex;
            
        }

        public static Lizard addLizard(Vector2 position)
        {
            if (Lizard.lizTex == null)
            {
                return null;
            }

            Lizard lizard = new Lizard(position);

            if (!lizard.tile.AddLizard(lizard))
            {
                Console.Out.WriteLine("Failure");
            }

            if (lizard != null)
            {
                player.lizards.Add(lizard);
            }

            return lizard;
        }

        public static Lizard addLizard(Tile tile)
        {
            return addLizard(tile.position);
        }

        public void Update(GameTime gameTime)
        {

            //move through list
            if (this.targetTile != null && this.targetTile == this.tile)
            {
                //hit our target, move on
                if (this.path.Count > 1)
                {
                    this.path.Remove(this.targetTile);
                    this.targetTile = this.path.First();
                }
                else if ((this.targetTile.position - this.position).Length() < TileMap.TILESIZE)//try to get a block or put one down
                {
                    //todo decide how blocks work here

                    //if no block, pick up
                    if (this.block == null && targetTile.block == this.pickupBlock)
                    {
                        this.block = targetTile.RemoveBlock();
                        this.pickupBlock = null;
                    }
                }

                if (this.dropTile != null && (this.dropTile.position - this.position).Length() < TileMap.TILESIZE * 2)
                {
                    this.block.Position = dropTile.position;
                    dropTile.world.AddBlock(this.block);
                    this.block = null;
                    this.dropTile = null;
                }

            }

            //if we have somewhere to move
            if (targetTile != null && position != targetTile.position + new Vector2(16))
            {
                //get the direction vector to it
                Vector2 dir = targetTile.position - this.position + new Vector2(16);

                //clip it to 3 max
                if (dir.Length() > 3f)
                {
                    dir.Normalize();
                    dir *= 3f;
                }

                //advance
                position += dir;

                //if we move, rotate to that way
                if (dir.Length() > 0.01)
                {
                    this.rotation = angerp(this.rotation,(float)Math.Atan2(dir.Y, dir.X),0.25f);
                }
                
                //get the tile at the new position
                Tile currTile = TileMap.getTileAtPos(this.position);

                if (currTile == this.tile)//if we don't change tiles
                {

                }
                else if(currTile.lizard == null && currTile.block == null)//if we advance onto a free tile
                {
                    this.tile.RemoveLizard(this);
                    this.tile = currTile;
                    this.tile.AddLizard(this);

                }
                else if (currTile.block != null && currTile.block == this.pickupBlock)// if we hit the block to pick up
                {
                    this.block = currTile.RemoveBlock();
                    this.pickupBlock = null;
                    
                    this.tile.RemoveLizard(this);
                    this.tile = currTile;
                    this.tile.AddLizard(this);
                }
                else if (currTile.lizard != null || currTile.block != null)//if we collide with a block or lizard
                {
                    //unmove, repath
                    this.position -= dir;

                    Tile thing2 = TileMap.getTileAtPos(this.position);

                    if (!Path(this.path.Last()))
                    {
                        Path(thing2);
                    }
                }

               

            }

        }

        private bool HasNode(List<Node> list, Node node)
        {
            foreach (Node listNode in list)
            {
                if (listNode.tile == node.tile)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Path(Tile targetTile)
        {
            List<Tile> tiles = new List<Tile>();
            tiles.Add(targetTile);

            return Path(tiles);
        }

        public bool Path(List<Tile> targetTiles)
        {

            Node startNode = new Node(null, this.tile.index);

            Node finalNode = null;

            List<Node> targetNodes = new List<Node>();
            foreach (Tile tile in targetTiles)
            {
                Node targetNode = new Node(null, tile.index);

                targetNodes.Add(targetNode);
                
                if (targetNode.x == startNode.x && targetNode.y == startNode.y)
                {
                    this.targetTile = targetNode.tile;
                    this.path = new List<Tile>();
                    path.Insert(0, tile);
                    return true;
                }
            
            }
            
            int radius = 32;

            

            bool pathed = false;

            List<Node> visited = new List<Node>();
            List<Node> edge = new List<Node>();

            edge.Add(startNode);


            Node left, right, up, down;

            while (edge.Count() > 0 && !pathed)
            {
                Node curr = edge.First();
                edge.Remove(curr);
                visited.Add(curr);

               
                up = down = right = left = null;
                List<Node> nexts = new List<Node>();

                if (curr.x - 1 > this.tile.index.X - radius)
                {
                    left = new Node(curr, curr.x - 1, curr.y);
                    nexts.Add(left);
                }
                if (curr.y - 1 > this.tile.index.Y - radius)
                {
                    up = new Node(curr, curr.x, curr.y - 1);
                    nexts.Add(up);
                }
                if (curr.x < this.tile.index.X + radius)
                {
                    right = new Node(curr, curr.x + 1, curr.y);
                    nexts.Add(right);
                }
                if (curr.y < this.tile.index.Y + radius)
                {
                    down = new Node(curr, curr.x, curr.y + 1);
                    nexts.Add(down);
                }

                //add the diagonals
                if (up != null && left != null && up.passable && left.passable)
                {
                //    nexts.Add(new Node(curr, curr.x - 1, curr.y - 1));
                }
                if (up != null && right != null && up.passable && right.passable)
                {
                  //  nexts.Add(new Node(curr, curr.x + 1, curr.y - 1));
                }
                if (down != null && left != null && down.passable && left.passable)
                {
                  //  nexts.Add(new Node(curr, curr.x - 1, curr.y + 1));
                }
                if (down != null && right != null && down.passable && right.passable)
                {
                //    nexts.Add(new Node(curr, curr.x + 1, curr.y + 1));
                }

                    foreach(Node n in nexts)
                    {
                        if(HasNode(edge,n) || HasNode(visited, n))
                        {
                            //done
                        }
                        else if((n.passable || (n.tile.block != null && n.tile.block == pickupBlock) )&& n.dist < 64)
                        {
                            edge.Add(n);

                            foreach (Node targetNode in targetNodes)
                            {
                                if (n.x == targetNode.x && n.y == targetNode.y)
                                {
                                    finalNode = n;
                                    pathed = true;
                                    break;
                                }
                            }
                        }

                    }
                   // edge.Sort((s1,s2) => s1.dist.CompareTo(s2.dist));
                }



            if (pathed && finalNode != null)
            {
                Node curr = finalNode;
                this.path = new List<Tile>();
                do
                {
                    this.path.Insert(0, curr.tile);
                    
                    curr = curr.prev; 
                } while (curr != null);
                this.targetTile = path.First();
                return true;
            }
            return false;


        }

        public float angerp(float a, float b, float amount)
        {
            a = MathHelper.WrapAngle(a);
            b = MathHelper.WrapAngle(b);

            if (Math.Abs(a - b) > Math.Abs(b - a))
            {
                return MathHelper.Lerp(a, b, amount);
            }
            else
            {
                return MathHelper.Lerp(b, a, amount);
            }

        }


        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Lizard.lizTex, position, null, Color.Green, this.rotation + (float)Math.PI / 2, new Vector2(24), 1.0f, SpriteEffects.None, 0f);
            if (block != null)
            {
                spriteBatch.Draw(block.tex, position, null, Color.White, this.rotation + (float)Math.PI / 2, new Vector2(16), 0.5f, SpriteEffects.None, 0f);
            }
            if (isSelected)
            {
                foreach (Tile tile in this.path)
                {
                    spriteBatch.Draw(targTex, tile.position + (tile == this.path.Last()? Vector2.Zero : new Vector2(12)), null, Color.White, 0f, Vector2.Zero, tile == this.path.Last()?1.0f : 0.25f,SpriteEffects.None, 0f);
                }

            }
        }
       

    }
}
