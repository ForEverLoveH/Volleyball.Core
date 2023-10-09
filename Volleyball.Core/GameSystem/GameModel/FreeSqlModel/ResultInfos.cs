using FreeSql.DataAnnotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Core.GameSystem.GameModel
{
    internal class ResultInfos
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        public string CreateTime { get; set; }

        public int SortId { get; set; }

        public int IsRemoved { get; set; }

        public string PersonId { get; set; }

        public int SportItemType { get; set; }

        public string PersonName { get; set; }

        public string PersonIdNumber { get; set; }

        public int RoundId { get; set; }

        public double Result { get; set; }

        public int State { get; set; }

        public int uploadState { get; set; }
    }
}