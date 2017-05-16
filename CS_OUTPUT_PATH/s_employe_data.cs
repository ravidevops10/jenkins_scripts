using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_employe_data
{
	/// 角色ID
	public int id;
	/// 学年
	public int grade;
	/// 年龄
	public int old;
	/// 星座
	public int zodiac;
	/// 血型
	public int blood_type;
	/// 身高
	public float height;
	/// 体重
	public float weight;
	/// 胸围
	public float chest;
	/// 罩杯
	public int cup;
	/// 腰围
	public float waist;
	/// 臀围
	public float hips;

	public static string FileName = "s_employe_data";
}
}