using FishNet.Object;

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

        [ServerRpc]
        public void CmdSetActiveWinPrefab(bool isActive)
        {
            RpcSetActiveWinPrefab(isActive);
            SetActiveWinPrefab(isActive);
        }

        [ObserversRpc]
        private void RpcSetActiveWinPrefab(bool isActive)
        {
            if (!IsServerInitialized) SetActiveWinPrefab(isActive);
        }

        private static void SetActiveWinPrefab(bool isActive)
        {
            WinGame.Instance.WinCanvas.SetActive(isActive);
        }
    }
}