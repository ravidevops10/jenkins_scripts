using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_rb_static
{
	/// id
	public int id;
	/// 名字
	public string nick_name;
	/// 团队等级
	public int level;
	/// vip经验
	public int vip_exp;
	/// 玩家头像
	public int icon_id;
	/// 角色信息
	public string employe;
	/// 角色1装备
	public string equip1;
	/// 角色2装备
	public string equip2;
	/// 角色3装备
	public string equip3;
	/// 指定竞技场排位
	public int ranking;
	/// 战力
	public int fight;

	public static string FileName = "s_rb_static";
}
}