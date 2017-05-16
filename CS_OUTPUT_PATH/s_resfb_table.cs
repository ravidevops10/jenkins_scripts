using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_resfb_table
{
	private s_resfb[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_resfb_table sInstance = null;
	public static s_resfb_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_resfb_table();
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

		entities = new s_resfb[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_resfb();
			s_resfb entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.type= int.Parse(vals[1]);
			entity.level= int.Parse(vals[2]);
			entity.enter_limit= int.Parse(vals[3]);
			entity.open_data = vals[4];
			entity.introduce = vals[5];
			entity.des = vals[6];
			entity.image_1 = vals[7];
			entity.image_2 = vals[8];
			entity.items = vals[9];
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_resfb.FileName;
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
	public s_resfb GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_resfb);
		}
	}

	public s_resfb GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_resfb);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
