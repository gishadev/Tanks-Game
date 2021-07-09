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

        bool _p1Capturing = false;
        bool _p2Capturing = false;

        SpriteRenderer _capturedCircleSR;

        private void Awake()
        {
            _capturedCircleSR = capturedCircle.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            // First player capturing.
            if (_p1Capturing && !_p2Capturing)
            {
                CaptureProgress -= Time.deltaTime / fullCaptureTime;
                if (CaptureProgress < -1f)
                {
                    GameManager.ScoreFirst();
                    Debug.Log("First player won!");
                }
            }

            // Second player capturing.
            else if (!_p1Capturing && _p2Capturing)
            {
                CaptureProgress += Time.deltaTime / fullCaptureTime;
                if (CaptureProgress > 1f)
                {
                    GameManager.ScoreSecond();
                    Debug.Log("Second player won!");
                }
            }

            _capturedCircleSR.color = LerpColor(Color.red, Color.white, Color.blue, CaptureProgress);
            capturedCircle.localScale = Vector2.one * Mathf.Abs(CaptureProgress);
        }

        private Color LerpColor(Color leftColor, Color middleColor, Color rightColor, float value)
        {
            if (value < 0)
                return leftColor;
            else if (value > 0)
                return rightColor;
            else
                return middleColor;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player1"))
                _p1Capturing = true;
            else if (other.CompareTag("Player2"))
                _p2Capturing = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player1"))
                _p1Capturing = false;
            else if (other.CompareTag("Player2"))
                _p2Capturing = false;
        }
    }
}
