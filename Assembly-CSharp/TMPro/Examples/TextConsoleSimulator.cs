using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class TextConsoleSimulator : MonoBehaviour
	{
		private TMP_Text symbol_001D;

		private bool symbol_000E;

		private void symbol_0012()
		{
			this.symbol_001D = base.gameObject.GetComponent<TMP_Text>();
		}

		private void symbol_0015()
		{
			base.StartCoroutine(this.symbol_0012(this.symbol_001D));
		}

		private void symbol_0016()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.symbol_0012));
		}

		private void symbol_0013()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.symbol_0012));
		}

		private void symbol_0012(UnityEngine.Object symbol_001D)
		{
			this.symbol_000E = true;
		}

		private IEnumerator symbol_0012(TMP_Text symbol_001D)
		{
			symbol_001D.ForceMeshUpdate();
			TMP_TextInfo textInfo = symbol_001D.textInfo;
			int characterCount = textInfo.characterCount;
			int num = 0;
			for (;;)
			{
				if (this.symbol_000E)
				{
					characterCount = textInfo.characterCount;
					this.symbol_000E = false;
				}
				if (num > characterCount)
				{
					yield return new WaitForSeconds(1f);
					num = 0;
				}
				symbol_001D.maxVisibleCharacters = num;
				num++;
				yield return new WaitForSeconds(0f);
			}
			yield break;
		}

		private IEnumerator symbol_0015(TMP_Text symbol_001D)
		{
			symbol_001D.ForceMeshUpdate();
			int wordCount = symbol_001D.textInfo.wordCount;
			int characterCount = symbol_001D.textInfo.characterCount;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (;;)
			{
				num2 = num % (wordCount + 1);
				if (num2 == 0)
				{
					num3 = 0;
				}
				else if (num2 < wordCount)
				{
					num3 = symbol_001D.textInfo.wordInfo[num2 - 1].lastCharacterIndex + 1;
				}
				else if (num2 == wordCount)
				{
					num3 = characterCount;
				}
				symbol_001D.maxVisibleCharacters = num3;
				if (num3 >= characterCount)
				{
					yield return new WaitForSeconds(1f);
				}
				num++;
				yield return new WaitForSeconds(0.1f);
			}
			yield break;
		}
	}
}
