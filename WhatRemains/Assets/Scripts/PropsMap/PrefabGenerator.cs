
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
     
    [ExecuteInEditMode]
    public class PrefabGenerator : MonoBehaviour
    {
        [Header("Input Settings")]
        [SerializeField] private SpriteDatabaseSO spriteDatabase;
        [SerializeField] private GameObject prefabTemplate;
     
        [Header("Output Settings")]
        [SerializeField] private string outputDirectory = "Assets/Prefabs/Nature";
        [SerializeField] private string prefabNameFormat = "Nature_{0}"; // Use {0} for index
     
        [Header("Collider Settings")]
        [SerializeField] private bool adjustCollider = true;
     
        [ContextMenu("Generate Prefabs")]
        private void GeneratePrefabs()
        {
            if (spriteDatabase == null)
            {
                Debug.LogError("SpriteDatabaseSO is not assigned in the Inspector.");
                return;
            }
     
            if (prefabTemplate == null)
            {
                Debug.LogError("Prefab template is not assigned in the Inspector.");
                return;
            }
     
            for (int i = 0; i < spriteDatabase.sprites.Count; i++)
            {
                // Instantiate the prefabTemplate as a new GameObject
                GameObject instanceRoot = Instantiate(prefabTemplate);
                SpriteRenderer sr = instanceRoot.GetComponent<SpriteRenderer>();
     
                if (sr != null)
                {
                    sr.sprite = spriteDatabase.sprites[i];
     
                    // Optionally reset BoxCollider2D
                    if (adjustCollider)
                    {
                        BoxCollider2D collider = instanceRoot.GetComponent<BoxCollider2D>();
                        if (collider != null)
                        {
                            collider.size = sr.bounds.size;
                            collider.offset = sr.bounds.center - instanceRoot.transform.position;
                        }
                    }
                }
                else
                {
                    Debug.LogError("SpriteRenderer not found on the prefab instance.");
                }
     
                // Save the instance as a new original prefab
                string prefabName = string.Format(prefabNameFormat, i + 1);
                string savePath = $"{outputDirectory}/{prefabName}.prefab";
                //PrefabUtility.SaveAsPrefabAsset(instanceRoot, savePath);
                DestroyImmediate(instanceRoot); // Clean up the instantiated object
            }
        }
    }