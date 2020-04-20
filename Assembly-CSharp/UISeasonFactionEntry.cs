using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISeasonFactionEntry : MonoBehaviour
{
	public TextMeshProUGUI m_expLabel;

	public Image m_logo;

	public TextMeshProUGUI m_expLevel;

	public _ButtonSwapSprite m_hitbox;

	public UIFactionProgressBar m_ProgressBarPrefab;

	public LayoutGroup m_FactionLevelContainer;

	private int m_factionId;

	private UIFactionProgressBar[] m_factionBars;

	private void Start()
	{
		this.m_hitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.SetupTooltip), null);
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		int activeFactionCompetition = ClientGameManager.Get().ActiveFactionCompetition;
		string longName = Faction.GetLongName(activeFactionCompetition, this.m_factionId);
		string loreDescription = Faction.GetLoreDescription(activeFactionCompetition, this.m_factionId);
		if (!longName.IsNullOrEmpty())
		{
			if (!loreDescription.IsNullOrEmpty())
			{
				UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
				uititledTooltip.Setup(longName, loreDescription, string.Empty);
				return true;
			}
		}
		return false;
	}

	private void SetupProgressBar(Faction faction, int tierIndex, long remainingScore, bool showText)
	{
		float[] rbga = FactionWideData.Get().GetRBGA(faction);
		float fillAmount;
		if (faction.Tiers[tierIndex].ContributionToComplete > remainingScore)
		{
			UIManager.SetGameObjectActive(this.m_factionBars[tierIndex].m_CompletedBar, false, null);
			UIManager.SetGameObjectActive(this.m_factionBars[tierIndex].m_ProgressFillBar, true, null);
			fillAmount = (float)remainingScore / (float)faction.Tiers[tierIndex].ContributionToComplete;
			this.m_factionBars[tierIndex].m_ProgressFillBar.color = new Color(rbga[0], rbga[1], rbga[2], rbga[3]);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_factionBars[tierIndex].m_CompletedBar, true, null);
			UIManager.SetGameObjectActive(this.m_factionBars[tierIndex].m_ProgressFillBar, false, null);
			this.m_factionBars[tierIndex].m_CompletedBar.color = new Color(rbga[0], rbga[1], rbga[2], rbga[3]);
			fillAmount = 1f;
		}
		this.m_factionBars[tierIndex].m_ProgressFillBar.fillAmount = fillAmount;
	}

	public void Setup(Faction faction, long score, int factionId)
	{
		if (this.m_factionBars != null)
		{
			if (this.m_factionBars.Length == faction.Tiers.Count)
			{
				goto IL_210;
			}
		}
		if (this.m_factionBars != null)
		{
			for (int i = 0; i < this.m_factionBars.Length; i++)
			{
				UnityEngine.Object.Destroy(this.m_factionBars[i].gameObject);
			}
		}
		this.m_factionBars = new UIFactionProgressBar[faction.Tiers.Count];
		float preferredWidth = (this.m_FactionLevelContainer.transform as RectTransform).sizeDelta.x / (float)this.m_factionBars.Length;
		for (int j = 0; j < this.m_factionBars.Length; j++)
		{
			this.m_factionBars[j] = UnityEngine.Object.Instantiate<UIFactionProgressBar>(this.m_ProgressBarPrefab);
			UIManager.ReparentTransform(this.m_factionBars[j].transform, this.m_FactionLevelContainer.gameObject.transform);
			this.m_factionBars[j].m_LayoutElement.preferredWidth = preferredWidth;
		}
		if (this.m_factionBars.Length == 1)
		{
			this.m_factionBars[0].m_EmptyBar.type = Image.Type.Sliced;
			this.m_factionBars[0].m_CompletedBar.type = Image.Type.Sliced;
			this.m_factionBars[0].m_ProgressFillBar.fillSlope = 3.9f;
			this.m_factionBars[0].m_ProgressFillBar.m_FillMin = 0.1f;
			this.m_factionBars[0].m_ProgressFillBar.m_FillMax = 0.9f;
			this.m_factionBars[0].m_ProgressFillBar.fillStart = 0f;
			(this.m_factionBars[0].m_ProgressFillBar.gameObject.transform as RectTransform).sizeDelta = new Vector2(72f, (this.m_factionBars[0].m_ProgressFillBar.gameObject.transform as RectTransform).sizeDelta.y);
		}
		IL_210:
		this.m_factionId = factionId;
		FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(faction.FactionGroupIDToUse);
		this.m_logo.sprite = Resources.Load<Sprite>(factionGroup.BannerPath);
		long num = score;
		bool flag = false;
		int num2 = 1;
		int k = 0;
		while (k < faction.Tiers.Count)
		{
			if (k == faction.Tiers.Count - 1)
			{
				goto IL_2BD;
			}
			if (score < faction.Tiers[k].ContributionToComplete)
			{
				goto IL_2BD;
			}
			bool flag2 = score < faction.Tiers[k].ContributionToComplete + faction.Tiers[k + 1].ContributionToComplete;
			IL_2BE:
			bool showText = flag2;
			int tierIndex = k;
			long remainingScore;
			if (score < 0L)
			{
				remainingScore = 0L;
			}
			else
			{
				remainingScore = score;
			}
			this.SetupProgressBar(faction, tierIndex, remainingScore, showText);
			if (score >= 0L)
			{
				num2 = k + 1;
				if (score < faction.Tiers[k].ContributionToComplete)
				{
					if (this.m_expLabel != null)
					{
						this.m_expLabel.text = score + " / " + faction.Tiers[k].ContributionToComplete;
						flag = true;
					}
					num2 = k + 1;
				}
			}
			score -= faction.Tiers[k].ContributionToComplete;
			k++;
			continue;
			IL_2BD:
			flag2 = true;
			goto IL_2BE;
		}
		this.m_expLevel.text = num2.ToString();
		if (!flag)
		{
			if (this.m_expLabel != null)
			{
				this.m_expLabel.text = num.ToString();
			}
		}
	}
}
