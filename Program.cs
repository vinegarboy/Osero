using System;

namespace Osero
{
    class Program
    {
        static void Main(string[] args){
            Bord bd  = new Bord();
            Console.WriteLine("AIオセロ");
            bd.Init_Bord();
            Console.WriteLine(bd.View_bord());
        }
    }
}
