using BaCon;
using Game.Params;
using Game.Root;
using R3;
using UnityEngine;
using Valve.VR.InteractionSystem;
using MyUtils;

namespace Core.Root
{
    public class CoreEntryPoint : MonoBehaviour
    {
        private Subject<Unit> _restartSignalSub = new();
        private Subject<Unit> _mainMenuSignalSub = new();
        [SerializeField] private Transform _basePosition;
        [SerializeField] private Transform _baseParent;
        
        public Observable<CoreExitParams> Run(DIContainer container, CoreEnterParams enterParams)
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
            
            
            
            Debug.Log($"Core ENTRY POINN: {enterParams.DataCORE} ");
            var mainMenuEnter = new MainMenuEnterParams(Scenes.CORE, "");
            var exitParamsMainMenu = new CoreExitParams(Scenes.MAINMENU, mainMenuEnter);
            var exitMainMenuSubject = _mainMenuSignalSub.Select(_ => exitParamsMainMenu);
            
            var restartEnter = new CoreEnterParams(Scenes.CORE, "");
            var exitParamsRestart = new CoreExitParams(Scenes.CORE, restartEnter);
            var exitRestartSubject = _restartSignalSub.Select(_ => exitParamsRestart);
            
            var exitSignal = exitMainMenuSubject.Merge(exitRestartSubject);
            
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

        public void GoToMenu()
        {
            _mainMenuSignalSub.OnNext(Unit.Default);
        }

        public void Restart()
        {
            _restartSignalSub.OnNext(Unit.Default);
        }
    }
}
