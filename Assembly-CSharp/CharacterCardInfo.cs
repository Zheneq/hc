using System;

[Serializable]
public struct CharacterCardInfo
{
	public CardType PrepCard;

	public CardType CombatCard;

	public CardType DashCard;

	public void Reset()
	{
		this.PrepCard = CardType.None;
		this.CombatCard = CardType.None;
		this.DashCard = CardType.None;
	}

	public string ToIdString()
	{
		return string.Format("{0}/{1}/{2}", (int)this.PrepCard, (int)this.CombatCard, (int)this.DashCard);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is CharacterCardInfo))
		{
			return false;
		}
		CharacterCardInfo characterCardInfo = (CharacterCardInfo)obj;
		if (this.PrepCard == characterCardInfo.PrepCard)
		{
			if (this.CombatCard == characterCardInfo.CombatCard)
			{
				return this.DashCard == characterCardInfo.DashCard;
			}
		}
		return false;
	}

	public bool HasEmptySelection()
	{
		if (this.PrepCard > CardType.NoOverride)
		{
			if (this.DashCard > CardType.NoOverride)
			{
				return this.CombatCard <= CardType.NoOverride;
			}
		}
		return true;
	}

	public bool Uninitialized()
	{
		if (this.PrepCard == CardType.NoOverride)
		{
			if (this.DashCard == CardType.NoOverride)
			{
				return this.CombatCard == CardType.NoOverride;
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		return this.PrepCard.GetHashCode() ^ this.CombatCard.GetHashCode() ^ this.DashCard.GetHashCode();
	}
}
