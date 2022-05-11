using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CasseBriques
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameServices gs;

        public Paddle paddle;
        public Ball ball;

        // Couleurs
        public Color colorXbox;

        public Main()
        {

            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            colorXbox = new Color(16, 124, 16);
        }

        protected override void Initialize()
        {
            this.gs = new GameServices();
            this.gs.SetGame(this);

            this.paddle = Paddle.GetInstance(this.gs);
            this.paddle.Init();

            this.ball = new Ball(this.gs);
            this.ball.Init();

            this.gs.SetBall(this.ball);
            this.gs.SetPaddle(this.paddle);



            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            this.paddle.Load();
            this.ball.Load();
        }

        protected override void Update(GameTime gameTime)
        {

            // QUITTER LE JEU
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            this.paddle.Update();
            this.ball.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            this.paddle.Draw(_spriteBatch);
            this.ball.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
