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
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2 != null)
			{
				while (true)
				{
					switch (7)
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
				m_aimDirection = base.Caster.GetActorCover().GetCoverOffset((ActorCover.CoverDirections)extraParams2.m_aimDirection);
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
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
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			InitializeFXStorage();
			m_placeholderShieldIndicator = new GameObject("Valkyrie_ShieldPlaceholder");
			GameObject fxParentObject = GetFxParentObject();
			m_placeholderShieldIndicator.transform.parent = fxParentObject.transform;
			m_placeholderShieldIndicator.transform.localRotation = Quaternion.identity;
			GameObject gameObject = HighlightUtils.Get().CreateConeCursor(0.7f, 360f);
			gameObject.transform.parent = m_placeholderShieldIndicator.transform;
			gameObject.transform.localPosition = 0.45f * Board.Get().squareSize * Vector3.forward;
			gameObject.transform.localRotation = Quaternion.LookRotation(Vector3.up);
			return;
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
			while (true)
			{
				switch (3)
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
			m_fx.transform.rotation = Quaternion.LookRotation(m_aimDirection);
		}
		if (!(m_placeholderShieldIndicator != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			m_placeholderShieldIndicator.transform.position = base.Caster.transform.position + 2f * Vector3.up;
			m_placeholderShieldIndicator.transform.rotation = Quaternion.LookRotation(m_aimDirection);
			return;
		}
	}
}
