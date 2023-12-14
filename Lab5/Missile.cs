using System;
using GameAlgoT2310;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Lab05
{
	public class Missile : GameObject
	{

		public Texture2D missile_texture;
		public Vector2 missilePosition;
        public Vector2 missileAngle;
		public bool isShooting;
		public double cooldownTimer;
		public const double cooldownDuration = 2.0;
        public Spaceship spaceship;
        public const int MaxMissiles = 5;
        public List<MissileInstance> missiles;

		public Missile(string name, Spaceship spaceship) : base(name)
		{
            this.spaceship = spaceship;
            missiles = new List<MissileInstance>();
		}

        public override void Initialize()
        {
			missile_texture = _game.Content.Load<Texture2D>("crosshair179");

        }

		public override void Draw()
		{
			_game.SpriteBatch.Begin();
            foreach (var missileInstance in missiles)
            {
                _game.SpriteBatch.Draw(
                    missile_texture,
                    missileInstance.position,
                    null,
                    Color.White,
                    0f, // No rotation for now, you can add rotation logic if needed
                    new Vector2(missile_texture.Width / 2, missile_texture.Height / 2),
                    1.0f,
                    SpriteEffects.None,
                    0
                );
            }
            _game.SpriteBatch.End();
		}

        public override void Update()
        {

			var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && !isShooting && cooldownTimer <= 0)
            {
                // Start shooting
                isShooting = true;

                missiles.Clear();
                for (int i =0; i< MaxMissiles; i++)
                {
                    float delay = i * 5f;
                    missiles.Add(new MissileInstance(spaceship.spaceShipPosition, delay));
                }


                // Reset cooldown timer
                cooldownTimer = cooldownDuration;

                
            }

            if (isShooting)
            {


                // Update missile positions
                foreach (var missileInstance in missiles)
                {
                    missileInstance.position += missileInstance.missileAngle * missileInstance.speed * ScalableGameTime.DeltaTime;
                }

                // Check if all missiles are out of bounds
                if (missiles.All(missile => IsOutOfScreenBounds(missile.position)))
                {
                    // Stop shooting if all missiles are out of bounds
                    isShooting = false;
                }
            }

            // Update cooldown timer
            if (cooldownTimer > 0)
            {
                cooldownTimer -= ScalableGameTime.DeltaTime;
            }
        }

        private bool IsOutOfScreenBounds(Vector2 position)
        {
            return position.X < 0 || position.X > _game.Graphics.PreferredBackBufferWidth ||
                   position.Y < 0 || position.Y > _game.Graphics.PreferredBackBufferHeight;
        }

        public class MissileInstance
        {
            public Vector2 position;
            public Vector2 missileAngle;
            public float speed;

            public MissileInstance(Vector2 startPosition, float delay)
            {
                position = startPosition;
                speed = 300f;

                var mouseState = Mouse.GetState();
                float angle = (float)Math.Atan2(mouseState.Y - position.Y, mouseState.X - position.X);

                missileAngle = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                missileAngle.Normalize();
                // Apply the delay to the missile
                position += missileAngle * delay * speed * ScalableGameTime.DeltaTime;
            }
        }
	}
}

