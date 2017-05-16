using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_intimacy_event_table
{
	private s_intimacy_event[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_intimacy_event_table sInstance = null;
	public static s_intimacy_event_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_intimacy_event_table();
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

		entities = new s_intimacy_event[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_intimacy_event();
			s_intimacy_event entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.level_back= int.Parse(vals[1]);
			entity.level_front= int.Parse(vals[2]);
			entity.talk= int.Parse(vals[3]);
			entity.weight= int.Parse(vals[4]);
			entity.drop_id= int.Parse(vals[5]);
			entity.employe_id= int.Parse(vals[6]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_intimacy_event.FileName;
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
	public s_intimacy_event GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_intimacy_event);
		}
	}

	public s_intimacy_event GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_intimacy_event);
		}
		else
		{
			return entities[idx];
		}
	}

}
}