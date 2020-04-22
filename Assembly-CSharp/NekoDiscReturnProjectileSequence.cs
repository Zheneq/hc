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
			DiscReturnProjectileExtraParams discReturnProjectileExtraParams = extraSequenceParams as DiscReturnProjectileExtraParams;
			if (discReturnProjectileExtraParams != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_setAnimDistParam = discReturnProjectileExtraParams.setAnimDistParamWithThisProjectile;
				m_shouldSetForNormalDiscParam = discReturnProjectileExtraParams.setAnimParamForNormalDisc;
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!m_setAnimDistParam)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_splineTraveled < m_splineFractionUntilImpact)
			{
				Animator modelAnimator = base.Caster.GetModelAnimator();
				modelAnimator.SetFloat(animDistToGoal, m_totalTravelDist2D * (m_splineFractionUntilImpact - m_splineTraveled));
			}
			return;
		}
	}

	protected override void SpawnFX()
	{
		base.SpawnFX();
		if (!m_shouldSetForNormalDiscParam)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Animator modelAnimator = base.Caster.GetModelAnimator();
			if (m_animParamToSet.IsNullOrEmpty())
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (modelAnimator != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						modelAnimator.SetInteger(m_animParamToSet, m_animParamValue);
						return;
					}
				}
				return;
			}
		}
	}

	protected override void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		base.SpawnImpactFX(impactPos, impactRot);
		if (!m_setAnimDistParam)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Animator modelAnimator = base.Caster.GetModelAnimator();
			modelAnimator.SetFloat(animDistToGoal, 0f);
			return;
		}
	}
}
