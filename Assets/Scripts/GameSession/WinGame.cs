using Mirror;
using Movement;
using TMPro;
using UnityEngine;

namespace GameSession
{
    /// <summary>
    /// Класс, отвечающий за победу в игре
    /// </summary>
    public class WinGame : MonoBehaviour
    {
        [SerializeField] private TMP_Text _winnerName;
        [SerializeField] private TMP_Text _scoreValue;
        [SerializeField] private GameObject _winCanvas;
        [SerializeField] private NetworkPlayerManager _networkPlayerManager;
        [SerializeField] private int _countDashToWin = 3;
        [SerializeField] private int _delayTime = 5;

        public int DelayTime => _delayTime;
        public GameObject WinCanvas => _winCanvas;
        public static WinGame Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        
        public void CheckWin(int score, string playerName)
        {
            if (score != _countDashToWin) return;
            DashAbility.Instance.IsWin = true;
            _winnerName.SetText(playerName);
            _scoreValue.SetText(score.ToString());
            _winCanvas.SetActive(true);
            _networkPlayerManager.EndOfGameSession();
        }
    }
}