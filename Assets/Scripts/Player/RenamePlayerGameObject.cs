using System.Collections;
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
            OnReadyToSpawnPlayerInfoPrefabs.AddListener(gameObject.GetComponent<SyncPlayers>().Init);
            OnReadySavePlayersNames.AddListener(gameObject.GetComponent<PlayerName>().Init);
           
            //JUST COMMENTED
            StartCoroutine(Init_Routine());
           
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

        [ObserversRpc(ExcludeServer = false)]
        private void RpcRenamePlayer()
        {
            if (IsServerInitialized) return;
            RenamePlayer();
            Debug.Log("RpcRenamePlayer");
        }

        private void RenamePlayer()
        {
            if (!char.IsDigit(gameObject.name[^1]))
            {
                gameObject.name = "Player " + (FillPlayerInfo.Instance.CanvasPanelHolder.childCount + 1);
                Debug.Log("[RenamePlayerGameObject] RenamePlayerGameObject");
            }
            
            OnReadySavePlayersNames.Invoke();
            OnReadyToSpawnPlayerInfoPrefabs.Invoke();

            Debug.Log("FillPlayerInfo.Instance.PlayerNames.Count = " + FillPlayerInfo.Instance.PlayerNames.Count);

            //if ((!char.IsDigit(gameObject.name[^1])) || (Owner.IsLocalClient))
            
        }
    }
}
