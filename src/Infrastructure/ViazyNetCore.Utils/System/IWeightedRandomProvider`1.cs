using System;
using System.Collections.Generic;
using System.Linq;

namespace System
{

    /// <summary>
    /// 定义一个实现加权平均算法的提供程序。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWeightedRandomProvider<T>
    {
        /// <summary>
        /// 获取下一个元素。
        /// </summary>
        /// <returns>元素。</returns>
        T Next();
        /// <summary>
        /// 获取元素数。
        /// </summary>
        int Count { get; }
    }

    class WeightedRandomProvider<T> : IWeightedRandomProvider<T>
    {
        class WeightedChance
        {
            public T Value;
            public decimal Weight;
            public decimal AdjustedWeight;
        }

        private readonly FastRandom _fastRandom;
        private List<WeightedChance> _weights;
        private bool _adjusted;
        private readonly object _syncObject = new object();
        public int Count { get; }

        public WeightedRandomProvider(FastRandom fastRandom, int length)
        {
            this._fastRandom = fastRandom;
            this._weights = new List<WeightedChance>();
            this.Count = length;
        }

        public void AddOrUpdate(T value, decimal weight)
        {
            if (weight < 0) throw new ArgumentException("Invalid weight value.", nameof(weight));

            var existing = this._weights.SingleOrDefault(x => EqualityComparer<T>.Default.Equals(x.Value, value));
            if (existing is null)
                this._weights.Add(new WeightedChance { Value = value, Weight = weight });
            else
                existing.Weight = weight;

            this._adjusted = false;
        }

        private static List<WeightedChance> CalculateAdjustedWeights(WeightedChance[] weights)
        {
            var sorted = weights.OrderBy(x => x.Weight).ToList();
            var weightSum = 0m;
            for (int i = 0; i < sorted.Count; i++)
            {
                weightSum += sorted[i].Weight;
                if (i == 0) sorted[i].AdjustedWeight = sorted[i].Weight;
                else sorted[i].AdjustedWeight = sorted[i].Weight + sorted[i - 1].AdjustedWeight;
            }

            if (weightSum != 1.0m)
                throw new InvalidOperationException("The total weight value is not 100% error.");

            return weights.OrderBy(x => x.AdjustedWeight).ToList();

        }

        public T Next()
        {
            if (!this._adjusted)
            {
                lock (this._syncObject)
                {
                    if (!this._adjusted)
                    {
                        this._weights = CalculateAdjustedWeights(this._weights.ToArray());
                        this._adjusted = true;
                    }
                }
            }
            var d = (decimal)this._fastRandom.NextDouble();
            foreach (var item in this._weights)
            {
                if (d < item.AdjustedWeight) return item.Value;
            }
            return default;
        }
    }
}
