using System.Collections;
using System.Collections.Generic;
using Microsoft.CSharp;
using TowerJump.Achievement;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework.ExampleCode
{
    public class AchievementExample : MonoBehaviour
    {
        private readonly UnityEvent _onClickEvent = new UnityEvent();
        private void Start()
        {
            //Get a ref to the AchievementManager
            var achievementManager = AchievementManager.Instance;

            //Do something when an achievement is completed
            achievementManager.OnComplete.AddListener(
                (achievement) =>
                {
                    Debug.Log($"Completed achievement: {achievement.DisplayName}");
                });

            //Add a eventHandler.
            //Name of the event and eventHandle (of type unityEvent)
            achievementManager.EventHandlers.RegisterEvent("OnClickEvent", _onClickEvent);

            //Register Achievement ( Make sure that the event used by the achievement has been added !
            achievementManager.AddAchievement(
                "First click", 
                "You completed your first click !", 
                "FIRST_CLICK",
                new List<AchievementProperty>
                {
                    new AchievementProperty("OnClickEvent", 1)
                });

            achievementManager.AddAchievement(
                "STOP CLICKING",
                "Gosh can you leave your left mouse button alone ? (press left mouse button 10 times)",
                "Keep_Clicking",
                new List<AchievementProperty>
                {
                    new AchievementProperty("OnClickEvent", 10)
                });
        }

        private void Update()
        {
           // //Example of a event Invoke Implementation
            if (Input.GetMouseButtonDown(0))
                _onClickEvent.Invoke();
        }
    }
}