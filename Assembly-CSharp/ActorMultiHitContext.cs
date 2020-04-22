using UnityEngine;

public class ActorMultiHitContext
{
	public int m_numHits;

	public int m_numHitsFromCover;

	public Vector3 m_hitOrigin = Vector3.zero;

	public static int CalcDamageFromNumHits(int numHits, int numFromCover, int baseDamage, int subseqDamage)
	{
		int num = Mathf.Max(0, baseDamage);
		int num2 = Mathf.Max(0, subseqDamage);
		float coverProtectionDmgMultiplier = GameplayData.Get().m_coverProtectionDmgMultiplier;
		int num3 = 0;
		if (numHits == numFromCover)
		{
			num3 = Mathf.RoundToInt(coverProtectionDmgMultiplier * (float)(num + (numHits - 1) * num2));
		}
		else if (numFromCover == 0)
		{
			num3 = num + (numHits - 1) * num2;
		}
		else
		{
			num3 = num + Mathf.RoundToInt(coverProtectionDmgMultiplier * (float)(numHits - 1) * (float)num2);
		}
		return Mathf.Max(0, num3);
	}
}
