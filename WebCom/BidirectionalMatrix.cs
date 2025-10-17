using System;
using WebCom.Helpers;

namespace WebCom;

public struct BidirectionalMatrix<T>
{
    private T[,] _matrix;

    private int[] _capacities = new int[2];
    private readonly int[] _offsets = new int[2];

    public BidirectionalMatrix()
    {
        _matrix = new T[0, 0];
    }

    public BidirectionalMatrix(int[] capacities)
    {
        _matrix = new T[capacities[0], capacities[1]];
        _capacities = capacities;
    }

    public void Resize(int[] capacities, bool[]? negative = null)
    {
        #region Validation
        if (capacities.Length != _capacities.Length)
        {
            throw new ArgumentException("New capacities should have the same number of dimensions as previous ones.", nameof(capacities));
        }

        if (negative != null && negative.Length != capacities.Length)
        {
            throw new ArgumentException("Negative indicators array must be of the same length as capacities array.", nameof(negative));
        }

        for (int i = 0; i < capacities.Length; i++)
        {
            if (capacities[i] < _capacities[i])
            {
                throw new ArgumentException("New capacity must be bigger than the previous one.", nameof(negative));
            }
        }
        #endregion

        // New matrix to copy elements into
        var matrix = new T[capacities[0], capacities[1]];
        var localOffsets = new int[_offsets.Length];

        // Figure out new offsets from capacities
        for (int i = 0; i < _offsets.Length; i++)
        {
            if (negative != null && negative[i])
            {
                localOffsets[i] = capacities[i] - _capacities[i];
                _offsets[i] += localOffsets[i];
            }
        }

        ArrayHelpers.Copy2D(_matrix, new[] { 0, 0 }, matrix, localOffsets);

        _matrix = matrix;
        _capacities = capacities;
    }

    public int GetLength(int dimension) => _matrix.GetLength(dimension);

    public int GetLowerBound(int dimension) => -_offsets[dimension];
    public int GetUpperBound(int dimension) => GetLength(dimension) + GetLowerBound(dimension);

    public void SetZerod(int i, int j, T value)
    {
        _matrix[i, j] = value;
    }

    public T GetZerod(int i, int j)
    {
        return _matrix[i, j];
    }

    public T this[int i, int j]
    {
        get => _matrix[i + _offsets[0], j + _offsets[1]];
        set => _matrix[i + _offsets[0], j + _offsets[1]] = value;
    }
}
