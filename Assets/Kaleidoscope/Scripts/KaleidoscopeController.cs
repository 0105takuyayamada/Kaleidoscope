using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Kaleidoscope
{
    public class KaleidoscopeController : MonoBehaviour
    {
        const float
            VIEW_SECOND = 3f,
            FADEIN_RATE = 5f,
            FADEOUT_RATE = 1f;

        float ignoreSecond;
        bool active;

        [SerializeField] CanvasGroup canvasGroup;

        [SerializeField] ParticleSystem particle;
        [SerializeField] KaleidoscopeRotator tubeRotator;
        [SerializeField] KaleidoscopeRotator cameraRotator;

        [SerializeField] Text timeScaleText;
        [SerializeField] Text emissionText;
        [SerializeField] Text tubeRotText;
        [SerializeField] Text cameraRotText;

        ParticleSystem.EmissionModule emissionModule;

        void Start()
        {
            emissionModule = particle.emission;

            timeScaleText.text = "x" + Time.timeScale.ToString("0.0");
            emissionText.text = emissionModule.rateOverTimeMultiplier.ToString();
            tubeRotText.text = tubeRotator.speed.ToString("0");
            cameraRotText.text = cameraRotator.speed.ToString("0.0");
        }

        void Update()
        {
            ignoreSecond += Time.deltaTime;

            if (Input.anyKeyDown)
            {
                ignoreSecond = 0f;
                if (!active) StartCoroutine(Coroutine());
            }
        }

        public void PushTimeScale(bool plus)
        {
            Time.timeScale = Mathf.Max(0f, Time.timeScale + (plus ? 0.1f : -0.1f));
            timeScaleText.text = "x" + Time.timeScale.ToString("0.0");
        }
        public void PushEmission(bool plus)
        {
            emissionModule.rateOverTimeMultiplier = emissionModule.rateOverTimeMultiplier + (plus ? 1 : -1);
            emissionText.text = emissionModule.rateOverTimeMultiplier.ToString();
        }

        public void PushTubeRotate(bool plus)
        {
            tubeRotator.speed += plus ? 15f : -15f;
            tubeRotText.text = tubeRotator.speed.ToString("0");
        }

        public void PushCameraRotate(bool plus)
        {
            cameraRotator.speed += plus ? 10f : -10f;
            cameraRotText.text = cameraRotator.speed.ToString("0");
        }

        IEnumerator Coroutine()
        {
            // FadeIn
            active = true;
            while (active && canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += Time.deltaTime * FADEIN_RATE;
                yield return null;
            }

            while (ignoreSecond < VIEW_SECOND * Time.timeScale) yield return null;

            // FadeOut
            active = false;
            while (!active && canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.deltaTime * FADEOUT_RATE;
                yield return null;
            }
        }
    }
}