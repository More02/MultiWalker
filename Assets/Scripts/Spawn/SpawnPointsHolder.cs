using System.Collections.Generic;
using UnityEngine;

namespace Spawn
{
    public class SpawnPointsHolder : MonoBehaviour
    {
        public List<Transform> AllSpawnPoints { get; } = new();

        public static SpawnPointsHolder Instanse { get; private set; }

        private void Awake()
        {
            Instanse = this;
            FillListOfPoints();
        }

        private void FillListOfPoints()
        {
            foreach (var point in transform.GetComponentsInChildren<Transform>())
            {
                if (point == transform) continue;
                AllSpawnPoints.Add(point);
            }
        }
    }
}