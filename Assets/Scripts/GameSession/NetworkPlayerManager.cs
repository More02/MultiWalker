using System.Collections;
using Mirror;
using Movement;
using Player;
using Spawn;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSession
{
    /// <summary>
    /// Класс NetworkManager, отвечающий за начало и конец игровой сессии
    /// </summary>
    public class NetworkPlayerManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            InstantiatePlayer(conn);
        }

        private void InstantiatePlayer(NetworkConnectionToClient conn)
        {
            var randomPlace = Random.Range(0, SpawnPointsHolder.Instance.AllSpawnPoints.Count - 1);
            var player = Instantiate(playerPrefab, SpawnPointsHolder.Instance.AllSpawnPoints[randomPlace].position,
                SpawnPointsHolder.Instance.AllSpawnPoints[randomPlace].rotation);
            player.name = $"{playerPrefab.name} {conn.connectionId + 1}";
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            SpawnPointsHolder.Instance.AllSpawnPoints.RemoveAt(randomPlace);
        }

        public void EndOfGameSession()
        {
            StartCoroutine(RestartSceneDelayed());
        }

        private IEnumerator RestartSceneDelayed()
        {
            yield return new WaitForSeconds(WinGame.Instance.DelayTime);

            var networkManager = gameObject.GetComponent<NetworkManager>();
            if (networkManager is null)
            {
                yield break;
            }

            SpawnPointsHolder.Instance.FillListOfPoints();

            for (var i = 0; i < FillPlayerInfo.Instance.PlayerScore.Count; i++)
            {
                FillPlayerInfo.Instance.PlayerScore[i] = 0;
            }

            foreach (var playerIdentity in NetworkClient.spawned.Values)
            {
                var randomPlace = Random.Range(0, SpawnPointsHolder.Instance.AllSpawnPoints.Count - 1);
                var player = playerIdentity.transform;
                player.position = SpawnPointsHolder.Instance.AllSpawnPoints[randomPlace].position;
                player.rotation = SpawnPointsHolder.Instance.AllSpawnPoints[randomPlace].rotation;
                SpawnPointsHolder.Instance.AllSpawnPoints.RemoveAt(randomPlace);
                player.gameObject.GetComponent<PlayerScore>().CmdChangeScore(0, player.gameObject.name);
                if (playerIdentity.isClient)
                {
                    SetActiveWinCanvas.Instance.CmdSetActiveWinPrefab(false);
                }

                WinGame.Instance.WinCanvas.SetActive(false);
                player.gameObject.GetComponent<DashAbility>().CountOfSuccessDash = 0;
                DashAbility.Instance.IsWin = false;
            }
        }
    }
}