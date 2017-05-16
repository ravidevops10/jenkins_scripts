using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_vip
{
	/// VIP等级
	public int vip_level;
	/// 累计VIP经验
	public int recharge;
	/// 招财猫次数
	public int unlock_luckycat;
	/// 招财猫几率
	public int luckycat_odds;
	/// 好友领取体力
	public int unlock_friends_physical;
	/// 竞技场-跳过战斗功能
	public int unlock_arena_battle_jump;
	/// 竞技场-购买挑战次数
	public int unlock_arena_buy_num;
	/// PVP2.0-购买挑战次数
	public int unlock_pvp_buy_num;
	/// 资源副本-免费扫荡
	public int unlock_resource_jump;
	/// 购买体力次数
	public int unlock_physical;
	/// 重置千层塔次数
	public int unlock_tower;
	/// 重置模拟训练次数
	public int unlock_simulation_train;
	/// 解锁千层塔扫荡
	public int unlock_tower_rush;
	/// 普通本次数
	public int unlock_normal_level;
	/// 精英本次数
	public int unlock_elite_level;
	/// 无尽模式复活次数
	public int endless_revive;
	/// 普通商店刷新次数
	public int shop;
	/// 竞技场刷新次数
	public int shop_arena;
	/// 荣誉战场刷新次数
	public int shop_pvp;
	/// 世界BOSS商店刷新次数
	public int shop_boss;
	/// 爬塔商店刷新次数
	public int shop_tower;
	/// 模拟训练商店刷新次数
	public int shop_train;
	/// 等级奖励
	public string award_vip;

	public static string FileName = "s_vip";
}
}