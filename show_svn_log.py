#-*- coding=gbk -*-
import re,sys
import os
import subprocess
import time
now = time.gmtime()
now_str = "%s-%s-%s_%s:%s:%s" % (now.tm_year, now.tm_mon, now.tm_mday, now.tm_hour + 8, now.tm_min, now.tm_sec)
t, PATH, MESSAGEFILE, CWD = sys.argv

CustomEditor1 = r"E:\\ResourceEditor\\CustomEditor"
CustomEditor2 = r"E:\\ResourceEditor\\CustomEditor\\Assets"
CustomEditor3 = r"E:\\ResourceEditor\\CustomEditor\\Assets\\Source"

StreamAssets = CustomEditor1 + "//StreamingAssets"

def get_svn_ver(_path):
    """
    get svn verion from log info.
    """
    def _do_cmd(_cmd):
        #commands = "svn log -l 10 %s" % _path
        p = subprocess.Popen(_cmd.split(" "), stdout = subprocess.PIPE, stderr = subprocess.PIPE, shell = True)     
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
    return (s_ver, s_user)

 
def main():
    """
    run tool.
    """
    
    os.chdir(StreamAssets)
    # step0:  get svn version from 3 paths.
    _ver = [get_svn_ver(CustomEditor1), get_svn_ver(CustomEditor2), get_svn_ver(CustomEditor3)]
    s_ver, s_user = sorted(_ver, key = lambda _item: _item[0])[-1]
    
    Ver_MSG = u"%s Latest Build-Res Commited, Original SVN source-rev is [%s], pushed by [%s]\n" % (now_str, s_ver, s_user)    

    with open(MESSAGEFILE, "wb") as F:
        F.write(Ver_MSG.encode("utf8"))

if __name__ == "__main__":
    main()
   