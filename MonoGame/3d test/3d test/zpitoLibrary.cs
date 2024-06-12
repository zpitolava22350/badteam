using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
//using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/*
 * 
 * TODO
 * 
 * Sprint system (and a way to toggle it off)
 * movement toggle (similar to mouseLock)
 * maybe a flying mode (similar to the replit map editor)
 * you fucking goober :3
 * 
 */

/*

    !!!!!!!!!!!!! REMEMBER TO ADD THIS UNDER THE NAMESPACE IN Game1.cs

    public static class ResourceManager {
        public static GraphicsDevice GraphicsDevice { get; set; }
    }
    
    !!!!!!!!!!!!! ALSO CHANGE THE NAMESPACE UNDER THIS COMMENT
*/

namespace _3d_test {

    internal class Player {

        private Vector3 pos;
        private Vector3 vel;
        private Vector2 rot;
        
        private float fov;

        public float mouseSensitivity { get; set; }
        public float movementSpeed { get; set; }
        public bool onGround { get; set; }
        public bool mouseLock { get; set; }
        public float gravity { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public float halfWidth { get; set; }
        public float halfHeight { get; set; }
        public float stepHeight { get; set; }
        public float jumpHeight { get; set; }
        public float damping { get; set; }

        public Player(Vector3 position, float moveSpeed) {
            this.pos = position;
            this.movementSpeed = moveSpeed;
            this.mouseSensitivity = 0.002f;
            this.mouseLock = true;
            this.rot = new Vector2(0, 0);
            this.fov = 90f;
            this.gravity = 15f;
            this.width = 0.6f;
            this.height = 1.8f;
            this.halfWidth = this.width / 2;
            this.halfHeight = this.height / 2;
            this.stepHeight = 0.6f;
            this.jumpHeight = 6f;
            this.damping = 0.1f;
            this.onGround = false;
        }
        
        public Vector3 position {
            get { return pos; }
            set { pos = value; }
        }
        public float x {
            get { return pos.X; }
            set { pos.X = value; }
        }
        public float y {
            get { return pos.Y; }
            set { pos.Y = value; }
        }
        public float z {
            get { return pos.Z; }
            set { pos.Z = value; }
        }

        public Vector3 velocity {
            get { return vel; }
            set { vel = value; }
        }
        public float xVel {
            get { return vel.X; }
            set { vel.X = value; }
        }
        public float yVel {
            get { return vel.Y; }
            set { vel.Y = value; }
        }
        public float zVel {
            get { return vel.Z; }
            set { vel.Z = value; }
        }

        public Vector2 rotation {
            get { return rot; }
            set { rot = value; }
        }
        public float r {
            get { return rot.Y; }
            set { rot.Y = value; }
        }
        public float t {
            get { return rot.X; }
            set { rot.X = value; }
        }

        public float fieldOfView {
            get { return fov * (MathHelper.Pi / 180f); }
            set { fov = value; }
        }

    }

    internal class Block {
        private Vector3 pos;
        private Vector3 scale;
        private float Wrap;

        private VertexPositionTexture[] vertices;
        private short[] indices;
        private Texture2D texture;

        /// <summary>
        /// Game block object
        /// </summary>
        /// <param name="Pos">Position (center)</param>
        /// <param name="Size">Size</param>
        /// <param name="wrap">Texture wrapping</param>
        public Block(Vector3 Pos, Vector3 Size, float wrap) {

            this.pos = Pos;
            this.scale = Size;
            this.wrap = wrap;

            this.vertices = new VertexPositionTexture[24];

            float dx2 = this.scale.X / 2;
            float dy2 = this.scale.Y / 2;
            float dz2 = this.scale.Z / 2;

            // Back (z-)
            this.vertices[0] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Vector2(0, 1));
            this.vertices[1] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Vector2(0, 0));
            this.vertices[2] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Vector2(1, 0));
            this.vertices[3] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Vector2(1, 1));

            // Front (z+)
            this.vertices[4] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Vector2(0, 1));
            this.vertices[5] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Vector2(0, 0));
            this.vertices[6] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Vector2(1, 0));
            this.vertices[7] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Vector2(1, 1));

            // Left (x-)
            this.vertices[8] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Vector2(0, 0));
            this.vertices[9] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Vector2(1, 0));
            this.vertices[10] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Vector2(1, 1));
            this.vertices[11] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Vector2(0, 1));

            // Right (x+)
            this.vertices[12] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Vector2(0, 0));
            this.vertices[13] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Vector2(1, 0));
            this.vertices[14] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Vector2(1, 1));
            this.vertices[15] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Vector2(0, 1));

            // Bottom (y-)
            this.vertices[16] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Vector2(0, 0));
            this.vertices[17] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Vector2(0, 1));
            this.vertices[18] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Vector2(1, 1));
            this.vertices[19] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Vector2(1, 0));

            // Top (y+)
            this.vertices[20] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Vector2(0, 0));
            this.vertices[21] = new VertexPositionTexture(new Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Vector2(0, 1));
            this.vertices[22] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Vector2(1, 1));
            this.vertices[23] = new VertexPositionTexture(new Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Vector2(1, 0));

            // Cube indices
            this.indices = new short[]
            {
                0, 2, 1, 0, 3, 2,       // Front  (z+)
                4, 5, 6, 4, 6, 7,       // Back   (z-)
                10, 11, 8, 10, 8, 9,    // Left   (x-)
                12, 13, 14, 12, 14, 15, // Right  (x+)
                16, 18, 19, 16, 17, 18, // Top    (y+)
                21, 22, 23, 21, 23, 20  // Bottom (y-)
            };
        }

        public Vector3 position {
            get { return pos; }
            set { pos = value; }
        }
        public float x {
            get { return pos.X; }
            set { pos.X = value; }
        }
        public float y {
            get { return pos.Y; }
            set { pos.Y = value; }
        }
        public float z {
            get { return pos.Z; }
            set { pos.Z = value; }
        }

        public Vector3 size {
            get { return scale; }
            set { scale = value; }
        }
        public float dx {
            get { return scale.X; }
            set { scale.X = value; }
        }
        public float dy {
            get { return scale.X; }
            set { scale.X = value; }
        }
        public float dz {
            get { return scale.X; }
            set { scale.X = value; }
        }

        public float wrap {
            get { return Wrap; }
            set { Wrap = value; }
        }

        public VertexPositionTexture[] vert {
            get { return vertices; }
            set { vertices = value; }
        }

        public short[] ind {
            get { return indices; }
            set { indices = value; }
        }

        public void Render() {
            ResourceManager.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, this.vert, 0, 24, this.ind, 0, 12);
        }

        public void collideFloor(Player player, float deltaTime) {
            bool inside = false;
            if (player.x - player.halfWidth < this.x + (this.dx * 0.5f) && player.x + player.halfWidth > this.x - (this.dx * 0.5f) && player.z - player.halfWidth < this.z + (this.dz * 0.5f) && player.z + player.halfWidth > this.z - (this.dz * 0.5f)) {
                inside = true;
            }
            if (inside) {
                if (player.y - player.halfHeight > this.y + (this.dy * 0.5f) && player.y - player.halfHeight + (player.yVel * deltaTime) < this.y + (this.dy * 0.5f)) {
                    //above, but hit ground next frame
                    player.y = this.y + (this.dy * 0.5f) + player.halfHeight + 0.0001f;
                    player.yVel = 0;
                    player.onGround = true;
                }
                if (player.y + player.halfHeight < this.y - (this.dy * 0.5f) && player.y + player.halfHeight + (player.yVel * deltaTime) > this.y - (this.dy * 0.5f)) {
                    //under, but hit head next frame
                    player.y = this.y - (this.dy * 0.5f) - player.halfHeight - 0.0001f;
                    player.yVel = 0;
                }
            }
        }
        public void collide(Player player, float deltaTime) {
            bool inY = false;
            bool canStep = false;
            if (player.y - player.halfHeight + (player.yVel * deltaTime) < this.y + (this.dy * 0.5f) && player.y + player.halfHeight + (player.yVel * deltaTime) > this.y - (this.dy * 0.5f)) {
                inY = true;
                if ((this.y + (this.dy * 0.5f)) - (player.y - player.halfHeight) <= player.stepHeight) {
                    canStep = true;
                }
            }

            if (inY) {

                bool inX = false;
                bool inXNext = false;
                bool inZ = false;
                bool inZNext = false;

                if (player.x + player.halfWidth > this.x - (this.dx * 0.5f) && player.x - player.halfWidth < this.x + (this.dx * 0.5f)) {
                    inX = true;
                }
                if (player.z + player.halfWidth > this.z - (this.dz * 0.5f) && player.z - player.halfWidth < this.z + (this.dz * 0.5f)) {
                    inZ = true;
                }
                if (player.x + player.halfWidth + (player.xVel * deltaTime) > this.x - (this.dx * 0.5f) && player.x - player.halfWidth + (player.xVel * deltaTime) < this.x + (this.dx * 0.5f)) {
                    inXNext = true;
                }
                if (player.z + player.halfWidth + (player.zVel * deltaTime) > this.z - (this.dz * 0.5f) && player.z - player.halfWidth + (player.zVel * deltaTime) < this.z + (this.dz * 0.5f)) {
                    inZNext = true;
                }

                if (inZ && !inX && inXNext) {
                    if (canStep && player.onGround) {
                        player.y = this.y + (this.dy * 0.5f) + player.halfHeight + 0.0001f;
                    } else {
                        if (player.x < this.x) {
                            player.x = this.x - (this.dx * 0.5f) - player.halfWidth;
                            player.xVel = 0;
                        }
                        if (player.x > this.x) {
                            player.x = this.x + (this.dx * 0.5f) + player.halfWidth;
                            player.xVel = 0;
                        }
                    }
                }

                if (inX && !inZ && inZNext) {
                    if (canStep && player.onGround) {
                        player.y = this.y + (this.dy * 0.5f) + player.halfHeight + 0.0001f;
                    } else {
                        if (player.z < this.z) {
                            player.z = this.z - (this.dz * 0.5f) - player.halfWidth;
                            player.zVel = 0;
                        }
                        if (player.z > this.z) {
                            player.z = this.z + (this.dz * 0.5f) + player.halfWidth;
                            player.zVel = 0;
                        }
                    }
                }

                //bugfix
                if (!inX && !inZ && inXNext && inZNext) {
                    if (Math.Abs(player.xVel) > Math.Abs(player.zVel)) {
                        if (player.z < this.z) {
                            player.z = this.z - (this.dz * 0.5f) - player.halfWidth;
                            player.zVel = 0;
                        }
                        if (player.z > this.z) {
                            player.z = this.z + (this.dz * 0.5f) + player.halfWidth;
                            player.zVel = 0;
                        }
                    } else {
                        if (player.x < this.x) {
                            player.x = this.x - (this.dx * 0.5f) - player.halfWidth;
                            player.xVel = 0;
                        }
                        if (player.x > this.x) {
                            player.x = this.x + (this.dx * 0.5f) + player.halfWidth;
                            player.xVel = 0;
                        }
                    }
                }

            }
        }
    }

    class zpitoHandler {

        public void renderAll(List<Block> blockList) {
            for(int i = 0; i < blockList.Count; i++) {
                blockList[i].Render();
            }
        }

        /// <summary>
        /// Takes in Player, Block, and Deltatime data
        /// and updates player position and rotation
        /// </summary>
        /// <param name="player">Player object</param>
        /// <param name="blocks">List of blocks</param>
        /// <param name="deltaTime">Deltatime float</param>
        public void updatePlayer(Player player, List<Block> blocks, float deltaTime) {

            Microsoft.Xna.Framework.Point screenCenter = new Microsoft.Xna.Framework.Point(ResourceManager.GraphicsDevice.Viewport.Width / 2, ResourceManager.GraphicsDevice.Viewport.Height / 2);

            // Get the current mouse state
            var mouseState = Mouse.GetState();

            // Calculate the difference between the current mouse position and the center of the screen
            int deltaX = mouseState.X - screenCenter.X;
            int deltaY = mouseState.Y - screenCenter.Y;

            // Update yaw and pitch based on mouse movement
            player.r -= deltaX * player.mouseSensitivity;
            player.t -= deltaY * player.mouseSensitivity;

            // Clamp the pitch to prevent flipping
            player.t = MathHelper.Clamp(player.t, -MathHelper.PiOver2, MathHelper.PiOver2);

            // Get keyboard state
            var kstate = Keyboard.GetState();

            // Move player
            if (kstate.IsKeyDown(Keys.W)) {
                player.xVel += (float)Math.Sin(player.r) * -player.movementSpeed * deltaTime;
                player.zVel += (float)Math.Cos(player.r) * -player.movementSpeed * deltaTime;
            }
            if (kstate.IsKeyDown(Keys.A)) {
                player.xVel += (float)Math.Sin(player.r + MathHelper.PiOver2) * -player.movementSpeed * deltaTime;
                player.zVel += (float)Math.Cos(player.r + MathHelper.PiOver2) * -player.movementSpeed * deltaTime;
            }
            if (kstate.IsKeyDown(Keys.S)) {
                player.xVel += (float)Math.Sin(player.r + MathHelper.Pi) * -player.movementSpeed * deltaTime;
                player.zVel += (float)Math.Cos(player.r + MathHelper.Pi) * -player.movementSpeed * deltaTime;
            }
            if (kstate.IsKeyDown(Keys.D)) {
                player.xVel += (float)Math.Sin(player.r - MathHelper.PiOver2) * -player.movementSpeed * deltaTime;
                player.zVel += (float)Math.Cos(player.r - MathHelper.PiOver2) * -player.movementSpeed * deltaTime;
            }
            player.yVel -= player.gravity * deltaTime;

            if (kstate.IsKeyDown(Keys.Space) && player.onGround) {
                player.yVel = player.jumpHeight;
            }

            player.onGround = false;
            for (int b = 0; b < blocks.Count; b++) {
                blocks[b].collideFloor(player, deltaTime);
            }

            for (int b = 0; b < blocks.Count; b++) {
                blocks[b].collide(player, deltaTime);
            }

            player.position += player.velocity * deltaTime;

            player.xVel = MathHelper.LerpPrecise(player.xVel, 0, player.damping);
            player.zVel = MathHelper.LerpPrecise(player.zVel, 0, player.damping);

            // Center the mouse cursor
            Mouse.SetPosition(screenCenter.X, screenCenter.Y);

        }

    }
}