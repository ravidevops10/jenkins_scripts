using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_item_table
{
	private s_item[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_item_table sInstance = null;
	public static s_item_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_item_table();
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

		entities = new s_item[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_item();
			s_item entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.name= int.Parse(vals[1]);
			entity.type= int.Parse(vals[2]);
			entity.sub_type= int.Parse(vals[3]);
			entity.color= int.Parse(vals[4]);
			entity.stack= int.Parse(vals[5]);
			entity.own= int.Parse(vals[6]);
			entity.use_level= int.Parse(vals[7]);
			entity.sell= int.Parse(vals[8]);
			entity.sell_price = vals[9];
			entity.use_type= int.Parse(vals[10]);
			entity.use_type_data = vals[11];
			entity.result_type_data= int.Parse(vals[12]);
			entity.use_jump= int.Parse(vals[13]);
			entity.use_jump_data= int.Parse(vals[14]);
			entity.icon = vals[15];
			entity.link = vals[16];
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_item.FileName;
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
	public s_item GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_item);
		}
	}

	public s_item GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_item);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
