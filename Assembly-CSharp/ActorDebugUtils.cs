using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorDebugUtils : MonoBehaviour
{
	private static ActorDebugUtils s_instance;

	private Dictionary<ActorDebugUtils.DebugCategory, ActorDebugUtils.DebugCategoryInfo> m_categoryToDebugContainer;

	private ActorData m_debugContextActor;

	public static ActorDebugUtils Get()
	{
		return ActorDebugUtils.s_instance;
	}

	private void Awake()
	{
		ActorDebugUtils.s_instance = this;
	}

	private void OnDestroy()
	{
		ActorDebugUtils.s_instance = null;
	}

	public bool ShowDebugGUI { get; set; }

	public bool ShowingCategory(ActorDebugUtils.DebugCategory cat, bool requireDebugWindowVisible = true)
	{
		if (!this.ShowDebugGUI)
		{
			if (requireDebugWindowVisible)
			{
				return false;
			}
		}
		ActorDebugUtils.DebugCategoryInfo debugCategoryInfo = this.GetDebugCategoryInfo(cat);
		bool result;
		if (debugCategoryInfo != null)
		{
			result = debugCategoryInfo.m_enabled;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public ActorDebugUtils.DebugCategoryInfo GetDebugCategoryInfo(ActorDebugUtils.DebugCategory cat)
	{
		if (this.m_categoryToDebugContainer != null)
		{
			if (this.m_categoryToDebugContainer.ContainsKey(cat))
			{
				return this.m_categoryToDebugContainer[cat];
			}
		}
		return null;
	}

	public ActorData GetDebugContextActor()
	{
		return this.m_debugContextActor;
	}

	public static void symbol_001D(Bounds symbol_001D, Color symbol_000E, float symbol_0012 = 0f)
	{
		Vector3 vector = new Vector3(symbol_001D.center.x - symbol_001D.extents.x, symbol_001D.center.y - symbol_001D.extents.y, symbol_001D.center.z + symbol_001D.extents.z);
		Vector3 vector2 = new Vector3(symbol_001D.center.x + symbol_001D.extents.x, symbol_001D.center.y - symbol_001D.extents.y, symbol_001D.center.z + symbol_001D.extents.z);
		Vector3 vector3 = new Vector3(symbol_001D.center.x - symbol_001D.extents.x, symbol_001D.center.y - symbol_001D.extents.y, symbol_001D.center.z - symbol_001D.extents.z);
		Vector3 vector4 = new Vector3(symbol_001D.center.x + symbol_001D.extents.x, symbol_001D.center.y - symbol_001D.extents.y, symbol_001D.center.z - symbol_001D.extents.z);
		Debug.DrawLine(vector, vector2, symbol_000E, symbol_0012);
		Debug.DrawLine(vector2, vector4, symbol_000E, symbol_0012);
		Debug.DrawLine(vector4, vector3, symbol_000E, symbol_0012);
		Debug.DrawLine(vector3, vector, symbol_000E, symbol_0012);
	}

	public static void SetTempDebugString(string value)
	{
		if (Application.isEditor)
		{
		}
	}

	public enum DebugCategory
	{
		None,
		CameraManager,
		Chatter,
		CursorPosition,
		CursorState,
		FreelancerSpecificStats,
		GeneralStats,
		LastKnownPosition,
		TheatricsOrder,
		ForTempDebug,
		NUM
	}

	public class DebugCategoryInfo
	{
		public bool m_enabled;

		public string m_stringToDisplay = string.Empty;

		public float m_spacing;
	}
}
