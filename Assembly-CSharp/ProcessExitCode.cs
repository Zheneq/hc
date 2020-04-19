using System;

public enum ProcessExitCode
{
	Unknown = -1,
	StartupError = -2,
	RuntimeError = -3,
	UnhandledException = -4,
	WatchdogTimeout = -5,
	Blacklisted = -6,
	MasterConflict = -7,
	Interrupted = -8,
	NoError = 0,
	Terminated,
	Restarting,
	InProgress = 0x64,
	SIGHUP = -0x7F,
	SIGINT,
	SIGQUIT,
	SIGABRT = -0x7A,
	SIGKILL = -0x77,
	SIGSEGV = -0x75,
	SIGTERM = -0x71
}
