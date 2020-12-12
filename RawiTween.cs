using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RawiTween
{
    public enum Ease
    {
        InSine,
        OutSine,
        InOutSine,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuint,
        OutQuint,
        InOutQuint,
        InCirc,
        OutCirc,
        InOutCirc,
        InElastic,
        OutElastic,
        InOutElastic,
        InQuad,
        OutQuad,
        InOutQuad,
        InQuart,
        OutQuart,
        InOutQuart,
        InExpo,
        OutExpo,
        InOutExpo,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        OutBounce,
        InOutBounce
    }
    
    public class TweenRunner:MonoBehaviour
    {
        static TweenRunner _Instance;

        public static TweenRunner Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<TweenRunner>();

                    if (_Instance == null)
                    {
                        var holder = new GameObject("TweenRunner");
                        DontDestroyOnLoad(holder);
                        _Instance = holder.AddComponent<TweenRunner>();
                    }
                }

                return _Instance;
            }
        }
        
        List<Tween> runningTween = new List<Tween>(); 

        public void RunTween(Tween tween, float duration, Action<float> onUpdate, Action onFinish)
        {
            StartCoroutine(TickRoutine(tween, duration, onUpdate, onFinish));
        }

        IEnumerator TickRoutine(Tween tween, float duration, Action<float> onUpdate, Action onFinish)
        {
            runningTween.Add(tween);
            
            float runningDuration = 0;
            while (runningDuration <= duration)
            {
                onUpdate?.Invoke(runningDuration/duration);
                runningDuration += Time.deltaTime;
                yield return null;
            }
            onFinish?.Invoke();

            runningTween.Remove(tween);
        }

    }
    
    
    public static class RawiTweenExtension
    {
        public static Tween MoveTo(this Transform obj, Vector3 targetPosition, float duration)
        {
            var tween = new Tween();
            
            Vector3 initialPosition = obj.position;
            TweenRunner.Instance.RunTween(
                tween,
                duration,
                f =>
                {
                    if (obj != null)
                    {
                        var x = TweenMethod.Ease(tween.Ease, f);
                        var y = TweenMethod.Ease(tween.Ease, f);
                        var z = TweenMethod.Ease(tween.Ease, f);
                        obj.position = new Vector3(
                            Mathf.Lerp(initialPosition.x, targetPosition.x, x),
                            Mathf.Lerp(initialPosition.y, targetPosition.y, y),
                            Mathf.Lerp(initialPosition.z, targetPosition.z, z));
                        
                    }
                },
                () =>
                {
                    obj.position = targetPosition;
                    tween.OnFinishAction?.Invoke();
                });

            return tween;
        }
        
        public static Tween To(Func<float> getter, Action<float> setter, float target, float duration)
        {
            var tween = new Tween();

            float initialValue = getter.Invoke();
            TweenRunner.Instance.RunTween(
                tween,
                duration,
                f =>
                {
                    var x = TweenMethod.Ease(tween.Ease, f);
                    setter?.Invoke(Mathf.Lerp(initialValue, target, x));
                },
                () =>
                {
                    setter?.Invoke(target);
                    tween.OnFinishAction?.Invoke();
                });

            return tween;
        }
    }
    
    public class Tween
    {
        public Ease Ease;

        public Action OnFinishAction;

        public Tween SetEase(Ease ease)
        {
            this.Ease = ease;
            return this;
        }

        public Tween OnFinish(Action onFinish)
        {
            this.OnFinishAction = onFinish;
            return this;
        }
    }
    
    /// <summary>
    /// Formulas thanks to https://easings.net/
    /// </summary>
    public static class TweenMethod
    {
        public static float Ease(Ease ease, float x)
        {
            switch (ease)
            {
                case RawiTween.Ease.InSine:
                    return InSine(x);
                case RawiTween.Ease.OutSine:
                    return OutSine(x);
                case RawiTween.Ease.InOutSine:
                    return InOutSine(x);
                case RawiTween.Ease.InCubic:
                    return InCubic(x);
                case RawiTween.Ease.OutCubic:
                    return OutCubic(x);
                case RawiTween.Ease.InOutCubic:
                    return InOutCubic(x);
                case RawiTween.Ease.InQuint:
                    return InQuint(x);
                case RawiTween.Ease.OutQuint:
                    return OutQuint(x);
                case RawiTween.Ease.InOutQuint:
                    return InOutQuint(x);
                case RawiTween.Ease.InCirc:
                    return InCirc(x);
                case RawiTween.Ease.OutCirc:
                    return OutCirc(x);
                case RawiTween.Ease.InOutCirc:
                    return InOutCirc(x);
                case RawiTween.Ease.InElastic:
                    return InElastic(x);
                case RawiTween.Ease.OutElastic:
                    return OutElastic(x);
                case RawiTween.Ease.InOutElastic:
                    return InOutElastic(x);
                case RawiTween.Ease.InQuad:
                    return InQuad(x);
                case RawiTween.Ease.OutQuad:
                    return OutQuad(x);
                case RawiTween.Ease.InOutQuad:
                    return InOutQuad(x);
                case RawiTween.Ease.InQuart:
                    return InQuart(x);
                case RawiTween.Ease.OutQuart:
                    return OutQuart(x);
                case RawiTween.Ease.InOutQuart:
                    return InOutQuart(x);
                case RawiTween.Ease.InExpo:
                    return InExpo(x);
                case RawiTween.Ease.OutExpo:
                    return OutExpo(x);
                case RawiTween.Ease.InOutExpo:
                    return InOutExpo(x);
                case RawiTween.Ease.InBack:
                    return InBack(x);
                case RawiTween.Ease.OutBack:
                    return OutBack(x);
                case RawiTween.Ease.InOutBack:
                    return InOutBack(x);
                case RawiTween.Ease.InBounce:
                    return InBounce(x);
                case RawiTween.Ease.OutBounce:
                    return OutBounce(x);
                case RawiTween.Ease.InOutBounce:
                    return InOutBounce(x);
                default:
                    throw new ArgumentOutOfRangeException(nameof(ease), ease, null);
            }
        }

        private static float InSine(float x)
        {
            return 1 - Mathf.Cos((x * Mathf.PI) / 2f);
        }

        private static float OutSine(float x)
        {
            return Mathf.Sin((x * Mathf.PI) / 2);
        }

        private static float InOutSine(float x)
        {
            return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
        }

        private static float InCubic(float x) 
        {
            return x * x * x;
        }

        private static float OutCubic(float x) 
        {
            return 1 - Mathf.Pow(1 - x, 3);
        }

        private static float InOutCubic(float x) 
        {
            return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
        }

        private static float InQuint(float x) {
            return x * x * x * x * x;
        }

        private static float OutQuint(float x) 
        {
            return 1 - Mathf.Pow(1 - x, 5);
        }

        private static float InOutQuint(float x) 
        {
            return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
        }
        
        private static float InCirc(float x) 
        {
            return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
        }
        
        private static float OutCirc(float x) 
        {
            return Mathf.Sqrt(1f - Mathf.Pow(x - 1f, 2f));
        }
        
        private static float InOutCirc(float x) 
        {
            return x < 0.5
                ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
                : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
        }
        
        private static float InElastic(float x)
        {
            float c4 = (2 * Mathf.PI) / 3;

            return x == 0
                ? 0
                : Math.Abs(x - 1) < 0.001f
                    ? 1
                    : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10f - 10.75f) * c4);
        }
        
        private static float OutElastic(float x)
        {
            float c4 = (2 * Mathf.PI) / 3;

            return x == 0
                ? 0
                : Math.Abs(x - 1) < 0.001f
                    ? 1
                    : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10f - 0.75f) * c4) + 1;
        }
        
        private static float InOutElastic(float x)
        {
            float c5 = (2 * Mathf.PI) / 4.5f;

            return x == 0
                ? 0
                : Math.Abs(x - 1) < 0.001f
                    ? 1
                    : x < 0.5
                        ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
                        : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
        }
        
        private static float InQuad(float x)
        {
            return x * x;
        }
        
        private static float OutQuad(float x)
        {
            return 1 - (1 - x) * (1 - x);
        }
        
        private static float InOutQuad(float x) 
        {
            return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
        }
        
        private static float InQuart(float x)
        {
            return x * x * x * x;
        }
        
        private static float OutQuart(float x)
        {
            return 1 - Mathf.Pow(1 - x, 4);
        }
        
        private static float InOutQuart(float x)
        {
            return x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
        }
        
        private static float InExpo(float x) 
        {
            return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
        }
        
        private static float OutExpo(float x)
        {
            return Math.Abs(x - 1) < 0.001f ? 1 : 1 - Mathf.Pow(2, -10 * x);
        }
        
        private static float InOutExpo(float x) 
        {
            return x == 0
                ? 0
                : Math.Abs(x - 1) < 0.001f
                    ? 1
                    : x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2
                        : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
        }
        
        private static float InBack(float x)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return c3 * x * x * x - c1 * x * x;
        }
        
        private static float OutBack(float x)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
        }
        
        private static float InOutBack(float x)
        {
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;

            return x < 0.5
                ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
                : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
        }
        
        private static float InBounce(float x) 
        {
            return 1f - OutBounce(1f - x);
        }
        
        private static float OutBounce(float x){
            float n1 = 7.5625f;
            float d1 = 2.75f;

            if (x < 1 / d1) {
                return n1 * x * x;
            } else if (x < 2 / d1) {
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            } else if (x < 2.5 / d1) {
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            } else {
                return n1 * (x -= 2.625f / d1) * x + 0.984375f;
            }
        }
        
        private static float InOutBounce(float x){
            return x < 0.5f
                ? (1 - OutBounce(1 - 2 * x)) / 2
                : (1 + OutBounce(2 * x - 1)) / 2;
        }
    }
}