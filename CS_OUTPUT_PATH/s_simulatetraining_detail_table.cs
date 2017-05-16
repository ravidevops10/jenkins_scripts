using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_simulatetraining_detail_table
{
	private s_simulatetraining_detail[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_simulatetraining_detail_table sInstance = null;
	public static s_simulatetraining_detail_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_simulatetraining_detail_table();
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

		entities = new s_simulatetraining_detail[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_simulatetraining_detail();
			s_simulatetraining_detail entity = entities[i];


			entity.endless_id= int.Parse(vals[0]);
			entity.level_1= int.Parse(vals[1]);
			entity.reward_1 = vals[2];
			entity.level_2= int.Parse(vals[3]);
			entity.reward_2 = vals[4];
			entity.level_3= int.Parse(vals[5]);
			entity.reward_3 = vals[6];
			entity.level_4= int.Parse(vals[7]);
			entity.reward_4 = vals[8];
			entity.level_5= int.Parse(vals[9]);
			entity.reward_5 = vals[10];
			keyIdxMap[ entity.endless_id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_simulatetraining_detail.FileName;
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
	public s_simulatetraining_detail GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_simulatetraining_detail);
		}
	}

	public s_simulatetraining_detail GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_simulatetraining_detail);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
