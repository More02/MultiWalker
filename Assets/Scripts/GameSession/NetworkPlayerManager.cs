using System;
using System.Collections;
using FishNet;
using FishNet.Component.Spawning;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Transporting;
using Movement;
using Player;
using Spawn;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSession
{
    /// <summary>
    /// Класс NetworkManager, отвечающий за начало и конец игровой сессии
    /// </summary>
    public class NetworkPlayerManager : MonoBehaviour
    {
        private void OnEnable()
        {
            //InstanceFinder.ServerManager.OnRemoteConnectionState += InstantiatePlayer;
            // InstanceFinder.ServerManager.OnRemoteConnectionState += InstantiatePlayer;
        }

        // public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        // {
        //     InstantiatePlayer(conn);
        // }

        // private void InstantiatePlayer(NetworkConnection conn, RemoteConnectionStateArgs _emoteConnectionStateArgs)
        // {
        //     var randomPlace = Random.Range(0, SpawnPointsHolder.Instance.AllSpawnPoints.Count - 1);
        //     var player = Instantiate(PlayerSpawner.playerPrefab, SpawnPointsHolder.Instance.AllSpawnPoints[randomPlace].position,
        //         SpawnPointsHolder.Instance.AllSpawnPoints[randomPlace].rotation);
        //     player.name = $"{playerPrefab.name} {conn.connectionId + 1}";
        //     InstanceFinder.ServerManager.AddPlayerForConnection(conn, player.gameObject);
        //     SpawnPointsHolder.Instance.AllSpawnPoints.RemoveAt(randomPlace);
        // }

        public void EndOfGameSession()
        {
            StartCoroutine(RestartSceneDelayed());
        }

        private IEnumerator RestartSceneDelayed()
        {
            yield return new WaitForSeconds(WinGame.Instance.DelayTime);

            var networkManager = InstanceFinder.NetworkManager;
            if (networkManager is null)
            {
                yield break;
            }

            //SpawnPointsHolder.Instance.FillListOfPoints();

            for (var i = 0; i < FillPlayerInfo.Instance.PlayerScore.Count; i++)
            {
                FillPlayerInfo.Instance.PlayerScore[i] = 0;
            }

            var stepCount = 0;
            foreach (var playerIdentity in InstanceFinder.ClientManager.Clients.Values)
            {
                //var randomPlace = Random.Range(0, SpawnPointsHolder.Instance.AllSpawnPoints.Count - 1);
                //var randomPlace = stepCount;
                var player = playerIdentity.FirstObject.transform;
                Debug.Log(player.gameObject.name);
                player.position = SpawnPointsHolder.Instance.AllSpawnPoints[stepCount].position;
                player.rotation = SpawnPointsHolder.Instance.AllSpawnPoints[stepCount].rotation;
               // SpawnPointsHolder.Instance.AllSpawnPoints.RemoveAt(randomPlace);
                player.gameObject.GetComponent<PlayerScore>().CmdChangeScore(0, player.gameObject.name);
                // if (playerIdentity.IsClientInitialized)
                // {
                    SetActiveWinCanvas.Instance.CmdSetActiveWinPrefab(false);
               // }

                WinGame.Instance.WinCanvas.SetActive(false);
                player.gameObject.GetComponent<DashAbility>().CountOfSuccessDash = 0;
                DashAbility.Instance.IsWin = false;

                stepCount++;
            }
           // SpawnPointsHolder.Instance.AllSpawnPoints.Clear();
        }
    }
}