using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_store
{
	/// 商店id
	public int id;
	/// 商店类型
	public int type;
	/// 货币类型
	public int money_type;
	/// 商店图标
	public string shop_icon;
	/// 商店商品数
	public int count;
	/// 代币获取途径
	public string token_way;
	/// 刷新时间点
	public string refresh_time;
	/// 是否-手动刷新
	public int refresh_operate;
	/// 是否-道具刷新
	public int refresh_item;

	public static string FileName = "s_store";
}
}