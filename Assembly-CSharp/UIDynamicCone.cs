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

	private const int c_coneAnglePerPiece = 0xA;

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
		this.m_maxConePieces = 0x24;
		if (this.m_maxConePieces * 0xA < 0x168)
		{
			this.m_maxConePieces++;
		}
		if (!this.m_emptyCone)
		{
			this.m_createdConeObject = this.CreateConeMesh(this.m_initialConeWidthAngle, this.m_initialConeRadius, this.m_coneMaterial);
		}
		if (this.m_showBorder)
		{
			this.m_maxBorderPieces = 0x48;
			if (this.m_maxBorderPieces * 5 < 0x168)
			{
				this.m_maxBorderPieces++;
			}
			this.m_createBorderObject = this.CreateBorderConeMesh(this.m_initialConeWidthAngle, this.m_initialConeRadius, this.m_borderMaterial);
		}
		this.m_sideA = HighlightUtils.Get().CreateBoundaryLine(this.m_initialConeRadius, true, true);
		this.m_sideB = HighlightUtils.Get().CreateBoundaryLine(this.m_initialConeRadius, true, false);
		if (this.m_sideA != null)
		{
			this.m_sideAParent = new GameObject();
			this.m_sideA.transform.parent = this.m_sideAParent.transform;
			this.m_sideA.transform.localPosition = new Vector3(0f, 0f, -this.c_borderZOffset);
			this.SetAsChild(this.m_sideAParent);
		}
		if (this.m_sideB != null)
		{
			this.m_sideBParent = new GameObject();
			this.m_sideB.transform.parent = this.m_sideBParent.transform;
			this.m_sideB.transform.localPosition = new Vector3(0f, 0f, -this.c_borderZOffset);
			this.SetAsChild(this.m_sideBParent);
		}
	}

	private void OnDestroy()
	{
		if (this.m_createdConeObject != null)
		{
			MeshFilter component = this.m_createdConeObject.GetComponent<MeshFilter>();
			if (component != null)
			{
				if (component.sharedMesh != null)
				{
					UnityEngine.Object.Destroy(component.sharedMesh);
				}
			}
		}
		if (this.m_createBorderObject != null)
		{
			MeshFilter component2 = this.m_createBorderObject.GetComponent<MeshFilter>();
			if (component2 != null)
			{
				if (component2.sharedMesh != null)
				{
					UnityEngine.Object.Destroy(component2.sharedMesh);
				}
			}
		}
	}

	private void SetAsChild(GameObject child)
	{
		if (child != null)
		{
			child.transform.parent = base.transform;
			child.transform.localPosition = Vector3.zero;
			child.transform.localRotation = Quaternion.identity;
		}
	}

	public GameObject CreateConeMesh(float coneWidthAngle, float coneRadius, Material material)
	{
		GameObject gameObject = new GameObject("ConeMeshObject");
		this.SetAsChild(gameObject);
		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
		component.shadowCastingMode = ShadowCastingMode.Off;
		component.reflectionProbeUsage = ReflectionProbeUsage.Off;
		component.receiveShadows = false;
		component.lightProbeUsage = LightProbeUsage.Off;
		MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
		this.m_coneMesh = component2.mesh;
		this.m_coneMesh.Clear();
		int num = 2 + this.m_maxConePieces;
		Vector3[] array = new Vector3[num];
		Vector2[] array2 = new Vector2[num];
		Vector3[] array3 = new Vector3[num];
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i] = new Vector3(0f, -1f, 0f);
		}
		array[0] = Vector3.zero;
		array2[0] = new Vector2(0.5f, 0.5f);
		int[] array4 = new int[3 * this.m_maxConePieces];
		this.m_coneMesh.vertices = array;
		this.m_coneMesh.uv = array2;
		this.AdjustConeMeshVertices(coneWidthAngle, coneRadius);
		for (int j = 0; j < this.m_maxConePieces; j++)
		{
			int num2 = j * 3;
			int num3 = 1 + j;
			int num4 = 1 + (j + 1);
			array4[num2] = 0;
			array4[num2 + 1] = num4;
			array4[num2 + 2] = num3;
		}
		this.m_coneMesh.triangles = array4;
		this.m_coneMesh.normals = array3;
		component.material = material;
		this.m_coneMesh.bounds = new Bounds(gameObject.transform.parent.position, Vector3.one * 200f);
		return gameObject;
	}

	public void AdjustConeMeshVertices(float coneWidthAngle, float coneRadius)
	{
		coneWidthAngle = Mathf.Max(0f, coneWidthAngle);
		coneRadius = Mathf.Max(0f, coneRadius);
		if (Mathf.Approximately(coneWidthAngle, this.m_currentAngleInWorld))
		{
			if (Mathf.Approximately(coneRadius, this.m_currentRadiusInWorld))
			{
				return;
			}
		}
		this.m_currentRadiusInWorld = coneRadius;
		this.m_currentAngleInWorld = coneWidthAngle;
		float num = 0.5f * coneWidthAngle;
		if (this.m_coneMesh != null)
		{
			Vector3[] vertices = this.m_coneMesh.vertices;
			Vector2[] uv = this.m_coneMesh.uv;
			float d = coneRadius;
			float num2 = 0f;
			for (int i = 0; i <= this.m_maxConePieces; i++)
			{
				float f = (90f + num2 - num) * 0.0174532924f;
				float num3 = Mathf.Cos(f);
				float num4 = Mathf.Sin(f);
				Vector3 a = new Vector3(num3, 0f, num4);
				vertices[1 + i] = a * d;
				uv[1 + i] = new Vector2(this.m_coneUvCenterX + this.m_coneUvRadius * num3, this.m_coneUvCenterY + this.m_coneUvRadius * num4);
				num2 = Mathf.Min(coneWidthAngle, num2 + 10f);
			}
			this.m_coneMesh.vertices = vertices;
			this.m_coneMesh.uv = uv;
		}
		if (this.m_showBorder)
		{
			this.AdjustConeBorderMeshVertices(coneWidthAngle, coneRadius);
		}
		if (coneWidthAngle < 360f)
		{
			this.SetBorderStartOffset(this.m_borderStartOffsetInSquares);
			if (this.m_sideA != null)
			{
				if (this.m_sideAParent != null)
				{
					this.m_sideAParent.transform.localRotation = Quaternion.LookRotation(-1f * VectorUtils.AngleDegreesToVector(90f + num));
				}
			}
			if (this.m_sideB != null && this.m_sideBParent != null)
			{
				this.m_sideBParent.transform.localRotation = Quaternion.LookRotation(-1f * VectorUtils.AngleDegreesToVector(90f - num));
			}
			this.SetSidesActive(!this.m_forceHideSides);
		}
		else
		{
			this.SetSidesActive(false);
		}
	}

	public void SetConeObjectActive(bool active)
	{
		if (this.m_createdConeObject != null)
		{
			UIManager.SetGameObjectActive(this.m_createdConeObject, active, null);
		}
	}

	public GameObject CreateBorderConeMesh(float coneWidthAngle, float coneRadius, Material material)
	{
		GameObject gameObject = new GameObject("ConeBorderMeshObject");
		this.SetAsChild(gameObject);
		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
		component.shadowCastingMode = ShadowCastingMode.Off;
		component.reflectionProbeUsage = ReflectionProbeUsage.Off;
		component.receiveShadows = false;
		component.lightProbeUsage = LightProbeUsage.Off;
		MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
		this.m_borderMesh = component2.mesh;
		this.m_borderMesh.Clear();
		int num = 2 + 2 * this.m_maxBorderPieces;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		Vector3[] array = new Vector3[num];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new Vector3(0f, -1f, 0f);
		}
		int[] array2 = new int[6 * this.m_maxBorderPieces];
		this.m_borderMesh.vertices = vertices;
		this.m_borderMesh.uv = uv;
		this.AdjustConeBorderMeshVertices(coneWidthAngle, coneRadius);
		for (int j = 0; j < this.m_maxBorderPieces; j++)
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
		this.m_borderMesh.triangles = array2;
		this.m_borderMesh.normals = array;
		component.material = material;
		this.m_borderMesh.bounds = new Bounds(gameObject.transform.parent.position, Vector3.one * 200f);
		return gameObject;
	}

	public void AdjustConeBorderMeshVertices(float coneWidthAngle, float coneRadius)
	{
		coneWidthAngle = Mathf.Max(0f, coneWidthAngle);
		coneRadius = Mathf.Max(0f, coneRadius);
		if (this.m_borderMesh != null)
		{
			Vector3[] vertices = this.m_borderMesh.vertices;
			Vector2[] uv = this.m_borderMesh.uv;
			float num = 0.5f * coneWidthAngle;
			float d = Mathf.Max(0f, coneRadius - this.m_borderThickness);
			float d2 = coneRadius;
			float num2 = 0f;
			for (int i = 0; i <= this.m_maxBorderPieces; i++)
			{
				float f = (90f + num2 - num) * 0.0174532924f;
				float num3 = Mathf.Cos(f);
				float num4 = Mathf.Sin(f);
				Vector3 a = new Vector3(num3, 0f, num4);
				vertices[2 * i] = a * d;
				vertices[2 * i + 1] = a * d2;
				float num5 = this.m_borderUvRadius * this.m_innerTextureUv;
				float num6 = this.m_borderUvRadius * this.m_outerTextureUv;
				uv[2 * i] = new Vector2(this.m_borderUvCenterX + num5 * num3, this.m_borderUvCenterY + num5 * num4);
				uv[2 * i + 1] = new Vector2(this.m_borderUvCenterX + num6 * num3, this.m_borderUvCenterY + num6 * num4);
				num2 = Mathf.Min(coneWidthAngle, num2 + 5f);
			}
			this.m_borderMesh.vertices = vertices;
			this.m_borderMesh.uv = uv;
		}
	}

	public void SetSidesActive(bool active)
	{
		if (this.m_sideA != null && this.m_sideA.activeSelf != active)
		{
			UIManager.SetGameObjectActive(this.m_sideA, active, null);
		}
		if (this.m_sideB != null && this.m_sideB.activeSelf != active)
		{
			UIManager.SetGameObjectActive(this.m_sideB, active, null);
		}
		this.m_currentSidesActive = active;
	}

	public void SetForceHideSides(bool forceHide)
	{
		this.m_forceHideSides = forceHide;
	}

	public void SetBorderActive(bool active)
	{
		if (this.m_createBorderObject != null)
		{
			if (this.m_createBorderObject.activeSelf != active)
			{
				this.m_createBorderObject.SetActive(active);
			}
		}
		this.m_currentBorderActive = active;
	}

	public void SetBorderStartOffset(float startOffsetInSquares)
	{
		this.m_borderStartOffsetInSquares = startOffsetInSquares;
		float num = Mathf.Max(0f, this.m_currentRadiusInWorld - this.c_borderZOffset) / Board.Get().squareSize;
		float num2 = this.m_borderStartOffsetInSquares * Board.Get().squareSize;
		float lengthInSquares = Mathf.Max(0f, num - this.m_borderStartOffsetInSquares);
		if (this.m_sideA != null)
		{
			if (this.m_sideAParent != null)
			{
				HighlightUtils.Get().ResizeBoundaryLine(lengthInSquares, this.m_sideA);
				this.m_sideA.transform.localPosition = new Vector3(0f, 0f, -this.c_borderZOffset - num2);
			}
		}
		if (this.m_sideB != null)
		{
			if (this.m_sideBParent != null)
			{
				HighlightUtils.Get().ResizeBoundaryLine(lengthInSquares, this.m_sideB);
				this.m_sideB.transform.localPosition = new Vector3(0f, 0f, -this.c_borderZOffset - num2);
			}
		}
	}
}
