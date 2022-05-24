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

        public Paddle _paddle;
        public Ball _ball;
        public BricksManager _briqueManager;

        public Landscape _landscape;
        public Hole _hole;

        // Couleurs
        public Color colorXbox;

        public Main()
        {

            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            colorXbox = new Color(16, 124, 16);

            ServicesLocator.AddService<IMain>(this);
        }

        protected override void Initialize()
        {
            // Taille de l'écran fixe
            _graphics.IsFullScreen = false; // A REVOIR POUR LA XBOX, Tester en 1080p ?
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            _paddle = Paddle.GetInstance();
            _paddle.Init();

            _ball = new Ball(LoadT2D("ball_blue_small")); // A REVOIR !!!
            _ball.Init();

            _briqueManager = new BricksManager();

            _landscape = new Landscape();
            _landscape.Init();

            _hole = new Hole(LoadT2D("hole_large_end_alt")); // A REVOIR !!!
            _hole.Init();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _paddle.Load();
            _ball.Load();
            _briqueManager.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            // QUITTER LE JEU
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            _paddle.Update();

            // Collisions
            _ball.UpdateColl(_paddle);

            _ball.UpdateColl(_hole);

            try
            {
                for (int i = _briqueManager._bricksList.Count-1; i >= 0; i--)
                {                           
                    _ball.UpdateColl(_briqueManager._bricksList[i]);
                }
            }

            catch (ArgumentException e)
            {
                Debug.WriteLine("Index :", e.Message);
            }
            
            for (int i = _briqueManager._spooler.Count - 1;i>=0;i--) // Suppression des briques à supprimer de la liste (cpt Obvious)
            {
                _briqueManager._bricksList.Remove(_briqueManager._spooler[i]);
            }
            _briqueManager._spooler.Clear();

            _ball.Update();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            _landscape.Draw();
            _hole.Draw();

            _paddle.Draw();
            _ball.Draw();
            _briqueManager.Draw();

            
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
    }
}
