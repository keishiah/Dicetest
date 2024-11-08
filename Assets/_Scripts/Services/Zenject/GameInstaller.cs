using _Scripts.Infrastructure;
using _Scripts.Infrastructure.Factories;
using _Scripts.Services.Dice;
using _Scripts.UI;
using Zenject;

namespace _Scripts.Services.Zenject
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSceneLoader();

            BindLevelLoader();

            BindFactory();

            BindDiceTracker();

            BindDiceThrower();

            BindStatisticsCounter();

            BindGameUiPresenter();
        }

        private void BindGameUiPresenter() => Container.Bind<GameUiPresenter>().AsSingle();

        private void BindStatisticsCounter() => Container.Bind<StatisticsCounter>().AsSingle();

        private void BindDiceThrower() => Container.Bind<DiceThrower>().AsSingle();

        private void BindFactory() => Container.Bind<GameFactory>().AsSingle();

        private void BindSceneLoader() =>
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();

        private void BindLevelLoader() => Container.Bind<LevelLoader>().AsSingle();

        private void BindDiceTracker() => Container.Bind<DiceTracker>().AsSingle();
    }
}