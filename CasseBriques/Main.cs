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
        public BriquesManager _briqueManager;

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
            this._paddle = Paddle.GetInstance();
            this._paddle.Init();

            this._ball = new Ball();
            this._ball.Init();

            this._briqueManager = new BriquesManager();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            this._paddle.Load();
            this._ball.Load();
            this._briqueManager.Load();
        }

        protected override void Update(GameTime gameTime)
        {

            // QUITTER LE JEU
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            this._paddle.Update();
            this._ball.Update();
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            this._paddle.Draw();
            this._ball.Draw();
            this._briqueManager.Draw();
            
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
