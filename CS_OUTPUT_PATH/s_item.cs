using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_item
{
	/// 道具id
	public int id;
	/// 道具名
	public int name;
	/// 道具类型
	public int type;
	/// 道具子类型
	public int sub_type;
	/// 品质
	public int color;
	/// 可叠加数量
	public int stack;
	/// 可拥有数量
	public int own;
	/// 使用等级
	public int use_level;
	/// 可否出售
	public int sell;
	/// 卖出钻石价格
	public string sell_price;
	/// 使用类型
	public int use_type;
	/// 使用类型参数
	public string use_type_data;
	/// 使用效果显示类型
	public int result_type_data;
	/// 跳转类型
	public int use_jump;
	/// 跳转参数
	public int use_jump_data;
	/// 图标
	public string icon;
	/// 链接
	public string link;

	public static string FileName = "s_item";
}
}