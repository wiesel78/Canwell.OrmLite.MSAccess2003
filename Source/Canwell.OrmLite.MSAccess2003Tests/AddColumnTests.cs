using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canwell.OrmLite.MSAccess2003;
using Canwell.OrmLite.MSAccess2003Tests.Entity;
using Canwell.OrmLite.MSAccess2003Tests.Mock.Settings;
using Canwell.OrmLite.MSAccess2003Tests.Utilities;
using Funq;
using NUnit.Framework;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace Canwell.OrmLite.MSAccess2003Tests
{
    [TestFixture]
    public class AddColumnTests
    {
        private static string DatabasePath = "AddColumnTest.mdb";
        private static string Password = "passme";
        private Container Container { get; set; }


        [OneTimeSetUp]
        public void GlobalInit()
        {
            Container = new Container();

            Container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                ConnectionStringEnum.WithPassword.Fmt(DatabasePath, Password),
                MsAccess2003Dialect.Provider)
            {
                AutoDisposeConnection = true
            });

        }

        [OneTimeTearDown]
        public void GlobalCleanUp()
        {
            Container.Dispose();

            FileUtilities.DeleteFileThenProcessesGetFree(DatabasePath);
        }

        [SetUp]
        public void Init()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.CreateTableIfNotExists<BasicColumnTableEntity>();
            }
        }

        [TearDown]
        public void CleanUp()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.DropTables(TableList.Types);
            }
        }

        [Test]
        public void AddColumn_BasicTableExists_AddColumn()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.AddColumn<ExtendedColumnTableEntity>(c => c.Phone);

                connection.Insert<ExtendedColumnTableEntity>(new ExtendedColumnTableEntity()
                {
                    EMail = "max@domain.com",
                    Name = "Entity Name",
                    Phone = "555 Nose"
                });

                var entity = connection.Select<ExtendedColumnTableEntity>().First();

                Assert.IsNotNull(entity.Phone);
            }
        }
    }
}
