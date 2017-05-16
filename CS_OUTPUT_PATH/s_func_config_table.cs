using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_func_config_table
{
	private s_func_config[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_func_config_table sInstance = null;
	public static s_func_config_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_func_config_table();
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

		entities = new s_func_config[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_func_config();
			s_func_config entity = entities[i];


			entity.lv= int.Parse(vals[0]);
			entity.func_list = vals[1];
			entity.show_list = vals[2];
			entity.func_show = vals[3];
			keyIdxMap[ entity.lv ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_func_config.FileName;
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
	public s_func_config GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_func_config);
		}
	}

	public s_func_config GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_func_config);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
