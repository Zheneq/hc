using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public static class WinUtils
{
	private static IntPtr s_unityWndHandle = WinUtils.User32.InvalidHandleValue;

	public const int GWL_STYLE = -0x10;

	public const int WS_THICKFRAME = 0x40000;

	public const int WS_CAPTION = 0xC00000;

	public const int WS_MAXIMIZE = 0x1000000;

	public const int WS_MINIMIZE = 0x20000000;

	public const int SWP_NOMOVE = 2;

	public const int SWP_NOSIZE = 1;

	public const int SWP_NOZORDER = 4;

	public const int SWP_FRAMECHANGED = 0x20;

	public const int SWP_NOSENDCHANGING = 0x400;

	private const uint CF_UNICODETEXT = 0xDU;

	public static void StoreUnityWndHandle()
	{
		WinUtils.s_unityWndHandle = WinUtils.User32.GetActiveWindow();
	}

	public static void BringApplicationToFront()
	{
		if (Options_UI.s_hwnd != WinUtils.User32.InvalidHandleValue)
		{
			uint currentThreadId = WinUtils.User32.GetCurrentThreadId();
			uint num;
			uint windowThreadProcessId = WinUtils.User32.GetWindowThreadProcessId(Options_UI.s_hwnd, out num);
			if (currentThreadId != windowThreadProcessId)
			{
				WinUtils.User32.AttachThreadInput(windowThreadProcessId, currentThreadId, true);
				WinUtils.User32.BringWindowToTop(Options_UI.s_hwnd);
				WinUtils.User32.AttachThreadInput(windowThreadProcessId, currentThreadId, false);
			}
		}
	}

	public static void FlashWindow()
	{
		WinUtils.User32.FLASHWINFO flashwinfo = default(WinUtils.User32.FLASHWINFO);
		flashwinfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(flashwinfo));
		flashwinfo.hwnd = Options_UI.s_hwnd;
		flashwinfo.dwFlags = 0xEU;
		flashwinfo.uCount = uint.MaxValue;
		flashwinfo.dwTimeout = 0U;
		WinUtils.User32.FlashWindowEx(ref flashwinfo);
	}

	public static void SetClipboardText(string text)
	{
		if (!WinUtils.User32.OpenClipboard(IntPtr.Zero))
		{
			return;
		}
		WinUtils.User32.EmptyClipboard();
		byte[] bytes = Encoding.Unicode.GetBytes(text + '\0');
		int length = bytes.GetLength(0);
		UIntPtr dwBytes = new UIntPtr(Convert.ToUInt32(length));
		IntPtr hMem = WinUtils.Kernel32.GlobalAlloc(0x2002U, dwBytes);
		IntPtr destination = WinUtils.Kernel32.GlobalLock(hMem);
		Marshal.Copy(bytes, 0, destination, length);
		WinUtils.Kernel32.GlobalUnlock(hMem);
		WinUtils.User32.SetClipboardData(0xDU, hMem);
		WinUtils.User32.CloseClipboard();
	}

	public static void OpenContainingFolder(string path)
	{
		path = path.Replace('/', '\\');
		Process.Start("explorer.exe", "/select," + path);
	}

	public static class User32
	{
		public const uint FLASHW_STOP = 0U;

		public const uint FLASHW_CAPTION = 1U;

		public const uint FLASHW_TRAY = 2U;

		public const uint FLASHW_ALL = 3U;

		public const uint FLASHW_TIMER = 4U;

		public const uint FLASHW_TIMERNOFG = 0xCU;

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
		public static extern bool GetWindowRect(IntPtr hWnd, out WinUtils.User32.RECT rect);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetClientRect(IntPtr hWnd, out WinUtils.User32.RECT rect);

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
		public static extern bool FlashWindowEx(ref WinUtils.User32.FLASHWINFO pwfi);

		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out WinUtils.User32.POINT lpPoint);

		[DllImport("user32.dll")]
		public static extern bool SetCursorPos(int xPos, int yPos);

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
	}

	public static class Kernel32
	{
		public const uint GMEM_DDESHARE = 0x2000U;

		public const uint GMEM_MOVEABLE = 2U;

		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalLock(IntPtr hMem);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalUnlock(IntPtr hMem);
	}
}
