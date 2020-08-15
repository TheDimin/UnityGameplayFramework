using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace TowerJump.Achievement
{
    /// <summary>
    /// Responsible for containing all references to EventHandlers / delegates that Achievements can Require
    /// </summary>
    public sealed class AchievementEventHandlers
    {
        /// <summary>
        /// All events known by the EventHandlers
        /// </summary>
        private readonly Dictionary<string, UnityEventBase> _events = new Dictionary<string, UnityEventBase>();

        /// <summary>
        /// Get a list of all the Events
        /// </summary>
        /// <returns>List of unity events</returns>
        public List<UnityEventBase> GetEventHandlers()
        {
            return _events.Values.ToList();
        }

        /// <summary>
        /// Register a Unity event
        /// </summary>
        /// <param name="name">Name users can use to find this event</param>
        /// <param name="eventHandle">Reference to the UnityEvent</param>
        /// <returns> Was the event register successfully ?</returns>
        public bool RegisterEvent(string name, UnityEventBase eventHandle)
        {
            if (_events.ContainsKey(name) || _events.ContainsValue(eventHandle))
            {
                Debug.LogWarning("Attempted to register a already register Event");
                return false;
            }

            _events.Add(name, eventHandle);
            return true;
        }

        /// <summary>
        /// Add a listener to a UnityEvent
        /// </summary>
        /// <param name="name">Name users can use to find this event</param>
        /// <param name="action">Action to be executed</param>
        /// <returns>Was a listener added successful</returns>
        public bool AddListener(string name, UnityAction action)
        {
            UnityEvent targetEvent = (UnityEvent)_events[name];
            if (targetEvent == null)
            {
                Debug.Log("Failed to find event named:" + name);
                return false;
            }

            targetEvent.AddListener(action);
            return true;
        }

        /// <summary>
        /// Add a listener to a UnityEvent with 1 parameter
        /// </summary>
        /// <typeparam name="T1">Type of UnityEvent parameter</typeparam>
        /// <param name="name">Name users can use to find this event</param>
        /// <param name="action">Action to be executed</param>
        /// <returns>Was a listener added successful</returns>
        public bool AddListener<T1>(string name, UnityAction<T1> action)
        {
            UnityEvent<T1> targetEvent = (UnityEvent<T1>)_events[name];
            if (targetEvent == null)
                return false;

            targetEvent.AddListener(action);
            return true;
        }

        /// <summary>
        /// Remove a listener from a UnityEvent
        /// </summary>
        /// <param name="name">Name users can use to find this event</param>
        /// <param name="action">Action you want to remove</param>
        /// <returns></returns>
        public bool RemoveListener(string name, UnityAction action)
        {
            var targetEvent = (UnityEvent)_events[name];
            if (targetEvent == null)
                return false;


            targetEvent.RemoveListener(action);
            return true;
        }

        /// <summary>
        /// Remove a listener from a UnityEvent with 1 parameter
        /// </summary>
        /// <typeparam name="T1">Type of UnityEvent parameter</typeparam>
        /// <param name="name">Name users can use to find this event</param>
        /// <param name="action">Action you want to remove</param>
        /// <returns></returns>
        public bool RemoveListener<T1>(string name, UnityAction<T1> action)
        {
            var targetEvent = (UnityEvent<T1>)_events[name];
            if (targetEvent == null)
                return false;


            targetEvent.RemoveListener(action);
            return true;
        }

    }
}