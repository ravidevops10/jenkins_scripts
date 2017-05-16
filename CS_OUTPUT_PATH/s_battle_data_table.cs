using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_battle_data_table
{
	private s_battle_data[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_battle_data_table sInstance = null;
	public static s_battle_data_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_battle_data_table();
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

		entities = new s_battle_data[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_battle_data();
			s_battle_data entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.type= int.Parse(vals[1]);
			entity.attack = float.Parse(vals[2]);
			entity.defense = float.Parse(vals[3]);
			entity.hp = float.Parse(vals[4]);
			entity.mp= int.Parse(vals[5]);
			entity.damage= int.Parse(vals[6]);
			entity.critical= int.Parse(vals[7]);
			entity.anti_critical= int.Parse(vals[8]);
			entity.critical_damage_rate = float.Parse(vals[9]);
			entity.anti_critical_damage_rate = float.Parse(vals[10]);
			entity.hurt_add_rate = float.Parse(vals[11]);
			entity.anti_hurt_add_rate = float.Parse(vals[12]);
			entity.attack_percent_rate = float.Parse(vals[13]);
			entity.defense_percent_rate = float.Parse(vals[14]);
			entity.hp_percent_rate = float.Parse(vals[15]);
			entity.mp_percent_rate = float.Parse(vals[16]);
			entity.hp_recovery= int.Parse(vals[17]);
			entity.mp_recovery= int.Parse(vals[18]);
			entity.fight= int.Parse(vals[19]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_battle_data.FileName;
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
	public s_battle_data GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_battle_data);
		}
	}

	public s_battle_data GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_battle_data);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
