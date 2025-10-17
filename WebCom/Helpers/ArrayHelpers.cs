using System;
using System.Collections.Generic;
using System.Text;

namespace WebCom.Helpers;

public class ArrayHelpers
{
    public static D[] Convert<S, D>(IList<S> src, Func<S, D> converter) => Convert(src, converter, 0, src.Count);

    public static D[] Convert<S, D>(IList<S> src, Func<S, D> converter, int startIndex, int count)
    {
        var dst = new D[count];

        if (count > src.Count - startIndex)
        {
            throw new ArgumentException($"The array length is different for array {nameof(src)} and {nameof(dst)}.");
        }

        for (int i = 0; i < dst.Length; i++)
        {
            dst[i] = converter(src[i + startIndex]);
        }

        return dst;
    }

    public static void Copy2D<T>(T[,] source, int[] sourceIndex, T[,] destination, int[] destinationIndex)
    {
        for (int i = sourceIndex[0]; i < source.GetLength(0); i++)
        {
            var zeroI = i - sourceIndex[0];

            for (int j = sourceIndex[1]; j < source.GetLength(1); j++)
            {
                var zeroJ = j - sourceIndex[1];

                destination[destinationIndex[0] + zeroI, destinationIndex[1] + zeroJ] = source[i, j];
            }
        }
    }
}
