using System;
using UnityEngine;
using UnityEngine.Rendering;

public class UIDynamicCone : MonoBehaviour
{
	[Range(0f, 360f)]
	public float m_initialConeWidthAngle = 45f;

	[Range(0.1f, 100f)]
	public float m_initialConeRadius = 1f;

	[Header("-- Cone UV center and radius (value range from 0 to 1) --")]
	public float m_coneUvCenterX = 0.5f;

	public float m_coneUvCenterY = 0.5f;

	public float m_coneUvRadius = 0.5f;

	[Header("-- Border -- ")]
	public bool m_showBorder = true;

	public float m_borderThickness = 0.2f;

	[Header("-- Border UV (TODO: modify after we know texture layout) --")]
	public float m_borderUvCenterX = 0.5f;

	public float m_borderUvCenterY = 0.5f;

	public float m_borderUvRadius = 0.5f;

	public float m_innerTextureUv;

	public float m_outerTextureUv = 0.2f;

	[Header("-- Materials --")]
	public Material m_coneMaterial;

	public Material m_borderMaterial;

	private GameObject m_sideA;

	private GameObject m_sideB;

	private GameObject m_sideAParent;

	private GameObject m_sideBParent;

	private float c_borderZOffset = 0.2f;

	private float m_borderStartOffsetInSquares;

	private Mesh m_coneMesh;

	private Mesh m_borderMesh;

	private GameObject m_createdConeObject;

	private GameObject m_createBorderObject;

	private int m_maxConePieces = -1;

	private const int c_coneAnglePerPiece = 10;

	private int m_maxBorderPieces = -1;

	private const int c_borderAnglePerPiece = 5;

	internal bool m_forceHideSides;

	internal bool m_emptyCone;

	internal float m_currentRadiusInWorld = 1f;

	internal float m_currentAngleInWorld;

	internal bool m_currentSidesActive = true;

	internal bool m_currentBorderActive = true;

	public void InitCone()
	{
		m_maxConePieces = 36;
		if (m_maxConePieces * 10 < 360)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_maxConePieces++;
		}
		if (!m_emptyCone)
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
			m_createdConeObject = CreateConeMesh(m_initialConeWidthAngle, m_initialConeRadius, m_coneMaterial);
		}
		if (m_showBorder)
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
			m_maxBorderPieces = 72;
			if (m_maxBorderPieces * 5 < 360)
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
				m_maxBorderPieces++;
			}
			m_createBorderObject = CreateBorderConeMesh(m_initialConeWidthAngle, m_initialConeRadius, m_borderMaterial);
		}
		m_sideA = HighlightUtils.Get().CreateBoundaryLine(m_initialConeRadius, true, true);
		m_sideB = HighlightUtils.Get().CreateBoundaryLine(m_initialConeRadius, true, false);
		if (m_sideA != null)
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
			m_sideAParent = new GameObject();
			m_sideA.transform.parent = m_sideAParent.transform;
			m_sideA.transform.localPosition = new Vector3(0f, 0f, 0f - c_borderZOffset);
			SetAsChild(m_sideAParent);
		}
		if (m_sideB != null)
		{
			m_sideBParent = new GameObject();
			m_sideB.transform.parent = m_sideBParent.transform;
			m_sideB.transform.localPosition = new Vector3(0f, 0f, 0f - c_borderZOffset);
			SetAsChild(m_sideBParent);
		}
	}

	private void OnDestroy()
	{
		if (m_createdConeObject != null)
		{
			MeshFilter component = m_createdConeObject.GetComponent<MeshFilter>();
			if (component != null)
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
				if (component.sharedMesh != null)
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
					UnityEngine.Object.Destroy(component.sharedMesh);
				}
			}
		}
		if (!(m_createBorderObject != null))
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
			MeshFilter component2 = m_createBorderObject.GetComponent<MeshFilter>();
			if (!(component2 != null))
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
				if (component2.sharedMesh != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						UnityEngine.Object.Destroy(component2.sharedMesh);
						return;
					}
				}
				return;
			}
		}
	}

	private void SetAsChild(GameObject child)
	{
		if (!(child != null))
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
			child.transform.parent = base.transform;
			child.transform.localPosition = Vector3.zero;
			child.transform.localRotation = Quaternion.identity;
			return;
		}
	}

	public GameObject CreateConeMesh(float coneWidthAngle, float coneRadius, Material material)
	{
		GameObject gameObject = new GameObject("ConeMeshObject");
		SetAsChild(gameObject);
		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
		component.shadowCastingMode = ShadowCastingMode.Off;
		component.reflectionProbeUsage = ReflectionProbeUsage.Off;
		component.receiveShadows = false;
		component.lightProbeUsage = LightProbeUsage.Off;
		MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
		m_coneMesh = component2.mesh;
		m_coneMesh.Clear();
		int num = 2 + m_maxConePieces;
		Vector3[] array = new Vector3[num];
		Vector2[] array2 = new Vector2[num];
		Vector3[] array3 = new Vector3[num];
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i] = new Vector3(0f, -1f, 0f);
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
			array[0] = Vector3.zero;
			array2[0] = new Vector2(0.5f, 0.5f);
			int[] array4 = new int[3 * m_maxConePieces];
			m_coneMesh.vertices = array;
			m_coneMesh.uv = array2;
			AdjustConeMeshVertices(coneWidthAngle, coneRadius);
			for (int j = 0; j < m_maxConePieces; j++)
			{
				int num2 = j * 3;
				int num3 = 1 + j;
				int num4 = 1 + (j + 1);
				array4[num2] = 0;
				array4[num2 + 1] = num4;
				array4[num2 + 2] = num3;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				m_coneMesh.triangles = array4;
				m_coneMesh.normals = array3;
				component.material = material;
				m_coneMesh.bounds = new Bounds(gameObject.transform.parent.position, Vector3.one * 200f);
				return gameObject;
			}
		}
	}

	public void AdjustConeMeshVertices(float coneWidthAngle, float coneRadius)
	{
		coneWidthAngle = Mathf.Max(0f, coneWidthAngle);
		coneRadius = Mathf.Max(0f, coneRadius);
		if (Mathf.Approximately(coneWidthAngle, m_currentAngleInWorld))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (Mathf.Approximately(coneRadius, m_currentRadiusInWorld))
			{
				return;
			}
		}
		m_currentRadiusInWorld = coneRadius;
		m_currentAngleInWorld = coneWidthAngle;
		float num = 0.5f * coneWidthAngle;
		if (m_coneMesh != null)
		{
			Vector3[] vertices = m_coneMesh.vertices;
			Vector2[] uv = m_coneMesh.uv;
			float d = coneRadius;
			float num2 = 0f;
			for (int i = 0; i <= m_maxConePieces; i++)
			{
				float f = (90f + num2 - num) * ((float)Math.PI / 180f);
				float num3 = Mathf.Cos(f);
				float num4 = Mathf.Sin(f);
				Vector3 a = new Vector3(num3, 0f, num4);
				vertices[1 + i] = a * d;
				uv[1 + i] = new Vector2(m_coneUvCenterX + m_coneUvRadius * num3, m_coneUvCenterY + m_coneUvRadius * num4);
				num2 = Mathf.Min(coneWidthAngle, num2 + 10f);
			}
			m_coneMesh.vertices = vertices;
			m_coneMesh.uv = uv;
		}
		if (m_showBorder)
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
			AdjustConeBorderMeshVertices(coneWidthAngle, coneRadius);
		}
		if (coneWidthAngle < 360f)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					SetBorderStartOffset(m_borderStartOffsetInSquares);
					if (m_sideA != null)
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
						if (m_sideAParent != null)
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
							m_sideAParent.transform.localRotation = Quaternion.LookRotation(-1f * VectorUtils.AngleDegreesToVector(90f + num));
						}
					}
					if (m_sideB != null && m_sideBParent != null)
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
						m_sideBParent.transform.localRotation = Quaternion.LookRotation(-1f * VectorUtils.AngleDegreesToVector(90f - num));
					}
					SetSidesActive(!m_forceHideSides);
					return;
				}
			}
		}
		SetSidesActive(false);
	}

	public void SetConeObjectActive(bool active)
	{
		if (!(m_createdConeObject != null))
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
			UIManager.SetGameObjectActive(m_createdConeObject, active);
			return;
		}
	}

	public GameObject CreateBorderConeMesh(float coneWidthAngle, float coneRadius, Material material)
	{
		GameObject gameObject = new GameObject("ConeBorderMeshObject");
		SetAsChild(gameObject);
		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
		component.shadowCastingMode = ShadowCastingMode.Off;
		component.reflectionProbeUsage = ReflectionProbeUsage.Off;
		component.receiveShadows = false;
		component.lightProbeUsage = LightProbeUsage.Off;
		MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
		m_borderMesh = component2.mesh;
		m_borderMesh.Clear();
		int num = 2 + 2 * m_maxBorderPieces;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		Vector3[] array = new Vector3[num];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new Vector3(0f, -1f, 0f);
		}
		int[] array2 = new int[6 * m_maxBorderPieces];
		m_borderMesh.vertices = vertices;
		m_borderMesh.uv = uv;
		AdjustConeBorderMeshVertices(coneWidthAngle, coneRadius);
		for (int j = 0; j < m_maxBorderPieces; j++)
		{
			int num2 = j * 6;
			int num3 = 2 * j;
			int num4 = 2 * j + 1;
			int num5 = 2 * (j + 1);
			int num6 = 2 * (j + 1) + 1;
			array2[num2] = num3;
			array2[num2 + 1] = num5;
			array2[num2 + 2] = num4;
			array2[num2 + 3] = num4;
			array2[num2 + 4] = num5;
			array2[num2 + 5] = num6;
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
			m_borderMesh.triangles = array2;
			m_borderMesh.normals = array;
			component.material = material;
			m_borderMesh.bounds = new Bounds(gameObject.transform.parent.position, Vector3.one * 200f);
			return gameObject;
		}
	}

	public void AdjustConeBorderMeshVertices(float coneWidthAngle, float coneRadius)
	{
		coneWidthAngle = Mathf.Max(0f, coneWidthAngle);
		coneRadius = Mathf.Max(0f, coneRadius);
		if (!(m_borderMesh != null))
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
			Vector3[] vertices = m_borderMesh.vertices;
			Vector2[] uv = m_borderMesh.uv;
			float num = 0.5f * coneWidthAngle;
			float d = Mathf.Max(0f, coneRadius - m_borderThickness);
			float d2 = coneRadius;
			float num2 = 0f;
			for (int i = 0; i <= m_maxBorderPieces; i++)
			{
				float f = (90f + num2 - num) * ((float)Math.PI / 180f);
				float num3 = Mathf.Cos(f);
				float num4 = Mathf.Sin(f);
				Vector3 a = new Vector3(num3, 0f, num4);
				vertices[2 * i] = a * d;
				vertices[2 * i + 1] = a * d2;
				float num5 = m_borderUvRadius * m_innerTextureUv;
				float num6 = m_borderUvRadius * m_outerTextureUv;
				uv[2 * i] = new Vector2(m_borderUvCenterX + num5 * num3, m_borderUvCenterY + num5 * num4);
				uv[2 * i + 1] = new Vector2(m_borderUvCenterX + num6 * num3, m_borderUvCenterY + num6 * num4);
				num2 = Mathf.Min(coneWidthAngle, num2 + 5f);
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				m_borderMesh.vertices = vertices;
				m_borderMesh.uv = uv;
				return;
			}
		}
	}

	public void SetSidesActive(bool active)
	{
		if (m_sideA != null && m_sideA.activeSelf != active)
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
			UIManager.SetGameObjectActive(m_sideA, active);
		}
		if (m_sideB != null && m_sideB.activeSelf != active)
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
			UIManager.SetGameObjectActive(m_sideB, active);
		}
		m_currentSidesActive = active;
	}

	public void SetForceHideSides(bool forceHide)
	{
		m_forceHideSides = forceHide;
	}

	public void SetBorderActive(bool active)
	{
		if (m_createBorderObject != null)
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
			if (m_createBorderObject.activeSelf != active)
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
				m_createBorderObject.SetActive(active);
			}
		}
		m_currentBorderActive = active;
	}

	public void SetBorderStartOffset(float startOffsetInSquares)
	{
		m_borderStartOffsetInSquares = startOffsetInSquares;
		float num = Mathf.Max(0f, m_currentRadiusInWorld - c_borderZOffset) / Board.Get().squareSize;
		float num2 = m_borderStartOffsetInSquares * Board.Get().squareSize;
		float lengthInSquares = Mathf.Max(0f, num - m_borderStartOffsetInSquares);
		if (m_sideA != null)
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
			if (m_sideAParent != null)
			{
				HighlightUtils.Get().ResizeBoundaryLine(lengthInSquares, m_sideA);
				m_sideA.transform.localPosition = new Vector3(0f, 0f, 0f - c_borderZOffset - num2);
			}
		}
		if (!(m_sideB != null))
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
			if (m_sideBParent != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					HighlightUtils.Get().ResizeBoundaryLine(lengthInSquares, m_sideB);
					m_sideB.transform.localPosition = new Vector3(0f, 0f, 0f - c_borderZOffset - num2);
					return;
				}
			}
			return;
		}
	}
}
