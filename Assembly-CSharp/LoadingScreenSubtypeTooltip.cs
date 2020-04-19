using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenSubtypeTooltip : MonoBehaviour
{
	public Image[] TooltipImages;

	public TextMeshProUGUI HeaderText;

	public TextMeshProUGUI TooltipText;

	public void Setup(GameSubTypeData.GameSubTypeInstructionDisplayInfo displayInfo)
	{
		this.HeaderText.text = displayInfo.HeaderStringLocalized;
		this.TooltipText.text = displayInfo.TooltipStringLocalized;
		for (int i = 0; i < this.TooltipImages.Length; i++)
		{
			if (i < displayInfo.IconResourceStrings.Length)
			{
				UIManager.SetGameObjectActive(this.TooltipImages[i], true, null);
				this.TooltipImages[i].sprite = (Sprite)Resources.Load(displayInfo.IconResourceStrings[i], typeof(Sprite));
			}
			else
			{
				UIManager.SetGameObjectActive(this.TooltipImages[i], false, null);
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(LoadingScreenSubtypeTooltip.Setup(GameSubTypeData.GameSubTypeInstructionDisplayInfo)).MethodHandle;
		}
	}
}
