using System;
using System.Collections.Generic;
using CameraManagerInternal;
using UnityEngine;

[Serializable]
public class CameraShot
{
	public string m_name = "Camera Shot Name";

	[Tooltip("Set to 0 to wait until end of animation or associated event.")]
	public float m_duration = 1f;

	public float m_fieldOfView;

	[Tooltip("Only applicable if camera type is Animated")]
	public bool m_useAnimatedFov;

	public global::CameraType m_type;

	public CameraTransitionType m_transitionInType;

	[Header("-- Anim Param Setters On Beginning of shot --")]
	public List<CameraShot.AnimParamSetAction> m_animParamToSetOnBegin = new List<CameraShot.AnimParamSetAction>();

	[Header("-- (On End of Turn) Anim Setters --")]
	public List<CameraShot.AnimParamSetAction> m_animParamToSetOnEndOfTurn = new List<CameraShot.AnimParamSetAction>();

	private float m_time;

	private GameObject m_cameraPoseObject;

	internal void Begin(uint shotIndex, ActorData actor)
	{
		this.m_time = 0f;
		if (this.m_type != global::CameraType.Isometric)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraShot.Begin(uint, ActorData)).MethodHandle;
			}
			CameraManager.Get().OnSpecialCameraShotBehaviorEnable(this.m_transitionInType);
		}
		FadeObjectsCameraComponent component = Camera.main.GetComponent<FadeObjectsCameraComponent>();
		if (component != null)
		{
			for (;;)
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
		CameraShot.SetAnimParamsForActor(actor, this.m_animParamToSetOnBegin);
		if (this.m_animParamToSetOnEndOfTurn.Count > 0)
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
			CameraShot.CharacterToAnimParamSetActions animParamSetActions = new CameraShot.CharacterToAnimParamSetActions(actor, this.m_animParamToSetOnEndOfTurn);
			CameraManager.Get().AddAnimParamSetActions(animParamSetActions);
		}
		MixSnapshots mixerSnapshotManager = AudioManager.GetMixerSnapshotManager();
		global::CameraType type = this.m_type;
		if (type != global::CameraType.Animated)
		{
			if (type != global::CameraType.Fixed_CasterAndTargets)
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
				if (type != global::CameraType.Isometric)
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
				}
			}
			else
			{
				Fixed_CasterAndTargetsCamera component2 = Camera.main.GetComponent<Fixed_CasterAndTargetsCamera>();
				if (component2 == null)
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
					Camera.main.gameObject.AddComponent<Fixed_CasterAndTargetsCamera>();
					component2 = Camera.main.GetComponent<Fixed_CasterAndTargetsCamera>();
					Log.Warning("Missing Fixed_CasterAndTargetsCamera component on main camera. Generating dynamically for now.", new object[0]);
				}
				this.m_cameraPoseObject = actor.gameObject.FindInChildren("camera" + this.GetCameraIndex(shotIndex), 0);
				component2.SetAnimator(this.m_cameraPoseObject);
				if (component != null)
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
					List<ActorData> list = SequenceManager.Get().FindSequenceTargets(actor);
					using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData actorData = enumerator.Current;
							if (actorData != null)
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
								component.AddDesiredVisibleObject(actorData.gameObject);
							}
						}
						for (;;)
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
				AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_action_cam", null);
				if (mixerSnapshotManager != null)
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
					mixerSnapshotManager.SetMix_TauntCam();
				}
				component2.enabled = true;
				actor.m_hideNameplate = true;
				if (CameraManager.Get().TauntBackgroundCamera != null)
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
					CameraManager.Get().TauntBackgroundCamera.gameObject.SetActive(true);
					CameraManager.Get().TauntBackgroundCamera.SetFixedCasterAndTargetObj(this.m_cameraPoseObject);
					CameraManager.Get().TauntBackgroundCamera.OnCamShotStart(this.m_type);
				}
			}
		}
		else
		{
			AnimatedCamera component3 = Camera.main.GetComponent<AnimatedCamera>();
			if (component3 == null)
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
				Camera.main.gameObject.AddComponent<AnimatedCamera>();
				component3 = Camera.main.GetComponent<AnimatedCamera>();
				Log.Warning("Missing AnimatedCamera component on main camera. Generating dynamically for now.", new object[0]);
			}
			this.m_cameraPoseObject = actor.gameObject.FindInChildren("camera" + this.GetCameraIndex(shotIndex), 0);
			component3.SetAnimator(this.m_cameraPoseObject);
			AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_action_cam", null);
			AudioManager.PostEvent("Set_state_action_cam", null);
			if (mixerSnapshotManager != null)
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
				mixerSnapshotManager.SetMix_TauntCam();
			}
			actor.m_hideNameplate = true;
			component3.enabled = true;
			if (CameraManager.Get().TauntBackgroundCamera != null)
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
				CameraManager.Get().TauntBackgroundCamera.gameObject.SetActive(true);
				CameraManager.Get().TauntBackgroundCamera.SetAnimatedCameraTargetObj(this.m_cameraPoseObject);
				CameraManager.Get().TauntBackgroundCamera.OnCamShotStart(this.m_type);
			}
		}
	}

	internal void End(ActorData actor)
	{
		FadeObjectsCameraComponent component = Camera.main.GetComponent<FadeObjectsCameraComponent>();
		if (component != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraShot.End(ActorData)).MethodHandle;
			}
			component.ResetDesiredVisibleObjects();
		}
		actor.m_hideNameplate = false;
		global::CameraType type = this.m_type;
		if (type != global::CameraType.Animated)
		{
			if (type != global::CameraType.Fixed_CasterAndTargets)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (type != global::CameraType.Isometric)
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
				}
			}
			else
			{
				Camera.main.GetComponent<Fixed_CasterAndTargetsCamera>().enabled = false;
				if (CameraManager.Get().TauntBackgroundCamera != null)
				{
					for (;;)
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
		AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_resolve", null);
		MixSnapshots mixerSnapshotManager = AudioManager.GetMixerSnapshotManager();
		if (mixerSnapshotManager != null)
		{
			mixerSnapshotManager.SetMix_ResolveCam();
		}
	}

	private float GetFieldOfView()
	{
		float result = this.m_fieldOfView;
		if (this.m_type == global::CameraType.Animated)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraShot.GetFieldOfView()).MethodHandle;
			}
			if (this.m_useAnimatedFov)
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
				if (this.m_cameraPoseObject != null)
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
					if (this.m_cameraPoseObject.transform.localScale.z > 1f)
					{
						return this.m_cameraPoseObject.transform.localScale.z;
					}
				}
			}
		}
		if (this.m_fieldOfView <= 0f)
		{
			result = CameraManager.Get().DefaultFOV;
		}
		return result;
	}

	internal static void SetAnimParamsForActor(ActorData actor, List<CameraShot.AnimParamSetAction> paramSetActions)
	{
		if (actor != null && actor.GetModelAnimator() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraShot.SetAnimParamsForActor(ActorData, List<CameraShot.AnimParamSetAction>)).MethodHandle;
			}
			if (paramSetActions != null)
			{
				foreach (CameraShot.AnimParamSetAction animParamSetAction in paramSetActions)
				{
					if (animParamSetAction.m_paramName.Length > 0)
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
						if (animParamSetAction.m_isTrigger)
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
							if (animParamSetAction.m_paramValue != 0)
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
								actor.GetModelAnimator().SetTrigger(animParamSetAction.m_paramName);
							}
							else
							{
								actor.GetModelAnimator().ResetTrigger(animParamSetAction.m_paramName);
							}
						}
						else
						{
							actor.GetModelAnimator().SetInteger(animParamSetAction.m_paramName, animParamSetAction.m_paramValue);
						}
					}
				}
			}
		}
	}

	internal bool Update()
	{
		Camera.main.fieldOfView = this.GetFieldOfView();
		this.m_time += Time.deltaTime;
		bool result;
		if (this.m_time >= this.m_duration)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraShot.Update()).MethodHandle;
			}
			result = (this.m_duration <= 0f);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void SetElapsedTime(float time)
	{
		this.m_time = time;
	}

	private uint GetCameraIndex(uint shotIndex)
	{
		return shotIndex % 2U;
	}

	public string GetDebugDescription(string linePrefix)
	{
		string text = string.Empty;
		string text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			linePrefix,
			"[Shot Name] ",
			this.m_name,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			linePrefix,
			"[Duration] ",
			this.m_duration,
			"\n"
		});
		text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			linePrefix,
			"[Type] ",
			this.m_type.ToString(),
			"\n"
		});
		text2 = text;
		return string.Concat(new string[]
		{
			text2,
			linePrefix,
			"[Transition In Type] ",
			this.m_transitionInType.ToString(),
			"\n"
		});
	}

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

		public List<CameraShot.AnimParamSetAction> m_animSetActions;

		public CharacterToAnimParamSetActions(ActorData actor, List<CameraShot.AnimParamSetAction> animSetActions)
		{
			this.m_actor = actor;
			this.m_animSetActions = animSetActions;
		}
	}
}
