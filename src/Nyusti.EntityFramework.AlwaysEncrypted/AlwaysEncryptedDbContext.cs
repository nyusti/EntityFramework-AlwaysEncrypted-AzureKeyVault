namespace Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault
{
    using System;
    using System.Data.Entity;

    /// <summary>
    /// Always encrypted context base
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext"/>
    [DbConfigurationType(typeof(AlwaysEncryptedDbConfiguration))]
    public class AlwaysEncryptedDbContext : DbContext
    {
        private readonly IAccessTokenProvider accessTokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlwaysEncryptedDbContext"/> class. Use for
        /// migration. This consturctor is useful for code first migration.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        public AlwaysEncryptedDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlwaysEncryptedDbContext"/> class. Use for runtime.
        /// </summary>
        /// <param name="nameOrConnectionString">The name or connection string.</param>
        /// <param name="accessTokenProvider">The access token provider to support dependency injection.</param>
        /// <exception cref="ArgumentNullException">accessTokenProvider is null</exception>
        public AlwaysEncryptedDbContext(string nameOrConnectionString, IAccessTokenProvider accessTokenProvider)
            : base(nameOrConnectionString)
        {
            this.accessTokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SetEncryptionConvention();

            if (this.accessTokenProvider != null)
            {
                modelBuilder.EnableEncryption(this.accessTokenProvider.GetToken);
            }
        }
    }
}