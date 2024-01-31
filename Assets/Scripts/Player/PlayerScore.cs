using FishNet.Object;
using GameSession;
using Movement;
using TMPro;

namespace Player
{
    /// <summary>
    /// Класс, отвечающий за счёт очков игроков
    /// </summary>
    public class PlayerScore : NetworkBehaviour
    {
        private void Start()
        {
            FillPlayerInfo.Instance.PlayerScore.Add(GetComponent<DashAbility>().CountOfSuccessDash);
        }

        [ServerRpc(RequireOwnership = false)]
        public void CmdChangeScore(int score, string playerName)
        {
            RpcChangeScore(score, playerName);
            SetScore(score, playerName);
            WinGame.Instance.CheckWin(score, playerName);
        }

        [ObserversRpc(ExcludeServer = true)]
        private void RpcChangeScore(int score, string playerName)
        {
            if (IsServerInitialized) return;
            SetScore(score, playerName);
            WinGame.Instance.CheckWin(score, playerName);
        }

        private static async void SetScore(int score, string playerName)
        {
            var fillPlayerInfo = FillPlayerInfo.Instance;
            for (var i = 0; i < FillPlayerInfo.Instance.PlayerNames.Count; i++)
            {
                if (playerName != fillPlayerInfo.PlayerNames[i]) continue;
                fillPlayerInfo.CanvasPanelHolder.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text =
                    score.ToString();
                FillPlayerInfo.Instance.PlayerScore[i] = score;
            }

            await fillPlayerInfo.RecountAllStats();
        }
    }
}