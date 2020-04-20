using System;
using UnityEngine;

public class NekoDiscReturnProjectileSequence : ArcingProjectileSequence
{
	public string m_animParamToSet = "IdleType";

	public int m_animParamValue = -1;

	private bool m_setAnimDistParam;

	private bool m_shouldSetForNormalDiscParam;

	private static readonly int animDistToGoal = Animator.StringToHash("DistToGoal");

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			NekoDiscReturnProjectileSequence.DiscReturnProjectileExtraParams discReturnProjectileExtraParams = extraSequenceParams as NekoDiscReturnProjectileSequence.DiscReturnProjectileExtraParams;
			if (discReturnProjectileExtraParams != null)
			{
				this.m_setAnimDistParam = discReturnProjectileExtraParams.setAnimDistParamWithThisProjectile;
				this.m_shouldSetForNormalDiscParam = discReturnProjectileExtraParams.setAnimParamForNormalDisc;
			}
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.m_setAnimDistParam)
		{
			if (this.m_splineTraveled < this.m_splineFractionUntilImpact)
			{
				Animator modelAnimator = base.Caster.GetModelAnimator();
				modelAnimator.SetFloat(NekoDiscReturnProjectileSequence.animDistToGoal, this.m_totalTravelDist2D * (this.m_splineFractionUntilImpact - this.m_splineTraveled));
			}
		}
	}

	protected override void SpawnFX()
	{
		base.SpawnFX();
		if (this.m_shouldSetForNormalDiscParam)
		{
			Animator modelAnimator = base.Caster.GetModelAnimator();
			if (!this.m_animParamToSet.IsNullOrEmpty())
			{
				if (modelAnimator != null)
				{
					modelAnimator.SetInteger(this.m_animParamToSet, this.m_animParamValue);
				}
			}
		}
	}

	protected override void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		base.SpawnImpactFX(impactPos, impactRot);
		if (this.m_setAnimDistParam)
		{
			Animator modelAnimator = base.Caster.GetModelAnimator();
			modelAnimator.SetFloat(NekoDiscReturnProjectileSequence.animDistToGoal, 0f);
		}
	}

	public class DiscReturnProjectileExtraParams : Sequence.IExtraSequenceParams
	{
		public bool setAnimDistParamWithThisProjectile;

		public bool setAnimParamForNormalDisc;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.setAnimDistParamWithThisProjectile);
			stream.Serialize(ref this.setAnimParamForNormalDisc);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.setAnimDistParamWithThisProjectile);
			stream.Serialize(ref this.setAnimParamForNormalDisc);
		}
	}
}
