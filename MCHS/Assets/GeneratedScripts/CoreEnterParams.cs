
namespace Game.Params
{
    public class CoreEnterParams : BaseEnterParams
    {
        public string DataCORE { get; }
        
        public CoreEnterParams(string lastScene, string dataCORE) : base(lastScene)
        {
            DataCORE = dataCORE;
        }
    }
}
