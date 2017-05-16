using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_pvp_wave_table
{
	private s_pvp_wave[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_pvp_wave_table sInstance = null;
	public static s_pvp_wave_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_pvp_wave_table();
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

		entities = new s_pvp_wave[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_pvp_wave();
			s_pvp_wave entity = entities[i];


			entity.wave_id= int.Parse(vals[0]);
			entity.create_ratio= int.Parse(vals[1]);
			entity.monster_max= int.Parse(vals[2]);
			keyIdxMap[ entity.wave_id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_pvp_wave.FileName;
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
	public s_pvp_wave GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_pvp_wave);
		}
	}

	public s_pvp_wave GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_pvp_wave);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
