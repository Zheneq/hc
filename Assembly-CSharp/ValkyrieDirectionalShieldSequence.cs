using System;
using UnityEngine;

public class ValkyrieDirectionalShieldSequence : SimpleAttachedVFXSequence
{
	public bool m_useTempShieldIndicator = true;

	private Vector3 m_aimDirection;

	private GameObject m_placeholderShieldIndicator;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ValkyrieDirectionalShieldSequence.ExtraParams extraParams2 = extraSequenceParams as ValkyrieDirectionalShieldSequence.ExtraParams;
			if (extraParams2 != null)
			{
				this.m_aimDirection = base.Caster.GetActorCover().GetCoverOffset((ActorCover.CoverDirections)extraParams2.m_aimDirection);
			}
		}
	}

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (this.m_useTempShieldIndicator)
		{
			base.InitializeFXStorage();
			this.m_placeholderShieldIndicator = new GameObject("Valkyrie_ShieldPlaceholder");
			GameObject fxParentObject = base.GetFxParentObject();
			this.m_placeholderShieldIndicator.transform.parent = fxParentObject.transform;
			this.m_placeholderShieldIndicator.transform.localRotation = Quaternion.identity;
			GameObject gameObject = HighlightUtils.Get().CreateConeCursor(0.7f, 360f);
			gameObject.transform.parent = this.m_placeholderShieldIndicator.transform;
			gameObject.transform.localPosition = 0.45f * Board.Get().squareSize * Vector3.forward;
			gameObject.transform.localRotation = Quaternion.LookRotation(Vector3.up);
		}
	}

	private void OnDisable()
	{
		if (this.m_placeholderShieldIndicator != null)
		{
			this.m_placeholderShieldIndicator.SetActive(false);
		}
		if (this.m_fx != null)
		{
			this.m_fx.SetActive(false);
		}
	}

	protected override void SetFxRotation()
	{
		if (this.m_fx != null)
		{
			this.m_fx.transform.rotation = Quaternion.LookRotation(this.m_aimDirection);
		}
		if (this.m_placeholderShieldIndicator != null)
		{
			this.m_placeholderShieldIndicator.transform.position = base.Caster.transform.position + 2f * Vector3.up;
			this.m_placeholderShieldIndicator.transform.rotation = Quaternion.LookRotation(this.m_aimDirection);
		}
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public sbyte m_aimDirection;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.m_aimDirection);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.m_aimDirection);
		}
	}
}
