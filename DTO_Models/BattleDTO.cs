using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Models
{
    public class BattleDTO
    {
        public string Player1ID { get; set; }
        public string Player2ID { get; set; }
        public int Army1 { get; set; }
        public int Army2 { get; set; }
        public int Points1 { get; set; }
        public int Points2 { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public double Result { get; set; }
    }
}
