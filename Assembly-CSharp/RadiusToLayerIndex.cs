using System;
using System.Collections.Generic;

[Serializable]
public class RadiusToLayerIndex : RadiusToDataBase
{
	public int m_index;

	public RadiusToLayerIndex(float radius)
	{
		m_radius = radius;
	}

	public static void SortAndSetLayerIndex(List<RadiusToLayerIndex> radiusList)
	{
		radiusList.Sort();
		for (int i = 0; i < radiusList.Count; i++)
		{
			radiusList[i].m_index = i;
		}
		while (true)
		{
			return;
		}
	}
}
