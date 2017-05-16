using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_pvp_constant_table
{
	private s_pvp_constant[]	entities;

	private Dictionary<string, int>	        keyIdxMap = new Dictionary<string, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_pvp_constant_table sInstance = null;
	public static s_pvp_constant_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_pvp_constant_table();
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

		entities = new s_pvp_constant[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_pvp_constant();
			s_pvp_constant entity = entities[i];


			entity.name = vals[0];
			entity.data_type = vals[1];
			entity.value = vals[2];
			keyIdxMap[ entity.name ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_pvp_constant.FileName;
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
	public s_pvp_constant GetEntityByKey(string key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_pvp_constant);
		}
	}

	public s_pvp_constant GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_pvp_constant);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
