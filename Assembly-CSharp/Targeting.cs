using System;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
	public Shader m_transparentUnlitShader;

	public float m_oscillatingAlpha = 0.25f;

	private static Targeting s_instance;

	private List<Vector3> m_tempVerticesList = new List<Vector3>();

	private List<Vector3> m_tempNormalsList = new List<Vector3>();

	private List<Vector2> m_tempUvsList = new List<Vector2>();

	private List<int> m_tempTrianglesList = new List<int>();

	public static Targeting GetTargeting()
	{
		return Targeting.s_instance;
	}

	private void Awake()
	{
		Targeting.s_instance = this;
	}

	private void OnDestroy()
	{
		Targeting.s_instance = null;
	}

	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = Mathf.Sin(5f * realtimeSinceStartup);
		this.m_oscillatingAlpha = 0.25f + Mathf.Clamp(num * 0.1f, -0.1f, 0.1f);
	}

	public GameObject CreateLineMesh(List<Vector3> points, float width, Color color, bool isChasing, Material material = null, GameObject previousGameObject = null, bool faceTowardCamera = false)
	{
		if (material == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Targeting.CreateLineMesh(List<Vector3>, float, Color, bool, Material, GameObject, bool)).MethodHandle;
			}
			if (isChasing)
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
				material = HighlightUtils.Get().m_ArrowChaseLineMaterial;
			}
			else
			{
				material = HighlightUtils.Get().m_ArrowLineMaterial;
			}
		}
		GameObject gameObject;
		if (previousGameObject == null)
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
			gameObject = new GameObject("ArrowLine");
		}
		else
		{
			gameObject = previousGameObject;
		}
		MeshFilter meshFilter;
		if (previousGameObject == null)
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
			meshFilter = gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
		}
		else
		{
			meshFilter = gameObject.GetComponent<MeshFilter>();
			meshFilter.mesh.Clear();
		}
		Mesh mesh = meshFilter.mesh;
		this.m_tempVerticesList.Clear();
		this.m_tempNormalsList.Clear();
		this.m_tempUvsList.Clear();
		List<Vector3> tempVerticesList = this.m_tempVerticesList;
		List<Vector3> tempNormalsList = this.m_tempNormalsList;
		List<Vector2> tempUvsList = this.m_tempUvsList;
		for (int i = 0; i < points.Count; i++)
		{
			Vector3 direction = Camera.main.WorldToRay(points[i]).direction;
			if (i == 0)
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
				Vector3 lhs = points[i + 1] - points[i];
				Vector3 normalized;
				if (faceTowardCamera)
				{
					normalized = Vector3.Cross(lhs, direction).normalized;
				}
				else
				{
					normalized = Vector3.Cross(lhs, new Vector3(0f, 1f, 0f)).normalized;
					normalized.y = 0f;
				}
				tempVerticesList.Add(points[i] + normalized * width);
				tempNormalsList.Add(new Vector3(0f, 1f, 0f));
				tempUvsList.Add(new Vector2(0f, 0f));
				tempVerticesList.Add(points[i] - normalized * width);
				tempNormalsList.Add(new Vector3(0f, 1f, 0f));
				tempUvsList.Add(new Vector2(0f, 1f));
			}
			else if (i == points.Count - 1)
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
				Vector3 normalized2 = (points[i] - points[i - 1]).normalized;
				Vector3 normalized3;
				if (faceTowardCamera)
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
					normalized3 = Vector3.Cross(normalized2, direction).normalized;
				}
				else
				{
					normalized3 = Vector3.Cross(normalized2, new Vector3(0f, 1f, 0f)).normalized;
					normalized3.y = 0f;
				}
				tempVerticesList.Add(points[i] + normalized3 * width);
				tempNormalsList.Add(new Vector3(0f, 1f, 0f));
				Vector3 vector = tempVerticesList[tempVerticesList.Count - 1] - tempVerticesList[tempVerticesList.Count - 3];
				vector.y = 0f;
				float magnitude = vector.magnitude;
				tempUvsList.Add(new Vector2(tempUvsList[tempUvsList.Count - 2].x + magnitude / 1f, 0f));
				tempVerticesList.Add(points[i] - normalized3 * width);
				tempNormalsList.Add(new Vector3(0f, 1f, 0f));
				vector = tempVerticesList[tempVerticesList.Count - 1] - tempVerticesList[tempVerticesList.Count - 3];
				vector.y = 0f;
				magnitude = vector.magnitude;
				tempUvsList.Add(new Vector2(tempUvsList[tempUvsList.Count - 3].x + magnitude / 1f, 1f));
			}
			else
			{
				Vector3 normalized4 = (points[i] - points[i - 1]).normalized;
				Vector3 normalized5 = (points[i + 1] - points[i]).normalized;
				Vector3 normalized6 = (normalized4 + normalized5).normalized;
				Vector3 normalized7;
				if (faceTowardCamera)
				{
					normalized7 = Vector3.Cross(normalized4, direction).normalized;
				}
				else
				{
					normalized7 = Vector3.Cross(normalized4, new Vector3(0f, 1f, 0f)).normalized;
				}
				Vector3 vector2;
				if (normalized6.sqrMagnitude == 0f)
				{
					vector2 = normalized7;
					vector2.y = 0f;
				}
				else if (faceTowardCamera)
				{
					vector2 = Vector3.Cross(normalized6, direction).normalized;
				}
				else
				{
					vector2 = Vector3.Cross(normalized6, new Vector3(0f, 1f, 0f)).normalized;
					vector2.y = 0f;
				}
				float num = Vector3.Dot(vector2, normalized7);
				if (num == 0f)
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
					num = 1f;
				}
				if (faceTowardCamera)
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
					if ((double)num < 0.9)
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
						goto IL_809;
					}
				}
				tempVerticesList.Add(points[i] + vector2 * width / num);
				tempNormalsList.Add(new Vector3(0f, 1f, 0f));
				Vector3 vector3 = tempVerticesList[tempVerticesList.Count - 1] - tempVerticesList[tempVerticesList.Count - 3];
				if (!faceTowardCamera)
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
					vector3.y = 0f;
				}
				float magnitude2 = vector3.magnitude;
				tempUvsList.Add(new Vector2(tempUvsList[tempUvsList.Count - 2].x + magnitude2 / 1f, 0f));
				tempVerticesList.Add(points[i] - vector2 * width / num);
				tempNormalsList.Add(new Vector3(0f, 1f, 0f));
				vector3 = tempVerticesList[tempVerticesList.Count - 1] - tempVerticesList[tempVerticesList.Count - 3];
				if (!faceTowardCamera)
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
					vector3.y = 0f;
				}
				magnitude2 = vector3.magnitude;
				tempUvsList.Add(new Vector2(tempUvsList[tempUvsList.Count - 3].x + magnitude2 / 1f, 1f));
				if (faceTowardCamera)
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
					if ((tempVerticesList[tempVerticesList.Count - 1] - tempVerticesList[tempVerticesList.Count - 3]).sqrMagnitude > (tempVerticesList[tempVerticesList.Count - 1] - tempVerticesList[tempVerticesList.Count - 4]).sqrMagnitude)
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
						Vector3 value = tempVerticesList[tempVerticesList.Count - 1];
						tempVerticesList[tempVerticesList.Count - 1] = tempVerticesList[tempVerticesList.Count - 2];
						tempVerticesList[tempVerticesList.Count - 2] = value;
					}
				}
			}
			IL_809:;
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		this.m_tempTrianglesList.Clear();
		List<int> tempTrianglesList = this.m_tempTrianglesList;
		for (int j = 0; j < tempVerticesList.Count - 2; j += 2)
		{
			tempTrianglesList.Add(j);
			tempTrianglesList.Add(j + 2);
			tempTrianglesList.Add(j + 1);
			tempTrianglesList.Add(j + 1);
			tempTrianglesList.Add(j + 2);
			tempTrianglesList.Add(j + 3);
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		mesh.vertices = tempVerticesList.ToArray();
		mesh.normals = tempNormalsList.ToArray();
		mesh.uv = tempUvsList.ToArray();
		mesh.triangles = tempTrianglesList.ToArray();
		if (previousGameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject.GetComponent<Renderer>().material);
		}
		gameObject.GetComponent<Renderer>().material = material;
		gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", color);
		return gameObject;
	}

	public unsafe GameObject CreateFancyArrowMesh(ref List<Vector3> points, float width, Color color, bool isChasing, ActorData theActor, AbilityUtil_Targeter.TargeterMovementType movementType = AbilityUtil_Targeter.TargeterMovementType.Movement, Material material = null, MovementPathStart previousLine = null, bool glowOn = true, float startOffset = 0.4f, float endOffset = 0.4f)
	{
		GameObject gameObject;
		if (previousLine != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Targeting.CreateFancyArrowMesh(List<Vector3>*, float, Color, bool, ActorData, AbilityUtil_Targeter.TargeterMovementType, Material, MovementPathStart, bool, float, float)).MethodHandle;
			}
			gameObject = previousLine.gameObject.transform.parent.gameObject;
		}
		else
		{
			gameObject = new GameObject("Arrow");
			gameObject.AddComponent<MovementPathParent>();
		}
		Vector3 vector = points[0] - points[1];
		Vector3 vector2 = points[points.Count - 1] - points[points.Count - 2];
		if (vector.sqrMagnitude > 0f)
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
			if (vector2.sqrMagnitude > 0f)
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
				vector.y = 0f;
				vector2.y = 0f;
				vector.Normalize();
				vector2.Normalize();
				if (previousLine != null)
				{
					GameObject gameObject2 = previousLine.gameObject;
					previousLine.SetColor(color);
					previousLine.Setup(theActor, isChasing, movementType);
					if (!glowOn)
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
						previousLine.SetGlow(glowOn);
					}
					gameObject2.transform.position = points[0];
					gameObject2.transform.forward = vector;
					MovementPathEnd endPiece = previousLine.endPiece;
					GameObject gameObject3 = endPiece.gameObject;
					endPiece.SetupColor(color);
					endPiece.Setup(theActor, isChasing);
					gameObject3.transform.position = points[points.Count - 1];
					gameObject3.transform.forward = vector2;
					List<Vector3> list;
					(list = points)[0] = list[0] - vector * Board.\u000E().squareSize * 0.4f;
					int index;
					(list = points)[index = points.Count - 1] = list[index] - vector2 * Board.\u000E().squareSize * 0.4f;
					GameObject gameObject4 = this.CreateLineMesh(points, width, color, isChasing, material, previousLine.linePiece, false);
					previousLine.AddLinePiece(gameObject4, gameObject);
					gameObject2.transform.parent = gameObject.transform;
					gameObject3.transform.parent = gameObject.transform;
					gameObject4.transform.parent = gameObject.transform;
				}
				else
				{
					MovementPathStart movementPathStart = UnityEngine.Object.Instantiate<MovementPathStart>(HighlightUtils.Get().m_ArrowStartPrefab);
					GameObject gameObject5 = movementPathStart.gameObject;
					movementPathStart.SetColor(color);
					movementPathStart.Setup(theActor, isChasing, movementType);
					if (!glowOn)
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
						movementPathStart.SetGlow(glowOn);
					}
					gameObject5.transform.position = points[0];
					gameObject5.transform.forward = vector;
					MovementPathEnd movementPathEnd = UnityEngine.Object.Instantiate<MovementPathEnd>(HighlightUtils.Get().m_ArrowEndPrefab);
					GameObject gameObject6 = movementPathEnd.gameObject;
					movementPathEnd.SetupColor(color);
					movementPathEnd.Setup(theActor, isChasing);
					gameObject6.transform.position = points[points.Count - 1];
					gameObject6.transform.forward = vector2;
					movementPathStart.endPiece = movementPathEnd;
					List<Vector3> list;
					(list = points)[0] = list[0] - vector * Board.\u000E().squareSize * startOffset;
					int index2;
					(list = points)[index2 = points.Count - 1] = list[index2] - vector2 * Board.\u000E().squareSize * endOffset;
					GameObject gameObject7 = this.CreateLineMesh(points, width, color, isChasing, material, null, false);
					movementPathStart.AddLinePiece(gameObject7, gameObject);
					gameObject5.transform.parent = gameObject.transform;
					gameObject6.transform.parent = gameObject.transform;
					gameObject7.transform.parent = gameObject.transform;
				}
			}
		}
		return gameObject;
	}

	public static GameObject CreateMesh(Vector3[] perimeterPts, string objectName, Material meshMaterial)
	{
		GameObject gameObject = new GameObject(objectName);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		Vector3[] array = new Vector3[perimeterPts.Length];
		Vector3[] array2 = new Vector3[perimeterPts.Length];
		Vector2[] array3 = new Vector2[perimeterPts.Length];
		for (int i = 0; i < perimeterPts.Length; i++)
		{
			array[i] = perimeterPts[i];
			array2[i] = new Vector3(0f, 1f, 0f);
			array3[i] = Vector2.zero;
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(Targeting.CreateMesh(Vector3[], string, Material)).MethodHandle;
		}
		int[] array4 = new int[(perimeterPts.Length - 2) * 3];
		for (int j = 1; j < perimeterPts.Length - 1; j++)
		{
			array4[(j - 1) * 3] = 0;
			array4[(j - 1) * 3 + 1] = j + 1;
			array4[(j - 1) * 3 + 2] = j;
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		mesh.vertices = array;
		mesh.normals = array2;
		mesh.uv = array3;
		mesh.triangles = array4;
		gameObject.GetComponent<Renderer>().material = meshMaterial;
		return gameObject;
	}

	public unsafe static GameObject CreateMeshWithBorder(Vector3[] perimeterPts, float borderThickness, string objectName, out GameObject outlineMeshObject, Material meshMaterial, Material outlineMaterial)
	{
		Vector3 b = default(Vector3);
		for (int i = 0; i < perimeterPts.Length; i++)
		{
			b.x += perimeterPts[i].x;
			b.z += perimeterPts[i].z;
		}
		b.x /= (float)perimeterPts.Length;
		b.z /= (float)perimeterPts.Length;
		b.y = 0f;
		Vector3[] array = new Vector3[perimeterPts.Length];
		for (int j = 0; j < perimeterPts.Length; j++)
		{
			Vector3 a = perimeterPts[j] - b;
			a.y = 0f;
			a.Normalize();
			a *= borderThickness;
			array[j] = new Vector3(perimeterPts[j].x + a.x, perimeterPts[j].y, perimeterPts[j].z + a.z);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(Targeting.CreateMeshWithBorder(Vector3[], float, string, GameObject*, Material, Material)).MethodHandle;
		}
		GameObject result = Targeting.CreateMesh(perimeterPts, objectName, meshMaterial);
		outlineMeshObject = new GameObject(objectName + "_outline");
		MeshFilter meshFilter = outlineMeshObject.AddComponent<MeshFilter>();
		outlineMeshObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		Vector3[] array2 = new Vector3[perimeterPts.Length * 2];
		Vector3[] array3 = new Vector3[perimeterPts.Length * 2];
		Vector2[] array4 = new Vector2[perimeterPts.Length * 2];
		for (int k = 0; k < perimeterPts.Length; k++)
		{
			array2[k] = new Vector3(perimeterPts[k].x, perimeterPts[k].y, perimeterPts[k].z);
			array2[4 + k] = new Vector3(array[k].x, array[k].y, array[k].z);
			array3[k] = new Vector3(0f, 1f, 0f);
			array3[4 + k] = new Vector3(0f, 1f, 0f);
			array4[k] = Vector2.zero;
			array4[4 + k] = Vector2.zero;
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		int[] array5 = new int[perimeterPts.Length * 2 * 3];
		for (int l = 0; l < perimeterPts.Length; l++)
		{
			int num = l;
			int num2 = (l + 1) % perimeterPts.Length;
			int num3 = l * 2 * 3;
			array5[num3] = num;
			array5[num3 + 1] = 4 + num2;
			array5[num3 + 2] = 4 + num;
			array5[num3 + 3] = num;
			array5[num3 + 4] = num2;
			array5[num3 + 5] = 4 + num2;
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		mesh.vertices = array2;
		mesh.normals = array3;
		mesh.uv = array4;
		mesh.triangles = array5;
		outlineMeshObject.GetComponent<Renderer>().sharedMaterial = outlineMaterial;
		return result;
	}
}
