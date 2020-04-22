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
		m_hitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, SetupTooltip);
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		int activeFactionCompetition = ClientGameManager.Get().ActiveFactionCompetition;
		string longName = Faction.GetLongName(activeFactionCompetition, m_factionId);
		string loreDescription = Faction.GetLoreDescription(activeFactionCompetition, m_factionId);
		if (!longName.IsNullOrEmpty())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!loreDescription.IsNullOrEmpty())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
						uITitledTooltip.Setup(longName, loreDescription, string.Empty);
						return true;
					}
					}
				}
			}
		}
		return false;
	}

	private void SetupProgressBar(Faction faction, int tierIndex, long remainingScore, bool showText)
	{
		float num = 0f;
		float[] rBGA = FactionWideData.Get().GetRBGA(faction);
		if (faction.Tiers[tierIndex].ContributionToComplete > remainingScore)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(m_factionBars[tierIndex].m_CompletedBar, false);
			UIManager.SetGameObjectActive(m_factionBars[tierIndex].m_ProgressFillBar, true);
			num = (float)remainingScore / (float)faction.Tiers[tierIndex].ContributionToComplete;
			m_factionBars[tierIndex].m_ProgressFillBar.color = new Color(rBGA[0], rBGA[1], rBGA[2], rBGA[3]);
		}
		else
		{
			UIManager.SetGameObjectActive(m_factionBars[tierIndex].m_CompletedBar, true);
			UIManager.SetGameObjectActive(m_factionBars[tierIndex].m_ProgressFillBar, false);
			m_factionBars[tierIndex].m_CompletedBar.color = new Color(rBGA[0], rBGA[1], rBGA[2], rBGA[3]);
			num = 1f;
		}
		m_factionBars[tierIndex].m_ProgressFillBar.fillAmount = num;
	}

	public void Setup(Faction faction, long score, int factionId)
	{
		if (m_factionBars != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_factionBars.Length == faction.Tiers.Count)
			{
				goto IL_0210;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (m_factionBars != null)
		{
			for (int i = 0; i < m_factionBars.Length; i++)
			{
				Object.Destroy(m_factionBars[i].gameObject);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_factionBars = new UIFactionProgressBar[faction.Tiers.Count];
		Vector2 sizeDelta = (m_FactionLevelContainer.transform as RectTransform).sizeDelta;
		float preferredWidth = sizeDelta.x / (float)m_factionBars.Length;
		for (int j = 0; j < m_factionBars.Length; j++)
		{
			m_factionBars[j] = Object.Instantiate(m_ProgressBarPrefab);
			UIManager.ReparentTransform(m_factionBars[j].transform, m_FactionLevelContainer.gameObject.transform);
			m_factionBars[j].m_LayoutElement.preferredWidth = preferredWidth;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		if (m_factionBars.Length == 1)
		{
			m_factionBars[0].m_EmptyBar.type = Image.Type.Sliced;
			m_factionBars[0].m_CompletedBar.type = Image.Type.Sliced;
			m_factionBars[0].m_ProgressFillBar.fillSlope = 3.9f;
			m_factionBars[0].m_ProgressFillBar.m_FillMin = 0.1f;
			m_factionBars[0].m_ProgressFillBar.m_FillMax = 0.9f;
			m_factionBars[0].m_ProgressFillBar.fillStart = 0f;
			RectTransform obj = m_factionBars[0].m_ProgressFillBar.gameObject.transform as RectTransform;
			Vector2 sizeDelta2 = (m_factionBars[0].m_ProgressFillBar.gameObject.transform as RectTransform).sizeDelta;
			obj.sizeDelta = new Vector2(72f, sizeDelta2.y);
		}
		goto IL_0210;
		IL_0210:
		m_factionId = factionId;
		FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(faction.FactionGroupIDToUse);
		m_logo.sprite = Resources.Load<Sprite>(factionGroup.BannerPath);
		long num = score;
		bool flag = false;
		int num2 = 1;
		for (int num3 = 0; num3 < faction.Tiers.Count; num3++)
		{
			int num4;
			if (num3 != faction.Tiers.Count - 1)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (score >= faction.Tiers[num3].ContributionToComplete)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					num4 = ((score < faction.Tiers[num3].ContributionToComplete + faction.Tiers[num3 + 1].ContributionToComplete) ? 1 : 0);
					goto IL_02be;
				}
			}
			num4 = 1;
			goto IL_02be;
			IL_02be:
			bool showText = (byte)num4 != 0;
			int tierIndex = num3;
			long remainingScore;
			if (score < 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				remainingScore = 0L;
			}
			else
			{
				remainingScore = score;
			}
			SetupProgressBar(faction, tierIndex, remainingScore, showText);
			if (score >= 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num2 = num3 + 1;
				if (score < faction.Tiers[num3].ContributionToComplete)
				{
					if (m_expLabel != null)
					{
						m_expLabel.text = score + " / " + faction.Tiers[num3].ContributionToComplete;
						flag = true;
					}
					num2 = num3 + 1;
				}
			}
			score -= faction.Tiers[num3].ContributionToComplete;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			m_expLevel.text = num2.ToString();
			if (flag)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (m_expLabel != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						m_expLabel.text = num.ToString();
						return;
					}
				}
				return;
			}
		}
	}
}
