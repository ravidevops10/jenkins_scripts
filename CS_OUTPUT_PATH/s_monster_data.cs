using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_monster_data
{
	/// 怪物属性ID
	public int id;
	/// 等级
	public int level;
	/// 分数
	public int score;
	/// 战力
	public int fight;
	/// 攻击
	public int attack;
	/// 防御
	public int defense;
	/// 生命
	public string hp;
	/// 能量
	public int mp;
	/// 生命恢复
	public int hp_recovery;
	/// 能量恢复
	public int mp_recovery;
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
	/// 怪物防御类型
	public string job;
	/// 怪物攻击类型
	public string element_type;

	public static string FileName = "s_monster_data";
}
}