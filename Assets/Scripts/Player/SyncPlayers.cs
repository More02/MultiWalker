using System.Linq;
using Mirror;

namespace Player
{
    /// <summary>
    /// Класс, отвечающий за синхронизацию списка игроков
    /// </summary>
    public class SyncPlayers : NetworkBehaviour
    {
        public override void OnStartLocalPlayer()
        {
            Init();
        }

        private void Init()
        {
            if (isLocalPlayer) SyncListOfPlayers();
            gameObject.GetComponent<CreatePlayerInfoPrefab>().CmdInstantiatePlayerInfoPrefab();
        }

        private void SyncListOfPlayers()
        {
            foreach (var playerIdentity in NetworkClient.spawned.Values.Where(playerIdentity =>
                         playerIdentity != NetworkClient.localPlayer))
            {
                gameObject.GetComponent<CreatePlayerInfoPrefab>().InstantiatePlayerInfoPrefab();
            }
        }
    }
}