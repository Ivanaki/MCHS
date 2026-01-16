using System.Collections;
using BaCon;
using Game.Params;
using R3;
using SaveLaod;
using UnityEngine;
using UnityEngine.SceneManagement;
using MainMenu.Root;
using Core.Root;

namespace Game.Root
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;
        private Coroutines _coroutines;
        private readonly DIContainer _rootContainer = new DIContainer();
        private DIContainer _cachedSceneContainer;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            _instance = new GameEntryPoint();
            _instance.RunGame();
        }

        private GameEntryPoint()
        {
            _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines.gameObject);
            
            var save = new JsonToFileLoadSaveService();
            _rootContainer.RegisterInstance<ILoadSaveService>(save);

            //_rootContainer.RegisterFactory(_ => new Account(save)).AsSingle();
            //_rootContainer.RegisterFactory(_ => new CursorLocker()).AsSingle();
        }

        private void RunGame()
        {
            string nameScene = Scenes.MAINMENU;
#if UNITY_EDITOR
            nameScene = SceneManager.GetActiveScene().name;

            if ( SceneManager.GetActiveScene().name != Scenes.MAINMENU && SceneManager.GetActiveScene().name != Scenes.CORE)
            {
                return;
            }
#endif
            _coroutines.StartCoroutine(InitSteamVR(nameScene));
        }

        private bool flag = true;
        private IEnumerator InitSteamVR(string SceneName)
        {
            if (flag)
            {
                yield return LoadScene(Scenes.STARTSTEAMVR);
                flag = false;
            }

            _coroutines.StartCoroutine(ScenesLoader(SceneName));
        }

        private IEnumerator ScenesLoader(string SceneName, BaseEnterParams baseEnterParams = null)
        {
            yield return null;
            switch (SceneName)
            {
                case Scenes.MAINMENU:
                    baseEnterParams ??= new MainMenuEnterParams("", "");
                    _coroutines.StartCoroutine(LoadAndStartMainMenu(baseEnterParams?.As<MainMenuEnterParams>()));
                    break;

                case Scenes.CORE: 
                    baseEnterParams ??= new CoreEnterParams("", "");
                    _coroutines.StartCoroutine(LoadAndStartCore(baseEnterParams?.As<CoreEnterParams>()));
                    break;

                default:
                    Debug.LogError("Unknown scene name");
                    break;
            }
        }
        
        private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams mainMenuEnterParams = null)
        {
            yield return LoadScene(Scenes.MAINMENU);

            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            var sceneContainer = _cachedSceneContainer = new DIContainer(_rootContainer);
            sceneEntryPoint.Run(sceneContainer, mainMenuEnterParams).Subscribe(mainMenuExitParams =>
            {
                _coroutines.StartCoroutine(ScenesLoader(mainMenuExitParams.SceneName, mainMenuExitParams.BaseEnterParams));
            });
        }
        
        
        private IEnumerator LoadAndStartCore(CoreEnterParams coreEnterParams)
        {
            yield return LoadScene(Scenes.CORE);
            
            var sceneEntryPoint = Object.FindFirstObjectByType<CoreEntryPoint>();
            var sceneContainer = _cachedSceneContainer = new DIContainer(_rootContainer); 
            sceneEntryPoint.Run(sceneContainer, coreEnterParams).Subscribe(coreExitParams =>
            {   
                _coroutines.StartCoroutine(ScenesLoader(coreExitParams.SceneName, coreExitParams.BaseEnterParams));
            });
        }

        
        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
    
    public class Coroutines : MonoBehaviour { }
}