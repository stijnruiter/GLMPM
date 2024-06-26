﻿using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RenderCommon.BufferObject;

[DebuggerDisplay("X={X}, Y={Y}")]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Point2D
{
    public float X;
    public float Y;

    public Point2D(float x, float y)
    {
        X = x;
        Y = y;
    }

}