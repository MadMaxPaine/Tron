using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tron
{
    class Block
    {
        public Rectangle Rect;
        readonly Texture2D _texture;
        public Block(Rectangle rect, Texture2D texture)
        {
            this.Rect = rect;
            this._texture = texture;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle screenRect = TronGame.GetScreenRect(Rect);
            spriteBatch.Draw(_texture, screenRect, Color.White);
        }
    }
}