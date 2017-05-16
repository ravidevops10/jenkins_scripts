using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_employe_star
{
	/// 星级id
	public int id;
	/// 角色ID
	public int role_id;
	/// 星级
	public int star;
	/// 金钱消耗
	public int cost_gold;
	/// 需求道具
	public string cost_item;
	/// 升星结果id
	public int output;
	/// 战斗属性ID
	public int battle_data;
	/// 是否跑马灯
	public int notice;

	public static string FileName = "s_employe_star";
}
}