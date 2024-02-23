using System.Collections.Generic;
using System.Text;
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

	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = Mathf.Sin(5f * realtimeSinceStartup);
		m_oscillatingAlpha = 0.25f + Mathf.Clamp(num * 0.1f, -0.1f, 0.1f);
	}

	public GameObject CreateLineMesh(List<Vector3> points, float width, Color color, bool isChasing, Material material = null, GameObject previousGameObject = null, bool faceTowardCamera = false)
	{
		if (material == null)
		{
			if (isChasing)
			{
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
			gameObject = new GameObject("ArrowLine");
		}
		else
		{
			gameObject = previousGameObject;
		}
		MeshFilter meshFilter;
		if (previousGameObject == null)
		{
			meshFilter = gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
		}
		else
		{
			meshFilter = gameObject.GetComponent<MeshFilter>();
			meshFilter.mesh.Clear();
		}
		Mesh mesh = meshFilter.mesh;
		m_tempVerticesList.Clear();
		m_tempNormalsList.Clear();
		m_tempUvsList.Clear();
		List<Vector3> tempVerticesList = m_tempVerticesList;
		List<Vector3> tempNormalsList = m_tempNormalsList;
		List<Vector2> tempUvsList = m_tempUvsList;
		for (int i = 0; i < points.Count; i++)
		{
			Vector3 direction = Camera.main.WorldToRay(points[i]).direction;
			if (i == 0)
			{
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
				continue;
			}
			if (i == points.Count - 1)
			{
				Vector3 normalized2 = (points[i] - points[i - 1]).normalized;
				Vector3 normalized3;
				if (faceTowardCamera)
				{
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
				Vector2 vector2 = tempUvsList[tempUvsList.Count - 2];
				tempUvsList.Add(new Vector2(vector2.x + magnitude / 1f, 0f));
				tempVerticesList.Add(points[i] - normalized3 * width);
				tempNormalsList.Add(new Vector3(0f, 1f, 0f));
				vector = tempVerticesList[tempVerticesList.Count - 1] - tempVerticesList[tempVerticesList.Count - 3];
				vector.y = 0f;
				magnitude = vector.magnitude;
				Vector2 vector3 = tempUvsList[tempUvsList.Count - 3];
				tempUvsList.Add(new Vector2(vector3.x + magnitude / 1f, 1f));
				continue;
			}
			Vector3 normalized4 = (points[i] - points[i - 1]).normalized;
			Vector3 normalized5 = (points[i + 1] - points[i]).normalized;
			Vector3 normalized6 = (normalized4 + normalized5).normalized;
			Vector3 vector4 = (!faceTowardCamera) ? Vector3.Cross(normalized4, new Vector3(0f, 1f, 0f)).normalized : Vector3.Cross(normalized4, direction).normalized;
			Vector3 vector5;
			if (normalized6.sqrMagnitude == 0f)
			{
				vector5 = vector4;
				vector5.y = 0f;
			}
			else if (faceTowardCamera)
			{
				vector5 = Vector3.Cross(normalized6, direction);
				vector5 = vector5.normalized;
			}
			else
			{
				vector5 = Vector3.Cross(normalized6, new Vector3(0f, 1f, 0f)).normalized;
				vector5.y = 0f;
			}
			float num = Vector3.Dot(vector5, vector4);
			if (num == 0f)
			{
				num = 1f;
			}
			if (faceTowardCamera)
			{
				if ((double)num < 0.9)
				{
					continue;
				}
			}
			tempVerticesList.Add(points[i] + vector5 * width / num);
			tempNormalsList.Add(new Vector3(0f, 1f, 0f));
			Vector3 vector6 = tempVerticesList[tempVerticesList.Count - 1] - tempVerticesList[tempVerticesList.Count - 3];
			if (!faceTowardCamera)
			{
				vector6.y = 0f;
			}
			float magnitude2 = vector6.magnitude;
			Vector2 vector7 = tempUvsList[tempUvsList.Count - 2];
			tempUvsList.Add(new Vector2(vector7.x + magnitude2 / 1f, 0f));
			tempVerticesList.Add(points[i] - vector5 * width / num);
			tempNormalsList.Add(new Vector3(0f, 1f, 0f));
			vector6 = tempVerticesList[tempVerticesList.Count - 1] - tempVerticesList[tempVerticesList.Count - 3];
			if (!faceTowardCamera)
			{
				vector6.y = 0f;
			}
			magnitude2 = vector6.magnitude;
			Vector2 vector8 = tempUvsList[tempUvsList.Count - 3];
			tempUvsList.Add(new Vector2(vector8.x + magnitude2 / 1f, 1f));
			if (!faceTowardCamera)
			{
				continue;
			}
			if ((tempVerticesList[tempVerticesList.Count - 1] - tempVerticesList[tempVerticesList.Count - 3]).sqrMagnitude > (tempVerticesList[tempVerticesList.Count - 1] - tempVerticesList[tempVerticesList.Count - 4]).sqrMagnitude)
			{
				Vector3 value = tempVerticesList[tempVerticesList.Count - 1];
				tempVerticesList[tempVerticesList.Count - 1] = tempVerticesList[tempVerticesList.Count - 2];
				tempVerticesList[tempVerticesList.Count - 2] = value;
			}
		}
		while (true)
		{
			m_tempTrianglesList.Clear();
			List<int> tempTrianglesList = m_tempTrianglesList;
			for (int j = 0; j < tempVerticesList.Count - 2; j += 2)
			{
				tempTrianglesList.Add(j);
				tempTrianglesList.Add(j + 2);
				tempTrianglesList.Add(j + 1);
				tempTrianglesList.Add(j + 1);
				tempTrianglesList.Add(j + 2);
				tempTrianglesList.Add(j + 3);
			}
			while (true)
			{
				mesh.vertices = tempVerticesList.ToArray();
				mesh.normals = tempNormalsList.ToArray();
				mesh.uv = tempUvsList.ToArray();
				mesh.triangles = tempTrianglesList.ToArray();
				if (previousGameObject != null)
				{
					Object.Destroy(gameObject.GetComponent<Renderer>().material);
				}
				gameObject.GetComponent<Renderer>().material = material;
				gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", color);
				return gameObject;
			}
		}
	}

	public GameObject CreateFancyArrowMesh(ref List<Vector3> points, float width, Color color, bool isChasing, ActorData theActor, AbilityUtil_Targeter.TargeterMovementType movementType = AbilityUtil_Targeter.TargeterMovementType.Movement, Material material = null, MovementPathStart previousLine = null, bool glowOn = true, float startOffset = 0.4f, float endOffset = 0.4f)
	{
		GameObject gameObject;
		if (previousLine != null)
		{
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
			if (vector2.sqrMagnitude > 0f)
			{
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
					points[0] -= vector * Board.Get().squareSize * 0.4f;
					points[points.Count - 1] -= vector2 * Board.Get().squareSize * 0.4f;
					GameObject gameObject4 = CreateLineMesh(points, width, color, isChasing, material, previousLine.linePiece);
					previousLine.AddLinePiece(gameObject4, gameObject);
					gameObject2.transform.parent = gameObject.transform;
					gameObject3.transform.parent = gameObject.transform;
					gameObject4.transform.parent = gameObject.transform;
				}
				else
				{
					MovementPathStart movementPathStart = Object.Instantiate(HighlightUtils.Get().m_ArrowStartPrefab);
					GameObject gameObject5 = movementPathStart.gameObject;
					movementPathStart.SetColor(color);
					movementPathStart.Setup(theActor, isChasing, movementType);
					if (!glowOn)
					{
						movementPathStart.SetGlow(glowOn);
					}
					gameObject5.transform.position = points[0];
					gameObject5.transform.forward = vector;
					MovementPathEnd movementPathEnd = Object.Instantiate(HighlightUtils.Get().m_ArrowEndPrefab);
					GameObject gameObject6 = movementPathEnd.gameObject;
					movementPathEnd.SetupColor(color);
					movementPathEnd.Setup(theActor, isChasing);
					gameObject6.transform.position = points[points.Count - 1];
					gameObject6.transform.forward = vector2;
					movementPathStart.endPiece = movementPathEnd;
					points[0] -= vector * Board.Get().squareSize * startOffset;
					points[points.Count - 1] -= vector2 * Board.Get().squareSize * endOffset;
					GameObject gameObject7 = CreateLineMesh(points, width, color, isChasing, material);
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
		Mesh mesh2 = meshFilter.mesh = new Mesh();
		Vector3[] array = new Vector3[perimeterPts.Length];
		Vector3[] array2 = new Vector3[perimeterPts.Length];
		Vector2[] array3 = new Vector2[perimeterPts.Length];
		for (int i = 0; i < perimeterPts.Length; i++)
		{
			array[i] = perimeterPts[i];
			array2[i] = new Vector3(0f, 1f, 0f);
			array3[i] = Vector2.zero;
		}
		while (true)
		{
			int[] array4 = new int[(perimeterPts.Length - 2) * 3];
			for (int j = 1; j < perimeterPts.Length - 1; j++)
			{
				array4[(j - 1) * 3] = 0;
				array4[(j - 1) * 3 + 1] = j + 1;
				array4[(j - 1) * 3 + 2] = j;
			}
			while (true)
			{
				mesh2.vertices = array;
				mesh2.normals = array2;
				mesh2.uv = array3;
				mesh2.triangles = array4;
				gameObject.GetComponent<Renderer>().material = meshMaterial;
				return gameObject;
			}
		}
	}

	public static GameObject CreateMeshWithBorder(Vector3[] perimeterPts, float borderThickness, string objectName, out GameObject outlineMeshObject, Material meshMaterial, Material outlineMaterial)
	{
		Vector3 b = default(Vector3);
		for (int i = 0; i < perimeterPts.Length; i++)
		{
			b.x += perimeterPts[i].x;
			b.z += perimeterPts[i].z;
		}
		b.x /= perimeterPts.Length;
		b.z /= perimeterPts.Length;
		b.y = 0f;
		Vector3[] array = new Vector3[perimeterPts.Length];
		for (int j = 0; j < perimeterPts.Length; j++)
		{
			Vector3 vector = perimeterPts[j] - b;
			vector.y = 0f;
			vector.Normalize();
			vector *= borderThickness;
			array[j] = new Vector3(perimeterPts[j].x + vector.x, perimeterPts[j].y, perimeterPts[j].z + vector.z);
		}
		while (true)
		{
			GameObject result = CreateMesh(perimeterPts, objectName, meshMaterial);
			outlineMeshObject = new GameObject(new StringBuilder().Append(objectName).Append("_outline").ToString());
			MeshFilter meshFilter = outlineMeshObject.AddComponent<MeshFilter>();
			outlineMeshObject.AddComponent<MeshRenderer>();
			Mesh mesh2 = meshFilter.mesh = new Mesh();
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
			while (true)
			{
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
				while (true)
				{
					mesh2.vertices = array2;
					mesh2.normals = array3;
					mesh2.uv = array4;
					mesh2.triangles = array5;
					outlineMeshObject.GetComponent<Renderer>().sharedMaterial = outlineMaterial;
					return result;
				}
			}
		}
	}
}
