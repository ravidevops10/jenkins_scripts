using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_simulatetraining_reward
{
	/// id
	public int id;
	/// 排名段-前
	public int rank_front;
	/// 排名段-后
	public int rank_back;
	/// 奖励
	public string reward;

	public static string FileName = "s_simulatetraining_reward";
}
}