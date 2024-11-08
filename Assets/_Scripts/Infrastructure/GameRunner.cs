using _Scripts.Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace _Scripts.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        private GameFactory _gameFactory;

        [Inject]
        void Construct(GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            CreateGameBootstrapper();
        }
        
        private void CreateGameBootstrapper()
        {
            var bootstrapper = FindFirstObjectByType<GameBootstrapper>();

            if (bootstrapper != null) 
                return;

            _gameFactory.CreateGameBootstrapper();
        }
    }
}