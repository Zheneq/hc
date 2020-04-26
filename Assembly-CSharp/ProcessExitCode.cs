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
	Terminated = 1,
	Restarting = 2,
	InProgress = 100,
	SIGHUP = -127,
	SIGINT = -126,
	SIGQUIT = -125,
	SIGABRT = -122,
	SIGKILL = -119,
	SIGSEGV = -117,
	SIGTERM = -113
}
