using System.Runtime.InteropServices;

namespace RenderCommon.Test;

public static class BufferObjectExtensions
{
    public static unsafe byte[] AsByteArray<T>(this T item) where T : struct
    {
        var size = Marshal.SizeOf(item);
        var array = new byte[size];
        var itemPtr = &item;
        var itemBytes = (byte*)itemPtr;

        for (var i = 0; i < size; ++i)
        {
            array[i] = itemBytes[i];
        }

        return array;
    }

    public static int SizeOf<T>(this T item) where T : struct
    {
        return Marshal.SizeOf(item);
    }
}