using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Tables
{
    public class Predict
    {
        public long seq { get; set; }
        public int fixture_id { get; set; }
        public string main_label { get; set; }
        public string sub_label { get; set; }
        public short value1 { get; set; }
        public short value2 { get; set; }
        public short value3 { get; set; }
        public short grade { get; set; }
        public bool is_recommned { get; set; }
    }
}