using System;
using System.Runtime.Loader;
using System.Runtime.InteropServices;

public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public IntPtr LoadUnmanagedLibrary(string absolutePath)
    {
        return LoadUnmanagedDll(absolutePath);
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        return LoadUnmanagedDllFromPath(unmanagedDllName);
    }

    // protected override Assembly Load(AssemblyName assemblyName)
    // {
    //     return null;
    // }
}
