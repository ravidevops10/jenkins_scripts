#-*- coding=utf8 -*-
"""
Created at 2015.12.11， modified at 2017.5.16
@author: albertcheng
This script is used to generate csharp table/class files from txt config files.
And it's also design to used in jenkins jobs.
"""

import os
import shutil

CS_OUTPUT_PATH = os.getenv("CS_OUTPUT_PATH")# or "./CS_OUTPUT_PATH"
TXT_INPUT_PATH = os.getenv("TEXT_CONFIG_PATH")# or "./TEXT_CONFIG_PATH"

def check_path(_path):
    """
    initialize path.
    """
    if os.path.exists(_path):
        shutil.rmtree(_path)
    os.makedirs(_path)


class ResTool(object):
    """
    restool class
    """
    def __init__(self):
        """
        class initialized
        """
        print "class initialized"
        check_path(CS_OUTPUT_PATH)
        for _file in os.listdir(TXT_INPUT_PATH):
            if not _file.endswith(".txt"):
                continue
            sheet_txt = os.path.join(TXT_INPUT_PATH, _file)
            print "handling with sheet file : ", sheet_txt
            sheet_data = self.read_txt(sheet_txt)
            #print "*" * 60
            #print repr(sheet_data[0])
            _type = [x.strip() for x in sheet_data[1].split("\t")]
            #print repr(_type)
            _name = [x.strip() for x in sheet_data[0].split("\t")]
            _comment = [x.strip() for x in sheet_data[2].split("\t")]

            self.generate_cs_from_data(CS_OUTPUT_PATH,
                                       os.path.basename(sheet_txt)[:-4],
                                       _comment,
                                       _name,
                                       _type)
            self.generate_cs_table_from_data(CS_OUTPUT_PATH,
                                             os.path.basename(sheet_txt)[:-4],
                                             _comment,
                                             _name,
                                             _type)
            self.generate_bin_from_data(CS_OUTPUT_PATH,
                                        os.path.basename(sheet_txt)[:-4],
                                        _comment,
                                        _name,
                                        _type,
                                        sheet_data[4:])

    def read_txt(self, _file):
        """
        read line data from text file.
        """
        with open(_file, "rb") as _txt:
            return _txt.readlines()

    def write_file(self, _file, data):
        """
        write lines data to a text file.
        """
        print "write to file - ", _file
        with open(_file, "wb") as _:
            _.writelines("\n".join([line.encode("utf8") for line in data]))

    def generate_cs_from_data(self, outpath, filename, _comment, _name, _type):
        """
        gen cs from config txt files
        """
        if _type is None and _name is None and _comment is None:
            return
        out_filename = os.path.join(outpath, filename + ".cs")
        out_stream = [u"using System;",
                      u"using System.Collections.Generic;",
                      u"using System.Text;",
                      u"using UnityEngine;\n",
                      u"namespace Table {",
                      u"public class %s\n{" % filename]

        for _type_idx, type_field in enumerate(_type):
            name_field = _name[_type_idx]
            comment_field = _comment[_type_idx].decode("utf8")

            if type_field.strip() == "" or name_field.strip() == "" or name_field.startswith("_"):
                continue
            if type_field.lower() == "boolean":
                type_field = "bool"
            if type_field != None:
                out_stream.append(u"\t/// %s" % comment_field)
                out_stream.append(u"\tpublic %s %s;" % (type_field, name_field))

        out_stream.append(u'\n\tpublic static string FileName = \"%s\";\n}\n}' % filename)
        self.write_file(out_filename, out_stream)

    def generate_cs_table_from_data(self, outpath, filename, _comment, _name, _type):
        """
        gen_CS_TABLE_From_Config
        """
        out_filename = os.path.join(outpath, filename + "_table.cs")
        out_stream = [u"using System;",
                      u"using System.Collections.Generic;",
                      u"using Joker.ResourceManager;",
                      u"using UnityEngine;\n",
                      u"namespace Table {",
                      u"public class %s_table\n{" % filename,
                      u"\tprivate %s[]\tentities;\n" % filename]

        if _type[0] == "string":
            out_stream.append(u"\tprivate Dictionary<string, int>\t\
        keyIdxMap = new Dictionary<string, int>();\n")
        else:
            out_stream.append(u"\tprivate Dictionary<int, int>\t\
        keyIdxMap = new Dictionary<int, int>();\n")
        out_stream.extend([
            u"\tprivate int count;\n\tpublic int Count\n\t{\n\
        \t\tget { return this.count; }\n\t}\n",
            u"\tstatic %s_table sInstance = null;\n\tpublic static %s_table Instance\n\
        \t{\n\t\tget\n\t\t{" % (filename, filename),
            u"\t\t\tif (sInstance == null)\n\t\t\t{\n\
            \t\t\t\tsInstance = new %s_table();\n\t\t\t\tsInstance.Load();\n\t\t\t\t}\n" % filename,
            u"\t\t\treturn sInstance; \n\t\t}\n\t}\n\n\
        \tvoid Load()\n\t{\n\t\tAction<string> onTableLoad = (text) => {",
            u"\t\tstring[] lines = text.Split(\"\\n\\r\".ToCharArray(), \
            StringSplitOptions.RemoveEmptyEntries);",
            u"\t\tint count = lines.Length;\n\t\tif (count <= 0) {\n\
        \t\t\treturn;\n\t\t}\n",
            u"\t\tentities = new %s[count];\n\t\tint realcount = 0;" %  filename,
            u"\t\tfor (int i = 0; i < count; ++i) {\n\t\t\tstring line = lines[i];",
            u"\t\t\tif (string.IsNullOrEmpty(line)) continue;",
            u"\t\t\tstring[] vals = line.Split('\\t');",
            u"\t\t\tif(vals.Length < 2) continue;\n",
            u"\t\t\tentities[i] = new %s();" % filename,
            u"\t\t\t%s entity = entities[i];\n\n" % filename
        ])

        field_index = 0
        for _type_idx, _type_val in enumerate(_type):
            type_field = _type_val
            name_field = _name[_type_idx]

            if type_field.strip() == "" or name_field.strip() == "" or name_field.startswith("_"):
                continue

            if type_field.lower() == "boolean":
                type_field = "bool"

            if type_field.lower() == "int":
                out_stream.append(u"\t\t\tentity.%s= int.Parse(vals[%s]);" \
                        % (name_field, field_index))
            elif type_field.lower() == "string":
                out_stream.append(u"\t\t\tentity.%s = vals[%s];" % (name_field, field_index))
            elif type_field.lower() == "float":
                out_stream.append(u"\t\t\tentity.%s = float.Parse(vals[%s]);" \
                        % (name_field, field_index))
            elif type_field.lower() == "bool":
                out_stream.append(u"\t\t\tentity.%s = int.Parse(vals[%s]) != 0;" \
                        % (name_field, field_index))
            elif type_field.lower() == "double":
                out_stream.append(u"\t\t\tentity.%s = double.Parse(vals[%s]) != 0;" \
                        % (name_field, field_index))
            else:
                out_stream.append(u"\t\t\tentity.%s = (Table.%s) ( int.Parse(vals[%s]) );" \
                        % (name_field, _type_idx, field_index))

            field_index += 1

        out_stream.extend([
            u"\t\t\tkeyIdxMap[ entity.%s ] = i;\n\t\t\t++realcount;\n\t\t}\n\n\n\
        \t\tthis.count = realcount;\n\t};\n" % _name[0],
              u"    string fileName = %s.FileName;" % filename,
              u"    TextAsset textAsset;",
              u"    if (AssemblySetting.LoadProtocInResource) {",
              u"        textAsset = Resources.Load<TextAsset>(\"Table/\" + fileName);",
              u"    } else {",
              u"        textAsset = RuntimeResourceManager.Instance.LoadCachedAsset<TextAsset>(fileName); ",
              u"    }",
              u"    if (textAsset == null) {",
              u"        return;",
              u"    }",
              u"    onTableLoad(textAsset.text);}"
        ])

        if _type[0] == "string":
            out_stream.append(u"\tpublic %s GetEntityByKey(string key)\
            \n\t{\n\t\tint idx;" %filename)
        else:
            out_stream.append(u"\tpublic %s GetEntityByKey(int key)\
            \n\t{\n\t\tint idx;" %filename)
        out_stream.extend([
            u"\t\tif (keyIdxMap.TryGetValue(key, out idx))\n\t\t{\n\t\t\treturn entities[idx];",
            u"\t\t}\n\t\telse\n\t\t{\n\t\t\treturn default(%s);" % filename,
            u"\t\t}\n\t}\n\n\tpublic %s GetEntityByIdx(int idx)\n\t{" % filename,
            u"\t\tif(idx < 0 || idx > count)\n\t\t{\n\t\t\treturn default(%s);" % filename,
            u"\t\t}\n\t\telse\n\t\t{\n\t\t\treturn entities[idx];\n\t\t}\n\t}\n\n}\n}\n"
        ])
        self.write_file(out_filename, out_stream)

    def generate_bin_from_data(self, outpath, filename, _comment, _name, _type, _data):
        """
        generate_bin_from_data
        """
        out_filename = os.path.join(outpath, filename + ".txt")
        out_stream = []

        for line in _data:
            line = [x.strip() for x in line.split("\t")]
            #print repr(line)
            _ = ""
            for _idx, _val in enumerate(line):
                _val = _val.strip()
                if isinstance(_val, unicode):
                    _val = str(_val)
                _type_field = _type[_idx].strip().lower()
                _name_field = _name[_idx].strip()

                if _name_field.startswith("_"):
                    continue
                if _val.endswith(".0"):
                    _val = _val[:-2]

                if _type_field in ["bool", "boolean", "int", "float", "double"]:
                    if _val == "":
                        _ += u"0\t"
                    else:
                        _ += _val + u"\t"
                elif _type_field == "string":
                    if not isinstance(_val, unicode):
                        _val = _val.decode("utf8")
                    _ += _val + u"\t"
            out_stream.append(_ + u"\r")
        self.write_file(out_filename, out_stream)
if __name__ == '__main__':
    ResTool()
    