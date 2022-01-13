using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class ContextVars
	{
		public Dictionary<int, int> IntVars = new Dictionary<int, int>();

		public Dictionary<int, float> FloatVars = new Dictionary<int, float>();

		public Dictionary<int, Vector3> VectorVars = new Dictionary<int, Vector3>();

		public static int GetKey(string name)
		{
			return Animator.StringToHash(name.Trim().ToUpper());
		}

		public void Clear()
		{
			IntVars.Clear();
			FloatVars.Clear();
			VectorVars.Clear();
		}

		public void SetInt(int index, int value)
		{
			IntVars[index] = value;
		}

		public void SetFloat(int index, float value)
		{
			FloatVars[index] = value;
		}

		public void SetVector(int index, Vector3 value)
		{
			VectorVars[index] = value;
		}

		public int GetInt(int index)
		{
			return IntVars[index];
		}

		public float GetFloat(int index)
		{
			return FloatVars[index];
		}

		public Vector3 GetVector(int index)
		{
			return VectorVars[index];
		}

		public bool TryGetInt(int index, out int value)
		{
			return IntVars.TryGetValue(index, out value);
		}

		public bool TryGetFloat(int index, out float value)
		{
			return FloatVars.TryGetValue(index, out value);
		}

		public bool TryGetVector(int index, out Vector3 value)
		{
			return VectorVars.TryGetValue(index, out value);
		}

		public bool Contains(int index, ContextValueType type)
		{
			switch (type)
			{
				case ContextValueType.Int:
					return IntVars.ContainsKey(index);
				case ContextValueType.Float:
					return FloatVars.ContainsKey(index);
				case ContextValueType.Vector3:
					return VectorVars.ContainsKey(index);
			}
			return false;
		}

		public bool ContainsInt(int index)
		{
			return IntVars.ContainsKey(index);
		}

		public bool ContaintFloat(int index)
		{
			return FloatVars.ContainsKey(index);
		}

		public bool ContainsVector(int index)
		{
			return VectorVars.ContainsKey(index);
		}

		public static string GetDebugString(string name, string value, bool actorContext = true)
		{
			return InEditorDescHelper.ContextVarName(name, actorContext) + " => " + value + "\n";
		}
	}
}
