using System;
namespace DominoServer
{
    public class Domino
    {
        public int FirstNum { get; set;}
        public int SecondNum { get; set;}
        
        public bool IsDouble { get; set; }
        public Domino(int firstNum, int secondNum)
        {
            FirstNum = firstNum;
            SecondNum = secondNum;
            IsDouble = firstNum == secondNum;

        }

        public Domino (Domino domino)
        {
            FirstNum = domino.FirstNum;
            SecondNum = domino.SecondNum;
            IsDouble = FirstNum == SecondNum;
        }
        public override bool Equals(object obj)
        {
            return obj is Domino domino &&
                   FirstNum == domino.FirstNum &&
                   SecondNum == domino.SecondNum;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return FirstNum.ToString() + ":" + SecondNum.ToString();
        }

    }
}
