using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CasseBriques
{
    public class MenuScene
    {
        private Button btnPlay;
        private Button btnQuit;
        private Texture2D header;

        private Entity[] _btnList;
        private int _currentBtn;
        public MenuScene()
        {
            _btnList = new Entity[2];
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                btnPlay = new Button(srvMain.LoadT2D("button_play"));
                btnQuit = new Button(srvMain.LoadT2D("button_quit"));
                header = srvMain.LoadT2D("Header");
            }
            btnPlay.pos = new Vector2(btnPlay.bounds.X / 2, 400);
            btnPlay.scale = 2.0f;
            _btnList[0] = btnPlay;

            btnQuit.pos = new Vector2((btnQuit.bounds.X / 2), 600);
            btnQuit.scale = 2.0f;
            _btnList[1] = btnQuit;

            _currentBtn = 0;
            _btnList[_currentBtn].scale = 2.2f;
        }
        public void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (_currentBtn > 0)
                {
                    _btnList[_currentBtn].scale = 2.0f;
                    _currentBtn--;
                    _btnList[_currentBtn].scale = 2.2f;
                }
            }
            else if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (_currentBtn < _btnList.Count() - 1)
                {
                    _btnList[_currentBtn].scale = 2.0f;
                    _currentBtn++;
                    _btnList[_currentBtn].scale = 2.2f;
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                IMain srvMain = ServicesLocator.GetService<IMain>();
                if (_currentBtn == 0)
                {
                    srvMain.LaunchGame(true);
                }else if(_currentBtn == 1){
                    if(srvMain != null)
                    {
                        srvMain.QuitGame();
                    }
                }
            }
        }
        public void Draw()
        {
            btnPlay.Draw();
            btnQuit.Draw();
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                srvMain.GetSpriteBatch().Draw(header, new Vector2(srvMain.GetBounds().X/2,200), null, Color.White, 0, new Vector2(header.Width/2,header.Height/2), 1, SpriteEffects.None, 0);
            }
        }
    }

    public class Button : Entity
    {
        public Button(Texture2D pTexture) : base(pTexture)
        {

        }
    }
}
