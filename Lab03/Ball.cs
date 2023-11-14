using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab03
{
    class Ball : GameObject
    {
        public Texture2D Texture;
        public Rectangle Bound;
        public float Speed;
        public Vector2 Velocity;

        private Random _rand = new Random();

        public Ball(string name) : base(name)
        {

        }

        public override void Initialize()
        {
            LoadContent();

            Origin.X = Texture.Width / 2f;
            Origin.Y = Texture.Height / 2f;
            Speed = 30f;
            Velocity = Speed * ComputeRandomDirection();
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("ball_red_small");
        }

        public override void Update()
        {
            Vector2 displacement = Velocity * _game.ElapsedSeconds;
            Position += displacement;
        }

        public override void Draw()
        {
            _game.SpriteBatch.Begin();
            _game.SpriteBatch.Draw(Texture, Position, null, Color.White
                                  , Orientation, Origin, Scale, SpriteEffects.None
                                  , 0f);
            _game.SpriteBatch.End();
        }

        private Vector2 ComputeRandomDirection()
        {
            // Generate "safe directions" where |Y| << |X| 
            float amount = _rand.NextSingle();
            float sign = _rand.NextSingle() < 0.5f ? -1f : 1f;

            Vector2 first = new Vector2(_game.Graphics.PreferredBackBufferWidth * 0.75f, -_game.Graphics.PreferredBackBufferHeight * 0.5f);
            Vector2 second = new Vector2(_game.Graphics.PreferredBackBufferWidth * 0.75f, _game.Graphics.PreferredBackBufferHeight * 0.5f);
            Vector2 direction = Vector2.Normalize(sign * Vector2.Lerp(first, second, amount));

            return direction;
        }
    }
}
