using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_task_main_table
{
	private s_task_main[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_task_main_table sInstance = null;
	public static s_task_main_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_task_main_table();
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

		entities = new s_task_main[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_task_main();
			s_task_main entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.next= int.Parse(vals[1]);
			entity.conditions_type= int.Parse(vals[2]);
			entity.conditions_data = vals[3];
			entity.award = vals[4];
			entity.icon = vals[5];
			entity.name = vals[6];
			entity.dec_task = vals[7];
			entity.link = vals[8];
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_task_main.FileName;
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
	public s_task_main GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_task_main);
		}
	}

	public s_task_main GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_task_main);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
