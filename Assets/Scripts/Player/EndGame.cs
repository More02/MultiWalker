using TMPro;
using UnityEngine;

namespace Player
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField]private TMP_Text _winnerName;
        [SerializeField]private TMP_Text _scoreValue;
        [SerializeField] private GameObject _winCanvas;

        public static EndGame Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void ShowWin(int score, string playerName)
        {
            if (score == 3)
            {
                _winnerName.SetText(playerName);
                _scoreValue.SetText(score.ToString());
                _winCanvas.SetActive(true);
            }
        }
    }
}