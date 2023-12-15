using Lab05;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
            System.Diagnostics.Debug.WriteLine(Graphics.PreferredBackBufferWidth );
            int randY = 0, randX = 0;
            int spawnMargin = 100; // Adjust this margin as needed
            if (Random.Next(2) == 0) // Randomly choose either left/right or top/bottom
            {
                if (Random.Next(2) == 0)
                {
                    randX = -spawnMargin; // Spawn to the left
                }
                else
                {
                    randX = Graphics.PreferredBackBufferWidth + spawnMargin; // Spawn to the right
                }
            }
            else
            {
                if (Random.Next(2) == 1)
                {
                    randY = -spawnMargin; // Spawn at the top
                }
                else
                {
                    randY = Graphics.PreferredBackBufferHeight + spawnMargin; // Spawn at the bottom
                }
            }
            Vector2 newPosition = new Vector2(randX, randY);
            //Vector2 newPosition = new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
            Background background = new Background("background");
            Spaceship spaceship = new Spaceship("spaceship");
            Missile missile = new Missile("missile", spaceship);
            Asteroid asteroid = new Asteroid("asteroid", newPosition);

            // Initialize all game objects
            GameObjectCollection.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Pre-load all assets here (e.g. textures, sprite font, etc.)
            // e.g. Content.Load<Texture2D>("texture-name")
            Content.Load<Texture2D>("spaceShips_009_right");
            Content.Load<Texture2D>("crosshair179");
            Content.Load<Texture2D>("purple");
            Content.Load<Texture2D>("spaceMeteors_002_small");

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