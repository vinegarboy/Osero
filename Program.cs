using System;

namespace Osero
{
    class Program
    {
        static void Main(string[] args){
            //インスタンス変数の宣言
            Bord bd  = new Bord();

            //初期化処理
            Console.WriteLine("AIオセロ");
            bd.Init_Bord();
            Console.WriteLine(bd.View_bord());

            //AIクラスのインスタンス変数の宣言
            AISys porn = new AISys(1);
            AISys john = new AISys(2);

            //ターンを数える変数
            int tarn = 1;

            //勝敗が決するまで繰り返す
            while(bd.judge_winner()==-1){

                //ポーンに学習させる
                porn.SetBord(bd.bord);

                //ポジションを決定させる
                int[] porn_pos = porn.most_flip_put_stone();

                //デバッグメッセージ
                Console.WriteLine($"ポーンの置く場所:{porn_pos[0]},{porn_pos[1]}");

                //エラー数値が返ってきていない場合は設置する。
                if(porn_pos[0] != -1&&porn_pos[1] != -1){
                    bd.put_stone(porn_pos[0],porn_pos[1],porn.getMyColor());
                    Console.WriteLine($"\n{tarn}ターン目の盤面\nポーンの番\n"+bd.View_bord());
                }

                //ジョンに盤面を見せる
                john.SetBord(bd.bord);

                //ジョンのポジションを決定
                int[] john_pos = john.most_flip_put_stone();

                //デバッグメッセージ
                Console.WriteLine($"ジョンの置く場所:{john_pos[0]},{john_pos[1]}");

                //エラー数値が返ってきていない場合は設置する
                if(john_pos[0] != -1&&john_pos[1] != -1){
                    bd.put_stone(john_pos[0],john_pos[1],john.getMyColor());
                    Console.WriteLine($"\n{tarn}ターン目の盤面\nジョンの番\n"+bd.View_bord());
                }

                //ターン数追加
                tarn++;
            }

            //結果発表
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
