# Canwell.OrmLite.MSAccess2003

## Install

Install the "Microsoft ADO Ext. 2.8 for DDL and Security 2.8" Library 
from the Com-Section in the Reference-Manager. In the Reference-Section of 
your project, find you the Library ADOX. Click on it and see the Properties. 
Set the Property Interop-Type-Embedded of false and local copy of true.

Install Canwell.OrmLite.MSAccess2003 via Nuget

## Usage

```
public override void Configure(Container container)
{
    // a typically msaccess connection string
    var connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Mode=Share Deny None;Jet OLEDB:Database Password={1}";
    connectionString = connectionString.Fmt(@".\pathtoyourdatabase.mdb", "passme");

    // register to your container
    container.Register<IDbConnectionFactory>(
    new OrmLiteConnectionFactory(connectionString, MsAccess2003Dialect.Provider));

    // a select example
    using(var connection = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
    {
        var entities = connection.Select<MessageEntity>(q => q.IsRead == false);

        Console.WriteLine("Unread counter : ", entities.Count);
    }
}
```



