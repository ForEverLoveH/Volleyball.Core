using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Core.GameSystem.GameModel
{
    public class treeViewModel
    {
        public string createTime { get; set; }
        public List<treeViewSchoolsModel> schools { get; set; }
    }

    public class treeViewSchoolsModel
    {
        public string schoolName { get; set; }
        public List<string> groups { get; set; }
    }
}