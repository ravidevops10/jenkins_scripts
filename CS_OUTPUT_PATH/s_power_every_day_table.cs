using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_power_every_day_table
{
	private s_power_every_day[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_power_every_day_table sInstance = null;
	public static s_power_every_day_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_power_every_day_table();
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

		entities = new s_power_every_day[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_power_every_day();
			s_power_every_day entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.activity_id= int.Parse(vals[1]);
			entity.time_phase = vals[2];
			entity.cost= int.Parse(vals[3]);
			entity.reward = vals[4];
			entity.name = vals[5];
			entity.des = vals[6];
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_power_every_day.FileName;
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
	public s_power_every_day GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_power_every_day);
		}
	}

	public s_power_every_day GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_power_every_day);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
