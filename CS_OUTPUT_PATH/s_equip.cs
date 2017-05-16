using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_equip
{
	/// id
	public int id;
	/// 装备名字
	public string name;
	/// 装备类型
	public int equip_type;
	/// 武器类型
	public int weapon_type;
	/// 职业限制
	public int job;
	/// 品质
	public int color;
	/// 进阶上限
	public int advanced_limit;
	/// 伤害类型
	public string hurt_type;
	/// 装备唯一
	public int only;
	/// 动画
	public string animation;
	/// 属性显示
	public string data_view;
	/// 可否出售
	public int sell;
	/// 是否跑马灯
	public int notice;

	public static string FileName = "s_equip";
}
}