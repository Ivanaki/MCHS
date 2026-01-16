#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SceneGenerator
{
    public static void CreateGameObjects()
    {
        GameObject lightObject = new GameObject("Directional Light");
        // Добавляем компонент Light и задаем его тип как Directional
        Light lightComp = lightObject.AddComponent<Light>();
        lightComp.type = LightType.Directional;
        // Задаем вращение, например, поворачиваем на 50 градусов по оси X
        lightObject.transform.rotation = Quaternion.Euler(50, 30, 0);

        // Создаем GameObject с именем EntryPoint
        GameObject entryPoint = new GameObject("EntryPoint");

        // Создаем дочерний объект "position"
        GameObject positionChild = new GameObject("Position");
        // Делаем его дочерним к EntryPoint
        positionChild.transform.parent = entryPoint.transform;

        // Создаем дочерний объект "Base parent"
        GameObject baseParentChild = new GameObject("BaseParent");
        // Делаем его дочерним к EntryPoint
        baseParentChild.transform.parent = entryPoint.transform;
    }
    
    public static void GenerateAssets(string projectName, string startSceneName, IReadOnlyList<string> sceneNames)
    {
        // Проверяем, существует ли папка проекта
        string projectPath = Path.Combine(Application.dataPath, projectName);
        if (!Directory.Exists(projectPath))
        {
            Directory.CreateDirectory(projectPath);
        }

        // Проверяем, существует ли папка Scenes
        string scenesPath = Path.Combine(projectPath, "Scenes");
        if (!Directory.Exists(scenesPath))
        {
            Directory.CreateDirectory(scenesPath);
        }

        // Создаем стартовую сцену
        CreateScene(Path.Combine(scenesPath, startSceneName + ".unity"));
        CreateScene(Path.Combine(scenesPath, "StartSteamVR" + ".unity"));

        // Создаем остальные сцены
        foreach (var sceneName in sceneNames)
        {
            CreateScene(Path.Combine(scenesPath, sceneName + ".unity"));
        }

        // Обновляем ассеты в Unity
        AssetDatabase.Refresh();
    }

    private static void CreateScene(string scenePath)
    {
        // Создаем новую сцену
        var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Сохраняем сцену по указанному пути
        EditorSceneManager.SaveScene(newScene, scenePath);
    }
}

#endif