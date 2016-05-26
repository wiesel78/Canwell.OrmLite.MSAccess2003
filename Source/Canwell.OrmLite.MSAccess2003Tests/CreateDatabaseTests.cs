using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Funq;
using System.Text;
using Canwell.OrmLite.MSAccess2003;
using Canwell.OrmLite.MSAccess2003Tests.Entity;
using Canwell.OrmLite.MSAccess2003Tests.Mock.Settings;
using Canwell.OrmLite.MSAccess2003Tests.Utilities;
using NUnit.Framework;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace Canwell.OrmLite.MSAccess2003Tests
{
    [TestFixture]
    public class CreateDatabaseTests
    {
        private static string DatabasePath = "CreateDatabaseTests.mdb";
        private static string Password = "passme";
        private Container Container { get; set; }


        [OneTimeSetUp]
        public void GlobalInit()
        {
            Container = new Container();
        }

        [OneTimeTearDown]
        public void GlobalCleanUp()
        {
            Container.Dispose();

            FileUtilities.DeleteFileThenProcessesGetFree(DatabasePath);
        }

        [Test]
        public void CreateDatabase_FileNotExist_CreatedWithPassword()
        {
            Container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                ConnectionStringEnum.WithPassword.Fmt(DatabasePath, Password),
                MsAccess2003Dialect.Provider
            ));

            var db = Container.Resolve<IDbConnectionFactory>();

            using (var connection = db.OpenDbConnection())
            {
                connection.CreateTableIfNotExists<OptionTableEntity>();
                connection.Insert<OptionTableEntity>(new OptionTableEntity
                {
                    Id = 1,
                    Name = "OptionName",
                    Value = "OptionValue"
                });
            }

            Assert.IsTrue(File.Exists(DatabasePath));
        }
    }
}
