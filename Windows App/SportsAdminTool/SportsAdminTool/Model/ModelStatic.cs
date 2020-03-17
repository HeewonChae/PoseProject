using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsAdminTool.Model
{
    public static class ModelStatic
    {
        public static void Init()
        {
            // Football Data Mapping API Model to App Model
            Model.Football.Mapper.FootballMapper.Mapping();
        }
    }
}