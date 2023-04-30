using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

namespace Player
{
    public class Stats : NetworkBehaviour
    {
        [Command(requiresAuthority = false)]
        public void CmdChangeScore(int score, string playerName)
        {
            RpcChangeScore(score, playerName);
            SetScore(score, playerName);
            ShowWin(score, playerName);
        }

        [ClientRpc]
        private void RpcChangeScore(int score, string playerName)
        {
            if (isServer) return;
            SetScore(score, playerName);
            ShowWin(score, playerName);
        }

        private static async void SetScore(int score, string playerName)
        {
            var infoCanvas = InfoCanvas.Instance;
            for (var i = 0; i < InfoCanvas.Instance.PlayerNames.Count; i++)
            {
                if (playerName == infoCanvas.PlayerNames[i])
                {
                    infoCanvas.CanvasPanelHolder.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text =
                        score.ToString();
                    InfoCanvas.Instance.PlayerScore[i] = score;
                }
            }

            await infoCanvas.RecountAllStats();
        }

        private static void ShowWin(int score, string playerName)
        {
            if (score == 3)
            {
                Debug.Log(playerName + " Win");
            }
        }
    }
}