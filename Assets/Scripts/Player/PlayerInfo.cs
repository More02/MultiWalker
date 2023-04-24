using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Player
{
    public static class PlayerInfo
    {
        private static Transform _canvasPanel;
        
        public static void FillPlayerInfo(GameObject playerInfoPanel, string playerName, Transform canvasPanel)
        {
            _canvasPanel = canvasPanel;
            var itemFromInfoPanel = playerInfoPanel.transform;
            itemFromInfoPanel.GetChild(0).GetComponent<TMP_Text>().text = playerName;
            itemFromInfoPanel.GetChild(1).GetComponent<TMP_Text>().text = 0.ToString();
        }
        
        public static void SetScore(int score, int localConnectionId)
        {
            _canvasPanel.GetChild(localConnectionId).GetChild(1).GetComponent<TMP_Text>().text = score.ToString();
        }
    }
}