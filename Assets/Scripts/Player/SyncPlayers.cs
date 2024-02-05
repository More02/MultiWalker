using System;
using System.Collections;
using System.Linq;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Класс, отвечающий за синхронизацию списка игроков
    /// </summary>
    public class SyncPlayers : NetworkBehaviour
    {
        private void Start()
        {
           // InstanceFinder.ClientManager.OnClientConnectionState += Init;
           
           //WAS COMMENTED
           // StartCoroutine(Init_Routine());
           //Init();
        }

        // public override void OnStartLocalPlayer()
        // {
        //     Init();
        // }
        
        private IEnumerator Init_Routine()
        {
            yield return new WaitForSeconds(1f);
            Init();
        }
        
        public void Init()
        {
            // Debug.Log("Init");
            // Debug.Log("IsClientInitialized = "+IsClientInitialized);
            // Debug.Log("OnStartClientCalled = "+OnStartClientCalled);
            // Debug.Log("Owner.IsLocalClient = "+Owner.IsLocalClient);
            if (Owner.IsLocalClient)
            {
               // Debug.Log("Init Owner.IsLocalClient");
                Debug.Log("[SyncPlayers] Init Owner.IsLocalClient");
                SyncListOfPlayers();
            }
            gameObject.GetComponent<CreatePlayerInfoPrefab>().CmdInstantiatePlayerInfoPrefab();
        }
        
        // private void Init(ClientConnectionStateArgs сlientConnectionStateArgs)
        // {
        //     if (Owner.IsLocalClient) SyncListOfPlayers();
        //     gameObject.GetComponent<CreatePlayerInfoPrefab>().CmdInstantiatePlayerInfoPrefab();
        // }

        private void SyncListOfPlayers()
        {
            foreach (var playerIdentity in InstanceFinder.ClientManager.Connection.Objects.Where(playerIdentity =>
                         playerIdentity != Owner.IsLocalClient))
            {
                gameObject.GetComponent<CreatePlayerInfoPrefab>().InstantiatePlayerInfoPrefab();
            }
        }
    }
}