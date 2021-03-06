﻿namespace Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault.Convetions
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using System.Reflection;
    using Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault.Annotations;

    /// <summary>
    /// Encrypted column annotation convention
    /// </summary>
    internal sealed class EncryptedColumnAnnotationConvention : AttributeToColumnAnnotationConvention<EncryptedColumnAttribute, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedColumnAnnotationConvention"/> class.
        /// </summary>
        public EncryptedColumnAnnotationConvention()
            : base(EncryptedColumnAttribute.AnnotationName, AnnotationFactory)
        {
        }

        /// <summary>
        /// Annotations the factory.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="encryptedAttributes">The encrypted attributes.</param>
        /// <returns>The annotation in string format</returns>
        private static string AnnotationFactory(PropertyInfo propertyInfo, IList<EncryptedColumnAttribute> encryptedAttributes)
        {
            return encryptedAttributes.Single().ToString();
        }
    }
}