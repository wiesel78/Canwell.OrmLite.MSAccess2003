using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using Funq;
using ServiceStack.OrmLite.MSAccess2003;
using ServiceStack.OrmLite.MSAccess2003Host.Dtos;
using ServiceStack.OrmLite.MSAccess2003Host.Entitites;
using ServiceStack.Text;

namespace ServiceStack.OrmLite.MSAccess2003Host
{
    class Program
    {
        private static string FilePath = @".\test.mdb";
        static string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Mode=Share Deny None;Jet OLEDB:Database Password=passme";

        static void Main(string[] args)
        {
            ConnectionString = ConnectionString.Fmt(FilePath);

            var container = new Container();

            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(ConnectionString,
                MsAccess2003Dialect.Provider));

            // switch between native sql over oledb and OrmLiteAbstractionLayer
            if(false)
                OledDbCommand(container);
            else
                OrmLiteCommand(container);

            BlockInput();
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
            var dbFactory = container.Resolve<IDbConnectionFactory>();

            using (var db = dbFactory.OpenDbConnection())
            {
                db.CreateTableIfNotExists<ExtendedTypesEntity>();

                var entity = new ExtendedTypesEntity()
                {
                    Id = Guid.NewGuid(),
                    BinaryColumn = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    StringColumn = new string[] { "asd", "sss", "aaa" },
                    TestObjectColumn = new TestObject()
                    {
                        BooleanField = true,
                        StringField = "bla"
                    }
                };

                db.Insert<ExtendedTypesEntity>(entity);

                var afterInsertEntity = db.Select<ExtendedTypesEntity>(s => s.Id == entity.Id)[0];

                Console.WriteLine("after insert : {0}", afterInsertEntity.BinaryColumn.Length);

                entity.BinaryColumn = new byte[] { 9, 8, 7 };
                entity.StringColumn = new string[] { "z" };
                entity.TestObjectColumn = new TestObject()
                {
                    BooleanField = false,
                    StringField = "blub"
                };

                db.Update<ExtendedTypesEntity>(entity);

                var afterUpdateEntity = db.Select<ExtendedTypesEntity>(s => s.Id == entity.Id)[0];

                Console.WriteLine("after update : {0}", afterUpdateEntity.BinaryColumn.Length);
            }
        }

        static void OledDbCommand(Container container )
        {
            var ormConnectionFactory = container.Resolve<IDbConnectionFactory>() as OrmLiteConnectionFactory;
            var ormConnection = ormConnectionFactory.CreateDbConnection() as OrmLiteConnection;
            var accessConnection = ormConnection.DbConnection as MsAccess2003DbConnection;
            var connection = accessConnection.Connection;

            connection.Open();

            using (var command = connection.CreateCommand())
            {
                var id = Guid.NewGuid().ToString();

                command.CommandText =
                    "INSERT INTO `TestTypeTable` (`Id`,`CharColumn`,`TextColumn`,`BooleanColumn`,`ByteColumn`,`ShortColumn`,`IntegerColumn`,`LongIntegerColumn`,`DoubleColumn`,`DateColumn`) VALUES ('{" + id + "}','a','Hallo Welt',FALSE,1,100,3000,3000,2.3,'14.05.2016 19:02:53')";
                command.ExecuteNonQuery();

                command.CommandText =
                    "UPDATE TestTypeTable SET BooleanColumn = FALSE \nWHERE `Id` = '{"+id+"}';";
                command.ExecuteNonQuery();

                command.CommandText = "SELECT `Id` ,`CharColumn` ,`TextColumn` ,`BooleanColumn` ,`ByteColumn` ,`ShortColumn` ,`IntegerColumn` ,`LongIntegerColumn` ,`DoubleColumn` ,`DateColumn`  \nFROM `TestTypeTable` \nWHERE `Id` = '{"+id+"}';";
                var reader = command.ExecuteReader(CommandBehavior.Default);
                var result = new List<TypeEntity>();

                while (reader.Read())
                {
                    var value = reader[3];

                    Console.WriteLine("{0}", value);
                }
            }

            connection.Close();
        }
    }
}
