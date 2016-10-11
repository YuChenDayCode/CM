using CMDB;

namespace MvcApplication.Models
{
    public class TEST_CZ
    {
        [Col_(IsPrimaryKey = true)]
        public int? ID { get; set; }

        public string CONTENT { get; set; }

        public int? ITEM { get; set; }

        public string ISOK { get; set; }
    }
}