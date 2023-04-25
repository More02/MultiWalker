using System;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class InfoCanvas: MonoBehaviour
    {
        [SerializeField] private GameObject _playerInfoPrefab;
        [SerializeField] private Transform _canvasPanelHolder;
        private string _playerName;

        public static InfoCanvas Instance { get; private set; }
        public GameObject PlayerInfoPrefab => _playerInfoPrefab;
        public Transform CanvasPanelHolder => _canvasPanelHolder;

        private void Awake()
        {
            Instance = this;
        }
        
        public static void FirstFillPlayerInfo(string playerName, GameObject playerInfoPanel)
        {
            var itemFromInfoPanel = playerInfoPanel.transform;
            itemFromInfoPanel.GetChild(0).GetComponent<TMP_Text>().text = playerName;
            itemFromInfoPanel.GetChild(1).GetComponent<TMP_Text>().text = 0.ToString();
        }
        
        public void SetScore(int score, int localConnectionId)
        {
            _canvasPanelHolder.GetChild(localConnectionId).GetChild(1).GetComponent<TMP_Text>().text = score.ToString();
        }
    }
}