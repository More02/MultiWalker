using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Класс, отвечающий за изменение таблицы информации о пользователях
    /// </summary>
    public class FillPlayerInfo : MonoBehaviour
    {
        [SerializeField] private GameObject _playerInfoPrefab;
        [SerializeField] private Transform _canvasPanelHolder;
        private string _playerName;

        public static FillPlayerInfo Instance { get; private set; }
        public List<string> PlayerNames { get; } = new();
        public List<int> PlayerScore { get; } = new();
        public GameObject PlayerInfoPrefab => _playerInfoPrefab;
        public Transform CanvasPanelHolder => _canvasPanelHolder;

        private void Awake()
        {
            Instance = this;
        }

        public async Task RenameAllPlayers()
        {
            while (PlayerNames.Count != _canvasPanelHolder.childCount)
            {
                await Task.Yield();
            }

            for (var i = 0; i < PlayerNames.Count; i++)
            {
                _canvasPanelHolder.GetChild(i).GetChild(0).GetComponent<TMP_Text>().SetText(PlayerNames[i]);
            }
        }

        public async Task RecountAllStats()
        {
            while (PlayerScore.Count != _canvasPanelHolder.childCount)
            {
                await Task.Yield();
            }

            for (var i = 0; i < PlayerScore.Count; i++)
            {
                _canvasPanelHolder.GetChild(i).GetChild(1).GetComponent<TMP_Text>().SetText(PlayerScore[i].ToString());
            }
        }
    }
}