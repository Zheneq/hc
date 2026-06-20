using UnityEngine;

public class NekoDiscReturnProjectileSequence : ArcingProjectileSequence
{
	public class DiscReturnProjectileExtraParams : IExtraSequenceParams
	{
		public bool setAnimDistParamWithThisProjectile;
		public bool setAnimParamForNormalDisc;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref setAnimDistParamWithThisProjectile);
			stream.Serialize(ref setAnimParamForNormalDisc);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref setAnimDistParamWithThisProjectile);
			stream.Serialize(ref setAnimParamForNormalDisc);
		}
	}

	public string m_animParamToSet = "IdleType";
	public int m_animParamValue = -1;

	private bool m_setAnimDistParam;
	private bool m_shouldSetForNormalDiscParam;
	private static readonly int animDistToGoal = Animator.StringToHash("DistToGoal");

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is DiscReturnProjectileExtraParams discReturnProjectileExtraParams)
			{
				m_setAnimDistParam = discReturnProjectileExtraParams.setAnimDistParamWithThisProjectile;
				m_shouldSetForNormalDiscParam = discReturnProjectileExtraParams.setAnimParamForNormalDisc;
			}
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (m_setAnimDistParam && m_splineTraveled < m_splineFractionUntilImpact)
		{
			Animator modelAnimator = Caster.GetModelAnimator();
			modelAnimator.SetFloat(animDistToGoal, m_totalTravelDist2D * (m_splineFractionUntilImpact - m_splineTraveled));
		}
	}

	protected override void SpawnFX()
	{
		base.SpawnFX();
		if (m_shouldSetForNormalDiscParam)
		{
			Animator modelAnimator = Caster.GetModelAnimator();
			if (!m_animParamToSet.IsNullOrEmpty() && modelAnimator != null)
			{
				modelAnimator.SetInteger(m_animParamToSet, m_animParamValue);
			}
		}
	}

	protected override void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		base.SpawnImpactFX(impactPos, impactRot);
		if (m_setAnimDistParam)
		{
			Animator modelAnimator = Caster.GetModelAnimator();
			modelAnimator.SetFloat(animDistToGoal, 0f);
		}
	}
}
