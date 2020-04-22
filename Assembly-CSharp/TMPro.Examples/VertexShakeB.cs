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
			if ((bool)(_001D = _0015))
			{
				_0016 = true;
			}
		}

		private IEnumerator coroutine0013()
		{
			_0015.ForceMeshUpdate();
			TMP_TextInfo textInfo = _0015.textInfo;
			Vector3[][] array = new Vector3[0][];
			_0016 = true;
			if (_0016)
			{
				if (array.Length < textInfo.meshInfo.Length)
				{
					while (true)
					{
						switch (1)
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
					array = new Vector3[textInfo.meshInfo.Length][];
				}
				for (int i = 0; i < textInfo.meshInfo.Length; i++)
				{
					int num = textInfo.meshInfo[i].vertices.Length;
					array[i] = new Vector3[num];
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				_0016 = false;
			}
			if (textInfo.characterCount == 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						yield return new WaitForSeconds(0.25f);
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			int lineCount = textInfo.lineCount;
			for (int j = 0; j < lineCount; j++)
			{
				int firstCharacterIndex = textInfo.lineInfo[j].firstCharacterIndex;
				int lastCharacterIndex = textInfo.lineInfo[j].lastCharacterIndex;
				Vector3 vector = (textInfo.characterInfo[firstCharacterIndex].bottomLeft + textInfo.characterInfo[lastCharacterIndex].topRight) / 2f;
				Quaternion q = Quaternion.Euler(0f, 0f, Random.Range(-0.25f, 0.25f));
				for (int k = firstCharacterIndex; k <= lastCharacterIndex; k++)
				{
					if (!textInfo.characterInfo[k].isVisible)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						continue;
					}
					int materialReferenceIndex = textInfo.characterInfo[k].materialReferenceIndex;
					int vertexIndex = textInfo.characterInfo[k].vertexIndex;
					Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
					Vector3 vector2 = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
					array[materialReferenceIndex][vertexIndex] = vertices[vertexIndex] - vector2;
					array[materialReferenceIndex][vertexIndex + 1] = vertices[vertexIndex + 1] - vector2;
					array[materialReferenceIndex][vertexIndex + 2] = vertices[vertexIndex + 2] - vector2;
					array[materialReferenceIndex][vertexIndex + 3] = vertices[vertexIndex + 3] - vector2;
					float d = Random.Range(0.95f, 1.05f);
					Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.one, Quaternion.identity, Vector3.one * d);
					array[materialReferenceIndex][vertexIndex] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex]);
					array[materialReferenceIndex][vertexIndex + 1] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 1]);
					array[materialReferenceIndex][vertexIndex + 2] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 2]);
					array[materialReferenceIndex][vertexIndex + 3] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 3]);
					array[materialReferenceIndex][vertexIndex] += vector2;
					array[materialReferenceIndex][vertexIndex + 1] += vector2;
					array[materialReferenceIndex][vertexIndex + 2] += vector2;
					array[materialReferenceIndex][vertexIndex + 3] += vector2;
					array[materialReferenceIndex][vertexIndex] -= vector;
					array[materialReferenceIndex][vertexIndex + 1] -= vector;
					array[materialReferenceIndex][vertexIndex + 2] -= vector;
					array[materialReferenceIndex][vertexIndex + 3] -= vector;
					matrix4x = Matrix4x4.TRS(Vector3.one, q, Vector3.one);
					array[materialReferenceIndex][vertexIndex] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex]);
					array[materialReferenceIndex][vertexIndex + 1] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 1]);
					array[materialReferenceIndex][vertexIndex + 2] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 2]);
					array[materialReferenceIndex][vertexIndex + 3] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 3]);
					array[materialReferenceIndex][vertexIndex] += vector;
					array[materialReferenceIndex][vertexIndex + 1] += vector;
					array[materialReferenceIndex][vertexIndex + 2] += vector;
					array[materialReferenceIndex][vertexIndex + 3] += vector;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						goto end_IL_075e;
					}
					continue;
					end_IL_075e:
					break;
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				for (int l = 0; l < textInfo.meshInfo.Length; l++)
				{
					textInfo.meshInfo[l].mesh.vertices = array[l];
					_0015.UpdateGeometry(textInfo.meshInfo[l].mesh, l);
				}
				yield return new WaitForSeconds(0.1f);
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
