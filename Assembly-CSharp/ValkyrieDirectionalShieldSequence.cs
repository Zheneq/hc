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

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ExtraParams ep = extraSequenceParams as ExtraParams;
			if (ep != null)
			{
				m_aimDirection = Caster.GetActorCover().GetCoverOffset((ActorCover.CoverDirections)ep.m_aimDirection);
			}
		}
	}

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (!m_useTempShieldIndicator)
		{
			return;
		}
		InitializeFXStorage();
		m_placeholderShieldIndicator = new GameObject("Valkyrie_ShieldPlaceholder");
		GameObject fxParentObject = GetFxParentObject();
		m_placeholderShieldIndicator.transform.parent = fxParentObject.transform;
		m_placeholderShieldIndicator.transform.localRotation = Quaternion.identity;
		GameObject cursor = HighlightUtils.Get().CreateConeCursor(0.7f, 360f);
		cursor.transform.parent = m_placeholderShieldIndicator.transform;
		cursor.transform.localPosition = 0.45f * Board.Get().squareSize * Vector3.forward;
		cursor.transform.localRotation = Quaternion.LookRotation(Vector3.up);
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
			m_placeholderShieldIndicator.transform.position = Caster.transform.position + 2f * Vector3.up;
			m_placeholderShieldIndicator.transform.rotation = Quaternion.LookRotation(m_aimDirection);
		}
	}
}
