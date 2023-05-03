using Mirror;

namespace GameSession
{
    /// <summary>
    /// Класс, отвечающий за активацию канваса победы на сервере и на клиенте
    /// </summary>
    public class SetActiveWinCanvas : NetworkBehaviour
    {
        public static SetActiveWinCanvas Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [Command(requiresAuthority = false)]
        public void CmdSetActiveWinPrefab(bool isActive)
        {
            RpcSetActiveWinPrefab(isActive);
            SetActiveWinPrefab(isActive);
        }

        [ClientRpc]
        private void RpcSetActiveWinPrefab(bool isActive)
        {
            if (!isServer) SetActiveWinPrefab(isActive);
        }

        private static void SetActiveWinPrefab(bool isActive)
        {
            WinGame.Instance.WinCanvas.SetActive(isActive);
        }
    }
}