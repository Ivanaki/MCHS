
namespace Game.Params
{
    public class CoreEnterParams : BaseEnterParams
    {
        public string DataCORE { get; }
        public bool IsGameStart { get; }
        
        public CoreEnterParams(string lastScene, string dataCORE, bool isGameStart) : base(lastScene)
        {
            DataCORE = dataCORE;
            IsGameStart = isGameStart;
        }
    }
}
