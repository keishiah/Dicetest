using System;
using System.Collections.Generic;
using System.Threading;
using _Scripts.Infrastructure.Factories;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Scripts.Services.Dice
{
    public class DiceThrower
    {
        public readonly ReactiveCommand OnThrowFinished = new();

        private const float ThrowRadius = 5f;
        private const float ThrowForce = 30f;
        private const float TorqueForce = 6f;
        private const float ConeAngle = 60;
        private const int BatchSize = 5;
        private readonly Vector3 _throwPoint = new(-10, -1, -33);
        private CancellationTokenSource _cancellationTokenSource;

        private GameFactory _gameFactory;
        private DiceTracker _diceTracker;
        private StatisticsCounter _statisticsCounter;

        [Inject]
        void Construct(GameFactory gameFactory, DiceTracker diceTracker, StatisticsCounter statisticsCounter)
        {
            _gameFactory = gameFactory;
            _diceTracker = diceTracker;
            _statisticsCounter = statisticsCounter;
        }

        public async void ThrowDices(int count)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _gameFactory.DicePool.DeactivateAllPoolUnits();

            _statisticsCounter.RegisterStartedThrow();
            await ThrowDicesAsync(count);
            await RegisterDicesResult();

            if (!OnThrowFinished.IsDisposed)
                OnThrowFinished?.Execute();
        }

        public void Cansel()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async UniTask RegisterDicesResult()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
                return;

            var activeDices = _gameFactory.DicePool.GetActiveElements();
            var diceValues = await _diceTracker.TrackActiveDices(activeDices, _cancellationTokenSource);
            _statisticsCounter.RegisterThrowResults(diceValues);
        }

        private async UniTask ThrowDicesAsync(int totalDices)
        {
            var cancellationToken = _cancellationTokenSource.Token;

            try
            {
                for (int i = 0; i < totalDices; i += BatchSize)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var currentBatchSize = Mathf.Min(BatchSize, totalDices - i);

                    var diceBatch = GetDices(currentBatchSize);

                    foreach (var dice in diceBatch)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var randomPosition = GetRandomPositionInRadius(_throwPoint, ThrowRadius);

                        dice.transform.position = randomPosition;
                        dice.SetActive(true);
                        var throwDirection = GetRandomDirectionInCone(Vector3.forward, ConeAngle);

                        var rb = dice.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.linearVelocity = Vector3.zero;
                            rb.angularVelocity = Vector3.zero;

                            rb.AddForce(throwDirection * ThrowForce, ForceMode.Impulse);

                            var randomTorque = GetRandomTorque();
                            rb.AddTorque(randomTorque, ForceMode.Impulse);
                        }
                    }

                    await UniTask.Delay((int)(.2f * 1000), cancellationToken: cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }


        private Vector3 GetRandomPositionInRadius(Vector3 center, float radius)
        {
            var randomCirclePoint = Random.insideUnitCircle * radius;
            return new Vector3(center.x + randomCirclePoint.x, center.y, center.z + randomCirclePoint.y);
        }

        private Vector3 GetRandomDirectionInCone(Vector3 forward, float angle)
        {
            var randomRotation = Quaternion.Euler(
                Random.Range(-angle / 2, angle / 2),
                Random.Range(-angle / 2, angle / 2),
                0
            );

            return randomRotation * forward.normalized;
        }

        private Vector3 GetRandomTorque()
        {
            return new Vector3(
                Random.Range(-TorqueForce, TorqueForce),
                Random.Range(-TorqueForce, TorqueForce),
                Random.Range(-TorqueForce, TorqueForce)
            );
        }

        private List<GameObject> GetDices(int count)
        {
            var diceObjects = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                diceObjects.Add(_gameFactory.DicePool.GetFreeElement());
            }

            return diceObjects;
        }
    }
}