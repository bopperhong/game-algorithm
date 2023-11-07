using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Lab01;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Texture2D ballTexture;
    Vector2 ballPosition;
    Texture2D paddleTexture;
    Texture2D backgroundTexture;
    Vector2 leftPaddle;
    Vector2 rightPaddle;
    float paddleSpeed;


    public Game1()
    {

        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        ballPosition = new Vector2 (_graphics.PreferredBackBufferWidth / 2,
        _graphics.PreferredBackBufferHeight / 2);

        leftPaddle = new Vector2(50, _graphics.PreferredBackBufferHeight / 2);
        rightPaddle = new Vector2(750, _graphics.PreferredBackBufferHeight / 2);
        paddleSpeed = 150f;


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        // Create a new SpriteBatch, which can be used to draw textures.
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        ballTexture = Content.Load<Texture2D>("ball_red_small");

        paddleTexture = Content.Load<Texture2D>("paddle_yellow");

        backgroundTexture = Content.Load<Texture2D>("background_green");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var kState = Keyboard.GetState();

        if (kState.IsKeyDown(Keys.W))
        {
            leftPaddle.Y -= paddleSpeed * elapsedTime;
        }

        if (kState.IsKeyDown(Keys.S))
        {
            leftPaddle.Y += paddleSpeed * elapsedTime;
        }

        if (leftPaddle.Y > _graphics.PreferredBackBufferHeight - paddleTexture.Height)
        {
            leftPaddle.Y = _graphics.PreferredBackBufferHeight - paddleTexture.Height;
        }
        if (leftPaddle.Y < paddleTexture.Height)
        {
            leftPaddle.Y = paddleTexture.Height;
        }

        if (kState.IsKeyDown(Keys.Up))
        {
            rightPaddle.Y -= paddleSpeed * elapsedTime;
        }

        if (kState.IsKeyDown(Keys.Down))
        {
            rightPaddle.Y += paddleSpeed * elapsedTime;
        }

        if (rightPaddle.Y > _graphics.PreferredBackBufferHeight - paddleTexture.Height)
        {
            rightPaddle.Y = _graphics.PreferredBackBufferHeight - paddleTexture.Height;
        }
        if (rightPaddle.Y < paddleTexture.Height)
        {
            rightPaddle.Y = paddleTexture.Height;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(samplerState: SamplerState.LinearWrap);
        _spriteBatch.Draw(
            backgroundTexture,
            new Vector2(0, 0),
            new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
            Color.White
            );
        _spriteBatch.End();


        _spriteBatch.Begin();
        _spriteBatch.Draw(
            ballTexture,
            ballPosition,
            null,
            Color.White,
            0f,
            new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f
            );
        _spriteBatch.Draw(
            paddleTexture,
            leftPaddle,
            null,
            Color.White,
            1.571f,
            new Vector2(paddleTexture.Width /2, paddleTexture.Height /2),
            new Vector2(1f,0.5f),
            SpriteEffects.None,
            0f
            );
        _spriteBatch.Draw(
            paddleTexture,
            rightPaddle,
            null,
            Color.White,
            1.571f,
            new Vector2(paddleTexture.Width /2, paddleTexture.Height /2),
            new Vector2(1f,0.5f),
            SpriteEffects.None,
            0f
            );
        _spriteBatch.End();
 

        base.Draw(gameTime);
    }
}

