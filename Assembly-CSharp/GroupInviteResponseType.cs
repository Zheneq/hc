using System;

public enum GroupInviteResponseType
{
	UNKNOWN,
	PlayerAccepted,
	PlayerStillAwaitingPreviousQuery,
	PlayerInCustomMatch,
	PlayerRejected,
	OfferExpired,
	RequestorSpamming
}
