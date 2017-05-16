using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_buyTimes_table
{
	private s_buyTimes[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_buyTimes_table sInstance = null;
	public static s_buyTimes_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_buyTimes_table();
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

		entities = new s_buyTimes[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_buyTimes();
			s_buyTimes entity = entities[i];


			entity.time= int.Parse(vals[0]);
			entity.buy_luckycat= int.Parse(vals[1]);
			entity.physical= int.Parse(vals[2]);
			entity.normal_level= int.Parse(vals[3]);
			entity.elite_level= int.Parse(vals[4]);
			entity.reflash_shop_normal= int.Parse(vals[5]);
			entity.reflash_shop_limit= int.Parse(vals[6]);
			entity.support_count_open= int.Parse(vals[7]);
			entity.train_num_buy= int.Parse(vals[8]);
			entity.equip_inherit_cost= int.Parse(vals[9]);
			entity.arena_buy_cost= int.Parse(vals[10]);
			entity.pvp_buy_cost= int.Parse(vals[11]);
			entity.tower_buy_cost= int.Parse(vals[12]);
			entity.simulation_train_cost= int.Parse(vals[13]);
			entity.shop = vals[14];
			entity.shop_arena = vals[15];
			entity.shop_pvp = vals[16];
			entity.shop_boss = vals[17];
			entity.shop_tower = vals[18];
			entity.shop_train = vals[19];
			entity.endless_revive_cost= int.Parse(vals[20]);
			keyIdxMap[ entity.time ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_buyTimes.FileName;
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
	public s_buyTimes GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_buyTimes);
		}
	}

	public s_buyTimes GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_buyTimes);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
