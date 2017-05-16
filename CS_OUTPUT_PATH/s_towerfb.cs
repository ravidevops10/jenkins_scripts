using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_towerfb
{
	/// id
	public int id;
	/// 下一层id
	public int next_id;
	/// 上一层id
	public int last_id;
	/// 层数
	public int index;
	/// 关卡类型
	public int type;
	/// 关卡战斗力
	public int battleforce;
	/// 时间限制
	public int time_limit;
	/// 通关奖励
	public string reward;
	/// 场景编号
	public int map_id;
	/// 首通奖励
	public string reward_first;
	/// 扫荡时间
	public int time_rush;

	public static string FileName = "s_towerfb";
}
}