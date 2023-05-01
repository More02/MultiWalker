using System.Linq;
using Mirror;
using Player;

namespace Spawn
{
    public class CreateInfoPrefab : NetworkBehaviour
    {
        public override void OnStartLocalPlayer()
        {
            Init();
        }

        private void Init()
        {
            if (isLocalPlayer) SyncListOfPlayers();
            CmdInstantiatePlayerInfoPanel(NetworkClient.localPlayer.name);
        }

        private void SyncListOfPlayers()
        {
            foreach (var playerIdentity in NetworkClient.spawned.Values.Where(playerIdentity =>
                         playerIdentity != NetworkClient.localPlayer))
            {
                InstantiatePlayerInfoPanel(playerIdentity.name);
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdInstantiatePlayerInfoPanel(string playerName)
        {
            RpcInstantiatePlayerInfoPanel(playerName);
            InstantiatePlayerInfoPanel(playerName);
        }

        [ClientRpc]
        private void RpcInstantiatePlayerInfoPanel(string playerName)
        {
            if (!isServer) InstantiatePlayerInfoPanel(playerName);
        }

        private static async void InstantiatePlayerInfoPanel(string playerName)
        {
            var playerInfoPrefab =
                Instantiate(InfoCanvas.Instance.PlayerInfoPrefab, InfoCanvas.Instance.CanvasPanelHolder);
            InfoCanvas.FirstFillPlayerInfo(playerName, playerInfoPrefab);
            await InfoCanvas.Instance.RenameAllPlayers();
            await InfoCanvas.Instance.RecountAllStats();
        }
    }
}