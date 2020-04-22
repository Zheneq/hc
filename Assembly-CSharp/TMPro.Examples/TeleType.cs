using System;
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
			this._0012 = base.GetComponent<TMP_Text>();
			this._0012.text = this._001D;
			this._0012.enableWordWrapping = true;
			this._0012.alignment = TextAlignmentOptions.Top;
		}

		private IEnumerator coroutine0015()
		{
			this._0012.ForceMeshUpdate();
			int characterCount = this._0012.textInfo.characterCount;
			int num = 0;
			int num2 = 0;
			for (;;)
			{
				num2 = num % (characterCount + 1);
				this._0012.maxVisibleCharacters = num2;
				if (num2 >= characterCount)
				{
					yield return new WaitForSeconds(1f);
					this._0012.text = this._000E;
					yield return new WaitForSeconds(1f);
					this._0012.text = this._001D;
					yield return new WaitForSeconds(1f);
				}
				num++;
				yield return new WaitForSeconds(0.05f);
			}
			yield break;
		}
	}
}
