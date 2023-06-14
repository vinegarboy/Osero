using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OseroAI{
    class AISystem{
        //盤面を保存する変数。0は何もおいてない状態。1が白で2が黒
        public int[,] bord_data = new int[8,8];

        //自分の石の色
        int mycolor = 0;

        private bool learnMode = false;

        //自分の石の情報をもらい取得する。
        public AISystem(int color){
            mycolor = color;
        }

        //盤面情報をセットする
        public void SetBord(int[,] bord_data){
            this.bord_data = bord_data;
        }

        //自分の色を返す
        public int getMyColor(){
            return mycolor;
        }

        //盤面のデータからランダムにおける位置に置く
        public int[] non_consider_put_stone(){

            //置ける場所を取得する。
            List<int[]> choice_list = can_put_list();

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
            List<int[]> choiceList = can_put_list();

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


        //現在の盤面状況を保存するファイルを制作する関数。
        public void learnBord(){
            string path = @"./learnData/";
            //現在の盤面データはファイル名にする。
            path += now_bord_to_learnFileName();
            if (!File.Exists(path)){
                //ファイルが存在していない場合は盤面データを保存する用のファイルを作成。
                File.CreateText(path);
            }
        }

        //学習するかどうかの関数。
        public void learnON(){
            learnMode = true;
            Directory.CreateDirectory("/learnData");
        }

        public void learnOFF(){
            learnMode = false;
        }

        //石が置ける位置をList<int[]>で返す関数
        private List<int[]> can_put_list() {
            // 置ける位置を格納するリスト
            List<int[]> put_list = new List<int[]>();

            // 盤面を走査して、指定された色の石が置ける場所を探す
            for (int x = 0; x < 8; x++) {
                for (int y = 0; y < 8; y++) {
                    // 既に石が置かれている場合はスキップ
                    if (bord_data[x, y] != 0) {
                        continue;
                    }

                    // 置けるかどうかを判定するフラグ
                    bool can_place = false;

                    // 左右、上下、斜めの8方向について石を裏返す処理を行う
                    for (int i = -1; i <= 1; i++) {
                        for (int j = -1; j <= 1; j++) {
                            // 自分自身の場合はスキップ
                            if (i == 0 && j == 0) {
                                continue;
                            }
                            // 現在の位置から指定方向に進んでいく
                            int cur_x = x + i;
                            int cur_y = y + j;

                            // 次の位置が盤面内かつ相手の石である限り石を探し続ける
                            while (cur_x >= 0 && cur_x < 8 && cur_y >= 0 && cur_y < 8 && bord_data[cur_x, cur_y] != 0 && bord_data[cur_x, cur_y] != mycolor) {
                                // 次の位置に進む
                                cur_x += i;
                                cur_y += j;
                            }

                            // 最後の位置が自分の石である場合、石を置ける場所とする
                            if (cur_x >= 0 && cur_x < 8 && cur_y >= 0 && cur_y < 8 && bord_data[cur_x, cur_y] == mycolor) {
                                can_place = true;
                            }
                        }
                    }

                    // 石を置ける場合、座標をリストに追加
                    if (can_place) {
                        put_list.Add(new int[] {x, y});
                    }
                }
            }

            // 置ける場所を返す
            return put_list;
        }

        //ひっくり返せる石の数を数える。
        private int CountFlipStones(int x, int y, int color){
            //座標とかの入力値のチェック
            if (x < 0 || x >= 8 || y < 0 || y >= 8 || (color != 1 && color != 2)){
                return -1;
            }

            //石がすでに配置されていないかをチェックする。
            if (bord_data[x,y] != 0){
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
                    while (nx >= 0 && nx < 8 && ny >= 0 && ny < 8 && bord_data[nx,ny] != 0 && bord_data[nx,ny] != color){
                        flipCount++;
                        nx += dx;
                        ny += dy;
                    }

                    //カウントがある場合は代入する。
                    if (nx >= 0 && nx < 8 && ny >= 0 && ny < 8 && bord_data[nx,ny] == color && flipCount > 0){
                        count += flipCount;
                    }
                }
            }
            return count;
        }

        private String now_bord_to_learnFileName(){
            String name = "";
            //現在の盤面データをファイル名用に変換する。
            for(int x = 0;x<8;x++){
                for(int y = 0;y<8;y ++){
                    name += Convert.ToString(bord_data[x,y]);
                }
            }
            return name;
        }
    }
}