using Gisha.Effects.Audio;
using UnityEngine;

namespace Gisha.TanksGame.Core
{
    public class CapturePoint : MonoBehaviour
    {
        [SerializeField] private float fullCaptureTime = 5f;
        [SerializeField] private Transform capturedCircle;

        public float CaptureProgress
        {
            get => _captureProgress;
            private set => _captureProgress = Mathf.Clamp(value, -1.01f, 1.01f);
        }
        float _captureProgress;

        bool _p1Capturing;
        bool _p2Capturing;

        SpriteRenderer _capturedCircleSR;

        private void Awake()
        {
            _capturedCircleSR = capturedCircle.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (Mathf.Abs(_captureProgress) >= 1f)
                AudioManager.Instance.StopSFX("capture");
            
            // First player capturing.
            if (_p1Capturing && !_p2Capturing)
            {
                CaptureProgress -= Time.deltaTime / fullCaptureTime;
                if (CaptureProgress < -1f)
                    GameManager.ScoreFirst();
            }
            // Second player capturing.
            else if (!_p1Capturing && _p2Capturing)
            {
                CaptureProgress += Time.deltaTime / fullCaptureTime;
                if (CaptureProgress > 1f)
                    GameManager.ScoreSecond();
            }
            else
                CaptureProgress -= Mathf.Sign(CaptureProgress) * Time.deltaTime / fullCaptureTime;

            _capturedCircleSR.color = LerpColor(Color.red, Color.white, Color.blue, CaptureProgress);
            capturedCircle.localScale = Vector2.one * Mathf.Abs(CaptureProgress);
        }

        private Color LerpColor(Color leftColor, Color middleColor, Color rightColor, float value)
        {
            if (value < 0)
                return leftColor;
            if (value > 0)
                return rightColor;
            return middleColor;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player1"))
                _p1Capturing = true;
            else if (other.CompareTag("Player2"))
                _p2Capturing = true;
            
            AudioManager.Instance.PlaySFX("capture");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player1"))
                _p1Capturing = false;
            else if (other.CompareTag("Player2"))
                _p2Capturing = false;
            
            AudioManager.Instance.StopSFX("capture");
        }
    }
}
