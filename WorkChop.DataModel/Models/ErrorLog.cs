using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChop.DataModel.Models
{
    public class ErrorLog
    {
        [BsonId]
        [BsonElement("_id")]
        public Guid ExceptionId { get; set; }
        public string Method { get; set; }
        public DateTime ErrorDateTime { get; set; }
        public string HelpLink { get; set; }
        public string InnerException { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string Exception { get; set; }
        public string UserId { get; set; }

    }
}
