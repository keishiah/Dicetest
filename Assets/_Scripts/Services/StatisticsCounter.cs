using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace _Scripts.Services
{
    public class StatisticsCounter
    {
        public ReactiveProperty<int> TotalThrows { get; } = new(0);
        public ReactiveProperty<int> LastThrowSum { get; } = new(0);
        public ReactiveProperty<float> LastThrowAvg { get; } = new(0f);
        public ReactiveProperty<int> TotalSum { get; } = new(0);
        public ReactiveProperty<float> TotalAvg { get; } = new(0f);

        public void RegisterStartedThrow() => TotalThrows.Value++;

        public void RegisterThrowResults(List<int> values)
        {
            if (values.Count == 0)
                return;

            LastThrowSum.Value = values.Sum();
            LastThrowAvg.Value = (float)LastThrowSum.Value / values.Count;
            TotalSum.Value += LastThrowSum.Value;
            TotalAvg.Value = (float)TotalSum.Value / TotalThrows.Value;
        }
    }
}