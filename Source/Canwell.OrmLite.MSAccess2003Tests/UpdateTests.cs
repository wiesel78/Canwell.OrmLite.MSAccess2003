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
    public class UpdateTests
    {
        private static string DatabasePath = "UpdateTests.mdb";
        private static string Password = "passme";

        private static Guid GuidTableId = Guid.NewGuid();
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
                connection.CreateTableIfNotExists(TableList.Types);
                connection.Insert<GuidTableEntity>(new GuidTableEntity[]
                {
                    new GuidTableEntity() { Name = "FirstRow", GuidColumn = GuidTableId },
                    new GuidTableEntity() { Name = "SecondRow", GuidColumn = Guid.NewGuid() },
                    new GuidTableEntity() { Name = "ThirdRow", GuidColumn = Guid.NewGuid() }
                });
                connection.Insert<TimeTableEntity>(new TimeTableEntity[]
                {
                    new TimeTableEntity() { TimeSpanColumn = new TimeSpan(30000), DateTimeColumn = new DateTime(2010,10,10)},
                    new TimeTableEntity() { TimeSpanColumn = new TimeSpan(20000), DateTimeColumn = new DateTime(2010,10,11) },
                    new TimeTableEntity() { TimeSpanColumn = new TimeSpan(10000), DateTimeColumn = new DateTime(2010,10,12) }
                });
                connection.Insert<NullableTableEntity>(new NullableTableEntity[]
                {
                    new NullableTableEntity() { NullableGuidColumn = null, NullableDateTimeColumn = null, NullableIntegerColumn = null},
                    new NullableTableEntity() { NullableGuidColumn = Guid.NewGuid(), NullableDateTimeColumn = new DateTime(2010,10,10), NullableIntegerColumn = 42},
                    new NullableTableEntity() { NullableGuidColumn = GuidTableId, NullableDateTimeColumn = null, NullableIntegerColumn = 1337},
                });
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
        public void UpdateNonDefaults_FilledGuidTable_UpdateName()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var updatedName = "UpdateFirstColumn";

                connection.UpdateNonDefaults<GuidTableEntity>(new GuidTableEntity { Name = updatedName }, q => q.GuidColumn == GuidTableId);

                var entity = connection.First<GuidTableEntity>(q => q.GuidColumn == GuidTableId);

                Assert.IsNotNull(entity);
                Assert.AreEqual(entity.Name, updatedName);
            }
        }

        [Test]
        public void Update_FilledGuidTable_UpdateName()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var updatedName = "UpdateFirstColumn";

                connection.Update<GuidTableEntity>(new GuidTableEntity { GuidColumn = GuidTableId, Name = updatedName });

                var entity = connection.First<GuidTableEntity>(q => q.GuidColumn == GuidTableId);

                Assert.IsNotNull(entity);
                Assert.AreEqual(entity.Name, updatedName);
            }
        }

        [Test]
        public void Update_FilledGuidTable_UpdateNameViaAnonymousType()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var updatedName = "UpdateFirstColumn";

                connection.Update<GuidTableEntity>(new { Name = updatedName }, q => q.GuidColumn == GuidTableId);

                var entity = connection.First<GuidTableEntity>(q => q.GuidColumn == GuidTableId);

                Assert.IsNotNull(entity);
                Assert.AreEqual(entity.Name, updatedName);
            }
        }
    }
}
