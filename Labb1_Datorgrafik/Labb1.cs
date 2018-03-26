﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Labb1_Datorgrafik
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Labb1 : Game
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice gd;
        SpriteBatch spriteBatch;
        Camera cam;

        //BasicEffect for rendering
        BasicEffect basicEffect;

        //Geometric info
        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;

        //Orbit
        bool orbit = false;

        ComponentManager cm = ComponentManager.GetInstance();
        SystemManager sm = SystemManager.GetInstance();

        public Labb1()
        {
            graphics = new GraphicsDeviceManager(this);
            gd = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, new PresentationParameters());
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Camera
            cam = new Camera(gd);


            //BasicEffect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;

            // Want to see the colors of the vertices, this needs to be on
            basicEffect.VertexColorEnabled = true;

            //Lighting requires normal information which VertexPositionColor does not have
            //If you want to use lighting and VPC you need to create a  custom def
            basicEffect.LightingEnabled = false;

            //Geometry  - a simple triangle about the origin
            triangleVertices = new VertexPositionColor[3];
            triangleVertices[0] = new VertexPositionColor(new Vector3(
                                  0, 20, 0), Color.Red);
            triangleVertices[1] = new VertexPositionColor(new Vector3(-
                                  20, -20, 0), Color.Green);
            triangleVertices[2] = new VertexPositionColor(new Vector3(
                                  20, -20, 0), Color.Blue);

            //Vert buffer
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(
                           VertexPositionColor), 3, BufferUsage.
                           WriteOnly);
            vertexBuffer.SetData(triangleVertices);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                cam.CamPosition = new Vector3(cam.CamPosition.X -1f, cam.CamPosition.Y, cam.CamPosition.Z);
                cam.CamTarget = new Vector3(cam.CamTarget.X - 1f, cam.CamTarget.Y, cam.CamTarget.Z);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                cam.CamPosition = new Vector3(cam.CamPosition.X + 1f, cam.CamPosition.Y, cam.CamPosition.Z);
                cam.CamTarget = new Vector3(cam.CamTarget.X + 1f, cam.CamTarget.Y, cam.CamTarget.Z);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                cam.CamPosition = new Vector3(cam.CamPosition.X, cam.CamPosition.Y - 1f, cam.CamPosition.Z);
                cam.CamTarget = new Vector3(cam.CamTarget.X, cam.CamTarget.Y - 1f, cam.CamTarget.Z);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                cam.CamPosition = new Vector3(cam.CamPosition.X, cam.CamPosition.Y + 1f, cam.CamPosition.Z);
                cam.CamTarget = new Vector3(cam.CamTarget.X, cam.CamTarget.Y + 1f, cam.CamTarget.Z);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                cam.CamPosition = new Vector3(cam.CamPosition.X, cam.CamPosition.Y, cam.CamPosition.Z + 1f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                cam.CamPosition = new Vector3(cam.CamPosition.X, cam.CamPosition.Y, cam.CamPosition.Z - 1f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                orbit = !orbit;
            }

            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                cam.CamPosition = Vector3.Transform(cam.CamPosition, rotationMatrix);
            }
            cam.ViewMatrix = Matrix.CreateLookAt(cam.CamPosition, cam.CamTarget, Vector3.Up);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            basicEffect.Projection = cam.ProjectionMatrix;
            basicEffect.View = cam.ViewMatrix;
            basicEffect.World = cam.WorldMatrix;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            //Turn off culling so we see both sides of our rendered triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            base.Draw(gameTime);
        }
    }
}