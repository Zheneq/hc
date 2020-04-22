using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class TextConsoleSimulator : MonoBehaviour
	{
		private TMP_Text _001D;

		private bool _000E;

		private void _0012()
		{
			_001D = base.gameObject.GetComponent<TMP_Text>();
		}

		private void _0015()
		{
			StartCoroutine(_0012(_001D));
		}

		private void _0016()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(_0012);
		}

		private void _0013()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(_0012);
		}

		private void _0012(Object _001D)
		{
			_000E = true;
		}

		private IEnumerator _0012(TMP_Text _001D)
		{
			_001D.ForceMeshUpdate();
			TMP_TextInfo textInfo = _001D.textInfo;
			int characterCount = textInfo.characterCount;
			int num = 0;
			if (_000E)
			{
				characterCount = textInfo.characterCount;
				_000E = false;
			}
			if (num > characterCount)
			{
				yield return new WaitForSeconds(1f);
			}
			_001D.maxVisibleCharacters = num;
			num++;
			yield return new WaitForSeconds(0f);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private IEnumerator _0015(TMP_Text _001D)
		{
			_001D.ForceMeshUpdate();
			int wordCount = _001D.textInfo.wordCount;
			int characterCount = _001D.textInfo.characterCount;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			while (true)
			{
				num2 = num % (wordCount + 1);
				if (num2 == 0)
				{
					num3 = 0;
				}
				else if (num2 < wordCount)
				{
					num3 = _001D.textInfo.wordInfo[num2 - 1].lastCharacterIndex + 1;
				}
				else if (num2 == wordCount)
				{
					num3 = characterCount;
				}
				_001D.maxVisibleCharacters = num3;
				if (num3 >= characterCount)
				{
					break;
				}
				num++;
				yield return new WaitForSeconds(0.1f);
			}
			while (true)
			{
				yield return new WaitForSeconds(1f);
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
