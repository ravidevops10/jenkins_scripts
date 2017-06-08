#-*- coding=utf8 -*-
"""

"""
import sys
import os
import re
import shutil
import urllib2
import json
import time
import subprocess

if len(sys.argv) == 2:
    platformType = sys.argv[1]
else:
    platformType = "Android"

# step 1: get svn version
CustomEditor1 = r"E:\\ResourceEditor\\CustomEditor"
CustomEditor2 = r"E:\\ResourceEditor\\CustomEditor\\Assets"
CustomEditor3 = r"E:\\ResourceEditor\\CustomEditor\\Assets\\Source"
StreamAssets = CustomEditor + "//StreamingAssets"
versionJson  = StreamAssets + "//platformType//version.json"
os.chdir(StreamAssets)



def get_svn_ver(_path):
    """
    get svn verion from log info.
    """
    def _do_cmd(_cmd):
        #commands = "svn log -l 10 %s" % _path
        p = subprocess.Popen(commands.split(" "), stdout = subprocess.PIPE, stderr = subprocess.PIPE, shell = True)     
        pout, perr = p.communicate()
        return [x.strip() for x in pout.split("-" * 72) if x.strip() != ""]

    SVN_LOG = _do_cmd("svn log -l 10 %s" % _path)
    s_ver = "None"
    s_user = "None"
    for _SVN in SVN_LOG:
        _info = _SVN.split("|")
        if _info[1].strip() == "PackageBuilder":
            continue
        else:
            s_ver = re.findall(r"[0-9]+", _info[0])[0]
            s_user = _info[1].strip()
            break
    return s_ver


def write_json(_file, _json):
    """
    # write json data to file.
    """
    with open(_file, "wb") as _f:
        json.dump(_json, _f, ensure_ascii=False,encoding="UTF-8")
    return _json
        
def init_ver(_ver):
    """
    # initialized all version argus.
    """
    return {"git": "1.4.8_abcdef", "svn": _ver, "build":"", "time":time.ctime()}

if __name__ == "__main__":
    _ver = [get_svn_ver(CustomEditor1), get_svn_ver(CustomEditor2), get_svn_ver(CustomEditor3)]
    s_ver = max([int(x) for x in _ver if x != None])

    print "py: found latest resource svn version is ", s_ver
    if os.path.exists(versionJson):
        client_version = json.load(versionJson)
        client_version["svn"] = s_ver
    else:
        client_version = init_ver(s_ver)
    print "py: json data is ready - ", client_version
    write_json(r"version.json", client_version)
    print "py: generate version.json finished."
