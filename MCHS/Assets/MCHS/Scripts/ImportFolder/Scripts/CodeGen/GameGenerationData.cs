#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameGenerationData", menuName = "Game Generation Data", order = 51)]
public class GameGenerationData : ScriptableObject
{
    [SerializeField] private string projectName = "NewProject"; // Новое поле для названия проекта
    [SerializeField] private string startSceneName = "MainMenu";
    [SerializeField] private List<string> sceneNames = new List<string>();
    [SerializeField] private bool vrMode = true;

    // Методы для доступа к данным
    public string ProjectName => projectName;
    public string StartSceneName => startSceneName;
    public List<string> SceneNames => sceneNames;
    public bool VRMode => vrMode;

    // Методы для изменения данных (доступны только внутри GameGenerationMenu)
    public void SetProjectName(string value) => projectName = value;
    public void SetStartSceneName(string value) => startSceneName = value;
    public void SetVRMode(bool value) => vrMode = value;
    public void AddScene(string sceneName) => sceneNames.Add(sceneName);
    public void RemoveSceneAt(int index) => sceneNames.RemoveAt(index);
    public void SetSceneName(int index, string value) => sceneNames[index] = value;
}
#endif