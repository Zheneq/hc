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
				if (!MedianOfSameFreelancer.HasValue)
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
					if (!AgainstRole.HasValue && !MedianOfRole.HasValue)
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
						if (!AgainstAll.HasValue)
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
							if (!MedianOfAll.HasValue)
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
								if (!AgainstPeers.HasValue)
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
