using System;

[Serializable]
public abstract class RadiusToDataBase : IComparable
{
	public float m_radius;

	public int CompareTo(object otherAsObj)
	{
		RadiusToDataBase radiusToDataBase = otherAsObj as RadiusToDataBase;
		if (radiusToDataBase == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return 1;
				}
			}
		}
		return ((int)m_radius).CompareTo((int)radiusToDataBase.m_radius);
	}
}
