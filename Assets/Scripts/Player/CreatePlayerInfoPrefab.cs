using FishNet.Object;

namespace Player
{
    /// <summary>
    /// Класс, отвечающий за создание префабов с информацией о пользователях
    /// </summary>
    public class CreatePlayerInfoPrefab : NetworkBehaviour
    {
        [ServerRpc]
        public void CmdInstantiatePlayerInfoPrefab()
        {
            RpcInstantiatePlayerInfoPrefab();
            InstantiatePlayerInfoPrefab();
        }

        [ObserversRpc]
        private void RpcInstantiatePlayerInfoPrefab()
        {
            if (!IsServerInitialized) InstantiatePlayerInfoPrefab();
        }

        public async void InstantiatePlayerInfoPrefab()
        {
            Instantiate(FillPlayerInfo.Instance.PlayerInfoPrefab, FillPlayerInfo.Instance.CanvasPanelHolder);
            await FillPlayerInfo.Instance.RenameAllPlayers();
            await FillPlayerInfo.Instance.RecountAllStats();
        }
    }
}