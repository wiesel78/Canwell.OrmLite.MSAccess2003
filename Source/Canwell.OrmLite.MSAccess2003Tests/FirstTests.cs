using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canwell.OrmLite.MSAccess2003;
using Canwell.OrmLite.MSAccess2003Tests.Entity;
using Canwell.OrmLite.MSAccess2003Tests.Enums;
using Canwell.OrmLite.MSAccess2003Tests.Mock.Settings;
using Canwell.OrmLite.MSAccess2003Tests.Utilities;
using Funq;
using NUnit.Framework;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace Canwell.OrmLite.MSAccess2003Tests
{
    [TestFixture]
    class FirstTests
    {
        private static string DatabasePath = "FirstTests.mdb";
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
                connection.CreateTableIfNotExists<OptionTableEntity>();
            }
        }

        [TearDown]
        public void CleanUp()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.DropTables(typeof(OptionTableEntity));
            }
        }

        [Test]
        public void FirstOrDefault_EmptyTable_EntityIsNull()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                var entity = connection.FirstOrDefault<OptionTableEntity>(q => q.Id == ((int)TestValuesEnum.FirstValue));

                //var entity =
                //    connection.SqlList<OptionTableEntity>(
                //        "SELECT TOP 1 `Id` ,`Name` ,`Value` FROM `OptionTable` WHERE (`Id` = 1) LIMIT 1");

                Assert.IsNull(entity);
            }
        }

        [Test]
        public void FirstOrDefault_FillesTableWithoutExpectedElement_EntityIsNull()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.Insert<OptionTableEntity>(new OptionTableEntity[]
                {
                    new OptionTableEntity(){Id = (int)TestValuesEnum.SecondValue, Value = "SecondValue", Name = "Second"}, 
                    new OptionTableEntity(){Id = (int)TestValuesEnum.ThirdValue, Value = "ThirdValue", Name = "Third"}, 
                    new OptionTableEntity(){Id = (int)TestValuesEnum.FourthValue, Value = "FourthValue", Name = "Fourth"}, 
                    new OptionTableEntity(){Id = (int)TestValuesEnum.FifthValue, Value = "FifthValue", Name = "Fifth"}
                });

                var entity = connection.FirstOrDefault<OptionTableEntity>(q => q.Name == "First");

                Assert.IsNull(entity);
            }
        }

        [Test]
        public void FirstOrDefault_FillesTableWithExpectedElement_EntitySelected()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.Insert<OptionTableEntity>(new OptionTableEntity[]
                {
                    new OptionTableEntity(){Id = (int)TestValuesEnum.FirstValue, Value = "FirstValue", Name = "First"},
                    new OptionTableEntity(){Id = (int)TestValuesEnum.SecondValue, Value = "SecondValue", Name = "Second"}, 
                    new OptionTableEntity(){Id = (int)TestValuesEnum.ThirdValue, Value = "ThirdValue", Name = "Third"}, 
                    new OptionTableEntity(){Id = (int)TestValuesEnum.FourthValue, Value = "FourthValue", Name = "Fourth"}, 
                    new OptionTableEntity(){Id = (int)TestValuesEnum.FifthValue, Value = "FifthValue", Name = "Fifth"}
                });

                var entity = connection.FirstOrDefault<OptionTableEntity>(q => q.Id == ((int)TestValuesEnum.FirstValue));

                Assert.IsNotNull(entity);
            }
        }
    }
}
