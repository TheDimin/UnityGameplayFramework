using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using JimTheKiwifruit;
using TowerJump.Save.Internal;
using TowerJump.Save.provider;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace TowerJump.Save
{
    /// <summary>
    /// Easy tool to make your class savable, manualy adding Saveclass attribute is also valid
    /// </summary>
    [SaveClass, RequireComponent(typeof(GuidComponent))]
    public abstract class SavableToDisk : MonoBehaviour
    {

    }

    namespace Internal
    {
        [Serializable]
        public class SaveObjectInfo
        {

            public GameObject GameObject;
            private Dictionary<string, Component> _saveComponents = new Dictionary<string, Component>();
            public readonly GuidComponent GuidComponent;

            public Guid GetGuid => GuidComponent.GetGuid();


            public SaveObjectInfo(GuidComponent guidComponent, List<MonoBehaviour> saveComponents)
            {
                GameObject = guidComponent.gameObject;

                if (saveComponents.Any(saveComponent => saveComponent.gameObject != GameObject))
                {
                    throw new ArgumentException("Component is not from the Game object");
                }

                foreach (var component in saveComponents)
                {
                    _saveComponents.Add(component.GetType().Name, component);
                }


                if (!GameObject.isStatic)
                {
                    _saveComponents.Add(GameObject.transform.GetType().Name, GameObject.transform);
                }



                GuidComponent = guidComponent;
            }

            public override int GetHashCode()
            {
                if (GuidComponent == null)
                    throw new NullReferenceException("Saved object expected the game object to have a GUID Component");

                return BitConverter.ToInt32(GuidComponent.GetGuid().ToByteArray(), 0);
            }

            public Component GetComponentFromName(string componentName)
            {
                return _saveComponents[componentName];
            }

            public Component[] GetComponents()
            {
                return _saveComponents.Values.ToArray();
            }

        }
    }

    //[ExecuteInEditMode]
    public sealed class SaveManager : Singleton<SaveManager>
    {
        protected static SaveProviderBase SaveProvider;
        protected static LoadProviderBase LoadProvider;

        private void OnApplicationQuit()
        {
            Save();
        }

        protected override void Awake()
        {
            base.Awake();

            if (Application.isPlaying)
            {
                DontDestroyOnLoad(this);
            }
        }

        private void Start()
        {
            if (!Application.isPlaying) return;

            //  SceneManager.sceneUnloaded += (scene) =>
            //  {
            //      SaveAllObjects();
            //  };

            //Load();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("Editor Save called");
                Save();
            }

            if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("Editor Load called");
                Load();
            }
        }
#endif

        /// <summary>
        /// Saves all currently loaded scenes
        /// </summary>
        public static void Save()
        {
            SaveAllObjects();
        }

        /// <summary>
        /// Set all savable data for loaded objects to latest save version
        /// </summary>
        public static void Load()
        {
            LoadAllObjects();
        }

        /// <summary>
        /// Saved all data for loaded objects
        /// </summary>
        private static void SaveAllObjects()
        {

            foreach (var saveObject in GetSaveObjects())
            {
                SaveProvider.Start(saveObject);
            }
        }

        /// <summary>
        /// Loads all data for loaded objects
        /// </summary>
        private static void LoadAllObjects()
        {
            foreach (var saveObject in GetSaveObjects())
            {
                LoadProvider.Start(saveObject);
            }
        }

        public static SaveObjectInfo[] GetSaveObjects()
        {
            var saveObjectInfos = new List<SaveObjectInfo>();
            var classes = GetSavableTypes();

            var guidObjects = (from guidComponentObject in FindObjectsOfType<GuidComponent>()
                               select guidComponentObject).ToArray();

            foreach (var guidObject in guidObjects)
            {
                var saveComponents = new List<MonoBehaviour>();
                saveComponents.AddRange(classes.Select(saveClass => (MonoBehaviour)guidObject.GetComponent(saveClass)).Where(savableComponent => savableComponent));
                if (saveComponents.Count != 0)
                    saveObjectInfos.Add(new SaveObjectInfo(guidObject, saveComponents));
            }
            return saveObjectInfos.ToArray();
        }



#if UNITY_EDITOR
        [MenuItem("Tools/SaveSystem/Fix missing Guids"), Obsolete("shouldn't be needed anymore")]
        private static void FixMissingGuidComponents()
        {
            foreach (var monoBehaviour in GetSavableMonoBehaviours())
            {
                if (!monoBehaviour.GetComponent<GuidComponent>())
                {
                    monoBehaviour.gameObject.AddComponent<GuidComponent>();
                }
            }

            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveScenes(GetOpenScenes());
        }

        private static MonoBehaviour[] GetSavableMonoBehaviours()
        {
            var classes = GetSavableTypes();

            return (from scene in GetOpenScenes()
                    from rootObject in scene.GetRootGameObjects()
                    from saveClass in classes
                    from vComponent in rootObject.GetComponentsInChildren(saveClass)
                    select vComponent).Cast<MonoBehaviour>().ToArray();
        }

        private static Scene[] GetOpenScenes()
        {

            var scenes = new List<Scene>();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                scenes.Add(SceneManager.GetSceneAt(i));
            }

            return scenes.ToArray();
        }

#endif




        private static Type[] GetSavableTypes()
        {
            return (from classType in Assembly.GetAssembly(typeof(SaveManager)).GetTypes()
                    where classType.IsDefined(typeof(SaveClassAttribute))
                    where classType.IsSubclassOf(typeof(MonoBehaviour))
                    where !classType.IsAbstract
                    where !classType.IsInterface
                    select classType).ToArray();
        }


    }
}

