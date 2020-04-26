using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public static class WinUtils
{
	public static class User32
	{
		public struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		public struct FLASHWINFO
		{
			public uint cbSize;

			public IntPtr hwnd;

			public uint dwFlags;

			public uint uCount;

			public uint dwTimeout;
		}

		public struct POINT
		{
			public int X;

			public int Y;
		}

		public const uint FLASHW_STOP = 0u;

		public const uint FLASHW_CAPTION = 1u;

		public const uint FLASHW_TRAY = 2u;

		public const uint FLASHW_ALL = 3u;

		public const uint FLASHW_TIMER = 4u;

		public const uint FLASHW_TIMERNOFG = 12u;

		internal static readonly IntPtr InvalidHandleValue = IntPtr.Zero;

		internal const int SW_SHOW = 5;

		[DllImport("User32.dll")]
		internal static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll")]
		internal static extern IntPtr GetActiveWindow();

		[DllImport("user32.dll")]
		internal static extern bool BringWindowToTop(IntPtr hWnd);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("kernel32.dll")]
		internal static extern uint GetCurrentThreadId();

		[DllImport("user32.dll")]
		internal static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

		[DllImport("user32.dll")]
		internal static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		[DllImport("user32.dll")]
		internal static extern long GetWindowLong(IntPtr hWnd, int index);

		[DllImport("user32.dll")]
		internal static extern long SetWindowLong(IntPtr hWnd, int index, long newLong);

		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		internal static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetClientRect(IntPtr hWnd, out RECT rect);

		[DllImport("user32.dll")]
		public static extern bool OpenClipboard(IntPtr hWndNewOwner);

		[DllImport("user32.dll")]
		public static extern bool EmptyClipboard();

		[DllImport("user32.dll")]
		public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

		[DllImport("user32.dll")]
		public static extern bool CloseClipboard();

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);

		[DllImport("user32.dll")]
		public static extern bool SetCursorPos(int xPos, int yPos);
	}

	public static class Kernel32
	{
		public const uint GMEM_DDESHARE = 8192u;

		public const uint GMEM_MOVEABLE = 2u;

		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalLock(IntPtr hMem);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalUnlock(IntPtr hMem);
	}

	private static IntPtr s_unityWndHandle = User32.InvalidHandleValue;

	public const int GWL_STYLE = -16;

	public const int WS_THICKFRAME = 262144;

	public const int WS_CAPTION = 12582912;

	public const int WS_MAXIMIZE = 16777216;

	public const int WS_MINIMIZE = 536870912;

	public const int SWP_NOMOVE = 2;

	public const int SWP_NOSIZE = 1;

	public const int SWP_NOZORDER = 4;

	public const int SWP_FRAMECHANGED = 32;

	public const int SWP_NOSENDCHANGING = 1024;

	private const uint CF_UNICODETEXT = 13u;

	public static void StoreUnityWndHandle()
	{
		s_unityWndHandle = User32.GetActiveWindow();
	}

	public static void BringApplicationToFront()
	{
		if (Options_UI.s_hwnd != User32.InvalidHandleValue)
		{
			uint currentThreadId = User32.GetCurrentThreadId();
			uint lpdwProcessId;
			uint windowThreadProcessId = User32.GetWindowThreadProcessId(Options_UI.s_hwnd, out lpdwProcessId);
			if (currentThreadId != windowThreadProcessId)
			{
				User32.AttachThreadInput(windowThreadProcessId, currentThreadId, true);
				User32.BringWindowToTop(Options_UI.s_hwnd);
				User32.AttachThreadInput(windowThreadProcessId, currentThreadId, false);
			}
		}
	}

	public static void FlashWindow()
	{
		User32.FLASHWINFO pwfi = default(User32.FLASHWINFO);
		pwfi.cbSize = Convert.ToUInt32(Marshal.SizeOf(pwfi));
		pwfi.hwnd = Options_UI.s_hwnd;
		pwfi.dwFlags = 14u;
		pwfi.uCount = uint.MaxValue;
		pwfi.dwTimeout = 0u;
		User32.FlashWindowEx(ref pwfi);
	}

	public static void SetClipboardText(string text)
	{
		if (User32.OpenClipboard(IntPtr.Zero))
		{
			User32.EmptyClipboard();
			byte[] bytes = Encoding.Unicode.GetBytes(text + '\0');
			int length = bytes.GetLength(0);
			UIntPtr dwBytes = new UIntPtr(Convert.ToUInt32(length));
			IntPtr hMem = Kernel32.GlobalAlloc(8194u, dwBytes);
			IntPtr destination = Kernel32.GlobalLock(hMem);
			Marshal.Copy(bytes, 0, destination, length);
			Kernel32.GlobalUnlock(hMem);
			User32.SetClipboardData(13u, hMem);
			User32.CloseClipboard();
		}
	}

	public static void OpenContainingFolder(string path)
	{
		path = path.Replace('/', '\\');
		Process.Start("explorer.exe", "/select," + path);
	}
}
