using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_lottery_table
{
	private s_lottery[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_lottery_table sInstance = null;
	public static s_lottery_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_lottery_table();
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

		entities = new s_lottery[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_lottery();
			s_lottery entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.type= int.Parse(vals[1]);
			entity.is_active= int.Parse(vals[2]);
			entity.weight= int.Parse(vals[3]);
			entity.prize_data = vals[4];
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_lottery.FileName;
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
	public s_lottery GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_lottery);
		}
	}

	public s_lottery GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_lottery);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
