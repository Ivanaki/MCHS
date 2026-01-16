
namespace Game.Params
{
    public class CoreExitParams
    {
        public string SceneName { get;}
        public BaseEnterParams BaseEnterParams { get;}

        public CoreExitParams(string sceneName, BaseEnterParams baseEnterParams)
        {
            BaseEnterParams = baseEnterParams;
            SceneName = sceneName;
        }
    }
}
