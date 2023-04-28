using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Player;
using UnityEngine;

namespace Spawn
{
    public class CreateInfoPrefab : NetworkBehaviour
    {
        // private List<NetworkIdentity> _connectedPlayers = new();
        public string PersonName { get; set; }

        public override void OnStartLocalPlayer()
        {
            Init();
        }

        public void Init()
        {
            if (isLocalPlayer) SyncListOfPlayers();
            CmdInstantiatePlayerInfoPanel(NetworkClient.localPlayer.name);
        }

        private void SyncListOfPlayers()
        {
            foreach (var player in NetworkClient.spawned.Where(player => player.Value != NetworkClient.localPlayer))
            {
                InstantiatePlayerInfoPanel(player.Value.name);
                //Debug.Log(player.Value.gameObject.name);
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

        private static void InstantiatePlayerInfoPanel(string playerName)
        {
            var playerInfoPanel =
                Instantiate(InfoCanvas.Instance.PlayerInfoPrefab, InfoCanvas.Instance.CanvasPanelHolder);
            //NetworkServer.Spawn(playerInfoPanel);
            //Debug.Log(NetworkServer.spawned.Count);
            Debug.Log(NetworkClient.spawned.Count);
            InfoCanvas.FirstFillPlayerInfo(playerName, playerInfoPanel);
        }

        

        private void Update()
        {
            Debug.Log(gameObject.name);
        }
    }
}