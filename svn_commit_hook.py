#-*- coding=gbk -*-
import re,sys
import os
import shutil
import subprocess
import logging
import time

_LOG = "C://Users//newbie//Desktop//SVN_Hook_Script//log"

CustomEditor1 = r"E:\\ResourceEditor\\CustomEditor"
CustomEditor2 = r"E:\\ResourceEditor\\CustomEditor\\Assets"
CustomEditor3 = r"E:\\ResourceEditor\\CustomEditor\\Assets\\Source"

StreamAssets = CustomEditor1 + "//StreamingAssets"

# 创建一个logger,可以考虑如何将它封装
logger = logging.getLogger('mylogger')  
logger.setLevel(logging.DEBUG)  
now = time.gmtime()
now_str = "%s-%s-%s_%s%s%s" % (now.tm_year, now.tm_mon, now.tm_mday, now.tm_hour, now.tm_min, now.tm_sec)
# 创建一个handler，用于写入日志文件  
fh = logging.FileHandler(os.path.join(os.getcwd(), _LOG + '//log_%s.txt' % now_str))  
fh.setLevel(logging.DEBUG)  

# 再创建一个handler，用于输出到控制台  
ch = logging.StreamHandler()  
ch.setLevel(logging.DEBUG)  

# 定义handler的输出格式  
formatter = logging.Formatter('%(asctime)s - %(levelname)s - %(message)s')  
fh.setFormatter(formatter)  
ch.setFormatter(formatter)  

# 给logger添加handler  
logger.addHandler(fh)  
logger.addHandler(ch) 

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
    return s_ver

def run_cmd(_command):
    pout = ""
    p = subprocess.Popen(_command.split("_"), stdout = subprocess.PIPE, stderr = subprocess.PIPE, shell = True)
    #logger.info("do git command -- [%s]" % _command)
    pout, perr = p.communicate()
    logger.info(pout)
    return pout

def parse_branch_info(pout):
    OCAL_DEV_VERSION = ""
    DEPLOY_DEV_VERSION = ""
    for n in pout.split("\n"):
        _str = n[2:]
        _d = re.findall(r"[a-zA-Z0-9\/]+", _str)
        if len(_d) > 0:
            _branch = _d[0]
            _version = _d[1]
            if _branch == "dev":
                LOCAL_DEV_VERSION = _version
            if "deploy" in _branch and "dev" in _branch:
                DEPLOY_DEV_VERSION = _version
                
    if LOCAL_DEV_VERSION == DEPLOY_DEV_VERSION:# and LOCAL_DEV_VERSION == ORIGIN_DEV_VERSION:
        logger.info("更新成功，GIT-DEV同步&发布完成")
        
    else:
        logger.error("更新失败，GIT-DEV同步&发布未完成，请检查同步日志")

def main():
    """
    run tool.
    """
    
    if not os.path.exists(_LOG):
        os.mkdir(_LOG)
    os.chdir(StreamAssets)
    # step0:  get svn version from 3 paths.
    _ver = [get_svn_ver(CustomEditor1), get_svn_ver(CustomEditor2), get_svn_ver(CustomEditor3)]
    s_ver = max([int(x) for x in _ver if x != None])
    logger.info("当前资源仓库的本地svn版本号是 %s" % s_ver)
    logger.info("*" * 20)
    # Step1: 先提交streamassets目录下的内容到git
    logger.info("准备提交streamassets到138的远程Git-Deploy仓库" )
    logger.info("*" * 20)
    commands = [
        #"git checkout dev",
        #"git status",
        "git_add_.",
        "git_commit_-m " + '"commit for the latest svn content, resource version is %s"' % s_ver, 
        "git_push_deploy_dev_-v"
        ]
    for _command in commands:
        run_cmd(_command)
    logger.info("提交streamassets内容到git完毕")
    logger.info("*" * 20)
    parse_branch_info(run_cmd("git_branch_-va"))

if __name__ == "__main__":
    main()
    time.sleep(10)
    sys.exit(0)




L