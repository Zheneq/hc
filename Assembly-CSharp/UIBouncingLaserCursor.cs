using System;
using System.Collections.Generic;
using UnityEngine;

public class UIBouncingLaserCursor : MonoBehaviour
{
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

	public List<UIBouncingLaserCursor.BounceSegment> m_bounceSegments;

	public void OnUpdated(Vector3 originalStart, List<Vector3> laserAnglePoints, float worldWidth)
	{
		this.m_worldWidth = worldWidth;
		if (this.m_worldWidth > 0f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBouncingLaserCursor.OnUpdated(Vector3, List<Vector3>, float)).MethodHandle;
			}
			if (laserAnglePoints.Count != 0)
			{
				for (int i = 0; i < laserAnglePoints.Count; i++)
				{
					Vector3 start;
					if (i == 0)
					{
						start = originalStart;
					}
					else
					{
						start = laserAnglePoints[i - 1];
					}
					bool isFirst = i == 0;
					bool isLast = i == laserAnglePoints.Count - 1;
					Vector3 end = laserAnglePoints[i];
					if (this.m_bounceSegments.Count < i + 1)
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
						this.m_bounceSegments.Add(new UIBouncingLaserCursor.BounceSegment(start, end, isFirst, isLast, this));
					}
					else
					{
						this.m_bounceSegments[i].UpdateSegment(start, end, isFirst, isLast);
					}
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int j = laserAnglePoints.Count; j < this.m_bounceSegments.Count; j++)
				{
					this.m_bounceSegments[j].Disable();
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
				UIManager.SetGameObjectActive(base.gameObject, true, null);
				return;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		Log.Error(string.Concat(new object[]
		{
			"BouncingLaserCursor with invalid dimensions: numPoints = ",
			laserAnglePoints.Count,
			", width = ",
			this.m_worldWidth,
			".  Disabling..."
		}), new object[0]);
		for (int k = 0; k < this.m_bounceSegments.Count; k++)
		{
			this.m_bounceSegments[k].Disable();
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		UIManager.SetGameObjectActive(base.gameObject, false, null);
	}

	private void Awake()
	{
		this.m_bounceSegments = new List<UIBouncingLaserCursor.BounceSegment>();
		if (this.m_startPrefab != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBouncingLaserCursor.Awake()).MethodHandle;
			}
			this.m_start = UnityEngine.Object.Instantiate<GameObject>(this.m_startPrefab);
			this.m_start.transform.parent = base.transform;
		}
		if (this.m_corner1Prefab != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_corner1 = UnityEngine.Object.Instantiate<GameObject>(this.m_corner1Prefab);
			this.m_corner1.transform.parent = base.transform;
		}
		if (this.m_corner2Prefab != null)
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
			this.m_corner2 = UnityEngine.Object.Instantiate<GameObject>(this.m_corner2Prefab);
			this.m_corner2.transform.parent = base.transform;
		}
		if (this.m_endWidthLinePrefab != null)
		{
			this.m_endWidthLine = UnityEngine.Object.Instantiate<GameObject>(this.m_endWidthLinePrefab);
			this.m_endWidthLine.transform.parent = base.transform;
		}
	}

	private void OnDestroy()
	{
		using (List<UIBouncingLaserCursor.BounceSegment>.Enumerator enumerator = this.m_bounceSegments.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIBouncingLaserCursor.BounceSegment bounceSegment = enumerator.Current;
				bounceSegment.DestroySegment();
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBouncingLaserCursor.OnDestroy()).MethodHandle;
			}
		}
		this.m_bounceSegments.Clear();
		if (this.m_start != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			UnityEngine.Object.Destroy(this.m_start);
			this.m_start = null;
		}
		if (this.m_corner1 != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			UnityEngine.Object.Destroy(this.m_corner1);
			this.m_corner1 = null;
		}
		if (this.m_corner2 != null)
		{
			UnityEngine.Object.Destroy(this.m_corner2);
			this.m_corner2 = null;
		}
		if (this.m_endWidthLine != null)
		{
			UnityEngine.Object.Destroy(this.m_endWidthLine);
			this.m_endWidthLine = null;
		}
	}

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
			this.m_parent = parent;
			if (this.m_parent.m_centerLinePrefab != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIBouncingLaserCursor.BounceSegment..ctor(Vector3, Vector3, bool, bool, UIBouncingLaserCursor)).MethodHandle;
				}
				this.m_centerLine = UnityEngine.Object.Instantiate<GameObject>(this.m_parent.m_centerLinePrefab);
				this.m_centerLine.transform.parent = this.m_parent.transform;
			}
			if (this.m_parent.m_lengthLine1Prefab != null)
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
				this.m_lengthLine1 = UnityEngine.Object.Instantiate<GameObject>(this.m_parent.m_lengthLine1Prefab);
				this.m_lengthLine1.transform.parent = this.m_parent.transform;
			}
			if (this.m_parent.m_lengthLine2Prefab != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_lengthLine2 = UnityEngine.Object.Instantiate<GameObject>(this.m_parent.m_lengthLine2Prefab);
				this.m_lengthLine2.transform.parent = this.m_parent.transform;
			}
			if (this.m_parent.m_interiorPrefab != null)
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
				this.m_interior = UnityEngine.Object.Instantiate<GameObject>(this.m_parent.m_interiorPrefab);
				this.m_interior.transform.parent = this.m_parent.transform;
			}
			this.UpdateSegment(start, end, isFirst, isLast);
		}

		public void UpdateSegment(Vector3 start, Vector3 end, bool isFirst, bool isLast)
		{
			float y = (float)Board.\u000E().BaselineHeight + this.m_parent.m_heightOffset;
			this.m_start = new Vector3(start.x, y, start.z);
			this.m_end = new Vector3(end.x, y, end.z);
			Vector3 vector = this.m_end - this.m_start;
			float magnitude = vector.magnitude;
			vector.Normalize();
			float num = this.m_parent.m_lengthPerCorner;
			if (isLast)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIBouncingLaserCursor.BounceSegment.UpdateSegment(Vector3, Vector3, bool, bool)).MethodHandle;
				}
				if (magnitude < num)
				{
					num = Mathf.Max(0.1f, magnitude);
				}
			}
			Quaternion rotation = Quaternion.LookRotation(vector);
			if (isFirst)
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
				if (this.m_parent.m_start != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_parent.m_start.transform.position = start + vector * this.m_parent.m_distCasterToStart;
					this.m_parent.m_start.transform.rotation = rotation;
				}
			}
			float num2 = 1f;
			if (this.m_centerLine != null)
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
				Vector3 position;
				if (isFirst)
				{
					position = this.m_start + vector * (this.m_parent.m_distCasterToStart + this.m_parent.m_distStartToCenterLine);
					num2 = magnitude - (this.m_parent.m_distCasterToStart + this.m_parent.m_distStartToCenterLine);
				}
				else
				{
					position = this.m_start;
					num2 = magnitude;
				}
				this.m_centerLine.transform.position = position;
				this.m_centerLine.transform.rotation = rotation;
				this.m_centerLine.transform.localScale = new Vector3(1f, 1f, num2);
				UIManager.SetGameObjectActive(this.m_centerLine, true, null);
			}
			float d = this.m_parent.m_worldWidth / 2f;
			Vector3 b = Vector3.Cross(vector, Vector3.up) * d;
			if (this.m_lengthLine1 != null && this.m_lengthLine2 != null)
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
				Vector3 a = this.m_end;
				float num3 = magnitude;
				if (isLast)
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
					a -= vector * num;
					num3 -= num;
				}
				if (isFirst)
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
					num3 -= this.m_parent.m_distCasterToStart;
				}
				num3 = Mathf.Max(num3, 0f);
				this.m_lengthLine1.transform.position = a + b;
				this.m_lengthLine2.transform.position = a - b;
				this.m_lengthLine1.transform.localScale = new Vector3(1f, 1f, num3);
				this.m_lengthLine2.transform.localScale = new Vector3(1f, 1f, num3);
				this.m_lengthLine1.transform.rotation = rotation;
				this.m_lengthLine2.transform.rotation = rotation;
				UIManager.SetGameObjectActive(this.m_lengthLine1, true, null);
				UIManager.SetGameObjectActive(this.m_lengthLine2, true, null);
			}
			if (this.m_interior != null)
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
				float num4 = magnitude;
				Vector3 position2;
				if (isFirst)
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
					position2 = this.m_start + vector * this.m_parent.m_distCasterToInterior;
					num4 -= this.m_parent.m_distCasterToInterior;
				}
				else
				{
					position2 = this.m_start;
				}
				if (isLast)
				{
					num4 -= num;
				}
				num4 = Mathf.Max(0.1f, num4);
				this.m_interior.transform.position = position2;
				this.m_interior.transform.localScale = new Vector3(this.m_parent.m_worldWidth, 1f, num4);
				this.m_interior.transform.rotation = rotation;
				UIManager.SetGameObjectActive(this.m_interior, true, null);
			}
			if (isLast)
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
				float z = 1f;
				if (!isFirst)
				{
					z = Mathf.Min(1f, num2 / this.m_parent.m_lengthPerCorner);
				}
				if (this.m_parent.m_corner1 != null && this.m_parent.m_corner2 != null)
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
					this.m_parent.m_corner1.transform.position = this.m_end + b;
					this.m_parent.m_corner2.transform.position = this.m_end - b;
					this.m_parent.m_corner1.transform.rotation = rotation;
					this.m_parent.m_corner2.transform.rotation = rotation;
					this.m_parent.m_corner1.transform.localScale = new Vector3(1f, 1f, z);
					this.m_parent.m_corner2.transform.localScale = new Vector3(1f, 1f, z);
				}
				if (this.m_parent.m_endWidthLine != null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_parent.m_endWidthLine.transform.position = this.m_end;
					this.m_parent.m_endWidthLine.transform.rotation = rotation;
					float x = this.m_parent.m_worldWidth - this.m_parent.m_widthPerCorner * 2f;
					this.m_parent.m_endWidthLine.transform.localScale = new Vector3(x, 1f, z);
				}
			}
		}

		public void Disable()
		{
			if (this.m_centerLine != null)
			{
				UIManager.SetGameObjectActive(this.m_centerLine, false, null);
			}
			if (this.m_lengthLine1 != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIBouncingLaserCursor.BounceSegment.Disable()).MethodHandle;
				}
				UIManager.SetGameObjectActive(this.m_lengthLine1, false, null);
			}
			if (this.m_lengthLine2 != null)
			{
				UIManager.SetGameObjectActive(this.m_lengthLine2, false, null);
			}
			if (this.m_interior != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(this.m_interior, false, null);
			}
		}

		public void DestroySegment()
		{
			if (this.m_centerLine != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIBouncingLaserCursor.BounceSegment.DestroySegment()).MethodHandle;
				}
				UnityEngine.Object.Destroy(this.m_centerLine);
				this.m_centerLine = null;
			}
			if (this.m_lengthLine1 != null)
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
				UnityEngine.Object.Destroy(this.m_lengthLine1);
				this.m_lengthLine1 = null;
			}
			if (this.m_lengthLine2 != null)
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
				UnityEngine.Object.Destroy(this.m_lengthLine2);
				this.m_lengthLine2 = null;
			}
			if (this.m_interior != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				UnityEngine.Object.Destroy(this.m_interior);
				this.m_interior = null;
			}
		}
	}
}
