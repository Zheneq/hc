using System;

[Serializable]
public class CardConfigOverride
{
	public CardType CardType;

	public bool Allowed;

	public CardConfigOverride Clone()
	{
		return (CardConfigOverride)base.MemberwiseClone();
	}
}
