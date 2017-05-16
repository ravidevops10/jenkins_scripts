using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_passive_skill
{
	/// 技能id
	public int id;
	/// 条件计算公式
	public string formula;
	/// 属性ID
	public int battle_data;
	/// 占中是否使用
	public int pure_data;

	public static string FileName = "s_passive_skill";
}
}