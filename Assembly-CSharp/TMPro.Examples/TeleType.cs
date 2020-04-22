using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class TeleType : MonoBehaviour
	{
		private string _001D = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";

		private string _000E = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";

		private TMP_Text _0012;

		private void _0015()
		{
			_0012 = GetComponent<TMP_Text>();
			_0012.text = _001D;
			_0012.enableWordWrapping = true;
			_0012.alignment = TextAlignmentOptions.Top;
		}

		private IEnumerator coroutine0015()
		{
			_0012.ForceMeshUpdate();
			int characterCount = _0012.textInfo.characterCount;
			int num = 0;
			int num2 = 0;
			num2 = num % (characterCount + 1);
			_0012.maxVisibleCharacters = num2;
			if (num2 >= characterCount)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						yield return new WaitForSeconds(1f);
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			num++;
			yield return new WaitForSeconds(0.05f);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
