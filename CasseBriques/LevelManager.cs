
using System.Diagnostics;
using System.IO;

namespace CasseBriques
{
    public class LevelManager : ILevel
    {
        public int _currentLevel;
        public int _nbLevels;
        IMain srvMain;

        public LevelManager()
        {
            ServicesLocator.AddService<ILevel>(this);
            _currentLevel = 1;
            _nbLevels = Directory.GetFiles("Levels/", "*.json", SearchOption.AllDirectories).Length;
            srvMain = ServicesLocator.GetService<IMain>();
        }
        public int GetCurrentLevel()
        {
            return _currentLevel;
        }

        public void SetCurrentLevel(int pLevel)
        {
            if (_currentLevel < _nbLevels)
            {
                _currentLevel++;
                IManager srvManager = ServicesLocator.GetService<IManager>();
                if (srvManager != null && srvManager is BricksManager)
                {
                    srvManager.LoadFromJson(_currentLevel.ToString());
                }
            }
            else // Fin de jeu
            {
                if (srvMain != null)
                {
                    srvMain.LaunchGame(false);
                    _currentLevel = 1;
                }
            }
        }
    }
}
