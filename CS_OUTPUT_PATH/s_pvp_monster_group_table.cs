using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_pvp_monster_group_table
{
	private s_pvp_monster_group[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_pvp_monster_group_table sInstance = null;
	public static s_pvp_monster_group_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_pvp_monster_group_table();
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

		entities = new s_pvp_monster_group[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_pvp_monster_group();
			s_pvp_monster_group entity = entities[i];


			entity.group_id= int.Parse(vals[0]);
			entity.group_name = vals[1];
			entity.monster_id_1= int.Parse(vals[2]);
			entity.monster_data_1 = vals[3];
			entity.create_value_1= int.Parse(vals[4]);
			entity.monster_name_1 = (Table.5) ( int.Parse(vals[5]) );
			entity.monster_id_2= int.Parse(vals[6]);
			entity.monster_data_2 = vals[7];
			entity.create_value_2= int.Parse(vals[8]);
			entity.monster_name_2 = (Table.9) ( int.Parse(vals[9]) );
			entity.monster_id_3= int.Parse(vals[10]);
			entity.monster_data_3 = vals[11];
			entity.create_value_3= int.Parse(vals[12]);
			entity.boss_id_1= int.Parse(vals[13]);
			entity.boss_data_1 = vals[14];
			entity.boss_create_value_1= int.Parse(vals[15]);
			entity.boss_name_1 = (Table.16) ( int.Parse(vals[16]) );
			entity.boss_id_2= int.Parse(vals[17]);
			entity.boss_data_2 = vals[18];
			entity.boss_create_value_2= int.Parse(vals[19]);
			entity.boss_name_2 = (Table.20) ( int.Parse(vals[20]) );
			entity.boss_id_3= int.Parse(vals[21]);
			entity.boss_data_3 = vals[22];
			entity.boss_create_value_3= int.Parse(vals[23]);
			entity.boss_name_3 = (Table.24) ( int.Parse(vals[24]) );
			keyIdxMap[ entity.group_id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_pvp_monster_group.FileName;
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
	public s_pvp_monster_group GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_pvp_monster_group);
		}
	}

	public s_pvp_monster_group GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_pvp_monster_group);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
