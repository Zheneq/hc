using System.Collections.Generic;
using UnityEngine;

public class ActorDebugUtils : MonoBehaviour
{
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
		public string m_stringToDisplay = "";
		public float m_spacing;
	}

	private static ActorDebugUtils s_instance;

	private Dictionary<DebugCategory, DebugCategoryInfo> m_categoryToDebugContainer;
	private ActorData m_debugContextActor;

	public bool ShowDebugGUI { get; set; }

	public static ActorDebugUtils Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public bool ShowingCategory(DebugCategory cat, bool requireDebugWindowVisible = true)
	{
		if (!ShowDebugGUI && requireDebugWindowVisible)
		{
			return false;
		}
		DebugCategoryInfo debugCategoryInfo = GetDebugCategoryInfo(cat);
		return debugCategoryInfo != null && debugCategoryInfo.m_enabled;
	}

	public DebugCategoryInfo GetDebugCategoryInfo(DebugCategory cat)
	{
		if (m_categoryToDebugContainer != null && m_categoryToDebugContainer.ContainsKey(cat))
		{
			return m_categoryToDebugContainer[cat];
		}
		return null;
	}

	public ActorData GetDebugContextActor()
	{
		return m_debugContextActor;
	}

	public static void DebugDrawBoundBase(Bounds bound, Color color, float duration = 0f)
	{
		Vector3 vector = new Vector3(bound.center.x - bound.extents.x, bound.center.y - bound.extents.y, bound.center.z + bound.extents.z);
		Vector3 vector2 = new Vector3(bound.center.x + bound.extents.x, bound.center.y - bound.extents.y, bound.center.z + bound.extents.z);
		Vector3 vector3 = new Vector3(bound.center.x - bound.extents.x, bound.center.y - bound.extents.y, bound.center.z - bound.extents.z);
		Vector3 vector4 = new Vector3(bound.center.x + bound.extents.x, bound.center.y - bound.extents.y, bound.center.z - bound.extents.z);
		Debug.DrawLine(vector, vector2, color, duration);
		Debug.DrawLine(vector2, vector4, color, duration);
		Debug.DrawLine(vector4, vector3, color, duration);
		Debug.DrawLine(vector3, vector, color, duration);
	}

	public static void SetTempDebugString(string value)
	{
		if (!Application.isEditor)
		{
			return;
		}
	}
}
