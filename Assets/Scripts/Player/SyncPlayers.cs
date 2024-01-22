using System.Linq;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;

namespace Player
{
    /// <summary>
    /// Класс, отвечающий за синхронизацию списка игроков
    /// </summary>
    public class SyncPlayers : NetworkBehaviour
    {
        private void OnEnable()
        {
            InstanceFinder.ClientManager.OnClientConnectionState += Init;
        }

        // public override void OnStartLocalPlayer()
        // {
        //     Init();
        // }

        private void Init(ClientConnectionStateArgs сlientConnectionStateArgs)
        {
            if (IsOwner) SyncListOfPlayers();
            gameObject.GetComponent<CreatePlayerInfoPrefab>().CmdInstantiatePlayerInfoPrefab();
        }

        private void SyncListOfPlayers()
        {
            foreach (var playerIdentity in InstanceFinder.ClientManager.Connection.Objects.Where(playerIdentity =>
                         playerIdentity != IsOwner))
            {
                gameObject.GetComponent<CreatePlayerInfoPrefab>().InstantiatePlayerInfoPrefab();
            }
        }
    }
}