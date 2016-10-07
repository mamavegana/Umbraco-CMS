﻿using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using NPoco;
using Umbraco.Core.DI;
using Umbraco.Core.Logging;

namespace Umbraco.Core.Persistence
{
    internal static class DbConnectionExtensions
    {
        public static string DetectProviderNameFromConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            var allKeys = builder.Keys.Cast<string>();

            var mySql = new[] { "Server", "Database", "Uid", "Pwd" };
            if (mySql.All(x => allKeys.InvariantContains(x)))
            {
                return Constants.DbProviderNames.MySql;
            }

            if (allKeys.InvariantContains("Data Source")
                //this dictionary is case insensitive
                && builder["Data source"].ToString().InvariantContains(".sdf"))
            {
                return Constants.DbProviderNames.SqlCe;
            }

            return Constants.DbProviderNames.SqlServer;
        }

        public static bool IsConnectionAvailable(string connectionString, string providerName)
        {
            if (providerName != Constants.DbProviderNames.SqlCe
                && providerName != Constants.DbProviderNames.MySql
                && providerName != Constants.DbProviderNames.SqlServer)
                throw new NotSupportedException($"Provider \"{providerName}\" is not supported.");

            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();

            if (connection == null)
                throw new InvalidOperationException($"Could not create a connection for provider \"{providerName}\".");

            connection.ConnectionString = connectionString;
            using (connection)
            {
                return connection.IsAvailable();
            }
        }

        public static bool IsAvailable(this IDbConnection connection)
        {
            try
            {
                connection.Open();
                connection.Close();
            }
            catch (DbException e)
            {
                // Don't swallow this error, the exception is super handy for knowing "why" its not available
                Current.Logger.Warn<IDbConnection>(e, "Configured database is reporting as not being available.");
                return false;
            }

            return true;
        }


    }
}
