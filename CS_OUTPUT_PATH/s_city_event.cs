using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_city_event
{
	/// 物件id
	public int id;
	/// 触发次数
	public int trigger_time;
	/// 触发几率
	public int trigger_odds;
	/// 重置时间
	public int reset_time;
	/// 恢复时间
	public int recover_time;

	public static string FileName = "s_city_event";
}
}