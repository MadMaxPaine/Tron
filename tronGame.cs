using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Tron
{
    public class TronGame : Game
    {
        #region Data
        public enum FontEnum
        { 
            Arial
        }
        public enum TextureEnum
        {
            Logo
        }

        private SpriteBatch _spriteBatch;
        private int _lev;
        private int _score;
        private int _life=3;
        Texture2D _blockTexture1;
        Texture2D _blockTexture2;
        Texture2D _blockTexture3;
        Texture2D _blockTexture4;
        Texture2D _blockTexture5;
        Texture2D _blockTexture6;
        Texture2D _blockTexture7;
        Texture2D _blockTexture8;
        Texture2D _blockTexture9;
        Texture2D _blockTexture10;
        Texture2D _blockTexture11;
        Texture2D _blockTree1;
        Texture2D _blockTree2;
        Texture2D _blockTree3;        
        Texture2D _idleTexture;
        Texture2D _runTexture;
        Texture2D _jumpTexture;
        Texture2D _cluDieTexture;
        Texture2D _cluRunTexture;

        Tron _hero;
        public int Fail=0;
        public readonly int Width;
        public readonly int Height;
        public int Sch;
        List<Block> _blocks;
        List<Bonus> _bonuses;
        List<Bonus> _lives;
        readonly List<Clu> _clu = new List<Clu>();
        List<Bonus> _tree;
        static int _scrollX;
        int _levelLength;
        private static int _i;
        public int J=0;
        private int _currentLevel;

        #endregion Data
        public static GameState State = GameState.MainMenu;
        private readonly MainMenu _menu;
        public static Dictionary<TextureEnum, Texture2D> TexturesMenu;
        public static Dictionary<FontEnum, SpriteFont> Fonts;
      

        //public new GraphicsDeviceManager GraphicsDevice;
        public TronGame()
        {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            Content.RootDirectory = @"Content";
            Width = graphics.PreferredBackBufferWidth = 400;
            Height = graphics.PreferredBackBufferHeight = 400;
            graphics.ApplyChanges();
            _menu = new MainMenu();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Fonts = new Dictionary <FontEnum, SpriteFont>();
            TexturesMenu = new Dictionary<TextureEnum, Texture2D>();
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _blockTexture1 = Content.Load<Texture2D>("Block/ground");
            _blockTexture2 = Content.Load<Texture2D>("Block/Block");
            _blockTexture3 = Content.Load<Texture2D>("Block/Block1");
            _blockTexture4 = Content.Load<Texture2D>("Block/Block2");
            _blockTexture5 = Content.Load<Texture2D>("Block/Block3");
            _blockTexture6 = Content.Load<Texture2D>("Block/Block4");
            _blockTexture7 = Content.Load<Texture2D>("Block/Bonus");
            _blockTexture8 = Content.Load<Texture2D>("Block/Bonus2");
            _blockTexture9 = Content.Load<Texture2D>("Block/Bonus3");
            _blockTexture10 = Content.Load<Texture2D>("Block/Bonus4");
            _blockTexture11 = Content.Load<Texture2D>("Block/life");
            _blockTree1 = Content.Load<Texture2D>("Tree/Tree1");
            _blockTree2 = Content.Load<Texture2D>("Tree/Tree2");
            _blockTree3 = Content.Load<Texture2D>("Tree/Tree3");          
            _idleTexture = Content.Load<Texture2D>("hero/Idle");
            _runTexture = Content.Load<Texture2D>("hero/Run");
            _jumpTexture = Content.Load<Texture2D>("hero/Jump");
            _cluDieTexture = Content.Load<Texture2D>("CLU/CLUDie");
            _cluRunTexture = Content.Load<Texture2D>("CLU/CLURunning");
            TexturesMenu.Add(TextureEnum.Logo, Content.Load<Texture2D>("logo/logo"));
            Fonts.Add(FontEnum.Arial,Content.Load<SpriteFont>("Font/Arial"));
            
            CreateLevel();
            Rectangle rect = new Rectangle(0, Height - _idleTexture.Height - 40, 40, 40);
            _hero = new Tron(rect, _idleTexture, _runTexture, _jumpTexture, this);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
                
                KeyboardState keyState = Keyboard.GetState();
                if (State == GameState.MainMenu)
                {
                    AudioManager.StopLevelTheme();
                    AudioManager.Level = null;
                    _hero.Rect.X = 0;
                    _hero.Rect.Y = 40;
                    _scrollX = 0;
                    _currentLevel = 0;
                    _life = 3;
                    _score = 0;
                    CreateLevel();
                    _menu.Update();
                }
                if (State == GameState.Exit)
                {
                    AudioManager.StopLevelTheme();
                    AudioManager.StopMenuTheme();
                    this.Exit();
                }
               
                if (State == GameState.Game)
                {
                    AudioManager.StopMenuTheme();
                    AudioManager.StartLevelTheme();
                    AudioManager.Menu = null;
                    if (keyState.IsKeyDown(Keys.Enter))
                    {
                        _lev = 0;
                        AudioManager.StopLevelTheme();
                        AudioManager.Level = null;
                    }
                    if (keyState.IsKeyDown(Keys.Escape))
                    {
                        State = GameState.MainMenu;
                        AudioManager.StopTitleTheme();
                        AudioManager.Em = null;
                        AudioManager.StopLoserTheme();
                        AudioManager.Death = null;
                        AudioManager.StopLevelTheme();
                        AudioManager.Level = null;
                    }
                    if (_life > 0&&_currentLevel<=5)
                    {
                        #region hero run
                        if (_hero.Rect.X > (_levelLength - 80))
                        {
                            _hero.Rect.X = 0;
                            _hero.Rect.Y = 40;
                            _scrollX = 0;
                            CreateLevel();
                            _i++;
                            _lev = 1;
                        }
                        if (_hero.Rect.Y > (Height - 70))
                        {
                            AudioManager.PlayHeroDies();
                            AudioManager.Hd = null;
                            _hero.Rect.X = 0;
                            _hero.Rect.Y = 40;
                            _life--;
                            _scrollX = 0;
                        }


                        if (keyState.IsKeyDown(Keys.Left))
                        {
                            _hero.StartRun(false);
                        }
                        else
                            if (keyState.IsKeyDown(Keys.Right))
                            {
                                _hero.StartRun(true);
                            }
                            else _hero.Stop();

                        if (keyState.IsKeyDown(Keys.Up))
                        {
                            _hero.Jump();
                        }
                        Rectangle heroScreenRect = GetScreenRect(_hero.Rect);
                        if (heroScreenRect.Left < Width / 2)

                            Scroll(-3 * gameTime.ElapsedGameTime.Milliseconds / 10);

                        if (heroScreenRect.Left > Width / 2)
                            Scroll(3 * gameTime.ElapsedGameTime.Milliseconds / 10);
                        _hero.Update(gameTime);

                        #endregion Hero run
                        #region Clu run and die
                        int sti = 0;
                        foreach (Clu dummy in _clu)
                        {
                            sti++;
                        }


                        for (int i = 0; i <= sti - 1; i++)
                        {

                            if (_clu[i].Rect.Y > (Height - 70) && _clu[i].Alive)
                            {
                                _clu.RemoveAt(i);
                                sti--;
                            }
                            else
                            {
                                Rectangle rect = new Rectangle(_clu[i].Rect.Center.X - 15, _clu[i].Rect.Center.Y - 21, 30, 1);
                                Rectangle rect1 = new Rectangle(_clu[i].Rect.Center.X - 20, _clu[i].Rect.Center.Y, 40, 39);
                                if (_hero.Rect.Intersects(rect) && _clu[i].Alive)
                                {
                                    AudioManager.PlayMonsterDies();
                                    AudioManager.Md = null;
                                    _clu[i].Stop();
                                    _clu[i].Alive = false;
                                    sti--;
                                    _score += 100;
                                }
                                else
                                {
                                    if (_hero.Rect.Intersects(rect1) && _clu[i].Alive)
                                    {
                                        AudioManager.PlayHeroDies();
                                        AudioManager.Hd = null;
                                        _hero.Rect.X = 0;
                                        _hero.Rect.Y = 40;
                                        _life--;
                                        _scrollX = 0;


                                    }
                                    else
                                    {
                                        if (_clu[i].Alive)
                                        {
                                            _clu[i].StartRun(false);
                                        }
                                    }
                                }
                            }
                            _clu[i].Update(gameTime);
                        }

                        #endregion Clu run and die
                    }
                }

                AudioManager.Update();
                base.Update(gameTime);
        }
     
        protected override void Draw(GameTime gameTime)
        {
            //KeyboardState keyState = Keyboard.GetState();

            if (State == GameState.MainMenu)
            {
                _hero.Rect.X = 0;
                _hero.Rect.Y = 40;
                _scrollX = 0;
                _currentLevel = 0;
                _life = 3;
                _score = 0;
                _i = 0;
                CreateLevel();                
                _menu.Draw(_spriteBatch);
              
            }
            if (State == GameState.Game)
            {
                
                if ((_lev == 1) && (_i < 5))
                {                                       
                    GraphicsDevice.Clear(Color.Black);
                    _spriteBatch.Begin();
                    _spriteBatch.DrawString(Fonts[FontEnum.Arial], "Level  " + (_i + 1), new Vector2(170, 180), Color.Red);
                    _spriteBatch.End();
                }
                else
                {
                    
                    if (_i < 5)
                    {
                        #region Hero
                        GraphicsDevice.Clear(Color.Black);
                        // TODO: Add your drawing code here
                        _spriteBatch.Begin();
                        foreach (Bonus tree in _tree)
                        {
                            tree.Draw(_spriteBatch);
                        }
                        foreach (Bonus bonus in _bonuses)
                        {
                            bonus.Draw(_spriteBatch);
                        }
                        _spriteBatch.End();
                        _spriteBatch.Begin();
                        foreach (Block block in _blocks)
                        {
                            block.Draw(_spriteBatch);
                        }                       
                        foreach (Bonus lives in _lives)
                        {
                            lives.Draw(_spriteBatch);
                        }
                        _spriteBatch.DrawString(Fonts[FontEnum.Arial], "Life:" + _life.ToString(), new Vector2(0, 0), Color.Red);
                        _spriteBatch.DrawString(Fonts[FontEnum.Arial], "Score:" + _score.ToString(), new Vector2(300, 0), Color.Blue);
                        _spriteBatch.End();
                        foreach(Clu clu in _clu)
                        {
                            clu.Draw(_spriteBatch);
                        }
                        _hero.Draw(_spriteBatch);
                        #endregion Hero
                        #region Lives
                        if (_life <= 0)
                        {
                            AudioManager.StopLevelTheme();
                            AudioManager.Level = null;
                            AudioManager.PlayLoserTheme();
                            
                            GraphicsDevice.Clear(Color.Black);
                            _spriteBatch.Begin();
                            _spriteBatch.DrawString(Fonts[FontEnum.Arial], "You Loser \nScore:" + _score.ToString(), new Vector2(Width / 2 - 40, Height / 2 - 50), Color.Red);
                            _spriteBatch.End();                    
                        }
                        #endregion Lives                
                    }
                    if (_i >= 5)
                    {
                        #region Exit
                        AudioManager.StopLevelTheme();
                        AudioManager.Level = null;
                        AudioManager.PlayTitleTheme();
                        
                        DrawTitles();       
                        #endregion Exit
                        
                    }             
                }
                base.Draw(gameTime);
            }     
        }
        
        //Old funks
        private void DrawTitles()
        {
            
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(Fonts[FontEnum.Arial], "Your winner!!! \nScore:" + _score.ToString(), new Vector2(Width / 2 - 50, Height/2-50), Color.Red);
            _spriteBatch.End();     
        }
        public bool CollidesWithLives(Rectangle rect)
        {
            foreach (Bonus bonus in _lives)
            {
                if (bonus.Rect1.Intersects(rect))
                {
                    AudioManager.PlayBonusPicked();
                    AudioManager.Bp = null;
                    _lives.Remove(bonus);
                    _life += 1;
                    return true;
                }
            }
            return false;
        }     
        public bool CollidesWithBonus(Rectangle rect)
        {
            foreach (Bonus bonus in _bonuses)
            {
                if (bonus.Rect1.Intersects(rect))
                {
                        AudioManager.PlayBonusPicked();
                        AudioManager.Bp = null;
                        _bonuses.Remove(bonus);    
                        _score += 20;
                        return true;
                }
            }
            return false;
        }
        public bool CollidesWithLevel(Rectangle rect)
        {
            foreach (Block block in _blocks)
            {
                  if (block.Rect.Intersects(rect))
                        return true;                              
            }            
            return false;
        }
        public static Rectangle GetScreenRect(Rectangle rect)
        {
            Rectangle screenRect = rect;
            screenRect.Offset(-_scrollX, 0);
            return screenRect;
        }

        private void Scroll(int dx)
        {
            if (_scrollX + dx >= 0 && _scrollX + dx <= _levelLength - 400)
                _scrollX += dx;
        }

        private void CreateLevel()
        {
                _currentLevel++;
                if (_currentLevel > 5)
                    _currentLevel = 1;
                _tree = new List<Bonus>();
                _lives = new List<Bonus>();
                _blocks = new List<Block>();
                _bonuses = new List<Bonus>();
                string[] s = File.ReadAllLines("Content/levels/level" + _currentLevel + ".txt");
                _levelLength = 40 * s[0].Length;
                int x = 0,x1=0;
                int y = 0,y1=0;

                _clu.Clear();

                foreach (string str in s)
                {
                    foreach (char c in str)
                    {
                        Rectangle rect = new Rectangle(x, y, 40, 40);
                        Rectangle rect2 = new Rectangle(x, y-15, 200, 200);
                        if (c == 'Y')
                        {

                            Block block = new Block(rect, _blockTexture1);
                            _blocks.Add(block);
                        }
                        if (c == 'X')
                        {

                            Block block = new Block(rect, _blockTexture2);
                            _blocks.Add(block);
                        }
                        if (c == '1')
                        {

                            Block block = new Block(rect, _blockTexture3);
                            _blocks.Add(block);
                        }
                        if (c == '2')
                        {

                            Block block = new Block(rect, _blockTexture4);
                            _blocks.Add(block);
                        }
                        if (c == '3')
                        {

                            Block block = new Block(rect, _blockTexture5);
                            _blocks.Add(block);
                        }
                        if (c == '4')
                        {
                            Block block = new Block(rect, _blockTexture6);
                            _blocks.Add(block);
                        }
                        if (c == 'M')
                        {
                            Rectangle rect1 = new Rectangle(x,y,40,40);
                            Clu monster = new Clu(rect1,_cluDieTexture, _cluRunTexture, true, this);
                            _clu.Add(monster);
                        }
                        if (c == 'R')
                        {                            
                            Bonus tree = new Bonus(rect2,_blockTree3);
                            _tree.Add(tree);
                        }
                        if (c == 'G')
                        {
                            Bonus tree = new Bonus(rect2, _blockTree1);
                            _tree.Add(tree);
                        }
                        if (c == 'U')
                        {
                            Bonus tree = new Bonus(rect2, _blockTree2);
                            _tree.Add(tree);
                        }
                       
                        
                        x += 40;
                    }
                    x = 0;
                    y += 40;
                
                    
                    foreach (string str1 in s)
                    {
                        foreach (char c1 in str1)
                        {
                            Rectangle rect = new Rectangle(x1+10, y1+10, 30, 30);
                            if (c1 == 'B')
                            {
                                Bonus bonus = new Bonus(rect, _blockTexture7);
                                _bonuses.Add(bonus);
                            }
                            if (c1 == 'T')
                            {
                                Bonus bonus = new Bonus(rect, _blockTexture8);
                                _bonuses.Add(bonus);
                            }
                            if (c1 == 'S')
                            {
                                Bonus bonus = new Bonus(rect, _blockTexture9);
                                _bonuses.Add(bonus);
                            }
                            if (c1 == 'A')
                            {
                                Bonus bonus = new Bonus(rect, _blockTexture10);
                                _bonuses.Add(bonus);
                            }                            
                            if (c1 == 'L')
                            {
                                Bonus bonus = new Bonus(rect, _blockTexture11);
                                _lives.Add(bonus);
                            }
                            x1 += 40;
                        }
                        x1 = 0;
                        y1 += 40;
                        
                    }
                }
                
        }
    }
}