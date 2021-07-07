using System.Collections;
using UnityEngine;

namespace Gisha.TanksGame.Core
{
    public class CapturePoint : MonoBehaviour
    {
        public float CaptureProgress { get; private set; }

        private IEnumerator CaptureCoroutine(float operation)
        {
            while (Mathf.Abs(CaptureProgress) < 1f)
            {
                CaptureProgress += Time.deltaTime * operation;
                yield return null;
            }

            Debug.Log("Point was captured!");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player1"))
                StartCoroutine(CaptureCoroutine(-1f));
            else if (other.CompareTag("Player2"))
                StartCoroutine(CaptureCoroutine(1f));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            StopAllCoroutines();
        }
    }
}
