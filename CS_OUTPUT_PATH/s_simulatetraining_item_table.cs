using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_simulatetraining_item_table
{
	private s_simulatetraining_item[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_simulatetraining_item_table sInstance = null;
	public static s_simulatetraining_item_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_simulatetraining_item_table();
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

		entities = new s_simulatetraining_item[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_simulatetraining_item();
			s_simulatetraining_item entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.type= int.Parse(vals[1]);
			entity.level= int.Parse(vals[2]);
			entity.icon = vals[3];
			entity.name = vals[4];
			entity.des = vals[5];
			entity.sell= int.Parse(vals[6]);
			entity.price = vals[7];
			entity.function= int.Parse(vals[8]);
			entity.function_data = vals[9];
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_simulatetraining_item.FileName;
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
	public s_simulatetraining_item GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_simulatetraining_item);
		}
	}

	public s_simulatetraining_item GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_simulatetraining_item);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
