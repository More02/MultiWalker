using System.Threading.Tasks;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] private TMP_Text _winnerName;
        [SerializeField] private TMP_Text _scoreValue;
        [SerializeField] private GameObject _winCanvas;
        [SerializeField] private NetworkPlayerManager _networkPlayerManager;
        public bool IsWin { get; set; }
        private int _delayTime = 5;

        public static EndGame Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            IsWin = false;
        }

        public void ShowWin(int score, string playerName)
        {
            if (score != 3) return;
            _winnerName.SetText(playerName);
            _scoreValue.SetText(score.ToString());
            _winCanvas.SetActive(true);
            IsWin = true;
            _networkPlayerManager.EndOfGameSession();
            // await Task.Delay(_delayTime*1000);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}