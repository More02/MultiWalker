using System;
using System.Collections;
using System.Linq;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;
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
          // StartCoroutine(Init_Routine());
           //Init();
        }

        // public override void OnStartLocalPlayer()
        // {
        //     Init();
        // }

        public void Init()
        {
            Debug.Log("Init");
            Debug.Log("IsClientInitialized = "+IsClientInitialized);
            Debug.Log("OnStartClientCalled = "+OnStartClientCalled);
            Debug.Log("IsOwner = "+IsOwner);
            if (IsOwner)
            {
                Debug.Log("Init IsOwner");
                SyncListOfPlayers();
            }
            gameObject.GetComponent<CreatePlayerInfoPrefab>().CmdInstantiatePlayerInfoPrefab();
        }
        
        // private void Init(ClientConnectionStateArgs сlientConnectionStateArgs)
        // {
        //     if (IsOwner) SyncListOfPlayers();
        //     gameObject.GetComponent<CreatePlayerInfoPrefab>().CmdInstantiatePlayerInfoPrefab();
        // }

        private IEnumerator Init_Routine()
        {
            yield return new WaitForSeconds(0.5f);
            Init();
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