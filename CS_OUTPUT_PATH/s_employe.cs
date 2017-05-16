using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_employe
{
	/// 角色ID
	public int id;
	/// 角色名称
	public int name;
	/// 角色职业
	public int job;
	/// 初始等级
	public int level_beginning;
	/// 初始品质
	public int quality_beginning;
	/// 初始星级
	public int star_beginning;
	/// 角色元素属性
	public string element_type;
	/// 技能
	public string skill;
	/// 颜色
	public int color;
	/// 初始解锁服饰
	public string clothes_beginning;
	/// 初始装备
	public string equip_beginning;
	/// 初始情绪值
	public int beginning_emotion;
	/// 服饰
	public string clothes;
	/// 喜欢的道具
	public string like_item;
	/// 角色碎片
	public string role_piece;
	/// 角色合成
	public string role_compose;
	/// 服饰套装属性
	public string clothes_group;
	/// 移动速度
	public string move_speed;
	/// 初始武器ID
	public int weapon_id;
	/// 走马灯是否提示
	public int notice;

	public static string FileName = "s_employe";
}
}