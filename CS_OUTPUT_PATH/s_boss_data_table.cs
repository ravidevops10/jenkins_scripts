using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_boss_data_table
{
	private s_boss_data[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_boss_data_table sInstance = null;
	public static s_boss_data_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_boss_data_table();
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

		entities = new s_boss_data[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_boss_data();
			s_boss_data entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.boss_id= int.Parse(vals[1]);
			entity.boss_level= int.Parse(vals[2]);
			entity.reward = vals[3];
			entity.monster_data= int.Parse(vals[4]);
			entity.levelup_time= int.Parse(vals[5]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_boss_data.FileName;
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
	public s_boss_data GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_boss_data);
		}
	}

	public s_boss_data GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_boss_data);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
