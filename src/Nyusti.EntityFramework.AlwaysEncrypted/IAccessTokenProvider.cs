namespace Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault
{
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;

    /// <summary>
    /// Access token provider interface
    /// </summary>
    public interface IAccessTokenProvider
    {
        /// <summary>
        /// Gets the authentication callback.
        /// </summary>
        /// <returns>The authenticaiton callback</returns>
        KeyVaultClient.AuthenticationCallback GetAuthenticationCallback();

        /// <summary>
        /// Gets the authentication token.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>The access token string</returns>
        Task<string> GetToken(string authority, string resource, string scope);
    }
}