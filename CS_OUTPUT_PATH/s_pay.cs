using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_pay
{
	/// id
	public int id;
	/// 类型
	public int type;
	/// 充值金额
	public int rmb;
	/// 获得VIP经验
	public int vip_exp;
	/// 获得元宝数
	public int amount;
	/// 首冲是否翻倍
	public int double;
	/// 限购次数
	public int buy_times;
	/// 图标
	public string icon;
	/// 文本描述
	public string text;

	public static string FileName = "s_pay";
}
}