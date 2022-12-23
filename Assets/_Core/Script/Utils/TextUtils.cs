using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextUtils
{
    public static string Loc ( this string s)
    {
        return I2.Loc.LocalizationManager.GetTranslation(s);
    }
    public static string LocUpper(this string s)
    {
        return I2.Loc.LocalizationManager.GetTranslation(s).ToUpper();
    }


    public static string Parse(this long num)
    {
        float f = num;
        if (num < 1000) return num.ToString();

        //K = 1,000
        if (1000 <= num && num < 1000000)
        {
            f /= 1000;
            return string.Format("{0:0.0}K", f);
        }
        //M = 1,000,000
        if (1000000 <= num && num < 1000000000)
        {
            f /= 1000000;
            return string.Format("{0:0.0}M", f);
        }
        //B = 1,000,000,000
        if (1000000000 <= num && num < 1000000000000)
        {
            f /= 1000000000;
            return string.Format("{0:0.0}B", f);
        }
        //T = 1,000,000,000,000
        if (1000000000000 <= num && num < long.MaxValue)
        {
            f /= 1000000000000;
            return string.Format("{0:0.0}T", f);
        }


        return num.ToString();
    }

    //给数加逗号
    public static string Comma(this int num)
    {
        return string.Format("{0:N0}", num);
    }

    //秒数转时间格式
    public static string SecToTime ( int sec)
    {
        int s = sec % 60;
        int m = (sec - s) / 60;

        return string.Format("{0:D1}:{1:D2}", m, s);
    }
}
