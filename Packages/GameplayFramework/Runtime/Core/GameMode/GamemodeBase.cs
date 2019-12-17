using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using GameplayFramework.Core.Internal;

namespace GameplayFramework.Core
{
    public abstract class GamemodeBase
    {
        static GamemodeBase gamemode;

        private List<PlayerControllerBase> playercontrollers = new List<PlayerControllerBase>();
        private MapData mapData;

        public void RegisterPlayerController(PlayerControllerBase playerController)
        {
            if (!gamemode.playercontrollers.Contains(playerController))
            {
                gamemode.playercontrollers.Add(playerController);
                SpawnPlayer(playerController);
            }
        }

        public static PlayerControllerBase GetPlayerController(int index = 0)
        {
            return gamemode.playercontrollers[index];
        }

        public static PlayerControllerType GetPlayerController<PlayerControllerType>(int index = 0) where PlayerControllerType : PlayerControllerBase
        {
            return (GetPlayerController(index) as PlayerControllerType);
        }

        public static GameModeType Get<GameModeType>() where GameModeType : GamemodeBase
        {
            return gamemode as GameModeType;
        }

      


        public void SpawnPlayer(PlayerControllerBase playercontroller)
        {
            return;
            // get prefab
            GameObject obj = null;// GameObject.Instantiate(Game.GetGameDefaults().PlayerPawn);
            PawnBase pawn = obj.GetComponent<PawnBase>();
            playercontroller.Posses(pawn);

            SpawnPoint pawnSpawnLocation;
            if (mapData.FindSpawnPointForPawn(pawn, out pawnSpawnLocation))
            {
                Transform spawnTransform = pawnSpawnLocation.GetSpawnPoint;
            //    obj.transform.position = spawnTransform.position;
          //      obj.transform.rotation = spawnTransform.rotation;
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            mapData.CollectGarbage();
        }
        
        
        internal void Start()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            mapData = new MapData();
            gamemode = this;
        }
        internal static void RegisterSpawn(SpawnPoint spawnPoint)
        {
            gamemode.mapData.RegisterSpawnPoint(spawnPoint);
        }
    }
}