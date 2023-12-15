using System;
using GameAlgoT2310;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Lab05
{
	public class Background : GameObject, ICollidable
	{

		public Texture2D Texture;
		public Rectangle Rectangle;
		private Rectangle _bound;

		public Background(string name) : base(name)
		{

		}

        public override void Initialize()
        {
			Texture = _game.Content.Load<Texture2D>("purple");
			Rectangle.X = 0;
			Rectangle.Y = 0;
			Rectangle.Width = _game.Graphics.PreferredBackBufferWidth;
			Rectangle.Height = _game.Graphics.PreferredBackBufferHeight;

			_bound = Rectangle;

        }

		public string GetGroupName()
		{
			return this.GetType().Name;
		}

		public Rectangle GetBound()
		{
			return _bound;
		}

		public override void Draw()
		{
			_game.SpriteBatch.Begin(samplerState: SamplerState.LinearWrap);
			_game.SpriteBatch.Draw(Texture, Vector2.Zero, Rectangle, Color.White);
			_game.SpriteBatch.End();
		}
    }
}

