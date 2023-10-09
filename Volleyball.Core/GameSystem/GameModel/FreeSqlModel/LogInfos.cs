using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Core.GameSystem.GameModel
{
    public class LogInfos
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Remark { get; set; }
        public string IdNumber { get; set; }
        public int State { get; set; }
        public string CreateTime { get; set; }
    }
}