using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class TierInfo
{
	public string NameLocalizationReference;

	public int MinPerDivision;

	public int MaxPerDivision;

	public string IconResource;

	public Dictionary<int, TierPoints> Points;

	public int MinMMRForBonus;

	public int MaxBonus = 0xA;

	public TierPointsTypes PointType = TierPointsTypes.Hundreds;

	public bool RatchetTier;

	public int DecayAmount;

	public TimeSpan DecayStart = TimeSpan.FromDays(3.0);

	public int CalculatePointsForWin(int groupSize, float mmr, float eloDeltaNeededForMaxBonus)
	{
		if (this.PointType == TierPointsTypes.DerivedFromMMR)
		{
			return 0;
		}
		TierPoints tierPoints;
		if (!this.Points.TryGetValue(groupSize, out tierPoints))
		{
			throw new Exception(string.Format("Bad groups size {0} playing in tier {1}", groupSize, this.Name));
		}
		if (mmr >= (float)this.MinMMRForBonus)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TierInfo.CalculatePointsForWin(int, float, float)).MethodHandle;
			}
			float num = mmr - (float)this.MinMMRForBonus;
			num = Math.Min(num, eloDeltaNeededForMaxBonus);
			num = Math.Max(1f, num);
			return tierPoints.PerWin + (int)Math.Ceiling((double)((float)this.MaxBonus * num / eloDeltaNeededForMaxBonus));
		}
		return tierPoints.PerWin;
	}

	public int CalculatePointsPerLoss(int groupSize)
	{
		if (this.PointType == TierPointsTypes.DerivedFromMMR)
		{
			return 0;
		}
		TierPoints tierPoints;
		if (this.Points.TryGetValue(groupSize, out tierPoints))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TierInfo.CalculatePointsPerLoss(int)).MethodHandle;
			}
			return tierPoints.PerLoss;
		}
		throw new Exception(string.Format("Bad groups size {0} playing in tier {1}", groupSize, this.Name));
	}

	[JsonIgnore]
	public string Name
	{
		get
		{
			if (this.NameLocalizationReference.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TierInfo.get_Name()).MethodHandle;
				}
				return null;
			}
			return this.NameLocalizationReference.Split("@".ToCharArray(), 2).First<string>();
		}
	}
}
