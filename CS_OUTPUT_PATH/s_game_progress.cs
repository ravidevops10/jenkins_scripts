using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_game_progress
{
	/// id
	public int id;
	/// id
	public int sub_id;
	/// 活动id
	public int activity_id;
	/// 系统类型
	public string type;
	/// 系统参数
	public string target;
	/// 奖励
	public string reward;

	public static string FileName = "s_game_progress";
}
}