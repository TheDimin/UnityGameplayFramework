
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JimTheKiwifruit;
using Newtonsoft.Json;
using TowerJump.Save;
using TowerJump.Save.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TowerJump.Achievement
{
    /// <summary>
    /// General Event for Achievements
    /// </summary>
    public class AchievementEvent : UnityEvent<Achievement> { }

    /// <summary>
    /// Event for AchievementProperty and its owning Achievement (for convenience)
    /// </summary>
    public class AchievementPropertyEvent : UnityEvent<Achievement, AchievementProperty> { }

    /// <summary>
    /// Manager class for achievements
    /// </summary>
    // KEEP IT SEALED SINGLETON PREVENTS INHERITANCE EITHER WAY

    [JsonObject(MemberSerialization.OptIn)]
    public sealed class AchievementManager : Singleton<AchievementManager>
    {

        public AchievementEvent OnComplete { get; private set; } = new AchievementEvent();

        public readonly AchievementEventHandlers EventHandlers = new AchievementEventHandlers();

        [JsonProperty, JsonConverter(typeof(DictionaryConverter))]//, JsonConverter(typeof(DictionaryConverter))]
        private Dictionary<string, Achievement> _achievements = new Dictionary<string, Achievement>();

        /// <summary>
        /// Returns the instance of the found achievement
        /// </summary>
        /// <typeparam name="TAchievement">Type of expected achievement</typeparam>
        /// <returns>Found achievement, null If its not found</returns>
        public Achievement GetAchievement(string achievementID)
        {
            return _achievements[achievementID];
        }

        public void RegisterGameEvent(string eventName, UnityEvent _event)
        {
            EventHandlers.RegisterEvent(eventName, _event);
        }

        /// <summary>
        /// Adds a new achievement to manager
        /// </summary>
        /// <typeparam name="TAchievement">Type of the achievement</typeparam>
        /// <returns>instance of the created achievement</returns>
        public Achievement AddAchievement(string displayName, string description, string id, List<AchievementProperty> properties)
        {
            if (!IsUniqueId(displayName)) return null;

            var newAchievement = new Achievement(displayName, description, id, properties);
            newAchievement.SetupAchievementProperties(EventHandlers);
            newAchievement.OnAchievementComplete.AddListener(() => OnComplete.Invoke(newAchievement));
            _achievements.Add(id, newAchievement);
            return newAchievement;
        }

        public void RebindAchievements()
        {
            foreach (var achievementProperty in _achievements.Values.SelectMany(achievement => achievement.AchievementProperties.Values))
            {
                Debug.Log(achievementProperty);
            }
        }

        private bool IsUniqueId(string achievementId) =>_achievements.ContainsKey(achievementId);
    }
}