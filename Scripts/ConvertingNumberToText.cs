using System;
using System.Collections.Generic;

[Serializable]
public static class ConvertingNumberToText
{
    private static List<(int, string)> _reductionNumbers = new List<(int, string)>
    {
        (1000000, "M"),
        (1000, "K")
    };
    
    public static string Conversion(float number)
    {
        for (int i = 0; i < _reductionNumbers.Count; i++)
        {
            if (number >= _reductionNumbers[i].Item1)
            {
                var format = number % _reductionNumbers[i].Item1 > 99 ? "0" : "0.0";
                return (number / _reductionNumbers[i].Item1).ToString(format) + _reductionNumbers[i].Item2;
            }
        }
        
        return number.ToString();
    }
}
