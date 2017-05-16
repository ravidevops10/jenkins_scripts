using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_resfb
{
	/// id
	public int id;
	/// 类型
	public int type;
	/// 开启等级
	public int level;
	/// 挑战次数
	public int enter_limit;
	/// 开放日期
	public string open_data;
	/// 玩法介绍
	public string introduce;
	/// 玩法说明
	public string des;
	/// 类型展示图
	public string image_1;
	/// 内部展示图
	public string image_2;
	/// 包含资源关卡ID
	public string items;

	public static string FileName = "s_resfb";
}
}