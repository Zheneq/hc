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

		public string m_stringToDisplay = string.Empty;

		public float m_spacing;
	}

	private static ActorDebugUtils s_instance;

	private Dictionary<DebugCategory, DebugCategoryInfo> m_categoryToDebugContainer;

	private ActorData m_debugContextActor;

	public bool ShowDebugGUI
	{
		get;
		set;
	}

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
		if (!ShowDebugGUI)
		{
			if (requireDebugWindowVisible)
			{
				return false;
			}
		}
		DebugCategoryInfo debugCategoryInfo = GetDebugCategoryInfo(cat);
		int result;
		if (debugCategoryInfo != null)
		{
			result = (debugCategoryInfo.m_enabled ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public DebugCategoryInfo GetDebugCategoryInfo(DebugCategory cat)
	{
		if (m_categoryToDebugContainer != null)
		{
			if (m_categoryToDebugContainer.ContainsKey(cat))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return m_categoryToDebugContainer[cat];
					}
				}
			}
		}
		return null;
	}

	public ActorData GetDebugContextActor()
	{
		return m_debugContextActor;
	}

	public static void _001D(Bounds _001D, Color _000E, float _0012 = 0f)
	{
		Vector3 center = _001D.center;
		float x = center.x;
		Vector3 extents = _001D.extents;
		float x2 = x - extents.x;
		Vector3 center2 = _001D.center;
		float y = center2.y;
		Vector3 extents2 = _001D.extents;
		float y2 = y - extents2.y;
		Vector3 center3 = _001D.center;
		float z = center3.z;
		Vector3 extents3 = _001D.extents;
		Vector3 vector = new Vector3(x2, y2, z + extents3.z);
		Vector3 center4 = _001D.center;
		float x3 = center4.x;
		Vector3 extents4 = _001D.extents;
		float x4 = x3 + extents4.x;
		Vector3 center5 = _001D.center;
		float y3 = center5.y;
		Vector3 extents5 = _001D.extents;
		float y4 = y3 - extents5.y;
		Vector3 center6 = _001D.center;
		float z2 = center6.z;
		Vector3 extents6 = _001D.extents;
		Vector3 vector2 = new Vector3(x4, y4, z2 + extents6.z);
		Vector3 center7 = _001D.center;
		float x5 = center7.x;
		Vector3 extents7 = _001D.extents;
		float x6 = x5 - extents7.x;
		Vector3 center8 = _001D.center;
		float y5 = center8.y;
		Vector3 extents8 = _001D.extents;
		float y6 = y5 - extents8.y;
		Vector3 center9 = _001D.center;
		float z3 = center9.z;
		Vector3 extents9 = _001D.extents;
		Vector3 vector3 = new Vector3(x6, y6, z3 - extents9.z);
		Vector3 center10 = _001D.center;
		float x7 = center10.x;
		Vector3 extents10 = _001D.extents;
		float x8 = x7 + extents10.x;
		Vector3 center11 = _001D.center;
		float y7 = center11.y;
		Vector3 extents11 = _001D.extents;
		float y8 = y7 - extents11.y;
		Vector3 center12 = _001D.center;
		float z4 = center12.z;
		Vector3 extents12 = _001D.extents;
		Vector3 vector4 = new Vector3(x8, y8, z4 - extents12.z);
		Debug.DrawLine(vector, vector2, _000E, _0012);
		Debug.DrawLine(vector2, vector4, _000E, _0012);
		Debug.DrawLine(vector4, vector3, _000E, _0012);
		Debug.DrawLine(vector3, vector, _000E, _0012);
	}

	public static void SetTempDebugString(string value)
	{
		if (!Application.isEditor)
		{
			return;
		}
		while (true)
		{
			return;
		}
	}
}
