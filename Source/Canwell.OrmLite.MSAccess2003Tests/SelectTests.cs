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
    class SelectTests
    {
        private static string DatabasePath = "SelectTests.mdb";
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
        public void Select_FilledGuidTable_GetById()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var entities = connection.Select<GuidTableEntity>(q => q.GuidColumn == GuidTableId);

                Assert.IsNotNull(entities);
                Assert.AreEqual(entities.First().Name, "FirstRow");
            }
        }

        [Test]
        public void Select_FilledGuidTable_GetByName()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var entities = connection.Select<GuidTableEntity>(q => q.Name == "ThirdRow");

                Assert.IsNotNull(entities);
                Assert.AreEqual(entities.First().Name, "ThirdRow");
            }
        }

        [Test]
        public void Select_FilledGuidTable_GetAllRows()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var entities = connection.Select<GuidTableEntity>();

                Assert.IsNotNull(entities);
                Assert.AreEqual(entities.Count, 3);
            }
        }

        [Test]
        public void Select_FilledTimeTable_GetAllAfterDate()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var entities = connection.Select<TimeTableEntity>(q => q.DateTimeColumn > new DateTime(2010,10,10));

                Assert.IsNotNull(entities);
                Assert.AreEqual(entities.Count, 2);
            }
        }

        [Test]
        public void Select_FilledTimeTable_GetAllWithSameDateAs()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var entities = connection.Select<TimeTableEntity>(q => q.DateTimeColumn == new DateTime(2010, 10, 10));

                Assert.IsNotNull(entities);
                Assert.AreEqual(entities.Count, 1);
            }
        }

        [Test]
        public void Select_FilledTimeTable_GetAllTimeSpanGreaterThen()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var entities = connection.Select<TimeTableEntity>(q => q.TimeSpanColumn > new TimeSpan(10000));

                Assert.IsNotNull(entities);
                Assert.AreEqual(entities.Count, 2);
            }
        }

        [Test]
        public void Select_FilledNullableTable_ReadAllValues()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var entities = connection.Select<NullableTableEntity>();

                Assert.IsNotNull(entities);
                Assert.AreEqual(entities.Count, 3);

                Assert.IsNull(entities[0].NullableGuidColumn);
                Assert.IsNull(entities[0].NullableDateTimeColumn);
                Assert.IsNull(entities[0].NullableIntegerColumn);

                Assert.AreNotEqual(Guid.Empty, entities[1].NullableGuidColumn ?? Guid.Empty);
                Assert.AreEqual(new DateTime(2010, 10, 10), entities[1].NullableDateTimeColumn ?? null);
                Assert.AreEqual(42, entities[1].NullableIntegerColumn ?? 0);

                Assert.AreEqual(GuidTableId, entities[2].NullableGuidColumn ?? Guid.Empty);
                Assert.IsNull(entities[2].NullableDateTimeColumn);
                Assert.AreEqual(1337, entities[2].NullableIntegerColumn ?? 0);
            }
        }

        [Test]
        public void Select_FilledNullableTable_SearchNullValues()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var entities = connection.Select<NullableTableEntity>(q => q.NullableGuidColumn == null);

                Assert.IsNotNull(entities);
                Assert.AreEqual(entities.Count,1);
            }
        }

        [Test]
        public void Select_FilledNullableTable_SearchNormalValues()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var entities = connection.Select<NullableTableEntity>(q => q.NullableGuidColumn == GuidTableId);

                Assert.IsNotNull(entities);
                Assert.AreEqual(entities.Count, 1);
                Assert.IsNull(entities.First().NullableDateTimeColumn);
                Assert.AreEqual(entities.First().NullableIntegerColumn, 1337);

            }
        }
    }
}
