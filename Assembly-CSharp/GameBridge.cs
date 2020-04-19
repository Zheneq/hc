using System;
using System.Runtime.InteropServices;

internal class GameBridge
{
	public static readonly uint ERROR_FATAL = 0x65U;

	public static readonly uint ERROR_UPDATE_FAILED = 0x66U;

	public static readonly uint ERROR_ALREADY_INITIALIZED = 0x67U;

	public static readonly uint DISCORD_STDOUT = 1U;

	public static readonly uint DISCORD_STDERR = 2U;

	[DllImport("discordsdk", EntryPoint = "Discord_Initialize")]
	public static extern void Initialize(string client, string resourcePath);

	[DllImport("discordsdk", EntryPoint = "Discord_Shutdown")]
	public static extern void Shutdown();

	[DllImport("discordsdk", EntryPoint = "Discord_SetReadyCallback")]
	public static extern void SetReadyCallback(GameBridge.ReadyCallback onReady, UIntPtr context);

	[DllImport("discordsdk", EntryPoint = "Discord_SetUpdatingCallback")]
	public static extern void SetUpdatingCallback(GameBridge.UpdatingCallback onUpdating, UIntPtr context);

	[DllImport("discordsdk", EntryPoint = "Discord_SetErrorCallback")]
	public static extern void SetErrorCallback(GameBridge.ErrorCallback onError, UIntPtr context);

	[DllImport("discordsdk", EntryPoint = "Discord_CaptureOutput")]
	public static extern void CaptureOutput(GameBridge.OutputCallback onOutput, UIntPtr context);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ReadyCallback(ushort port, UIntPtr context);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void UpdatingCallback(uint progress, UIntPtr context);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ErrorCallback(uint code, [MarshalAs(UnmanagedType.LPStr)] string message, UIntPtr context);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OutputCallback(uint type, [MarshalAs(UnmanagedType.LPStr)] string message, UIntPtr context);
}
