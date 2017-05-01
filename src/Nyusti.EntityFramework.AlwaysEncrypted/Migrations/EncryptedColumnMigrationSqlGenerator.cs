namespace Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Utilities;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using Nyusti.EntityFramework.AlwaysEncrypted.AzureKeyVault.Annotations;

    /// <summary>
    /// Extended SQL generator
    /// </summary>
    /// <seealso cref="System.Data.Entity.SqlServer.SqlServerMigrationSqlGenerator"/>
    internal sealed class EncryptedColumnMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        private static readonly char[] AttributeSeparator = new[] { EncryptedColumnAttribute.AttributeSeparator };

        /// <inheritdoc/>
        protected override void Generate(AlterColumnOperation alterColumnOperation)
        {
            if (alterColumnOperation == null)
            {
                throw new ArgumentNullException(nameof(alterColumnOperation));
            }

            base.Generate(alterColumnOperation);
            var annotations = alterColumnOperation.Column.Annotations;

            // Retrieve names.
            var tableName = alterColumnOperation.Table;
            var columnName = alterColumnOperation.Column.Name;

            // Column encryption process.
            UnsupportedColumnEncryped(annotations, tableName, columnName);
        }

        /// <inheritdoc/>
        protected override void Generate(ColumnModel column, IndentedTextWriter writer)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            string baseCommand;
            using (var baseWriter = Writer())
            {
                base.Generate(column, baseWriter);
                baseCommand = baseWriter.InnerWriter.ToString();
            }

            // Generate column encrypted.
            baseCommand = GenerateColumnEncrypted(column, baseCommand);

            // Write result.
            writer.Write(baseCommand);
        }

        private static string GenerateColumnEncrypted(ColumnModel column, string command)
        {
            // Obtain annotation info
            column.Annotations.TryGetValue(EncryptedColumnAttribute.AnnotationName, out AnnotationValues values);
            if (values == null)
            {
                return command;
            }

            // Do SQL generation for column using annotation value as appropriate.
            var value = values.NewValue as string;

            // Exit if no value is defined.
            if (value == null)
            {
                return command;
            }

            var attributeValues = value.Split(AttributeSeparator, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToArray();

            // Bind the constraint.
            var keyName = attributeValues[0];
            var encryptionType = attributeValues[1];

            // Remove any other default collation if this is a string. String fields need to have
            // BIN2 collation.
            var collate = string.Empty;
            if (column.ClrType == typeof(string) && string.Equals(encryptionType, EncryptionType.Deterministic.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                collate = " COLLATE Latin1_General_BIN2";
                var indexOfCollation = command.IndexOf(" COLLATE ", StringComparison.OrdinalIgnoreCase);
                if (indexOfCollation > 0)
                {
                    command = command.Remove(indexOfCollation);
                }
            }

            return $"{command} {collate} ENCRYPTED WITH (ENCRYPTION_TYPE = {encryptionType}, ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256', COLUMN_ENCRYPTION_KEY = {keyName}) ";
        }

        private static void UnsupportedColumnEncryped(IDictionary<string, AnnotationValues> annotations, string tableName, string columnName)
        {
            // Obtain annotation.
            annotations.TryGetValue(EncryptedColumnAttribute.AnnotationName, out AnnotationValues encryptedAnnotation);
            if (encryptedAnnotation == null)
            {
                return;
            }

            // Unsupported operation.
            throw new MigrationsException($"Unable to modify column '{columnName}' of table '{tableName}'. Column Always Encrypted must be added when the column is created, this column is already created.");
        }
    }
}