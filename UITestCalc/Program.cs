using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UITestCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            Calc a = new Calc();
            a.StartApp();
            a.Test1();
            a.Test2();
            a.CloseApp();
        }
    }
}
