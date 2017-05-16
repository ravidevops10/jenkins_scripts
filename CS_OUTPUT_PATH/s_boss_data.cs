using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_boss_data
{
	/// id
	public int id;
	/// boss的id
	public int boss_id;
	/// BOSS等级
	public int boss_level;
	/// 挑战奖励
	public string reward;
	/// 怪物属性ID
	public int monster_data;
	/// 升级时间
	public int levelup_time;

	public static string FileName = "s_boss_data";
}
}