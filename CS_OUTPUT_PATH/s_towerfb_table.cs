using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_towerfb_table
{
	private s_towerfb[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_towerfb_table sInstance = null;
	public static s_towerfb_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_towerfb_table();
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

		entities = new s_towerfb[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_towerfb();
			s_towerfb entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.next_id= int.Parse(vals[1]);
			entity.last_id= int.Parse(vals[2]);
			entity.index= int.Parse(vals[3]);
			entity.type= int.Parse(vals[4]);
			entity.battleforce= int.Parse(vals[5]);
			entity.time_limit= int.Parse(vals[6]);
			entity.reward = vals[7];
			entity.map_id= int.Parse(vals[8]);
			entity.reward_first = vals[9];
			entity.time_rush= int.Parse(vals[10]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_towerfb.FileName;
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
	public s_towerfb GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_towerfb);
		}
	}

	public s_towerfb GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_towerfb);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
