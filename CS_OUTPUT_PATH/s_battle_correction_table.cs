using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_battle_correction_table
{
	private s_battle_correction[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_battle_correction_table sInstance = null;
	public static s_battle_correction_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_battle_correction_table();
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

		entities = new s_battle_correction[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_battle_correction();
			s_battle_correction entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.hurt_pve = float.Parse(vals[1]);
			entity.hurt_evp = float.Parse(vals[2]);
			entity.hurt_pvp = float.Parse(vals[3]);
			entity.hurt_pve_negative = float.Parse(vals[4]);
			entity.hurt_evp_negative = float.Parse(vals[5]);
			entity.hurt_pvp_negative = float.Parse(vals[6]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_battle_correction.FileName;
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
	public s_battle_correction GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_battle_correction);
		}
	}

	public s_battle_correction GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_battle_correction);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
