using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_task_daily
{
	/// 每日id
	public int id;
	/// 前置等级
	public int level;
	/// 完成条件类型
	public int conditions_type;
	/// 完成条件参数
	public string conditions_data;
	/// 常态奖励
	public string award;
	/// 活跃度奖励
	public int award_daily;
	/// 图标
	public string icon;
	/// 每日任务名称
	public string name;
	/// 任务描述
	public string dec_task;
	/// 跳转链接
	public string link;

	public static string FileName = "s_task_daily";
}
}