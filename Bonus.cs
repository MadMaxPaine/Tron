using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tron
{
    class Bonus
    {
        public Rectangle Rect1;
        readonly Texture2D _texture;
        public Bonus(Rectangle rect, Texture2D texture)
        {
            this.Rect1 = rect;
            this._texture = texture;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle screenRect = TronGame.GetScreenRect(Rect1);
            spriteBatch.Draw(_texture, screenRect, Color.White);
        }
    }
}