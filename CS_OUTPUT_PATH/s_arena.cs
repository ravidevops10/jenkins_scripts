using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_arena
{
	/// id
	public int id;
	/// 排名段-前
	public int rank_front;
	/// 排名段-后
	public int rank_back;
	/// 每日奖励
	public string reward_daily;
	/// 冲刺奖励钻石
	public int reward_rush;
	/// 位置搜索
	public string search_front;
	/// 位置搜索
	public string search_back;

	public static string FileName = "s_arena";
}
}