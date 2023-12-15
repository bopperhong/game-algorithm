using System;
using GameAlgoT2310;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Lab05
{
    public class Spaceship : GameObject
    {

        public Texture2D spaceShip_texture;
        public Vector2 spaceShipPosition;
        public Vector2 spaceShipAngle;
        float flyingSpeed;
        public float angle;

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
            _game.SpriteBatch.Draw(
                spaceShip_texture,
                spaceShipPosition,
                null,
                Color.White,
                (float)angle,
                new Vector2(spaceShip_texture.Width /2, spaceShip_texture.Height / 2),
                1.0f,
                SpriteEffects.None,
                0
                );
            _game.SpriteBatch.End();
        }

        public override void Update()
        {

            var kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.W))
            {
                spaceShipPosition.Y -= flyingSpeed * ScalableGameTime.DeltaTime;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                spaceShipPosition.Y += flyingSpeed * ScalableGameTime.DeltaTime;
            }
            if (kState.IsKeyDown(Keys.A))
            {
                spaceShipPosition.X -= flyingSpeed * ScalableGameTime.DeltaTime;
            }
            if (kState.IsKeyDown(Keys.D))
            {
                spaceShipPosition.X += flyingSpeed * ScalableGameTime.DeltaTime;
            }

            var mouseState = Mouse.GetState();

            angle = (float)Math.Atan2(mouseState.Y - spaceShipPosition.Y, mouseState.X - spaceShipPosition.X);
            spaceShipAngle.X = (float)Math.Cos(angle);
            spaceShipAngle.Y = (float)Math.Sin(angle);
        }
    }
}

