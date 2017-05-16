using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_pvp_rank_config
{
	/// 段位ID
	public int rank_id;
	/// 段位名
	public string rank_name;
	/// 段位最低积分
	public int rank_cent_down;
	/// 段位最高积分
	public int rank_cent_up;
	/// 初始匹配范围
	public int match_range;
	/// 战斗积分
	public string fight_cent;
	/// 战斗胜利奖励
	public string win_reward;
	/// 战斗失败奖励
	public string fail_reward;
	/// 段位提升奖励
	public string grade_up_reward;
	/// 衰减系数
	public float decrease_ratio;
	/// 段位icon1
	public string rank_icon1;
	/// 段位icon2
	public string rank_icon2;
	/// 匹配参照物
	public string match_entity;
	/// 积分兑换比
	public double cent_exchange;
	/// 段位排名
	public string week_reward_rank;
	/// 每周奖励
	public string week_reward;
	/// 是否跑马灯
	public int notice;

	public static string FileName = "s_pvp_rank_config";
}
}