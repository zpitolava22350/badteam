using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

/*
 * 
 * TODO
 * 
 * Sprint system (and a way to toggle it off)
 * Crouching
 * maybe a flying mode (similar to the replit map editor)
 * 
 */

/*

    !!!!!!!!!!!!! ADD THIS UNDER THE NAMESPACE IN Game1.cs

    public static class ResourceManager {
        public static GraphicsDevice GraphicsDevice { get; set; }
    }

    !!!!!!!!!!!!! ADD THIS TO LOADCONTENT FUNCTION

    for (int b = 0; b < blocks.Count; b++) {if (!texDict.ContainsKey(blocks[b].texture)) {texDict[blocks[b].texture] = Content.Load<Texture2D>(blocks[b].texture);texDict[blocks[b].texture].GraphicsDevice.SamplerStates[0] = new SamplerState { Filter = TextureFilter.Point, AddressU = TextureAddressMode.Wrap, AddressV = TextureAddressMode.Wrap };}};foreach (string key in texDict.Keys) {effDict[key] = new BasicEffect(GraphicsDevice) {TextureEnabled = true,Texture = texDict[key],Projection = Matrix.CreatePerspectiveFieldOfView(player.fieldOfView, GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f)};};
    
    !!!!!!!!!!!!! ALSO CHANGE THE NAMESPACE UNDER THIS COMMENT
*/

namespace _3d_test {

    internal class Player {

        private Microsoft.Xna.Framework.Vector3 pos;
        private Microsoft.Xna.Framework.Vector3 vel;
        private Microsoft.Xna.Framework.Vector2 rot;
        private Microsoft.Xna.Framework.Vector3 prevVel;
        private Microsoft.Xna.Framework.Vector3 nextVel;
        public float deltaTime { get; private set; }
        
        private float fov;

        public float mouseSensitivity { get; set; }
        public float movementSpeed { get; set; }
        public bool onGround { get; set; }
        public bool mouseLock { get; set; }
        public bool canMove { get; set; }
        public float gravity { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public float halfWidth { get; set; }
        public float halfHeight { get; set; }
        public float stepHeight { get; set; }
        public bool debugCanStepX { get; set; }
        public bool debugCanStepZ { get; set; }
        public float jumpHeight { get; set; }
        public float damping { get; set; }
        public Microsoft.Xna.Framework.Vector3 cameraPosition { get; set; }

        public Player(Microsoft.Xna.Framework.Vector3 position, float moveSpeed) {
            this.pos = position;
            this.movementSpeed = moveSpeed;
            this.mouseSensitivity = 0.002f;
            this.mouseLock = true;
            this.canMove = true;
            this.rot = new Microsoft.Xna.Framework.Vector2(0, 0);
            this.fov = 90f;
            this.gravity = 15f;
            this.width = 0.6f;
            this.height = 1.8f;
            this.halfWidth = this.width / 2;
            this.halfHeight = this.height / 2;
            this.stepHeight = 0.6f;
            this.jumpHeight = 6f;
            this.damping = 10f;
            this.onGround = false;
            this.cameraPosition = position + new Microsoft.Xna.Framework.Vector3(0, this.halfHeight/2f, 0);
            this.debugCanStepX = false;
            this.debugCanStepZ = false;
        }

        public void bruh(float buh) {
            deltaTime = buh;
        }
        
        public Microsoft.Xna.Framework.Vector3 position {
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

        public Microsoft.Xna.Framework.Vector3 velocity {
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

        public Microsoft.Xna.Framework.Vector3 prevVelocity {
            get { return prevVel; }
            set { prevVel = value; }
        }
        public float prevxVel {
            get { return prevVel.X; }
            set { prevVel.X = value; }
        }
        public float prevyVel {
            get { return prevVel.Y; }
            set { prevVel.Y = value; }
        }
        public float prevzVel {
            get { return prevVel.Z; }
            set { prevVel.Z = value; }
        }

        public Microsoft.Xna.Framework.Vector3 nextVelocity {
            get { return nextVel; }
            set { nextVel = value; }
        }
        public float nextxVel {
            get { return nextVel.X; }
            set { nextVel.X = value; }
        }
        public float nextyVel {
            get { return nextVel.Y; }
            set { nextVel.Y = value; }
        }
        public float nextzVel {
            get { return nextVel.Z; }
            set { nextVel.Z = value; }
        }

        public float xMax {
            get { return pos.X + halfWidth; }
        }
        public float xMin {
            get { return pos.X - halfWidth; }
        }
        public float yMax {
            get { return pos.Y + halfHeight; }
        }
        public float yMin {
            get { return pos.Y - halfHeight; }
        }
        public float zMax {
            get { return pos.Z + halfWidth; }
        }
        public float zMin {
            get { return pos.Z - halfWidth; }
        }

        public float xMaxNext {
            get { return pos.X + halfWidth + (vel.X * deltaTime); }
        }
        public float xMinNext {
            get { return pos.X - halfWidth + (vel.X * deltaTime); }
        }
        public float yMaxNext {
            get { return pos.Y + halfHeight + (vel.Y * deltaTime); }
        }
        public float yMinNext {
            get { return pos.Y - halfHeight + (vel.Y * deltaTime); }
        }
        public float zMaxNext {
            get { return pos.Z + halfWidth + (vel.Z * deltaTime); }
        }
        public float zMinNext {
            get { return pos.Z - halfWidth + (vel.Z * deltaTime); }
        }

        public float xMaxNextPrev {
            get { return pos.X + halfWidth + (prevVel.X * deltaTime); }
        }
        public float xMinNextPrev {
            get { return pos.X - halfWidth + (prevVel.X * deltaTime); }
        }
        public float yMaxNextPrev {
            get { return pos.Y + halfHeight + (prevVel.Y * deltaTime); }
        }
        public float yMinNextPrev {
            get { return pos.Y - halfHeight + (prevVel.Y * deltaTime); }
        }
        public float zMaxNextPrev {
            get { return pos.Z + halfWidth + (prevVel.Z * deltaTime); }
        }
        public float zMinNextPrev {
            get { return pos.Z - halfWidth + (prevVel.Z * deltaTime); }
        }

        public Microsoft.Xna.Framework.Vector2 rotation {
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
        private Microsoft.Xna.Framework.Vector3 pos;
        private Microsoft.Xna.Framework.Vector3 scale;
        private float Wrap;
        public string texture { get; set; }

        private VertexPositionTexture[] vertices;
        private short[] indices;

        /// <summary>
        /// Game block object
        /// </summary>
        /// <param name="Pos">Position (center)</param>
        /// <param name="Size">Size</param>
        /// <param name="wrap">Texture wrapping</param>
        public Block(Microsoft.Xna.Framework.Vector3 Pos, Microsoft.Xna.Framework.Vector3 Size, string texture, float wrap) {

            this.pos = Pos;
            this.scale = Size;
            this.wrap = wrap;
            this.texture = texture;

            this.vertices = new VertexPositionTexture[24];

            float dx2 = this.scale.X / 2;
            float dy2 = this.scale.Y / 2;
            float dz2 = this.scale.Z / 2;

            // Back (z-)
            this.vertices[0] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[1] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, this.dy / this.wrap));
            this.vertices[2] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, this.dy / this.wrap));
            this.vertices[3] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, 0));

            // Front (z+)
            this.vertices[4] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[5] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, this.dy / this.wrap));
            this.vertices[6] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, this.dy / this.wrap));
            this.vertices[7] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, 0));

            // Left (x-)
            this.vertices[8] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dy / this.wrap, this.dz / this.wrap));
            this.vertices[9] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, this.dz / this.wrap));
            this.vertices[10] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[11] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dy / this.wrap, 0));

            // Right (x+)
            this.vertices[12] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dy / this.wrap, this.dz / this.wrap));
            this.vertices[13] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, this.dz / this.wrap));
            this.vertices[14] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[15] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dy / this.wrap, 0));

            // Bottom (y-)
            this.vertices[16] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[17] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, this.dz / this.wrap));
            this.vertices[18] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, this.dz / this.wrap));
            this.vertices[19] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, 0));

            // Top (y+)
            this.vertices[20] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, this.dz / this.wrap));
            this.vertices[21] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[22] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, 0));
            this.vertices[23] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, this.dz / this.wrap));

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

        public Microsoft.Xna.Framework.Vector3 position {
            get { return pos; }
            set { pos = value; this.remake(); }
        }
        public float x {
            get { return pos.X; }
            set { pos.X = value; this.remake(); }
        }
        public float y {
            get { return pos.Y; }
            set { pos.Y = value; this.remake(); }
        }
        public float z {
            get { return pos.Z; }
            set { pos.Z = value; this.remake(); }
        }

        public Microsoft.Xna.Framework.Vector3 size {
            get { return scale; }
            set { scale = value; this.remake(); }
        }
        public float dx {
            get { return scale.X; }
            set { scale.X = value; this.remake(); }
        }
        public float dy {
            get { return scale.Y; }
            set { scale.Y = value; this.remake(); }
        }
        public float dz {
            get { return scale.Z; }
            set { scale.Z = value; this.remake(); }
        }

        public float xMax {
            get { return this.x + (this.dx / 2); }
        }
        public float xMin {
            get { return this.x - (this.dx / 2); }
        }
        public float yMax {
            get { return this.y + (this.dy / 2); }
        }
        public float yMin {
            get { return this.y - (this.dy / 2); }
        }
        public float zMax {
            get { return this.z + (this.dz / 2); }
        }
        public float zMin {
            get { return this.z - (this.dz / 2); }
        }

        public float wrap {
            get { return Wrap; }
            set { Wrap = value; this.remake(); }
        }

        public VertexPositionTexture[] vert {
            get { return vertices; }
            set { vertices = value; }
        }

        public short[] ind {
            get { return indices; }
            set { indices = value; }
        }

        public void remake() {
            this.vertices = new VertexPositionTexture[24];

            float dx2 = this.scale.X / 2;
            float dy2 = this.scale.Y / 2;
            float dz2 = this.scale.Z / 2;

            // Back (z-)
            this.vertices[0] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[1] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, this.dy / this.wrap));
            this.vertices[2] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, this.dy / this.wrap));
            this.vertices[3] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, 0));

            // Front (z+)
            this.vertices[4] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[5] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, this.dy / this.wrap));
            this.vertices[6] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, this.dy / this.wrap));
            this.vertices[7] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, 0));

            // Left (x-)
            this.vertices[8] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dy / this.wrap, this.dz / this.wrap));
            this.vertices[9] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, this.dz / this.wrap));
            this.vertices[10] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[11] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dy / this.wrap, 0));

            // Right (x+)
            this.vertices[12] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dy / this.wrap, this.dz / this.wrap));
            this.vertices[13] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, this.dz / this.wrap));
            this.vertices[14] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[15] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dy / this.wrap, 0));

            // Bottom (y-)
            this.vertices[16] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[17] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, this.dz / this.wrap));
            this.vertices[18] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, this.dz / this.wrap));
            this.vertices[19] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y - dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, 0));

            // Top (y+)
            this.vertices[20] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(0, this.dz / this.wrap));
            this.vertices[21] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X - dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(0, 0));
            this.vertices[22] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z - dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, 0));
            this.vertices[23] = new VertexPositionTexture(new Microsoft.Xna.Framework.Vector3(this.pos.X + dx2, this.pos.Y + dy2, this.pos.Z + dz2), new Microsoft.Xna.Framework.Vector2(this.dx / this.wrap, this.dz / this.wrap));

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

        public void render() {
            ResourceManager.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, this.vert, 0, 24, this.ind, 0, 12);
        }

        public void collideFloor(Player player) {
            bool insideNext = false;
            if (player.xMaxNext > this.xMin && player.xMinNext < this.xMax && player.zMaxNext > this.zMin && player.zMinNext < this.zMax) {
                insideNext = true;
            }
            if (insideNext && player.yMin + 0.0001f >= this.yMax && player.yMinNext < this.yMax) {
                player.y = this.y + (this.dy/2) + player.halfHeight;
                player.yVel = 0;
                player.onGround = true;
            }
            if (insideNext && player.yMax < this.yMin && player.yMaxNext > this.yMin) {
                player.y = this.y - (this.dy / 2) - player.halfHeight;
                player.yVel = 0;
            }
        }
        public void step(Player player) {
            bool insideYNext = false;
            bool insideX = false;
            bool insideXNext = false;
            bool insideZ = false;
            bool insideZNext = false;

            if (player.yMaxNextPrev > this.yMin && player.yMinNextPrev < this.yMax) {
                insideYNext = true;
            }
            if (player.zMax > this.zMin && player.zMin < this.zMax) {
                insideZ = true;
            }
            if (player.zMaxNextPrev > this.zMin && player.zMinNextPrev < this.zMax) {
                insideZNext = true;
            }
            if (player.xMax > this.xMin && player.xMin < this.xMax) {
                insideX = true;
            }
            if (player.xMaxNextPrev > this.xMin && player.xMinNextPrev < this.xMax) {
                insideXNext = true;
            }

            if (insideXNext && insideYNext && insideZNext && player.yMin > this.yMax - player.stepHeight && player.onGround) {
                if (insideX && player.debugCanStepZ) {
                    player.nextVelocity = player.prevVelocity;
                    player.y = this.y + (this.dy / 2) + player.halfHeight;
                    player.nextyVel = 0;
                    player.onGround = true;
                }
                if (insideZ && player.debugCanStepX) {
                    player.nextVelocity = player.prevVelocity;
                    player.y = this.y + (this.dy / 2) + player.halfHeight;
                    player.nextyVel = 0;
                    player.onGround = true;
                }
            }

        }
        public void collide(Player player) {
            bool insideYNext = false;
            bool insideX = false;
            bool insideXNext = false;
            bool insideZ = false;
            bool insideZNext = false;

            if (player.yMaxNextPrev > this.yMin && player.yMinNextPrev < this.yMax) {
                insideYNext = true;
            }
            if (player.zMax > this.zMin && player.zMin < this.zMax) {
                insideZ = true;
            }
            if (player.zMaxNextPrev > this.zMin && player.zMinNextPrev < this.zMax) {
                insideZNext = true;
            }
            if (player.xMax > this.xMin && player.xMin < this.xMax) {
                insideX = true;
            }
            if (player.xMaxNextPrev > this.xMin && player.xMinNextPrev < this.xMax) {
                insideXNext = true;
            }
            if (insideYNext) {
                if (insideZNext && insideZ && player.xMin >= this.xMax && player.xMinNext < this.xMax) {
                    player.x = this.x + (this.dx / 2) + player.halfWidth;
                    player.nextxVel = 0;
                    if (player.yMin <= this.yMax - player.stepHeight) {
                        player.debugCanStepX = false;
                    }
                }
                if (insideZNext && insideZ && player.xMax <= this.xMin && player.xMaxNext > this.xMin) {
                    player.x = this.x - (this.dx / 2) - player.halfWidth;
                    player.nextxVel = 0;
                    if (player.yMin <= this.yMax - player.stepHeight) {
                        player.debugCanStepX = false;
                    }
                }
                if (insideXNext && insideX && player.zMin >= this.zMax && player.zMinNext < this.zMax) {
                    player.z = this.z + (this.dz / 2) + player.halfWidth;
                    player.nextzVel = 0;
                    if (player.yMin <= this.yMax - player.stepHeight) {
                        player.debugCanStepZ = false;
                    }
                }
                if (insideXNext && insideX && player.zMax <= this.zMin && player.zMaxNext > this.zMin) {
                    player.z = this.z - (this.dz / 2) - player.halfWidth;
                    player.nextzVel = 0;
                    if (player.yMin <= this.yMax - player.stepHeight) {
                        player.debugCanStepZ = false;
                    }
                }
            }
            // corner bug fix oh god my brain
            if (insideXNext && insideYNext && insideZNext) {
                if (!insideX && !insideZ) {
                    if (Math.Abs(player.prevxVel) < Math.Abs(player.prevzVel)) {
                        if (player.x > this.x) {
                            player.x = this.x + (this.dx / 2) + player.halfWidth;
                            player.nextxVel = 0;
                            if (player.yMin <= this.yMax - player.stepHeight) {
                                player.debugCanStepX = false;
                            }
                        } else {
                            player.x = this.x - (this.dx / 2) - player.halfWidth;
                            player.nextxVel = 0;
                            if (player.yMin <= this.yMax - player.stepHeight) {
                                player.debugCanStepX = false;
                            }
                        }
                    } else {
                        if (player.z > this.z) {
                            player.z = this.z + (this.dz / 2) + player.halfWidth;
                            player.nextzVel = 0;
                            if (player.yMin <= this.yMax - player.stepHeight) {
                                player.debugCanStepZ = false;
                            }
                        } else {
                            player.z = this.z - (this.dz / 2) - player.halfWidth;
                            player.nextzVel = 0;
                            if (player.yMin <= this.yMax - player.stepHeight) {
                                player.debugCanStepZ = false;
                            }
                        }
                    }
                }
            }

        }
    }

    class zpitoHandler {

        public void renderAll(List<Block> blockList, Dictionary<string, BasicEffect> dictionary, Player player) {

            ResourceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ResourceManager.GraphicsDevice.BlendState = BlendState.Opaque;
            ResourceManager.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            List<int> indexes = new List<int>();
            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(player.r, player.t, 0);
            Microsoft.Xna.Framework.Vector3 lookDirection = Microsoft.Xna.Framework.Vector3.Transform(Microsoft.Xna.Framework.Vector3.Forward, rotationMatrix);
            Microsoft.Xna.Framework.Vector3 upDirection = Microsoft.Xna.Framework.Vector3.Transform(Microsoft.Xna.Framework.Vector3.Up, rotationMatrix);
            Matrix viewMatrix = Matrix.CreateLookAt(player.cameraPosition, player.cameraPosition + lookDirection, upDirection);

            foreach(string key in dictionary.Keys) {
                indexes.Clear();
                for(int i = 0; i < blockList.Count; i++) {
                    if (blockList[i].texture == key) {
                        indexes.Add(i);
                        
                    }
                }
                dictionary[key].View = viewMatrix;
                ResourceManager.GraphicsDevice.SamplerStates[0] = new SamplerState { Filter = TextureFilter.Point, AddressU = TextureAddressMode.Wrap, AddressV = TextureAddressMode.Wrap };
                foreach (var pass in dictionary[key].CurrentTechnique.Passes) {
                    pass.Apply();
                    for (int i = 0; i < indexes.Count; i++) {
                        blockList[indexes[i]].render();
                    }
                }
            }

            ResourceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            ResourceManager.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            ResourceManager.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

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

            player.bruh(deltaTime);

            if (player.canMove) {
                if (player.mouseLock) {
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
                }

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
                if (kstate.IsKeyDown(Keys.Space) && player.onGround) {
                    player.yVel = player.jumpHeight;
                }
            }
            
            player.yVel -= player.gravity * deltaTime;

            player.onGround = false;
            for (int b = 0; b < blocks.Count; b++) {
                blocks[b].collideFloor(player);
            }
            player.prevVelocity = player.velocity;
            player.nextVelocity = player.velocity;
            player.debugCanStepX = true;
            player.debugCanStepZ = true;
            for (int b = 0; b < blocks.Count; b++) {
                blocks[b].collide(player);
            }
            for (int b = 0; b < blocks.Count; b++) {
                blocks[b].step(player);
            }

            player.velocity = player.nextVelocity;

            player.position += player.velocity * deltaTime;

            player.cameraPosition = player.position + new Microsoft.Xna.Framework.Vector3(0, player.halfHeight / 2f, 0);

            player.xVel = MathHelper.LerpPrecise(player.xVel, 0, Math.Min(player.damping * deltaTime, 1));
            player.zVel = MathHelper.LerpPrecise(player.zVel, 0, Math.Min(player.damping * deltaTime, 1));

            player.prevVelocity = player.velocity;

            if (player.mouseLock) {
                Mouse.SetPosition(screenCenter.X, screenCenter.Y);
            }

        }

    }
}