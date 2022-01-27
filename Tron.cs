using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Tron
{
    class Tron
    {
        //public int Score = 0;
        public Rectangle Rect;
        readonly Texture2D _idle;
        readonly Texture2D _run;
        readonly Texture2D _jump;
        bool _isRunning;        
        bool _isRunningRight;
        bool _isJumping;
        float _yVelocity;
        float maxYVelocity = 10;
        float g = 0.2f;
        readonly int _frameWidth;
        readonly int _frameHeight;
        int _currentFrame;
        int _timeElapsed;
        int timeForFrame = 100;
        readonly TronGame _game;
        private int Frames => _run.Width / _frameWidth;

        public Tron(Rectangle rect, Texture2D idle, Texture2D run, Texture2D jump, TronGame game)
        {
            this.Rect = rect;
            this._idle = idle;
            this._run = run;
            this._jump = jump;

            _frameWidth = _frameHeight = run.Height;

            this._game = game;
        }        
        public void StartRun(bool isRight)
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _currentFrame = 0;
                _timeElapsed = 0;
            }
            _isRunningRight = isRight;

        }
        public void Stop()
        {
            _isRunning = false;
        }
        public void Jump()
        {
            if (!_isJumping && _yVelocity == 0.0f)
            {
                _isJumping = true;
                _currentFrame = 0;
                _timeElapsed = 0;
                _yVelocity = maxYVelocity;
            }
        }

        private void ApplyGravity(GameTime gameTime)
        {
            
            _yVelocity = (float)(_yVelocity - 1.65*g * gameTime.ElapsedGameTime.Milliseconds / 10);
            float dy = _yVelocity * gameTime.ElapsedGameTime.Milliseconds / 10;

            Rectangle nextPosition = Rect;
            nextPosition.Offset(0, -(int)dy);

            Rectangle boundingRect = GetBoundingRect(nextPosition);
            if (boundingRect.Top > 0 && boundingRect.Bottom < _game.Height
                && !_game.CollidesWithLevel(boundingRect))
                Rect = nextPosition;
            if (boundingRect.Top > 0 && boundingRect.Bottom < _game.Height
                && !_game.CollidesWithLevel(boundingRect) && (_game.CollidesWithBonus(boundingRect)||_game.CollidesWithLives(boundingRect)))
            {
                Rect = nextPosition;               
            }
            
            
            bool collideOnFallDown = (_game.CollidesWithLevel(boundingRect) && _yVelocity < 0);

            if (boundingRect.Bottom > _game.Height || collideOnFallDown)
            {
                _yVelocity = 0;
                _isJumping = false;
            }
            
           

        }
        public void Update(GameTime gameTime)
        {
            _timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            int tempTime = timeForFrame;
            
            if (_timeElapsed > tempTime)
            {
                _currentFrame = (_currentFrame + 1) % Frames;
                _timeElapsed = 0;
            }

            if (_isRunning)
            {
                int dx = 3 * gameTime.ElapsedGameTime.Milliseconds / 10;
                if (!_isRunningRight)
                    dx = -dx;

                Rectangle nextPosition = Rect;
                nextPosition.Offset(dx, 0);

                Rectangle boundingRect = GetBoundingRect(nextPosition);
                Rectangle screenRect = TronGame.GetScreenRect(boundingRect);

                if (screenRect.Left > 0 && screenRect.Right < _game.Width
                    && !_game.CollidesWithLevel(boundingRect))
                    Rect = nextPosition;
            }

            ApplyGravity(gameTime);
        }
        private Rectangle GetBoundingRect(Rectangle rectangle)
        {
            int width = (int)(rectangle.Width * 0.4f);
            int x = rectangle.Left + (int)(rectangle.Width * 0.2f);

            return new Rectangle(x, rectangle.Top, width, rectangle.Height);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle(_currentFrame * _frameWidth, 0, _frameWidth, _frameHeight);
            SpriteEffects effects = SpriteEffects.None;
            if (_isRunningRight)
                effects = SpriteEffects.FlipHorizontally;

            Rectangle screenRect = TronGame.GetScreenRect(Rect);
            spriteBatch.Begin();
            if (_isJumping)
            {
                spriteBatch.Draw(_jump, screenRect, r, Color.White, 0, Vector2.Zero, effects, 0);
            }
            else
                if (_isRunning)
                {
                    spriteBatch.Draw(_run, screenRect, r, Color.White, 0, Vector2.Zero, effects, 0);
                }
                else
                {
                    spriteBatch.Draw(_idle, screenRect, Color.White);
                }
            spriteBatch.End();
        }
    }
}