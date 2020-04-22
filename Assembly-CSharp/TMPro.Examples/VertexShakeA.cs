using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexShakeA : MonoBehaviour
	{
		public float _001D = 1f;

		public float _000E = 1f;

		public float _0012 = 1f;

		public float _0015 = 1f;

		private TMP_Text _0016;

		private bool _0013;

		private void _0018()
		{
			_0016 = GetComponent<TMP_Text>();
		}

		private void _0009()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(_0018);
		}

		private void _0019()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(_0018);
		}

		private void _0011()
		{
			StartCoroutine(coroutine0018());
		}

		private void _0018(Object _001D)
		{
			if (!(_001D = _0016))
			{
				return;
			}
			while (true)
			{
				_0013 = true;
				return;
			}
		}

		private IEnumerator coroutine0018()
		{
			_0016.ForceMeshUpdate();
			TMP_TextInfo textInfo = _0016.textInfo;
			Vector3[][] array = new Vector3[0][];
			_0013 = true;
			if (_0013)
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
				_0013 = false;
			}
			if (textInfo.characterCount == 0)
			{
				yield return new WaitForSeconds(0.25f);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			int lineCount = textInfo.lineCount;
			for (int j = 0; j < lineCount; j++)
			{
				int firstCharacterIndex = textInfo.lineInfo[j].firstCharacterIndex;
				int lastCharacterIndex = textInfo.lineInfo[j].lastCharacterIndex;
				Vector3 vector = (textInfo.characterInfo[firstCharacterIndex].bottomLeft + textInfo.characterInfo[lastCharacterIndex].topRight) / 2f;
				Quaternion q = Quaternion.Euler(0f, 0f, Random.Range(-0.25f, 0.25f) * _0015);
				for (int k = firstCharacterIndex; k <= lastCharacterIndex; k++)
				{
					if (!textInfo.characterInfo[k].isVisible)
					{
						continue;
					}
					int materialReferenceIndex = textInfo.characterInfo[k].materialReferenceIndex;
					int vertexIndex = textInfo.characterInfo[k].vertexIndex;
					Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
					array[materialReferenceIndex][vertexIndex] = vertices[vertexIndex] - vector;
					array[materialReferenceIndex][vertexIndex + 1] = vertices[vertexIndex + 1] - vector;
					array[materialReferenceIndex][vertexIndex + 2] = vertices[vertexIndex + 2] - vector;
					array[materialReferenceIndex][vertexIndex + 3] = vertices[vertexIndex + 3] - vector;
					float d = Random.Range(0.995f - 0.001f * _0012, 1.005f + 0.001f * _0012);
					Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.one, q, Vector3.one * d);
					array[materialReferenceIndex][vertexIndex] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex]);
					array[materialReferenceIndex][vertexIndex + 1] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 1]);
					array[materialReferenceIndex][vertexIndex + 2] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 2]);
					array[materialReferenceIndex][vertexIndex + 3] = matrix4x.MultiplyPoint3x4(array[materialReferenceIndex][vertexIndex + 3]);
					array[materialReferenceIndex][vertexIndex] += vector;
					array[materialReferenceIndex][vertexIndex + 1] += vector;
					array[materialReferenceIndex][vertexIndex + 2] += vector;
					array[materialReferenceIndex][vertexIndex + 3] += vector;
				}
			}
			while (true)
			{
				for (int l = 0; l < textInfo.meshInfo.Length; l++)
				{
					textInfo.meshInfo[l].mesh.vertices = array[l];
					_0016.UpdateGeometry(textInfo.meshInfo[l].mesh, l);
				}
				while (true)
				{
					yield return new WaitForSeconds(0.1f);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}
	}
}
