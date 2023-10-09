using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Core.GameSystem.GameModel
{
    public class DbPersonInfos
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        public string CreateTime { get; set; }

        public int SortId { get; set; }

        public int IsRemoved { get; set; }

        public string ProjectId { get; set; }

        public string SchoolName { get; set; }

        public string GradeName { get; set; }

        public string ClassNumber { get; set; }

        public string GroupName { get; set; }

        public string Name { get; set; }

        public string IdNumber { get; set; }

        public int Sex { get; set; }

        public int State { get; set; }

        public int FinalScore { get; set; }

        public string BeginTime { get; set; }

        public int uploadState { get; set; }

        public string uploadGroup { get; set; }
    }
}