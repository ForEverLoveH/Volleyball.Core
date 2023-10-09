using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Core.GameSystem.GameModel
{
    public class SportProjectInfos
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        public string CreateTime { get; set; }

        public int SortId { get; set; }

        public int IsRemoved { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public int RoundCount { get; set; }

        public int BestScoreMode { get; set; }

        public int TestMethod { get; set; }

        public int FloatType { get; set; }

        public int TurnsNumber0 { get; set; }

        public int TurnsNumber1 { get; set; }
    }
}