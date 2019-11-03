using System;
using UniRx.Async;

namespace Utils
{
    public static class AnimationRunner
    {
        private const int MinimumIntervalMs = 20;
        
        public static async UniTask Run(Action<float> action, float durationInSeconds)
        {
            float t = 0;

            var delta = MinimumIntervalMs / durationInSeconds / 1000;
            
            while (t <= 1)
            {
                action(t);
                t += delta;
                await UniTask.Delay(TimeSpan.FromMilliseconds(MinimumIntervalMs));
            }
        }
    }
}