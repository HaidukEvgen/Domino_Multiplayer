using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DominoClient
{
    public class Domino
    {
        public int FirstNum { set; get; }
        public int SecondNum { set; get; }

        public Domino(int firstNum, int secondNum)
        {
            FirstNum = firstNum;
            SecondNum = secondNum;
        }

    }
}
