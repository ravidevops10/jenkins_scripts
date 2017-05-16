using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_lottery
{
	/// 奖池ID
	public int id;
	/// 奖池类型
	public int type;
	/// 是否激活
	public int is_active;
	/// 权重
	public int weight;
	/// 奖励内容
	public string prize_data;

	public static string FileName = "s_lottery";
}
}