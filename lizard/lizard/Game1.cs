using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using lizard.player;
using lizard.world;
using lizard.world.block;


namespace lizard
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        World world;
        Player player;

        public static Texture2D debugTex;

        public Game1(){
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1152;
            graphics.PreferredBackBufferHeight = 640;
        
            Content.RootDirectory = "Content";
        }
        protected override void Initialize(){
            base.Initialize();
        }
        protected override void UnloadContent() { }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);


            debugTex = new Texture2D(GraphicsDevice, 1, 1);
            Color[] white = { new Color(1f, 1f, 1f) };
            debugTex.SetData(white);

            BgTile.addTexture(Content.Load<Texture2D>("dirtText"));
            BgTile.addTexture(Content.Load<Texture2D>("grassText"));
            BgTile.addTexture(Content.Load<Texture2D>("floorText"));
            BgTile.addTexture(Content.Load<Texture2D>("waterText"));
            BgTile.addTexture(Content.Load<Texture2D>("rockText"));

            BlockTexture.addTexture(Content.Load<Texture2D>("woodBlockText"));
            BlockTexture.addTexture(Content.Load<Texture2D>("treeBlockText"));


            

            Lizard.initLizard(Content.Load<Texture2D>("liz"),Content.Load<Texture2D>("dest"));



            this.player = new Player(new Camera(GraphicsDevice.Viewport),
                                    Content.Load<Texture2D>("cursor"),
                                    Content.Load<Texture2D>("indicator"),
                                new Vector2(GraphicsDevice.Viewport.Width,GraphicsDevice.Viewport.Height));
            Lizard.player = player;
       
            this.world = new World(player);

            player.Init(this.world);

        }

        
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)){this.Exit();}

            player.Update(gameTime);
            
            world.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, player.camera.GetCameraTransformation());

           world.Render(spriteBatch);

        spriteBatch.End();

        spriteBatch.Begin();

            player.Render(spriteBatch);

        spriteBatch.End();
         

            base.Draw(gameTime);
        }
    }
}
