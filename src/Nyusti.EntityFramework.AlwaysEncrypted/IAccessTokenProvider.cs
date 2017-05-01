namespace Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault
{
    using System.Threading.Tasks;

    /// <summary>
    /// Access token provider interface
    /// </summary>
    public interface IAccessTokenProvider
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>The access token.</returns>
        Task<string> GetToken(string authority, string resource, string scope);
    }
}