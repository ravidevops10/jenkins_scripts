using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_power_every_day
{
	/// id
	public int id;
	/// 活动id
	public int activity_id;
	/// 时间段
	public string time_phase;
	/// 补签消耗
	public int cost;
	/// 奖励
	public string reward;
	/// 名称
	public string name;
	/// 描述
	public string des;

	public static string FileName = "s_power_every_day";
}
}