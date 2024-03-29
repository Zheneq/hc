using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

public class TierInfo
{
	public string NameLocalizationReference;

	public int MinPerDivision;

	public int MaxPerDivision;

	public string IconResource;

	public Dictionary<int, TierPoints> Points;

	public int MinMMRForBonus;

	public int MaxBonus = 10;

	public TierPointsTypes PointType = TierPointsTypes.Hundreds;

	public bool RatchetTier;

	public int DecayAmount;

	public TimeSpan DecayStart = TimeSpan.FromDays(3.0);

	[JsonIgnore]
	public string Name
	{
		get
		{
			if (NameLocalizationReference.IsNullOrEmpty())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return null;
					}
				}
			}
			return NameLocalizationReference.Split("@".ToCharArray(), 2).First();
		}
	}

	public int CalculatePointsForWin(int groupSize, float mmr, float eloDeltaNeededForMaxBonus)
	{
		if (PointType == TierPointsTypes.DerivedFromMMR)
		{
			return 0;
		}
		if (Points.TryGetValue(groupSize, out TierPoints value))
		{
			if (mmr >= (float)MinMMRForBonus)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						float val = mmr - (float)MinMMRForBonus;
						val = Math.Min(val, eloDeltaNeededForMaxBonus);
						val = Math.Max(1f, val);
						return value.PerWin + (int)Math.Ceiling((float)MaxBonus * val / eloDeltaNeededForMaxBonus);
					}
					}
				}
			}
			return value.PerWin;
		}
		throw new Exception($"Bad groups size {groupSize} playing in tier {Name}");
	}

	public int CalculatePointsPerLoss(int groupSize)
	{
		if (PointType == TierPointsTypes.DerivedFromMMR)
		{
			return 0;
		}
		if (Points.TryGetValue(groupSize, out TierPoints value))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return value.PerLoss;
				}
			}
		}
		throw new Exception($"Bad groups size {groupSize} playing in tier {Name}");
	}
}
