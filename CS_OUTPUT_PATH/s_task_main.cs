using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_task_main
{
	/// 任务id
	public int id;
	/// 后续任务ID
	public int next;
	/// 完成条件类型
	public int conditions_type;
	/// 完成条件参数
	public string conditions_data;
	/// 任务奖励
	public string award;
	/// 图标
	public string icon;
	/// 任务名称
	public string name;
	/// 任务描述
	public string dec_task;
	/// 跳转链接
	public string link;

	public static string FileName = "s_task_main";
}
}