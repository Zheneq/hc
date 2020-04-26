using Newtonsoft.Json;
using System;

[Serializable]
public class PercentileInfo
{
	public int? AgainstSameFreelancer;

	public int? AgainstRole;

	public int? AgainstAll;

	public int? AgainstPeers;

	public float? MedianOfSameFreelancer;

	public float? MedianOfRole;

	public float? MedianOfAll;

	public float? MedianOfPeers;

	[JsonIgnore]
	public bool HasData
	{
		get
		{
			int result;
			if (!AgainstSameFreelancer.HasValue)
			{
				if (!MedianOfSameFreelancer.HasValue)
				{
					if (!AgainstRole.HasValue && !MedianOfRole.HasValue)
					{
						if (!AgainstAll.HasValue)
						{
							if (!MedianOfAll.HasValue)
							{
								if (!AgainstPeers.HasValue)
								{
									result = (MedianOfPeers.HasValue ? 1 : 0);
									goto IL_00c0;
								}
							}
						}
					}
				}
			}
			result = 1;
			goto IL_00c0;
			IL_00c0:
			return (byte)result != 0;
		}
	}
}
