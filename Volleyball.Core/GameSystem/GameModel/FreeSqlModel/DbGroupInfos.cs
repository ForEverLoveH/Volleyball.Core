using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Core.GameSystem.GameModel
{
    public class DbGroupInfos
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        public string CreateTime { get; set; }

        public int SortId { get; set; }

        public int IsRemoved { get; set; }

        public string ProjectId { get; set; }

        public string Name { get; set; }

        public int IsAllTested { get; set; }

        public int State { get; set; }
    }
}