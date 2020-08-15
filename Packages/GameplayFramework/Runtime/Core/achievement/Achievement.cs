using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TowerJump.Save.Internal;
using UnityEngine.Events;

namespace TowerJump.Achievement
{
    /// <summary>
    /// Base class For achievements
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Achievement //: IElementKey
    {
        public Achievement()
        {

        }

        /// <summary>
        /// Event Called when this achievement is completed
        /// </summary>
        public UnityEvent OnAchievementComplete { get; } = new UnityEvent();

        /// <summary>
        /// Name that can be displayed to the user
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Information a bout this achievement for the user
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// ID Has to be a unique name that is used for the save system
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Properties of this achievement
        /// </summary>
        [JsonProperty,JsonConverter(typeof(DictionaryConverter))]
        public Dictionary<string,AchievementProperty> AchievementProperties { get; private set; } = new Dictionary<string, AchievementProperty>();

        /// <summary>
        /// Construct a new Achievement.
        /// </summary>
        /// <param name="displayName">Name of the achievement</param>
        /// <param name="description">Explanation of what the user has to do to complete this achievement</param>
        /// <param name="id">ID Used for saving and external applications (eg steam)</param>
        /// <param name="achievementProperties">Properties defining the requirements to complete this achievement</param>
        internal Achievement(string displayName, string description, string id, List<AchievementProperty> achievementProperties)
        {
            DisplayName = displayName;
            Description = description;
            Id = id;
            foreach (var achievementProperty in achievementProperties)
            {
                AchievementProperties.Add(achievementProperty.EventName,achievementProperty);
            }
        }

        /// <summary>
        /// Called after creation of Achievement instance.
        /// DO NOT CALL UNLESS YOU KNOW WHAT YOU ARE DOING.
        /// </summary>
        /// <param name="eventHandlers">Reference to Event Handlers</param>
        public void SetupAchievementProperties(AchievementEventHandlers eventHandlers)
        {
            foreach (var achievementProperty in AchievementProperties)
            {
                achievementProperty.Value.Register(eventHandlers);
                achievementProperty.Value.SetOwner(this);
            }
        }

        /// <summary>
        /// Checks if this Achievement is completed.
        /// </summary>
        /// <returns>Returns true if its completed</returns>
        public virtual bool IsCompleted()
        {
            return AchievementProperties.All(achievementProperty => achievementProperty.Value.IsCompleted());
        }

        /// <summary>
        /// Function called by a property that was just completed
        /// </summary>
        internal void OnPropertyCompleted(Achievement achievement, AchievementProperty property)
        {
            if (this != achievement) return;

            if (IsCompleted())
            {
                OnAchievementComplete.Invoke();
            }
        }

        public string GetKeyElementName()
        {
            return nameof(Id);
        }

        public object GetKeyValue()
        {
            return Id;
        }
    }
}