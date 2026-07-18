public static class EffectDebugConfig
{
	public static bool m_traceAddAndRemove;

	public const string EFFECT_DEBUG_HEADER = "<color=green>Effect</color>: ";

	public static bool TracingAddAndRemove()
	{
#if SERVER
		return true; // custom
#else
        return false; // reactor
#endif
	}
}
