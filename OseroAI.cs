using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Osero;

namespace OseroAI{
    class AISystem{
        //AIの脳内に存在する盤面情報
        Bord bord = new Bord();

        //自分の石の色
        int mycolor = 0;


        //自分の石の情報をもらい取得する。
        public AISystem(int color){
            mycolor = color;
        }

        //盤面情報をセットする
        public void SetBord(int[,] bord_data){
            bord.bord = bord_data;
        }

        //自分の色を返す
        public int getMyColor(){
            return mycolor;
        }

        //盤面のデータからランダムにおける位置に置く
        public int[] non_consider_put_stone(){

            //置ける場所を取得する。
            List<int[]> choice_list = bord.can_put_list(mycolor);

            //おける場所がない場合、存在しない座標を返す。
            if(choice_list.Count == 0){
                return new int[]{-1,-1};
            }

            //ランダムにおける場所を返す。
            return choice_list[new Random().Next(choice_list.Count)];
        }

        //最も置ける場所に石を置く。
        public int[] most_flip_put_stone(){
            int maxFlips = -1;
            int[] maxCoords = new int[] { -1, -1 };

            // 置ける場所を取得する。
            List<int[]> choiceList = bord.can_put_list(mycolor);

            // おける場所がない場合、存在しない座標を返す。
            if (choiceList.Count == 0){
                return new int[] { -1, -1 };
            }

            // 最も多くの石をひっくり返せる場所を探す。
            foreach (int[] coords in choiceList){
                int flips = CountFlipStones(coords[0], coords[1], mycolor);
                if (flips > maxFlips){
                    maxFlips = flips;
                    maxCoords = coords;
                }
            }
            return maxCoords;
        }


        //ひっくり返せる石の数を数える。
        private int CountFlipStones(int x, int y, int color){
            //座標とかの入力値のチェック
            if (x < 0 || x >= 8 || y < 0 || y >= 8 || (color != 1 && color != 2)){
                return -1;
            }

            //石がすでに配置されていないかをチェックする。
            if (bord.bord[x,y] != 0){
                return -1;
            }

            //カウント変数
            int count = 0;

            //走査して石を数える。
            for (int dx = -1; dx <= 1; dx++){
                for (int dy = -1; dy <= 1; dy++){

                    //中心は無視
                    if (dx == 0 && dy == 0){
                        continue;
                    }

                    //探索先の設定
                    int nx = x + dx;
                    int ny = y + dy;
                    int flipCount = 0;

                    //石を置ける限り走査を続ける
                    while (nx >= 0 && nx < 8 && ny >= 0 && ny < 8 && bord.bord[nx,ny] != 0 && bord.bord[nx,ny] != color){
                        flipCount++;
                        nx += dx;
                        ny += dy;
                    }

                    //カウントがある場合は代入する。
                    if (nx >= 0 && nx < 8 && ny >= 0 && ny < 8 && bord.bord[nx,ny] == color && flipCount > 0){
                        count += flipCount;
                    }
                }
            }
            return count;
        }

    }
}