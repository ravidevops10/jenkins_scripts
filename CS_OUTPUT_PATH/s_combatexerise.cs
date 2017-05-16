using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_combatexerise
{
	/// id
	public int id;
	/// 下一层id
	public int next_id;
	/// 类型
	public int type;
	/// 图标
	public string icon;
	/// 时间限制
	public int time_limit;
	/// 场景编号
	public string map_id;
	/// 通关固定奖励
	public string reward;
	/// 掉落id
	public int prize;
	/// 匹配战力范围
	public string search;
	/// 跑马灯标志
	public int notice;

	public static string FileName = "s_combatexerise";
}
}