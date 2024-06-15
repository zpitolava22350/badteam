using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace _3d_test {

    public static class ResourceManager {
        public static GraphicsDevice GraphicsDevice { get; set; }
    }

    public class Game1: Game {

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        private zpitoHandler zpitoHandler = new zpitoHandler();
        List<Block> blocks = new List<Block>();
        Dictionary<string, Texture2D> texDict = new Dictionary<string, Texture2D>();
        Dictionary<string, BasicEffect> effDict = new Dictionary<string, BasicEffect>();
        private Player player;

        private Random rand = new Random();

        private string[] textures = new string[14];

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            AllocConsole();

            //target framerate
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 144.0);

            //consistent framerate
            IsFixedTimeStep = true;

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();

            textures[0] = "blue_ice";
            textures[1] = "brick";
            textures[2] = "dark_gray";
            textures[3] = "grass";
            textures[4] = "gray";
            textures[5] = "log";
            textures[6] = "orange";
            textures[7] = "powder_snow";
            textures[8] = "purpur_block";
            textures[9] = "reinforceddeepslate";
            textures[10] = "sand";
            textures[11] = "smoothstone";
            textures[12] = "stone";
            textures[13] = "water";

            player = new Player(new Vector3(0, 5, 0), 50f);

            player.mouseLock = true;
            IsMouseVisible = !player.mouseLock;

            for(int x = -20; x < 20 ; x++) {
                for(int y = -20; y < 20 ; y++) {
                    blocks.Add(new Block(new Vector3(x, -(float)rand.Next(0, 10), y), new Vector3(1, 1, 1), textures[rand.Next(0, 14)], 1));
                }
            }

        }

        protected override void Initialize() {
            base.Initialize();
            ResourceManager.GraphicsDevice = GraphicsDevice;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("ArialFont");
            
            for (int b = 0; b < blocks.Count; b++) {if (!texDict.ContainsKey(blocks[b].texture)) {texDict[blocks[b].texture] = Content.Load<Texture2D>(blocks[b].texture);texDict[blocks[b].texture].GraphicsDevice.SamplerStates[0] = new SamplerState { Filter = TextureFilter.Point, AddressU = TextureAddressMode.Wrap, AddressV = TextureAddressMode.Wrap };}};foreach (string key in texDict.Keys) {effDict[key] = new BasicEffect(GraphicsDevice) {TextureEnabled = true,Texture = texDict[key],Projection = Matrix.CreatePerspectiveFieldOfView(player.fieldOfView, GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f)};};

        }

        protected override void Update(GameTime gameTime) {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            IsMouseVisible = !player.mouseLock;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            zpitoHandler.updatePlayer(player, blocks, deltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            zpitoHandler.renderAll(blocks, effDict, player);
            
            spriteBatch.Begin();
            spriteBatch.DrawString(font, player.r.ToString(), new Vector2(10, 10), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}