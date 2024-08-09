using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;

[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[module: UnverifiableCode]

[assembly: AssemblyVersion (
    ThisAssembly.Git.BaseVersion.Major + "." +
    ThisAssembly.Git.BaseVersion.Minor + "." +
#if SERVER
    ThisAssembly.Git.BaseVersion.Patch + "." +
    ThisAssembly.Git.Commits
#else
    ThisAssembly.Git.SemVer.Patch
#endif
    )]

#if SERVER
[assembly: AssemblyFileVersion (
    ThisAssembly.Git.BaseVersion.Major + "." +
    ThisAssembly.Git.BaseVersion.Minor + "." +
    ThisAssembly.Git.BaseVersion.Patch + "." +
    ThisAssembly.Git.Commits
)]
#else
[assembly: AssemblyFileVersion (
    ThisAssembly.Git.BaseVersion.Major + "." +
    ThisAssembly.Git.BaseVersion.Minor + "." +
    ThisAssembly.Git.SemVer.Patch
    )]
#endif

[assembly: AssemblyInformationalVersion (BuildVersion.s_version)]

[assembly: AssemblyProduct (
#if SERVER
    "Atlas Reactor Custom Server"
#else
    "Atlas Reactor Custom Client"
#endif
    )]