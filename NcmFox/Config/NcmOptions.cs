namespace NcmFox.Config;

/// <summary>
/// Represents configuration options for NCM file processing.
/// </summary>
/// <remarks>
/// <para>
/// This record provides immutable configuration options that control various aspects
/// of NCM file parsing and decryption, including buffer sizes and security limits.
/// </para>
/// <para>
/// Example:
/// <code>
/// var options = new NcmOptions
/// {
///     BufferSize = 64 * 1024,
///     MaxCoverSize = 10 * 1024 * 1024
/// };
/// NcmConfig.Configure(options);
/// </code>
/// </para>
/// </remarks>
public sealed record NcmOptions
{
    /// <summary>
    /// Gets or initializes the buffer size (in bytes) used for audio decryption.
    /// </summary>
    /// <value>
    /// The buffer size in bytes. Default is 1 MB (1,048,576 bytes).
    /// </value>
    /// <remarks>
    /// <para>
    /// Larger buffer sizes may improve throughput for large files but increase memory usage.
    /// Smaller buffer sizes reduce memory footprint but may slightly decrease performance.
    /// </para>
    /// <para>
    /// Recommended range: 64 KB to 4 MB.
    /// </para>
    /// </remarks>
    public int BufferSize { get; init; } = 1024 * 1024;

    /// <summary>
    /// Gets or initializes the maximum allowed cover image size (in bytes).
    /// </summary>
    /// <value>
    /// The maximum cover image size in bytes. Default is 25 MB (26,214,400 bytes).
    /// </value>
    /// <remarks>
    /// <para>
    /// This limit prevents memory exhaustion from malformed or malicious NCM files
    /// with extremely large embedded cover images.
    /// </para>
    /// <para>
    /// Set to <c>int.MaxValue</c> to disable the limit (not recommended).
    /// </para>
    /// </remarks>
    public int MaxCoverSize { get; init; } = 25 * 1024 * 1024;

    /// <summary>
    /// Gets or initializes the maximum allowed metadata size (in bytes).
    /// </summary>
    /// <value>
    /// The maximum metadata size in bytes. Default is 1 MB (1,048,576 bytes).
    /// </value>
    /// <remarks>
    /// <para>
    /// This limit prevents memory exhaustion from malformed or malicious NCM files
    /// with extremely large metadata sections.
    /// </para>
    /// <para>
    /// Set to <c>int.MaxValue</c> to disable the limit (not recommended).
    /// </para>
    /// </remarks>
    public int MaxMetadataSize { get; init; } = 1024 * 1024;

    /// <summary>
    /// Gets or initializes a value indicating whether metadata size validation is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> to validate metadata size against <see cref="MaxMetadataSize"/>; otherwise, <c>false</c>.
    /// Default is <c>true</c>.
    /// </value>
    /// <remarks>
    /// <para>
    /// When enabled, an <see cref="InvalidDataException"/> is thrown if the metadata
    /// section exceeds <see cref="MaxMetadataSize"/>.
    /// </para>
    /// <para>
    /// Disabling this check may pose security risks when processing untrusted files.
    /// </para>
    /// </remarks>
    public bool EnableMetaDataSizeCheck { get; init; } = true;

    /// <summary>
    /// Gets or initializes a value indicating whether cover image size validation is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> to validate cover image size against <see cref="MaxCoverSize"/>; otherwise, <c>false</c>.
    /// Default is <c>true</c>.
    /// </value>
    /// <remarks>
    /// <para>
    /// When enabled, an <see cref="InvalidDataException"/> is thrown if the cover image
    /// exceeds <see cref="MaxCoverSize"/>.
    /// </para>
    /// <para>
    /// Disabling this check may pose security risks when processing untrusted files.
    /// </para>
    /// </remarks>
    public bool EnableCoverSizeCheck { get; init; } = true;

    /// <summary>
    /// Gets the default configuration options.
    /// </summary>
    /// <value>
    /// A <see cref="NcmOptions"/> instance with default values.
    /// </value>
    public static NcmOptions Default { get; } = new();
}