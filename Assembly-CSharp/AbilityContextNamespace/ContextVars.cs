using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class ContextVars
	{
		public Dictionary<int, int> \u001D = new Dictionary<int, int>();

		public Dictionary<int, float> \u000E = new Dictionary<int, float>();

		public Dictionary<int, Vector3> \u0012 = new Dictionary<int, Vector3>();

		public static int \u0015(string \u001D)
		{
			return Animator.StringToHash(\u001D.Trim().ToUpper());
		}

		public void \u0015()
		{
			this.\u001D.Clear();
			this.\u000E.Clear();
			this.\u0012.Clear();
		}

		public void \u0016(int \u001D, int \u000E)
		{
			this.\u001D[\u001D] = \u000E;
		}

		public void \u0015(int \u001D, float \u000E)
		{
			this.\u000E[\u001D] = \u000E;
		}

		public void \u0015(int \u001D, Vector3 \u000E)
		{
			this.\u0012[\u001D] = \u000E;
		}

		public int \u0015(int \u001D)
		{
			return this.\u001D[\u001D];
		}

		public float \u0015(int \u001D)
		{
			return this.\u000E[\u001D];
		}

		public Vector3 \u0015(int \u001D)
		{
			return this.\u0012[\u001D];
		}

		public bool \u0015(int \u001D, out int \u000E)
		{
			return this.\u001D.TryGetValue(\u001D, out \u000E);
		}

		public bool \u0015(int \u001D, out float \u000E)
		{
			return this.\u000E.TryGetValue(\u001D, out \u000E);
		}

		public bool \u0015(int \u001D, out Vector3 \u000E)
		{
			return this.\u0012.TryGetValue(\u001D, out \u000E);
		}

		public bool \u0015(int \u001D, ContextValueType \u000E)
		{
			if (\u000E == ContextValueType.\u001D)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ContextVars.\u0015(int, ContextValueType)).MethodHandle;
				}
				return this.\u001D.ContainsKey(\u001D);
			}
			if (\u000E == ContextValueType.\u000E)
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
				return this.\u000E.ContainsKey(\u001D);
			}
			if (\u000E == ContextValueType.\u0012)
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
				return this.\u0012.ContainsKey(\u001D);
			}
			return false;
		}

		public bool \u0015(int \u001D)
		{
			return this.\u001D.ContainsKey(\u001D);
		}

		public bool \u0016(int \u001D)
		{
			return this.\u000E.ContainsKey(\u001D);
		}

		public bool \u0013(int \u001D)
		{
			return this.\u0012.ContainsKey(\u001D);
		}

		public static string \u0015(string \u001D, string \u000E, bool \u0012 = true)
		{
			return InEditorDescHelper.ContextVarName(\u001D, \u0012) + " => " + \u000E + "\n";
		}
	}
}
