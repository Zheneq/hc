using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace A
{
	internal class \u0019
	{
		internal const uint \u001D = 0x400U;

		internal const uint \u000E = 0x40U;

		internal const int \u0012 = 0;

		private static bool \u0015;

		[DllImport("kernel32.dll", EntryPoint = "SetLastError", ExactSpelling = true)]
		internal static extern void \u0016(uint \u001D);

		[DllImport("kernel32.dll", EntryPoint = "CloseHandle", ExactSpelling = true)]
		internal static extern int \u0016(IntPtr \u001D);

		[DllImport("kernel32.dll", EntryPoint = "OpenProcess", ExactSpelling = true)]
		internal static extern IntPtr \u0013(uint \u001D, int \u000E, uint \u0012);

		[DllImport("kernel32.dll", EntryPoint = "GetCurrentProcessId", ExactSpelling = true)]
		internal static extern uint \u0016();

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "LoadLibrary", SetLastError = true)]
		internal static extern IntPtr \u0016(string \u001D);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern \u0019.\u000E \u0016(IntPtr \u001D, string \u000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern \u0019.\u0012 \u0016(IntPtr \u001D, string \u000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern \u0019.\u0013 \u0016(IntPtr \u001D, string \u000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern \u0019.\u0015 \u0016(IntPtr \u001D, string \u000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern \u0019.\u0016 \u0016(IntPtr \u001D, string \u000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern \u0019.\u0009 \u0016(IntPtr \u001D, string \u000E);

		private static int \u0016(IntPtr \u001D, IntPtr \u000E)
		{
			string[] array = new string[]
			{
				"OLLYDBG"
			};
			string strA = \u0019.\u0013(\u001D);
			foreach (string strB in array)
			{
				if (string.Compare(strA, strB, true) == 0)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(\u0019.\u0016(IntPtr, IntPtr)).MethodHandle;
					}
					\u0019.\u0015 = true;
					return 0;
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			return 1;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetClassName")]
		internal static extern int \u0016(IntPtr \u001D, StringBuilder \u000E, int \u0012);

		internal static string \u0013(IntPtr \u001D)
		{
			StringBuilder stringBuilder = new StringBuilder(0x104);
			\u0019.\u0016(\u001D, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		internal static void \u0016()
		{
			if (\u0019.\u0016())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(\u0019.\u0016()).MethodHandle;
				}
				string arg = "Debugger";
				throw new Exception(string.Format("{0} was found - this software cannot be executed under the {0}.", arg));
			}
		}

		internal static bool \u0016()
		{
			try
			{
				if (Debugger.IsAttached)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(\u0019.\u0016()).MethodHandle;
					}
					return true;
				}
				IntPtr u001D = \u0019.\u0016("kernel32.dll");
				\u0019.\u0015 u = \u0019.\u0016(u001D, "IsDebuggerPresent");
				if (u != null && u() != 0)
				{
					return true;
				}
				uint u2 = \u0019.\u0016();
				IntPtr intPtr = \u0019.\u0013(0x400U, 0, u2);
				if (intPtr != IntPtr.Zero)
				{
					try
					{
						\u0019.\u0013 u3 = \u0019.\u0016(u001D, "CheckRemoteDebuggerPresent");
						if (u3 != null)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							int num = 0;
							if (u3(intPtr, ref num) != 0)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (num != 0)
								{
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									return true;
								}
							}
						}
					}
					finally
					{
						\u0019.\u0016(intPtr);
					}
				}
				bool flag = false;
				try
				{
					\u0019.\u0016(new IntPtr(0x12345678));
				}
				catch
				{
					flag = true;
				}
				if (flag)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					return true;
				}
				try
				{
					IntPtr u001D2 = \u0019.\u0016("user32.dll");
					\u0019.\u0009 u4 = \u0019.\u0016(u001D2, "EnumWindows");
					if (u4 != null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						\u0019.\u0015 = false;
						u4(new \u0019.\u0018(\u0019.\u0016), IntPtr.Zero);
						if (\u0019.\u0015)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
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
		internal class \u001D
		{
			internal IntPtr \u001D;

			internal IntPtr \u000E;

			internal IntPtr \u0012;

			internal IntPtr \u0015;

			internal IntPtr \u0016;

			internal IntPtr \u0013;
		}

		internal delegate int \u000E(IntPtr ProcessHandle, int ProcessInformationClass, \u0019.\u001D ProcessInformation, uint ProcessInformationLength, out uint ReturnLength);

		internal delegate int \u0012(IntPtr ProcessHandle, int ProcessInformationClass, out uint debugPort, uint ProcessInformationLength, out uint ReturnLength);

		internal delegate int \u0015();

		internal delegate void \u0016([MarshalAs(UnmanagedType.LPStr)] string lpOutputString);

		internal delegate int \u0013(IntPtr hProcess, ref int pbDebuggerPresent);

		internal delegate int \u0018(IntPtr wnd, IntPtr lParam);

		internal delegate int \u0009(\u0019.\u0018 lpEnumFunc, IntPtr lParam);
	}
}
