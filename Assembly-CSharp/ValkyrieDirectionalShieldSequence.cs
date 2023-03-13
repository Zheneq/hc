using UnityEngine;

public class ValkyrieDirectionalShieldSequence : SimpleAttachedVFXSequence
{
	public class ExtraParams : IExtraSequenceParams
	{
		public sbyte m_aimDirection;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref m_aimDirection);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref m_aimDirection);
		}
	}

	public bool m_useTempShieldIndicator = true;

	private Vector3 m_aimDirection;

	private GameObject m_placeholderShieldIndicator;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		for (int i = 0; i < extraParams.Length; i++)
		{
            ExtraParams extraParams2 = extraParams[i] as ExtraParams;
			if (extraParams2 != null)
			{
				m_aimDirection = base.Caster.GetActorCover().GetCoverOffset((ActorCover.CoverDirections)extraParams2.m_aimDirection);
			}
		}
	}

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (m_useTempShieldIndicator)
		{
			base.InitializeFXStorage();
			m_placeholderShieldIndicator = new GameObject("Valkyrie_ShieldPlaceholder");
			GameObject fxParentObject = base.GetFxParentObject();
			m_placeholderShieldIndicator.transform.parent = fxParentObject.transform;
			m_placeholderShieldIndicator.transform.localRotation = Quaternion.identity;
			GameObject gameObject = HighlightUtils.Get().CreateConeCursor(0.7f, 360f);
			gameObject.transform.parent = m_placeholderShieldIndicator.transform;
			gameObject.transform.localPosition = 0.45f * Board.Get().squareSize * Vector3.forward;
			gameObject.transform.localRotation = Quaternion.LookRotation(Vector3.up);
		}
	}

	private void OnDisable()
	{
		if (m_placeholderShieldIndicator != null)
		{
			m_placeholderShieldIndicator.SetActive(false);
		}
		if (m_fx != null)
		{
			m_fx.SetActive(false);
		}
	}

	protected override void SetFxRotation()
	{
		if (m_fx != null)
		{
			m_fx.transform.rotation = Quaternion.LookRotation(m_aimDirection);
		}
		if (m_placeholderShieldIndicator != null)
		{
			m_placeholderShieldIndicator.transform.position = base.Caster.transform.position + 2f * Vector3.up;
			m_placeholderShieldIndicator.transform.rotation = Quaternion.LookRotation(m_aimDirection);
		}
	}
}
