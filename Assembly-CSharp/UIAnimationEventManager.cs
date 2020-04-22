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
					while (true)
					{
						switch (2)
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
					if (animationEventTracker.callbackWithGameObjectParam == null)
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
						if (!animationEventTracker.AnimatorWasInactive)
						{
							goto IL_013e;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				if (!animationController.gameObject.activeInHierarchy)
				{
					continue;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(animationEventTracker.AnimationCheckTime <= Time.time))
				{
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (animationEventTracker.callback != null)
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
					animationEventTracker.callback();
				}
				if (animationEventTracker.callbackWithGameObjectParam != null)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (Layer == ActiveAnimations[num].AnimationLayerToPlayOn)
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
					ActiveAnimations.RemoveAt(num);
					flag = true;
				}
			}
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (checkCurrentState)
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
				if (!flag)
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
					if (animator.isActiveAndEnabled)
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
						if (animator.isInitialized)
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
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(animator, true);
			}
			bool flag2 = true;
			if (animator.gameObject.activeInHierarchy)
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
				if (animator.isInitialized)
				{
					goto IL_0178;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			flag2 = false;
			goto IL_0178;
			IL_0178:
			if (AnimNameForDoneCallback.IsNullOrEmpty())
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
				AnimNameForDoneCallback = AnimToPlay;
			}
			float num2 = 0f;
			for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
			{
				if (animator.runtimeAnimatorController.animationClips[i].name == AnimToPlay)
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
