using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework.Core
{
    [System.Serializable]
    public class MapData
    {
        private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        public void RegisterSpawnPoint(SpawnPoint spawnPoint)
        {
            spawnPoints.Add(spawnPoint);
        }


        public bool FindSpawnPointForPawn(PawnBase pawn, out SpawnPoint spawnPoint)
        {
            List<SpawnPoint> possibPoints = new List<SpawnPoint>();

            foreach (SpawnPoint PossibleSpawnPoint in spawnPoints)
            {
                if (PossibleSpawnPoint.CanPawnSpawnAtPoint(pawn))
                {
                    possibPoints.Add(PossibleSpawnPoint);
                }
            }

            if (possibPoints.Count == 0)
            {
                Debug.LogError($"Failed to find spawnpoint for '{pawn.ToString()}'");
                spawnPoint = null;
                return false;
            }

            SpawnPoint smallPoint = null;
            foreach (var point in possibPoints)
            {
                if (smallPoint == null)
                {
                    smallPoint = point;
                    continue;
                }

                if (smallPoint.spawncount > point.spawncount)
                    smallPoint = point;
            }

            spawnPoint = smallPoint;

            return true;
        }

        /// <summary>
        /// Check for nullref spawnpoints , and remove them;
        /// </summary>
        internal void CollectGarbage()
        {
            List<int> toDeleteIndexes = new List<int>();

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (spawnPoints[i] == null)
                    toDeleteIndexes.Add(i);
            }

            //reverse loop trough the list, remove null reff spawnpoints from the end to the start;
            for (int i = toDeleteIndexes.Count; i > 0; i--)
            {
                spawnPoints.RemoveAt(i);
            }

        }
    }
}
