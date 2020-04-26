using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexJitter : MonoBehaviour
	{
		private struct VertexAnim
		{
			public float _001D;

			public float _000E;

			public float _0012;
		}

		public float _001D = 1f;

		public float _000E = 1f;

		public float _0012 = 1f;

		private TMP_Text _0015;

		private bool _0016;

		private void _0013()
		{
			_0015 = GetComponent<TMP_Text>();
		}

		private void _0018()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(coroutine0013);
		}

		private void _0009()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(coroutine0013);
		}

		private void _0019()
		{
			StartCoroutine(coroutine0013_2());
		}

		private void coroutine0013(Object _001D)
		{
			if (!(_001D == _0015))
			{
				return;
			}
			while (true)
			{
				_0016 = true;
				return;
			}
		}

		private IEnumerator coroutine0013_2()
		{
			_0015.ForceMeshUpdate();
			TMP_TextInfo textInfo = _0015.textInfo;
			int num = 0;
			_0016 = true;
			VertexAnim[] array = new VertexAnim[1024];
			for (int i = 0; i < 1024; i++)
			{
				array[i]._001D = Random.Range(10f, 25f);
				array[i]._0012 = Random.Range(1f, 3f);
			}
			while (true)
			{
				TMP_MeshInfo[] array2 = textInfo.CopyMeshInfoVertexData();
				if (_0016)
				{
					array2 = textInfo.CopyMeshInfoVertexData();
					_0016 = false;
				}
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
					/*Error: Unable to find new state assignment for yield return*/;
				}
				for (int j = 0; j < characterCount; j++)
				{
					TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[j];
					if (tMP_CharacterInfo.isVisible)
					{
						VertexAnim vertexAnim = array[j];
						int materialReferenceIndex = textInfo.characterInfo[j].materialReferenceIndex;
						int vertexIndex = textInfo.characterInfo[j].vertexIndex;
						Vector3[] vertices = array2[materialReferenceIndex].vertices;
						Vector2 v = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
						Vector3 vector = v;
						Vector3[] vertices2 = textInfo.meshInfo[materialReferenceIndex].vertices;
						vertices2[vertexIndex] = vertices[vertexIndex] - vector;
						vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - vector;
						vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - vector;
						vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - vector;
						vertexAnim._000E = Mathf.SmoothStep(0f - vertexAnim._001D, vertexAnim._001D, Mathf.PingPong((float)num / 25f * vertexAnim._0012, 1f));
						Vector3 a = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0f);
						Matrix4x4 matrix4x = Matrix4x4.TRS(a * _0012, Quaternion.Euler(0f, 0f, Random.Range(-5f, 5f) * _001D), Vector3.one);
						vertices2[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex]);
						vertices2[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
						vertices2[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
						vertices2[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
						vertices2[vertexIndex] += vector;
						vertices2[vertexIndex + 1] += vector;
						vertices2[vertexIndex + 2] += vector;
						vertices2[vertexIndex + 3] += vector;
						array[j] = vertexAnim;
					}
				}
				for (int k = 0; k < textInfo.meshInfo.Length; k++)
				{
					textInfo.meshInfo[k].mesh.vertices = textInfo.meshInfo[k].vertices;
					_0015.UpdateGeometry(textInfo.meshInfo[k].mesh, k);
				}
				num++;
				yield return new WaitForSeconds(0.1f);
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
