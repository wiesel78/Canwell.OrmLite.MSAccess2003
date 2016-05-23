using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Canwell.OrmLite.MSAccess2003;
using Canwell.OrmLite.MSAccess2003Tests.Entity;
using Canwell.OrmLite.MSAccess2003Tests.Mock.Settings;
using Canwell.OrmLite.MSAccess2003Tests.Utilities;
using Funq;
using NUnit.Framework;
using ServiceStack.Common.Extensions;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace Canwell.OrmLite.MSAccess2003Tests
{
    [TestFixture]
    public class CreateTableTests
    {
        private static string DatabasePath = "CreateTableTest.mdb";
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

        [TearDown]
        public void CleanUp()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.DropTables(TableList.Types);
            }
        }


        [TestCase(typeof(NumberTableEntity))]
        [TestCase(typeof(TextTableEntity))]
        [TestCase(typeof(TimeTableEntity))]
        [TestCase(typeof(GuidTableEntity))]
        [TestCase(typeof(NullableTableEntity))]
        [TestCase(typeof(AnnotationTableEntity))]
        public void CreateTableIfNotExists_TableNotExist_TableCreated(Type entity)
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.CreateTableIfNotExists(entity);

                Assert.IsTrue(connection.TableExists(AttributeUtilities.GetAlias(entity)));
            }
        }

        [TestCase(typeof(NumberTableEntity))]
        [TestCase(typeof(TextTableEntity))]
        [TestCase(typeof(TimeTableEntity))]
        [TestCase(typeof(GuidTableEntity))]
        [TestCase(typeof(NullableTableEntity))]
        [TestCase(typeof(AnnotationTableEntity))]
        public void CreateTable_TableNotExist_TableCreated(Type entity)
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.CreateTable(false,entity);

                Assert.IsTrue(connection.TableExists(AttributeUtilities.GetAlias(entity)));
            }
        }

        [Test]
        public void CreateTables_TableNotExist_TableCreated()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.CreateTables(false, new Type[]
                {
                    typeof(NumberTableEntity),
                    typeof(TextTableEntity),
                    typeof(TimeTableEntity),
                    typeof(GuidTableEntity),
                    typeof(NullableTableEntity),
                    typeof(AnnotationTableEntity)
                });

                Assert.IsTrue(connection.TableExists(AttributeUtilities.GetAlias<NumberTableEntity>()));
                Assert.IsTrue(connection.TableExists(AttributeUtilities.GetAlias<TextTableEntity>()));
                Assert.IsTrue(connection.TableExists(AttributeUtilities.GetAlias<TimeTableEntity>()));
                Assert.IsTrue(connection.TableExists(AttributeUtilities.GetAlias<GuidTableEntity>()));
                Assert.IsTrue(connection.TableExists(AttributeUtilities.GetAlias<NullableTableEntity>()));
                Assert.IsTrue(connection.TableExists(AttributeUtilities.GetAlias<AnnotationTableEntity>()));
            }
        }

        [Test]
        public void DropTable_TableExists_TableDroped()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.CreateTableIfNotExists<NumberTableEntity>();

                connection.DropTable<NumberTableEntity>();

                Assert.IsFalse(connection.TableExists(AttributeUtilities.GetAlias<NumberTableEntity>()));
            }
        }



        [Test]
        public void DropAndCreateTable_TableExists_TableCreated()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.CreateTableIfNotExists<NumberTableEntity>();
                connection.DropAndCreateTable<NumberTableEntity>();

                Assert.IsTrue(connection.TableExists(AttributeUtilities.GetAlias<NumberTableEntity>()));
            }
        }
    }
}
