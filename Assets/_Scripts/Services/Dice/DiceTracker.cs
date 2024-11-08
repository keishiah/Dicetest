using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Services.Dice
{
    public class DiceTracker
    {
        private const float AngularVelocityThreshold = 0.01f;
        private const float VelocityThreshold = 0.01f;

        private readonly DiceValue _diceValue = new();

        public async UniTask<List<int>> TrackActiveDices(List<GameObject> dices,
            CancellationTokenSource cancellationTokenSource)
        {
            var cancellationToken = cancellationTokenSource.Token;
            var results = new List<int>();

            try
            {
                await UniTask.Delay(500, cancellationToken: cancellationToken);

                await UniTask.WhenAll(dices.ConvertAll(dice => TrackDiceMoving(dice, cancellationToken)));

                if (cancellationToken.IsCancellationRequested)
                    return results;

                foreach (var dice in dices)
                {
                    var topFaceValue = _diceValue.GetTopFace(dice.transform);
                    results.Add(topFaceValue);
                }
            }
            catch (OperationCanceledException)
            {
            }

            return results;
        }


        private async UniTask TrackDiceMoving(GameObject dice, CancellationToken cancellationToken)
        {
            var diceRb = dice.GetComponent<Rigidbody>();
            try
            {
                await UniTask.WaitUntil(() =>
                        (diceRb.linearVelocity.magnitude < VelocityThreshold &&
                         diceRb.angularVelocity.magnitude < AngularVelocityThreshold),
                    cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}