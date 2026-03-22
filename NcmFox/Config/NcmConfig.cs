namespace NcmFox.Config;

public static class NcmConfig
{
    private static NcmOptions _current = NcmOptions.Default;

    public static NcmOptions Current => Volatile.Read(ref _current);

    public static void Configure(NcmOptions options)
    {
        Volatile.Write(ref _current, options ?? NcmOptions.Default);
    }
}