using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using lizard.world;
using lizard.world.block;

namespace lizard.player
{
    class Player
    {
        private const int MOUSESCROLL = 32;
        private const int KEYSCROLL = 1536;

        public Vector2 vpSize;

        public Camera camera;

        public World world;

        public Lizard selected;
        public List<Lizard> lizards;

        public Vector2 cursorPos;

        public Texture2D cursorText;
        public Texture2D indicatorText;


        //int tileID = 0;

        private MouseState currMouse;
        private MouseState lastMouse;

        public Player(Camera camera, Texture2D cursor, Texture2D indicator, Vector2 vpSize)
        {
            this.vpSize = vpSize;

            this.cursorText = cursor;
            this.indicatorText = indicator;
            this.cursorPos = new Vector2(vpSize.X/2,vpSize.Y/2);

            this.camera = camera;
            
            
            this.lizards = new List<Lizard>();

            lastMouse = Mouse.GetState();
        }

        public void Init(World world)
        {
            this.world = world;
            camera.Position = new Vector2(TileMap.TILESIZE * TileMap.worldSize * 0.5f);


        }

        public void Update(GameTime gameTime)
        {
            UpdateCursor();
            HandleCamera();

            currMouse = Mouse.GetState();
            KeyboardState currKeys = Keyboard.GetState();

          
            if (currMouse.LeftButton == ButtonState.Pressed && lastMouse.LeftButton == ButtonState.Released)
            {
                Lizard lizard = getLizardAtMouse();

                if (lizard != null)
                {
                    //unselect old
                    if (selected != null)
                    {
                        selected.isSelected = false;
                    }

                    selected = lizard;
                    selected.isSelected = true;
                }
                else if (selected != null) 
                {
                    selected.isSelected = false;
                    selected = null;
                }
            }



            if (currMouse.RightButton == ButtonState.Pressed && lastMouse.RightButton == ButtonState.Released && !currKeys.IsKeyDown(Keys.LeftShift))
            {       
                if (selected != null)
                {
                    selected.pickupBlock = null;

                    Tile attempt = TileMap.getTileAtPos(worldSpaceMouse());

                    //if attempt is valid, path there
                    if (attempt != null && attempt.lizard == null && attempt.Passable)
                    {
                        if ((attempt == selected.path.Last() || (selected.dropTile != null && attempt == selected.dropTile)) && selected.block != null)//clicking twice on a place when carrying a block drops
                        {
                            List<Tile> places = new List<Tile>();
                            places.Add(TileMap.getTileByIndex((int)attempt.index.X - 1, (int)attempt.index.Y));
                            places.Add(TileMap.getTileByIndex((int)attempt.index.X + 1, (int)attempt.index.Y));
                            places.Add(TileMap.getTileByIndex((int)attempt.index.X, (int)attempt.index.Y - 1));
                            places.Add(TileMap.getTileByIndex((int)attempt.index.X, (int)attempt.index.Y + 1));

                            selected.Path(places);
                            selected.dropTile = attempt;
                        }
                        else
                        {
                            selected.Path(attempt);
                        }
                    
                    }
                    else if (attempt.block != null && this.selected.block == null)//if there is a block there, pick it up
                    {
                        selected.pickupBlock = attempt.block;
                        selected.Path(attempt);
                    }
                }
            }




            if (currMouse.MiddleButton == ButtonState.Pressed )//&& lastMouse.MiddleButton == ButtonState.Released)
            {

                Tile tile = TileMap.getTileAtPos(worldSpaceMouse());

                if (tile != null && tile.lizard == null && tile.block == null)
                {
                    //find index to add at
                    Vector2 offset = worldSpaceMouse() - tile.position;

                    world.AddBlock(new TreeBlock(tile.position), false);
                }
                

                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.RightControl) && Keyboard.GetState().IsKeyDown(Keys.I))
            {
                world.SaveLevel("new.txt");
            }

            lastMouse = currMouse;
        }

        public void Render(SpriteBatch spriteBatch)
        {

            if (selected != null)
            {
                spriteBatch.Draw(indicatorText, selected.position - camera.Position + new Vector2(-32), Color.Chartreuse);
            }
            spriteBatch.Draw(cursorText, cursorPos, Color.White);

        }

        private Lizard selectLizard()
        {
            Tile tile = getTileAtMouse();

            Vector2 tileOffset = worldSpaceMouse() - tile.position;
            int currSub = (tileOffset.X > TileMap.TILESIZE / 2 ? 1 : 0) + (tileOffset.Y > TileMap.TILESIZE / 2 ? 2 : 0);


            
            return tile == null? null : tile.lizard;
        }

        private Tile getTileAtMouse()
        {
            Vector2 worldPos = new Vector2(camera.Position.X, camera.Position.Y);
            worldPos += cursorPos;

            Tile tile = TileMap.getTileAtPos(worldPos);
            return tile;
        }

        private Tile getTileAtMouse(Vector2 offset)
        {
            Vector2 worldPos = new Vector2(camera.Position.X, camera.Position.Y);
            worldPos += cursorPos + offset;

            Tile tile = TileMap.getTileAtPos(worldPos);
            return tile;
        }

        private Lizard getLizardAtMouse()
        {
            Lizard retLizard = null;

            float currDiff = 999f;

            float testDiff;

            Vector2 wsm = worldSpaceMouse();

            foreach (Lizard lizard in this.lizards)
            {
                testDiff= (lizard.position - wsm).Length();
                if (currDiff > testDiff && testDiff < TileMap.TILESIZE)
                {
                    currDiff = (lizard.position - wsm).Length();
                    retLizard = lizard;
                }
            }


            return retLizard;
        }

        private Vector2 worldSpaceMouse()
        {
            return camera.Position + cursorPos;
        }

        private void UpdateCursor()
        {
            MouseState ms = Mouse.GetState();

            //get mouse delta
            Vector2 mousePos = new Vector2(ms.X, ms.Y);
            Vector2 diff = new Vector2(vpSize.X / 2, vpSize.Y / 2);
            diff = mousePos - diff;

            //reset mouse
            Mouse.SetPosition((int)(vpSize.X / 2), (int)(vpSize.Y / 2));

            //move our cursor
            cursorPos += diff;

            //clamp it to valid positions
            cursorPos.X = MathHelper.Clamp(cursorPos.X, 0, vpSize.X - 1);
            cursorPos.Y = MathHelper.Clamp(cursorPos.Y, 0, vpSize.Y - 1);
        }

        private void HandleCamera()
        {
            
            //camera shift
            Vector2 camShift = Vector2.Zero;

            //cursor movement
            if (cursorPos.X < MOUSESCROLL)
            {
                camShift.X -= (float)Math.Pow((MOUSESCROLL - cursorPos.X), 2);
            }
            else if (cursorPos.X > (vpSize.X - MOUSESCROLL))
            {
                camShift.X += (float)Math.Pow((cursorPos.X - (vpSize.X - MOUSESCROLL)), 2);
            }

            if (cursorPos.Y < MOUSESCROLL)
            {
                camShift.Y -= (float)Math.Pow((MOUSESCROLL - cursorPos.Y), 2);
            }
            else if (cursorPos.Y > (vpSize.Y - MOUSESCROLL))
            {
                camShift.Y += (float)Math.Pow(cursorPos.Y - (vpSize.Y - MOUSESCROLL), 2);
            }

            KeyboardState ks = Keyboard.GetState();
            
            //Keyboard movement
            if (ks.IsKeyDown(Keys.W))
            {
                camShift.Y -= KEYSCROLL;
            }
            else if (ks.IsKeyDown(Keys.S))
            {
                camShift.Y += KEYSCROLL;
            }

            if (ks.IsKeyDown(Keys.A))
            {
                camShift.X -= KEYSCROLL;
            }
            else if (ks.IsKeyDown(Keys.D))
            {
                camShift.X += KEYSCROLL;
            }

            //scale the camera shift back to reasonable
            camShift /= 192f;
            

            //new pos
            Vector2 nextCamPos = camera.Position + camShift;

            nextCamPos.X = MathHelper.Clamp(nextCamPos.X, 0f, TileMap.worldSize * TileMap.TILESIZE - vpSize.X);
            nextCamPos.Y = MathHelper.Clamp(nextCamPos.Y, 0f, TileMap.worldSize * TileMap.TILESIZE - vpSize.Y);

            camera.LookAtCenter(nextCamPos);
            

            //reset mouse if too close to edge of screen;

        }

        

    }
}
