using System;

public static class GlyphTag
{
	public static bool IsHandleWithoutNumber(string handle)
	{
		bool result;
		if (!handle.IsNullOrEmpty())
		{
			result = (handle.LastIndexOf("#") == -1);
		}
		else
		{
			result = false;
		}
		return result;
	}
}
