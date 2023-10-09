using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Core.GameSystem.GameModel
{
    public class RaceStudentData
    {
        public int RaceStudentDataId;
        public string id;
        public string idNumber;
        public string name;
        public int score;
        public int RoundId;

        //状态 0:未测试 1:已测试 2:中退 3:缺考 4:犯规
        public int state;
    }
}