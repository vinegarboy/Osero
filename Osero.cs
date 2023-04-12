
using System;
using System.Collections.Generic;

namespace Osero{

	class Bord{

		//盤面を保存する変数。0は何もおいてない状態。1が白で2が黒
		String[,] bord = new String[8,8];

		//盤面のリセットを行う関数
		public void Init_Bord(){
			for(int x = 0;x<8;x++){
				for(int y = 0;y<8;y ++){
					bord[x,y] = "0";
				}
			}

			//最初の4マス分の配置
			bord[3,3] = "1";
			bord[3,4] = "0";
			bord[4,3] = "0";
			bord[4,4] = "1";
		}

		// 指定されたx、yの位置に指定された色の石を置けるかどうかを判定し、石を裏返す処理を行う関数
		bool put_set(int x, int y, int color) {
			// 既に石が置かれている場合は置けない。また色が指定されている範囲内かを調べる
			if (bord[x, y] != "0"&&color > 0&&color<3) {
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
					while (cur_x >= 0 && cur_x < 8 && cur_y >= 0 && cur_y < 8 && bord[cur_x, cur_y] != "0" && bord[cur_x, cur_y] != color.ToString()) {
						// 裏返す石をリストに追加
						flip_list.Add(new int[] {cur_x, cur_y});

						// 次の位置に進む
						cur_x += i;
						cur_y += j;
					}

					// 最後の位置が自分の石である場合、石を裏返す
					if (cur_x >= 0 && cur_x < 8 && cur_y >= 0 && cur_y < 8 && bord[cur_x, cur_y] == color.ToString()) {
						can_place = true;

						// 裏返す石を裏返す
						foreach (int[] flip_pos in flip_list) {
							bord[flip_pos[0], flip_pos[1]] = color.ToString();
						}
					}
				}
			}
			// 置ける場所があるかどうかを返す
			return can_place;
		}
	}

	class AISys{
		
	}

}