using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexJitter : MonoBehaviour
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
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(VertexJitter.\u0013(UnityEngine.Object)).MethodHandle;
				}
				this.\u0016 = true;
			}
		}

		private IEnumerator \u0013()
		{
			this.\u0015.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.\u0015.textInfo;
			int num = 0;
			this.\u0016 = true;
			VertexJitter.VertexAnim[] array = new VertexJitter.VertexAnim[0x400];
			for (int i = 0; i < 0x400; i++)
			{
				array[i].\u001D = UnityEngine.Random.Range(10f, 25f);
				array[i].\u0012 = UnityEngine.Random.Range(1f, 3f);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(VertexJitter.<AnimateVertexColors>c__Iterator0.MoveNext()).MethodHandle;
			}
			TMP_MeshInfo[] array2 = textInfo.CopyMeshInfoVertexData();
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
					array2 = textInfo.CopyMeshInfoVertexData();
					this.\u0016 = false;
				}
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
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
				else
				{
					for (int j = 0; j < characterCount; j++)
					{
						TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[j];
						if (tmp_CharacterInfo.isVisible)
						{
							VertexJitter.VertexAnim vertexAnim = array[j];
							int materialReferenceIndex = textInfo.characterInfo[j].materialReferenceIndex;
							int vertexIndex = textInfo.characterInfo[j].vertexIndex;
							Vector3[] vertices = array2[materialReferenceIndex].vertices;
							Vector2 v = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
							Vector3 b = v;
							Vector3[] vertices2 = textInfo.meshInfo[materialReferenceIndex].vertices;
							vertices2[vertexIndex] = vertices[vertexIndex] - b;
							vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - b;
							vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - b;
							vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - b;
							vertexAnim.\u000E = Mathf.SmoothStep(-vertexAnim.\u001D, vertexAnim.\u001D, Mathf.PingPong((float)num / 25f * vertexAnim.\u0012, 1f));
							Vector3 a = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), UnityEngine.Random.Range(-0.25f, 0.25f), 0f);
							Matrix4x4 matrix4x = Matrix4x4.TRS(a * this.\u0012, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-5f, 5f) * this.\u001D), Vector3.one);
							vertices2[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex]);
							vertices2[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
							vertices2[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
							vertices2[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
							vertices2[vertexIndex] += b;
							vertices2[vertexIndex + 1] += b;
							vertices2[vertexIndex + 2] += b;
							vertices2[vertexIndex + 3] += b;
							array[j] = vertexAnim;
						}
					}
					for (int k = 0; k < textInfo.meshInfo.Length; k++)
					{
						textInfo.meshInfo[k].mesh.vertices = textInfo.meshInfo[k].vertices;
						this.\u0015.UpdateGeometry(textInfo.meshInfo[k].mesh, k);
					}
					num++;
					yield return new WaitForSeconds(0.1f);
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			yield break;
		}

		private struct VertexAnim
		{
			public float \u001D;

			public float \u000E;

			public float \u0012;
		}
	}
}
