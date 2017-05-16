using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_employe_quality
{
	/// 阶级id
	public int id;
	/// 角色ID
	public int id_role;
	/// 阶级等级
	public int quality;
	/// 最大角色等级
	public int level_max;
	/// 角色等级需求
	public int precondition_level;
	/// 金币消耗
	public int cost_gold;
	/// 进阶消耗材料
	public string material;
	/// 升品结果id
	public int output;
	/// 战斗属性ID
	public int battle_data;
	/// 技能
	public string skill;

	public static string FileName = "s_employe_quality";
}
}