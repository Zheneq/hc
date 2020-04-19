using System;
using TMPro;
using UnityEngine;

public class UIRankListDisplayEntry : MonoBehaviour
{
	public RectTransform m_selfHighlight;

	public _SelectableBtn m_theBtn;

	public TextMeshProUGUI StreakText;

	public TextMeshProUGUI NameText;

	public TextMeshProUGUI DivisionText;

	public TextMeshProUGUI RankText;

	public TextMeshProUGUI LastMatchText;

	public TextMeshProUGUI TotalMatchesText;

	public TextMeshProUGUI ChangeText;

	[HideInInspector]
	public string AccountHandle;

	[HideInInspector]
	public long AccountID;

	private void Awake()
	{
		this.m_theBtn.spriteController.GetComponent<UITooltipClickObject>().Setup(TooltipType.PlayerBannerMenu, new TooltipPopulateCall(this.EntryClicked), null);
	}

	private bool EntryClicked(UITooltipBase tooltip)
	{
		(tooltip as GameOverBannerMenu).Setup(this.AccountHandle, this.AccountID);
		return true;
	}
}
