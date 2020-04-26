using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace A
{
	internal class kernel
	{
		[StructLayout(LayoutKind.Sequential)]
		internal class type001D
		{
			internal IntPtr _001D;

			internal IntPtr _000E;

			internal IntPtr _0012;

			internal IntPtr _0015;

			internal IntPtr _0016;

			internal IntPtr _0013;
		}

		internal delegate int delegate000E(IntPtr ProcessHandle, int ProcessInformationClass, type001D ProcessInformation, uint ProcessInformationLength, out uint ReturnLength);

		internal delegate int delegate0012(IntPtr ProcessHandle, int ProcessInformationClass, out uint debugPort, uint ProcessInformationLength, out uint ReturnLength);

		internal delegate int delegateIntNoParam();

		internal delegate void delegateVoidParamString([MarshalAs(UnmanagedType.LPStr)] string lpOutputString);

		internal delegate int delegate0013(IntPtr hProcess, ref int pbDebuggerPresent);

		internal delegate int delegate0018(IntPtr wnd, IntPtr lParam);

		internal delegate int delegate0009(delegate0018 lpEnumFunc, IntPtr lParam);

		internal const uint const001D = 1024u;

		internal const uint const000E = 64u;

		internal const int const0012 = 0;

		private static bool staticFlag0015;

		[DllImport("kernel32.dll", ExactSpelling = true)]
		internal static extern void SetLastError(uint _001D);

		[DllImport("kernel32.dll", ExactSpelling = true)]
		internal static extern int CloseHandle(IntPtr _001D);

		[DllImport("kernel32.dll", ExactSpelling = true)]
		internal static extern IntPtr OpenProcess(uint _001D, int _000E, uint _0012);

		[DllImport("kernel32.dll", ExactSpelling = true)]
		internal static extern uint GetCurrentProcessId();

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr LoadLibrary(string _001D);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern delegate000E GetProcAddress000E(IntPtr _001D, string _000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern delegate0012 GetProcAddress0012(IntPtr _001D, string _000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern delegate0013 GetProcAddress0013(IntPtr _001D, string _000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern delegateIntNoParam GetProcAddress0015(IntPtr _001D, string _000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern delegateVoidParamString GetProcAddress0016(IntPtr _001D, string _000E);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
		internal static extern delegate0009 GetProcAddress0009(IntPtr _001D, string _000E);

		private static int _0016(IntPtr _001D, IntPtr _000E)
		{
			string[] array = new string[1]
			{
				"OLLYDBG"
			};
			string strA = _0013(_001D);
			string[] array2 = array;
			foreach (string strB in array2)
			{
				if (string.Compare(strA, strB, true) != 0)
				{
					continue;
				}
				while (true)
				{
					staticFlag0015 = true;
					return 0;
				}
			}
			while (true)
			{
				return 1;
			}
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetClassName")]
		internal static extern int _0016(IntPtr _001D, StringBuilder _000E, int _0012);

		internal static string _0013(IntPtr _001D)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			_0016(_001D, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		internal static void method0016()
		{
			if (!_0016())
			{
				return;
			}
			while (true)
			{
				string arg = "Debugger";
				throw new Exception(string.Format("{0} was found - this software cannot be executed under the {0}.", arg));
			}
		}

		internal static bool _0016()
		{
			try
			{
				if (Debugger.IsAttached)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
				IntPtr intPtr = LoadLibrary("kernel32.dll");
				delegateIntNoParam procAddress = GetProcAddress0015(intPtr, "IsDebuggerPresent");
				if (procAddress != null && procAddress() != 0)
				{
					return true;
				}
				uint currentProcessId = GetCurrentProcessId();
				IntPtr intPtr2 = OpenProcess(1024u, 0, currentProcessId);
				if (intPtr2 != IntPtr.Zero)
				{
					try
					{
						delegate0013 procAddress2 = GetProcAddress0013(intPtr, "CheckRemoteDebuggerPresent");
						if (procAddress2 != null)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
								{
									int pbDebuggerPresent = 0;
									if (procAddress2(intPtr2, ref pbDebuggerPresent) != 0)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												break;
											default:
												if (pbDebuggerPresent != 0)
												{
													while (true)
													{
														switch (6)
														{
														case 0:
															break;
														default:
															return true;
														}
													}
												}
												goto end_IL_0076;
											}
										}
									}
									goto end_IL_0076;
								}
								}
							}
						}
						end_IL_0076:;
					}
					finally
					{
						CloseHandle(intPtr2);
					}
				}
				bool flag = false;
				try
				{
					CloseHandle(new IntPtr(305419896));
				}
				catch
				{
					flag = true;
				}
				if (flag)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
				try
				{
					IntPtr intPtr3 = LoadLibrary("user32.dll");
					delegate0009 procAddress3 = GetProcAddress0009(intPtr3, "EnumWindows");
					if (procAddress3 != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								staticFlag0015 = false;
								procAddress3(_0016, IntPtr.Zero);
								if (staticFlag0015)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											return true;
										}
									}
								}
								goto end_IL_0103;
							}
						}
					}
					end_IL_0103:;
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
	}
}
