using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Canwell.OrmLite.MSAccess2003;
using Canwell.OrmLite.MSAccess2003Host.Dtos;
using Canwell.OrmLite.MSAccess2003Host.Entitites;
using Funq;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace Canwell.OrmLite.MSAccess2003Host
{
    class Program
    {
        private static string FilePath = @".\test.mdb";
        private static string FilePassword = "passme";

        static string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Mode=Share Deny None;Jet OLEDB:Database Password={1}";
        //static string ConnectionString = ":memory:";

        static void Main(string[] args)
        {
            ConnectionString = ConnectionString.Fmt(FilePath, FilePassword);

            File.Delete(FilePath);

            var container = new Container();

            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(ConnectionString,
                MsAccess2003Dialect.Provider));
            //container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(ConnectionString,
            //    SqliteDialect.Provider));

            OrmLiteCommand(container);

            //BlockInput();
        }

        static void BlockInput()
        {
            while (true)
            {
                Console.ReadLine();
            }
        }

        static void OrmLiteCommand(Container container)
        {
            using(var connection = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                connection.CreateTableIfNotExists<AutoIncrementTest1Entity>();
                //GUID DEFAULT GenGUID()
                //ALTER COLUMN Id AUTOINCREMENT
                connection.AlterColumn<AutoIncrementTest3Entity>(x => x.Id);

                connection.SaveAll(new List<AutoIncrementTest3Entity>()
                {
                    new AutoIncrementTest3Entity()
                    {
                        Name = "Hans"
                    },
                    new AutoIncrementTest3Entity()
                    {
                        Name = "Wurst"
                    },
                    new AutoIncrementTest3Entity()
                    {
                        Name = "Karl"
                    },
                    new AutoIncrementTest3Entity()
                    {
                        Name = "Pupsnase"
                    }
                });

                var entities = connection.Select<AutoIncrementTest3Entity>();

                Console.WriteLine("{0}", entities);

            }
        }
    }
}
