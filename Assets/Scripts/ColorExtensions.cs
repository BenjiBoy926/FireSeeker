using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorExtensions
{
    #region Public Methods
    public static Color ComponentMax(Color a, Color b)
    {
        Color result = new Color();

        for (int i = 0; i < 4; i++)
        {
            result[i] = Mathf.Max(a[i], b[i]);
        }

        return result;
    }
    #endregion
}
