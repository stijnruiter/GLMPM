namespace RenderCommon.Test.Utils;

internal class SparseMatrix<T>
{
    public T EmptyValue { get; }
    public int Columns { get; }
    public int Rows { get; }

    private Dictionary<long, T> _values = new();

    public SparseMatrix(int columns, int rows, T emptyValue)
    {
        EmptyValue = emptyValue;
        Columns = columns;
        Rows = rows;
    }

    public void SetValue(int x, int y, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Equals(EmptyValue))
        {
            _values.Remove(Index(x, y));
            return;
        }

        _values[Index(x, y)] = value;
    }
    public T GetValue(int x, int y) => _values.GetValueOrDefault(Index(x, y), EmptyValue);

    public T this[int x, int y]
    {
        get => GetValue(x, y);
        set => SetValue(x, y, value);
    }

    public T this[Range x, int y]
    {
        set => this[x, y..y] = value;
    }
    public T this[int x, Range y]
    {
        set => this[x..x, y] = value;
    }

    public T this[Range x, Range y]
    {
        set
        {
            var xRange = ClipRange(x, Columns - 1);
            var yRange = ClipRange(y, Rows - 1);
            for (var xi = xRange.lower; xi <= xRange.upper; xi++)
            {
                for (var yi = yRange.lower; yi <= yRange.upper; yi++)
                {
                    this[xi, yi] = value;
                }
            }
        }
    }

    private (int lower, int upper) ClipRange(Range x, int max, int min = 0)
    {
        if (x.Start.IsFromEnd)
        {
            if (x.End.IsFromEnd)
                return (0, max);

            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(x.End.Value, max);
            return (0, x.End.Value);
        }

        if (x.End.IsFromEnd)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(x.Start.Value, min);
            return (x.Start.Value, max);
        }

        return x.Start.Value < x.End.Value
                ? (x.Start.Value, x.End.Value)
                : (x.End.Value, x.Start.Value);
    }

    private long Index(int x, int y)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(x);
        ArgumentOutOfRangeException.ThrowIfNegative(y);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(x, Columns);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(y, Rows);
        int v = (y * Columns);
        return v + x;
    }
    private (int X, int Y) Coord(long index) => ((int)(index % Columns), (int)(index / Columns));

}