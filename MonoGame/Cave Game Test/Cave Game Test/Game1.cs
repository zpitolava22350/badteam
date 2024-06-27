using Cave_Game_Test;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;

namespace Cave_Game_Test {

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

        FastNoiseLite noise = new FastNoiseLite();
        private float noiseScale = 5f;
        private float threshold = 0.3f;

        private int renderDistance = 40;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            AllocConsole();

            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

            //target framerate
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 144.0);

            //consistent framerate
            IsFixedTimeStep = true;

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();

            player = new Player(new Vector3(0, 5, 0), 50f);

            player.mouseLock = true;
            IsMouseVisible = !player.mouseLock;

            float low = noise.GetNoise(rand.NextSingle() * 10000, rand.NextSingle() * 10000, rand.NextSingle() * 10000);
            float high = noise.GetNoise(rand.NextSingle() * 10000, rand.NextSingle() * 10000, rand.NextSingle() * 10000);
            float temp;

            for(int i = 0; i < 10000; i++) {
                temp = noise.GetNoise(rand.NextSingle()*10000, rand.NextSingle()*10000, rand.NextSingle()*10000);
                low = Math.Min(low, temp);
                high = Math.Max(high, temp);
            }

            Console.WriteLine(low.ToString());
            Console.WriteLine(high.ToString());

            do {
                noise.SetSeed(rand.Next());
            } while (noise.GetNoise(player.x, player.y, player.z) > threshold*0.9f);

            for (int x = -renderDistance; x < renderDistance+1; x++) {
                for (int y = -renderDistance; y < renderDistance+1; y++) {
                    for (int z = -renderDistance; z < renderDistance+1; z++) {
                        if (noise.GetNoise(x * noiseScale, y * noiseScale, z * noiseScale) > threshold) {
                            if(
                                noise.GetNoise((x + 1) * noiseScale, y * noiseScale, z * noiseScale) < threshold ||
                                noise.GetNoise((x - 1) * noiseScale, y * noiseScale, z * noiseScale) < threshold ||
                                noise.GetNoise(x * noiseScale, (y + 1) * noiseScale, z * noiseScale) < threshold ||
                                noise.GetNoise(x * noiseScale, (y - 1) * noiseScale, z * noiseScale) < threshold ||
                                noise.GetNoise(x * noiseScale, y * noiseScale, (z + 1) * noiseScale) < threshold ||
                                noise.GetNoise(x * noiseScale, y * noiseScale, (z - 1) * noiseScale) < threshold
                                ) {
                                blocks.Add(new Block(new Vector3(x, y, z), new Vector3(1, 1, 1), "stone", 1));
                            }
                        }
                    }
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

            for (int b = 0; b < blocks.Count; b++) { 
                if (!texDict.ContainsKey(blocks[b].texture)) { 
                    texDict[blocks[b].texture] = Content.Load<Texture2D>(blocks[b].texture); 
                    texDict[blocks[b].texture].GraphicsDevice.SamplerStates[0] = new SamplerState { Filter = TextureFilter.Point, AddressU = TextureAddressMode.Wrap, AddressV = TextureAddressMode.Wrap };
                } 
            }; 
            foreach (string key in texDict.Keys) { 
                effDict[key] = new BasicEffect(GraphicsDevice) {
                    TextureEnabled = true, 
                    Texture = texDict[key], 
                    Projection = Matrix.CreatePerspectiveFieldOfView(player.fieldOfView, GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f) 
                };
            };

        }

        void unloadBlocks() {
            for(int b = blocks.Count-1; b >= 0; b--) {
                if (blocks[b].x < player.x - renderDistance || blocks[b].x > player.x + renderDistance ||
                    blocks[b].y < player.y - renderDistance || blocks[b].y > player.y + renderDistance ||
                    blocks[b].z < player.z - renderDistance || blocks[b].z > player.z + renderDistance) {
                    blocks.RemoveAt(b);
                }
            }
        }

        void loadBlocks(Vector3 old) {

            for (int x = -renderDistance; x < renderDistance+1; x++) {
                for (int y = -renderDistance; y < renderDistance+1; y++) {
                    for (int z = -renderDistance; z < renderDistance+1; z++) {
                        if (x+old.X >= renderDistance || x+old.X <= -renderDistance ||
                            y+old.Y >= renderDistance || y+old.Y <= -renderDistance ||
                            z+old.Z >= renderDistance || z+old.Z <= -renderDistance) {
                            if (noise.GetNoise((x + (int)Math.Round(player.x)) * noiseScale, (y + (int)Math.Round(player.y)) * noiseScale, (z + (int)Math.Round(player.z)) * noiseScale) > threshold) {
                                if (
                                noise.GetNoise(((x + (int)Math.Round(player.x)) + 1) * noiseScale, (y + (int)Math.Round(player.y)) * noiseScale, (z + (int)Math.Round(player.z)) * noiseScale) < threshold ||
                                noise.GetNoise(((x + (int)Math.Round(player.x)) - 1) * noiseScale, (y + (int)Math.Round(player.y)) * noiseScale, (z + (int)Math.Round(player.z)) * noiseScale) < threshold ||
                                noise.GetNoise((x + (int)Math.Round(player.x)) * noiseScale, ((y + (int)Math.Round(player.y)) + 1) * noiseScale, (z + (int)Math.Round(player.z)) * noiseScale) < threshold ||
                                noise.GetNoise((x + (int)Math.Round(player.x)) * noiseScale, ((y + (int)Math.Round(player.y)) - 1) * noiseScale, (z + (int)Math.Round(player.z)) * noiseScale) < threshold ||
                                noise.GetNoise((x + (int)Math.Round(player.x)) * noiseScale, (y + (int)Math.Round(player.y)) * noiseScale, ((z + (int)Math.Round(player.z)) + 1) * noiseScale) < threshold ||
                                noise.GetNoise((x + (int)Math.Round(player.x)) * noiseScale, (y + (int)Math.Round(player.y)) * noiseScale, ((z + (int)Math.Round(player.z)) - 1) * noiseScale) < threshold
                                ) {
                                    blocks.Add(new Block(new Vector3(x + (int)Math.Round(player.x), y + (int)Math.Round(player.y), z + (int)Math.Round(player.z)), new Vector3(1, 1, 1), "stone", 1));
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime) {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            IsMouseVisible = !player.mouseLock;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Vector3 oldPos = player.position;

            zpitoHandler.updatePlayer(player, blocks, deltaTime);

            if(!(Math.Round(player.x) == Math.Round(oldPos.X) && Math.Round(player.y) == Math.Round(oldPos.Y) && Math.Round(player.z) == Math.Round(oldPos.Z))) {
                int xdiff = (int)Math.Round(player.x) - (int)Math.Round(oldPos.X);
                int ydiff = (int)Math.Round(player.y) - (int)Math.Round(oldPos.Y);
                int zdiff = (int)Math.Round(player.z) - (int)Math.Round(oldPos.Z);
                Vector3 oldv3 = new Vector3(xdiff, ydiff, zdiff);
                unloadBlocks();
                loadBlocks(oldv3);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            zpitoHandler.renderAll(blocks, effDict, player);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, noise.GetNoise(player.x, player.y, player.z).ToString(), new Vector2(10, 10), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}