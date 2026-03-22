namespace NcmFox.Config;

/// <summary>
/// Provides global configuration management for NCM file processing.
/// </summary>
/// <remarks>
/// <para>
/// This static class manages the global configuration state used by all NCM operations.
/// Configuration is thread-safe and can be changed at runtime.
/// </para>
/// <para>
/// Example:
/// <code>
/// // Set custom configuration
/// NcmConfig.Configure(new NcmOptions
/// {
///     BufferSize = 64 * 1024,
///     EnableCoverSizeCheck = false
/// });
/// 
/// // Read current configuration
/// var current = NcmConfig.Current;
/// </code>
/// </para>
/// </remarks>
public static class NcmConfig
{
    private static NcmOptions _current = NcmOptions.Default;

    /// <summary>
    /// Gets the current configuration options.
    /// </summary>
    /// <value>
    /// The current <see cref="NcmOptions"/> instance being used by all NCM operations.
    /// </value>
    /// <remarks>
    /// This property is thread-safe and returns a snapshot of the current configuration.
    /// </remarks>
    public static NcmOptions Current => Volatile.Read(ref _current);

    /// <summary>
    /// Configures the global NCM processing options.
    /// </summary>
    /// <param name="options">
    /// The <see cref="NcmOptions"/> to use. If <c>null</c>, defaults to <see cref="NcmOptions.Default"/>.
    /// </param>
    /// <remarks>
    /// <para>
    /// This method is thread-safe and can be called at any time.
    /// Changes take effect immediately for all subsequent operations.
    /// </para>
    /// <para>
    /// Note: Changing configuration while processing files may lead to inconsistent behavior.
    /// It is recommended to configure once at application startup.
    /// </para>
    /// </remarks>
    public static void Configure(NcmOptions options)
    {
        Volatile.Write(ref _current, options ?? NcmOptions.Default);
    }
}