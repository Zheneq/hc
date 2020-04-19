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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorDebugUtils.ShowingCategory(ActorDebugUtils.DebugCategory, bool)).MethodHandle;
			}
			if (requireDebugWindowVisible)
			{
				return false;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		ActorDebugUtils.DebugCategoryInfo debugCategoryInfo = this.GetDebugCategoryInfo(cat);
		bool result;
		if (debugCategoryInfo != null)
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorDebugUtils.GetDebugCategoryInfo(ActorDebugUtils.DebugCategory)).MethodHandle;
			}
			if (this.m_categoryToDebugContainer.ContainsKey(cat))
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
				return this.m_categoryToDebugContainer[cat];
			}
		}
		return null;
	}

	public ActorData GetDebugContextActor()
	{
		return this.m_debugContextActor;
	}

	public static void \u001D(Bounds \u001D, Color \u000E, float \u0012 = 0f)
	{
		Vector3 vector = new Vector3(\u001D.center.x - \u001D.extents.x, \u001D.center.y - \u001D.extents.y, \u001D.center.z + \u001D.extents.z);
		Vector3 vector2 = new Vector3(\u001D.center.x + \u001D.extents.x, \u001D.center.y - \u001D.extents.y, \u001D.center.z + \u001D.extents.z);
		Vector3 vector3 = new Vector3(\u001D.center.x - \u001D.extents.x, \u001D.center.y - \u001D.extents.y, \u001D.center.z - \u001D.extents.z);
		Vector3 vector4 = new Vector3(\u001D.center.x + \u001D.extents.x, \u001D.center.y - \u001D.extents.y, \u001D.center.z - \u001D.extents.z);
		Debug.DrawLine(vector, vector2, \u000E, \u0012);
		Debug.DrawLine(vector2, vector4, \u000E, \u0012);
		Debug.DrawLine(vector4, vector3, \u000E, \u0012);
		Debug.DrawLine(vector3, vector, \u000E, \u0012);
	}

	public static void SetTempDebugString(string value)
	{
		if (Application.isEditor)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorDebugUtils.SetTempDebugString(string)).MethodHandle;
			}
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
