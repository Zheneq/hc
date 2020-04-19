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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RadiusToDataBase.CompareTo(object)).MethodHandle;
			}
			return 1;
		}
		return ((int)this.m_radius).CompareTo((int)radiusToDataBase.m_radius);
	}
}
