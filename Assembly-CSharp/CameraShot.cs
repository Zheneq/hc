using CameraManagerInternal;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CameraShot
{
	[Serializable]
	public class AnimParamSetAction
	{
		public string m_paramName;

		public int m_paramValue;

		public bool m_isTrigger;
	}

	public class CharacterToAnimParamSetActions
	{
		public ActorData m_actor;

		public List<AnimParamSetAction> m_animSetActions;

		public CharacterToAnimParamSetActions(ActorData actor, List<AnimParamSetAction> animSetActions)
		{
			m_actor = actor;
			m_animSetActions = animSetActions;
		}
	}

	public string m_name = "Camera Shot Name";

	[Tooltip("Set to 0 to wait until end of animation or associated event.")]
	public float m_duration = 1f;

	public float m_fieldOfView;

	[Tooltip("Only applicable if camera type is Animated")]
	public bool m_useAnimatedFov;

	public CameraType m_type;

	public CameraTransitionType m_transitionInType;

	[Header("-- Anim Param Setters On Beginning of shot --")]
	public List<AnimParamSetAction> m_animParamToSetOnBegin = new List<AnimParamSetAction>();

	[Header("-- (On End of Turn) Anim Setters --")]
	public List<AnimParamSetAction> m_animParamToSetOnEndOfTurn = new List<AnimParamSetAction>();

	private float m_time;

	private GameObject m_cameraPoseObject;

	internal void Begin(uint shotIndex, ActorData actor)
	{
		m_time = 0f;
		if (m_type != 0)
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
			CameraManager.Get().OnSpecialCameraShotBehaviorEnable(m_transitionInType);
		}
		FadeObjectsCameraComponent component = Camera.main.GetComponent<FadeObjectsCameraComponent>();
		if (component != null)
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
			component.ClearDesiredVisibleObjects();
			component.AddDesiredVisibleObject(actor.gameObject);
		}
		SetAnimParamsForActor(actor, m_animParamToSetOnBegin);
		if (m_animParamToSetOnEndOfTurn.Count > 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			CharacterToAnimParamSetActions animParamSetActions = new CharacterToAnimParamSetActions(actor, m_animParamToSetOnEndOfTurn);
			CameraManager.Get().AddAnimParamSetActions(animParamSetActions);
		}
		MixSnapshots mixerSnapshotManager = AudioManager.GetMixerSnapshotManager();
		CameraType type = m_type;
		switch (type)
		{
		case CameraType.Animated:
		{
			AnimatedCamera component3 = Camera.main.GetComponent<AnimatedCamera>();
			if (component3 == null)
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
				Camera.main.gameObject.AddComponent<AnimatedCamera>();
				component3 = Camera.main.GetComponent<AnimatedCamera>();
				Log.Warning("Missing AnimatedCamera component on main camera. Generating dynamically for now.");
			}
			m_cameraPoseObject = actor.gameObject.FindInChildren("camera" + GetCameraIndex(shotIndex));
			component3.SetAnimator(m_cameraPoseObject);
			AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_action_cam");
			AudioManager.PostEvent("Set_state_action_cam");
			if (mixerSnapshotManager != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				mixerSnapshotManager.SetMix_TauntCam();
			}
			actor.m_hideNameplate = true;
			component3.enabled = true;
			if (!(CameraManager.Get().TauntBackgroundCamera != null))
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
				CameraManager.Get().TauntBackgroundCamera.gameObject.SetActive(true);
				CameraManager.Get().TauntBackgroundCamera.SetAnimatedCameraTargetObj(m_cameraPoseObject);
				CameraManager.Get().TauntBackgroundCamera.OnCamShotStart(m_type);
				return;
			}
		}
		case CameraType.Fixed_CasterAndTargets:
		{
			Fixed_CasterAndTargetsCamera component2 = Camera.main.GetComponent<Fixed_CasterAndTargetsCamera>();
			if (component2 == null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				Camera.main.gameObject.AddComponent<Fixed_CasterAndTargetsCamera>();
				component2 = Camera.main.GetComponent<Fixed_CasterAndTargetsCamera>();
				Log.Warning("Missing Fixed_CasterAndTargetsCamera component on main camera. Generating dynamically for now.");
			}
			m_cameraPoseObject = actor.gameObject.FindInChildren("camera" + GetCameraIndex(shotIndex));
			component2.SetAnimator(m_cameraPoseObject);
			if (component != null)
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
				List<ActorData> list = SequenceManager.Get().FindSequenceTargets(actor);
				using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						if (current != null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							component.AddDesiredVisibleObject(current.gameObject);
						}
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_action_cam");
			if (mixerSnapshotManager != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				mixerSnapshotManager.SetMix_TauntCam();
			}
			component2.enabled = true;
			actor.m_hideNameplate = true;
			if (!(CameraManager.Get().TauntBackgroundCamera != null))
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
				CameraManager.Get().TauntBackgroundCamera.gameObject.SetActive(true);
				CameraManager.Get().TauntBackgroundCamera.SetFixedCasterAndTargetObj(m_cameraPoseObject);
				CameraManager.Get().TauntBackgroundCamera.OnCamShotStart(m_type);
				return;
			}
		}
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (type != 0)
			{
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			return;
		}
	}

	internal void End(ActorData actor)
	{
		FadeObjectsCameraComponent component = Camera.main.GetComponent<FadeObjectsCameraComponent>();
		if (component != null)
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
			component.ResetDesiredVisibleObjects();
		}
		actor.m_hideNameplate = false;
		CameraType type = m_type;
		if (type != CameraType.Animated)
		{
			if (type != CameraType.Fixed_CasterAndTargets)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (type != 0)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			else
			{
				Camera.main.GetComponent<Fixed_CasterAndTargetsCamera>().enabled = false;
				if (CameraManager.Get().TauntBackgroundCamera != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					CameraManager.Get().TauntBackgroundCamera.OnCamShotStop();
					CameraManager.Get().TauntBackgroundCamera.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			Camera.main.GetComponent<AnimatedCamera>().enabled = false;
			if (CameraManager.Get().TauntBackgroundCamera != null)
			{
				CameraManager.Get().TauntBackgroundCamera.OnCamShotStop();
				CameraManager.Get().TauntBackgroundCamera.gameObject.SetActive(false);
			}
		}
		AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_resolve");
		MixSnapshots mixerSnapshotManager = AudioManager.GetMixerSnapshotManager();
		if (mixerSnapshotManager != null)
		{
			mixerSnapshotManager.SetMix_ResolveCam();
		}
	}

	private float GetFieldOfView()
	{
		float result = m_fieldOfView;
		if (m_type == CameraType.Animated)
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
			if (m_useAnimatedFov)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_cameraPoseObject != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					Vector3 localScale = m_cameraPoseObject.transform.localScale;
					if (localScale.z > 1f)
					{
						Vector3 localScale2 = m_cameraPoseObject.transform.localScale;
						result = localScale2.z;
						goto IL_00ae;
					}
				}
			}
		}
		if (m_fieldOfView <= 0f)
		{
			result = CameraManager.Get().DefaultFOV;
		}
		goto IL_00ae;
		IL_00ae:
		return result;
	}

	internal static void SetAnimParamsForActor(ActorData actor, List<AnimParamSetAction> paramSetActions)
	{
		if (!(actor != null) || !(actor.GetModelAnimator() != null))
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
			if (paramSetActions != null)
			{
				foreach (AnimParamSetAction paramSetAction in paramSetActions)
				{
					if (paramSetAction.m_paramName.Length > 0)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (paramSetAction.m_isTrigger)
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
							if (paramSetAction.m_paramValue != 0)
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
								actor.GetModelAnimator().SetTrigger(paramSetAction.m_paramName);
							}
							else
							{
								actor.GetModelAnimator().ResetTrigger(paramSetAction.m_paramName);
							}
						}
						else
						{
							actor.GetModelAnimator().SetInteger(paramSetAction.m_paramName, paramSetAction.m_paramValue);
						}
					}
				}
			}
			return;
		}
	}

	internal bool Update()
	{
		Camera.main.fieldOfView = GetFieldOfView();
		m_time += Time.deltaTime;
		int result;
		if (!(m_time < m_duration))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = ((m_duration <= 0f) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public void SetElapsedTime(float time)
	{
		m_time = time;
	}

	private uint GetCameraIndex(uint shotIndex)
	{
		return shotIndex % 2u;
	}

	public string GetDebugDescription(string linePrefix)
	{
		string empty = string.Empty;
		string text = empty;
		empty = text + linePrefix + "[Shot Name] " + m_name + "\n";
		text = empty;
		empty = text + linePrefix + "[Duration] " + m_duration + "\n";
		text = empty;
		empty = text + linePrefix + "[Type] " + m_type.ToString() + "\n";
		text = empty;
		return text + linePrefix + "[Transition In Type] " + m_transitionInType.ToString() + "\n";
	}
}
