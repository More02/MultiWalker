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
            Debug.Log("CmdRenamePlayer");
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
            gameObject.name = "Player " + FillPlayerInfo.Instance.CanvasPanelHolder.childCount;
            Debug.Log("RenamePlayer");
            OnReadySavePlayersNames.Invoke();
            OnReadyToSpawnPlayerInfoPrefabs.Invoke();
        }
    }
}
