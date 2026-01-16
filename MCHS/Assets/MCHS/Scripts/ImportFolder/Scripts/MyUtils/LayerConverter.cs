using UnityEngine;

namespace MyUtils
{
    public static class LayerConverter
    {
        public static int Convert(LayerMask layerMask)
        {
            return (int)Mathf.Log(layerMask.value, 2);
        }

        public static bool CheckLayerMask(LayerMask layerMask, int layerToCheck)
        {
            return (layerMask.value & (1 << layerToCheck)) != 0;
        }
    }
}