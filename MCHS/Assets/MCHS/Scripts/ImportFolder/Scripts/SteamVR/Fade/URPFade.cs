//сли не URP то коммитим

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MySteamVR.Fade
{
    public class URPFade : MonoBehaviour
    {
        private static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        
        private static readonly UnityEvent<float, float, float> Fade = new();
        public static void StartFade(float duration)
        {
            Fade?.Invoke(duration, 0f, duration);
        }
        public static void StartFade(float durationFadeIn, float durationFadeOut)
        {
            Fade?.Invoke(durationFadeIn, 0f, durationFadeOut);
        }
        public static void StartFade(float durationFadeIn, float durationFadeStay, float durationFadeOut)
        {
            Fade?.Invoke(durationFadeIn, durationFadeStay, durationFadeOut);
        }
        
        private Coroutine _fadeCoroutine;
        private ColorParameter _cp;

        private void Awake()
        {
            var localVolume = new GameObject("LocalVolume", typeof(Volume), typeof(SphereCollider));
            
            localVolume.transform.SetParent(transform);
            localVolume.transform.localPosition = Vector3.zero;
            localVolume.transform.localRotation = Quaternion.identity;
            
            var sphereCollider = localVolume.GetComponent<SphereCollider>();
            sphereCollider.radius = 0.2f;
            
            var profile = ScriptableObject.CreateInstance<VolumeProfile>();
            profile.Add(typeof(ColorAdjustments));
            
            var volume = localVolume.GetComponent<Volume>();
            volume.isGlobal = false;
            volume.priority = -1;
            volume.profile = profile;

            if (volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments))
            {
                _cp = colorAdjustments.colorFilter;
                colorAdjustments.SetAllOverridesTo(true);
            }
            else
                Debug.LogWarning("No color adjustments found");
        }

        private void OnStartFade(float durationFadeIn, float durationFadeStay, float durationFadeOut)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            
            _fadeCoroutine = StartCoroutine(FadeCoroutine(durationFadeIn, durationFadeStay, durationFadeOut));
        }
        
        private IEnumerator FadeCoroutine(float durationFadeIn, float durationFadeStay, float durationFadeOut)
        {
            var from = Color.white;
            var to = Color.black;
            var elapsedTime = 0f;
            
            _cp.value = from;

            if (durationFadeIn > 0f)
            {
                while (elapsedTime < durationFadeIn)
                {
                    _cp.Interp(from, to, elapsedTime / durationFadeIn);
                    elapsedTime += Time.deltaTime;
                    yield return WaitForEndOfFrame;
                }
            }

            _cp.value = to;

            yield return new WaitForSeconds(durationFadeStay);

            if (durationFadeOut > 0f)
            {
                elapsedTime = 0f;
                while (elapsedTime < durationFadeOut)
                {
                    _cp.Interp(to, from, elapsedTime / durationFadeOut);
                    elapsedTime += Time.deltaTime;
                    yield return WaitForEndOfFrame;
                }
            }

            _cp.value = from;
            _fadeCoroutine = null;
        }

        private void OnEnable()
        {
            Fade.AddListener(OnStartFade);
        }

        private void OnDisable()
        {
            Fade.RemoveListener(OnStartFade);
        }
    }
}