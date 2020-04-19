using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexZoom : MonoBehaviour
	{
		public float \u001D = 1f;

		public float \u000E = 1f;

		public float \u0012 = 1f;

		private TMP_Text \u0015;

		private bool \u0016;

		private void \u0013()
		{
			this.\u0015 = base.GetComponent<TMP_Text>();
		}

		private void \u0018()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.\u0013));
		}

		private void \u0009()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.\u0013));
		}

		private void \u0019()
		{
			base.StartCoroutine(this.\u0013());
		}

		private void \u0013(UnityEngine.Object \u001D)
		{
			if (\u001D == this.\u0015)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(VertexZoom.\u0013(UnityEngine.Object)).MethodHandle;
				}
				this.\u0016 = true;
			}
		}

		private IEnumerator \u0013()
		{
			VertexZoom.<AnimateVertexColors>c__Iterator0.<AnimateVertexColors>c__AnonStorey1 <AnimateVertexColors>c__AnonStorey = new VertexZoom.<AnimateVertexColors>c__Iterator0.<AnimateVertexColors>c__AnonStorey1();
			<AnimateVertexColors>c__AnonStorey.\u000E = this;
			this.\u0015.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.\u0015.textInfo;
			TMP_MeshInfo[] array = textInfo.CopyMeshInfoVertexData();
			<AnimateVertexColors>c__AnonStorey.\u001D = new List<float>();
			List<int> list = new List<int>();
			this.\u0016 = true;
			for (;;)
			{
				if (this.\u0016)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(VertexZoom.<AnimateVertexColors>c__Iterator0.MoveNext()).MethodHandle;
					}
					array = textInfo.CopyMeshInfoVertexData();
					this.\u0016 = false;
				}
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
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
					yield return new WaitForSeconds(0.25f);
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else
				{
					<AnimateVertexColors>c__AnonStorey.\u001D.Clear();
					list.Clear();
					for (int i = 0; i < characterCount; i++)
					{
						TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[i];
						if (!tmp_CharacterInfo.isVisible)
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
						}
						else
						{
							int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
							int vertexIndex = textInfo.characterInfo[i].vertexIndex;
							Vector3[] vertices = array[materialReferenceIndex].vertices;
							Vector2 v = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
							Vector3 b = v;
							Vector3[] vertices2 = textInfo.meshInfo[materialReferenceIndex].vertices;
							vertices2[vertexIndex] = vertices[vertexIndex] - b;
							vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - b;
							vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - b;
							vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - b;
							float num = UnityEngine.Random.Range(1f, 1.5f);
							<AnimateVertexColors>c__AnonStorey.\u001D.Add(num);
							list.Add(<AnimateVertexColors>c__AnonStorey.\u001D.Count - 1);
							Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, Vector3.one * num);
							vertices2[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex]);
							vertices2[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
							vertices2[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
							vertices2[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
							vertices2[vertexIndex] += b;
							vertices2[vertexIndex + 1] += b;
							vertices2[vertexIndex + 2] += b;
							vertices2[vertexIndex + 3] += b;
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
					for (int j = 0; j < textInfo.meshInfo.Length; j++)
					{
						list.Sort(new Comparison<int>(<AnimateVertexColors>c__AnonStorey.\u0012));
						textInfo.meshInfo[j].SortGeometry(list);
						textInfo.meshInfo[j].mesh.vertices = textInfo.meshInfo[j].vertices;
						textInfo.meshInfo[j].mesh.uv = textInfo.meshInfo[j].uvs0;
						textInfo.meshInfo[j].mesh.colors32 = textInfo.meshInfo[j].colors32;
						this.\u0015.UpdateGeometry(textInfo.meshInfo[j].mesh, j);
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
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}
	}
}
