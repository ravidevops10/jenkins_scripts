using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_intimacy_event
{
	/// 事件ID
	public int id;
	/// 亲密度等级起
	public int level_back;
	/// 亲密度等级止
	public int level_front;
	/// 对话包ID
	public int talk;
	/// 权重
	public int weight;
	/// 掉落列表
	public int drop_id;
	/// 角色ID
	public int employe_id;

	public static string FileName = "s_intimacy_event";
}
}