using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_gm_activity
{
	/// 活动id
	public int id;
	/// 强制开关
	public int switches;
	/// 图标
	public string icon;
	/// 排序id
	public int order_id;
	/// 活动名称
	public string name;
	/// 活动描述
	public string des;
	/// 活动banner
	public string banner;
	/// 生效时间类型
	public int timer_type;
	/// 生效阶段开始时间参数
	public string enable_time;
	/// 生效阶段持续时长
	public int enable_last_time;
	/// 活动开放的开始时间参数
	public string open_time;
	/// 活动持续时长
	public int open_last_time;
	/// 调用活动类型
	public string template_type;

	public static string FileName = "s_gm_activity";
}
}