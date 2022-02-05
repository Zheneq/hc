// ROGUES
// SERVER
namespace AbilityContextNamespace
{
	public class ContextKeys
	{
		// removed in rogues
		public static ContextNameKeyPair s_AngleFromCenter = new ContextNameKeyPair("AngleFromCenter");
		public static ContextNameKeyPair s_BarrierWidth = new ContextNameKeyPair("BarrierWidth");
		// removed in rogues
		public static ContextNameKeyPair s_CasterLowHealth = new ContextNameKeyPair("CasterLowHealth");
		public static ContextNameKeyPair s_CenterPos = new ContextNameKeyPair("CenterPos");
		// removed in rogues
		public static ContextNameKeyPair s_ChargeEndPos = new ContextNameKeyPair("ChargeEndPos");
		public static ContextNameKeyPair s_DistFromMin = new ContextNameKeyPair("DistFromMin");
		public static ContextNameKeyPair s_DistFromStart = new ContextNameKeyPair("DistFromStart");
		public static ContextNameKeyPair s_FacingDir = new ContextNameKeyPair("FacingDir");
		public static ContextNameKeyPair s_HitCount = new ContextNameKeyPair("HitCount");
		public static ContextNameKeyPair s_HitOrder = new ContextNameKeyPair("HitOrder");
		public static ContextNameKeyPair s_InAoe = new ContextNameKeyPair("InAoe");
		// removed in rogues
		public static ContextNameKeyPair s_InEndAoe = new ContextNameKeyPair("InEndAoe");
		public static ContextNameKeyPair s_LaserEndPos = new ContextNameKeyPair("LaserEndPos");
		public static ContextNameKeyPair s_Layer = new ContextNameKeyPair("Layer");
		public static ContextNameKeyPair s_LayersActive = new ContextNameKeyPair("LayersActive");
		// removed in rogues
		public static ContextNameKeyPair s_NumEnemies = new ContextNameKeyPair("NumEnemies");
		public static ContextNameKeyPair s_Radius = new ContextNameKeyPair("Radius");

		// added in rogues
#if SERVER
		public static ContextNameKeyPair s_InLaser = new ContextNameKeyPair("InLaser");
		public static ContextNameKeyPair s_PercentHealthLost = new ContextNameKeyPair("PercentHealthLost");
		public static ContextNameKeyPair s_TargetHealthPercentage = new ContextNameKeyPair("TargetHealthPercentage");
		public static ContextNameKeyPair s_TargeterIndex = new ContextNameKeyPair("TargeterIndex");
		public static ContextNameKeyPair s_SegmentID = new ContextNameKeyPair("SegmentID");
		public static ContextNameKeyPair s_EndpointIndex = new ContextNameKeyPair("EndpointIndex");
		public static ContextNameKeyPair s_ScalePercent = new ContextNameKeyPair("ScalePercent");
		public static ContextNameKeyPair s_FacingPerpDir = new ContextNameKeyPair("FacingPerpDir");
		public static ContextNameKeyPair s_NumEnemyHits = new ContextNameKeyPair("NumEnemyHits");
		public static ContextNameKeyPair s_NumAllyHits = new ContextNameKeyPair("NumAllyHits");
		public static ContextNameKeyPair s_TargetHitPos = new ContextNameKeyPair("TargetHitPos");
		public static ContextNameKeyPair s_directHitSquareCount = new ContextNameKeyPair("DirectHitSquareCount");
		public static ContextNameKeyPair s_hitsWithinPrimaryRadius = new ContextNameKeyPair("HitsWithinPrimaryRadius");
		public static ContextNameKeyPair s_KnockbackOrigin = new ContextNameKeyPair("KnockbackOrigin");
#endif

		// added in rogues
#if SERVER
		public static bool IsNonActorSpecific(int key)
		{
			return key == s_BarrierWidth.GetKey()
				|| key == s_CenterPos.GetKey()
				|| key == s_FacingDir.GetKey()
				|| key == s_LaserEndPos.GetKey()
				|| key == s_NumEnemyHits.GetKey()
				|| key == s_NumAllyHits.GetKey()
				|| key == s_LayersActive.GetKey()
				|| key == s_Radius.GetKey()
				|| key == s_TargetHitPos.GetKey()
				|| key == s_directHitSquareCount.GetKey()
				|| key == s_hitsWithinPrimaryRadius.GetKey();
		}
#endif
	}
}
