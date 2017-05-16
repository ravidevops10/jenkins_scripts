using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_resfb_item
{
	/// 难度副本ID
	public int id;
	/// 类型
	public int resource_id;
	/// 难度
	public int difficulty;
	/// 开启等级
	public int level;
	/// 时间限制
	public int time_limit;
	/// 关卡战斗力
	public int battleforce;
	/// 难度掉落id
	public int prize;
	/// 奖励
	public string reward;
	/// 场景编号
	public int map_id;

	public static string FileName = "s_resfb_item";
}
}