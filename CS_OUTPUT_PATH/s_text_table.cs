using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_text_table
{
	private s_text[]	entities;

	private Dictionary<string, int>	        keyIdxMap = new Dictionary<string, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_text_table sInstance = null;
	public static s_text_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_text_table();
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

		entities = new s_text[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_text();
			s_text entity = entities[i];


			entity.key_name = vals[0];
			entity.zh_CN = vals[1];
			keyIdxMap[ entity.key_name ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_text.FileName;
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
	public s_text GetEntityByKey(string key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_text);
		}
	}

	public s_text GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_text);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
