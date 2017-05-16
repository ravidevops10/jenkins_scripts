using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_boss_award_table
{
	private s_boss_award[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_boss_award_table sInstance = null;
	public static s_boss_award_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_boss_award_table();
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

		entities = new s_boss_award[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_boss_award();
			s_boss_award entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.rank_front= int.Parse(vals[1]);
			entity.rank_back= int.Parse(vals[2]);
			entity.reward_kill = vals[3];
			entity.reward_notkill = vals[4];
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_boss_award.FileName;
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
	public s_boss_award GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_boss_award);
		}
	}

	public s_boss_award GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_boss_award);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
