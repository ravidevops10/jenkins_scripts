using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_simulatetraining_item
{
	/// id
	public int id;
	/// 道具类型
	public int type;
	/// 道具等级
	public int level;
	/// 道具图标
	public string icon;
	/// 道具名字
	public string name;
	/// 道具描述
	public string des;
	/// 是否出售
	public int sell;
	/// 购买价格
	public string price;
	/// 道具功能
	public int function;
	/// 功能参数
	public string function_data;

	public static string FileName = "s_simulatetraining_item";
}
}