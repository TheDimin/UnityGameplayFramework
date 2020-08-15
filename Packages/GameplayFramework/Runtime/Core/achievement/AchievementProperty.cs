using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace TowerJump.Achievement
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AchievementProperty
    {
        /// <summary>
        /// Event Invoked when this achievement property made progress
        /// </summary>
        public AchievementPropertyEvent OnProgress { get; } = new AchievementPropertyEvent();
        /// <summary>
        /// Event Invoked when this achievement property Has reached its target amount
        /// </summary>
        public AchievementPropertyEvent OnComplete { get; } = new AchievementPropertyEvent();

        private UnityEvent propertyAlreadyCompleted = new UnityEvent();

        /// <summary>
        /// Name of the event register in AchievementEventHandlers
        /// </summary>
        public string EventName { get; private set; }

        /// <summary>
        /// Owning Achievement of this Property
        /// </summary>
        public Achievement Owner { get; private set; }
        /// <summary>
        /// Faction of the current progress
        /// </summary>
        public float ProgressFaction => TargetAmount / (float)CurrentAmount;

        /// <summary>
        /// Amount this property expects to complete
        /// Can only be set ONCE 
        /// </summary>
        public ushort TargetAmount { get; private set; }

        /// <summary>
        /// current Amount of progress for this property
        /// </summary>
        [JsonProperty]
        public int CurrentAmount { get; private set; }

        public AchievementProperty(string eventName, ushort targetAmount)
        {
            EventName = eventName;
            TargetAmount = targetAmount;
        }

        /// <summary>
        /// Register this property to Its AchievementHandler
        /// </summary>
        /// <param name="achievementEventHandler"></param>
        internal virtual void Register(AchievementEventHandlers achievementEventHandler)
        {
            //Register to the Event Handler value change
            achievementEventHandler.AddListener(EventName, EventListener);
            OnComplete.AddListener((a, b) =>
            {
                achievementEventHandler.RemoveListener(EventName, EventListener);
            });

            propertyAlreadyCompleted.AddListener(() =>
            {
                achievementEventHandler.RemoveListener(EventName, EventListener);
            });
        }

        protected virtual void EventListener()
        {
            UpdateProgress(1);
        }

        internal void RebindProperty()
        {

        }

        /// <summary>
        /// Set the Owner of this AchievementProperty
        /// </summary>
        /// <param name="owner"></param>
        internal void SetOwner(Achievement owner)
        {
            if (Owner == null)
            {
                Owner = owner;
            }
            else
            {
                throw new Exception("Attempted to change owner of a achievementProperty");
            }

            OnComplete.AddListener(owner.OnPropertyCompleted);
        }


        /// <summary>
        /// Update the progress of this property
        /// </summary>
        /// <param name="incrementValue"></param>
        //TODO support for value type achievement (eg fall 500 meters)
        protected virtual void UpdateProgress(uint incrementValue)
        {
            //Most likly happend after loading in game. just remove the update caller and call it a day.
            if (CurrentAmount >= TargetAmount)
            {
                propertyAlreadyCompleted.Invoke();
                return;
            }

       

            CurrentAmount += (int)incrementValue;
            OnProgress.Invoke(Owner, this);

            Debug.Log(CurrentAmount);

            if (CurrentAmount >= TargetAmount)
            {
                OnComplete.Invoke(Owner, this);
            }
        }

        /// <summary>
        /// Has the target amount been reached
        /// </summary>
        /// <returns></returns>
        public bool IsCompleted()
        {
            return TargetAmount >= CurrentAmount;
        }
    }
}
