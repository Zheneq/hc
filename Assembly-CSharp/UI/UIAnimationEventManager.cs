using System.Collections.Generic;
using UnityEngine;

public class UIAnimationEventManager : MonoBehaviour
{
	public delegate void AnimationDoneCallback();

	public delegate void AnimationDoneCallbackWithGameObjectParam(GameObject gameObject);

	public class AnimationEventTracker
	{
		public Animator AnimationController;

		public string AnimationToPlay;

		public int AnimationLayerToPlayOn;

		public float NormalizedTimeToStartPlay;

		public string AnimNameToTrackForDoneCallback;

		public AnimationDoneCallback callback;

		public AnimationDoneCallbackWithGameObjectParam callbackWithGameObjectParam;

		public GameObject gameObjectParam;

		public bool AnimatorWasInactive;

		public float AnimationCheckTime;
	}

	private List<AnimationEventTracker> ActiveAnimations = new List<AnimationEventTracker>();

	private static UIAnimationEventManager s_instance;

	public static UIAnimationEventManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void Update()
	{
		for (int i = 0; i < ActiveAnimations.Count; i++)
		{
			AnimationEventTracker animationEventTracker = ActiveAnimations[i];
			Animator animationController = animationEventTracker.AnimationController;
			if (animationController != null)
			{
				if (animationEventTracker.callback == null)
				{
					if (animationEventTracker.callbackWithGameObjectParam == null)
					{
						if (!animationEventTracker.AnimatorWasInactive)
						{
							goto IL_013e;
						}
					}
				}
				if (!animationController.gameObject.activeInHierarchy)
				{
					continue;
				}
				if (!(animationEventTracker.AnimationCheckTime <= Time.time))
				{
					continue;
				}
				if (animationEventTracker.AnimatorWasInactive)
				{
					animationController.Play(animationEventTracker.AnimationToPlay, animationEventTracker.AnimationLayerToPlayOn, animationEventTracker.NormalizedTimeToStartPlay);
					animationEventTracker.AnimatorWasInactive = false;
				}
				if (!IsAnimationDone(animationController, animationEventTracker.AnimNameToTrackForDoneCallback, animationEventTracker.AnimationLayerToPlayOn))
				{
					continue;
				}
				if (animationEventTracker.callback != null)
				{
					animationEventTracker.callback();
				}
				if (animationEventTracker.callbackWithGameObjectParam != null)
				{
					animationEventTracker.callbackWithGameObjectParam(animationEventTracker.gameObjectParam);
				}
				ActiveAnimations.RemoveAt(i);
				i--;
				continue;
			}
			goto IL_013e;
			IL_013e:
			ActiveAnimations.RemoveAt(i);
			i--;
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void PlayAnimation(Animator animator, string AnimToPlay, AnimationDoneCallback callbackOnDone, string AnimNameForDoneCallback = "", int Layer = 0, float NormalizedTime = 0f, bool setAnimatorGameObjectActive = true, bool checkCurrentState = false, AnimationDoneCallbackWithGameObjectParam callbackOnDoneWithGameObjectParam = null, GameObject gameObjectParamForCallback = null)
	{
		if (animator == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Log.Error("Animator is null");
					return;
				}
			}
		}
		if (AnimToPlay.IsNullOrEmpty())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Log.Error("Attempted to play null animation");
					return;
				}
			}
		}
		bool flag = false;
		for (int num = ActiveAnimations.Count - 1; num >= 0; num--)
		{
			if (ActiveAnimations[num].AnimationController == animator)
			{
				if (Layer == ActiveAnimations[num].AnimationLayerToPlayOn)
				{
					ActiveAnimations.RemoveAt(num);
					flag = true;
				}
			}
		}
		while (true)
		{
			if (checkCurrentState)
			{
				if (!flag)
				{
					if (animator.isActiveAndEnabled)
					{
						if (animator.isInitialized)
						{
							if (animator.GetCurrentAnimatorStateInfo(Layer).IsName(AnimToPlay))
							{
								while (true)
								{
									switch (7)
									{
									default:
										return;
									case 0:
										break;
									}
								}
							}
						}
					}
				}
			}
			if (setAnimatorGameObjectActive)
			{
				UIManager.SetGameObjectActive(animator, true);
			}
			bool flag2 = true;
			if (animator.gameObject.activeInHierarchy)
			{
				if (animator.isInitialized)
				{
					goto IL_0178;
				}
			}
			flag2 = false;
			goto IL_0178;
			IL_0178:
			if (AnimNameForDoneCallback.IsNullOrEmpty())
			{
				AnimNameForDoneCallback = AnimToPlay;
			}
			float num2 = 0f;
			for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
			{
				if (animator.runtimeAnimatorController.animationClips[i].name == AnimToPlay)
				{
					num2 = animator.runtimeAnimatorController.animationClips[i].length;
				}
			}
			AnimationEventTracker animationEventTracker = new AnimationEventTracker();
			animationEventTracker.AnimationController = animator;
			animationEventTracker.AnimationToPlay = AnimToPlay;
			animationEventTracker.AnimNameToTrackForDoneCallback = AnimNameForDoneCallback;
			animationEventTracker.callback = callbackOnDone;
			animationEventTracker.AnimatorWasInactive = !flag2;
			animationEventTracker.AnimationLayerToPlayOn = Layer;
			animationEventTracker.NormalizedTimeToStartPlay = NormalizedTime;
			animationEventTracker.AnimationCheckTime = Time.time + num2;
			animationEventTracker.callbackWithGameObjectParam = callbackOnDoneWithGameObjectParam;
			animationEventTracker.gameObjectParam = gameObjectParamForCallback;
			AnimationEventTracker item = animationEventTracker;
			if (flag2)
			{
				animator.Play(AnimToPlay, Layer, NormalizedTime);
			}
			ActiveAnimations.Add(item);
			return;
		}
	}

	public static bool IsAnimationDone(Animator animator, string animName, int layer)
	{
		if (!animator.isInitialized)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(layer);
		if (currentAnimatorClipInfo.Length == 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		AnimatorClipInfo animatorClipInfo = currentAnimatorClipInfo[0];
		AnimationClip clip = animatorClipInfo.clip;
		if (clip == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (clip.name != animName)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return true;
	}
}
