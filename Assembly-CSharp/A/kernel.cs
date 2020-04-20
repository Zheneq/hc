using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace A
{
	internal class kernel
	{
		internal const uint const001D = 0x400U;

		internal const uint const000E = 0x40U;

		internal const int const0012 = 0;

		private static bool staticFlag0015;

		[DllImport("kernel32.dll", ExactSpelling = true)]
		internal static extern void SetLastError(uint symbol_001D);

		[DllImport("kernel32.dll", ExactSpelling = true)]
		internal static extern int CloseHandle(IntPtr symbol_001D);

		[DllImport("kernel32.dll", ExactSpelling = true)]
		internal static extern IntPtr OpenProcess(uint symbol_001D, int symbol_000E, uint symbol_0012);

		[DllImport("kernel32.dll", ExactSpelling = true)]
		internal static extern uint GetCurrentProcessId();

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr LoadLibrary(string symbol_001D);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern kernel.delegate000E GetProcAddress000E(IntPtr symbol_001D, string symbol_000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern kernel.delegate0012 GetProcAddress0012(IntPtr symbol_001D, string symbol_000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern kernel.delegate0013 GetProcAddress0013(IntPtr symbol_001D, string symbol_000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern kernel.delegateIntNoParam GetProcAddress0015(IntPtr symbol_001D, string symbol_000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern kernel.delegateVoidParamString GetProcAddress0016(IntPtr symbol_001D, string symbol_000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern kernel.delegate0009 GetProcAddress0009(IntPtr symbol_001D, string symbol_000E);

		private static int symbol_0016(IntPtr symbol_001D, IntPtr symbol_000E)
		{
			string[] array = new string[]
			{
				"OLLYDBG"
			};
			string strA = kernel.symbol_0013(symbol_001D);
			foreach (string strB in array)
			{
				if (string.Compare(strA, strB, true) == 0)
				{
					kernel.staticFlag0015 = true;
					return 0;
				}
			}
			return 1;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetClassName")]
		internal static extern int symbol_0016(IntPtr symbol_001D, StringBuilder symbol_000E, int symbol_0012);

		internal static string symbol_0013(IntPtr symbol_001D)
		{
			StringBuilder stringBuilder = new StringBuilder(0x104);
			kernel.symbol_0016(symbol_001D, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		internal static void method0016()
		{
			if (kernel.symbol_0016())
			{
				string arg = "Debugger";
				throw new Exception(string.Format("{0} was found - this software cannot be executed under the {0}.", arg));
			}
		}

		internal static bool symbol_0016()
		{
			try
			{
				if (Debugger.IsAttached)
				{
					return true;
				}
				IntPtr u001D = kernel.LoadLibrary("kernel32.dll");
				kernel.delegateIntNoParam procAddress = kernel.GetProcAddress0015(u001D, "IsDebuggerPresent");
				if (procAddress != null && procAddress() != 0)
				{
					return true;
				}
				uint currentProcessId = kernel.GetCurrentProcessId();
				IntPtr intPtr = kernel.OpenProcess(0x400U, 0, currentProcessId);
				if (intPtr != IntPtr.Zero)
				{
					try
					{
						kernel.delegate0013 procAddress2 = kernel.GetProcAddress0013(u001D, "CheckRemoteDebuggerPresent");
						if (procAddress2 != null)
						{
							int num = 0;
							if (procAddress2(intPtr, ref num) != 0)
							{
								if (num != 0)
								{
									return true;
								}
							}
						}
					}
					finally
					{
						kernel.CloseHandle(intPtr);
					}
				}
				bool flag = false;
				try
				{
					kernel.CloseHandle(new IntPtr(0x12345678));
				}
				catch
				{
					flag = true;
				}
				if (flag)
				{
					return true;
				}
				try
				{
					IntPtr u001D2 = kernel.LoadLibrary("user32.dll");
					kernel.delegate0009 procAddress3 = kernel.GetProcAddress0009(u001D2, "EnumWindows");
					if (procAddress3 != null)
					{
						kernel.staticFlag0015 = false;
						procAddress3(new kernel.delegate0018(kernel.symbol_0016), IntPtr.Zero);
						if (kernel.staticFlag0015)
						{
							return true;
						}
					}
				}
				catch
				{
				}
			}
			catch
			{
			}
			return false;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal class type001D
		{
			internal IntPtr symbol_001D;

			internal IntPtr symbol_000E;

			internal IntPtr symbol_0012;

			internal IntPtr symbol_0015;

			internal IntPtr symbol_0016;

			internal IntPtr symbol_0013;
		}

		internal delegate int delegate000E(IntPtr ProcessHandle, int ProcessInformationClass, kernel.type001D ProcessInformation, uint ProcessInformationLength, out uint ReturnLength);

		internal delegate int delegate0012(IntPtr ProcessHandle, int ProcessInformationClass, out uint debugPort, uint ProcessInformationLength, out uint ReturnLength);

		internal delegate int delegateIntNoParam();

		internal delegate void delegateVoidParamString([MarshalAs(UnmanagedType.LPStr)] string lpOutputString);

		internal delegate int delegate0013(IntPtr hProcess, ref int pbDebuggerPresent);

		internal delegate int delegate0018(IntPtr wnd, IntPtr lParam);

		internal delegate int delegate0009(kernel.delegate0018 lpEnumFunc, IntPtr lParam);
	}
}
