using System;
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

            this.paddle = Paddle.GetInstance();
            this.paddle.Init(GraphicsDevice);

            this.ball = new Ball(this.gs);
            this.ball.Init();



            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            this.paddle.Load(this);
            this.ball.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if(this.paddle.vel.X >= -20)
                {
                    this.paddle.vel.X -= 2;
                }
                
            }else if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (this.paddle.vel.X <= 20)
                {
                    this.paddle.vel.X += 2;
                }
            }
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
