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
    public class SaveTests
    {
        private static string DatabasePath = "SaveTests.mdb";
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
                connection.CreateTableIfNotExists<OptionTableEntity>();
                connection.CreateTableIfNotExists<ReferenceTableEntity>();
                connection.CreateTableIfNotExists<PrimaryGuidEntity>();

                connection.Insert<OptionTableEntity>(new OptionTableEntity[]
                {
                    new OptionTableEntity() { Name = "Option1", Value = "OptionValue1"},
                    new OptionTableEntity() { Name = "Option2", Value = "OptionValue2"}
                });
                connection.Insert <ReferenceTableEntity> (new ReferenceTableEntity[]
                {
                    new ReferenceTableEntity() { Name = "ReferenceName1", OptionsId = 1},
                    new ReferenceTableEntity() { Name = "ReferenceName2", OptionsId = 1}
                });
            }
        }

        [TearDown]
        public void CleanUp()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.DropTable<ReferenceTableEntity>();
                connection.DropTable<OptionTableEntity>();
                connection.DropTable<PrimaryGuidEntity>();
            }
        }

        [Test]
        public void Save_FilledReferenceTable_SaveNewEntity()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.Save<ReferenceTableEntity>(new ReferenceTableEntity()
                {
                    Name = "ReferenceName3",
                    OptionsId = 2
                });

                var entity = connection.First<ReferenceTableEntity>(q => q.Id == 3);

                Assert.IsNotNull(entity);
                Assert.AreEqual(entity.Name, "ReferenceName3");
            }
        }

        [Test]
        public void Save_FilledReferenceTable_SaveExistsEntity()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {

                var entity = connection.First<ReferenceTableEntity>(q => q.Id == 1);
                entity.Name = "ReferenceNameSave1";

                connection.Save<ReferenceTableEntity>(entity);

                var entitySaved = connection.First<ReferenceTableEntity>(q => q.Id == 1);

                Assert.IsNotNull(entity);
                Assert.AreEqual(entity.Name, "ReferenceNameSave1");
            }
        }

        [Test]
        public void Save_FilledReferenceTable_SaveExistsEntityList()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {

                var entities = connection.Select<ReferenceTableEntity>();

                entities[0].Name = "ReferenceNameSave1";
                entities[1].Name = "ReferenceNameSave2";

                connection.SaveAll<ReferenceTableEntity>(entities);

                var entitiesSaved = connection.Select<ReferenceTableEntity>();

                Assert.IsNotNull(entitiesSaved);
                Assert.AreEqual(entitiesSaved.Count, 2);
                Assert.AreEqual(entitiesSaved[0].Name, "ReferenceNameSave1");
                Assert.AreEqual(entitiesSaved[1].Name, "ReferenceNameSave2");
            }
        }

        [Test]
        public void SaveAll_EmptyPrimaryGuidTable_FillWithGuidIsSetEntities()
        {
            using (var connection = Container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {

                var entities = new List<PrimaryGuidEntity>()
                {
                    new PrimaryGuidEntity()
                    {
                        UserName = "Philip"
                    },
                    new PrimaryGuidEntity()
                    {
                        UserName = "Peter"
                    }
                };

                Assert.DoesNotThrow(() => connection.SaveAll<PrimaryGuidEntity>(entities));
            }
        }
    }
}
