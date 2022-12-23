using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Global Data", menuName = "Starblaze/Global Data")]
public class GlobalData : ScriptableObject
{
    [FoldoutGroup("Params")] public int targetFrameRate = 60;
}
