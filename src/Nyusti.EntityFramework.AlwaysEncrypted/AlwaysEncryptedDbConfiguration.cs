namespace Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault
{
    using System.Data.Entity;
    using System.Data.Entity.SqlServer;
    using Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault.Migrations;

    /// <summary>
    /// Always encrypted DbContext configuration
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbConfiguration"/>
    public class AlwaysEncryptedDbConfiguration : DbConfiguration
    {
        private const string DefaultProvider = "System.Data.SqlClient";

        /// <summary>
        /// Initializes a new instance of the <see cref="AlwaysEncryptedDbConfiguration"/> class.
        /// </summary>
        public AlwaysEncryptedDbConfiguration()
        {
            this.SetExecutionStrategy(DefaultProvider, () => new SqlAzureExecutionStrategy());
            this.SetMigrationSqlGenerator(DefaultProvider, () => new EncryptedColumnMigrationSqlGenerator());
        }
    }
}