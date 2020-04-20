using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexShakeA : MonoBehaviour
	{
		public float symbol_001D = 1f;

		public float symbol_000E = 1f;

		public float symbol_0012 = 1f;

		public float symbol_0015 = 1f;

		private TMP_Text symbol_0016;

		private bool symbol_0013;

		private void symbol_0018()
		{
			this.symbol_0016 = base.GetComponent<TMP_Text>();
		}

		private void symbol_0009()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.symbol_0018));
		}

		private void symbol_0019()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.symbol_0018));
		}

		private void symbol_0011()
		{
			base.StartCoroutine(this.coroutine0018());
		}

		private void symbol_0018(UnityEngine.Object symbol_001D)
		{
			if (this.symbol_0016)
			{
				this.symbol_0013 = true;
			}
		}

		private IEnumerator coroutine0018()
		{
			this.symbol_0016.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.symbol_0016.textInfo;
			Vector3[][] array = new Vector3[0][];
			this.symbol_0013 = true;
			for (;;)
			{
				if (this.symbol_0013)
				{
					if (array.Length < textInfo.meshInfo.Length)
					{
						array = new Vector3[textInfo.meshInfo.Length][];
					}
					for (int i = 0; i < textInfo.meshInfo.Length; i++)
					{
						int num = textInfo.meshInfo[i].vertices.Length;
						array[i] = new Vector3[num];
					}
					this.symbol_0013 = false;
				}
				if (textInfo.characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
				}
				else
				{
					int lineCount = textInfo.lineCount;
					for (int j = 0; j < lineCount; j++)
					{
						int firstCharacterIndex = textInfo.lineInfo[j].firstCharacterIndex;
						int lastCharacterIndex = textInfo.lineInfo[j].lastCharacterIndex;
						Vector3 b = (textInfo.characterInfo[firstCharacterIndex].bottomLeft + textInfo.characterInfo[lastCharacterIndex].topRight) / 2f;
						Quaternion q = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-0.25f, 0.25f) * this.symbol_0015);
						for (int k = firstCharacterIndex; k <= lastCharacterIndex; k++)
						{
							if (!textInfo.characterInfo[k].isVisible)
							{
							}
							else
							{
								int materialReferenceIndex = textInfo.characterInfo[k].materialReferenceIndex;
								int vertexIndex = textInfo.characterInfo[k].vertexIndex;
								Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
								array[materialReferenceIndex][vertexIndex] = vertices[vertexIndex] - b;
								array[materialReferenceIndex][vertexIndex + 1] = vertices[vertexIndex + 1] - b;
								array[materialReferenceIndex][vertexIndex + 2] = vertices[vertexIndex + 2] - b;
								array[materialReferenceIndex][vertexIndex + 3] = vertices[vertexIndex + 3] - b;
								float d = UnityEngine.Random.Range(0.995f - 0.001f * this.symbol_0012, 1.005f + 0.001f * this.symbol_0012);
								Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.one, q, Vector3.one * d);
								array[materialReferenceIndex][vertexIndex] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex]);
								array[materialReferenceIndex][vertexIndex + 1] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 1]);
								array[materialReferenceIndex][vertexIndex + 2] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 2]);
								array[materialReferenceIndex][vertexIndex + 3] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 3]);
								array[materialReferenceIndex][vertexIndex] += b;
								array[materialReferenceIndex][vertexIndex + 1] += b;
								array[materialReferenceIndex][vertexIndex + 2] += b;
								array[materialReferenceIndex][vertexIndex + 3] += b;
							}
						}
					}
					for (int l = 0; l < textInfo.meshInfo.Length; l++)
					{
						textInfo.meshInfo[l].mesh.vertices = array[l];
						this.symbol_0016.UpdateGeometry(textInfo.meshInfo[l].mesh, l);
					}
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}
	}
}
