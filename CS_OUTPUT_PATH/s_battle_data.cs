using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_battle_data
{
	/// 属性ID
	public int id;
	/// 属性类型
	public int type;
	/// 攻击
	public float attack;
	/// 防御
	public float defense;
	/// 生命
	public float hp;
	/// 能量值
	public int mp;
	/// 附加伤害
	public int damage;
	/// 暴击
	public int critical;
	/// 抗暴
	public int anti_critical;
	/// 暴击伤害
	public float critical_damage_rate;
	/// 爆伤抵抗
	public float anti_critical_damage_rate;
	/// 伤害增加
	public float hurt_add_rate;
	/// 伤害抵抗
	public float anti_hurt_add_rate;
	/// 攻击百分比
	public float attack_percent_rate;
	/// 防御百分比
	public float defense_percent_rate;
	/// 生命百分比
	public float hp_percent_rate;
	/// 能量值百分比
	public float mp_percent_rate;
	/// 生命回复
	public int hp_recovery;
	/// 能量回复
	public int mp_recovery;
	/// 战斗力
	public int fight;

	public static string FileName = "s_battle_data";
}
}