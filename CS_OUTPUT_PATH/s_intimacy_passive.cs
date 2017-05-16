using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_intimacy_passive
{
	/// 被动技能id
	public int id;
	/// 归属角色
	public int role;
	/// 名字
	public string name;
	/// 说明
	public string des;
	/// 亲密度解锁等级
	public int level;
	/// 生命
	public int hp;
	/// 能量值
	public int mp;
	/// 攻击
	public int attack;
	/// 防御
	public int defense;
	/// 暴击
	public int critical;
	/// 暴击伤害
	public float critical_damage;
	/// 能量回复
	public int mp_recovery;

	public static string FileName = "s_intimacy_passive";
}
}