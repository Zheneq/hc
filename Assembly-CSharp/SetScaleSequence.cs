using UnityEngine;

public class SetScaleSequence : Sequence
{
	private enum ScalingState
	{
		None,
		ScalingToTarget,
		RestoringScale
	}

	public float m_targetScale = 1f;

	public float m_toTargetScaleSpeed;

	public float m_restoreScaleSpeed;

	[AnimEventPicker]
	public Object m_startEvent;

	[AnimEventPicker]
	public Object m_endEvent;

	private ScalingState m_scalingState;

	public override void FinishSetup()
	{
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			StartScaling();
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					StartScaling();
					return;
				}
			}
		}
		if (!(m_endEvent == parameter))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (m_restoreScaleSpeed <= 0f)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						RestoreScale();
						return;
					}
				}
			}
			StartRestoreScale();
			return;
		}
	}

	private void Update()
	{
		if (m_scalingState == ScalingState.None || !(base.Caster != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(base.Caster.GetActorModelData() != null))
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				GameObject gameObject = base.Caster.GetActorModelData().gameObject;
				Vector3 localScale = gameObject.transform.localScale;
				float x = localScale.x;
				if (m_scalingState == ScalingState.ScalingToTarget)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
						{
							float num = m_targetScale - x;
							if (Mathf.Abs(num) < 0.05f)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										SetToTargetScale();
										return;
									}
								}
							}
							float num2;
							if (num > 0f)
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
								num2 = Mathf.Min(m_targetScale, x + GameTime.deltaTime * m_toTargetScaleSpeed);
							}
							else
							{
								num2 = Mathf.Max(m_targetScale, x - GameTime.deltaTime * m_toTargetScaleSpeed);
							}
							float d = num2;
							base.Caster.GetActorModelData().gameObject.transform.localScale = d * Vector3.one;
							return;
						}
						}
					}
				}
				if (m_scalingState != ScalingState.RestoringScale)
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
					float num3 = 1f - x;
					if (Mathf.Abs(num3) < 0.05f)
					{
						RestoreScale();
						return;
					}
					float num4;
					if (num3 > 0f)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						num4 = Mathf.Min(1f, x + GameTime.deltaTime * m_restoreScaleSpeed);
					}
					else
					{
						num4 = Mathf.Max(1f, x - GameTime.deltaTime * m_restoreScaleSpeed);
					}
					float d2 = num4;
					base.Caster.GetActorModelData().gameObject.transform.localScale = d2 * Vector3.one;
					return;
				}
			}
		}
	}

	private void OnDisable()
	{
		RestoreScale();
	}

	private void StartScaling()
	{
		if (!(m_targetScale > 0f))
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
			if (m_toTargetScaleSpeed <= 0f)
			{
				SetToTargetScale();
			}
			else
			{
				m_scalingState = ScalingState.ScalingToTarget;
			}
			return;
		}
	}

	private void SetToTargetScale()
	{
		m_scalingState = ScalingState.None;
		if (!(m_targetScale > 0f) || !(base.Caster != null))
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
			if (base.Caster.GetActorModelData() != null)
			{
				base.Caster.GetActorModelData().gameObject.transform.localScale = m_targetScale * Vector3.one;
			}
			return;
		}
	}

	private void StartRestoreScale()
	{
		m_scalingState = ScalingState.RestoringScale;
	}

	private void RestoreScale()
	{
		m_scalingState = ScalingState.None;
		if (!(base.Caster != null) || !(base.Caster.GetActorModelData() != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			base.Caster.GetActorModelData().gameObject.transform.localScale = Vector3.one;
			return;
		}
	}
}
