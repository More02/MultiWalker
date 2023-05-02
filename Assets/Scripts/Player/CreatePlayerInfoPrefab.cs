using Mirror;

namespace Player
{
    /// <summary>
    /// Класс, отвечающий за создание префабов с информацией о пользователях
    /// </summary>
    public class CreatePlayerInfoPrefab : NetworkBehaviour
    {
        [Command(requiresAuthority = false)]
        public void CmdInstantiatePlayerInfoPrefab()
        {
            RpcInstantiatePlayerInfoPrefab();
            InstantiatePlayerInfoPrefab();
        }

        [ClientRpc]
        private void RpcInstantiatePlayerInfoPrefab()
        {
            if (!isServer) InstantiatePlayerInfoPrefab();
        }

        public async void InstantiatePlayerInfoPrefab()
        {
            Instantiate(FillPlayerInfo.Instance.PlayerInfoPrefab, FillPlayerInfo.Instance.CanvasPanelHolder);
            await FillPlayerInfo.Instance.RenameAllPlayers();
            await FillPlayerInfo.Instance.RecountAllStats();
        }
    }
}