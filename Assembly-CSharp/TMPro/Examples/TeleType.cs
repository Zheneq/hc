using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class TeleType : MonoBehaviour
	{
		private string \u001D = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";

		private string \u000E = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";

		private TMP_Text \u0012;

		private void \u0015()
		{
			this.\u0012 = base.GetComponent<TMP_Text>();
			this.\u0012.text = this.\u001D;
			this.\u0012.enableWordWrapping = true;
			this.\u0012.alignment = TextAlignmentOptions.Top;
		}

		private IEnumerator \u0015()
		{
			this.\u0012.ForceMeshUpdate();
			int characterCount = this.\u0012.textInfo.characterCount;
			int num = 0;
			int num2 = 0;
			for (;;)
			{
				num2 = num % (characterCount + 1);
				this.\u0012.maxVisibleCharacters = num2;
				if (num2 >= characterCount)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TeleType.<Start>c__Iterator0.MoveNext()).MethodHandle;
					}
					yield return new WaitForSeconds(1f);
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					this.\u0012.text = this.\u000E;
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
					this.\u0012.text = this.\u001D;
					yield return new WaitForSeconds(1f);
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
				num++;
				yield return new WaitForSeconds(0.05f);
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
	}
}
