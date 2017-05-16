using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_simulatetraining_config
{
	/// id
	public int id;
	/// 下一场id
	public int next_id;
	/// 关卡战斗力
	public int battleforce;
	/// 无尽模式编号
	public string endless_id;
	/// 掉落id
	public int prize;

	public static string FileName = "s_simulatetraining_config";
}
}