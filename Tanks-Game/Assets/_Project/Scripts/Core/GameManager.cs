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

        private void Update()
        {
            if (_duration < 0f)
            {
                var progress = FindObjectOfType<CapturePoint>().CaptureProgress;
                if (progress < 0f)
                    ScoreFirst();
                else if (progress > 0f)
                    ScoreSecond();
                else
                    ScoreTie();
            }

            _duration -= Time.deltaTime;

            timerText.text = TimeFormat(_duration);
        }

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

        public static void ScoreFirst()
        {
            Instance._p1Score++;
            Instance.p1TextScore.text = Instance._p1Score.ToString();

            LoadRandomLevel();
            Instance.ResetTime();
        }

        public static void ScoreSecond()
        {
            Instance._p2Score++;
            Instance.p2TextScore.text = Instance._p2Score.ToString();

            LoadRandomLevel();
            Instance.ResetTime();
        }

        public static void ScoreTie()
        {
            LoadRandomLevel();
            Instance.ResetTime();
        }

        public static void LoadRandomLevel()
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
        }

        private string TimeFormat(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time - 60f * minutes);

            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
