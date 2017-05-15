#-*- coding=utf-8 -*-
"""
# author : albertcheng
# date   : 2017.5.8
# script : used for read data from excel,
#          and upload data to mysql.
#          It's coded for jenkins automatic jobs.
"""

import os
import platform
import shutil
import time
import xlrd

if platform.system().lower() == "windows":
    # checking platform and set encoding.
    DEFAULT_ENCODING = "gbk"
else:
    DEFAULT_ENCODING = "utf8"
# set env constants
EXCEL_CONFIG_PATH = os.getenv("EXCEL_CONFIG_PATH")
TEXT_CONFIG_PATH = os.getenv("TEXT_CONFIG_PATH")
WRITE_TO_FILE = "./%s/" % TEXT_CONFIG_PATH + "%s.txt"
NAME_START_ROW = 1
DATA_START_ROW = 4
class EasyXLS(object):
    """
    used for read excel.
    """

    def __init__(self):
        print "tool initialled."
        self.starttime = time.time()
        self.name_row = []
        self.name_ignore_index = []

    def get_sheets(self, _file):
        """
        get sheets from workbook
        """
        if not os.path.exists(_file):#.encode("utf8")
            print "xls not exists -- %s" % _file#.encode("gbk")
            return []
        else:
            print "found excel : %s" % _file
            workbook = xlrd.open_workbook(filename=_file, encoding_override="utf8")
            # return all static config sheet.
            return [_ for _ in workbook.sheets() if _.name.startswith("s_")]

    def check_int(self, _data):
        """
        check all int argument and return string. ## all(ord(c) < 128 for c in keyword)
        """
        if isinstance(_data, (float, int)):
            return str(int(_data)) if str(_data).endswith(".0") else str(_data)
        elif isinstance(_data, unicode):
            return _data.encode("utf-8")
        else:
            return _data

    def append_string(self, _row, data):
        """
        add string to data list. filter empty or useless row data.
        """
        def _(cell):
            return len(str(cell) if isinstance(cell, (float, int)) else cell)
        if len([x for x in _row if _(x) > 0]) > 1:
            data.append("\t".join([self.check_int(cell) for cell in _row]) + "\n")
        return data

    def read_name_row(self, _sheet):
        """
        read name row's data.
        """
        _row = _sheet.row_values(NAME_START_ROW)
        _ignores = [_row.index(x) for x in _row if x.startswith("_")]
        #self.name_ignore_index = name_ignore_index
        return self.append_string([x for x in _row if not x.startswith("_")], []), _row, _ignores

    def read_data_row(self, _row, _sheet):
        """
        read data row's data and filter useless row data.
        debug code:
        if _sheet.name == "s_dropOut":
            print len([_sheet.cell(_row, _col).value for _col \
            in range(_sheet.ncols) if _col not in self.name_ignore_index])
            print len(_sheet.row_values(_row))
            print self.name_row
            print _sheet.row_values(_row)
            print [_sheet.cell(_row, _col).value for _col in range(_sheet.ncols)\
             if _col not in self.name_ignore_index]
            print "*" * 20
        """
        return [_sheet.cell(_row, _col).value for _col in range(_sheet.ncols) \
                if _col not in self.name_ignore_index]
        #return  _sheet.row_values(_row)

    def read_data_from_sheets(self, _sheets):
        """
        read_data_from_sheets
        """
        for _sheet in _sheets:
			if _sheet.name != 's_employe_data':
				continue
            print "read data from excel - [%s]" % _sheet.name
            sheet_data, self.name_row, self.name_ignore_index = self.read_name_row(_sheet)
			print self.name_row
			print self.name_ignore_index
            for _row in range(DATA_START_ROW, _sheet.nrows):
                # read and append row data
                sheet_data = self.append_string(self.read_data_row(_row, _sheet), sheet_data)
				print sheet_data[1]
				break
            #print "read data from excel - [%s] finished." % _sheet.name
            write_to_text(_sheet.name, sheet_data)
            #print "write data to text file finished."
            #print "used time : %.2fs" % (time.time() - self.starttime)
            self.starttime = time.time()
			break

def scan_files():
    """
    scan an get all files and return
    ##.decode(DEFAULT_ENCODING))\
    """
    return [os.path.join(EXCEL_CONFIG_PATH, x)\
             for x in os.listdir("./" + EXCEL_CONFIG_PATH)]

def write_to_text(_name, _data):
    """
    write data to text file.
    """
    with open(WRITE_TO_FILE % _name, "wb") as _f:
        _f.writelines(_data)


if __name__ == "__main__":
	print TEXT_CONFIG_PATH
	print EXCEL_CONFIG_PATH
    START_TIME = time.time()
    if os.path.exists(TEXT_CONFIG_PATH):
        shutil.rmtree(TEXT_CONFIG_PATH)
    os.makedirs(TEXT_CONFIG_PATH)
    XLS_READER = EasyXLS()
    print "initialled time : %.2fs" % (time.time() - START_TIME)
    for nExcel in scan_files():
        print "*" * 20
        print "ready to handle with excel : %s" % nExcel#.encode("gbk")
        XLS_READER.read_data_from_sheets(XLS_READER.get_sheets(nExcel))
    print "totally used time : %.2fs" % (time.time() - START_TIME)
