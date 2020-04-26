public static class GlyphTag
{
	public static bool IsHandleWithoutNumber(string handle)
	{
		int result;
		if (!handle.IsNullOrEmpty())
		{
			result = ((handle.LastIndexOf("#") == -1) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}
}
