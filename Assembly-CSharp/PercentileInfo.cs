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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(PercentileInfo.get_HasData()).MethodHandle;
				}
				if (this.MedianOfSameFreelancer == null)
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
					if (this.AgainstRole == null && this.MedianOfRole == null)
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
						if (this.AgainstAll == null)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.MedianOfAll == null)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (this.AgainstPeers == null)
								{
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
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
