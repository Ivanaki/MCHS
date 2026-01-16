#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CodeGen
{
    public static class LayerUtils
    {
        public static void AddCustomTags()
        {
            string[] newTags = { "HeadCollider", "Finger"}; // Ваши теги
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tags = tagManager.FindProperty("tags");

            // Проверяем, какие теги уже существуют
            bool hasChanges = false;
            foreach (string newTag in newTags)
            {
                bool tagExists = false;
            
                // Проверяем, есть ли тег в списке
                for (int i = 0; i < tags.arraySize; i++)
                {
                    SerializedProperty tag = tags.GetArrayElementAtIndex(i);
                    if (tag.stringValue == newTag)
                    {
                        tagExists = true;
                        break;
                    }
                }

                // Если тега нет — добавляем
                if (!tagExists)
                {
                    tags.InsertArrayElementAtIndex(tags.arraySize);
                    SerializedProperty newTagProperty = tags.GetArrayElementAtIndex(tags.arraySize - 1);
                    newTagProperty.stringValue = newTag;
                    Debug.Log($"Added tag: '{newTag}'");
                    hasChanges = true;
                }
                else
                {
                    Debug.LogWarning($"Tag '{newTag}' already exists!");
                }
            }

            if (hasChanges)
            {
                tagManager.ApplyModifiedProperties();
                AssetDatabase.SaveAssets(); // Сохраняем изменения
            }
        }
        
        public static void AddCustomLayers()
        {
            string[] newLayers =
            {
                "Player", "NotRenderSteamVRCamera", "HeadCollider", "HolderLaserLayer", "PlayerCamera", "LaserActive"
            };
            int startIndex = 10; // Начинаем с 10-го слоя (User Layer 8 в интерфейсе Unity)

            SerializedObject tagManager =
                new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");

            // Добавляем слои, начиная с указанной позиции
            for (int i = 0; i < newLayers.Length; i++)
            {
                int targetIndex = startIndex + i;

                // Проверяем, чтобы не выйти за пределы массива
                if (targetIndex >= layers.arraySize)
                {
                    Debug.LogWarning($"Cannot add layer '{newLayers[i]}' at index {targetIndex}: out of bounds!");
                    continue;
                }

                SerializedProperty layer = layers.GetArrayElementAtIndex(targetIndex);

                // Если слот пустой, добавляем слой
                if (string.IsNullOrEmpty(layer.stringValue))
                {
                    layer.stringValue = newLayers[i];
                    Debug.Log($"Added layer: '{newLayers[i]}' at index {targetIndex}");
                }
                else
                {
                    Debug.LogWarning($"Layer slot {targetIndex} is already occupied by '{layer.stringValue}'");
                }
            }

            tagManager.ApplyModifiedProperties();
        }
    }
}
#endif