using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Shuffle
{
    public static List<T> ShuffleList<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
        return (List<T>)ts;
    }
}

