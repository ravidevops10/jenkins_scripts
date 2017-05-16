using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_exp
{
	/// 等级
	public int level;
	/// 团队当级经验
	public int team_exp;
	/// 团队累计经验
	public int team_exp_total;
	/// 角色当级经验
	public int role_exp;
	/// 角色经验
	public int role_exp_total;
	/// 角色亲密当级经验
	public int intimate_exp;
	/// 角色亲密累计经验
	public int intimate_exp_total;
	/// 装备强化金币消耗
	public int equip_intensify_gold;
	/// 装备强化总计金币消耗
	public int equip_intensify_gold_total;
	/// 训练团队经验
	public int train_team_exp;
	/// 训练角色经验
	public int train_exp;
	/// 训练角色亲密经验
	public int train_intimate_exp;
	/// 体力上限
	public int max_physical;
	/// 升级体力奖励
	public int award_physical;
	/// 战斗属性ID
	public int battle_data;
	/// PVP单局胜利奖励
	public string pvp_reward_win;
	/// PVP单局失败奖励
	public string pvp_reward_lose;

	public static string FileName = "s_exp";
}
}