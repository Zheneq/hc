using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexZoom : MonoBehaviour
	{
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
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(_0013);
		}

		private void _0009()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(_0013);
		}

		private void _0019()
		{
			StartCoroutine(coroutine0013());
		}

		private void _0013(Object _001D)
		{
			if (!(_001D == _0015))
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				_0016 = true;
				return;
			}
		}

		private IEnumerator coroutine0013()
		{
			_0015.ForceMeshUpdate();
			TMP_TextInfo textInfo = _0015.textInfo;
			TMP_MeshInfo[] array = textInfo.CopyMeshInfoVertexData();
			List<float> list = new List<float>();
			List<int> list2 = new List<int>();
			_0016 = true;
			while (true)
			{
				if (_0016)
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
					array = textInfo.CopyMeshInfoVertexData();
					_0016 = false;
				}
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					break;
				}
				list.Clear();
				list2.Clear();
				for (int i = 0; i < characterCount; i++)
				{
					TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[i];
					if (!tMP_CharacterInfo.isVisible)
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
						continue;
					}
					int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = textInfo.characterInfo[i].vertexIndex;
					Vector3[] vertices = array[materialReferenceIndex].vertices;
					Vector2 v = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
					Vector3 vector = v;
					Vector3[] vertices2 = textInfo.meshInfo[materialReferenceIndex].vertices;
					vertices2[vertexIndex] = vertices[vertexIndex] - vector;
					vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - vector;
					vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - vector;
					vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - vector;
					float num = Random.Range(1f, 1.5f);
					list.Add(num);
					list2.Add(list.Count - 1);
					Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, Vector3.one * num);
					vertices2[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex]);
					vertices2[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
					vertices2[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
					vertices2[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
					vertices2[vertexIndex] += vector;
					vertices2[vertexIndex + 1] += vector;
					vertices2[vertexIndex + 2] += vector;
					vertices2[vertexIndex + 3] += vector;
					Vector2[] uvs = array[materialReferenceIndex].uvs0;
					Vector2[] uvs2 = textInfo.meshInfo[materialReferenceIndex].uvs0;
					uvs2[vertexIndex] = uvs[vertexIndex];
					uvs2[vertexIndex + 1] = uvs[vertexIndex + 1];
					uvs2[vertexIndex + 2] = uvs[vertexIndex + 2];
					uvs2[vertexIndex + 3] = uvs[vertexIndex + 3];
					Color32[] colors = array[materialReferenceIndex].colors32;
					Color32[] colors2 = textInfo.meshInfo[materialReferenceIndex].colors32;
					colors2[vertexIndex] = colors[vertexIndex];
					colors2[vertexIndex + 1] = colors[vertexIndex + 1];
					colors2[vertexIndex + 2] = colors[vertexIndex + 2];
					colors2[vertexIndex + 3] = colors[vertexIndex + 3];
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int j = 0; j < textInfo.meshInfo.Length; j++)
				{
					list2.Sort((int _001D, int _000E) => list[_001D].CompareTo(list[_000E]));
					textInfo.meshInfo[j].SortGeometry(list2);
					textInfo.meshInfo[j].mesh.vertices = textInfo.meshInfo[j].vertices;
					textInfo.meshInfo[j].mesh.uv = textInfo.meshInfo[j].uvs0;
					textInfo.meshInfo[j].mesh.colors32 = textInfo.meshInfo[j].colors32;
					_0015.UpdateGeometry(textInfo.meshInfo[j].mesh, j);
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				yield return new WaitForSeconds(0.25f);
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
