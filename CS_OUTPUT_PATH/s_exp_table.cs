using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_exp_table
{
	private s_exp[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_exp_table sInstance = null;
	public static s_exp_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_exp_table();
				sInstance.Load();
				}

			return sInstance; 
		}
	}

        	void Load()
	{
		Action<string> onTableLoad = (text) => {
		string[] lines = text.Split("\n\r".ToCharArray(),             StringSplitOptions.RemoveEmptyEntries);
		int count = lines.Length;
		if (count <= 0) {
        			return;
		}

		entities = new s_exp[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_exp();
			s_exp entity = entities[i];


			entity.level= int.Parse(vals[0]);
			entity.team_exp= int.Parse(vals[1]);
			entity.team_exp_total= int.Parse(vals[2]);
			entity.role_exp= int.Parse(vals[3]);
			entity.role_exp_total= int.Parse(vals[4]);
			entity.intimate_exp= int.Parse(vals[5]);
			entity.intimate_exp_total= int.Parse(vals[6]);
			entity.equip_intensify_gold= int.Parse(vals[7]);
			entity.equip_intensify_gold_total= int.Parse(vals[8]);
			entity.train_team_exp= int.Parse(vals[9]);
			entity.train_exp= int.Parse(vals[10]);
			entity.train_intimate_exp= int.Parse(vals[11]);
			entity.max_physical= int.Parse(vals[12]);
			entity.award_physical= int.Parse(vals[13]);
			entity.battle_data= int.Parse(vals[14]);
			entity.pvp_reward_win = vals[15];
			entity.pvp_reward_lose = vals[16];
			keyIdxMap[ entity.level ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_exp.FileName;
    TextAsset textAsset;
    if (AssemblySetting.LoadProtocInResource) {
        textAsset = Resources.Load<TextAsset>("Table/" + fileName);
    } else {
        textAsset = RuntimeResourceManager.Instance.LoadCachedAsset<TextAsset>(fileName); 
    }
    if (textAsset == null) {
        return;
    }
    onTableLoad(textAsset.text);}
	public s_exp GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_exp);
		}
	}

	public s_exp GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_exp);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
