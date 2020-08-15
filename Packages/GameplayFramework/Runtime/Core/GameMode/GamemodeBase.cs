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
        private GameObject PlayerPawn;

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


        public GamemodeBase(GameObject playerPawn)
        {
            gamemode = this;
            this.PlayerPawn = playerPawn;
        }

        public GameObject SpawnPlayer(PlayerControllerBase playercontroller)
        {
            // get prefab
            GameObject newPawn = GameObject.Instantiate(PlayerPawn);
            PawnBase pawn = newPawn.GetComponent<PawnBase>();
            playercontroller.Posses(pawn);

            SpawnPoint pawnSpawnLocation;
            if (mapData.FindSpawnPointForPawn(pawn, out pawnSpawnLocation))
            {
                pawnSpawnLocation.spawncount++;
                Transform spawnTransform = pawnSpawnLocation.GetSpawnPoint;
                newPawn.transform.position = spawnTransform.position;
                newPawn.transform.rotation = spawnTransform.rotation;
            }

            return newPawn;
        }

        public static void Respawn(PlayerControllerBase playercontroller)
        {
            PawnBase pawn = playercontroller.possedPawn.GetComponent<PawnBase>();
            SpawnPoint pawnSpawnLocation;
            if (gamemode.mapData.FindSpawnPointForPawn(pawn, out pawnSpawnLocation))
            {
                pawnSpawnLocation.spawncount++;
                Transform spawnTransform = pawnSpawnLocation.GetSpawnPoint;
                pawn.transform.position = spawnTransform.position;
                pawn.transform.rotation = spawnTransform.rotation;
            }
        }


        public static void MovePlayerToSpawn()
        {

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
            if(gamemode.mapData == null)
                gamemode.mapData = new MapData();

            gamemode.mapData.RegisterSpawnPoint(spawnPoint);
        }
    }
}