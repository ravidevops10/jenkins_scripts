using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_boss_award
{
	/// id
	public int id;
	/// 排名段-前
	public int rank_front;
	/// 排名段-后
	public int rank_back;
	/// 击杀奖励
	public string reward_kill;
	/// 未击杀奖励
	public string reward_notkill;

	public static string FileName = "s_boss_award";
}
}