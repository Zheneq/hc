using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexJitter : MonoBehaviour
	{
		public float symbol_001D = 1f;

		public float symbol_000E = 1f;

		public float symbol_0012 = 1f;

		private TMP_Text symbol_0015;

		private bool symbol_0016;

		private void symbol_0013()
		{
			this.symbol_0015 = base.GetComponent<TMP_Text>();
		}

		private void symbol_0018()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.coroutine0013));
		}

		private void symbol_0009()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.coroutine0013));
		}

		private void symbol_0019()
		{
			base.StartCoroutine(this.coroutine0013_2());
		}

		private void coroutine0013(UnityEngine.Object symbol_001D)
		{
			if (symbol_001D == this.symbol_0015)
			{
				this.symbol_0016 = true;
			}
		}

		private IEnumerator coroutine0013_2()
		{
			this.symbol_0015.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.symbol_0015.textInfo;
			int num = 0;
			this.symbol_0016 = true;
			VertexJitter.VertexAnim[] array = new VertexJitter.VertexAnim[0x400];
			for (int i = 0; i < 0x400; i++)
			{
				array[i].symbol_001D = UnityEngine.Random.Range(10f, 25f);
				array[i].symbol_0012 = UnityEngine.Random.Range(1f, 3f);
			}
			TMP_MeshInfo[] array2 = textInfo.CopyMeshInfoVertexData();
			for (;;)
			{
				if (this.symbol_0016)
				{
					array2 = textInfo.CopyMeshInfoVertexData();
					this.symbol_0016 = false;
				}
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
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
							vertexAnim.symbol_000E = Mathf.SmoothStep(-vertexAnim.symbol_001D, vertexAnim.symbol_001D, Mathf.PingPong((float)num / 25f * vertexAnim.symbol_0012, 1f));
							Vector3 a = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), UnityEngine.Random.Range(-0.25f, 0.25f), 0f);
							Matrix4x4 matrix4x = Matrix4x4.TRS(a * this.symbol_0012, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-5f, 5f) * this.symbol_001D), Vector3.one);
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
						this.symbol_0015.UpdateGeometry(textInfo.meshInfo[k].mesh, k);
					}
					num++;
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}

		private struct VertexAnim
		{
			public float symbol_001D;

			public float symbol_000E;

			public float symbol_0012;
		}
	}
}
