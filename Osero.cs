using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Osero{

    class Bord{

        //盤面を保存する変数。0は何もおいてない状態。1が白で2が黒
        public int[,] bord = new int[8,8];

        //盤面のリセットを行う関数
        public void Init_Bord(){
            for(int x = 0;x<8;x++){
                for(int y = 0;y<8;y ++){
                    bord[x,y] = 0;
                }
            }

            //最初の4マス分の配置
            bord[3,3] = 1;
            bord[3,4] = 2;
            bord[4,3] = 2;
            bord[4,4] = 1;
        }

        public String View_bord(){
            //返す用の変数
            String ret = "";

            //変数retに追加していく
            for(int y = 0;y<8;y++){
                for(int x = 0;x<8;x++){
                    ret+=bord[x,y];
                }
                //改行を加える。
                ret+="\n";
            }

            return ret;
        }

        // 指定されたx、yの位置に指定された色の石を置けるかどうかを判定し、石を裏返す処理を行う関数
        public bool put_stone(int x, int y, int color) {
            // 既に石が置かれている場合は置けない。また色が指定されている範囲内かを調べる
            if (bord[x, y] != 0&&color > 0&&color<3) {
                return false;
            }

            // 置ける場所があるかどうかを判定するフラグ
            bool can_place = false;

            // 左右、上下、斜めの8方向について石を裏返す処理を行う
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    // 自分自身の場合はスキップ
                    if (i == 0 && j == 0) {
                        continue;
                    }

                    // 裏返す石を格納するリスト
                    List<int[]> flip_list = new List<int[]>();

                    // 現在の位置から指定方向に進んでいく
                    int cur_x = x + i;
                    int cur_y = y + j;

                    // 次の位置が盤面内かつ相手の石である限り石を探し続ける
                    while (cur_x >= 0 && cur_x < 8 && cur_y >= 0 && cur_y < 8 && bord[cur_x, cur_y] != 0 && bord[cur_x, cur_y] != color) {
                        // 裏返す石をリストに追加
                        flip_list.Add(new int[] {cur_x, cur_y});

                        // 次の位置に進む
                        cur_x += i;
                        cur_y += j;
                    }

                    // 最後の位置が自分の石である場合、石を裏返す
                    if (cur_x >= 0 && cur_x < 8 && cur_y >= 0 && cur_y < 8 && bord[cur_x, cur_y] == color) {
                        can_place = true;
                        flip_list.Add(new int[]{x,y});
                        // 裏返す石を裏返す
                        foreach (int[] flip_pos in flip_list) {
                            bord[flip_pos[0], flip_pos[1]] = color;
                        }
                    }
                }
            }
            // 置ける場所があるかどうかを返す
            return can_place;
        }

        // 指定された色の番号に対して、その色の石が置ける位置をList<int[]>で返す関数
        public List<int[]> can_put_list(int color) {
            // 置ける位置を格納するリスト
            List<int[]> put_list = new List<int[]>();

            // 盤面を走査して、指定された色の石が置ける場所を探す
            for (int x = 0; x < 8; x++) {
                for (int y = 0; y < 8; y++) {
                    // 既に石が置かれている場合はスキップ
                    if (bord[x, y] != 0) {
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
                            while (cur_x >= 0 && cur_x < 8 && cur_y >= 0 && cur_y < 8 && bord[cur_x, cur_y] != 0 && bord[cur_x, cur_y] != color) {
                                // 次の位置に進む
                                cur_x += i;
                                cur_y += j;
                            }

                            // 最後の位置が自分の石である場合、石を置ける場所とする
                            if (cur_x >= 0 && cur_x < 8 && cur_y >= 0 && cur_y < 8 && bord[cur_x, cur_y] == color) {
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

        //どちらかが勝利しているかを判定する。
        public int judge_winner(){
            // 判定用の石の数を初期化
            int whiteCount = 0;
            int blackCount = 0;

            //石の数を比較する。
            for (int i = 0; i < 8; i++){
                for (int j = 0; j < 8; j++){
                    if (bord[i, j] == 1) whiteCount++;
                    else if (bord[i, j] == 2) blackCount++;
                }
            }

            // 盤面上における白または黒の石が置ける場所があるかどうかをチェック
            bool whiteCanPut = can_put_list(1).Count > 0;
            bool blackCanPut = can_put_list(2).Count > 0;

            //プレイヤーの石が消失してる場合の判定チェック
            if (whiteCount == 0) return 2; // 白の石がすべて消失した場合、黒の勝ち
            else if (blackCount == 0) return 1; // 黒の石がすべて消失した場合、白の勝ち

            // どちらのプレイヤーも石を置けない場合、石の数を比較して勝敗を決定
            if (!whiteCanPut && !blackCanPut){
                if (whiteCount > blackCount) return 1; // 白の勝ち
                else if (whiteCount < blackCount) return 2; // 黒の勝ち
                else return 0; // 引き分け
            }

            // どちらかのプレイヤーが石を置ける場合、まだ勝敗が決まっていないことを示す
            return -1;
        }
    }

    class AISys{
        //AIの脳内に存在する盤面情報
        Bord bord = new Bord();

        //自分の石の色
        int mycolor = 0;


        //自分の石の情報をもらい取得する。
        public AISys(int color){
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