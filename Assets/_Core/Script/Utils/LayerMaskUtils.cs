using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskUtils
{
    public static bool Includes(this LayerMask layerMask, int layer)
    {
        return (layerMask.value & 1 << layer) > 0;
    }
}
