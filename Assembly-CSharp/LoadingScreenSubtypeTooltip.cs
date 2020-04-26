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
		HeaderText.text = displayInfo.HeaderStringLocalized;
		TooltipText.text = displayInfo.TooltipStringLocalized;
		for (int i = 0; i < TooltipImages.Length; i++)
		{
			if (i < displayInfo.IconResourceStrings.Length)
			{
				UIManager.SetGameObjectActive(TooltipImages[i], true);
				TooltipImages[i].sprite = (Sprite)Resources.Load(displayInfo.IconResourceStrings[i], typeof(Sprite));
			}
			else
			{
				UIManager.SetGameObjectActive(TooltipImages[i], false);
			}
		}
		while (true)
		{
			return;
		}
	}
}
