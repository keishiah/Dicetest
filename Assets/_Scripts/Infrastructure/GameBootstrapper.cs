using _Scripts.Infrastructure.AssetManagement;
using UnityEngine;
using Zenject;

namespace _Scripts.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private LevelLoader _levelLoader;

        [Inject]
        void Construct(LevelLoader levelLoader)
        {
            _levelLoader = levelLoader;
        }

        private void Start()
        {
            _levelLoader.LoadLevel(AssetPath.GameplayScene);

            DontDestroyOnLoad(this);
        }
    }
}