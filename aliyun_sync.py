#-*- coding=utf8 -*-

import requests
import re
import sys
import os
import time
import json
from aliyunsdkcore.client import AcsClient
from aliyunsdkalidns.request.v20150109 import UpdateDomainRecordRequest,\
        DescribeDomainRecordsRequest, \
        DescribeDomainRecordInfoRequest
import logging
logger = logging.getLogger("")
logger.setLevel(logging.INFO)
fh = logging.FileHandler("aliyun.sync.log")
fh.setLevel(logging.DEBUG)
ch = logging.StreamHandler()
ch.setLevel(logging.INFO)
formatter = logging.Formatter('%(asctime)s - %(levelname)s - %(message)s')
fh.setFormatter(formatter)
ch.setFormatter(formatter)

logger.addHandler(fh)
logger.addHandler(ch)

class MyRouter(object):
    """
    router class.
    """
    def __init__(self):
        """
        # 初始化通用的参数配置，及通用的headers数据构建
        """
        self.router_admin = "router_admin"
        self.router_passwd = "router_passwd"
        self.headers = {
            "Content-Type":"application/x-www-form-urlencoded",
            "User-Agent":"Mozilla/5.0 (Windows NT 10.0; WOW64) \
            AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.82 Safari/537.36",
            "Upgrade-Insecure-Requests":"1",
            "Cache-Control":"max-age=0",
            "Connection":"keep-alive",
            "Origin": "http://192.168.1.1",
            "Referer": "",
            "Accept-Language": "zh-CN,zh;q=0.8,en;q=0.6"
            }
        self.cookie = {}

    def login(self):
        """
        # 登录，并获取sessionID
        """
        login_data = {
            "save2Cookie":"",
            "vldcode":"",
            "account":self.router_admin,
            "password":self.router_passwd,
            "btnSubmit":"+%B5%C7%C2%BC+"
            }
        login_url = "http://192.168.1.1/userLogin.asp"
        self.headers["Referer"] = login_url
        ret = requests.post(login_url, data=login_data, headers=self.headers)
        print ret.content
        session_id = re.search(r"var sessionid=\"[a-zA-Z0-9^\"]+", \
            ret.content).group().split("\"")[1]
        return "JSESSIONID=%s" % session_id
        

    def get_router_ip(self, session_id):
        """
        # 获取路由器的基础信息
        """
        print "login in router with seesionID : %s" % session_id
        _url = "http://192.168.1.1/maintain_basic.asp"
        ret = requests.get(_url, headers=self.headers, cookies={"Cookie":session_id})
        #print ret.text
        ret_split = ret.text.split("var wan_basic_info=new Array(")[1]
        wan_basic_info = ret_split.split(");")[0]
        outer_ip = wan_basic_info.split(";")[5]
        return outer_ip


class MyAliyun(object):
    """
    nothing to say.
    """
    def __init__(self):
        self.accessKey = "LTAIz97htsfic2av"
        self.secretKey = "IqHvVdBiBfdg7RC3Lev9O0IxfKfwVM"
        self.regionID = "cn-hangzhou"
        self.domain = "youthcreative.cn"
        self.client = AcsClient(self.accessKey, self.secretKey,self.regionID)
        
    def get_record_id(self):
        request = DescribeDomainRecordsRequest.DescribeDomainRecordsRequest()
        request.set_DomainName(self.domain)
        request.set_accept_format("json")
        result = json.loads(self.client.do_action_with_exception(request))
        request_id = result["RequestId"]
        DomainRecords = result["DomainRecords"]["Record"]
        for _domain in DomainRecords:
            if _domain["Type"] == "A":
                return _domain
        return None
    
    def set_dns_ip(self, domain, router):
        logger.info("current domain info is %s" % domain)
        request = UpdateDomainRecordRequest.UpdateDomainRecordRequest()
        request.set_RR(domain["RR"])
        request.set_Type(domain["Type"])
        request.set_Value(router)
        request.set_RecordId(domain["RecordId"])
        request.set_TTL(domain["TTL"])
        request.set_accept_format("json")
        result = self.client.do_action_with_exception(request)
        
        logger.info("aliyun updated. new seesion is %s" % result)


def get_router_ip(url = "http://ip.cn/"):
    page = requests.get(url)
    router_ip = re.search("[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+", \
            page.content).group()
    return router_ip


def main():
    router = MyRouter()
    aliyun = MyAliyun()
    session = router.login()
    start_time = time.time()
    while 1:
        logger.info("*******loop***********")
        router_ip = router.get_router_ip(session)
        domain = aliyun.get_record_id()
        if domain is None:
            logger.info("error in parsing domain.")
            time.sleep(60)
            continue

        domain_ip = domain["Value"]
        domain_id = domain["RecordId"]
        if domain_ip == router_ip:
            logger.info("domain ip is %s" % domain_ip)
            logger.info("router ip is %s" % router_ip)
            logger.info("do not need to sync aliyun's domain ip.")
            time.sleep(5)
        else:
            logger.info("domain ip is %s" % domain_ip)
            logger.info("router ip is %s" % router_ip)
            logger.info("ready to modify.")
            aliyun.set_dns_ip(domain, router_ip)
            time.sleep(5)
        
if __name__ == "__main__":
    main()