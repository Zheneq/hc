using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UIDialogPopupManager : UIScene
{
	public RectTransform m_allDialogs;

	public UITwoButtonDialog m_twoBtnDialogPrefab;

	public UIProgressCancelDialog m_progressCancelDialogPrefab;

	public UIOneButtonDialog m_oneButtonDialogPrefab;

	public UIReportBugDialogBox m_reportDialogPrefab;

	public UIRatingDialogBox m_ratingDialogPrefab;

	public UIStorePurchaseForCashDialogBox m_storePurchaseForCashPrefab;

	public UIStorePurchaseGameDialogBox m_storePurchaseGamePrefab;

	public UIStorePurchaseItemDialogBox m_storePurchaseItemPrefab;

	public UIDebugModSelectionDialog m_debugMODSelectionPrefab;

	public UIPartyInvitePopDialogBox m_partyInvitePopupPrefab;

	public UITrustWarEndDialog m_trustWarEndDialogPrefab;

	public UISingleInputLineInputDialogBox m_singleLineInputPrefab;

	private static UIDialogPopupManager s_instance;

	private List<UIDialogBox> m_openBoxes;

	[HideInInspector]
	private bool m_wasBackgroundVisible;

	internal static bool Ready
	{
		get;
		private set;
	}

	private static Action OnReadyHolder;
	public static event Action OnReady
	{
		add
		{
			Action action = UIDialogPopupManager.OnReadyHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref UIDialogPopupManager.OnReadyHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action action = UIDialogPopupManager.OnReadyHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref UIDialogPopupManager.OnReadyHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	static UIDialogPopupManager()
	{
		UIDialogPopupManager.OnReadyHolder = delegate
		{
		};
	}

	public static UIDialogPopupManager Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.DialogPopups;
	}

	public override void Awake()
	{
		s_instance = this;
		if (base.gameObject.transform.parent == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		base.Awake();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public bool IsDialogBoxOpen()
	{
		return m_openBoxes.Count != 0;
	}

	public void Start()
	{
		UIManager.SetGameObjectActive(m_allDialogs, false);
		m_openBoxes = new List<UIDialogBox>();
		Ready = true;
		if (UIDialogPopupManager.OnReadyHolder == null)
		{
			return;
		}
		while (true)
		{
			UIDialogPopupManager.OnReadyHolder();
			return;
		}
	}

	public void CloseDialog(UIDialogBox boxType)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.DialogBoxButton);
		if (m_openBoxes.Contains(boxType))
		{
			m_openBoxes.Remove(boxType);
			boxType.ClearCallback();
			UnityEngine.Object.Destroy(boxType.gameObject);
			boxType = null;
		}
		if (m_openBoxes.Count == 0)
		{
			UIManager.SetGameObjectActive(m_allDialogs, false);
		}
	}

	private UIDialogBox CreateNewDialogBox(DialogBoxType boxType)
	{
		UIDialogBox uIDialogBox = null;
		switch (boxType)
		{
		case DialogBoxType.TwoButton:
			uIDialogBox = UnityEngine.Object.Instantiate(m_twoBtnDialogPrefab);
			break;
		case DialogBoxType.ProgressCancel:
			uIDialogBox = UnityEngine.Object.Instantiate(m_progressCancelDialogPrefab);
			break;
		case DialogBoxType.OneButton:
			uIDialogBox = UnityEngine.Object.Instantiate(m_oneButtonDialogPrefab);
			break;
		case DialogBoxType.ReportBug:
			uIDialogBox = UnityEngine.Object.Instantiate(m_reportDialogPrefab);
			break;
		case DialogBoxType.Rating:
			uIDialogBox = UnityEngine.Object.Instantiate(m_ratingDialogPrefab);
			break;
		case DialogBoxType.PurchaseGame:
			uIDialogBox = UnityEngine.Object.Instantiate(m_storePurchaseGamePrefab);
			break;
		case DialogBoxType.PurchaseForCash:
			uIDialogBox = UnityEngine.Object.Instantiate(m_storePurchaseForCashPrefab);
			break;
		case DialogBoxType.PurchaseItem:
			uIDialogBox = UnityEngine.Object.Instantiate(m_storePurchaseItemPrefab);
			break;
		case DialogBoxType._001D:
			uIDialogBox = UnityEngine.Object.Instantiate(m_debugMODSelectionPrefab);
			break;
		case DialogBoxType.PartyInvite:
			uIDialogBox = UnityEngine.Object.Instantiate(m_partyInvitePopupPrefab);
			break;
		case DialogBoxType.TrustWarEnd:
			uIDialogBox = UnityEngine.Object.Instantiate(m_trustWarEndDialogPrefab);
			break;
		case DialogBoxType.SingleLineInput:
			uIDialogBox = UnityEngine.Object.Instantiate(m_singleLineInputPrefab);
			break;
		}
		if (uIDialogBox != null)
		{
			RectTransform rectTransform = uIDialogBox.transform as RectTransform;
			rectTransform.SetParent(m_allDialogs.transform);
			rectTransform.SetAsLastSibling();
			rectTransform.localScale = Vector3.one;
			rectTransform.localEulerAngles = Vector3.zero;
			rectTransform.localPosition = new Vector3(0f, 0f, -6000f);
			m_openBoxes.Add(uIDialogBox);
		}
		UIManager.SetGameObjectActive(m_allDialogs, true);
		return uIDialogBox;
	}

	public static UIDebugModSelectionDialog OpenDebugModSelectionDialog(AbilityData.AbilityEntry selectedAbility, int inAbilityIndex, UICharacterAbilitiesPanel abilitiesPanel, UIDialogBox.DialogButtonCallback OnAccept = null)
	{
		if (Get() == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		UIDebugModSelectionDialog uIDebugModSelectionDialog = Get().CreateNewDialogBox(DialogBoxType._001D) as UIDebugModSelectionDialog;
		uIDebugModSelectionDialog.Setup(selectedAbility, inAbilityIndex, abilitiesPanel, OnAccept);
		return uIDebugModSelectionDialog;
	}

	public static UIPartyInvitePopDialogBox OpenPartyInviteDialog(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, UIDialogBox.DialogButtonCallback blockCallback, UIDialogBox.DialogButtonCallback leftButtonCallback = null, UIDialogBox.DialogButtonCallback rightButtonCallback = null)
	{
		if (Get() == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		UIPartyInvitePopDialogBox uIPartyInvitePopDialogBox = Get().CreateNewDialogBox(DialogBoxType.PartyInvite) as UIPartyInvitePopDialogBox;
		uIPartyInvitePopDialogBox.Setup(Title, Description, LeftButtonLabel, RightButtonLabel, blockCallback, leftButtonCallback, rightButtonCallback);
		return uIPartyInvitePopDialogBox;
	}

	public static UITwoButtonDialog OpenTwoButtonDialog(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, UIDialogBox.DialogButtonCallback leftButtonCallback = null, UIDialogBox.DialogButtonCallback rightButtonCallback = null, bool CallLeftOnClose = false, bool CallRightOnClose = false)
	{
		if (Get() == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		UITwoButtonDialog uITwoButtonDialog = Get().CreateNewDialogBox(DialogBoxType.TwoButton) as UITwoButtonDialog;
		uITwoButtonDialog.Setup(Title, Description, LeftButtonLabel, RightButtonLabel, leftButtonCallback, rightButtonCallback);
		return uITwoButtonDialog;
	}

	public static UIProgressCancelDialog OpenProgressCancelButton(string Title, string Description, string CancelButtonLabelText, float initialVal, UIDialogBox.DialogButtonCallback callback = null)
	{
		if (Get() == null)
		{
			return null;
		}
		UIProgressCancelDialog uIProgressCancelDialog = Get().CreateNewDialogBox(DialogBoxType.ProgressCancel) as UIProgressCancelDialog;
		uIProgressCancelDialog.Setup(Title, Description, CancelButtonLabelText, initialVal, callback);
		return uIProgressCancelDialog;
	}

	public static UIStorePurchaseItemDialogBox OpenPurchaseItemDialog(UIPurchaseableItem item, UIDialogBox.DialogButtonCallback callback = null, UIStorePurchaseItemDialogBox.PurchaseCloseDialogCallback closeCallback = null)
	{
		if (Get() == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		UIStorePurchaseItemDialogBox uIStorePurchaseItemDialogBox = Get().CreateNewDialogBox(DialogBoxType.PurchaseItem) as UIStorePurchaseItemDialogBox;
		uIStorePurchaseItemDialogBox.Setup(item, closeCallback);
		return uIStorePurchaseItemDialogBox;
	}

	public static UIStorePurchaseForCashDialogBox OpenPurchaseForCashDialog(UIPurchaseableItem item, PaymentMethodsResponse response, UIDialogBox.DialogButtonCallback callback = null)
	{
		if (Get() == null)
		{
			return null;
		}
		UIStorePurchaseForCashDialogBox uIStorePurchaseForCashDialogBox = Get().CreateNewDialogBox(DialogBoxType.PurchaseForCash) as UIStorePurchaseForCashDialogBox;
		uIStorePurchaseForCashDialogBox.Setup(item, response);
		return uIStorePurchaseForCashDialogBox;
	}

	public static UIStorePurchaseGameDialogBox OpenPurchaseGameDialog(UIPurchaseableItem item, PaymentMethodsResponse response, UIDialogBox.DialogButtonCallback callback = null)
	{
		if (Get() == null)
		{
			return null;
		}
		UIStorePurchaseGameDialogBox uIStorePurchaseGameDialogBox = Get().CreateNewDialogBox(DialogBoxType.PurchaseGame) as UIStorePurchaseGameDialogBox;
		uIStorePurchaseGameDialogBox.Setup(item, response);
		return uIStorePurchaseGameDialogBox;
	}

	public static UIOneButtonDialog OpenOneButtonDialog(string Title, string Description, string ButtonLabelText, UIDialogBox.DialogButtonCallback callback = null, int fontSize = -1, bool allowDuplicate = false)
	{
		if (Get() == null)
		{
			return null;
		}
		if (!allowDuplicate)
		{
			using (List<UIDialogBox>.Enumerator enumerator = Get().m_openBoxes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIDialogBox current = enumerator.Current;
					UIOneButtonDialog uIOneButtonDialog = current as UIOneButtonDialog;
					if (uIOneButtonDialog != null)
					{
						if (uIOneButtonDialog.m_Title.text == Title && uIOneButtonDialog.m_Desc.text == Description && uIOneButtonDialog.m_ButtonLabel[0].text == ButtonLabelText && uIOneButtonDialog.GetCallbackReference() == callback)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									return null;
								}
							}
						}
					}
				}
			}
		}
		UIOneButtonDialog uIOneButtonDialog2 = Get().CreateNewDialogBox(DialogBoxType.OneButton) as UIOneButtonDialog;
		uIOneButtonDialog2.Setup(Title, Description, ButtonLabelText, callback);
		if (fontSize > 0)
		{
			uIOneButtonDialog2.m_Desc.fontSize = fontSize;
		}
		return uIOneButtonDialog2;
	}

	public static UIReportBugDialogBox OpenReportBugDialog(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, UIDialogBox.DialogButtonCallback callback = null, UIDialogBox.DialogButtonCallback callback2 = null)
	{
		if (Get() == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		UIReportBugDialogBox uIReportBugDialogBox = Get().CreateNewDialogBox(DialogBoxType.ReportBug) as UIReportBugDialogBox;
		uIReportBugDialogBox.Setup(Title, Description, LeftButtonLabel, RightButtonLabel, callback, callback2);
		return uIReportBugDialogBox;
	}

	public static UIRatingDialogBox OpenRatingDialog(string Title, string Description, UIDialogBox.DialogButtonCallback callback = null, UIDialogBox.DialogButtonCallback cancelCallback = null)
	{
		if (Get() == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		UIRatingDialogBox uIRatingDialogBox = Get().CreateNewDialogBox(DialogBoxType.Rating) as UIRatingDialogBox;
		uIRatingDialogBox.Setup(Title, Description, callback, cancelCallback);
		return uIRatingDialogBox;
	}

	public static UITrustWarEndDialog OpenTrustWarEndDialog()
	{
		if (Get() == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		using (List<UIDialogBox>.Enumerator enumerator = Get().m_openBoxes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIDialogBox current = enumerator.Current;
				UITrustWarEndDialog x = current as UITrustWarEndDialog;
				if (x != null)
				{
					return null;
				}
			}
		}
		UITrustWarEndDialog uITrustWarEndDialog = Get().CreateNewDialogBox(DialogBoxType.TrustWarEnd) as UITrustWarEndDialog;
		uITrustWarEndDialog.Setup();
		RectTransform rectTransform = uITrustWarEndDialog.transform as RectTransform;
		rectTransform.offsetMax = Vector2.zero;
		rectTransform.offsetMin = Vector2.zero;
		return uITrustWarEndDialog;
	}

	public static UISingleInputLineInputDialogBox OpenSingleLineInputDialog(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, UIDialogBox.DialogButtonCallback callback = null, UIDialogBox.DialogButtonCallback callback2 = null)
	{
		if (Get() == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		UISingleInputLineInputDialogBox uISingleInputLineInputDialogBox = Get().CreateNewDialogBox(DialogBoxType.SingleLineInput) as UISingleInputLineInputDialogBox;
		uISingleInputLineInputDialogBox.Setup(Title, Description, LeftButtonLabel, RightButtonLabel, callback, callback2);
		return uISingleInputLineInputDialogBox;
	}

	private void HideTopMenu()
	{
		if (m_openBoxes != null)
		{
			for (int i = 0; i < m_openBoxes.Count; i++)
			{
				m_openBoxes[i].DoCloseCallback();
				UnityEngine.Object.Destroy(m_openBoxes[i].gameObject);
			}
			m_openBoxes.Clear();
		}
		UIManager.SetGameObjectActive(m_allDialogs, m_wasBackgroundVisible);
	}

	public void HideAllMenus()
	{
		HideTopMenu();
	}
}
