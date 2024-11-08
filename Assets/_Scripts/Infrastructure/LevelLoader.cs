using _Scripts.Infrastructure.Factories;
using _Scripts.UI;

namespace _Scripts.Infrastructure
{
    public class LevelLoader
    {
        private readonly SceneLoader _sceneLoader;
        private readonly GameFactory _gameFactory;
        private readonly GameUiPresenter _gameUiPresenter;

        public LevelLoader(SceneLoader sceneLoader, GameFactory gameFactory, GameUiPresenter gameUiPresenter)
        {
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _gameUiPresenter = gameUiPresenter;
        }

        public void LoadLevel(string sceneName) => _sceneLoader.LoadScene(sceneName, OnLoaded);

        private void OnLoaded() => InitLevel();

        private void InitLevel()
        {
            _gameFactory.CreateDicePool();
            _gameUiPresenter.InitializePresenter();
        }
    }
}