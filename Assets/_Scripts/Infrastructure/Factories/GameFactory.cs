using _Scripts.Infrastructure.AssetManagement;
using UnityEngine;
using Zenject;

namespace _Scripts.Infrastructure.Factories
{
    public class GameFactory
    {
        public DicePooler DicePool { get; private set; }

        private readonly DiContainer _container;


        public GameFactory(DiContainer container)
        {
            _container = container;
        }

        public void CreateGameBootstrapper()
        {
            _container.InstantiatePrefabResource(AssetPath.GameBootsTrapper);
        }

        public void CreateDicePool()
        {
            var parentTransform = new GameObject("DiceParent").transform;
            var dicePrefab = Resources.Load<GameObject>(AssetPath.DicePrefab);
            DicePool = new DicePooler(dicePrefab, parentTransform);
            DicePool.CreatePool(50);
        }
    }
}