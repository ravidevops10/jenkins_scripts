using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_boss
{
	/// id
	public int id;
	/// 下个id
	public int next_id;
	/// BOSS名称
	public string name;
	/// 美术资源
	public string resource;
	/// 持续天数
	public int continued_day;
	/// 场景编号
	public string map_id;
	/// 时间限制
	public int time_limit;
	/// 基本挑战次数
	public int challenge_count;
	/// 挑战cd
	public int challenge_cd;

	public static string FileName = "s_boss";
}
}