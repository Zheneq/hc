using System;

[Serializable]
public struct CharacterCardInfo
{
	public CardType PrepCard;

	public CardType CombatCard;

	public CardType DashCard;

	public void Reset()
	{
		PrepCard = CardType.None;
		CombatCard = CardType.None;
		DashCard = CardType.None;
	}

	public string ToIdString()
	{
		return $"{(int)PrepCard}/{(int)CombatCard}/{(int)DashCard}";
	}

	public override bool Equals(object obj)
	{
		if (!(obj is CharacterCardInfo))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return false;
			}
		}
		CharacterCardInfo characterCardInfo = (CharacterCardInfo)obj;
		int result;
		if (PrepCard == characterCardInfo.PrepCard)
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
			if (CombatCard == characterCardInfo.CombatCard)
			{
				result = ((DashCard == characterCardInfo.DashCard) ? 1 : 0);
				goto IL_005e;
			}
		}
		result = 0;
		goto IL_005e;
		IL_005e:
		return (byte)result != 0;
	}

	public bool HasEmptySelection()
	{
		int result;
		if (PrepCard > CardType.NoOverride)
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
			if (DashCard > CardType.NoOverride)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				result = ((CombatCard <= CardType.NoOverride) ? 1 : 0);
				goto IL_003e;
			}
		}
		result = 1;
		goto IL_003e;
		IL_003e:
		return (byte)result != 0;
	}

	public bool Uninitialized()
	{
		int result;
		if (PrepCard == CardType.NoOverride)
		{
			while (true)
			{
				switch (7)
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
			if (DashCard == CardType.NoOverride)
			{
				result = ((CombatCard == CardType.NoOverride) ? 1 : 0);
				goto IL_002f;
			}
		}
		result = 0;
		goto IL_002f;
		IL_002f:
		return (byte)result != 0;
	}

	public override int GetHashCode()
	{
		return PrepCard.GetHashCode() ^ CombatCard.GetHashCode() ^ DashCard.GetHashCode();
	}
}
