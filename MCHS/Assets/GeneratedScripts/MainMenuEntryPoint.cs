using BaCon;
using Game.Params;
using Game.Root;
using R3;
using UnityEngine;
using Valve.VR.InteractionSystem;
using MyUtils;

namespace MainMenu.Root
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        private Subject<Unit> _restartSignalSub = new();
        private Subject<Unit> _exitSignalSubCore = new();
        //private Subject<Unit> _exitSignalSubCarDriver = new();
        
        [SerializeField] private Transform _basePosition;
        [SerializeField] private Transform _baseParent;
        
        public Observable<MainMenuExitParams> Run(DIContainer container, MainMenuEnterParams enterParams)
        {
            if (Player.instance != null)
            {
                var player = Player.instance;
                var playerCamera = player.hmdTransforms[0].GetComponent<Camera>();
                player.transform.position = _basePosition.position;
                player.transform.SetParent(_baseParent);
                player.transform.localRotation = Quaternion.identity;
                
                
                
                
                
            }
            else
            {
                var eventSystem = new EventSystemCreator();
            }
            
            

            Debug.Log($"MainMenu ENTRY POINT: save file name = , level to load = ");
            
            
            var coreEnterParams = new CoreEnterParams(Scenes.MAINMENU,"car4");
            var coreMainMenuExitParams = new MainMenuExitParams(Scenes.CORE,coreEnterParams);
            var coreExitSignalSubject = _exitSignalSubCore.Select(_ => coreMainMenuExitParams);

            //var carDriverEnterParams = new CarDriverEnterParams(Scenes.MAINMENU,"car4");
            //var carDriverMainMenuExitParams = new MainMenuExitParams(Scenes.CARDRIVER,carDriverEnterParams);
            //var carDriverExitSignalSubject = _exitSignalSubCarDriver.Select(_ => carDriverMainMenuExitParams);
            
            var restartEnter = new MainMenuEnterParams(Scenes.MAINMENU, "");
            var exitParamsRestart = new MainMenuExitParams(Scenes.MAINMENU, restartEnter);
            var exitRestartSubject = _restartSignalSub.Select(_ => exitParamsRestart);
            
            var exitSignal = exitRestartSubject.Merge(coreExitSignalSubject);
            
            exitSignal.Subscribe(_ =>
            {
                if (Player.instance != null)
                {
                    Player.instance.transform.SetParent(null);
                    Player.instance.transform.rotation = Quaternion.identity;
                    DontDestroyOnLoad(Player.instance.gameObject);
                }
            });
            
            return exitSignal;
        }

        public void Exit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        
        public void GoToCore()
        {
            _exitSignalSubCore.OnNext(Unit.Default);
        }

        //public void GoToCarDriver()
        //{
        //    _exitSignalSubCarDriver.OnNext(Unit.Default);
        //}
        
        public void Restart()
        {
            _restartSignalSub.OnNext(Unit.Default);
        }
    }
}
