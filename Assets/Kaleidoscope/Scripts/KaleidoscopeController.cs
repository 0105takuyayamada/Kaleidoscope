using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Kaleidoscope
{
    public class KaleidoscopeController : MonoBehaviour
    {
        const float
            VIEW_SECOND = 2f,
            FADEIN_RATE = 5f,
            FADEOUT_RATE = 2.5f;

        float
            ignoreSecond,
            holdTimeScale;
        bool active,
            pause;

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
            ignoreSecond += Time.unscaledDeltaTime;

            if (Input.anyKeyDown)
            {
                ignoreSecond = 0f;
                if (!active) StartCoroutine(Coroutine());
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                pause = !pause;
                if (pause)
                {
                    holdTimeScale = Time.timeScale;
                    Time.timeScale = 0f;
                    return;
                }
                Time.timeScale = holdTimeScale;
                holdTimeScale = 0f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) PushCameraRotate(false);
            if (Input.GetKeyDown(KeyCode.Alpha2)) PushCameraRotate(true);
            if (Input.GetKeyDown(KeyCode.Q)) PushTubeRotate(false);
            if (Input.GetKeyDown(KeyCode.W)) PushTubeRotate(true);
            if (Input.GetKeyDown(KeyCode.A)) PushEmission(false);
            if (Input.GetKeyDown(KeyCode.S)) PushEmission(true);
            if (Input.GetKeyDown(KeyCode.Z)) PushTimeScale(false);
            if (Input.GetKeyDown(KeyCode.X)) PushTimeScale(true);

        }

        public void PushTimeScale(bool plus)
        {
            if (pause)
            {
                pause = false;
                Time.timeScale = holdTimeScale;
            }
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
                canvasGroup.alpha += Time.unscaledDeltaTime * FADEIN_RATE;
                yield return null;
            }

            while (ignoreSecond < VIEW_SECOND) yield return null;

            // FadeOut
            active = false;
            while (!active && canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.unscaledDeltaTime * FADEOUT_RATE;
                yield return null;
            }
        }
    }
}