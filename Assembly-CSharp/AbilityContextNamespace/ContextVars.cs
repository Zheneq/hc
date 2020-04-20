using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class ContextVars
	{
		public Dictionary<int, int> IntVars = new Dictionary<int, int>();

		public Dictionary<int, float> FloatVars = new Dictionary<int, float>();

		public Dictionary<int, Vector3> VectorVars = new Dictionary<int, Vector3>();

		public static int GetHash(string \u001D)
		{
			return Animator.StringToHash(\u001D.Trim().ToUpper());
		}

		public void Clear()
		{
			this.IntVars.Clear();
			this.FloatVars.Clear();
			this.VectorVars.Clear();
		}

		public void SetInt(int index, int value)
		{
			this.IntVars[index] = value;
		}

		public void SetFloat(int index, float value)
		{
			this.FloatVars[index] = value;
		}

		public void SetVector(int index, Vector3 value)
		{
			this.VectorVars[index] = value;
		}

		public int GetInt(int index)
		{
			return this.IntVars[index];
		}

		public float GetFloat(int index)
		{
			return this.FloatVars[index];
		}

		public Vector3 GetVector(int index)
		{
			return this.VectorVars[index];
		}

		public bool TryGetInt(int index, out int value)
		{
			return this.IntVars.TryGetValue(index, out value);
		}

		public bool TryGetFloat(int index, out float value)
		{
			return this.FloatVars.TryGetValue(index, out value);
		}

		public bool TryGetVector(int index, out Vector3 value)
		{
			return this.VectorVars.TryGetValue(index, out value);
		}

		public bool Contains(int index, ContextValueType type)
		{
			if (type == ContextValueType.INT)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ContextVars.Contains(int, ContextValueType)).MethodHandle;
				}
				return this.IntVars.ContainsKey(index);
			}
			if (type == ContextValueType.FLOAT)
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
				return this.FloatVars.ContainsKey(index);
			}
			if (type == ContextValueType.VECTOR)
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
				return this.VectorVars.ContainsKey(index);
			}
			return false;
		}

		public bool ContainsInt(int index)
		{
			return this.IntVars.ContainsKey(index);
		}

		public bool ContaintFloat(int index)
		{
			return this.FloatVars.ContainsKey(index);
		}

		public bool ContainsVector(int index)
		{
			return this.VectorVars.ContainsKey(index);
		}

		public static string GetDebugString(string name, string value, bool actorContext = true)
		{
			return InEditorDescHelper.ContextVarName(name, actorContext) + " => " + value + "\n";
		}
	}
}
