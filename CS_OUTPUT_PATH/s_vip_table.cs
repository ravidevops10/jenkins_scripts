using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_vip_table
{
	private s_vip[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_vip_table sInstance = null;
	public static s_vip_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_vip_table();
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

		entities = new s_vip[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_vip();
			s_vip entity = entities[i];


			entity.vip_level= int.Parse(vals[0]);
			entity.recharge= int.Parse(vals[1]);
			entity.unlock_luckycat= int.Parse(vals[2]);
			entity.luckycat_odds= int.Parse(vals[3]);
			entity.unlock_friends_physical= int.Parse(vals[4]);
			entity.unlock_arena_battle_jump= int.Parse(vals[5]);
			entity.unlock_arena_buy_num= int.Parse(vals[6]);
			entity.unlock_pvp_buy_num= int.Parse(vals[7]);
			entity.unlock_resource_jump= int.Parse(vals[8]);
			entity.unlock_physical= int.Parse(vals[9]);
			entity.unlock_tower= int.Parse(vals[10]);
			entity.unlock_simulation_train= int.Parse(vals[11]);
			entity.unlock_tower_rush= int.Parse(vals[12]);
			entity.unlock_normal_level= int.Parse(vals[13]);
			entity.unlock_elite_level= int.Parse(vals[14]);
			entity.endless_revive= int.Parse(vals[15]);
			entity.shop= int.Parse(vals[16]);
			entity.shop_arena= int.Parse(vals[17]);
			entity.shop_pvp= int.Parse(vals[18]);
			entity.shop_boss= int.Parse(vals[19]);
			entity.shop_tower= int.Parse(vals[20]);
			entity.shop_train= int.Parse(vals[21]);
			entity.award_vip = vals[22];
			keyIdxMap[ entity.vip_level ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_vip.FileName;
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
	public s_vip GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_vip);
		}
	}

	public s_vip GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_vip);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
