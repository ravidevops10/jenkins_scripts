using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_global_table
{
	private s_global[]	entities;

	private Dictionary<string, int>	        keyIdxMap = new Dictionary<string, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_global_table sInstance = null;
	public static s_global_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_global_table();
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

		entities = new s_global[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_global();
			s_global entity = entities[i];


			entity.name = vals[0];
			entity.data_type = vals[1];
			entity.value = vals[2];
			entity.system_type= int.Parse(vals[3]);
			entity.send= int.Parse(vals[4]);
			keyIdxMap[ entity.name ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_global.FileName;
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
	public s_global GetEntityByKey(string key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_global);
		}
	}

	public s_global GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_global);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
