using Lab04;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameAlgoT2310
{
    public class GameEngine : Game
    {
        // Graphics Device and Sprite Batch made public
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        // Engine Related
        public CollisionEngine CollisionEngine;
        public Random Random;

        public GameEngine()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Initialize Game Object
            GameObject.SetGame(this);

            // Initialize Engines
            CollisionEngine = new CollisionEngine();
            Random = new Random();

            // Initialize Scalable Game Time
            ScalableGameTime.TimeScale = 1f;
        } 

        protected override void Initialize()
        {
            LoadContent();

            // Construct game objects here.           
            Background background = new Background("background");
            Spaceship spaceship = new Spaceship("spaceship");

            // Initialize all game objects
            GameObjectCollection.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Pre-load all assets here (e.g. textures, sprite font, etc.)
            // e.g. Content.Load<Texture2D>("texture-name")

        }

        protected override void Update(GameTime gameTime)
        {
            // Compute scaled time
            ScalableGameTime.Process(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update Game Objects
            GameObjectCollection.Update();

            // Update Collision Engine
            CollisionEngine.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GameObjectCollection.Draw();
        }

        protected override void EndDraw()
        {
            base.EndDraw();
            GameObjectCollection.EndDraw();
        }
    }
}