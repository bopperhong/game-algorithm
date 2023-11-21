using System;
using GameAlgoT2310;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Lab04
{
	public class Spaceship : GameObject
	{

		public Texture2D spaceShip_texture;
		public Vector2 spaceShipPosition;
		float flyingSpeed;

		public Spaceship(string name) : base(name)
		{

		}

        public override void Initialize()
        {
			spaceShip_texture = _game.Content.Load<Texture2D>("spaceShips_009_right");
			spaceShipPosition = new Vector2(_game.Graphics.PreferredBackBufferWidth / 2, _game.Graphics.PreferredBackBufferHeight / 2);
			flyingSpeed = 100f;

        }

		public override void Draw()
		{
			_game.SpriteBatch.Begin();
			_game.SpriteBatch.Draw(spaceShip_texture, spaceShipPosition, null, Color.White);
			_game.SpriteBatch.End();
		}

        public override void Update()
        {

			var kState = Keyboard.GetState();
			if (kState.IsKeyDown(Keys.W))
			{
				spaceShipPosition.Y -= flyingSpeed * ScalableGameTime.DeltaTime;
			}
		}
	}
}

