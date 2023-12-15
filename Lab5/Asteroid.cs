using System;
using GameAlgoT2310;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Lab05
{
    public class Asteroid : GameObject
    {

        public Texture2D asteroid_texture;
        public Vector2 asteroidPosition;
        public Vector2 asteroidAngle;
        float flyingSpeed;
        
        public float asteroidRotation;
        public List<Asteroid> asteroids;
        public Vector2 direction;
        public Asteroid(string name, Vector2 newPosition) : base(name)
        {
            asteroidPosition = newPosition;
        }

        public override void Initialize()
        {
            asteroid_texture = _game.Content.Load<Texture2D>("spaceMeteors_002_small");
            
            flyingSpeed = 300f;
            asteroidRotation = 1.0f;

            Vector2 centerScreen = new Vector2(_game.Graphics.PreferredBackBufferWidth / 2, _game.Graphics.PreferredBackBufferHeight / 2);
            float angle = (float)Math.Atan2(centerScreen.Y - asteroidPosition.Y, centerScreen.X - asteroidPosition.X);
            direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

        }

        public override void Draw()
        {
            _game.SpriteBatch.Begin();
            _game.SpriteBatch.Draw(
                asteroid_texture,
                asteroidPosition,
                null,
                Color.White,
                asteroidRotation,
                new Vector2(asteroid_texture.Width /2, asteroid_texture.Height / 2),
                1.0f,
                SpriteEffects.None,
                0
                );
            _game.SpriteBatch.End();
        }

        public override void Update()
        {
            asteroidRotation += 0.1f;
            
            
            asteroidPosition += direction * flyingSpeed * ScalableGameTime.DeltaTime;

            
        }
    }
}

