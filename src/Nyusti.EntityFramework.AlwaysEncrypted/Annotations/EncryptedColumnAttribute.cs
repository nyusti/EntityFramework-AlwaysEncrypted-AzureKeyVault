namespace Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault.Annotations
{
    using System;

    /// <summary>
    /// Specifies encrypting columns by using the Always Encrypted feature. <remarks>Please notice
    /// that this feature is only supported by Microsoft SQL Server 2016 onwards. Please visit
    /// https://msdn.microsoft.com/en-us/library/mt163865.aspx for more information.</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EncryptedColumnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedColumnAttribute"/> class.
        /// </summary>
        /// <param name="keyName">Column encryption key to use.</param>
        /// <param name="encryptionType">The encryption type</param>
        public EncryptedColumnAttribute(string keyName, EncryptionType encryptionType = EncryptionType.Randomized)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException($"{nameof(keyName)} is null or empty", nameof(keyName));
            }

            this.KeyName = keyName;
            this.EncryptionType = encryptionType;
        }

        /// <summary>
        /// Gets the name of the key.
        /// </summary>
        /// <value>The name of the key.</value>
        public string KeyName { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public EncryptionType EncryptionType { get; private set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.KeyName}|{this.EncryptionType}";
        }
    }
}