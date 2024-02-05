using FishNet.Object;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Класс, отвечающий за создание префабов с информацией о пользователях
    /// </summary>
    public class CreatePlayerInfoPrefab : NetworkBehaviour
    {
        [ServerRpc(RequireOwnership = false)]
        public void CmdInstantiatePlayerInfoPrefab()
        {
            Debug.Log("[CreatePlayerInfoPrefab] CmdInstantiatePlayerInfoPrefab");
            RpcInstantiatePlayerInfoPrefab();
            InstantiatePlayerInfoPrefab();
        }
        
        [ObserversRpc(ExcludeServer = true)]
        private void RpcInstantiatePlayerInfoPrefab()
        {
            if (!IsServerInitialized) InstantiatePlayerInfoPrefab();
        }

        public async void InstantiatePlayerInfoPrefab()
        {
            Debug.Log("[CreatePlayerInfoPrefab] InstantiatePlayerInfoPrefab");
            Instantiate(FillPlayerInfo.Instance.PlayerInfoPrefab, FillPlayerInfo.Instance.CanvasPanelHolder);
            await FillPlayerInfo.Instance.RenameAllPlayers();
            await FillPlayerInfo.Instance.RecountAllStats();
        }
    }
}