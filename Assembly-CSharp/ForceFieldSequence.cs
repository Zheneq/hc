﻿using System;
using UnityEngine;

public class ForceFieldSequence : Sequence
{
	public Transform m_forceFieldPrefab;

	private Transform m_forceFieldVFX;

	public AudioClip m_shieldStart;

	public AudioClip m_shieldEnd;

	[AudioEvent(false)]
	public string m_startAudioEvent;

	private void Update()
	{
		if (this.m_forceFieldPrefab)
		{
			if (this.m_initialized && this.m_forceFieldVFX == null && base.Target)
			{
				Vector3 bonePosition = base.Target.GetBonePosition("upperRoot_JNT");
				this.m_forceFieldVFX = UnityEngine.Object.Instantiate<Transform>(this.m_forceFieldPrefab, bonePosition, Quaternion.identity);
				this.m_forceFieldVFX.transform.parent = base.transform;
				foreach (ActorData target in base.Targets)
				{
					base.Source.OnSequenceHit(this, target, null, ActorModelData.RagdollActivation.HealthBased, true);
				}
				if (this.m_shieldStart != null)
				{
					base.Target.GetComponent<AudioSource>().PlayOneShot(this.m_shieldStart);
				}
				if (!string.IsNullOrEmpty(this.m_startAudioEvent))
				{
					AudioManager.PostEvent(this.m_startAudioEvent, this.m_forceFieldVFX.gameObject);
				}
			}
		}
		if (this.m_forceFieldVFX != null)
		{
			if (base.Target)
			{
				Renderer[] componentsInChildren = this.m_forceFieldVFX.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer in componentsInChildren)
				{
					renderer.enabled = base.Target.GetActorModelDataRenderer().enabled;
				}
				Vector3 bonePosition2 = base.Target.GetBonePosition("upperRoot_JNT");
				this.m_forceFieldVFX.position = bonePosition2;
			}
		}
	}

	private void OnDisable()
	{
		if (this.m_forceFieldVFX)
		{
			if (base.Target != null && base.Target.GetComponent<AudioSource>() != null)
			{
				base.Target.GetComponent<AudioSource>().PlayOneShot(this.m_shieldEnd);
			}
			UnityEngine.Object.Destroy(this.m_forceFieldVFX.gameObject);
		}
	}
}
