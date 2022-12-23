using UnityEngine;
using System.Collections;
using MoreMountains.NiceVibrations;

public class HapticManager : MonoBehaviour
{
    public static HapticManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (Instantiate(Resources.Load("SYS Haptic Manager")) as GameObject).GetComponent<HapticManager>();
                DontDestroyOnLoad(instance.gameObject);
            }

            //用于强制在 Android 上添加震动权限
            if ( false ) Handheld.Vibrate();

            return instance;
        }
    }
    private static HapticManager instance = null;

    public void Trigger(HapticTypes type)
    {
        if (enabled == false) return;

        MMVibrationManager.Haptic(type);
    }
}
