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
		if ((bool)m_forceFieldPrefab)
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
			if (m_initialized && m_forceFieldVFX == null && (bool)base.Target)
			{
				Vector3 bonePosition = base.Target.GetBonePosition("upperRoot_JNT");
				m_forceFieldVFX = Object.Instantiate(m_forceFieldPrefab, bonePosition, Quaternion.identity);
				m_forceFieldVFX.transform.parent = base.transform;
				ActorData[] targets = base.Targets;
				foreach (ActorData target in targets)
				{
					base.Source.OnSequenceHit(this, target, null);
				}
				if (m_shieldStart != null)
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
					base.Target.GetComponent<AudioSource>().PlayOneShot(m_shieldStart);
				}
				if (!string.IsNullOrEmpty(m_startAudioEvent))
				{
					AudioManager.PostEvent(m_startAudioEvent, m_forceFieldVFX.gameObject);
				}
			}
		}
		if (!(m_forceFieldVFX != null))
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
			if (!base.Target)
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
				Renderer[] componentsInChildren = m_forceFieldVFX.GetComponentsInChildren<Renderer>();
				Renderer[] array = componentsInChildren;
				foreach (Renderer renderer in array)
				{
					renderer.enabled = base.Target.GetActorModelDataRenderer().enabled;
				}
				Vector3 bonePosition2 = base.Target.GetBonePosition("upperRoot_JNT");
				m_forceFieldVFX.position = bonePosition2;
				return;
			}
		}
	}

	private void OnDisable()
	{
		if (!m_forceFieldVFX)
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
			if (base.Target != null && base.Target.GetComponent<AudioSource>() != null)
			{
				base.Target.GetComponent<AudioSource>().PlayOneShot(m_shieldEnd);
			}
			Object.Destroy(m_forceFieldVFX.gameObject);
			return;
		}
	}
}
