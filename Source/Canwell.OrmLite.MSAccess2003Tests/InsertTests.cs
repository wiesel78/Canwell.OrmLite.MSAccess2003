using System;
using System.Data.OleDb;
using System.Linq;
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
    public class InsertTests
    {
        private static string DatabasePath = "InsertTest.mdb";
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
                connection.CreateTableIfNotExists<NumberTableEntity>();
                connection.CreateTableIfNotExists<TimeTableEntity>();
                connection.CreateTableIfNotExists<TextTableEntity>();
                connection.CreateTableIfNotExists<NullableTableEntity>();
                connection.CreateTableIfNotExists<GuidTableEntity>();
                connection.CreateTableIfNotExists<AnnotationTableEntity>();
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
        public void Insert_InsertNumbers_InsertSuccessful()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.Insert<NumberTableEntity>(new NumberTableEntity()
                {
                    ByteColumn = 200,
                    DoubleColumn = 0.001,
                    IntegerColumn = 300000,
                    LongIntegerColumn = 300000,
                    ShortColumn = 200
                });

                var values = connection.Select<NumberTableEntity>().First();

                Assert.AreEqual(values.ByteColumn, 200);
                Assert.AreEqual(values.DoubleColumn, 0.001);
                Assert.AreEqual(values.IntegerColumn, 300000);
                Assert.AreEqual(values.LongIntegerColumn, 300000);
                Assert.AreEqual(values.ShortColumn, 200);
            }
        }

        [Test]
        public void Insert_InsertText_InsertSuccessful()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.Insert<TextTableEntity>(new TextTableEntity()
                {
                    CharColumn = 'd',
                    TextColumn = "hallo welt"
                });

                var values = connection.Select<TextTableEntity>().First();

                Assert.AreEqual(values.CharColumn, 'd');
                Assert.AreEqual(values.TextColumn, "hallo welt");
            }
        }

        [Test]
        public void Insert_InsertTime_InsertSuccessful()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.Insert<TimeTableEntity>(new TimeTableEntity()
                {
                    DateTimeColumn = new DateTime(2000,1,1,1,1,1),
                    TimeSpanColumn = new TimeSpan(2, 2, 2, 2)
                });

                var values = connection.Select<TimeTableEntity>().First();

                Assert.AreEqual(values.DateTimeColumn.Ticks, new DateTime(2000, 1, 1, 1, 1, 1).Ticks);
                Assert.AreEqual(values.TimeSpanColumn.Ticks, new TimeSpan(2, 2, 2, 2).Ticks);
            }
        }

        [Test]
        public void Insert_InsertNullable_InsertSuccessful()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.Insert<NullableTableEntity>(new NullableTableEntity()
                {
                    NullableDateTimeColumn = (DateTime?)null,
                    NullableGuidColumn = (Guid?)null,
                    NullableIntegerColumn = (int?)null
                });

                var values = connection.Select<NullableTableEntity>().First();

                Assert.IsNull(values.NullableDateTimeColumn);
                Assert.IsNull(values.NullableGuidColumn);
                Assert.IsNull(values.NullableIntegerColumn);

                connection.Insert<NullableTableEntity>(new NullableTableEntity()
                {
                    NullableDateTimeColumn = new DateTime(2000, 1, 1, 1, 1, 1),
                    NullableGuidColumn = Guid.NewGuid(),
                    NullableIntegerColumn = 3000000
                });

                values = connection.Select<NullableTableEntity>().Last();

                Assert.IsNotNull(values.NullableDateTimeColumn);
                Assert.IsNotNull(values.NullableGuidColumn);
                Assert.IsNotNull(values.NullableIntegerColumn);

                Assert.AreEqual(values.NullableDateTimeColumn.Value.Ticks, new DateTime(2000, 1, 1, 1, 1, 1).Ticks);
                Assert.AreEqual(values.NullableIntegerColumn.Value, 3000000);
                Assert.AreNotEqual(values.NullableGuidColumn.Value, Guid.Empty);
            }
        }

        [Test]
        public void Insert_InsertAnnotation_InsertSuccessful()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.Insert<AnnotationTableEntity>(new AnnotationTableEntity()
                {
                    UniqueStringColumn = "uniqueString"
                });

                var values = connection.Select<AnnotationTableEntity>().First();

                Assert.AreEqual(values.Id, 1);
                Assert.AreEqual(values.DefaultBooleanColumn, false);
                Assert.AreEqual(values.UniqueStringColumn, "uniqueString");

                connection.Insert<AnnotationTableEntity>(new AnnotationTableEntity()
                {
                    UniqueStringColumn = "uniqueString2",
                    DefaultBooleanColumn = true
                });

                values = connection.Select<AnnotationTableEntity>().Last();

                Assert.AreEqual(values.Id, 2);
                Assert.AreEqual(values.DefaultBooleanColumn, true);
                Assert.AreEqual(values.UniqueStringColumn, "uniqueString2");

                Assert.Throws<OleDbException>(() => connection.Insert<AnnotationTableEntity>(new AnnotationTableEntity()
                {
                    UniqueStringColumn = "uniqueString"
                }));
            }
        }
    }
}
