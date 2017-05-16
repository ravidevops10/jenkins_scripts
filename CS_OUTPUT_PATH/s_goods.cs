using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_goods
{
	/// ID
	public int id;
	/// 商店id
	public int shop_id;
	/// 优先级
	public int priority;
	/// 解锁条件
	public string condition;
	/// 权重
	public int wight;
	/// 商品数据
	public string goods;
	/// 购买价格
	public string price;
	/// 限购次数
	public int restriction;

	public static string FileName = "s_goods";
}
}