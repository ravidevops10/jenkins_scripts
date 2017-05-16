using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_buyTimes
{
	/// 次数
	public int time;
	/// 招财猫花费
	public int buy_luckycat;
	/// 购买体力
	public int physical;
	/// 普通本次数
	public int normal_level;
	/// 精英本次数
	public int elite_level;
	/// 刷新普通商店
	public int reflash_shop_normal;
	/// 刷新限时商店
	public int reflash_shop_limit;
	/// 支援位置解锁
	public int support_count_open;
	/// 训练位置扩充
	public int train_num_buy;
	/// 继承的钻石消耗
	public int equip_inherit_cost;
	/// 竞技场购买挑战次数
	public int arena_buy_cost;
	/// PVP2.0购买次数
	public int pvp_buy_cost;
	/// 能力评估重置花费
	public int tower_buy_cost;
	/// 作战演习重置花费
	public int simulation_train_cost;
	/// 普通商店刷新消耗
	public string shop;
	/// 竞技场刷新消耗
	public string shop_arena;
	/// PVP2.0刷新消耗
	public string shop_pvp;
	/// BOSS商店刷新消耗
	public string shop_boss;
	/// 爬塔商店刷新次数
	public string shop_tower;
	/// 模拟训练商店刷新次数
	public string shop_train;
	/// 无尽模式复活花费
	public int endless_revive_cost;

	public static string FileName = "s_buyTimes";
}
}