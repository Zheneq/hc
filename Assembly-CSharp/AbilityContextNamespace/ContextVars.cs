using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class ContextVars
	{
		public Dictionary<int, int> m_intVars = new Dictionary<int, int>();
		public Dictionary<int, float> m_floatVars = new Dictionary<int, float>();
		public Dictionary<int, Vector3> m_vec3Vars = new Dictionary<int, Vector3>();

		public static int ToContextKey(string input)
		{
			return Animator.StringToHash(input.Trim().ToUpper());
		}

		public void ClearData()
		{
			m_intVars.Clear();
			m_floatVars.Clear();
			m_vec3Vars.Clear();
		}

		public void SetValue(int key, int value)
		{
			m_intVars[key] = value;
		}

		public void SetValue(int key, float value)
		{
			m_floatVars[key] = value;
		}

		public void SetValue(int key, Vector3 value)
		{
			m_vec3Vars[key] = value;
		}

		public int GetValueInt(int key)
		{
			return m_intVars[key];
		}

		public float GetValueFloat(int key)
		{
			return m_floatVars[key];
		}

		public Vector3 GetValueVec3(int key)
		{
			return m_vec3Vars[key];
		}

		public bool TryGetInt(int key, out int value)
		{
			return m_intVars.TryGetValue(key, out value);
		}

		public bool TryGetFloat(int key, out float value)
		{
			return m_floatVars.TryGetValue(key, out value);
		}

		public bool TryGetVector(int key, out Vector3 value)
		{
			return m_vec3Vars.TryGetValue(key, out value);
		}

		public bool HasVar(int key, ContextValueType valueType)
		{
			switch (valueType)
			{
				case ContextValueType.Int:
					return m_intVars.ContainsKey(key);
				case ContextValueType.Float:
					return m_floatVars.ContainsKey(key);
				case ContextValueType.Vector3:
					return m_vec3Vars.ContainsKey(key);
			}
			return false;
		}

		public bool HasVarInt(int key)
		{
			return m_intVars.ContainsKey(key);
		}

		public bool HasVarFloat(int key)
		{
			return m_floatVars.ContainsKey(key);
		}

		public bool HasVarVec3(int key)
		{
			return m_vec3Vars.ContainsKey(key);
		}

		public static string GetContextUsageStr(string contextName, string usage, bool actorContext = true)
		{
			return InEditorDescHelper.ContextVarName(contextName, actorContext) + " => " + usage + "\n";
		}
	}
}
