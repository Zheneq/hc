using System;
using Newtonsoft.Json;

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
			if (this.AgainstSameFreelancer == null)
			{
				if (this.MedianOfSameFreelancer == null)
				{
					if (this.AgainstRole == null && this.MedianOfRole == null)
					{
						if (this.AgainstAll == null)
						{
							if (this.MedianOfAll == null)
							{
								if (this.AgainstPeers == null)
								{
									return this.MedianOfPeers != null;
								}
							}
						}
					}
				}
			}
			return true;
		}
	}
}
