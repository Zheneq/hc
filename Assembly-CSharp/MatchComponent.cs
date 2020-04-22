using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class MatchComponent : ICloneable
{
	[Serializable]
	public struct Actor
	{
		public CharacterType Character;

		public Team Team;

		public bool IsPlayer;
	}

	public List<Actor> Actors = new List<Actor>();

	public DateTime MatchTime
	{
		get;
		set;
	}

	public PlayerGameResult Result
	{
		get;
		set;
	}

	public int Kills
	{
		get;
		set;
	}

	public CharacterType CharacterUsed
	{
		get;
		set;
	}

	public GameType GameType
	{
		get;
		set;
	}

	public string MapName
	{
		get;
		set;
	}

	public int NumOfTurns
	{
		get;
		set;
	}

	public string SubTypeLocTag
	{
		get;
		set;
	}

	public CharacterType GetFirstPlayerCharacter()
	{
		if (Actors == null)
		{
			return CharacterUsed;
		}
		for (int i = 0; i < Actors.Count; i++)
		{
			Actor actor = Actors[i];
			if (actor.IsPlayer)
			{
				Actor actor2 = Actors[i];
				return actor2.Character;
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return CharacterUsed;
		}
	}

	public string GetTimeDifferenceText()
	{
		TimeSpan difference = DateTime.UtcNow - MatchTime;
		return StringUtil.GetTimeDifferenceText(difference);
	}

	public string GetSubTypeNameTerm()
	{
		object obj;
		if (SubTypeLocTag != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			obj = SubTypeLocTag.Split("@".ToCharArray()).First();
		}
		else
		{
			obj = "unknown";
		}
		string text = (string)obj;
		if (text == "unknown")
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
			switch (GameType)
			{
			case GameType.Ranked:
				text = "GenericRanked";
				break;
			case GameType.NewPlayerSolo:
				text = "GenericNewPlayerSolo";
				break;
			case GameType.Custom:
				text = "GenericCustom";
				break;
			case GameType.Tutorial:
				text = "GenericTutorial";
				break;
			case GameType.Practice:
				text = "GenericPractice";
				break;
			}
		}
		if (text == "unknown")
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
			if (MapName.EndsWith("CTF"))
			{
				text = "GenericBriefcase";
			}
		}
		return text;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
