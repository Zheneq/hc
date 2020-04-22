using System.Collections.Generic;
using UnityEngine;

public class UIBouncingLaserCursor : MonoBehaviour
{
	public class BounceSegment
	{
		private Vector3 m_start;

		private Vector3 m_end;

		private UIBouncingLaserCursor m_parent;

		public GameObject m_centerLine;

		public GameObject m_lengthLine1;

		public GameObject m_lengthLine2;

		public GameObject m_interior;

		public BounceSegment(Vector3 start, Vector3 end, bool isFirst, bool isLast, UIBouncingLaserCursor parent)
		{
			m_parent = parent;
			if (m_parent.m_centerLinePrefab != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_centerLine = Object.Instantiate(m_parent.m_centerLinePrefab);
				m_centerLine.transform.parent = m_parent.transform;
			}
			if (m_parent.m_lengthLine1Prefab != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				m_lengthLine1 = Object.Instantiate(m_parent.m_lengthLine1Prefab);
				m_lengthLine1.transform.parent = m_parent.transform;
			}
			if (m_parent.m_lengthLine2Prefab != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				m_lengthLine2 = Object.Instantiate(m_parent.m_lengthLine2Prefab);
				m_lengthLine2.transform.parent = m_parent.transform;
			}
			if (m_parent.m_interiorPrefab != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				m_interior = Object.Instantiate(m_parent.m_interiorPrefab);
				m_interior.transform.parent = m_parent.transform;
			}
			UpdateSegment(start, end, isFirst, isLast);
		}

		public void UpdateSegment(Vector3 start, Vector3 end, bool isFirst, bool isLast)
		{
			float y = (float)Board.Get().BaselineHeight + m_parent.m_heightOffset;
			m_start = new Vector3(start.x, y, start.z);
			m_end = new Vector3(end.x, y, end.z);
			Vector3 vector = m_end - m_start;
			float magnitude = vector.magnitude;
			vector.Normalize();
			float num = m_parent.m_lengthPerCorner;
			if (isLast)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (magnitude < num)
				{
					num = Mathf.Max(0.1f, magnitude);
				}
			}
			Quaternion rotation = Quaternion.LookRotation(vector);
			if (isFirst)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_parent.m_start != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					m_parent.m_start.transform.position = start + vector * m_parent.m_distCasterToStart;
					m_parent.m_start.transform.rotation = rotation;
				}
			}
			float num2 = 1f;
			if (m_centerLine != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 position;
				if (isFirst)
				{
					position = m_start + vector * (m_parent.m_distCasterToStart + m_parent.m_distStartToCenterLine);
					num2 = magnitude - (m_parent.m_distCasterToStart + m_parent.m_distStartToCenterLine);
				}
				else
				{
					position = m_start;
					num2 = magnitude;
				}
				m_centerLine.transform.position = position;
				m_centerLine.transform.rotation = rotation;
				m_centerLine.transform.localScale = new Vector3(1f, 1f, num2);
				UIManager.SetGameObjectActive(m_centerLine, true);
			}
			float d = m_parent.m_worldWidth / 2f;
			Vector3 b = Vector3.Cross(vector, Vector3.up) * d;
			if (m_lengthLine1 != null && m_lengthLine2 != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 end2 = m_end;
				float num3 = magnitude;
				if (isLast)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					end2 -= vector * num;
					num3 -= num;
				}
				if (isFirst)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					num3 -= m_parent.m_distCasterToStart;
				}
				num3 = Mathf.Max(num3, 0f);
				m_lengthLine1.transform.position = end2 + b;
				m_lengthLine2.transform.position = end2 - b;
				m_lengthLine1.transform.localScale = new Vector3(1f, 1f, num3);
				m_lengthLine2.transform.localScale = new Vector3(1f, 1f, num3);
				m_lengthLine1.transform.rotation = rotation;
				m_lengthLine2.transform.rotation = rotation;
				UIManager.SetGameObjectActive(m_lengthLine1, true);
				UIManager.SetGameObjectActive(m_lengthLine2, true);
			}
			if (m_interior != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				float num4 = magnitude;
				Vector3 position2;
				if (isFirst)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					position2 = m_start + vector * m_parent.m_distCasterToInterior;
					num4 -= m_parent.m_distCasterToInterior;
				}
				else
				{
					position2 = m_start;
				}
				if (isLast)
				{
					num4 -= num;
				}
				num4 = Mathf.Max(0.1f, num4);
				m_interior.transform.position = position2;
				m_interior.transform.localScale = new Vector3(m_parent.m_worldWidth, 1f, num4);
				m_interior.transform.rotation = rotation;
				UIManager.SetGameObjectActive(m_interior, true);
			}
			if (!isLast)
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				float z = 1f;
				if (!isFirst)
				{
					z = Mathf.Min(1f, num2 / m_parent.m_lengthPerCorner);
				}
				if (m_parent.m_corner1 != null && m_parent.m_corner2 != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					m_parent.m_corner1.transform.position = m_end + b;
					m_parent.m_corner2.transform.position = m_end - b;
					m_parent.m_corner1.transform.rotation = rotation;
					m_parent.m_corner2.transform.rotation = rotation;
					m_parent.m_corner1.transform.localScale = new Vector3(1f, 1f, z);
					m_parent.m_corner2.transform.localScale = new Vector3(1f, 1f, z);
				}
				if (m_parent.m_endWidthLine != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						m_parent.m_endWidthLine.transform.position = m_end;
						m_parent.m_endWidthLine.transform.rotation = rotation;
						float x = m_parent.m_worldWidth - m_parent.m_widthPerCorner * 2f;
						m_parent.m_endWidthLine.transform.localScale = new Vector3(x, 1f, z);
						return;
					}
				}
				return;
			}
		}

		public void Disable()
		{
			if (m_centerLine != null)
			{
				UIManager.SetGameObjectActive(m_centerLine, false);
			}
			if (m_lengthLine1 != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				UIManager.SetGameObjectActive(m_lengthLine1, false);
			}
			if (m_lengthLine2 != null)
			{
				UIManager.SetGameObjectActive(m_lengthLine2, false);
			}
			if (!(m_interior != null))
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				UIManager.SetGameObjectActive(m_interior, false);
				return;
			}
		}

		public void DestroySegment()
		{
			if (m_centerLine != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				Object.Destroy(m_centerLine);
				m_centerLine = null;
			}
			if (m_lengthLine1 != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				Object.Destroy(m_lengthLine1);
				m_lengthLine1 = null;
			}
			if (m_lengthLine2 != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				Object.Destroy(m_lengthLine2);
				m_lengthLine2 = null;
			}
			if (!(m_interior != null))
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				Object.Destroy(m_interior);
				m_interior = null;
				return;
			}
		}
	}

	public GameObject m_startPrefab;

	public GameObject m_centerLinePrefab;

	public GameObject m_lengthLine1Prefab;

	public GameObject m_lengthLine2Prefab;

	public GameObject m_interiorPrefab;

	public GameObject m_corner1Prefab;

	public GameObject m_corner2Prefab;

	public GameObject m_endWidthLinePrefab;

	public float m_distCasterToStart = 0.75f;

	public float m_distStartToCenterLine;

	public float m_distCasterToInterior = 0.75f;

	public float m_widthPerCorner = 0.1f;

	public float m_lengthPerCorner = 1.5f;

	public float m_heightOffset;

	private float m_worldWidth;

	private GameObject m_start;

	private GameObject m_corner1;

	private GameObject m_corner2;

	private GameObject m_endWidthLine;

	public List<BounceSegment> m_bounceSegments;

	public void OnUpdated(Vector3 originalStart, List<Vector3> laserAnglePoints, float worldWidth)
	{
		m_worldWidth = worldWidth;
		if (!(m_worldWidth <= 0f))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (laserAnglePoints.Count != 0)
			{
				for (int i = 0; i < laserAnglePoints.Count; i++)
				{
					Vector3 start = (i != 0) ? laserAnglePoints[i - 1] : originalStart;
					bool isFirst = i == 0;
					bool isLast = i == laserAnglePoints.Count - 1;
					Vector3 end = laserAnglePoints[i];
					if (m_bounceSegments.Count < i + 1)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						m_bounceSegments.Add(new BounceSegment(start, end, isFirst, isLast, this));
					}
					else
					{
						m_bounceSegments[i].UpdateSegment(start, end, isFirst, isLast);
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					for (int j = laserAnglePoints.Count; j < m_bounceSegments.Count; j++)
					{
						m_bounceSegments[j].Disable();
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						UIManager.SetGameObjectActive(base.gameObject, true);
						return;
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		Log.Error("BouncingLaserCursor with invalid dimensions: numPoints = " + laserAnglePoints.Count + ", width = " + m_worldWidth + ".  Disabling...");
		for (int k = 0; k < m_bounceSegments.Count; k++)
		{
			m_bounceSegments[k].Disable();
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			UIManager.SetGameObjectActive(base.gameObject, false);
			return;
		}
	}

	private void Awake()
	{
		m_bounceSegments = new List<BounceSegment>();
		if (m_startPrefab != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_start = Object.Instantiate(m_startPrefab);
			m_start.transform.parent = base.transform;
		}
		if (m_corner1Prefab != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			m_corner1 = Object.Instantiate(m_corner1Prefab);
			m_corner1.transform.parent = base.transform;
		}
		if (m_corner2Prefab != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			m_corner2 = Object.Instantiate(m_corner2Prefab);
			m_corner2.transform.parent = base.transform;
		}
		if (m_endWidthLinePrefab != null)
		{
			m_endWidthLine = Object.Instantiate(m_endWidthLinePrefab);
			m_endWidthLine.transform.parent = base.transform;
		}
	}

	private void OnDestroy()
	{
		using (List<BounceSegment>.Enumerator enumerator = m_bounceSegments.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BounceSegment current = enumerator.Current;
				current.DestroySegment();
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					goto end_IL_000e;
				}
			}
			end_IL_000e:;
		}
		m_bounceSegments.Clear();
		if (m_start != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			Object.Destroy(m_start);
			m_start = null;
		}
		if (m_corner1 != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			Object.Destroy(m_corner1);
			m_corner1 = null;
		}
		if (m_corner2 != null)
		{
			Object.Destroy(m_corner2);
			m_corner2 = null;
		}
		if (m_endWidthLine != null)
		{
			Object.Destroy(m_endWidthLine);
			m_endWidthLine = null;
		}
	}
}
