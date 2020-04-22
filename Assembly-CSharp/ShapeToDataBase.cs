using System;

[Serializable]
public abstract class ShapeToDataBase : IComparable
{
	public AbilityAreaShape m_shape;

	public int CompareTo(object otherAsObj)
	{
		ShapeToDataBase shapeToDataBase = otherAsObj as ShapeToDataBase;
		if (shapeToDataBase == null)
		{
			return 1;
		}
		int shape = (int)m_shape;
		return shape.CompareTo((int)shapeToDataBase.m_shape);
	}
}
