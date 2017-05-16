using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_employe_data_table
{
	private s_employe_data[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_employe_data_table sInstance = null;
	public static s_employe_data_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_employe_data_table();
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

		entities = new s_employe_data[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_employe_data();
			s_employe_data entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.grade= int.Parse(vals[1]);
			entity.old= int.Parse(vals[2]);
			entity.zodiac= int.Parse(vals[3]);
			entity.blood_type= int.Parse(vals[4]);
			entity.height = float.Parse(vals[5]);
			entity.weight = float.Parse(vals[6]);
			entity.chest = float.Parse(vals[7]);
			entity.cup= int.Parse(vals[8]);
			entity.waist = float.Parse(vals[9]);
			entity.hips = float.Parse(vals[10]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_employe_data.FileName;
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
	public s_employe_data GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_employe_data);
		}
	}

	public s_employe_data GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_employe_data);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
