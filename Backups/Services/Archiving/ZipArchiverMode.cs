namespace Backups.Services.Archiving
{
    public enum ZipArchiverMode
    {
        /// <summary>
        /// Archive files to single zip-archive.
        /// </summary>
        ToSingleZip,

        /// <summary>
        /// Archive files to split zip-archives.
        /// </summary>
        ToSplitZips,
    }
}