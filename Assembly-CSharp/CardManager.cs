using System;
using UnityEngine;
using UnityEngine.Networking;

public class CardManager : NetworkBehaviour
{
	public GameObject m_dataPrefab;

	private CardManagerData m_dataPrefabInstanceComponent;

	private static CardManager s_instance;

	public GameObject[] m_cardIndex
	{
		get
		{
			GameObject[] result;
			if (this.m_dataPrefabInstanceComponent == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CardManager.get_m_cardIndex()).MethodHandle;
				}
				result = null;
			}
			else
			{
				result = this.m_dataPrefabInstanceComponent.m_cardIndex;
			}
			return result;
		}
	}

	internal static CardManager Get()
	{
		return CardManager.s_instance;
	}

	public bool ShowingInGameCardUI { get; set; }

	private void Awake()
	{
		this.m_dataPrefabInstanceComponent = CardManagerData.Get();
		CardManager.s_instance = this;
		this.ShowingInGameCardUI = true;
		Log.Info("Set CardManager reference", new object[0]);
	}

	private void OnDestroy()
	{
		CardManager.s_instance = null;
	}

	public void SetDeckAndGiveCards(ActorData actor, CharacterCardInfo cardInfo, bool isDebugRequest = false)
	{
	}

	internal void RemoveUsedCards()
	{
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result;
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
