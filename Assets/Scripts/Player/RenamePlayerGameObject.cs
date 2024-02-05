using System.Collections;
using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class RenamePlayerGameObject : NetworkBehaviour
    {
        public UnityEvent OnReadyToSpawnPlayerInfoPrefabs;
        public UnityEvent OnReadySavePlayersNames;
        
        private void Start()
        {
            OnReadyToSpawnPlayerInfoPrefabs.AddListener(GetComponent<SyncPlayers>().Init);
            OnReadySavePlayersNames.AddListener(GetComponent<PlayerName>().Init);
           
            //JUST COMMENTED
            if (Owner.IsLocalClient) StartCoroutine(Init_Routine());
            
            // CmdRenamePlayer();

            // RpcRenamePlayer();
            //gameObject.name = "Player " + FillPlayerInfo.Instance.CanvasPanelHolder.childCount;
        }

        public IEnumerator Init_Routine()
        {
            yield return new WaitForSeconds(0.3f);
            CmdRenamePlayer();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void CmdRenamePlayer()
        {
            Debug.Log("[RenamePlayerGameObject] CmdRenamePlayerGameObject");
            RpcRenamePlayer();
            RenamePlayer();
        }

        [ObserversRpc(ExcludeServer = true)]
        private void RpcRenamePlayer()
        {
            if (!(IsServerInitialized && IsClientInitialized)) RenamePlayer();
            Debug.Log("RpcRenamePlayer");
        }

        private void RenamePlayer()
        {
            if (!char.IsDigit(gameObject.name[^1]))
            {
                GetComponent<PlayerName>().Name = "Player " + (InstanceFinder.ClientManager.Clients.Values.Count);
                Debug.Log("[RenamePlayerGameObject] RenamePlayerGameObject");
                OnReadySavePlayersNames.Invoke();
                OnReadyToSpawnPlayerInfoPrefabs.Invoke();
            }

            Debug.Log("FillPlayerInfo.Instance.PlayerNames.Count = " + InstanceFinder.ClientManager.Clients.Values.Count);

            //if ((!char.IsDigit(gameObject.name[^1])) || (Owner.IsLocalClient))
            
        }
    }
}
