using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameplayFramework.Core;
using System.IO;

namespace GameplayFramework.Editor
{
    public class GameExplorer : EditorWindow
    {

        static GameExplorer editorInstance;

        [MenuItem("GameplayFramework/GameExplorer")]
        public static void Open()
        {
            if (editorInstance == null)
            {
                editorInstance = EditorWindow.GetWindow<GameExplorer>();
                editorInstance.titleContent = new GUIContent("GameExplorer");
            }

            GameSettings settings = FindObjectOfType<GameSettings>();
            if(settings == null)
            {
                settings = Resources.Load<GameSettings>("GameSettings");

            }

            

            //in case a user deleted the settings object
            if (settings == null)
            {
                Debug.LogWarning("Created new instance of gameSettings");
                settings = CreateInstance<GameSettings>();
                
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }

                //TODO create this inside Packages/GameplayFramework/GameSettings.asset instead
                AssetDatabase.CreateAsset(settings, "Assets/Resources/GameSettings.asset");
            }

            Debug.Log($"pre:{settings.gamemode != null}");
            settings.gamemode = new GamemodeBase();


            Debug.Log($"preLUL:{settings.a}");
            settings.a = "ree";
            Debug.Log($"postLUL:{settings.a}");
            Debug.Log($"post:{settings.gamemode != null}");
            EditorUtility.SetDirty(settings);

            AssetDatabase.SaveAssets();

            editorInstance.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("test");

        }
    }
}
