#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using CodeGen;

public static class GamePlayGenerator
{
    public static void GenerateGame(string startSceneName, IReadOnlyList<string> sceneNames, bool VRMode)
    {
        if (Application.isPlaying)
        {
            Debug.LogWarning("Game is already running");
            return;
        }

        foreach (string sceneName in sceneNames)
        {
            if (sceneName.IndexOf(" ", StringComparison.Ordinal) != -1 || startSceneName.IndexOf(" ", StringComparison.Ordinal) != -1)
            {
                Debug.LogWarning("Scenes names have space");
                return;
            }
        }
        
        if(!VRMode) return;
        
        var basePath = $"{Application.dataPath}/GeneratedScripts/";
        var gameEntryPointPath = $"{basePath}GameEntryPoint.cs";
        var startSceneEntryPointPath = $"{basePath}{startSceneName}EntryPoint.cs";
        var scenesPath = $"{basePath}Scenes.cs";

        // Создаем директорию, если она не существует
        if (!System.IO.Directory.Exists(basePath))
        {
            System.IO.Directory.CreateDirectory(basePath);
        }

        var scenesCode = $@"namespace Game.Root
{{
    public static class Scenes
    {{
        public const string STARTSTEAMVR = ""StartSteamVR"";
        {$"public const string {startSceneName.ToUpper()} = \"{startSceneName}\";"}
        {String.Join("\n        ",sceneNames.Where(layerName => layerName != "")
            .Select(layerName => $"public const string {layerName.ToUpper()} = \"{layerName}\";"))}

        public static readonly string[] GameplayScenes = {{{String.Join(", ", sceneNames.Where(layerName => layerName != "").Select(layerName => @$"""{layerName}""") )}}};
    }}
}}";
        
        var gameEntryPointCode = $@"using System.Collections;
using BaCon;
using Game.Params;
using R3;
using SaveLaod;
using UnityEngine;
using UnityEngine.SceneManagement;
using {startSceneName}.Root;
{String.Join("\n",sceneNames.Where(sceneName => sceneName != "").Select(sceneName => $"using {sceneName}.Root;"))}

namespace Game.Root
{{
    public class GameEntryPoint
    {{
        private static GameEntryPoint _instance;
        private Coroutines _coroutines;
        private readonly DIContainer _rootContainer = new DIContainer();
        private DIContainer _cachedSceneContainer;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {{
            _instance = new GameEntryPoint();
            _instance.RunGame();
        }}

        private GameEntryPoint()
        {{
            _coroutines = new GameObject(""[COROUTINES]"").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines.gameObject);
            
            var save = new JsonToFileLoadSaveService();
            _rootContainer.RegisterInstance<ILoadSaveService>(save);

            //_rootContainer.RegisterFactory(_ => new Account(save)).AsSingle();
            //_rootContainer.RegisterFactory(_ => new CursorLocker()).AsSingle();
        }}

        private void RunGame()
        {{
            string nameScene = Scenes.{startSceneName.ToUpper()};
#if UNITY_EDITOR
            nameScene = SceneManager.GetActiveScene().name;

            if ( SceneManager.GetActiveScene().name != Scenes.{startSceneName.ToUpper()} && {String.Join(" && ",
                sceneNames.Where(sceneName => sceneName != "").Select(sceneName => $"SceneManager.GetActiveScene().name != Scenes.{sceneName.ToUpper()}"))})
            {{
                return;
            }}
#endif
            _coroutines.StartCoroutine(InitSteamVR(nameScene));
        }}

        private bool flag = true;
        private IEnumerator InitSteamVR(string SceneName)
        {{
            if (flag)
            {{
                yield return LoadScene(Scenes.STARTSTEAMVR);
                flag = false;
            }}

            _coroutines.StartCoroutine(ScenesLoader(SceneName));
        }}

        private IEnumerator ScenesLoader(string SceneName, BaseEnterParams baseEnterParams = null)
        {{
            yield return null;
            switch (SceneName)
            {{
                case Scenes.{startSceneName.ToUpper()}:
                    baseEnterParams ??= new {startSceneName}EnterParams("""", """");
                    _coroutines.StartCoroutine(LoadAndStart{startSceneName}(baseEnterParams?.As<{startSceneName}EnterParams>()));
                    break;
{String.Join("\n",sceneNames.Where(sceneName => sceneName != "").Select(sceneName => 
$@"
                case Scenes.{sceneName.ToUpper()}: 
                    baseEnterParams ??= new {sceneName}EnterParams("""", """");
                    _coroutines.StartCoroutine(LoadAndStart{sceneName}(baseEnterParams?.As<{sceneName}EnterParams>()));
                    break;
"))}
                default:
                    Debug.LogError(""Unknown scene name"");
                    break;
            }}
        }}
        
        private IEnumerator LoadAndStart{startSceneName}({startSceneName}EnterParams {startSceneName.ToLowerFirst()}EnterParams = null)
        {{
            yield return LoadScene(Scenes.{startSceneName.ToUpper()});

            var sceneEntryPoint = Object.FindFirstObjectByType<{startSceneName}EntryPoint>();
            var sceneContainer = _cachedSceneContainer = new DIContainer(_rootContainer);
            sceneEntryPoint.Run(sceneContainer, {startSceneName.ToLowerFirst()}EnterParams).Subscribe({startSceneName.ToLowerFirst()}ExitParams =>
            {{
                _coroutines.StartCoroutine(ScenesLoader({startSceneName.ToLowerFirst()}ExitParams.SceneName, {startSceneName.ToLowerFirst()}ExitParams.BaseEnterParams));
            }});
        }}
        
        {String.Join("\n \n", sceneNames.Where(sceneName => sceneName != "").Select(sceneName =>
$@"
        private IEnumerator LoadAndStart{sceneName}({sceneName}EnterParams {sceneName.ToLowerFirst()}EnterParams)
        {{
            yield return LoadScene(Scenes.{sceneName.ToUpper()});
            
            var sceneEntryPoint = Object.FindFirstObjectByType<{sceneName}EntryPoint>();
            var sceneContainer = _cachedSceneContainer = new DIContainer(_rootContainer); 
            sceneEntryPoint.Run(sceneContainer, {sceneName.ToLowerFirst()}EnterParams).Subscribe({sceneName.ToLowerFirst()}ExitParams =>
            {{   
                _coroutines.StartCoroutine(ScenesLoader({sceneName.ToLowerFirst()}ExitParams.SceneName, {sceneName.ToLowerFirst()}ExitParams.BaseEnterParams));
            }});
        }}
"))}
        
        private IEnumerator LoadScene(string sceneName)
        {{
            yield return SceneManager.LoadSceneAsync(sceneName);
        }}
    }}
    
    public class Coroutines : MonoBehaviour {{ }}
}}";

        foreach (var sceneName in sceneNames)
        {
            var pathEnterPoint = $"{basePath}{sceneName}EntryPoint.cs";
            var codeEnterPoint = 
$@"using BaCon;
using Game.Params;
using Game.Root;
using R3;
using UnityEngine;
using Valve.VR.InteractionSystem;
using MyUtils;

namespace {sceneName}.Root
{{
    public class {sceneName}EntryPoint : MonoBehaviour
    {{
        private Subject<Unit> _restartSignalSub = new();
        private Subject<Unit> _mainMenuSignalSub = new();
        [SerializeField] private Transform _basePosition;
        [SerializeField] private Transform _baseParent;
        
        public Observable<{sceneName}ExitParams> Run(DIContainer container, {sceneName}EnterParams enterParams)
        {{          
            if (Player.instance != null)
            {{
                var player = Player.instance;
                var playerCamera = player.hmdTransforms[0].GetComponent<Camera>();
                player.transform.position = _basePosition.position;
                player.transform.SetParent(_baseParent);
                player.transform.localRotation = Quaternion.identity;
                
                
                
                
                
            }}
            else
            {{
                var eventSystem = new EventSystemCreator();
            }}
            
            
            
            Debug.Log($""{sceneName} ENTRY POINN: {{enterParams.Data{sceneName.ToUpper()}}} "");
            var mainMenuEnter = new MainMenuEnterParams(Scenes.{sceneName.ToUpper()}, """");
            var exitParamsMainMenu = new {sceneName}ExitParams(Scenes.MAINMENU, mainMenuEnter);
            var exitMainMenuSubject = _mainMenuSignalSub.Select(_ => exitParamsMainMenu);
            
            var restartEnter = new {sceneName}EnterParams(Scenes.{sceneName.ToUpper()}, """");
            var exitParamsRestart = new {sceneName}ExitParams(Scenes.{sceneName.ToUpper()}, restartEnter);
            var exitRestartSubject = _restartSignalSub.Select(_ => exitParamsRestart);
            
            var exitSignal = exitMainMenuSubject.Merge(exitRestartSubject);
            
            exitSignal.Subscribe(_ =>
            {{
                if (Player.instance != null)
                {{
                    Player.instance.transform.SetParent(null);
                    Player.instance.transform.rotation = Quaternion.identity;
                    DontDestroyOnLoad(Player.instance.gameObject);
                }}
            }});
            
            return exitSignal;
        }}

        public void GoToMenu()
        {{
            _mainMenuSignalSub.OnNext(Unit.Default);
        }}

        public void Restart()
        {{
            _restartSignalSub.OnNext(Unit.Default);
        }}
    }}
}}
";
            
            var pathEnter = $"{basePath}{sceneName}EnterParams.cs";
            var codeEnter = 
$@"
namespace Game.Params
{{
    public class {sceneName}EnterParams : BaseEnterParams
    {{
        public string Data{sceneName.ToUpper()} {{ get; }}
        
        public {sceneName}EnterParams(string lastScene, string data{sceneName.ToUpper()}) : base(lastScene)
        {{
            Data{sceneName.ToUpper()} = data{sceneName.ToUpper()};
        }}
    }}
}}
";
            
            var pathExit = $"{basePath}{sceneName}ExitParams.cs";
            var codeExit = 
$@"
namespace Game.Params
{{
    public class {sceneName}ExitParams
    {{
        public string SceneName {{ get;}}
        public BaseEnterParams BaseEnterParams {{ get;}}

        public {sceneName}ExitParams(string sceneName, BaseEnterParams baseEnterParams)
        {{
            BaseEnterParams = baseEnterParams;
            SceneName = sceneName;
        }}
    }}
}}
";
            
            
            System.IO.File.WriteAllText(pathEnterPoint, codeEnterPoint);
            System.IO.File.WriteAllText(pathEnter, codeEnter);
            System.IO.File.WriteAllText(pathExit, codeExit);
        }

        var startSceneEntryPointCode = 
$@"using BaCon;
using Game.Params;
using Game.Root;
using R3;
using UnityEngine;
using Valve.VR.InteractionSystem;
using MyUtils;

namespace {startSceneName}.Root
{{
    public class {startSceneName}EntryPoint : MonoBehaviour
    {{
        private Subject<Unit> _restartSignalSub = new();
        {String.Join("\n        ", sceneNames.Where(sceneName => sceneName != "").Select(sceneName => $"private Subject<Unit> _exitSignalSub{sceneName} = new();"))}
        //private Subject<Unit> _exitSignalSubCarDriver = new();
        
        [SerializeField] private Transform _basePosition;
        [SerializeField] private Transform _baseParent;
        
        public Observable<{startSceneName}ExitParams> Run(DIContainer container, {startSceneName}EnterParams enterParams)
        {{
            if (Player.instance != null)
            {{
                var player = Player.instance;
                var playerCamera = player.hmdTransforms[0].GetComponent<Camera>();
                player.transform.position = _basePosition.position;
                player.transform.SetParent(_baseParent);
                player.transform.localRotation = Quaternion.identity;
                
                
                
                
                
            }}
            else
            {{
                var eventSystem = new EventSystemCreator();
            }}
            
            

            Debug.Log($""{startSceneName} ENTRY POINT: save file name = , level to load = "");
            
            {string.Join("\n \n", sceneNames.Where(sceneName => sceneName != "").Select(sceneName => 
@$"
            var {sceneName.ToLowerFirst()}EnterParams = new {sceneName}EnterParams(Scenes.{startSceneName.ToUpper()},""car4"");
            var {sceneName.ToLowerFirst()}{startSceneName}ExitParams = new {startSceneName}ExitParams(Scenes.{sceneName.ToUpper()},{sceneName.ToLowerFirst()}EnterParams);
            var {sceneName.ToLowerFirst()}ExitSignalSubject = _exitSignalSub{sceneName}.Select(_ => {sceneName.ToLowerFirst()}{startSceneName}ExitParams);
"))}
            //var carDriverEnterParams = new CarDriverEnterParams(Scenes.MAINMENU,""car4"");
            //var carDriverMainMenuExitParams = new MainMenuExitParams(Scenes.CARDRIVER,carDriverEnterParams);
            //var carDriverExitSignalSubject = _exitSignalSubCarDriver.Select(_ => carDriverMainMenuExitParams);
            
            var restartEnter = new {startSceneName}EnterParams(Scenes.{startSceneName.ToUpper()}, """");
            var exitParamsRestart = new {startSceneName}ExitParams(Scenes.{startSceneName.ToUpper()}, restartEnter);
            var exitRestartSubject = _restartSignalSub.Select(_ => exitParamsRestart);
            
            var exitSignal = exitRestartSubject{string.Join("", sceneNames.Where(sceneName => sceneName != "").Select(sceneName =>$".Merge({sceneName.ToLowerFirst()}ExitSignalSubject)"))};
            
            exitSignal.Subscribe(_ =>
            {{
                if (Player.instance != null)
                {{
                    Player.instance.transform.SetParent(null);
                    Player.instance.transform.rotation = Quaternion.identity;
                    DontDestroyOnLoad(Player.instance.gameObject);
                }}
            }});
            
            return exitSignal;
        }}

        public void Exit()
        {{
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }}

        {string.Join("\n \n", sceneNames.Where(sceneName => sceneName != "").Select(sceneName => 
$@"
        public void GoTo{sceneName}()
        {{
            _exitSignalSub{sceneName}.OnNext(Unit.Default);
        }}
"))}
        //public void GoToCarDriver()
        //{{
        //    _exitSignalSubCarDriver.OnNext(Unit.Default);
        //}}
        
        public void Restart()
        {{
            _restartSignalSub.OnNext(Unit.Default);
        }}
    }}
}}
";

                    
        var pathEnterStartScene = $"{basePath}{startSceneName}EnterParams.cs";
        var codeEnterStartScene = 
            $@"
namespace Game.Params
{{
    public class {startSceneName}EnterParams : BaseEnterParams
    {{
        public string Data{startSceneName.ToUpper()} {{ get; }}
        
        public {startSceneName}EnterParams(string lastScene, string data{startSceneName.ToUpper()}) : base(lastScene)
        {{
            Data{startSceneName.ToUpper()} = data{startSceneName.ToUpper()};
        }}
    }}
}}
";
            
        var pathExitStartScene = $"{basePath}{startSceneName}ExitParams.cs";
        var codeExitStartScene = 
            $@"
namespace Game.Params
{{
    public class {startSceneName}ExitParams
    {{
        public string SceneName {{ get;}}
        public BaseEnterParams BaseEnterParams {{ get;}}

        public {startSceneName}ExitParams(string sceneName, BaseEnterParams baseEnterParams)
        {{
            BaseEnterParams = baseEnterParams;
            SceneName = sceneName;
        }}
    }}
}}
";


        
        var baseEnterParamsPath = $"{basePath}BaseEnterParams.cs";
        var baseEnterParamsCode = $@"namespace Game.Params
{{
    public abstract class BaseEnterParams
    {{
        public string LastScene {{ get; }}

        public BaseEnterParams(string lastScene)
        {{
            LastScene = lastScene;
        }}
        
        public T As<T>() where T : BaseEnterParams
        {{
            return (T)this;
        }}
    }}
}}";
        var entryPointScriptsPath = $"{basePath}EntryPointsScripts.cs";
        var entryPointScriptsCode = $@"
namespace Game.Params
{{
    public static class EntryPointsScripts
    {{
        
    }}
}}";
        //System.IO.File.WriteAllText(entryPointScriptsPath, entryPointScriptsCode);
        
        
        System.IO.File.WriteAllText(scenesPath, scenesCode);
        System.IO.File.WriteAllText(gameEntryPointPath, gameEntryPointCode);
        System.IO.File.WriteAllText(startSceneEntryPointPath, startSceneEntryPointCode);
        System.IO.File.WriteAllText(baseEnterParamsPath, baseEnterParamsCode);
        System.IO.File.WriteAllText(pathEnterStartScene, codeEnterStartScene);
        System.IO.File.WriteAllText(pathExitStartScene, codeExitStartScene);
        AssetDatabase.Refresh(); // Обновляем AssetDatabase, чтобы Unity увидел новый файл
    }

    /*public static T GetScript<T>(string sceneName) where T : MonoBehaviour
    {
        switch (sceneName)
        {
            case Scenes.MAINMENU:
                // Предполагается, что MainMenuEntryPoint имеет тип T или его можно привести
                return (T)MainMenuEntryPoint;
            default:
                Debug.LogError($"Сцена {sceneName} не поддерживается.");
                return default(T);
        }
    }*/


}

#endif