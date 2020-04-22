using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexColorCycler : MonoBehaviour
	{
		private TMP_Text _001D;

		private void _000E()
		{
			_001D = GetComponent<TMP_Text>();
		}

		private void _0012()
		{
			StartCoroutine(coroutine000E());
		}

		private IEnumerator coroutine000E()
		{
			TMP_TextInfo textInfo = _001D.textInfo;
			int num = 0;
			Color32 color = _001D.color;
			int characterCount = textInfo.characterCount;
			if (characterCount == 0)
			{
				yield return new WaitForSeconds(0.25f);
			}
			int materialReferenceIndex = textInfo.characterInfo[num].materialReferenceIndex;
			Color32[] colors = textInfo.meshInfo[materialReferenceIndex].colors32;
			int vertexIndex = textInfo.characterInfo[num].vertexIndex;
			if (textInfo.characterInfo[num].isVisible)
			{
				color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), byte.MaxValue);
				colors[vertexIndex] = color;
				colors[vertexIndex + 1] = color;
				colors[vertexIndex + 2] = color;
				colors[vertexIndex + 3] = color;
				_001D.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			}
			num = (num + 1) % characterCount;
			yield return new WaitForSeconds(0.05f);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
