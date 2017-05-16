using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_pvp_rank_config_table
{
	private s_pvp_rank_config[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_pvp_rank_config_table sInstance = null;
	public static s_pvp_rank_config_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_pvp_rank_config_table();
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

		entities = new s_pvp_rank_config[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_pvp_rank_config();
			s_pvp_rank_config entity = entities[i];


			entity.rank_id= int.Parse(vals[0]);
			entity.rank_name = vals[1];
			entity.rank_cent_down= int.Parse(vals[2]);
			entity.rank_cent_up= int.Parse(vals[3]);
			entity.match_range= int.Parse(vals[4]);
			entity.fight_cent = vals[5];
			entity.win_reward = vals[6];
			entity.fail_reward = vals[7];
			entity.grade_up_reward = vals[8];
			entity.decrease_ratio = float.Parse(vals[9]);
			entity.rank_icon1 = vals[10];
			entity.rank_icon2 = vals[11];
			entity.match_entity = vals[12];
			entity.cent_exchange = double.Parse(vals[13]) != 0;
			entity.week_reward_rank = vals[14];
			entity.week_reward = vals[15];
			entity.notice= int.Parse(vals[16]);
			keyIdxMap[ entity.rank_id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_pvp_rank_config.FileName;
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
	public s_pvp_rank_config GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_pvp_rank_config);
		}
	}

	public s_pvp_rank_config GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_pvp_rank_config);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
