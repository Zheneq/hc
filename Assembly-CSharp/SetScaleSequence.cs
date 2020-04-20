using System;
using UnityEngine;

public class SetScaleSequence : Sequence
{
	public float m_targetScale = 1f;

	public float m_toTargetScaleSpeed;

	public float m_restoreScaleSpeed;

	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_endEvent;

	private SetScaleSequence.ScalingState m_scalingState;

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
		{
			this.StartScaling();
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.StartScaling();
		}
		else if (this.m_endEvent == parameter)
		{
			if (this.m_restoreScaleSpeed <= 0f)
			{
				this.RestoreScale();
			}
			else
			{
				this.StartRestoreScale();
			}
		}
	}

	private void Update()
	{
		if (this.m_scalingState != SetScaleSequence.ScalingState.None && base.Caster != null)
		{
			if (base.Caster.GetActorModelData() != null)
			{
				GameObject gameObject = base.Caster.GetActorModelData().gameObject;
				float x = gameObject.transform.localScale.x;
				if (this.m_scalingState == SetScaleSequence.ScalingState.ScalingToTarget)
				{
					float num = this.m_targetScale - x;
					if (Mathf.Abs(num) < 0.05f)
					{
						this.SetToTargetScale();
					}
					else
					{
						float num2;
						if (num > 0f)
						{
							num2 = Mathf.Min(this.m_targetScale, x + GameTime.deltaTime * this.m_toTargetScaleSpeed);
						}
						else
						{
							num2 = Mathf.Max(this.m_targetScale, x - GameTime.deltaTime * this.m_toTargetScaleSpeed);
						}
						float d = num2;
						base.Caster.GetActorModelData().gameObject.transform.localScale = d * Vector3.one;
					}
				}
				else if (this.m_scalingState == SetScaleSequence.ScalingState.RestoringScale)
				{
					float num3 = 1f - x;
					if (Mathf.Abs(num3) < 0.05f)
					{
						this.RestoreScale();
					}
					else
					{
						float num4;
						if (num3 > 0f)
						{
							num4 = Mathf.Min(1f, x + GameTime.deltaTime * this.m_restoreScaleSpeed);
						}
						else
						{
							num4 = Mathf.Max(1f, x - GameTime.deltaTime * this.m_restoreScaleSpeed);
						}
						float d2 = num4;
						base.Caster.GetActorModelData().gameObject.transform.localScale = d2 * Vector3.one;
					}
				}
			}
		}
	}

	private void OnDisable()
	{
		this.RestoreScale();
	}

	private void StartScaling()
	{
		if (this.m_targetScale > 0f)
		{
			if (this.m_toTargetScaleSpeed <= 0f)
			{
				this.SetToTargetScale();
			}
			else
			{
				this.m_scalingState = SetScaleSequence.ScalingState.ScalingToTarget;
			}
		}
	}

	private void SetToTargetScale()
	{
		this.m_scalingState = SetScaleSequence.ScalingState.None;
		if (this.m_targetScale > 0f && base.Caster != null)
		{
			if (base.Caster.GetActorModelData() != null)
			{
				base.Caster.GetActorModelData().gameObject.transform.localScale = this.m_targetScale * Vector3.one;
			}
		}
	}

	private void StartRestoreScale()
	{
		this.m_scalingState = SetScaleSequence.ScalingState.RestoringScale;
	}

	private void RestoreScale()
	{
		this.m_scalingState = SetScaleSequence.ScalingState.None;
		if (base.Caster != null && base.Caster.GetActorModelData() != null)
		{
			base.Caster.GetActorModelData().gameObject.transform.localScale = Vector3.one;
		}
	}

	private enum ScalingState
	{
		None,
		ScalingToTarget,
		RestoringScale
	}
}
