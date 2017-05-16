using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_task_box
{
	/// 唯一id
	public int id;
	/// 等级段-前
	public int level_front;
	/// 等级段-后
	public int level_back;
	/// 活跃度值
	public string grade;
	/// 低档活跃度奖励
	public string award;

	public static string FileName = "s_task_box";
}
}