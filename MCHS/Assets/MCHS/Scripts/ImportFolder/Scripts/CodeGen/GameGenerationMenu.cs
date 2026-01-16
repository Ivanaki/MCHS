#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using CodeGen;

public class GameGenerationMenu : EditorWindow
{
    private GameGenerationData data; // Ссылка на ScriptableObject
    private Vector2 scrollPosition; // Для хранения позиции прокрутки

    [MenuItem("Tools/Generate Entry Point (by ivanaki)")]
    public static void ShowWindow()
    {
        GetWindow<GameGenerationMenu>("Generate Entry Point (by ivanaki)");
    }

    private void OnEnable()
    {
        // Загружаем данные при открытии окна
        LoadData();
    }

    private void OnDestroy()
    {
        // Сохраняем данные при закрытии окна
        SaveData();
    }

    private void OnGUI()
    {
        if (data == null)
        {
            EditorGUILayout.LabelField("Data not loaded. Please create a GameGenerationData asset.");
            if (GUILayout.Button("Create GameGenerationData"))
            {
                CreateDataAsset();
            }
            return;
        }

        // Начинаем область прокрутки
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Поле для названия проекта
        string newProjectName = EditorGUILayout.TextField("Project Name", data.ProjectName);
        if (newProjectName != data.ProjectName)
        {
            data.SetProjectName(newProjectName);
            EditorUtility.SetDirty(data); // Помечаем данные как измененные
        }

        // Поле для имени стартовой сцены
        string newStartSceneName = EditorGUILayout.TextField("StartSceneName", data.StartSceneName);
        if (newStartSceneName != data.StartSceneName)
        {
            data.SetStartSceneName(newStartSceneName);
            EditorUtility.SetDirty(data); // Помечаем данные как измененные
        }

        // Галочка для VRMode
        bool newVRMode = EditorGUILayout.Toggle("VRMode", data.VRMode);
        if (newVRMode != data.VRMode)
        {
            data.SetVRMode(newVRMode);
            EditorUtility.SetDirty(data); // Помечаем данные как измененные
        }

        GUILayout.Space(10);

        // Заголовок для списка сцен
        GUILayout.Label("Game scenes", EditorStyles.boldLabel);

        // Отображение списка сцен
        for (int i = 0; i < data.SceneNames.Count; i++)
        {
            GUILayout.BeginHorizontal();
            // Нумерация сцен: 1:, 2:, и т.д. с уменьшенным отступом
            GUILayout.Label((i + 1) + ":", GUILayout.Width(20)); // Уменьшаем ширину label
            string newSceneName = EditorGUILayout.TextField("", data.SceneNames[i]); // Убираем лишний отступ
            if (newSceneName != data.SceneNames[i])
            {
                data.SetSceneName(i, newSceneName);
                EditorUtility.SetDirty(data); // Помечаем данные как измененные
            }

            // Кнопка для удаления выбранной сцены
            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                data.RemoveSceneAt(i);
                EditorUtility.SetDirty(data); // Помечаем данные как измененные
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Scene"))
        {
            data.AddScene("");
            EditorUtility.SetDirty(data); // Помечаем данные как измененные
        }

        GUILayout.Space(10);

        // Кнопка для генерации точки входа
        if (GUILayout.Button("Generate Entry Point"))
        {
            GenerateEntryPoint();
        }

        // Кнопка для генерации сцен
        if (GUILayout.Button("Generate Scenes"))
        {
            GenerateScene();
        }

        if (GUILayout.Button("Create GameObjects"))
        {
            CreateGameObjects();
        }

        if (GUILayout.Button("Add layers and tags"))
        {
            AddLayersAndTags();
        }

        // Заканчиваем область прокрутки
        EditorGUILayout.EndScrollView();
    }

    private void AddLayersAndTags()
    {
        SaveData();
        LayerUtils.AddCustomLayers();
        LayerUtils.AddCustomTags();
    }
    
    private void GenerateEntryPoint()
    {
        SaveData();
        GamePlayGenerator.GenerateGame(data.StartSceneName, data.SceneNames, data.VRMode); // Передаем данные в GenerateGame
    }

    private void GenerateScene()
    {
        SaveData();
        SceneGenerator.GenerateAssets(data.ProjectName, data.StartSceneName, data.SceneNames); // Передаем данные в GenerateAssets
    }
    
    private void CreateGameObjects()
    {
        SaveData();
        SceneGenerator.CreateGameObjects(); // Передаем данные в GenerateAssets
    }

    private void LoadData()
    {
        // Ищем существующий GameGenerationData в папке проекта
        string[] guids = AssetDatabase.FindAssets("t:GameGenerationData");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            data = AssetDatabase.LoadAssetAtPath<GameGenerationData>(path);
        }
    }

    private void SaveData()
    {
        if (data != null)
        {
            EditorUtility.SetDirty(data); // Помечаем данные как измененные
            AssetDatabase.SaveAssets(); // Сохраняем изменения на диск
        }
    }

    private void CreateDataAsset()
    {
        // Создаем новый GameGenerationData
        data = ScriptableObject.CreateInstance<GameGenerationData>();

        // Сохраняем его в папке Assets
        AssetDatabase.CreateAsset(data, "Assets/GameGenerationData.asset");
        AssetDatabase.SaveAssets();

        // Загружаем данные
        LoadData();
    }
}
#endif