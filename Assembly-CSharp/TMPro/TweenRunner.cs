using System.Collections;
using UnityEngine;

namespace TMPro
{
	internal class TweenRunner<T> where T : struct, ITweenValue
	{
		protected MonoBehaviour m_CoroutineContainer;

		protected IEnumerator m_Tween;

		private static IEnumerator Start(T tweenInfo)
		{
			if (!tweenInfo.ValidTarget())
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
						yield break;
					}
				}
			}
			float elapsedTime2 = 0f;
			if (elapsedTime2 < tweenInfo.duration)
			{
				float num = elapsedTime2;
				float num2;
				if (tweenInfo.ignoreTimeScale)
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
					num2 = Time.unscaledDeltaTime;
				}
				else
				{
					num2 = Time.deltaTime;
				}
				elapsedTime2 = num + num2;
				float percentage = Mathf.Clamp01(elapsedTime2 / tweenInfo.duration);
				tweenInfo.TweenValue(percentage);
				yield return null;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				tweenInfo.TweenValue(1f);
				yield break;
			}
		}

		public void Init(MonoBehaviour coroutineContainer)
		{
			m_CoroutineContainer = coroutineContainer;
		}

		public void StartTween(T info)
		{
			if (m_CoroutineContainer == null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						Debug.LogWarning("Coroutine container not configured... did you forget to call Init?");
						return;
					}
				}
			}
			StopTween();
			if (!m_CoroutineContainer.gameObject.activeInHierarchy)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						info.TweenValue(1f);
						return;
					}
				}
			}
			m_Tween = Start(info);
			m_CoroutineContainer.StartCoroutine(m_Tween);
		}

		public void StopTween()
		{
			if (m_Tween == null)
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
				m_CoroutineContainer.StopCoroutine(m_Tween);
				m_Tween = null;
				return;
			}
		}
	}
}
