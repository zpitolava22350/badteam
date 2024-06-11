using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace _3d_test {

    public static class ResourceManager {
        public static GraphicsDevice GraphicsDevice { get; set; }
    }

    public class Game1: Game {

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private BasicEffect basicEffect;
        private zpitoHandler zpitoHandler = new zpitoHandler();

        List<Block> blocks = new List<Block>();

        private Random rand = new Random();

        private Texture2D cubeTexture;

        private Player player;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //graphics.ApplyChanges();

            //consistent framerate
            IsFixedTimeStep = true;

            player = new Player(new Vector3(0, 2, 0), 5f, 0.002f);
        }

        protected override void Initialize() {

            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 144.0);

            base.Initialize();

            ResourceManager.GraphicsDevice = GraphicsDevice;

            // Hide the mouse cursor
            IsMouseVisible = false;

            // Create the cube vertices
            blocks.Add(new Block(new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1));

        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the texture
            cubeTexture = Content.Load<Texture2D>("grass");

            SamplerState customSamplerState = new SamplerState {
                Filter = TextureFilter.Point, // Nearest-neighbor interpolation
                AddressU = TextureAddressMode.Wrap, // Clamp texture horizontally
                AddressV = TextureAddressMode.Wrap // Clamp texture vertically
            };

            // Apply the custom SamplerState to the texture
            cubeTexture.GraphicsDevice.SamplerStates[0] = customSamplerState;

            // Set up the basic effect
            basicEffect = new BasicEffect(GraphicsDevice) {
                TextureEnabled = true,
                Texture = cubeTexture,
                Projection = Matrix.CreatePerspectiveFieldOfView(player.fieldOfView, GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f)
            };

            //graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        protected override void Update(GameTime gameTime) {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            zpitoHandler.updatePlayer(player, blocks, deltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Create the rotation matrix from the yaw and pitch
            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(player.r, player.t, 0);

            // Define the camera's look direction and up vector
            Vector3 lookDirection = Vector3.Transform(Vector3.Forward, rotationMatrix);
            Vector3 upDirection = Vector3.Transform(Vector3.Up, rotationMatrix);

            // Create the view matrix
            Matrix viewMatrix = Matrix.CreateLookAt(player.position, player.position + lookDirection, upDirection);

            // Apply the view matrix to the basic effect
            basicEffect.View = viewMatrix;

            foreach (var pass in basicEffect.CurrentTechnique.Passes) {
                pass.Apply();
                zpitoHandler.renderAll(blocks);
            }

            base.Draw(gameTime);
        }
    }
}