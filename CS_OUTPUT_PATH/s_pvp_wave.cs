using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_pvp_wave
{
	/// 怪物波次id
	public int wave_id;
	/// 击杀创建比率
	public int create_ratio;
	/// 怪物最大数量限制
	public int monster_max;

	public static string FileName = "s_pvp_wave";
}
}