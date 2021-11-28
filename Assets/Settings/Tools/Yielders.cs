using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static class Yielders
    {
        private class FloatComparer: IEqualityComparer<float>
        {
            bool IEqualityComparer<float>.Equals(float x, float y) => Math.Abs(x - y) < 0.01f;

            int IEqualityComparer<float>.GetHashCode(float obj) => obj.GetHashCode();
        }

        private static readonly Dictionary<float, WaitForSeconds> _timeIntervals =
            new Dictionary<float, WaitForSeconds>(100, new FloatComparer());

        public static WaitForEndOfFrame EndOfFrame { get; } = new WaitForEndOfFrame();

        public static WaitForFixedUpdate FixedUpdate { get; } = new WaitForFixedUpdate();

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            if (!_timeIntervals.TryGetValue(seconds, out var waitForSeconds))
            {
                _timeIntervals.Add(seconds, waitForSeconds = new WaitForSeconds(seconds));
            }

            return waitForSeconds;
        }
    }
}