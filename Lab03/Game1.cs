using Lab02William;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;

namespace Lab03
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        public float ElapsedSeconds;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set the game for GameObject
            GameObject.SetGame(this);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // base.Initialize();
            LoadContent();

            Ball ball = new Ball("ball");

            foreach (var (_, obj) in GameObjectCollection.Objects)
            {
                obj.Initialize();
            }

            ball.Position.X = Graphics.PreferredBackBufferWidth / 2f;
            ball.Position.Y = Graphics.PreferredBackBufferHeight / 2f;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Content.Load<Texture2D>("ball_red_small");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // base.Update(gameTime);
            ElapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var (_, obj) in GameObjectCollection.Objects)
            {
                obj.Update();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // base.Draw(gameTime);
            foreach (var (_, obj) in GameObjectCollection.Objects)
            {
                obj.Draw();
            }
        }
    }
}