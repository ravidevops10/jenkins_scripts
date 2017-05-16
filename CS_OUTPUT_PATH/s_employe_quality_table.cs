using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_employe_quality_table
{
	private s_employe_quality[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_employe_quality_table sInstance = null;
	public static s_employe_quality_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_employe_quality_table();
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

		entities = new s_employe_quality[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_employe_quality();
			s_employe_quality entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.id_role= int.Parse(vals[1]);
			entity.quality= int.Parse(vals[2]);
			entity.level_max= int.Parse(vals[3]);
			entity.precondition_level= int.Parse(vals[4]);
			entity.cost_gold= int.Parse(vals[5]);
			entity.material = vals[6];
			entity.output= int.Parse(vals[7]);
			entity.battle_data= int.Parse(vals[8]);
			entity.skill = vals[9];
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_employe_quality.FileName;
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
	public s_employe_quality GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_employe_quality);
		}
	}

	public s_employe_quality GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_employe_quality);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
