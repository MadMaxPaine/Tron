using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Tron
{
    class MainMenu
    {
        
        private int _selected;
        private bool _arrowPressed = true;
        public void Update()
        {  
            KeyboardState keyboard = Keyboard.GetState();
            if (!_arrowPressed)
            {
               
                if (keyboard.IsKeyDown((Keys.Up)))
                    _selected = _selected == 0 ? 1 : 0;
                if (keyboard.IsKeyDown((Keys.Down)))
                    _selected = _selected == 0 ? 1 : 0;
            }
             
            if (keyboard.IsKeyDown((Keys.Enter)))
            {
                TronGame.State = _selected == 0 ? GameState.Game : GameState.Exit;
            }

            if (keyboard.IsKeyUp((Keys.Up)) && keyboard.IsKeyUp((Keys.Down)))
                _arrowPressed = false;
            else _arrowPressed = true;


        }
        public void Draw(SpriteBatch spritebatch)
        {
            AudioManager.PlayMenuTheme();
            spritebatch.Begin();
            spritebatch.Draw(TronGame.TexturesMenu[TronGame.TextureEnum.Logo], new Rectangle(0, 0, 400, 400), Color.White);               
            spritebatch.DrawString(TronGame.Fonts[TronGame.FontEnum.Arial],
                "Play game",new Vector2(300, 330), _selected == 0 ? Color.Red:Color.Blue);
            spritebatch.DrawString(TronGame.Fonts[TronGame.FontEnum.Arial],
                "Exit", new Vector2(300, 350), _selected == 1 ? Color.Red : Color.Blue);


            spritebatch.End();
        }
    }
}