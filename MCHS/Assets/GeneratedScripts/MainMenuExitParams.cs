
namespace Game.Params
{
    public class MainMenuExitParams
    {
        public string SceneName { get;}
        public BaseEnterParams BaseEnterParams { get;}

        public MainMenuExitParams(string sceneName, BaseEnterParams baseEnterParams)
        {
            BaseEnterParams = baseEnterParams;
            SceneName = sceneName;
        }
    }
}
