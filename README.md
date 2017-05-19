# EntityFramework-AlwaysEncrypted-AzureKeyVault
Entity Framework extension for Always Encrypted support through Azure Key Vault

## Prerequisites
- Microsoft SQL Server 2016 or Azure Database
- Azure Key Vault access (on keys: get, wrap, unwrap)
- At least one key uploaded into the Key Vault
- At least one Column Master key created in the SQL database
- At least one Column encryption key created based on one or two Column Master keys

## Additional informations
- [Always Encrypted with Azure Key Vault tutorial](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-always-encrypted-azure-key-vault)
- [Configure Always Encrypted with SSMS](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/configure-always-encrypted-using-sql-server-management-studio)
- [Create Column Master key](https://docs.microsoft.com/en-us/sql/t-sql/statements/create-column-master-key-transact-sql)
- [Create Column Encryption key](https://docs.microsoft.com/en-us/sql/t-sql/statements/create-column-encryption-key-transact-sql) (Use of SSMS in recommended)
- [Getting Access Token from Azure AD](https://docs.microsoft.com/en-us/azure/key-vault/key-vault-use-from-web-application)
- [Always Encrypted features and limitations](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/always-encrypted-database-engine)

## Usage
- Create an implementation of the `IAccessTokenProvider` (for more information see Getting Access Token from Azure AD)
- Derive your DbContext from the `AlwaysEncryptedDbContext`
- Add the `Column Encryption Setting=Enabled` setting to your connection sting
- Add the `EncryptedColumnAttribute` to the desired entity property
- Create a new migration file with the `Add-Migration` command
- Run the migration with the `Update-Database` command

## Notes
- Filters and joins can only be made on deterministic encrypted columns
- If filtering a collection the filter variable must be placed into a separate variable before passing it to the expression
- __The migration will only works for newly created columns__
- __The columns has to be nullable__
- Please see the limitatios for supported data types