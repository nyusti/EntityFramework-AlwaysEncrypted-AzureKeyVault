namespace Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using Microsoft.Azure.KeyVault;
    using Microsoft.SqlServer.Management.AlwaysEncrypted.AzureKeyVaultProvider;
    using Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault.Convetions;

    /// <summary>
    /// Model builder extensions
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Sets the encryption convetion.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <returns>The configured model builder.</returns>
        public static DbModelBuilder SetEncryptionConvention(this DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Conventions.Add<EncryptedColumnAnnotationConvention>();

            return modelBuilder;
        }

        /// <summary>
        /// Enables the encryption on the DbContext.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="authenticationCallback">The authentication callback.</param>
        /// <remarks>
        /// If authentication callback is not set, remove the Column Encryption Setting=Enabled from
        /// the connection string.
        /// </remarks>
        /// <returns>The configured model builder.</returns>
        public static DbModelBuilder EnableEncryption(this DbModelBuilder modelBuilder, KeyVaultClient.AuthenticationCallback authenticationCallback)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            var azureKeyVaultProvider = new SqlColumnEncryptionAzureKeyVaultProvider(authenticationCallback);
            var providers = new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>
                {
                    { SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, azureKeyVaultProvider }
                };

            SqlConnection.RegisterColumnEncryptionKeyStoreProviders(providers);

            return modelBuilder;
        }
    }
}