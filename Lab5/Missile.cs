using System;
using GameAlgoT2310;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Xna.Framework.Audio;

namespace Lab05
{
	public class Missile : GameObject, ICollidable
	{

		public Texture2D missile_texture;
		public Vector2 missilePosition;
        public Vector2 missileAngle;
		public bool isShooting;
		public double cooldownTimer;
        public Spaceship spaceship;
        public const int MaxMissiles = 5;
        public List<MissileInstance> missiles;
        public int missilesFiredCount;
        public const double firingInterval = 0.1;
        public const double burstInterval = 5;
        public const double cooldownInterval = 1;
        public double firingTimer;
        public bool horizontalBurstMode;
        public MouseState previousMouseState;
        public float gap = -30f;
        public double burstCooldown;
        public double burstTimer;
        public SoundEffect missile_effect;
        private Rectangle _bound;
        

        public Missile(string name, Spaceship spaceship) : base(name)
		{
            this.spaceship = spaceship;
            missiles = new List<MissileInstance>();
            horizontalBurstMode = false;
		}

        public override void Initialize()
        {
			missile_texture = _game.Content.Load<Texture2D>("crosshair179");
            cooldownTimer = 0;
            burstTimer = -1;
            burstCooldown = 0;
            //missile_effect = _game.Content.Load<SoundEffect>("laserShooting");

            _bound.Width = missile_texture.Width;
            _bound.Height = missile_texture.Height;
            _bound.Location = Position.ToPoint();
            _game.CollisionEngine.Listen(typeof(Background), typeof(Missile), CollisionEngine.NotAABB);

        }

        public string GetGroupName()
        {
            return this.GetType().Name;
        }

        public Rectangle GetBound()
        {
            _bound.Location = (Position - Origin).ToPoint();
            return _bound;
        }

        public void OnCollision(CollisionInfo collisionInfo)
        {
            if (collisionInfo.Other is Background)
            {
                GameObjectCollection.DeInstantiate(this);
            }
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
                    0f, 
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
            if (mouseState.LeftButton == ButtonState.Pressed && cooldownTimer <= 0)
            {
                // Start shooting
                isShooting = true;
                missilesFiredCount = 0;
                if (!horizontalBurstMode)
                {
                    cooldownTimer = cooldownInterval;
                }
            }

            //from chatgpt (modified)
            if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
            {
                
                horizontalBurstMode = !horizontalBurstMode;
                burstTimer = 2;
                burstCooldown = 0;
            }
            else if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released && horizontalBurstMode)
            {
                horizontalBurstMode = !horizontalBurstMode;
                isShooting = true;
            }

                previousMouseState = mouseState;

            if (isShooting && missilesFiredCount < MaxMissiles )
            {
                if (horizontalBurstMode)
                {
                    gap = -30f;
                    for (int i = 0; i < MaxMissiles; i++)
                    {
                        float missile_gap = gap;
                        missiles.Add(new MissileInstanceHorizontalBurst(spaceship.spaceShipPosition, missile_gap));
                        missilesFiredCount++;
                        gap += 15f;
                    }
                    isShooting = false;
                }
                else
                {
                    firingTimer -= ScalableGameTime.DeltaTime;

                    if (firingTimer <= 0)
                    {
                        missiles.Add(new MissileInstance(spaceship.spaceShipPosition, gap));
                        missilesFiredCount++;
                        firingTimer = firingInterval;
                    }
                }
            }

            if (isShooting)
            {
                foreach (var missileInstance in missiles)
                {
                    missileInstance.Update();
                }

                //if (missiles.All(missile => IsOutOfScreenBounds(missile.position)))
                //{
                    
                //    missiles.Clear();
                //}
            }

            // Update cooldown timer
            if (cooldownTimer > 0)
            {
                cooldownTimer -= ScalableGameTime.DeltaTime;
            }
            if (burstTimer > 0)
            {
                burstTimer -= ScalableGameTime.DeltaTime;
            }
            if (burstCooldown > 0)
            {
                burstCooldown -= ScalableGameTime.DeltaTime;
            }
            if (horizontalBurstMode && burstTimer < 0)
            {
                burstCooldown = burstInterval;
                isShooting = true;
                horizontalBurstMode = !horizontalBurstMode;
            }
        }

        //private bool IsOutOfScreenBounds(Vector2 position)
        //{
        //    return position.X < 0 || position.X > _game.Graphics.PreferredBackBufferWidth ||
        //           position.Y < 0 || position.Y > _game.Graphics.PreferredBackBufferHeight;
        //}

        public class MissileInstance
        {
            public Vector2 position;
            public Vector2 missileAngle;
            public float speed;
            public float verticalOffset;

            public MissileInstance(Vector2 startPosition, float gap)
            {
                position = startPosition;
                speed = 300f;

                var mouseState = Mouse.GetState();
                float angle = (float)Math.Atan2(mouseState.Y - position.Y, mouseState.X - position.X);

                missileAngle = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                // Apply the delay to the missile
                position += missileAngle * speed * ScalableGameTime.DeltaTime;

            }

            public void Update()
            {
                //position += missileAngle * speed * ScalableGameTime.DeltaTime;
                verticalOffset += 5f * ScalableGameTime.DeltaTime;
                float sineWave = (float)Math.Sin(verticalOffset);
                position += new Vector2(missileAngle.X , missileAngle.Y + sineWave) * 5f;
                //position += missileAngle * sineWave * 5f;
            }
        }

        public class MissileInstanceHorizontalBurst : MissileInstance
        {
            public float missile_gap;
            
            public MissileInstanceHorizontalBurst(Vector2 startPosition, float gap) : base(startPosition, gap)
            {
                float speed = 500f;

                missile_gap = gap;
                var mouseState = Mouse.GetState();
                float angle = (float)Math.Atan2(mouseState.Y - position.Y, mouseState.X - position.X);

                missileAngle = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));


                float deltaX = missile_gap * (float)Math.Asinh(missileAngle.Y);
                float deltaY = missile_gap * (float)Math.Asinh(missileAngle.X);
                position.X = startPosition.X + deltaX ;
                position.Y = startPosition.Y + deltaY ;
                //position += missileAngle * speed * ScalableGameTime.DeltaTime;

            }


        }
    }
}

