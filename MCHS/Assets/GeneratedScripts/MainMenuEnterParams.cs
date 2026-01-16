
namespace Game.Params
{
    public class MainMenuEnterParams : BaseEnterParams
    {
        public string DataMAINMENU { get; }
        
        public MainMenuEnterParams(string lastScene, string dataMAINMENU) : base(lastScene)
        {
            DataMAINMENU = dataMAINMENU;
        }
    }
}
