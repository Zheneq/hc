using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class TeleType : MonoBehaviour
	{
		private string symbol_001D = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";

		private string symbol_000E = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";

		private TMP_Text symbol_0012;

		private void symbol_0015()
		{
			this.symbol_0012 = base.GetComponent<TMP_Text>();
			this.symbol_0012.text = this.symbol_001D;
			this.symbol_0012.enableWordWrapping = true;
			this.symbol_0012.alignment = TextAlignmentOptions.Top;
		}

		private IEnumerator coroutine0015()
		{
			this.symbol_0012.ForceMeshUpdate();
			int characterCount = this.symbol_0012.textInfo.characterCount;
			int num = 0;
			int num2 = 0;
			for (;;)
			{
				num2 = num % (characterCount + 1);
				this.symbol_0012.maxVisibleCharacters = num2;
				if (num2 >= characterCount)
				{
					yield return new WaitForSeconds(1f);
					this.symbol_0012.text = this.symbol_000E;
					yield return new WaitForSeconds(1f);
					this.symbol_0012.text = this.symbol_001D;
					yield return new WaitForSeconds(1f);
				}
				num++;
				yield return new WaitForSeconds(0.05f);
			}
			yield break;
		}
	}
}
