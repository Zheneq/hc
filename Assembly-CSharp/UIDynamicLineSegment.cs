using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDynamicLineSegment : MonoBehaviour
{
	public Material m_segmentMaterialSolid;

	public Material m_segmentMaterialDotted;

	private Mesh m_segmentMesh;

	private GameObject m_segmentObject;

	internal float m_currentLengthInWorld = 1f;

	internal float m_currentWidthInWorld = 1f;

	internal bool m_dotted;

	internal Color m_currentColor = Color.white;

	private const float c_initialMeshLength = 5f;

	private void Awake()
	{
	}

	private void OnDestroy()
	{
		if (this.m_segmentObject != null)
		{
			Renderer[] componentsInChildren = this.m_segmentObject.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer renderer in componentsInChildren)
			{
				HighlightUtils.DestroyMaterials(renderer.materials);
			}
			MeshFilter component = this.m_segmentObject.GetComponent<MeshFilter>();
			if (component != null)
			{
				if (component.sharedMesh != null)
				{
					UnityEngine.Object.Destroy(component.sharedMesh);
				}
			}
		}
	}

	public GameObject CreateSegmentMesh(float width, bool dotted, Color color)
	{
		if (this.m_segmentObject != null)
		{
			return this.m_segmentObject;
		}
		this.m_currentLengthInWorld = 5f;
		this.m_currentWidthInWorld = width;
		this.m_dotted = dotted;
		this.m_currentColor = color;
		List<Vector3> list = new List<Vector3>();
		list.Add(Vector3.zero);
		list.Add(new Vector3(0f, 0f, 5f));
		Material material;
		if (dotted)
		{
			material = this.m_segmentMaterialDotted;
		}
		else
		{
			material = this.m_segmentMaterialSolid;
		}
		Material material2 = material;
		GameObject gameObject = new GameObject("LineSegment");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		this.m_segmentMesh = meshFilter.mesh;
		List<Vector3> list2 = new List<Vector3>();
		List<Vector3> list3 = new List<Vector3>();
		List<Vector2> list4 = new List<Vector2>();
		for (int i = 0; i < list.Count; i++)
		{
			if (i == 0)
			{
				Vector3 lhs = list[i + 1] - list[i];
				Vector3 normalized = Vector3.Cross(lhs, new Vector3(0f, 1f, 0f)).normalized;
				normalized.y = 0f;
				list2.Add(list[i] + normalized * width);
				list3.Add(new Vector3(0f, 1f, 0f));
				list4.Add(new Vector2(0f, 0f));
				list2.Add(list[i] - normalized * width);
				list3.Add(new Vector3(0f, 1f, 0f));
				list4.Add(new Vector2(0f, 1f));
			}
			else if (i == list.Count - 1)
			{
				Vector3 normalized2 = (list[i] - list[i - 1]).normalized;
				Vector3 normalized3 = Vector3.Cross(normalized2, new Vector3(0f, 1f, 0f)).normalized;
				normalized3.y = 0f;
				list2.Add(list[i] + normalized3 * width);
				list3.Add(new Vector3(0f, 1f, 0f));
				Vector3 vector = list2[list2.Count - 1] - list2[list2.Count - 3];
				vector.y = 0f;
				float magnitude = vector.magnitude;
				list4.Add(new Vector2(list4[list4.Count - 2].x + magnitude / 1f, 0f));
				list2.Add(list[i] - normalized3 * width);
				list3.Add(new Vector3(0f, 1f, 0f));
				vector = list2[list2.Count - 1] - list2[list2.Count - 3];
				vector.y = 0f;
				magnitude = vector.magnitude;
				list4.Add(new Vector2(list4[list4.Count - 3].x + magnitude / 1f, 1f));
			}
			else
			{
				Vector3 normalized4 = (list[i] - list[i - 1]).normalized;
				Vector3 normalized5 = (list[i + 1] - list[i]).normalized;
				Vector3 normalized6 = (normalized4 + normalized5).normalized;
				Vector3 normalized7 = Vector3.Cross(normalized4, new Vector3(0f, 1f, 0f)).normalized;
				Vector3 vector2;
				if (normalized6.sqrMagnitude == 0f)
				{
					vector2 = normalized7;
				}
				else
				{
					vector2 = Vector3.Cross(normalized6, new Vector3(0f, 1f, 0f)).normalized;
				}
				float num = Vector3.Dot(vector2, normalized7);
				if (num == 0f)
				{
					num = 1f;
				}
				vector2.y = 0f;
				list2.Add(list[i] + vector2 * width / num);
				list3.Add(new Vector3(0f, 1f, 0f));
				Vector3 vector3 = list2[list2.Count - 1] - list2[list2.Count - 3];
				vector3.y = 0f;
				float magnitude2 = vector3.magnitude;
				list4.Add(new Vector2(list4[list4.Count - 2].x + magnitude2 / 1f, 0f));
				list2.Add(list[i] - vector2 * width / num);
				list3.Add(new Vector3(0f, 1f, 0f));
				vector3 = list2[list2.Count - 1] - list2[list2.Count - 3];
				vector3.y = 0f;
				magnitude2 = vector3.magnitude;
				list4.Add(new Vector2(list4[list4.Count - 3].x + magnitude2 / 1f, 1f));
			}
		}
		List<int> list5 = new List<int>();
		for (int j = 0; j < list.Count - 1; j++)
		{
			list5.Add(j * 2);
			list5.Add(j * 2 + 2);
			list5.Add(j * 2 + 1);
			list5.Add(j * 2 + 1);
			list5.Add(j * 2 + 2);
			list5.Add(j * 2 + 3);
		}
		this.m_segmentMesh.vertices = list2.ToArray();
		this.m_segmentMesh.normals = list3.ToArray();
		this.m_segmentMesh.uv = list4.ToArray();
		this.m_segmentMesh.triangles = list5.ToArray();
		gameObject.GetComponent<Renderer>().material = material2;
		gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", color);
		this.m_segmentObject = gameObject;
		return gameObject;
	}

	public void AdjustDynamicLineSegmentMesh(float lengthInWorld, Color color)
	{
		if (this.m_segmentObject != null)
		{
			this.AdjustSegmentLength(lengthInWorld);
			this.AdjustSegmentColor(color);
		}
	}

	public void AdjustSegmentLength(float lengthInWorld)
	{
		if (this.m_segmentObject != null)
		{
			this.m_currentLengthInWorld = lengthInWorld;
			float z = lengthInWorld / 5f;
			Vector3 localScale = new Vector3(1f, 1f, z);
			this.m_segmentObject.transform.localScale = localScale;
		}
	}

	public void AdjustSegmentColor(Color color)
	{
		if (this.m_segmentObject != null)
		{
			this.m_currentColor = color;
			Renderer component = this.m_segmentObject.GetComponent<Renderer>();
			if (component != null)
			{
				if (component.material != null)
				{
					component.material.SetColor("_TintColor", color);
				}
			}
		}
	}
}
