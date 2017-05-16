using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_resfb_item_table
{
	private s_resfb_item[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_resfb_item_table sInstance = null;
	public static s_resfb_item_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_resfb_item_table();
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

		entities = new s_resfb_item[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_resfb_item();
			s_resfb_item entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.resource_id= int.Parse(vals[1]);
			entity.difficulty= int.Parse(vals[2]);
			entity.level= int.Parse(vals[3]);
			entity.time_limit= int.Parse(vals[4]);
			entity.battleforce= int.Parse(vals[5]);
			entity.prize= int.Parse(vals[6]);
			entity.reward = vals[7];
			entity.map_id= int.Parse(vals[8]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_resfb_item.FileName;
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
	public s_resfb_item GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_resfb_item);
		}
	}

	public s_resfb_item GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_resfb_item);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
