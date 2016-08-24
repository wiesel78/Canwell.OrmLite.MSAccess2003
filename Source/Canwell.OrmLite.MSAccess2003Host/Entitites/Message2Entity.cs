using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Host.Entitites
{
    [Alias("Message")]
    public class Message2Entity
    {
        [AutoIncrement]
        public int Message_id { get; set; }
        public int? Reservierung_id { get; set; }
        public int? Kunde_id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? SendDate { get; set; }
        public int MessageState { get; set; }
        public string MessageSubject { get; set; }
        public string MessageType { get; set; }
        public string MessageSource { get; set; }
        public string MessageSourcePlain { get; set; }
        public string Recipient { get; set; }
        public string RecipientCC { get; set; }
        public string ErrorMessage { get; set; }
        public string UserName { get; set; }
    }
}
