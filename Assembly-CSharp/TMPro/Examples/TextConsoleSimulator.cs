using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class TextConsoleSimulator : MonoBehaviour
	{
		private TMP_Text \u001D;

		private bool \u000E;

		private void \u0012()
		{
			this.\u001D = base.gameObject.GetComponent<TMP_Text>();
		}

		private void \u0015()
		{
			base.StartCoroutine(this.\u0012(this.\u001D));
		}

		private void \u0016()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.\u0012));
		}

		private void \u0013()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.\u0012));
		}

		private void \u0012(UnityEngine.Object \u001D)
		{
			this.\u000E = true;
		}

		private IEnumerator \u0012(TMP_Text \u001D)
		{
			\u001D.ForceMeshUpdate();
			TMP_TextInfo textInfo = \u001D.textInfo;
			int characterCount = textInfo.characterCount;
			int num = 0;
			for (;;)
			{
				if (this.\u000E)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(TextConsoleSimulator.<RevealCharacters>c__Iterator0.MoveNext()).MethodHandle;
					}
					characterCount = textInfo.characterCount;
					this.\u000E = false;
				}
				if (num > characterCount)
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
					yield return new WaitForSeconds(1f);
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					num = 0;
				}
				\u001D.maxVisibleCharacters = num;
				num++;
				yield return new WaitForSeconds(0f);
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			yield break;
		}

		private IEnumerator \u0015(TMP_Text \u001D)
		{
			\u001D.ForceMeshUpdate();
			int wordCount = \u001D.textInfo.wordCount;
			int characterCount = \u001D.textInfo.characterCount;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (;;)
			{
				num2 = num % (wordCount + 1);
				if (num2 == 0)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(TextConsoleSimulator.<RevealWords>c__Iterator1.MoveNext()).MethodHandle;
					}
					num3 = 0;
				}
				else if (num2 < wordCount)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					num3 = \u001D.textInfo.wordInfo[num2 - 1].lastCharacterIndex + 1;
				}
				else if (num2 == wordCount)
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
					num3 = characterCount;
				}
				\u001D.maxVisibleCharacters = num3;
				if (num3 >= characterCount)
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
					yield return new WaitForSeconds(1f);
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				num++;
				yield return new WaitForSeconds(0.1f);
			}
			yield break;
		}
	}
}
