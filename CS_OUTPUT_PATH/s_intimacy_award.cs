using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_intimacy_award
{
	/// 奖励ID
	public int id;
	/// 归属角色
	public int role;
	/// 奖励内容
	public string award;
	/// 亲密度解锁等级
	public int level;

	public static string FileName = "s_intimacy_award";
}
}