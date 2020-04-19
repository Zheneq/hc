using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexShakeB : MonoBehaviour
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
			if (this.\u0015)
			{
				this.\u0016 = true;
			}
		}

		private IEnumerator \u0013()
		{
			this.\u0015.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.\u0015.textInfo;
			Vector3[][] array = new Vector3[0][];
			this.\u0016 = true;
			for (;;)
			{
				if (this.\u0016)
				{
					if (array.Length < textInfo.meshInfo.Length)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(VertexShakeB.<AnimateVertexColors>c__Iterator0.MoveNext()).MethodHandle;
						}
						array = new Vector3[textInfo.meshInfo.Length][];
					}
					for (int i = 0; i < textInfo.meshInfo.Length; i++)
					{
						int num = textInfo.meshInfo[i].vertices.Length;
						array[i] = new Vector3[num];
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
					this.\u0016 = false;
				}
				if (textInfo.characterCount == 0)
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
					yield return new WaitForSeconds(0.25f);
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
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
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					for (int l = 0; l < textInfo.meshInfo.Length; l++)
					{
						textInfo.meshInfo[l].mesh.vertices = array[l];
						this.\u0015.UpdateGeometry(textInfo.meshInfo[l].mesh, l);
					}
					yield return new WaitForSeconds(0.1f);
					for (;;)
					{
						switch (6)
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
	}
}
