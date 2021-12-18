using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.MongodbDemo
{
    public class Customer
    {
        [BsonId]
        public long Id { get; set; }

        public string CellPhone { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
