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

        [SerializeField] private TMP_Text p1TextScore;
        [SerializeField] private TMP_Text p2TextScore;

        int _p1Score = 0;
        int _p2Score = 0;

        private void Awake()
        {
            CreateInstance();
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
        }

        public static void ScoreSecond()
        {
            Instance._p2Score++;
            Instance.p2TextScore.text = Instance._p2Score.ToString();

            LoadRandomLevel();
        }

        public static void ScoreTie()
        {
            LoadRandomLevel();
        }

        public static void LoadRandomLevel()
        {
            SceneManager.LoadScene(0);
        }
    }
}
