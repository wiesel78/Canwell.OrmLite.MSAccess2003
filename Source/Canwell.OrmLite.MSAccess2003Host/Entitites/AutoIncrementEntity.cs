using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Host.Entitites
{
    [Alias("AutoIncrementTable")]
    public class AutoIncrementEntity
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }

        public string TextColumn { get; set; }
    }
}
