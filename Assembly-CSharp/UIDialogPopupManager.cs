using System;
using System.Collections.Generic;
using System.Threading;
using LobbyGameClientMessages;
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

	// Note: this type is marked as 'beforefieldinit'.
	static UIDialogPopupManager()
	{
		UIDialogPopupManager.OnReady = delegate()
		{
		};
	}

	internal static bool Ready { get; private set; }

	public static event Action OnReady
	{
		add
		{
			Action action = UIDialogPopupManager.OnReady;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref UIDialogPopupManager.OnReady, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.add_OnReady(Action)).MethodHandle;
			}
		}
		remove
		{
			Action action = UIDialogPopupManager.OnReady;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref UIDialogPopupManager.OnReady, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.remove_OnReady(Action)).MethodHandle;
			}
		}
	}

	public static UIDialogPopupManager Get()
	{
		return UIDialogPopupManager.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.DialogPopups;
	}

	public override void Awake()
	{
		UIDialogPopupManager.s_instance = this;
		if (base.gameObject.transform.parent == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.Awake()).MethodHandle;
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		base.Awake();
	}

	private void OnDestroy()
	{
		UIDialogPopupManager.s_instance = null;
	}

	public bool IsDialogBoxOpen()
	{
		return this.m_openBoxes.Count != 0;
	}

	public void Start()
	{
		UIManager.SetGameObjectActive(this.m_allDialogs, false, null);
		this.m_openBoxes = new List<UIDialogBox>();
		UIDialogPopupManager.Ready = true;
		if (UIDialogPopupManager.OnReady != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.Start()).MethodHandle;
			}
			UIDialogPopupManager.OnReady();
		}
	}

	public void CloseDialog(UIDialogBox boxType)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.DialogBoxButton);
		if (this.m_openBoxes.Contains(boxType))
		{
			this.m_openBoxes.Remove(boxType);
			boxType.ClearCallback();
			UnityEngine.Object.Destroy(boxType.gameObject);
			boxType = null;
		}
		if (this.m_openBoxes.Count == 0)
		{
			UIManager.SetGameObjectActive(this.m_allDialogs, false, null);
		}
	}

	private UIDialogBox CreateNewDialogBox(DialogBoxType boxType)
	{
		UIDialogBox uidialogBox = null;
		switch (boxType)
		{
		case DialogBoxType.TwoButton:
			uidialogBox = UnityEngine.Object.Instantiate<UITwoButtonDialog>(this.m_twoBtnDialogPrefab);
			break;
		case DialogBoxType.ProgressCancel:
			uidialogBox = UnityEngine.Object.Instantiate<UIProgressCancelDialog>(this.m_progressCancelDialogPrefab);
			break;
		case DialogBoxType.OneButton:
			uidialogBox = UnityEngine.Object.Instantiate<UIOneButtonDialog>(this.m_oneButtonDialogPrefab);
			break;
		case DialogBoxType.ReportBug:
			uidialogBox = UnityEngine.Object.Instantiate<UIReportBugDialogBox>(this.m_reportDialogPrefab);
			break;
		case DialogBoxType.Rating:
			uidialogBox = UnityEngine.Object.Instantiate<UIRatingDialogBox>(this.m_ratingDialogPrefab);
			break;
		case DialogBoxType.PurchaseItem:
			uidialogBox = UnityEngine.Object.Instantiate<UIStorePurchaseItemDialogBox>(this.m_storePurchaseItemPrefab);
			break;
		case DialogBoxType.\u001D:
			uidialogBox = UnityEngine.Object.Instantiate<UIDebugModSelectionDialog>(this.m_debugMODSelectionPrefab);
			break;
		case DialogBoxType.PartyInvite:
			uidialogBox = UnityEngine.Object.Instantiate<UIPartyInvitePopDialogBox>(this.m_partyInvitePopupPrefab);
			break;
		case DialogBoxType.PurchaseGame:
			uidialogBox = UnityEngine.Object.Instantiate<UIStorePurchaseGameDialogBox>(this.m_storePurchaseGamePrefab);
			break;
		case DialogBoxType.PurchaseForCash:
			uidialogBox = UnityEngine.Object.Instantiate<UIStorePurchaseForCashDialogBox>(this.m_storePurchaseForCashPrefab);
			break;
		case DialogBoxType.TrustWarEnd:
			uidialogBox = UnityEngine.Object.Instantiate<UITrustWarEndDialog>(this.m_trustWarEndDialogPrefab);
			break;
		case DialogBoxType.SingleLineInput:
			uidialogBox = UnityEngine.Object.Instantiate<UISingleInputLineInputDialogBox>(this.m_singleLineInputPrefab);
			break;
		}
		if (uidialogBox != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.CreateNewDialogBox(DialogBoxType)).MethodHandle;
			}
			RectTransform rectTransform = uidialogBox.transform as RectTransform;
			rectTransform.SetParent(this.m_allDialogs.transform);
			rectTransform.SetAsLastSibling();
			rectTransform.localScale = Vector3.one;
			rectTransform.localEulerAngles = Vector3.zero;
			rectTransform.localPosition = new Vector3(0f, 0f, -6000f);
			this.m_openBoxes.Add(uidialogBox);
		}
		UIManager.SetGameObjectActive(this.m_allDialogs, true, null);
		return uidialogBox;
	}

	public static UIDebugModSelectionDialog OpenDebugModSelectionDialog(AbilityData.AbilityEntry selectedAbility, int inAbilityIndex, UICharacterAbilitiesPanel abilitiesPanel, UIDialogBox.DialogButtonCallback OnAccept = null)
	{
		if (UIDialogPopupManager.Get() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.OpenDebugModSelectionDialog(AbilityData.AbilityEntry, int, UICharacterAbilitiesPanel, UIDialogBox.DialogButtonCallback)).MethodHandle;
			}
			return null;
		}
		UIDebugModSelectionDialog uidebugModSelectionDialog = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.\u001D) as UIDebugModSelectionDialog;
		uidebugModSelectionDialog.Setup(selectedAbility, inAbilityIndex, abilitiesPanel, OnAccept);
		return uidebugModSelectionDialog;
	}

	public static UIPartyInvitePopDialogBox OpenPartyInviteDialog(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, UIDialogBox.DialogButtonCallback blockCallback, UIDialogBox.DialogButtonCallback leftButtonCallback = null, UIDialogBox.DialogButtonCallback rightButtonCallback = null)
	{
		if (UIDialogPopupManager.Get() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.OpenPartyInviteDialog(string, string, string, string, UIDialogBox.DialogButtonCallback, UIDialogBox.DialogButtonCallback, UIDialogBox.DialogButtonCallback)).MethodHandle;
			}
			return null;
		}
		UIPartyInvitePopDialogBox uipartyInvitePopDialogBox = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.PartyInvite) as UIPartyInvitePopDialogBox;
		uipartyInvitePopDialogBox.Setup(Title, Description, LeftButtonLabel, RightButtonLabel, blockCallback, leftButtonCallback, rightButtonCallback);
		return uipartyInvitePopDialogBox;
	}

	public static UITwoButtonDialog OpenTwoButtonDialog(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, UIDialogBox.DialogButtonCallback leftButtonCallback = null, UIDialogBox.DialogButtonCallback rightButtonCallback = null, bool CallLeftOnClose = false, bool CallRightOnClose = false)
	{
		if (UIDialogPopupManager.Get() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.OpenTwoButtonDialog(string, string, string, string, UIDialogBox.DialogButtonCallback, UIDialogBox.DialogButtonCallback, bool, bool)).MethodHandle;
			}
			return null;
		}
		UITwoButtonDialog uitwoButtonDialog = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.TwoButton) as UITwoButtonDialog;
		uitwoButtonDialog.Setup(Title, Description, LeftButtonLabel, RightButtonLabel, leftButtonCallback, rightButtonCallback, false, false);
		return uitwoButtonDialog;
	}

	public static UIProgressCancelDialog OpenProgressCancelButton(string Title, string Description, string CancelButtonLabelText, float initialVal, UIDialogBox.DialogButtonCallback callback = null)
	{
		if (UIDialogPopupManager.Get() == null)
		{
			return null;
		}
		UIProgressCancelDialog uiprogressCancelDialog = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.ProgressCancel) as UIProgressCancelDialog;
		uiprogressCancelDialog.Setup(Title, Description, CancelButtonLabelText, initialVal, callback);
		return uiprogressCancelDialog;
	}

	public static UIStorePurchaseItemDialogBox OpenPurchaseItemDialog(UIPurchaseableItem item, UIDialogBox.DialogButtonCallback callback = null, UIStorePurchaseItemDialogBox.PurchaseCloseDialogCallback closeCallback = null)
	{
		if (UIDialogPopupManager.Get() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.OpenPurchaseItemDialog(UIPurchaseableItem, UIDialogBox.DialogButtonCallback, UIStorePurchaseItemDialogBox.PurchaseCloseDialogCallback)).MethodHandle;
			}
			return null;
		}
		UIStorePurchaseItemDialogBox uistorePurchaseItemDialogBox = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.PurchaseItem) as UIStorePurchaseItemDialogBox;
		uistorePurchaseItemDialogBox.Setup(item, closeCallback);
		return uistorePurchaseItemDialogBox;
	}

	public static UIStorePurchaseForCashDialogBox OpenPurchaseForCashDialog(UIPurchaseableItem item, PaymentMethodsResponse response, UIDialogBox.DialogButtonCallback callback = null)
	{
		if (UIDialogPopupManager.Get() == null)
		{
			return null;
		}
		UIStorePurchaseForCashDialogBox uistorePurchaseForCashDialogBox = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.PurchaseForCash) as UIStorePurchaseForCashDialogBox;
		uistorePurchaseForCashDialogBox.Setup(item, response);
		return uistorePurchaseForCashDialogBox;
	}

	public static UIStorePurchaseGameDialogBox OpenPurchaseGameDialog(UIPurchaseableItem item, PaymentMethodsResponse response, UIDialogBox.DialogButtonCallback callback = null)
	{
		if (UIDialogPopupManager.Get() == null)
		{
			return null;
		}
		UIStorePurchaseGameDialogBox uistorePurchaseGameDialogBox = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.PurchaseGame) as UIStorePurchaseGameDialogBox;
		uistorePurchaseGameDialogBox.Setup(item, response);
		return uistorePurchaseGameDialogBox;
	}

	public static UIOneButtonDialog OpenOneButtonDialog(string Title, string Description, string ButtonLabelText, UIDialogBox.DialogButtonCallback callback = null, int fontSize = -1, bool allowDuplicate = false)
	{
		if (UIDialogPopupManager.Get() == null)
		{
			return null;
		}
		if (!allowDuplicate)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.OpenOneButtonDialog(string, string, string, UIDialogBox.DialogButtonCallback, int, bool)).MethodHandle;
			}
			using (List<UIDialogBox>.Enumerator enumerator = UIDialogPopupManager.Get().m_openBoxes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIDialogBox uidialogBox = enumerator.Current;
					UIOneButtonDialog uioneButtonDialog = uidialogBox as UIOneButtonDialog;
					if (uioneButtonDialog != null)
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
						if (uioneButtonDialog.m_Title.text == Title && uioneButtonDialog.m_Desc.text == Description && uioneButtonDialog.m_ButtonLabel[0].text == ButtonLabelText && uioneButtonDialog.GetCallbackReference() == callback)
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
							return null;
						}
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		UIOneButtonDialog uioneButtonDialog2 = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.OneButton) as UIOneButtonDialog;
		uioneButtonDialog2.Setup(Title, Description, ButtonLabelText, callback);
		if (fontSize > 0)
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
			uioneButtonDialog2.m_Desc.fontSize = (float)fontSize;
		}
		return uioneButtonDialog2;
	}

	public static UIReportBugDialogBox OpenReportBugDialog(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, UIDialogBox.DialogButtonCallback callback = null, UIDialogBox.DialogButtonCallback callback2 = null)
	{
		if (UIDialogPopupManager.Get() == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.OpenReportBugDialog(string, string, string, string, UIDialogBox.DialogButtonCallback, UIDialogBox.DialogButtonCallback)).MethodHandle;
			}
			return null;
		}
		UIReportBugDialogBox uireportBugDialogBox = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.ReportBug) as UIReportBugDialogBox;
		uireportBugDialogBox.Setup(Title, Description, LeftButtonLabel, RightButtonLabel, callback, callback2);
		return uireportBugDialogBox;
	}

	public static UIRatingDialogBox OpenRatingDialog(string Title, string Description, UIDialogBox.DialogButtonCallback callback = null, UIDialogBox.DialogButtonCallback cancelCallback = null)
	{
		if (UIDialogPopupManager.Get() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.OpenRatingDialog(string, string, UIDialogBox.DialogButtonCallback, UIDialogBox.DialogButtonCallback)).MethodHandle;
			}
			return null;
		}
		UIRatingDialogBox uiratingDialogBox = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.Rating) as UIRatingDialogBox;
		uiratingDialogBox.Setup(Title, Description, callback, cancelCallback);
		return uiratingDialogBox;
	}

	public static UITrustWarEndDialog OpenTrustWarEndDialog()
	{
		if (UIDialogPopupManager.Get() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.OpenTrustWarEndDialog()).MethodHandle;
			}
			return null;
		}
		using (List<UIDialogBox>.Enumerator enumerator = UIDialogPopupManager.Get().m_openBoxes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIDialogBox uidialogBox = enumerator.Current;
				UITrustWarEndDialog x = uidialogBox as UITrustWarEndDialog;
				if (x != null)
				{
					return null;
				}
			}
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
		UITrustWarEndDialog uitrustWarEndDialog = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.TrustWarEnd) as UITrustWarEndDialog;
		uitrustWarEndDialog.Setup();
		RectTransform rectTransform = uitrustWarEndDialog.transform as RectTransform;
		rectTransform.offsetMax = Vector2.zero;
		rectTransform.offsetMin = Vector2.zero;
		return uitrustWarEndDialog;
	}

	public static UISingleInputLineInputDialogBox OpenSingleLineInputDialog(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, UIDialogBox.DialogButtonCallback callback = null, UIDialogBox.DialogButtonCallback callback2 = null)
	{
		if (UIDialogPopupManager.Get() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.OpenSingleLineInputDialog(string, string, string, string, UIDialogBox.DialogButtonCallback, UIDialogBox.DialogButtonCallback)).MethodHandle;
			}
			return null;
		}
		UISingleInputLineInputDialogBox uisingleInputLineInputDialogBox = UIDialogPopupManager.Get().CreateNewDialogBox(DialogBoxType.SingleLineInput) as UISingleInputLineInputDialogBox;
		uisingleInputLineInputDialogBox.Setup(Title, Description, LeftButtonLabel, RightButtonLabel, callback, callback2);
		return uisingleInputLineInputDialogBox;
	}

	private void HideTopMenu()
	{
		if (this.m_openBoxes != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDialogPopupManager.HideTopMenu()).MethodHandle;
			}
			for (int i = 0; i < this.m_openBoxes.Count; i++)
			{
				this.m_openBoxes[i].DoCloseCallback();
				UnityEngine.Object.Destroy(this.m_openBoxes[i].gameObject);
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_openBoxes.Clear();
		}
		UIManager.SetGameObjectActive(this.m_allDialogs, this.m_wasBackgroundVisible, null);
	}

	public void HideAllMenus()
	{
		this.HideTopMenu();
	}
}
