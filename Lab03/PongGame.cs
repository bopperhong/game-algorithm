using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Lab02William
{
    public class PongGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Background and Centre Vertical Line
        private Texture2D _backgroundTexture;
        private Rectangle _backgroundBound;
        private Rectangle _backgroundBoundMonogame;
        private Vector2 _linePosition;
        private float _lineLength;
        private float _lineThickness;

        // Ball
        private Texture2D _ballTexture;
        private Rectangle _ballBound;
        private Vector2 _ballOrigin;
        private Vector2 _ballPosition;
        private Vector2 _ballScale;
        private float _ballSpeed;
        private Vector2 _ballVelocity;
        private Vector2 _ballDisplacement;

        // Paddle (Common Data)
        private Vector2 _paddleOrigin;
        private Vector2 _paddleScale;
        private float _paddleOrientation;

        // Left Paddle
        private Texture2D _leftPaddleTexture;
        private Rectangle _leftPaddleBound;
        private Vector2 _leftPaddlePosition;
        private float _leftPaddleSpeed;
        private Vector2 _leftPaddleVelocity;
        private Keys _leftPaddleMoveUpKey;
        private Keys _leftPaddleMoveDownKey;

        // Right Paddle
        private Texture2D _rightPaddleTexture;
        private Rectangle _rightPaddleBound;
        private Vector2 _rightPaddlePosition;
        private float _rightPaddleSpeed;
        private Vector2 _rightPaddleVelocity;
        private Keys _rightPaddleMoveUpKey;
        private Keys _rightPaddleMoveDownKey;

        // Scoring System
        private int _scorePlayerOne;
        private int _scorePlayerTwo;
        private Vector2 _scorePlayerOnePosition;
        private Vector2 _scorePlayerTwoPosition;
        private string _gameoverText;
        private Vector2 _gameoverTextPosition;
        private Vector2 _scorePositionOffset;
        private Vector2 _scoreDimensions;
        private Vector2 _gameoverDimensions;
        private SpriteFont _spriteFont;
        private bool _isGameOver;

        // Game States
        private enum GameState
        {
            Start,
            Playing,
            GameOver
        }
        private GameState _gameState;

        // Useful Positional Variables
        private Vector2 _windowCentrePosition;
        private Vector2 _paddlePositionOffset;

        // Others
        private Random _rand;
        private float _timeScale;

        public PongGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Initialize Random
            _rand = new Random();

            _timeScale = 1.0f;
        }

        protected override void Initialize()
        {
            LoadContent();

            // Useful Positional Variables
            _windowCentrePosition = new Vector2(_graphics.PreferredBackBufferWidth / 2f, _graphics.PreferredBackBufferHeight / 2f);
            _paddlePositionOffset = new Vector2(0.9f * _windowCentrePosition.X, 0f);

            // Background & Lines
            _backgroundBound.Width = _graphics.PreferredBackBufferWidth;
            _backgroundBound.Height = _graphics.PreferredBackBufferHeight;
            _backgroundBoundMonogame.Width = _graphics.PreferredBackBufferWidth;
            _backgroundBoundMonogame.Height = _graphics.PreferredBackBufferHeight;
            _linePosition = new Vector2(_backgroundBound.Width / 2f, 0f);
            _lineLength = _backgroundBound.Height;
            _lineThickness = 10f;

            // Ball
            _ballScale = Vector2.One;
            _ballOrigin = CentreOrigin(_ballTexture);
            _ballPosition = _windowCentrePosition;
            _ballSpeed = 350f;
            _ballVelocity = _ballSpeed * Ball_ComputeRandomDirection();
            _ballBound.X = (int)(_ballPosition.X - _ballOrigin.X);
            _ballBound.Y = (int)(_ballPosition.Y - _ballOrigin.Y);
            _ballBound.Width = _ballTexture.Width;
            _ballBound.Height = _ballTexture.Height;

            // Left Paddle
            _paddleScale = Vector2.One;
            _paddleOrigin = CentreOrigin(_leftPaddleTexture);
            _leftPaddlePosition = _windowCentrePosition - _paddlePositionOffset;
            _leftPaddleSpeed = 200f;
            _leftPaddleMoveUpKey = Keys.W;
            _leftPaddleMoveDownKey = Keys.S;
            _leftPaddleBound.X = (int)(_leftPaddlePosition.X - _ballOrigin.X);
            _leftPaddleBound.Y = (int)(_leftPaddlePosition.Y - _ballOrigin.Y);
            _leftPaddleBound.Width = _leftPaddleTexture.Width;
            _leftPaddleBound.Height = _leftPaddleTexture.Height;

            // Right Paddle
            _rightPaddlePosition = _windowCentrePosition + _paddlePositionOffset;
            _rightPaddleSpeed = 200f;
            _rightPaddleMoveUpKey = Keys.Up;
            _rightPaddleMoveDownKey = Keys.Down;
            _leftPaddleBound.X = (int)(_rightPaddlePosition.X - _ballOrigin.X);
            _leftPaddleBound.Y = (int)(_rightPaddlePosition.Y - _ballOrigin.Y);
            _rightPaddleBound.Width = _rightPaddleTexture.Width;
            _rightPaddleBound.Height = _rightPaddleTexture.Height;

            // Scoring System
            _scorePlayerOne = 0;
            _scorePlayerTwo = 0;
            _scorePositionOffset = new Vector2(50f, 10f);
            _scoreDimensions = _spriteFont.MeasureString($"{_scorePlayerOne}");
            _scorePlayerOnePosition = new Vector2()
            {
                X = _graphics.PreferredBackBufferWidth / 2f - _scoreDimensions.X - _scorePositionOffset.X,
                Y = _scorePositionOffset.Y
            };
            _scorePlayerTwoPosition = new Vector2()
            {
                X = _graphics.PreferredBackBufferWidth / 2f + _scorePositionOffset.X,
                Y = _scorePositionOffset.Y
            };
            _gameoverDimensions = _spriteFont.MeasureString("Player 1 Wins");
            _gameoverTextPosition = new Vector2()
            {
                X = (_graphics.PreferredBackBufferWidth - _gameoverDimensions.X) / 2f,
                Y = (_graphics.PreferredBackBufferHeight - _gameoverDimensions.Y) / 2f
            };
            _isGameOver = false;

            // Game State
            _gameState = GameState.Start;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _backgroundTexture = Content.Load<Texture2D>("background_green");
            _ballTexture = Content.Load<Texture2D>("ball_red_small");
            _leftPaddleTexture = Content.Load<Texture2D>("paddle_yellow_left");
            _rightPaddleTexture = Content.Load<Texture2D>("paddle_yellow_right");
            _spriteFont = Content.Load<SpriteFont>("Score");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape)
               )
                Exit();

            // TODO: Add your update logic here
            if (_gameState == GameState.Start)
            {
                _timeScale = 0f;

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    _timeScale = 1f;
                    _gameState = GameState.Playing;
                }
            }
            else if (_gameState == GameState.Playing)
            {
                float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds * _timeScale;

                // Ball Update
                _ballDisplacement = _ballVelocity * elapsedSeconds;
                _ballPosition += _ballDisplacement;

                // Left Paddle Update
                if (Keyboard.GetState().IsKeyDown(_leftPaddleMoveUpKey))
                {
                    _leftPaddleVelocity = -Vector2.UnitY * _leftPaddleSpeed;
                }
                else if (Keyboard.GetState().IsKeyDown(_leftPaddleMoveDownKey))
                {
                    _leftPaddleVelocity =  Vector2.UnitY * _leftPaddleSpeed;
                }
                _leftPaddlePosition += _leftPaddleVelocity * elapsedSeconds;

                // Right Paddle Update
                if (Keyboard.GetState().IsKeyDown(_rightPaddleMoveUpKey))
                {
                    _rightPaddleVelocity = -Vector2.UnitY * _rightPaddleSpeed;
                }
                else if (Keyboard.GetState().IsKeyDown(_rightPaddleMoveDownKey))
                {
                    _rightPaddleVelocity =  Vector2.UnitY * _rightPaddleSpeed;
                }
                _rightPaddlePosition += _rightPaddleVelocity * elapsedSeconds;

                // Collision Handling
                // - Paddle vs Window
                UpdateBounds(ref _leftPaddleBound, _leftPaddlePosition, _paddleOrigin);
                HandlePaddleWindowCollision(_leftPaddleBound, _backgroundBound, _paddleOrigin, ref _leftPaddlePosition);
                UpdateBounds(ref _rightPaddleBound, _rightPaddlePosition, _paddleOrigin);
                HandlePaddleWindowCollision(_rightPaddleBound, _backgroundBound, _paddleOrigin, ref _rightPaddlePosition);

                // - Ball vs Window
                UpdateBounds(ref _ballBound, _ballPosition, _ballOrigin);
                HandleBallWindowCollision(_ballBound, _backgroundBound);
                // HandleBallWindowCollision();

                // - Paddle vs Ball
                UpdateBounds(ref _ballBound, _ballPosition, _ballOrigin);
                HandlePaddleBallCollision(_leftPaddleBound, _rightPaddleBound, _ballBound, elapsedSeconds);

                // Determine end of rally
                _gameState = CheckGameState(_ballBound, _backgroundBound);
            }
            else if (_gameState == GameState.GameOver)
            {
                // Pause the game
                _timeScale = 0.0f;

                // Resume and start the next rally
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    RestartGame();
                    _gameState = GameState.Playing;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Background
            _spriteBatch.Begin(samplerState: SamplerState.LinearWrap);
            _spriteBatch.Draw(_backgroundTexture, Vector2.Zero, _backgroundBound, Color.White);
            _spriteBatch.End();

            // Game Objects
            _spriteBatch.Begin();

            // - Centre Vertical Line
            _spriteBatch.DrawVerticalLine(_linePosition, _lineLength, Color.Green, _lineThickness);

            // - Left Paddle
            _spriteBatch.Draw(_leftPaddleTexture, _leftPaddlePosition, null, Color.White, _paddleOrientation,
                              _paddleOrigin, _paddleScale, SpriteEffects.None, 0.0f);

            // - Right Paddle
            _spriteBatch.Draw(_rightPaddleTexture, _rightPaddlePosition, null, Color.White, _paddleOrientation,
                              _paddleOrigin, _paddleScale, SpriteEffects.None, 0.0f);

            // - Ball
            _spriteBatch.Draw(_ballTexture, _ballPosition, null, Color.White, 0.0f,
                              _ballOrigin, _ballScale, SpriteEffects.None, 0.0f);

            // Scores
            _spriteBatch.DrawString(_spriteFont, $"{_scorePlayerOne}", _scorePlayerOnePosition, Color.White);
            _spriteBatch.DrawString(_spriteFont, $"{_scorePlayerTwo}", _scorePlayerTwoPosition, Color.White);

            // Winning message
            if (_isGameOver)
            {
                _spriteBatch.DrawString(_spriteFont, _gameoverText, _gameoverTextPosition, Color.White);
            }
            
            _spriteBatch.End();
        }

        private Vector2 CentreOrigin(Texture2D texture)
        {
            return new Vector2()
            {
                X = texture.Width / 2.0f,
                Y = texture.Height / 2.0f
            };
        }

        private Vector2 Ball_ComputeRandomDirection()
        {
            // Generate "safe directions" where |Y| << |X| 
            float amount = _rand.NextSingle();
            float sign = _rand.NextSingle() < 0.5f ? -1f : 1f;

            Vector2 first = new Vector2(_graphics.PreferredBackBufferWidth * 0.75f, -_graphics.PreferredBackBufferHeight * 0.5f);
            Vector2 second = new Vector2(_graphics.PreferredBackBufferWidth * 0.75f, _graphics.PreferredBackBufferHeight * 0.5f);
            Vector2 direction = Vector2.Normalize(sign * Vector2.Lerp(first, second, amount));

            return direction;
        }

        private void UpdateBounds(ref Rectangle bound, Vector2 position, Vector2 origin)
        {
            bound.X = (int)(position.X - origin.X);
            bound.Y = (int)(position.Y - origin.Y);
        }

        private void HandlePaddleWindowCollision(Rectangle paddleBound, Rectangle windowBound, Vector2 origin
                                                , ref Vector2 position)
        {
            // Top border
            if (paddleBound.Top < windowBound.Top)
            {
                position.Y = origin.Y;
            }
            // Bottom border
            else if (paddleBound.Bottom > windowBound.Bottom)
            {
                position.Y = windowBound.Bottom - origin.Y;
            }
        }

        private void HandleBallWindowCollision(Rectangle ballBound, Rectangle windowBound)
        {
            // Top and bottom border
            if (ballBound.Top < windowBound.Top || ballBound.Bottom > windowBound.Bottom)
            {
                _ballVelocity.Y = -_ballVelocity.Y;
            }
        }

        private void HandlePaddleBallCollision(Rectangle leftPaddleBound, Rectangle rightPaddleBound, Rectangle ballBound, float elapsedSeconds)
        {
            if (DetectCollision_AABB(leftPaddleBound, ballBound))
            {
                HandlePaddleBallCollision(leftPaddleBound, ballBound, _ballDisplacement, 
                                            _leftPaddlePosition, elapsedSeconds, ref _ballPosition, ref _ballVelocity);
                
            }

            if (DetectCollision_AABB(rightPaddleBound, ballBound))
            {
                HandlePaddleBallCollision(rightPaddleBound, ballBound, _ballDisplacement,
                                          _rightPaddlePosition, elapsedSeconds, ref _ballPosition, ref _ballVelocity);
            }
        }

        private bool DetectCollision_AABB(Rectangle thisBound, Rectangle thatBound)
        {
            return !(thisBound.Right < thatBound.Left || thisBound.Left > thatBound.Right ||
                     thisBound.Bottom < thatBound.Top || thisBound.Top > thatBound.Bottom);
        }

        private void HandlePaddleBallCollision(Rectangle paddleBound, Rectangle ballBound
                                              ,Vector2 ballDisplacement, Vector2 paddlePosition, float elapsedSeconds
                                              ,ref Vector2 ballPosition, ref Vector2 ballVelocity)
        {
            // Assume paddle Rectangle and ball Rectangle already collided

            // Compute summed width and height
            int sumWidth = ballBound.Width + paddleBound.Width;
            int sumHeight = ballBound.Height + paddleBound.Height;

            // Compute the collision locus Rectangle area (used to get the 
            Rectangle locusRectangleF = new Rectangle()
            {
                X = (int)paddlePosition.X - sumWidth / 2,
                Y = (int)paddlePosition.Y - sumHeight / 2,
                Width = sumWidth,
                Height = sumHeight
            };

            float alphaX = -1f;
            float alphaY = -1f;
            Vector2 reflectionNormal = Vector2.Zero;
            Vector2 oldBallPosition = ballPosition - ballDisplacement; // p0

            // NOTE: Without converting float to integer, precision issues could occur.

            // Left and Right of Locus Rectangle
            if ((int)oldBallPosition.X <= locusRectangleF.Left && locusRectangleF.Left <= (int)ballPosition.X)
            {
                alphaX = (locusRectangleF.Left - (int)oldBallPosition.X) / ballDisplacement.X;
                reflectionNormal -= Vector2.UnitX;
            }
            else if ((int)ballPosition.X <= locusRectangleF.Right && locusRectangleF.Right <= (int)oldBallPosition.X)
            {
                alphaX = (locusRectangleF.Right - (int)oldBallPosition.X) / ballDisplacement.X;
                reflectionNormal += Vector2.UnitX;
            }
            
            // Top and Bottom of Locus Rectangle
            if ((int)oldBallPosition.Y <= locusRectangleF.Top && locusRectangleF.Top <= (int)ballPosition.Y)
            {
                alphaY = (locusRectangleF.Top - (int)oldBallPosition.Y) / ballDisplacement.Y;
                reflectionNormal -= Vector2.UnitY;
            }
            else if ((int)ballPosition.Y <= locusRectangleF.Bottom && locusRectangleF.Bottom <= (int)oldBallPosition.Y)
            {
                alphaY = (locusRectangleF.Bottom - (int)oldBallPosition.Y) / ballDisplacement.Y;
                reflectionNormal += Vector2.UnitY;
            }
            // Do this because Vector2.Reflect(v, normal) requires normal to be normalized.
            reflectionNormal.Normalize();

            float alpha = (alphaX > -1f) ? alphaX : ((alphaY > -1f) ? alphaY : -1f);

            Debug.WriteLine($"alphaX = {alphaX}, alphaY = {alphaY}");
            Debug.WriteLine($"alpha = {alpha}");

            // Update ball position to collision position and ball velocity
            if (alpha > -1f)
            {
                Debug.WriteLine($"Reflection Normal = {reflectionNormal}");

                Vector2 maintainedDisplacement = alpha * ballDisplacement;
                Vector2 remainingDisplacement = ballDisplacement - maintainedDisplacement;
                Vector2 reflectionDisplacement = Vector2.Reflect(remainingDisplacement, reflectionNormal);

                ballPosition = oldBallPosition + maintainedDisplacement + reflectionDisplacement;
                ballVelocity = Vector2.Reflect(ballVelocity, reflectionNormal);
            }
            else
            {
                Debug.WriteLine($"Locus Rectangle = {locusRectangleF}");
                Debug.WriteLine($"Old Ball Position = {oldBallPosition}");
                Debug.WriteLine($"Ball Position = {ballPosition}");
            }
        }

        private GameState CheckGameState(Rectangle ballBound, Rectangle windowBound)
        {
            if (ballBound.Right > windowBound.Right)
            {
                // Update Player One Score
                _scorePlayerOne++;
                _gameoverText = "Player 1 Wins";
                _scoreDimensions = _spriteFont.MeasureString($"{_scorePlayerOne}");
                _scorePlayerOnePosition.X = _graphics.PreferredBackBufferWidth / 2f - _scoreDimensions.X - _scorePositionOffset.X;

                // Update time scale and game state
                _timeScale = 0f;
                _isGameOver = true;

                return GameState.GameOver;
            }
            else if (ballBound.Left < windowBound.Left)
            {
                // Update Player Two Score
                _scorePlayerTwo++;
                _gameoverText = "Player 2 Wins";
                _scoreDimensions = _spriteFont.MeasureString($"{_scorePlayerTwo}");
                _scorePlayerTwoPosition.X = _graphics.PreferredBackBufferWidth / 2f + _scorePositionOffset.X;

                // Update time scale and game state
                _timeScale = 0f;
                _isGameOver = true;

                return GameState.GameOver;
            }
            else
            {
                return GameState.Playing;
            }
        }

        private void RestartGame()
        {
            // Paddles
            _leftPaddlePosition = _windowCentrePosition - _paddlePositionOffset;
            _rightPaddlePosition = _windowCentrePosition + _paddlePositionOffset;
            _leftPaddleVelocity = Vector2.Zero;
            _rightPaddleVelocity = Vector2.Zero;

            // Ball
            _ballPosition = _windowCentrePosition;
            _ballVelocity = _ballSpeed * Ball_ComputeRandomDirection();

            // Scoring system
            _isGameOver = false;

            _timeScale = 1f;
        }
    }
}