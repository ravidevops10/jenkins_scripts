using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_battle_correction
{
	/// 索引ID
	public int id;
	/// 人打怪伤害修正
	public float hurt_pve;
	/// 怪打人伤害修正
	public float hurt_evp;
	/// 人打人伤害修正
	public float hurt_pvp;
	/// 人打怪伤害修正
	public float hurt_pve_negative;
	/// 怪打人伤害修正
	public float hurt_evp_negative;
	/// 人打人伤害修正
	public float hurt_pvp_negative;

	public static string FileName = "s_battle_correction";
}
}