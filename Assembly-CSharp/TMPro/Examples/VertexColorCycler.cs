using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexColorCycler : MonoBehaviour
	{
		private TMP_Text \u001D;

		private void \u000E()
		{
			this.\u001D = base.GetComponent<TMP_Text>();
		}

		private void \u0012()
		{
			base.StartCoroutine(this.coroutine000E());
		}

		private IEnumerator coroutine000E()
		{
			TMP_TextInfo textInfo = this.\u001D.textInfo;
			int num = 0;
			Color32 color = this.\u001D.color;
			for (;;)
			{
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(VertexColorCycler.<AnimateVertexColors>c__Iterator0.MoveNext()).MethodHandle;
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
					int materialReferenceIndex = textInfo.characterInfo[num].materialReferenceIndex;
					Color32[] colors = textInfo.meshInfo[materialReferenceIndex].colors32;
					int vertexIndex = textInfo.characterInfo[num].vertexIndex;
					if (textInfo.characterInfo[num].isVisible)
					{
						color = new Color32((byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), byte.MaxValue);
						colors[vertexIndex] = color;
						colors[vertexIndex + 1] = color;
						colors[vertexIndex + 2] = color;
						colors[vertexIndex + 3] = color;
						this.\u001D.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
					}
					num = (num + 1) % characterCount;
					yield return new WaitForSeconds(0.05f);
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
			}
			yield break;
		}
	}
}
