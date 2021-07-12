using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gisha.TanksGame.Core
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager Instance { get; set; }
        #endregion

        [Header("Scores")]
        [SerializeField] private TMP_Text p1TextScore;
        [SerializeField] private TMP_Text p2TextScore;

        [Header("Timer")]
        [SerializeField] private float roundDurationInSeconds = 120f;
        [SerializeField] private TMP_Text timerText;
        [Space]
        [SerializeField] private float levelChangeDelay = 5f;

        [Header("RoundResult")]
        [SerializeField] private RoundResultGUI roundResultGUI;


        int _p1Score = 0;
        int _p2Score = 0;
        float _duration;

        private void Awake()
        {
            CreateInstance();
        }

        private void Start()
        {
            ResetTime();
            
        }

        public static void ScoreDraw() => Instance.StartCoroutine(Instance.ScorePlayerCoroutine(0));
        public static void ScoreFirst() => Instance.StartCoroutine(Instance.ScorePlayerCoroutine(1));
        public static void ScoreSecond() => Instance.StartCoroutine(Instance.ScorePlayerCoroutine(2));

        private void CreateInstance()
        {
            DontDestroyOnLoad(gameObject);

            if (Instance == null)
                Instance = this;
            else
            {
                if (Instance != this)
                    Destroy(gameObject);
            }
        }

        private IEnumerator RoundTimeCoroutine()
        {
            while (true)
            {
                if (_duration < 0f)
                {
                    ScoreDraw();
                    timerText.text = string.Empty;
                    break;
                }

                else
                {
                    _duration -= Time.deltaTime;
                    timerText.text = TimeFormat(_duration);
                }

                yield return null;
            }
        }

        /// <summary>
        /// Score players with 1 or 2 and draw with 0
        /// </summary>
        private IEnumerator ScorePlayerCoroutine(int playerNum)
        {
            if (playerNum != 0 && playerNum != 1 && playerNum != 2)
            {
                Debug.LogError("Player Num can only be 1 (left) or 2 (right) or 0 (draw)");
                yield break;
            }

            FindObjectOfType<CapturePoint>().enabled = false;

            switch (playerNum)
            {
                case 0:
                    roundResultGUI.ShowDraw();
                    break;
                case 1:
                    _p1Score++;
                    p1TextScore.text = _p1Score.ToString();
                    roundResultGUI.ShowP1Won();
                    break;
                case 2:
                    _p2Score++;
                    p2TextScore.text = _p2Score.ToString();
                    roundResultGUI.ShowP2Won();
                    break;
            }

            yield return new WaitForSeconds(levelChangeDelay);
            LoadRandomLevel();
            ResetTime();
            roundResultGUI.ResetGUI();
        }

        private void LoadRandomLevel()
        {
            if (SceneManager.sceneCountInBuildSettings < 2)
            {
                Debug.LogError("There is less than 2 scenes!");
                return;
            }

            var currentIndex = SceneManager.GetActiveScene().buildIndex;
            var randomIndex = currentIndex;

            while (randomIndex == currentIndex)
                randomIndex = Random.Range(0, SceneManager.sceneCountInBuildSettings);

            SceneManager.LoadScene($"Level_{randomIndex + 1}");
        }

        private void ResetTime()
        {
            _duration = roundDurationInSeconds;
            StartCoroutine(RoundTimeCoroutine());
        }

        private string TimeFormat(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time - 60f * minutes);

            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    [System.Serializable]
    internal class RoundResultGUI
    {
        [SerializeField] private GameObject p1WonImage;
        [SerializeField] private GameObject p2WonImage;
        [SerializeField] private GameObject drawImage;

        public void ShowP1Won() => ShowResult(p1WonImage);
        public void ShowP2Won() => ShowResult(p2WonImage);
        public void ShowDraw() => ShowResult(drawImage);

        public void ResetGUI()
        {
            p1WonImage.SetActive(false);
            p2WonImage.SetActive(false);
            drawImage.SetActive(false);
        }

        private void ShowResult(GameObject target)
        {
            p1WonImage.SetActive(false);
            p2WonImage.SetActive(false);
            drawImage.SetActive(false);

            target.SetActive(true);
        }
    }
}
