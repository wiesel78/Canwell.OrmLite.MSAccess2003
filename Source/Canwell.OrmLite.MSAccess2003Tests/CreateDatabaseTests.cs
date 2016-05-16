using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Funq;
using System.Text;
using Canwell.OrmLite.MSAccess2003;
using Canwell.OrmLite.MSAccess2003Tests.Mock.Settings;
using NUnit.Framework;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace Canwell.OrmLite.MSAccess2003Tests
{
    [TestFixture]
    public class CreateDatabaseTests
    {
        private static string FilePath = @".\test.mdb";
        private static string Password = "passme";

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void CreateDatabase_FileNotExist_CreatedWithPassword()
        {
            var container = new Container();

            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                ConnectionStringEnum.WithPassword.Fmt(FilePath, Password),
                new MsAccess2003OrmLiteDialectProvider()
            ));

            var db = container.Resolve<IDbConnectionFactory>();

            using (var connection = db.CreateDbConnection())
            {
                Assert.IsTrue(File.Exists(FilePath));   
            }
        }

        [TearDown]
        public void TearDown()
        {
            if(File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}
