using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameplayFramework.Core
{

    /// <summary>
    /// A location the player can spawn at
    /// </summary>
    [AddComponentMenu("GameplayFramework/SpawnPoint")]
    public class SpawnPoint : MonoBehaviour
    {
        public Transform GetSpawnPoint => transform;

        /// <summary>
        /// Checks if the given pawn is allowed to spawn at this location
        /// With inheritance you can modify the pawn requirements
        /// </summary>
        /// <param name="pawn">pawn that tries to spawn</param>
        /// <returns>returns true when the player is allowed to spawn here</returns>
        public virtual bool CanPawnSpawnAtPoint(PawnBase pawn)
        {
            return true;
        }

        private void Awake()
        {
            transform.localScale = Vector3.one;
            GamemodeBase.RegisterSpawn(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.black;
            Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(transform.forward * 5, transform.up), 1, EventType.Repaint);

            //TODO check if the player could spawn here (throw error if spawn point is stuck inside a wall)
        }

#endif
    }
}
