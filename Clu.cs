using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tron
{
    class Clu
    {
        //public int Score = 0;
        public Rectangle Rect;
        public bool Alive;
        private readonly Texture2D _run;
        private readonly Texture2D _die;
        bool _isRunning;
        bool _isRunningRight;
        private int _dx;
        float _yVelocity;
       
        float g = 0.2f;
        readonly int _frameWidth;
        readonly int _frameHeight;
        int _currentFrame;
        int _timeElapsed;
        int timeForFrame = 100;
        readonly TronGame _game;
        private int Frames => _run.Width / _frameWidth;

        public Clu(Rectangle rect, Texture2D die, Texture2D run, bool alive, TronGame game)
        {
            this.Rect = rect;
            this.Alive = alive;
            this._run = run;
            this._die = die;

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

        private void ApplyGravity(GameTime gameTime)
        {
            _yVelocity = _yVelocity - g * gameTime.ElapsedGameTime.Milliseconds / 10;
            float dy = _yVelocity * gameTime.ElapsedGameTime.Milliseconds / 10;

            Rectangle nextPosition = Rect;
            nextPosition.Offset(0, -(int)dy);

            Rectangle boundingRect = GetBoundingRect(nextPosition);
            if (boundingRect.Top > 0 && boundingRect.Bottom < _game.Height
                && !_game.CollidesWithLevel(boundingRect))
                Rect = nextPosition;
            


            bool collideOnFallDown = (_game.CollidesWithLevel(boundingRect) && _yVelocity < 0);

            if (boundingRect.Bottom > _game.Height || collideOnFallDown)
            {
                _yVelocity = 0;
               
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
                _dx = gameTime.ElapsedGameTime.Milliseconds / 10;
                if (!_isRunningRight)
                    _dx = -_dx;

                
                Rectangle nextPosition = Rect;
                nextPosition.Offset(_dx, 0);

                Rectangle boundingRect = GetBoundingRect(nextPosition);
                Rectangle screenRect = TronGame.GetScreenRect(boundingRect);

               if(screenRect.Right < _game.Width
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
            
                if (_isRunning)
                {
                    spriteBatch.Draw(_run, screenRect, r, Color.White, 0, Vector2.Zero, effects, 0);
                }
                else
                {
                    Rectangle screenRect1 = new Rectangle(screenRect.Center.X-20,screenRect.Center.Y,40,20);
                    spriteBatch.Draw(_die,screenRect1, Color.White);
                }
            spriteBatch.End();
        }
    }
}