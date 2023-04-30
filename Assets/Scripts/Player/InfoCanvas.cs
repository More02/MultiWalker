using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Player
{
    public class InfoCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _playerInfoPrefab;
        [SerializeField] private Transform _canvasPanelHolder;
        private string _playerName;

        public static InfoCanvas Instance { get; private set; }
        public GameObject PlayerInfoPrefab => _playerInfoPrefab;
        public Transform CanvasPanelHolder => _canvasPanelHolder;

        public List<string> PlayerNames { get; } = new();
        public List<int> PlayerScore { get; } = new();

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

        public static void FirstFillPlayerInfo(string playerName, GameObject playerInfoPrefab)
        {
            var itemFromInfoPrefab = playerInfoPrefab.transform;
            itemFromInfoPrefab.GetChild(1).GetComponent<TMP_Text>().text = 0.ToString();
        }
        
    }
}