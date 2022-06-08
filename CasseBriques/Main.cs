//MAIN
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CasseBriques
{
    public class Main : Game, IMain
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public JsonManager _JsonManager;
        public LevelManager _lvlManager;
        public AnimManager _animManager;
        public ParticleManager _partManager;

        public ImageLoader _imageLoader;

        public Paddle _paddle;
        public Ball _ball;
        public Laser _laser;
        public BricksManager _briqueManager;
        public Landscape _landscape;
        public Hole _hole;

        public bool _gameplay; // Gameplay ou dans le menu ?
        private bool _GameOver;
        public MenuScene _menu;

        // Ecran
        public Color colorXbox;
        private bool shakeViewport;
        private double shakeStartAngle;
        private double shakeRadius;
        private Random rand;
        private float shakeDuration;
        private float shakeTimer;

        public Main()
        {

            _graphics = new GraphicsDeviceManager(this);
            ServicesLocator.AddService<IMain>(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            colorXbox = new Color(16, 124, 16);
            _gameplay = false; // Menu ou Gameplay ?
        }

        protected override void Initialize()
        {
            // Taille de l'écran fixe
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            rand = new Random();
            shakeDuration = 2;
            shakeTimer = shakeDuration;
            _GameOver = false;

            _JsonManager = new JsonManager();
            _lvlManager = new LevelManager();
            _partManager = new ParticleManager();
            _animManager = new AnimManager();
            _imageLoader = new ImageLoader(this);

            _landscape = new Landscape();
            _hole = new Hole("hole_large_end_alt");

            _menu = new MenuScene();

            _paddle = Paddle.GetInstance();

            _ball = new Ball("ball_blue_small");

            _briqueManager = new BricksManager();

            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _briqueManager.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            // QUITTER LE JEU
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                QuitGame();
            }

            if(_gameplay)
            {
                // Update Collisions
                _paddle.Update();

                _ball.UpdateColl(_paddle);
                _ball.UpdateColl(_hole);

                for (int i = _briqueManager._bricksList.Count - 1; i >= 0; i--)
                {
                    _ball.UpdateColl(_briqueManager._bricksList[i]);
                }
                for (int i = _briqueManager._staticList.Count - 1; i >= 0; i--)
                {
                    _ball.UpdateColl(_briqueManager._staticList[i]);
                }

                for (int i = _briqueManager._spooler.Count - 1; i >= 0; i--) // Suppression des briques à supprimer de la liste (cpt Obvious)
                {
                    _briqueManager._bricksList.Remove(_briqueManager._spooler[i]);
                }
                _briqueManager._spooler.Clear();

                _briqueManager.Update();

                if(_briqueManager._bricksList.Count <= 0)
                {
                    IHole srvHole = ServicesLocator.GetService<IHole>();
                    if(srvHole != null)
                    {
                        srvHole.SetState(true);
                    }
                }

                _ball.Update(gameTime);
                _partManager.Update(gameTime);
            }
            else
            {
                _menu.Update();
            }
            _animManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // Screen shake
            Vector2 offset = new Vector2(0, 0);
            if (shakeViewport)
            {
                offset = new Vector2((float)(Math.Sin(shakeStartAngle) * shakeRadius), (float)(Math.Cos(shakeStartAngle) * shakeRadius));
                shakeRadius -= 1.0f;
                shakeStartAngle += (150 + rand.Next(60));
                shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if(shakeTimer >= shakeDuration)
                {
                    shakeViewport = false;
                    shakeTimer = shakeDuration;
                }
            }

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateTranslation(offset.X, offset.Y, 0));

            _landscape.Draw();

            if (_gameplay)
            {
                _paddle.Draw();
                _hole.Draw();
                _briqueManager.Draw();
                _partManager.Draw();
                _ball.Draw();
                _animManager.Draw();
            }
            else
            {
                _menu.Draw();
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public Vector2 GetBounds()
        {
            Vector2 bounds = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            return bounds;
        }
        public Texture2D LoadT2D(string pTex)
        {
            return Content.Load<Texture2D>(pTex);
        }
        public SpriteBatch GetSpriteBatch()
        {
            return _spriteBatch;
        }
        public void Shake(float pRadius)
        {
            shakeRadius = pRadius; 
            shakeViewport = true;
        }
        public void QuitGame()
        {
            Exit();
        }
        public void LaunchGame(bool pBool)
        {
            _gameplay = pBool;
        }

        public void SetGameOver(bool pGO)
        {
            _GameOver = pGO;
        }

        public bool GetGameOver()
        {
            return _GameOver;
        }
    }
}
