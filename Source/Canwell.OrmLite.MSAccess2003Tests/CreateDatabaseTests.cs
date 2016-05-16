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
        private Container Container { get; set; }

        [SetUp]
        public void SetUp()
        {
            Container = new Container();
        }

        [Test]
        public void CreateDatabase_FileNotExist_CreatedWithPassword()
        {
            Container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                ConnectionStringEnum.WithPassword.Fmt(FilePath, Password),
                MsAccess2003Dialect.Provider
            ));

            var db = Container.Resolve<IDbConnectionFactory>();

            Assert.IsTrue(File.Exists(FilePath));
        }

        [TearDown]
        public void TearDown()
        {
            if(File.Exists(FilePath))
                File.Delete(FilePath);

            Container.Dispose();
        }
    }
}
