using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            //Console.WriteLine("");
            Console.WriteLine(Int10CalcTo32(32 * 32 * 32 * 32 - 2,5)); ;
           // Cal32ToInt10("ZZ");
           Console.ReadKey();
        }

        public static string Int10CalcTo32(int inputNum,int maxSize)
        {
            int max = 0;
            var result = new string[20];
            var displayArr = new string[]
            {
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "M",
                "N", "P", "Q", "R", "T", "U", "V", "W", "X", "Y", "Z"
            }; 
            int ten = inputNum;
            int arrSize = displayArr.Length;
            string lResult = "";
            do
            {
                var sixteen = ten % arrSize;
                ten = ten / arrSize;
                result[max] = displayArr[sixteen];
                lResult = result[max]+ lResult;
                max++;
            } while (ten != 0);
            lResult = lResult.PadLeft(maxSize, '0');
            return lResult;
        }

        public static double Cal32ToInt10(string inputNum)
        {
            if (string.IsNullOrEmpty(inputNum))
            {
                return -1;
            }
            var displayStr = "0123456789ABCDEFGHJKMNPQRTUVWXYZ";
            int disLength = displayStr.ToArray().Length;
            double numResult = 0;
            var inputArr = inputNum.ToArray().Reverse().ToList();
            
            for (int i=0;i < inputArr.Count;i++)
            {
                int index = displayStr.IndexOf(inputArr[i]);
                if (index < 0)
                {
                    return -1;
                }
                numResult += index* Math.Pow(disLength, i);
            }

            return numResult;
        }
    }
}
