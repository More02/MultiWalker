using UnityEngine;

namespace Player
{
    public class PlayerFactory : MonoBehaviour
    {
        [SerializeField] private Transform _playerPrefab;

        private void InstantiateCards()
        {
            var i = 0;
            while (i < SpawnPointsHolder.Instanse.AllSpawnPoints.Count)
            {
                var randomPlace = Random.Range(0, SpawnPointsHolder.Instanse.AllSpawnPoints.Count - 1);
                if (CheckAddChildrenAvailable(randomPlace))
                {
                    Instantiate(_playerPrefab, transform.GetChild(randomPlace));
                }
                else
                {
                    while (!CheckAddChildrenAvailable(randomPlace))
                    {
                        randomPlace = Random.Range(0, SpawnPointsHolder.Instanse.AllSpawnPoints.Count);
                    }

                    Instantiate(_playerPrefab, transform.GetChild(randomPlace));
                }

                i++;
            }
        }

        private bool CheckAddChildrenAvailable(int randomPlace)
        {
            return transform.GetChild(randomPlace).childCount == 0;
        }

        private void Start()
        {
            InstantiateCards();
        }
    }
}