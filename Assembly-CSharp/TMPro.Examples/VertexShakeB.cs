using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexShakeB : MonoBehaviour
	{
		public float _001D = 1f;

		public float _000E = 1f;

		public float _0012 = 1f;

		private TMP_Text _0015;

		private bool _0016;

		private void _0013()
		{
			this._0015 = base.GetComponent<TMP_Text>();
		}

		private void _0018()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this._0013));
		}

		private void _0009()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this._0013));
		}

		private void _0019()
		{
			base.StartCoroutine(this.coroutine0013());
		}

		private void _0013(UnityEngine.Object _001D)
		{
			if (this._0015)
			{
				this._0016 = true;
			}
		}

		private IEnumerator coroutine0013()
		{
			this._0015.ForceMeshUpdate();
			TMP_TextInfo textInfo = this._0015.textInfo;
			Vector3[][] array = new Vector3[0][];
			this._0016 = true;
			for (;;)
			{
				if (this._0016)
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
					this._0016 = false;
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
						Quaternion q = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-0.25f, 0.25f));
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
								Vector3 b2 = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
								array[materialReferenceIndex][vertexIndex] = vertices[vertexIndex] - b2;
								array[materialReferenceIndex][vertexIndex + 1] = vertices[vertexIndex + 1] - b2;
								array[materialReferenceIndex][vertexIndex + 2] = vertices[vertexIndex + 2] - b2;
								array[materialReferenceIndex][vertexIndex + 3] = vertices[vertexIndex + 3] - b2;
								float d = UnityEngine.Random.Range(0.95f, 1.05f);
								Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.one, Quaternion.identity, Vector3.one * d);
								array[materialReferenceIndex][vertexIndex] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex]);
								array[materialReferenceIndex][vertexIndex + 1] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 1]);
								array[materialReferenceIndex][vertexIndex + 2] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 2]);
								array[materialReferenceIndex][vertexIndex + 3] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 3]);
								array[materialReferenceIndex][vertexIndex] += b2;
								array[materialReferenceIndex][vertexIndex + 1] += b2;
								array[materialReferenceIndex][vertexIndex + 2] += b2;
								array[materialReferenceIndex][vertexIndex + 3] += b2;
								array[materialReferenceIndex][vertexIndex] -= b;
								array[materialReferenceIndex][vertexIndex + 1] -= b;
								array[materialReferenceIndex][vertexIndex + 2] -= b;
								array[materialReferenceIndex][vertexIndex + 3] -= b;
								matrix4x = Matrix4x4.TRS(Vector3.one, q, Vector3.one);
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
						this._0015.UpdateGeometry(textInfo.meshInfo[l].mesh, l);
					}
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}
	}
}
