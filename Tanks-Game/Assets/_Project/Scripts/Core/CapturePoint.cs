using UnityEngine;

namespace Gisha.TanksGame.Core
{
    public class CapturePoint : MonoBehaviour
    {
        [SerializeField] private float fullCaptureTime = 5f;

        public float CaptureProgress { private set; get; }

        bool _p1Capturing = false;
        bool _p2Capturing = false;

        SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            // First player capturing.
            if (_p1Capturing && !_p2Capturing)
            {
                CaptureProgress -= Time.deltaTime / fullCaptureTime;
                if (CaptureProgress > 1f)
                    Debug.Log("First player won!");
            }

            // Second player capturing.
            else if (!_p1Capturing && _p2Capturing)
            {
                CaptureProgress += Time.deltaTime / fullCaptureTime;
                if (CaptureProgress < -1f)
                    Debug.Log("Second player won!");
            }

            _spriteRenderer.color = LerpColor(Color.red, Color.white, Color.blue, CaptureProgress);
        }

        private Color LerpColor(Color leftColor, Color middleColor, Color rightColor, float value)
        {
            float absValue = Mathf.Abs(value);
            if (value < 0)
                return Color.Lerp(middleColor, leftColor, absValue);
            else if (value > 0)
                return Color.Lerp(middleColor, rightColor, absValue);
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
