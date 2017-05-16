using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Table {
public class s_chapter
{
	/// 章节ID
	public int id;
	/// 类型
	public int type;
	/// 章节序号
	public int index;
	/// 开启等级
	public int level;
	/// 星级奖励条件
	public string star_reward_condition;
	/// 星级奖励内容
	public string star_reward;
	/// 章节名字
	public int name;
	/// 章节描述
	public int des;
	/// 是否跑马灯
	public int notice;

	public static string FileName = "s_chapter";
}
}