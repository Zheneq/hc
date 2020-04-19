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
					RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscReturnProjectileSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				this.m_setAnimDistParam = discReturnProjectileExtraParams.setAnimDistParamWithThisProjectile;
				this.m_shouldSetForNormalDiscParam = discReturnProjectileExtraParams.setAnimParamForNormalDisc;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.m_setAnimDistParam)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscReturnProjectileSequence.OnUpdate()).MethodHandle;
			}
			if (this.m_splineTraveled < this.m_splineFractionUntilImpact)
			{
				Animator animator = base.Caster.\u000E();
				animator.SetFloat(NekoDiscReturnProjectileSequence.animDistToGoal, this.m_totalTravelDist2D * (this.m_splineFractionUntilImpact - this.m_splineTraveled));
			}
		}
	}

	protected override void SpawnFX()
	{
		base.SpawnFX();
		if (this.m_shouldSetForNormalDiscParam)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscReturnProjectileSequence.SpawnFX()).MethodHandle;
			}
			Animator animator = base.Caster.\u000E();
			if (!this.m_animParamToSet.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (animator != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					animator.SetInteger(this.m_animParamToSet, this.m_animParamValue);
				}
			}
		}
	}

	protected override void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		base.SpawnImpactFX(impactPos, impactRot);
		if (this.m_setAnimDistParam)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscReturnProjectileSequence.SpawnImpactFX(Vector3, Quaternion)).MethodHandle;
			}
			Animator animator = base.Caster.\u000E();
			animator.SetFloat(NekoDiscReturnProjectileSequence.animDistToGoal, 0f);
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
