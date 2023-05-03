using Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Model;
using MonoGame;
using SharpDX.Direct3D9;
using System;

namespace track_designer
{
    public class TrackDesignerGame : Game
    {
        //XNA graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        State state;
        UpdateAgent updateAgent;
        DrawAgent drawAgent;

        public TrackDesignerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            state = new State();
            updateAgent = new UpdateAgent(state);

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            drawAgent = new DrawAgent(state, spriteBatch);
        }

        protected override void LoadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            updateAgent.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            drawAgent.Draw(gameTime);

            base.Draw(gameTime);
        }

    }
}