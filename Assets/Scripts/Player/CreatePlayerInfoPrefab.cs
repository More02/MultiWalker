using Mirror;

namespace Player
{
    /// <summary>
    /// Класс, отвечающий за создание префабов с информацией о пользователях
    /// </summary>
    public class CreatePlayerInfoPrefab : NetworkBehaviour
    {
        [Command(requiresAuthority = false)]
        public void CmdInstantiatePlayerInfoPrefab(string playerName)
        {
            RpcInstantiatePlayerInfoPrefab(playerName);
            InstantiatePlayerInfoPrefab(playerName);
        }

        [ClientRpc]
        private void RpcInstantiatePlayerInfoPrefab(string playerName)
        {
            if (!isServer) InstantiatePlayerInfoPrefab(playerName);
        }

        public async void InstantiatePlayerInfoPrefab(string playerName)
        {
            var playerInfoPrefab =
                Instantiate(FillPlayerInfo.Instance.PlayerInfoPrefab, FillPlayerInfo.Instance.CanvasPanelHolder);
            await FillPlayerInfo.Instance.RenameAllPlayers();
            await FillPlayerInfo.Instance.RecountAllStats();
        }
    }
}