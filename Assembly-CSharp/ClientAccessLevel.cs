using System;

public enum ClientAccessLevel
{
	Unknown,
	None,
	Locked = 0xA,
	Queued,
	Free = 0x14,
	Full = 0x16,
	VIP = 0x19,
	Agent = 0x1E,
	Admin = 0x28
}
