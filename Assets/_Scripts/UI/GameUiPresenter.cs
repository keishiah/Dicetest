using System;
using _Scripts.Services;
using _Scripts.Services.Dice;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace _Scripts.UI
{
    public class GameUiPresenter
    {
        private DiceThrower _diceThrower;
        private GameUi _gameUi;
        private StatisticsCounter _statisticsCounter;

        private IDisposable _destroySubscription;
        private CompositeDisposable _statisticsSubscriptions;

        [Inject]
        void Construct(DiceThrower diceThrower, StatisticsCounter statisticsCounter)
        {
            _diceThrower = diceThrower;
            _statisticsCounter = statisticsCounter;
        }

        public void InitializePresenter()
        {
            _gameUi = GameObject.Find("GameUi").GetComponent<GameUi>();

            _gameUi.playButton.onClick.AddListener(ThrowDices);
            _gameUi.InitSlider();

            _diceThrower.OnThrowFinished.Subscribe(_ => OnTrowFinished());
            SubscribeStatistics();
        }

        private void ThrowDices()
        {
            _diceThrower.ThrowDices((int)_gameUi.maxValueSlider.value);
            TurnButtonAndSlider(false);
        }

        private void OnTrowFinished() => TurnButtonAndSlider(true);

        private void TurnButtonAndSlider(bool turn)
        {
            _gameUi.playButton.gameObject.SetActive(turn);
            _gameUi.maxValueSlider.gameObject.SetActive(turn);
        }

        private void SubscribeStatistics()
        {
            _statisticsSubscriptions = new CompositeDisposable();

            _statisticsSubscriptions.Add(_statisticsCounter.TotalThrows
                .Subscribe(totalThrows => _gameUi.UpdateTotalThrows(totalThrows)));

            _statisticsSubscriptions.Add(_statisticsCounter.LastThrowSum
                .Subscribe(lastThrowSum => _gameUi.UpdateLastThrowSum(lastThrowSum)));

            _statisticsSubscriptions.Add(_statisticsCounter.LastThrowAvg
                .Subscribe(lastThrowAvg => _gameUi.UpdateLastThrowAvg(lastThrowAvg)));

            _statisticsSubscriptions.Add(_statisticsCounter.TotalSum
                .Subscribe(totalSum => _gameUi.UpdateTotalSum(totalSum)));

            _statisticsSubscriptions.Add(_statisticsCounter.TotalAvg
                .Subscribe(totalAvg => _gameUi.UpdateTotalAvg(totalAvg)));

            _destroySubscription = _gameUi.OnDestroyAsObservable()
                .Subscribe(_ => OnGameFinished());
        }

        private void OnGameFinished()
        {
            _diceThrower.Cansel();
            _diceThrower.OnThrowFinished?.Dispose();
            _destroySubscription?.Dispose();
            _statisticsSubscriptions?.Dispose();
        }
    }
}