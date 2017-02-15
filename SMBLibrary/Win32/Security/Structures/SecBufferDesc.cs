/* Copyright (C) 2014-2017 Tal Aloni <tal.aloni.il@gmail.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the GNU Lesser Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SMBLibrary.Win32.Security
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SecBufferDesc : IDisposable
    {
        public uint ulVersion;
        public uint cBuffers;    // Indicates the number of SecBuffer structures in the pBuffers array.
        public IntPtr pBuffers; // Pointer to an array of SecBuffer structures.

        public SecBufferDesc(SecBuffer buffer) : this(new SecBuffer[] { buffer })
        {
        }

        public SecBufferDesc(SecBuffer[] buffers)
        {
            int secBufferSize = Marshal.SizeOf(typeof(SecBuffer));
            ulVersion = (uint)SecBufferType.SECBUFFER_VERSION;
            cBuffers = (uint)buffers.Length;
            pBuffers = Marshal.AllocHGlobal(buffers.Length * secBufferSize);
            IntPtr currentBuffer = pBuffers;
            for (int index = 0; index < buffers.Length; index++)
            {
                Marshal.StructureToPtr(buffers[index], currentBuffer, false);
                currentBuffer = new IntPtr(currentBuffer.ToInt64() + secBufferSize);
            }
        }

        public void Dispose()
        {
            if (pBuffers != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pBuffers);
                pBuffers = IntPtr.Zero;
            }
        }
    }
}
