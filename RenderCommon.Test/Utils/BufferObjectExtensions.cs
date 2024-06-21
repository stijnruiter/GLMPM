using System.Runtime.InteropServices;

namespace RenderCommon.Test.Utils;

public static class BufferObjectExtensions
{
    public static unsafe byte[] AsByteArray<T>(this T item) where T : struct
    {
        var size = Marshal.SizeOf(item);
        var array = new byte[size];
#pragma warning disable CS8500
        var itemPtr = &item;
#pragma warning restore CS8500
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