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
            }
            btnPlay.pos = new Vector2(btnPlay.bounds.X /4, 400);
            btnPlay.scale = 2.0f;
            _btnList[0] = btnPlay;

            btnQuit.pos = new Vector2(3*(btnQuit.bounds.X / 4), 400);
            btnQuit.scale = 2.0f;
            _btnList[1] = btnQuit;

            _currentBtn = 0;
            _btnList[_currentBtn].scale = 2.2f;
        }
        public void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (_currentBtn > 0)
                {
                    _btnList[_currentBtn].scale = 2.0f;
                    _currentBtn--;
                    _btnList[_currentBtn].scale = 2.2f;
                }
            }
            else if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (_currentBtn < _btnList.Count() - 1)
                {
                    _btnList[_currentBtn].scale = 2.0f;
                    _currentBtn++;
                    _btnList[_currentBtn].scale = 2.2f;
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.A))
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
        }
    }

    public class Button : Entity
    {
        public Button(Texture2D pTexture) : base(pTexture)
        {

        }
    }
}
