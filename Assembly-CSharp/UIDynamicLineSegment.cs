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
		if (!(m_segmentObject != null))
		{
			return;
		}
		Renderer[] componentsInChildren = m_segmentObject.GetComponentsInChildren<Renderer>(true);
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			HighlightUtils.DestroyMaterials(renderer.materials);
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			MeshFilter component = m_segmentObject.GetComponent<MeshFilter>();
			if (!(component != null))
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (component.sharedMesh != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						Object.Destroy(component.sharedMesh);
						return;
					}
				}
				return;
			}
		}
	}

	public GameObject CreateSegmentMesh(float width, bool dotted, Color color)
	{
		if (m_segmentObject != null)
		{
			return m_segmentObject;
		}
		m_currentLengthInWorld = 5f;
		m_currentWidthInWorld = width;
		m_dotted = dotted;
		m_currentColor = color;
		List<Vector3> list = new List<Vector3>();
		list.Add(Vector3.zero);
		list.Add(new Vector3(0f, 0f, 5f));
		Material material;
		if (dotted)
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
			material = m_segmentMaterialDotted;
		}
		else
		{
			material = m_segmentMaterialSolid;
		}
		Material material2 = material;
		GameObject gameObject = new GameObject("LineSegment");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		m_segmentMesh = meshFilter.mesh;
		List<Vector3> list2 = new List<Vector3>();
		List<Vector3> list3 = new List<Vector3>();
		List<Vector2> list4 = new List<Vector2>();
		for (int i = 0; i < list.Count; i++)
		{
			if (i == 0)
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
				Vector3 lhs = list[i + 1] - list[i];
				Vector3 normalized = Vector3.Cross(lhs, new Vector3(0f, 1f, 0f)).normalized;
				normalized.y = 0f;
				list2.Add(list[i] + normalized * width);
				list3.Add(new Vector3(0f, 1f, 0f));
				list4.Add(new Vector2(0f, 0f));
				list2.Add(list[i] - normalized * width);
				list3.Add(new Vector3(0f, 1f, 0f));
				list4.Add(new Vector2(0f, 1f));
				continue;
			}
			if (i == list.Count - 1)
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
				Vector3 normalized2 = (list[i] - list[i - 1]).normalized;
				Vector3 normalized3 = Vector3.Cross(normalized2, new Vector3(0f, 1f, 0f)).normalized;
				normalized3.y = 0f;
				list2.Add(list[i] + normalized3 * width);
				list3.Add(new Vector3(0f, 1f, 0f));
				Vector3 vector = list2[list2.Count - 1] - list2[list2.Count - 3];
				vector.y = 0f;
				float magnitude = vector.magnitude;
				Vector2 vector2 = list4[list4.Count - 2];
				list4.Add(new Vector2(vector2.x + magnitude / 1f, 0f));
				list2.Add(list[i] - normalized3 * width);
				list3.Add(new Vector3(0f, 1f, 0f));
				vector = list2[list2.Count - 1] - list2[list2.Count - 3];
				vector.y = 0f;
				magnitude = vector.magnitude;
				Vector2 vector3 = list4[list4.Count - 3];
				list4.Add(new Vector2(vector3.x + magnitude / 1f, 1f));
				continue;
			}
			Vector3 normalized4 = (list[i] - list[i - 1]).normalized;
			Vector3 normalized5 = (list[i + 1] - list[i]).normalized;
			Vector3 normalized6 = (normalized4 + normalized5).normalized;
			Vector3 normalized7 = Vector3.Cross(normalized4, new Vector3(0f, 1f, 0f)).normalized;
			Vector3 vector4 = (normalized6.sqrMagnitude != 0f) ? Vector3.Cross(normalized6, new Vector3(0f, 1f, 0f)).normalized : normalized7;
			float num = Vector3.Dot(vector4, normalized7);
			if (num == 0f)
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
				num = 1f;
			}
			vector4.y = 0f;
			list2.Add(list[i] + vector4 * width / num);
			list3.Add(new Vector3(0f, 1f, 0f));
			Vector3 vector5 = list2[list2.Count - 1] - list2[list2.Count - 3];
			vector5.y = 0f;
			float magnitude2 = vector5.magnitude;
			Vector2 vector6 = list4[list4.Count - 2];
			list4.Add(new Vector2(vector6.x + magnitude2 / 1f, 0f));
			list2.Add(list[i] - vector4 * width / num);
			list3.Add(new Vector3(0f, 1f, 0f));
			vector5 = list2[list2.Count - 1] - list2[list2.Count - 3];
			vector5.y = 0f;
			magnitude2 = vector5.magnitude;
			Vector2 vector7 = list4[list4.Count - 3];
			list4.Add(new Vector2(vector7.x + magnitude2 / 1f, 1f));
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
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			m_segmentMesh.vertices = list2.ToArray();
			m_segmentMesh.normals = list3.ToArray();
			m_segmentMesh.uv = list4.ToArray();
			m_segmentMesh.triangles = list5.ToArray();
			gameObject.GetComponent<Renderer>().material = material2;
			gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", color);
			m_segmentObject = gameObject;
			return gameObject;
		}
	}

	public void AdjustDynamicLineSegmentMesh(float lengthInWorld, Color color)
	{
		if (!(m_segmentObject != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AdjustSegmentLength(lengthInWorld);
			AdjustSegmentColor(color);
			return;
		}
	}

	public void AdjustSegmentLength(float lengthInWorld)
	{
		if (!(m_segmentObject != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_currentLengthInWorld = lengthInWorld;
			float z = lengthInWorld / 5f;
			Vector3 localScale = new Vector3(1f, 1f, z);
			m_segmentObject.transform.localScale = localScale;
			return;
		}
	}

	public void AdjustSegmentColor(Color color)
	{
		if (!(m_segmentObject != null))
		{
			return;
		}
		m_currentColor = color;
		Renderer component = m_segmentObject.GetComponent<Renderer>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (component.material != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					component.material.SetColor("_TintColor", color);
					return;
				}
			}
			return;
		}
	}
}
