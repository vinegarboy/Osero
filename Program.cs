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

            AISys porn = new AISys(1);
            AISys john = new AISys(2);
            int tarn = 1;
            while(bd.judge_winner()==-1){
                porn.SetBord(bd.bord);

                int[] porn_pos = porn.most_flip_put_stone();

                Console.WriteLine($"ポーンの置く場所:{porn_pos[0]},{porn_pos[1]}");

                if(porn_pos[0] != -1&&porn_pos[1] != -1){
                    bd.put_stone(porn_pos[0],porn_pos[1],porn.getMyColor());
                    Console.WriteLine($"\n{tarn}ターン目の盤面\nポーンの番\n"+bd.View_bord());
                }

                john.SetBord(bd.bord);

                int[] john_pos = john.most_flip_put_stone();
                Console.WriteLine($"ジョンの置く場所:{john_pos[0]},{john_pos[1]}");

                if(john_pos[0] != -1&&john_pos[1] != -1){
                    bd.put_stone(john_pos[0],john_pos[1],john.getMyColor());
                    Console.WriteLine($"\n{tarn}ターン目の盤面\nジョンの番\n"+bd.View_bord());
                }
                tarn++;
            }

            Console.WriteLine($"\n最終結果\n"+bd.View_bord());

            if(bd.judge_winner()==1){
                Console.WriteLine("ポーンの勝ち");
            }else if(bd.judge_winner()==2){
                Console.WriteLine("ジョンの勝ち");
            }else{
                Console.WriteLine("引き分け");
            }
        }
    }
}
