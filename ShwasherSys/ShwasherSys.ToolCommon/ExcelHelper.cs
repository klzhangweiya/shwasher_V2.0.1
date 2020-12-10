using NPOI.HSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NPOI.HSSF.Util;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace ShwasherSys
{
    public static class ExcelHelper
    {
        #region Excel导入

        /// <summary>
        /// 从Excel取数据并记录到List集合里
        /// </summary>
        /// <param name="cellHeader">单元头的值和名称：{ { "UserName", "姓名" }, { "Age", "年龄" } };</param>
        /// <param name="filePath">保存文件绝对路径</param>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="startIndex">数据行开始序列，默认为1（即第二列，从0开始）</param>
        /// <returns>转换后的List对象集合</returns>
        public static List<T> ExcelToEntityList<T>(this Dictionary<string, string> cellHeader, string filePath, out StringBuilder errorMsg, int startIndex = 1) where T : new()
        {
            List<T> enlist = new List<T>();
            errorMsg = new StringBuilder();
            try
            {
                if (Regex.IsMatch(filePath, ".xls$")) // 2003
                {
                    enlist = Excel2003ToEntityList<T>(cellHeader, filePath, out errorMsg, startIndex);
                }
                else if (Regex.IsMatch(filePath, ".xlsx$")) // 2007
                {
                    enlist = Excel2007ToEntityList<T>(cellHeader, filePath, out errorMsg, startIndex);
                }
                return enlist;
            }
            catch (Exception ex)
            {
                //typeof(ExcelHelper).LogError(ex);
                return default(List<T>);
            }
        }

        /// <summary>
        /// 从Excel2003取数据并记录到List集合里
        /// </summary>
        /// <param name="cellHeader">单元头的Key和Value：{ { "UserName", "姓名" }, { "Age", "年龄" } };</param>
        /// <param name="filePath">保存文件绝对路径</param>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="startIndex"></param>
        /// <returns>转换好的List对象集合</returns>
        private static List<T> Excel2003ToEntityList<T>(this Dictionary<string, string> cellHeader, string filePath, out StringBuilder errorMsg, int startIndex = 1) where T : new()
        {
            errorMsg = new StringBuilder(); // 错误信息,Excel转换到实体对象时，会有格式的错误信息
            List<T> enlist = new List<T>(); // 转换后的集合
            try
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(fs);
                    HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0); // 获取此文件第一个Sheet页
                    for (int rowIndex = startIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        // 1.判断当前行是否空行，若空行就不在进行读取下一行操作，结束Excel读取操作
                        IRow row = sheet.GetRow(rowIndex);
                        if (row == null)
                        {
                            break;
                        }
                        // 2.每一个Excel row转换为一个实体对象
                        T en = new T();
                        ExcelRowToEntity(cellHeader, row, rowIndex, en, ref errorMsg);

                        enlist.Add(en);
                    }
                }
                return enlist;
            }
            catch (Exception ex)
            {
                //typeof(ExcelHelper).LogError(ex);
                return default(List<T>);
            }
        }

        /// <summary>
        /// 从Excel2007取数据并记录到List集合里
        /// </summary>
        /// <param name="cellHeader">单元头的Key和Value：{ { "UserName", "姓名" }, { "Age", "年龄" } };</param>
        /// <param name="filePath">保存文件绝对路径</param>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="startIndex">数据行开始序列，默认为1（即第二列，从0开始）</param>
        /// <returns>转换好的List对象集合</returns>
        private static List<T> Excel2007ToEntityList<T>(this Dictionary<string, string> cellHeader, string filePath, out StringBuilder errorMsg, int startIndex = 1) where T : new()
        {
            errorMsg = new StringBuilder(); // 错误信息,Excel转换到实体对象时，会有格式的错误信息
            List<T> enlist = new List<T>(); // 转换后的集合
            try
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(fs);
                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0); // 获取此文件第一个Sheet页
                    for (int rowIndex = startIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        // 1.判断当前行是否空行，若空行就不在进行读取下一行操作，结束Excel读取操作
                        IRow row = sheet.GetRow(rowIndex);
                        if (row == null)
                        {
                            break;
                        }
                        // 2.每一个Excel row转换为一个实体对象
                        T en = new T();
                        ExcelRowToEntity(cellHeader, row, rowIndex, en, ref errorMsg);
                        enlist.Add(en);
                    }
                }
                return enlist;
            }
            catch (Exception ex)
            {
                //typeof(ExcelHelper).LogError(ex);
                return default(List<T>);
            }
        }

        #endregion Excel导入

        #region Excel导出

        public static string ToExcel2003(List<ToExcelObj> cellHeader, IList enList,
            string sheetName, string filePath)
        {
            var lcRetVal = "";
            try
            {
                string fileName = "D-" + sheetName + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls"; // 文件名称
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = Path.Combine(filePath, fileName);
                // 2.解析单元格头部，设置单元头的中文名称
                HSSFWorkbook workbook = new HSSFWorkbook(); // 工作簿
                ISheet sheet = workbook.CreateSheet(sheetName); // 工作表
                IRow row = sheet.CreateRow(0);//创建
                string headcss = $"bgc:{ColorType.LightBlue.ToString()};" +
                                 $"warp:{HorizontalAlignment.Center.ToString()};" +
                                 $"align:{HorizontalAlignment.Center.ToString()};" +
                                 $"v-align:{VerticalAlignment.Center.ToString()};" +
                                 $"b:{BorderStyle.Medium.ToString()};" +
                                 $"bc:{ColorType.Black.ToString()};" +
                                 "inden:0;" +
                                 "df:@;"+
                                 $"fc:{ColorType.White.ToString()};" +//font-color
                                 "fn:宋体;" +//font-name
                                 "fs:15;" +//font-size
                                 "fw:normal;" +//font-weight
                                 "fu:none;" +//font-underline
                                 "fi:false;" +//font-italic
                                 "fst:false;" +//font-strikeout
                                 "fss:none;";//font-superscript;
                for (int i = 0; i < cellHeader.Count; i++)
                {
                    var cellhead = row.CreateCell(i);
                    cellhead.SetCellValue(cellHeader[i].ShowColumn); // 列名为Key的值
                    CellStyleCss.Instants.Css(cellhead, headcss);
                    sheet.AutoSizeColumn(i);
                }
                // 3.List对象的值赋值到Excel的单元格里
                int rowIndex = 1; // 从第二行开始赋值(第一行已设置为单元头)
                foreach (var en in enList)
                {
                    IRow rowTmp = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < cellHeader.Count; i++) // 根据指定的属性名称，获取对象指定属性的值
                    {
                        string cellValue = ""; // 单元格的值
                        object properotyValue = null; // 属性的值
                        PropertyInfo properotyInfo = en.GetType().GetProperty(cellHeader[i].MapColumn);
                        if (properotyInfo != null)
                        {
                            properotyValue = properotyInfo.GetValue(en, null);
                        }
                        // 3.3 属性值经过转换赋值给单元格值
                        if (properotyValue != null)
                        {
                            cellValue = properotyValue.ToString();
                            // 3.3.1 对时间初始值赋值为空
                            if (cellValue.Trim() == "0001/1/1 0:00:00" || cellValue.Trim() == "0001/1/1 23:59:59")
                            {
                                cellValue = "";
                            }
                        }
                        // 3.4 填充到Excel的单元格里
                        var cl= rowTmp.CreateCell(i);
                        cl.SetCellValue(cellValue);
                        //CellStyleCss.Instants.Css(cl, cellHeader[i].StyleStr);
                        //sheet.AutoSizeColumn(i);
                    }
                    
                    rowIndex++;
                }

                // 4.生成文件
                FileStream file = new FileStream(filePath, FileMode.Create);
                workbook.Write(file);
                file.Close();
                lcRetVal = fileName;
            }
            catch (Exception ex)
            {
               LogHelper.LogError("excel导出",ex); 
            }

            return lcRetVal;
        }

        /// <summary>
        /// 实体类集合导出到EXCLE2003
        /// </summary>
        /// <param name="cellHeader">单元头的Key和Value：{ { "UserName", "姓名" }, { "Age", "年龄" } };</param>
        /// <param name="enList">数据源</param>
        /// <param name="sheetName">工作表名称</param>
        /// <param name="filePath">文件的下载地址</param>
        /// <returns></returns>
        public static string EntityListToExcel2003(this Dictionary<string, string> cellHeader, IList enList, string sheetName, string filePath)
        {
            var lcRetVal = "";
            try
            {
                string fileName = "D-" + sheetName + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls"; // 文件名称
                //string urlPath = "UpFiles/ExcelFiles/" + fileName; // 文件下载的URL地址，供给前台下载
                //string filePath = HttpContext.Current.Server.MapPath("\\" + urlPath); // 文件路径

                // 1.检测是否存在文件夹，若不存在就建立个文件夹
                //string directoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = Path.Combine(filePath, fileName);
                // 2.解析单元格头部，设置单元头的中文名称
                HSSFWorkbook workbook = new HSSFWorkbook(); // 工作簿
                ISheet sheet = workbook.CreateSheet(sheetName); // 工作表
                IRow row = sheet.CreateRow(0);
                List<string> keys = cellHeader.Keys.ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(cellHeader[keys[i]]); // 列名为Key的值
                }
                // 3.List对象的值赋值到Excel的单元格里
                int rowIndex = 1; // 从第二行开始赋值(第一行已设置为单元头)
                foreach (var en in enList)
                {
                    IRow rowTmp = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < keys.Count; i++) // 根据指定的属性名称，获取对象指定属性的值
                    {
                        string cellValue = ""; // 单元格的值
                        object properotyValue = null; // 属性的值
                        System.Reflection.PropertyInfo properotyInfo; // 属性的信息

                        // 3.1 若属性头的名称包含'.',就表示是子类里的属性，那么就要遍历子类，eg：UserEn.UserName
                        if (keys[i].IndexOf(".", StringComparison.Ordinal) >= 0)
                        {
                            // 3.1.1 解析子类属性(这里只解析1层子类，多层子类未处理)
                            string[] properotyArray = keys[i].Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                            string subClassName = properotyArray[0]; // '.'前面的为子类的名称
                            string subClassProperotyName = properotyArray[1]; // '.'后面的为子类的属性名称
                            System.Reflection.PropertyInfo subClassInfo = en.GetType().GetProperty(subClassName); // 获取子类的类型
                            if (subClassInfo != null)
                            {
                                // 3.1.2 获取子类的实例
                                var subClassEn = en.GetType().GetProperty(subClassName)?.GetValue(en, null);
                                // 3.1.3 根据属性名称获取子类里的属性类型
                                properotyInfo = subClassInfo.PropertyType.GetProperty(subClassProperotyName);
                                if (properotyInfo != null)
                                {
                                    properotyValue = properotyInfo.GetValue(subClassEn, null); // 获取子类属性的值
                                }
                            }
                        }
                        else
                        {
                            // 3.2 若不是子类的属性，直接根据属性名称获取对象对应的属性
                            properotyInfo = en.GetType().GetProperty(keys[i]);
                            if (properotyInfo != null)
                            {
                                properotyValue = properotyInfo.GetValue(en, null);
                            }
                        }

                        // 3.3 属性值经过转换赋值给单元格值
                        if (properotyValue != null)
                        {
                            cellValue = properotyValue.ToString();
                            // 3.3.1 对时间初始值赋值为空
                            if (cellValue.Trim() == "0001/1/1 0:00:00" || cellValue.Trim() == "0001/1/1 23:59:59")
                            {
                                cellValue = "";
                            }
                        }

                        // 3.4 填充到Excel的单元格里
                        rowTmp.CreateCell(i).SetCellValue(cellValue);
                    }
                    rowIndex++;
                }

                // 4.生成文件
                FileStream file = new FileStream(filePath, FileMode.Create);
                workbook.Write(file);
                file.Close();
                lcRetVal = fileName;
            }
            catch (Exception ex)
            {
                //typeof(ExcelHelper).LogError(ex);
            }

            return lcRetVal;
        }

        public static string EntityListToExcel2003(this HSSFWorkbook workbook,
            string sheetName, string filePath)
        {
            var lcRetVal = "";
            try
            {
                string fileName = "D-" + sheetName + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls"; // 文件名称
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                   
                }
                filePath = Path.Combine(filePath, fileName);
                FileStream file = new FileStream(filePath, FileMode.Create);
                workbook.Write(file);
                file.Close();
                lcRetVal = fileName;
            }
            catch (Exception ex)
            {
                //typeof(ExcelHelper).LogError(ex);
            }

            return lcRetVal;
        }
        public static HSSFWorkbook EntityListToExcel2003book(this Dictionary<string, string> cellHeader, IList enList,
            string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(); // 工作簿
            ISheet sheet = workbook.CreateSheet(sheetName); // 工作表
            IRow row = sheet.CreateRow(0);
            List<string> keys = cellHeader.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                row.CreateCell(i).SetCellValue(cellHeader[keys[i]]); // 列名为Key的值
            }
            // 3.List对象的值赋值到Excel的单元格里
            int rowIndex = 1; // 从第二行开始赋值(第一行已设置为单元头)
            foreach (var en in enList)
            {
                IRow rowTmp = sheet.CreateRow(rowIndex);
                for (int i = 0; i < keys.Count; i++) // 根据指定的属性名称，获取对象指定属性的值
                {
                    string cellValue = ""; // 单元格的值
                    object properotyValue = null; // 属性的值
                    System.Reflection.PropertyInfo properotyInfo; // 属性的信息

                    // 3.1 若属性头的名称包含'.',就表示是子类里的属性，那么就要遍历子类，eg：UserEn.UserName
                    if (keys[i].IndexOf(".", StringComparison.Ordinal) >= 0)
                    {
                        // 3.1.1 解析子类属性(这里只解析1层子类，多层子类未处理)
                        string[] properotyArray = keys[i].Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                        string subClassName = properotyArray[0]; // '.'前面的为子类的名称
                        string subClassProperotyName = properotyArray[1]; // '.'后面的为子类的属性名称
                        System.Reflection.PropertyInfo subClassInfo = en.GetType().GetProperty(subClassName); // 获取子类的类型
                        if (subClassInfo != null)
                        {
                            // 3.1.2 获取子类的实例
                            var subClassEn = en.GetType().GetProperty(subClassName)?.GetValue(en, null);
                            // 3.1.3 根据属性名称获取子类里的属性类型
                            properotyInfo = subClassInfo.PropertyType.GetProperty(subClassProperotyName);
                            if (properotyInfo != null)
                            {
                                properotyValue = properotyInfo.GetValue(subClassEn, null); // 获取子类属性的值
                            }
                        }
                    }
                    else
                    {
                        // 3.2 若不是子类的属性，直接根据属性名称获取对象对应的属性
                        properotyInfo = en.GetType().GetProperty(keys[i]);
                        if (properotyInfo != null)
                        {
                            properotyValue = properotyInfo.GetValue(en, null);
                        }
                    }

                    // 3.3 属性值经过转换赋值给单元格值
                    if (properotyValue != null)
                    {
                        cellValue = properotyValue.ToString();
                        // 3.3.1 对时间初始值赋值为空
                        if (cellValue.Trim() == "0001/1/1 0:00:00" || cellValue.Trim() == "0001/1/1 23:59:59")
                        {
                            cellValue = "";
                        }
                    }

                    // 3.4 填充到Excel的单元格里
                    rowTmp.CreateCell(i).SetCellValue(cellValue);
                }
                rowIndex++;
            }

            return workbook;
        }
        /// <summary>
        /// 实体类集合导出到EXCLE2007
        /// </summary>
        /// <param name="cellHeader">单元头的Key和Value：{ { "UserName", "姓名" }, { "Age", "年龄" } };</param>
        /// <param name="enList">数据源</param>
        /// <param name="sheetName">工作表名称</param>
        /// <param name="filePath">文件的下载地址</param>
        /// <returns></returns>
        public static string EntityListToExcel2007(this Dictionary<string, string> cellHeader, IList enList, string sheetName, string filePath)
        {
            var lcRetVal = "";
            try
            {
                string fileName = "D-" + sheetName + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx"; // 文件名称

                // 1.检测是否存在文件夹，若不存在就建立个文件夹
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = Path.Combine(filePath, fileName);
                // 2.解析单元格头部，设置单元头的中文名称
                XSSFWorkbook workbook = new XSSFWorkbook(); // 工作簿
                ISheet sheet = workbook.CreateSheet(sheetName); // 工作表
                IRow row = sheet.CreateRow(0);
                List<string> keys = cellHeader.Keys.ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(cellHeader[keys[i]]); // 列名为Key的值
                }
                // 3.List对象的值赋值到Excel的单元格里
                int rowIndex = 1; // 从第二行开始赋值(第一行已设置为单元头)
                foreach (var en in enList)
                {
                    IRow rowTmp = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < keys.Count; i++) // 根据指定的属性名称，获取对象指定属性的值
                    {
                        string cellValue = ""; // 单元格的值
                        object properotyValue = null; // 属性的值
                        System.Reflection.PropertyInfo properotyInfo; // 属性的信息

                        // 3.1 若属性头的名称包含'.',就表示是子类里的属性，那么就要遍历子类，eg：UserEn.UserName
                        if (keys[i].IndexOf(".", StringComparison.Ordinal) >= 0)
                        {
                            // 3.1.1 解析子类属性(这里只解析1层子类，多层子类未处理)
                            string[] properotyArray = keys[i].Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                            string subClassName = properotyArray[0]; // '.'前面的为子类的名称
                            string subClassProperotyName = properotyArray[1]; // '.'后面的为子类的属性名称
                            System.Reflection.PropertyInfo subClassInfo = en.GetType().GetProperty(subClassName); // 获取子类的类型
                            if (subClassInfo != null)
                            {
                                // 3.1.2 获取子类的实例
                                var subClassEn = en.GetType().GetProperty(subClassName)?.GetValue(en, null);
                                // 3.1.3 根据属性名称获取子类里的属性类型
                                properotyInfo = subClassInfo.PropertyType.GetProperty(subClassProperotyName);
                                if (properotyInfo != null)
                                {
                                    properotyValue = properotyInfo.GetValue(subClassEn, null); // 获取子类属性的值
                                }
                            }
                        }
                        else
                        {
                            // 3.2 若不是子类的属性，直接根据属性名称获取对象对应的属性
                            properotyInfo = en.GetType().GetProperty(keys[i]);
                            if (properotyInfo != null)
                            {
                                properotyValue = properotyInfo.GetValue(en, null);
                            }
                        }

                        // 3.3 属性值经过转换赋值给单元格值
                        if (properotyValue != null)
                        {
                            cellValue = properotyValue.ToString();
                            // 3.3.1 对时间初始值赋值为空
                            if (cellValue.Trim() == "0001/1/1 0:00:00" || cellValue.Trim() == "0001/1/1 23:59:59")
                            {
                                cellValue = "";
                            }
                        }

                        // 3.4 填充到Excel的单元格里
                        rowTmp.CreateCell(i).SetCellValue(cellValue);
                    }
                    rowIndex++;
                }

                // 4.生成文件
                FileStream file = new FileStream(filePath, FileMode.Create);
                workbook.Write(file);
                file.Close();
                lcRetVal = fileName;
            }
            catch (Exception ex)
            {
                //typeof(ExcelHelper).LogError(ex);
            }

            return lcRetVal;
        }

        #endregion Excel导出

        #region Common

        /// <summary>
        /// Excel row转换为实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cellHeader">单元头的Key和Value：{ { "UserName", "姓名" }, { "Age", "年龄" } };</param>
        /// <param name="row">Excel row</param>
        /// <param name="rowIndex">row index</param>
        /// <param name="en">实体</param>
        /// <param name="errorMsg">错误信息</param>
        private static void ExcelRowToEntity<T>(Dictionary<string, string> cellHeader, IRow row, int rowIndex, T en, ref StringBuilder errorMsg)
        {
            List<string> keys = cellHeader.Keys.ToList(); // 要赋值的实体对象属性名称
            string errStr = ""; // 当前行转换时，是否有错误信息，格式为：第1行数据转换异常：XXX列；
            for (int i = 0; i < keys.Count; i++)
            {
                // 1.若属性头的名称包含'.',就表示是子类里的属性，那么就要遍历子类，eg：UserEn.TrueName
                if (keys[i].IndexOf(".", StringComparison.Ordinal) >= 0)
                {
                    // 1)解析子类属性
                    string[] properotyArray = keys[i].Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    string subClassName = properotyArray[0]; // '.'前面的为子类的名称
                    string subClassProperotyName = properotyArray[1]; // '.'后面的为子类的属性名称
                    System.Reflection.PropertyInfo subClassInfo = en.GetType().GetProperty(subClassName); // 获取子类的类型
                    if (subClassInfo != null)
                    {
                        // 2)获取子类的实例
                        var subClassEn = en.GetType().GetProperty(subClassName)?.GetValue(en, null);
                        // 3)根据属性名称获取子类里的属性信息
                        System.Reflection.PropertyInfo properotyInfo = subClassInfo.PropertyType.GetProperty(subClassProperotyName);
                        if (properotyInfo != null)
                        {
                            try
                            {
                                // Excel单元格的值转换为对象属性的值，若类型不对，记录出错信息
                                properotyInfo.SetValue(subClassEn, GetExcelCellToProperty(properotyInfo.PropertyType, row.GetCell(i)), null);
                            }
                            catch (Exception e)
                            {
                                //typeof(ExcelHelper).LogError(ex);
                                if (errStr.Length == 0)
                                {
                                    errStr = "第" + rowIndex + "行数据转换异常：";
                                }
                                errStr += cellHeader[keys[i]] + "列；";

                            }

                        }
                    }
                }
                else
                {
                    // 2.给指定的属性赋值
                    System.Reflection.PropertyInfo properotyInfo = en.GetType().GetProperty(keys[i]);
                    if (properotyInfo != null)
                    {
                        try
                        {
                            // Excel单元格的值转换为对象属性的值，若类型不对，记录出错信息
                            properotyInfo.SetValue(en, GetExcelCellToProperty(properotyInfo.PropertyType, row.GetCell(i)), null);
                        }
                        catch (Exception e)
                        {
                            //typeof(ExcelHelper).LogError(ex);
                            if (errStr.Length == 0)
                            {
                                errStr = "第" + rowIndex + "行数据转换异常：";
                            }
                            errStr += cellHeader[keys[i]] + "列；";
                        }
                    }
                }
            }
            // 若有错误信息，就添加到错误信息里
            if (errStr.Length > 0)
            {
                errorMsg.AppendLine(errStr);
            }
        }

        /// <summary>
        /// Excel Cell转换为实体的属性值
        /// </summary>
        /// <param name="distanceType">目标对象类型</param>
        /// <param name="sourceCell">对象属性的值</param>
        private static object GetExcelCellToProperty(Type distanceType, ICell sourceCell)
        {
            object rs = distanceType.IsValueType ? Activator.CreateInstance(distanceType) : null;

            // 1.判断传递的单元格是否为空
            if (sourceCell == null || string.IsNullOrEmpty(sourceCell.ToString()))
            {
                return rs;
            }

            // 2.Excel文本和数字单元格转换，在Excel里文本和数字是不能进行转换，所以这里预先存值
            object sourceValue = null;
            switch (sourceCell.CellType)
            {
                case CellType.Blank:
                    break;

                case CellType.Boolean:
                    break;

                case CellType.Error:
                    break;

                case CellType.Formula:
                    break;

                case CellType.Numeric:
                    sourceValue = sourceCell.NumericCellValue;
                    break;

                case CellType.String:
                    sourceValue = sourceCell.StringCellValue;
                    break;

                case CellType.Unknown:
                    break;
            }

            string valueDataType = distanceType.Name;

            // 在这里进行特定类型的处理
            switch (valueDataType.ToLower()) // 以防出错，全部小写
            {
                case "string":
                    rs = sourceValue?.ToString();
                    break;
                case "int":
                case "int16":
                case "int32":
                    rs = (int)Convert.ChangeType(sourceCell.NumericCellValue.ToString(CultureInfo.InvariantCulture), distanceType);
                    break;
                case "float":
                case "single":
                    rs = (float)Convert.ChangeType(sourceCell.NumericCellValue.ToString(CultureInfo.InvariantCulture), distanceType);
                    break;
                case "datetime":
                    rs = sourceCell.DateCellValue;
                    break;
                case "guid":
                    rs = (Guid)Convert.ChangeType(sourceCell.NumericCellValue.ToString(CultureInfo.InvariantCulture), distanceType);
                    return rs;
            }
            return rs;
        }

        #endregion

        public static HSSFWorkbook CreateWorkBook03(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return new HSSFWorkbook();
            }
            FileStream file = new FileStream(filePath, FileMode.Open);
            return new HSSFWorkbook(file);
        }

        public static XSSFWorkbook CreateWorkBook07(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return new XSSFWorkbook();
            }
            return new XSSFWorkbook(filePath);
        }

        /// <summary>
        /// 创建Sheet
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="defaultWidth"></param>
        /// <param name="defaultHeight"></param>
        /// <param name="is07"></param>
        /// <param name="workbook"></param>
        /// <returns></returns>
        public static ISheet GenerateSheet(this string sheetName, int defaultWidth = 20, int defaultHeight = 20, bool is07 = true, IWorkbook workbook = null)
        {
            workbook = is07 ? workbook ?? CreateWorkBook07() : workbook ?? CreateWorkBook03();
            var sheet = workbook.CreateSheet(sheetName);
            sheet.DefaultColumnWidth = defaultWidth;
            sheet.DefaultRowHeight = (short)(defaultHeight * 20);
            return sheet;
        }

        /// <summary>
        /// 创建行
        /// </summary>
        /// <param name="sheet">表</param>
        /// <param name="rowIndex">第几行（从1开始计数）</param>
        /// <returns></returns>
        public static IRow GenerateRow(this ISheet sheet, int rowIndex)
        {
            rowIndex = rowIndex <= 0 ? 1 : rowIndex;
            var row = sheet.GetRow(rowIndex - 1) ?? sheet.CreateRow(rowIndex - 1);
            
            return row;
        }
        //插入
        public static  void InsertRows(this ISheet sheet, int insertRowIndex, List<IRow> formatRows)
        {
            foreach (var row in formatRows)
            {
                var r = row.Sheet.GenerateRow(insertRowIndex);
                if (r != null)
                {
                    row.Sheet.RemoveRow(r);
                }
                row.CopyRowTo(insertRowIndex - 1);
                insertRowIndex++;
            }
        }
        //取
        public static List<IRow> GetRows(this ISheet sheet, int startRowIndex,int endRowIndex)
        {
            List<IRow> rows = new List<IRow>();
            for (int i = 0; i <= endRowIndex-startRowIndex; i++)
            {
                rows.Add(sheet.GenerateRow(startRowIndex+i));
            }
            return rows;
        }
      
        /// <summary>
        /// 创建单元格
        /// </summary>
        /// <param name="sheet">表</param>
        /// <param name="rowIndex">第几行（从1开始计数）</param>
        /// <param name="columnIndex">第几列（从1开始计数）</param>
        /// <param name="val">值</param>
        /// <param name="cellType">单元格类型</param>
        /// <returns></returns>
        public static ICell GenerateCell(this ISheet sheet, int rowIndex, int columnIndex, string val = null, CellType cellType = CellType.String)
        {
            var row = sheet.GenerateRow(rowIndex);
            var cell = GenerateCell(row, columnIndex, val, cellType);
            return cell;
        }

        /// <summary>
        /// 创建单元格
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnIndex">第几列（从1开始计数）</param>
        /// <param name="val">值</param>
        /// <param name="cellType">单元格类型</param>
        /// <returns></returns>
        public static ICell GenerateCell(this IRow row, int columnIndex, string val = null, CellType cellType = CellType.String)
        {
            columnIndex = columnIndex <= 0 ? 1 : columnIndex;
            var cell = row.GetCell(columnIndex - 1) ?? row.CreateCell(columnIndex - 1);
            if (!string.IsNullOrEmpty(val))
            {
                cell.SetCellValue(val);
                cell.SetCellType(cellType);
            }
            return cell;
        }

        /// <summary>
        /// 设置列宽度
        /// </summary>
        /// <param name="sheet">表</param>
        /// <param name="columnIndex">第几列（从1开始计数）</param>
        /// <param name="width">宽度</param>
        /// <returns></returns>
        public static ISheet SetCellWidth(this ISheet sheet, int columnIndex, int width)
        {
            sheet.SetColumnWidth(columnIndex - 1, width * 256);
            return sheet;
        }

        /// <summary>
        /// 设置列宽度
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnIndex">第几列（从1开始计数）</param>
        /// <param name="width">宽度</param>
        /// <returns></returns>
        public static IRow SetCellWidth(this IRow row, int columnIndex, int width)
        {
            row.Sheet.SetColumnWidth(columnIndex - 1, width * 256);
            return row;
        }

        /// <summary>
        /// 设置列宽度
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="columnIndex">第几列（从1开始计数）</param>
        /// <param name="width">宽度</param>
        /// <returns></returns>
        public static ICell SetCellWidth(this ICell cell, int columnIndex, int width)
        {
            cell.Sheet.SetColumnWidth(columnIndex - 1, width * 256);
            return cell;
        }

        /// <summary>
        /// 设置行高度
        /// </summary>
        /// <param name="sheet">表</param>
        /// <param name="rowIndex">第几行（从1开始计数）</param>
        /// <param name="height">宽度</param>
        /// <returns></returns>
        public static ISheet SetRowHeight(this ISheet sheet, int rowIndex, int height)
        {
            sheet.GetRow(rowIndex - 1).SetRowHeight(height);
            return sheet;
        }
        /// <summary>
        /// 设置行高度
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="height">宽度</param>
        /// <returns></returns>
        public static IRow SetRowHeight(this IRow row, int height)
        {
            row.Height = (short)(height * 20);
            return row;
        }

        /// <summary>
        /// 设置行高度
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="height">宽度</param>
        /// <returns></returns>
        public static ICell SetRowHeight(this ICell cell, int height)
        {
            cell.Row.SetRowHeight(height);
            return cell;
        }


        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="styleStr">样式字符串</param>
        /// <param name="instants">样式转换器</param>
        /// <returns></returns>
        public static ICell SetCellCss(this ICell cell, CellStyleCss instants = null, string styleStr = null)
        {
            instants = instants ?? CellStyleCss.Instants;
            return instants.Css(cell, styleStr);
        }
        public static ICell SetValue<T>(this ICell cell,  object obj,ICellStyle cellStyle)
        {
            cell.CellStyle = cellStyle;
            return SetValue<T>(cell, obj);
        }

        public static ICell SetValue(this ICell cell, string obj, ICellStyle cellStyle)
        {
            cell.CellStyle = cellStyle;
            return SetValue<string>(cell, obj);
        }
        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ICell SetValue(this ICell cell, string obj)
        {
            obj = obj ?? "";
            return SetValue<string>(cell, obj);
        }

        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <typeparam name="T">值类型（sting,bool,double,DateTime)</typeparam>
        /// <param name="cell"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ICell SetValue<T>(this ICell cell, object obj)
        {
            Type type = typeof(T);
            if (type == typeof(string))
            {
                cell.SetCellValue(obj.ToString());
            }
            else if (type == typeof(bool) && bool.TryParse(obj.ToString(), out var bValue))
            {
                cell.SetCellValue(bValue);
            }
            else if ((type == typeof(int) || type == typeof(double) || type == typeof(decimal)) && double.TryParse(obj.ToString(), out var iValue))
            {
                cell.SetCellValue(iValue);
            }
            else if (type == typeof(DateTime) && DateTime.TryParse(obj.ToString(), out var dValue))
            {
                cell.SetCellValue(dValue);
            }

            return cell;
        }

        #region DATA-FORMAT

        public static ICellStyle SetCellDateTime(this ICell cell, DateTime value, string formatStr = "yyyy-MM-dd HH:mm:ss")
        {
            cell.SetCellValue(value);
            cell.SetDataFormat(formatStr);
            return cell.CellStyle;
        }

        public static ICellStyle SetCellString(this ICell cell, double value, string formatStr = "@")
        {
            cell.SetCellValue(value);
            cell.SetDataFormat(formatStr);
            return cell.CellStyle;
        }

        public static ICellStyle SetCellDouble(this ICell cell, double value, string formatStr = "0.00")
        {
            cell.SetCellValue(value);
            cell.SetDataFormat(formatStr);
            return cell.CellStyle;
        }

        public static ICellStyle SetCellCurrency(this ICell cell, double value, string formatStr = "¥#,###.##")
        {
            cell.SetCellValue(value);
            cell.SetDataFormat(formatStr);
            return cell.CellStyle;
        }

        public static ICellStyle SetCellPercent(this ICell cell, double value, string formatStr = "0.00%")
        {
            cell.SetCellValue(value);
            cell.SetDataFormat(formatStr);
            return cell.CellStyle;
        }

        public static void SetDataFormat(this ICell cell, string formatStr = "¥#,##0.00")
        {
            var cellStyle = cell.CellStyle ?? GetCellStyle(cell);
            var format = cell.Sheet.Workbook.CreateDataFormat();
            cellStyle.DataFormat = format.GetFormat(formatStr);
            cell.SetCellStyle(cellStyle);
        }

        #endregion

        #region 合并单元格

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="firstRow">开始行数（从1开始计数）</param>
        /// <param name="lastRow">结束行数（从1开始计数）</param>
        /// <param name="firstColumn">开始列数（从1开始计数）</param>
        /// <param name="lastColumn">结束列数（从1开始计数）</param>
        /// <param name="borderTypeStr">边框样式（默认细线）</param>
        /// <param name="borderColorStr">边框颜色（默认黑色）</param>
        /// <returns></returns>
        public static ISheet MergedRegion(this ISheet sheet, int firstRow, int lastRow, int firstColumn, int lastColumn)
        {
            var region = GetCellRegion(firstRow, lastRow, firstColumn, lastColumn);
            sheet.MergedRegion(region);
            return sheet;
        }

        public static ISheet MergedRegion(this ISheet sheet, CellRangeAddress region)
        {
            sheet.AddMergedRegion(region);
            //SetRegionBorder(sheet, region, borderTypeStr, borderColorStr);
            return sheet;
        }

        public static CellRangeAddress GetCellRegion(this int firstRow, int lastRow, int firstColumn, int lastColumn)
        {
            firstRow = firstRow <= 0 ? 1 : firstRow;
            firstColumn = firstColumn <= 0 ? 1 : firstColumn;
            lastRow = lastRow <= 0 ? 1 : lastRow;
            lastColumn = lastColumn <= 0 ? 1 : lastColumn;
            return new CellRangeAddress(firstRow - 1, lastRow - 1, firstColumn - 1, lastColumn - 1);
        }

        public static ISheet SetRegionBorder(this CellRangeAddress region, ISheet sheet, string borderTypeStr, string borderColorStr)
        {
            ICellStyle style = sheet.Workbook.CreateCellStyle();
            borderTypeStr = borderTypeStr ?? "Thin";
            borderColorStr = borderColorStr ?? "Black";
            style.BorderColors(borderColorStr);
            style.BorderTypes(borderTypeStr);
            for (int i = region.FirstRow; i <= region.LastRow; i++)
            {
                IRow row = sheet.GenerateRow(i + 1);
                if (i == region.FirstRow)
                {

                }
                else if (i == region.LastRow)
                {

                }

                var leftCell = row.GenerateCell(region.FirstColumn + 1);

                leftCell.CellStyle.BorderLeft = style.BorderLeft;
                leftCell.CellStyle.LeftBorderColor = style.LeftBorderColor;

                var rightCell = row.GenerateCell(region.LastColumn + 1);
                rightCell.CellStyle.BorderRight = style.BorderRight;
                rightCell.CellStyle.RightBorderColor = style.RightBorderColor;
                //for (int j = region.FirstColumn; j <= region.LastColumn; j++)
                //{
                //    ICell singleCell = row.GenerateCell((short)j);
                //}
            }

            return sheet;
        }

        #endregion

        public static ICellStyle GetCellStyle(this ICell cell)
        {
            return cell.CellStyle ?? cell.Sheet.Workbook.CreateCellStyle();
        }
        public static void SetCellStyle(this ICell cell, ICellStyle cellStyle)
        {
            cell.CellStyle = cellStyle;
        }
        public static ICell SetCellStyleAndValue(this ICell cell, ICellStyle cellStyle, string obj)
        {
            cell.CellStyle = cellStyle;
            return SetValue<string>(cell, obj);
        }
        public static ICell SetCellStyleAndValue<T>(this ICell cell, ICellStyle cellStyle,object obj)
        {
            cell.CellStyle = cellStyle;
            return SetValue<T>(cell, obj);
        }
        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="sheet">表</param>
        /// <param name="startRow">第几行后开始插入（源行）</param>
        /// <param name="count">插入的行数</param>
        /// <returns></returns>
        public static void InsertRows(this ISheet sheet, int startRow, int count)
        {
            var rowSource = sheet.GenerateRow(startRow);
            sheet.ShiftRows(startRow, sheet.LastRowNum, count, true, false);
            if (rowSource == null)
                return;
            var rowStyle = rowSource.RowStyle;
            for (int i = startRow + 1; i <= startRow + count; i++)
            {
                var rowInsert = sheet.GenerateRow(i);
                rowInsert.Height = rowSource.Height;
                if (rowStyle!=null)
                {
                    rowInsert.RowStyle = rowStyle;
                }
                for (int col = 1; col <= rowSource.LastCellNum; col++)
                {
                    var cellSource = rowSource.GenerateCell(col);
                    var cellInsert = rowInsert.GenerateCell(col);
                    var cellStyle = cellSource.CellStyle;
                    //设置单元格样式　　　　
                    if (cellStyle != null)
                        cellInsert.CellStyle = cellSource.CellStyle;
                }
            }
        }
        /// <summary>
        /// 保存工作簿
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string SaveWorkBook(this ISheet sheet, string filePath, string fileName)
        {
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath.EndsWith("\\") ? filePath : filePath + "\\";
                FileStream file = new FileStream(filePath + fileName, FileMode.Create);
                sheet.Workbook.Write(file);
                file.Close();
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 保存工作簿
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string SaveWorkBook(this IWorkbook workbook, string filePath, string fileName)
        {
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                filePath = filePath.EndsWith("\\") ? filePath : filePath + "\\";
                FileStream file = new FileStream(filePath + fileName, FileMode.Create);
                workbook.Write(file);
                file.Close();
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }

    public class CellStyleCss
    {
        public static CellStyleCss Instants => new CellStyleCss()
        {
            //缩写
            DefaultStyle = $"bgc:{ColorType.White.ToString()};" +
                           $"warp:{HorizontalAlignment.Center.ToString()};" +
                           $"align:{HorizontalAlignment.Center.ToString()};" +
                           $"v-align:{VerticalAlignment.Center.ToString()};" +
                           $"b:{BorderStyle.Dotted.ToString()};" +
                           $"bc:{ColorType.Black.ToString()};" +
                           "inden:0;" +
                           "df:@;"
        };

        public void SetDefaultStyle(string styleStr)
        {
            var dic = new SortedDictionary<string, string>();
            DefaultStyle = dic.GetCleanStyle("");
            DefaultStyle = dic.GetCleanStyle(styleStr);
        }

        /// <summary>
        /// 默认样式css
        /// </summary>
        private string DefaultStyle { get; set; }

        //标准写法
        //private static readonly  string DefaultFontStyle = "font-color:black;" +
        //                                        "font-name:Arial;" +
        //                                        "font-size:10;" +
        //                                        "font-weight:normal;" +
        //                                        "font-underline:none;" +
        //                                        "font-italic:false;" +
        //                                        "font-strikeout:false;" +
        //                                        "font-superscript:none;"+
        //                                        "background-color:white;"+
        //                                        "text-align:none;"+
        //                                        "vertical-align:none;"+
        //                                        "data-format:none;border-type:Thin";

        /// <summary>
        /// 把css样式设置给单元格
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="styleStr"></param>
        /// <returns></returns>
        public ICell Css(ICell cell, string styleStr = null)
        {
            var dic = new SortedDictionary<string, string>();
            var sortedCss = dic.GetCleanStyle(DefaultStyle);
            if (!string.IsNullOrEmpty(styleStr))
                sortedCss = dic.GetCleanStyle(styleStr);
            var cssKey = $"CellStyle_{sortedCss.Md5()}";
            var workbook = cell.Sheet.Workbook;
            ICellStyle cellStyle = workbook.GetCellStyle(cssKey);
            if (cellStyle == null)
            {
                cellStyle = workbook.GetCellStyle(dic, cell);
                workbook.AttachedCellStyle(cssKey, cellStyle);
            }
            cell.CellStyle = cellStyle;
            //cell.CellStyle = workbook.GetCellStyle(dic, cell);
            return cell;//返回单元格方便流水式编程
        }
        public ICellStyle GetCssStyle(IWorkbook workbook, string styleStr)
        {
            var dic = new SortedDictionary<string, string>();
            var sortedCss = dic.GetCleanStyle(DefaultStyle);
            if (!string.IsNullOrEmpty(styleStr))
                sortedCss = dic.GetCleanStyle(styleStr);
            var cssKey = $"CellStyle_{sortedCss.Md5()}";
            var cellStyle = workbook.GetCellStyle(cssKey);
            if (cellStyle == null)
            {
                cellStyle = workbook.GetCellStyle(dic);
                workbook.AttachedCellStyle(cssKey, cellStyle);
            }
            return cellStyle;
        }

    }

    internal static class CellStyleRender
    {

        #region 解析css样式

        /// <summary>
        /// 默认字体样式
        /// </summary>
        private static string DefaultFontStyle { get; } = $"fc:{ColorType.Black.ToString()};" +//font-color
                                                          "fn:宋体;" +//font-name
                                                          "fs:12;" +//font-size
                                                          "fw:normal;" +//font-weight
                                                          "fu:none;" +//font-underline
                                                          "fi:false;" +//font-italic
                                                          "fst:false;" +//font-strikeout
                                                          "fss:none;";//font-superscript

        #region 设置样式

        /// <summary>
        /// 缓存
        /// </summary>
        private static readonly ConditionalWeakTable<IWorkbook, Dictionary<string, ICellStyle>> Table =
            new ConditionalWeakTable<IWorkbook, Dictionary<string, ICellStyle>>();

        /// <summary>
        /// 获取CellStyle
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="dic"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static ICellStyle GetCellStyle(this IWorkbook workbook, SortedDictionary<string, string> dic, ICell cell = null)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            //if (cell != null) 
            //{
            //    cellStyle.CloneStyleFrom(cell.CellStyle);
            //}
            var fontStyles = dic.Where(w => w.Key.StartsWith("font-")).ToArray();
            var fontDic = new SortedDictionary<string, string>();
            foreach (var kv in fontStyles)
            {
                fontDic.Add(kv.Key, kv.Value);
            }
            var font = workbook.GetFont(fontDic);
            cellStyle.SetFont(font);//TODO 在基于style.xls基础的样式上增加css时，会造成原字体设置的丢失
            var xdic = dic.Where(w => !w.Key.StartsWith("font-")).ToArray();
            foreach (var kvp in xdic)
            {
                FireCssAccess(cellStyle, workbook, kvp);
            }
            return cellStyle;
        }



        /// <summary>
        /// 从缓存读取CellStyle
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static ICellStyle GetCellStyle(this IWorkbook workbook, string propertyName)
        {
            if (!Table.TryGetValue(workbook, out var values)) return null;
            if (values.TryGetValue(propertyName, out var temp))
                return temp;
            return null;
        }

        /// <summary>
        /// 缓存CellStyle
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void AttachedCellStyle(this IWorkbook workbook, string propertyName, ICellStyle value)
        {
            if (!Table.TryGetValue(workbook, out var values))
            {
                values = new Dictionary<string, ICellStyle>();
                Table.Add(workbook, values);
            }
            values[propertyName] = value;
        }

        /// <summary>
        /// Md5 key
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5(this string input)
        {
            if (input == null)
                input = string.Empty;
            byte[] data = Encoding.UTF8.GetBytes(input.Trim().ToLowerInvariant());
            using (var md5 = new MD5CryptoServiceProvider())
            {
                data = md5.ComputeHash(data);
            }

            var ret = new StringBuilder();
            foreach (byte b in data)
            {
                ret.Append(b.ToString("x2").ToLowerInvariant());
            }
            return ret.ToString();
        }

        /// <summary>
        /// 设置不是字体的样式
        /// </summary>
        /// <param name="style"></param>
        /// <param name="workbook"></param>
        /// <param name="kvp"></param>
        private static void FireCssAccess(ICellStyle style, IWorkbook workbook, KeyValuePair<string, string> kvp)
        {
            switch (kvp.Key)
            {
                case "WrapText":
                    style.TextWrap(kvp.Value);
                    break;
                case "Indention":
                    style.TextIndention(kvp.Value);
                    break;
                case "text-align":
                    style.TextAlign(kvp.Value);
                    break;
                case "vertical-align":
                    style.VerticalAlign(kvp.Value);
                    break;
                case "background-color":
                    style.BackgroundColor(kvp.Value);
                    break;
                case "border-type":
                    style.BorderTypes(kvp.Value);
                    break;
                case "top-border-type":
                    style.BorderTopTypes(kvp.Value);
                    break;
                case "right-border-type":
                    style.BorderRightTypes(kvp.Value);
                    break;
                case "bottom-border-type":
                    style.BorderBottomTypes(kvp.Value);
                    break;
                case "left-border-type":
                    style.BorderLeftTypes(kvp.Value);
                    break;
                case "border-color":
                    style.BorderColors(kvp.Value);
                    break;
                case "top-border-color":
                    style.BorderTopColors(kvp.Value);
                    break;
                case "right-border-color":
                    style.BorderRightColors(kvp.Value);
                    break;
                case "bottom-border-color":
                    style.BorderBottomColors(kvp.Value);
                    break;
                case "left-border-color":
                    style.BorderLeftColors(kvp.Value);
                    break;
                case "data-format":
                    style.DataFormat(workbook, kvp.Value);
                    break;
            }
        }

        /// <summary>
        /// 获取字体样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="fontdic"></param>
        /// <returns></returns>
        private static IFont GetFont(this IWorkbook workbook, SortedDictionary<string, string> fontdic)
        {
            var weight = fontdic.FontWeight();
            var color = fontdic.FontColor();
            var size = fontdic.FontSize();
            var name = fontdic.FontName();
            var underline = fontdic.FontUnderline();
            var italic = fontdic.FontItalic();
            var strikeout = fontdic.FontStrikeout();
            var offset = fontdic.ConvertToSuperScript();

            var findHeight = (short)(size * 20);
            var font = workbook.FindFont(weight, color, findHeight, name, italic, strikeout, offset, underline);
            if (font == null)
            {
                font = workbook.CreateFont();
                font.Boldweight = weight;
                font.Color = color;
                font.FontHeightInPoints = size;
                font.FontName = name;
                font.Underline = underline;
                font.IsItalic = italic;
                font.IsStrikeout = strikeout;
                font.TypeOffset = offset;
            }
            return font;
        }

        #endregion

        #region 获取样式

        /// <summary>
        /// 默认设置
        /// </summary>
        /// <param name="dic"></param>
        public static void InitStyleDic(this SortedDictionary<string, string> dic)
        {
            var cssItems = GetCssItems(DefaultFontStyle);

            foreach (var cssitem in cssItems)
            {
                var kvp = GetCssKeyValue(cssitem);
                if (dic.ContainsKey(kvp.Key))
                    dic[kvp.Key] = kvp.Value; //覆盖相同key的值
                else
                    dic.Add(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// 获取样式简洁字符串
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static string GetCleanStyle(this SortedDictionary<string, string> dic, string style)
        {

            style = Regex.Replace(style.Trim(), "\\s+", " ");
            style = Regex.Replace(style, "\\s;\\s", ";");
            style = Regex.Replace(style, "\\s:\\s", ":");
            InitStyleDic(dic);
            var cssItems = GetCssItems(style.TrimEnd(';'));
            foreach (var cssitem in cssItems)
            {
                var kvp = GetCssKeyValue(cssitem);
                if (dic.ContainsKey(kvp.Key))
                    dic[kvp.Key] = kvp.Value; //覆盖相同key的值
                else
                    dic.Add(kvp.Key, kvp.Value);
            }

            var sortedCss = string.Join(";", dic.Select(s => $"{s.Key}:{s.Value}").ToArray());
            return sortedCss;
        }

        /// <summary>
        /// 获取样式数组
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private static string[] GetCssItems(string style)
        {
            var cssItems = Regex.Split(style, ";");
            cssItems = cssItems.Where(w => !string.IsNullOrWhiteSpace(w)).ToArray();
            return cssItems;
        }

        /// <summary>
        /// 获取css样式
        /// </summary>
        /// <param name="css"></param>
        /// <returns></returns>
        private static KeyValuePair<string, string> GetCssKeyValue(string css)
        {
            var cssKeyValueArray = Regex.Split(css, ":").ToArray();
            var cssKey = cssKeyValueArray[0].StandardCssKey();
            var cssValue = cssKey == "font-name" ? cssKeyValueArray[1] : cssKeyValueArray[1].ToUpper(); //字体不应变大写
            var kv = new KeyValuePair<string, string>(cssKey, cssValue);
            return kv;
        }

        #endregion

        #region 转换Css的 Key

        /// <summary>
        /// 缩写Key 转换成标准Key
        /// </summary>
        /// <param name="csskey"></param>
        /// <returns></returns>
        private static string StandardCssKey(this string csskey)
        {
            if (CssKeyDic.ContainsKey(csskey))
            {
                var sKey = CssKeyDic[csskey];
                return sKey;
            }
            return csskey;
        }
        /// <summary>
        /// key 转换字典
        /// </summary>
        private static Dictionary<string, string> CssKeyDic => new Dictionary<string, string>
        {
            {"color", "font-color"},
            {"fc", "font-color"},
            {"fw", "font-weight"},
            {"fn", "font-name"},
            {"fs", "font-size"},
            {"italic", "font-italic"},
            {"fi", "font-italic"},
            {"underline", "font-underline"},
            {"fu", "font-underline"},
            {"u", "font-underline"},
            {"deleteline", "font-strikeout"},
            {"d-line", "font-strikeout"},
            {"strikeout", "font-strikeout"},
            {"fst", "font-strikeout"},
            {"d", "font-strikeout"},
            {"font-offset", "font-superscript"},
            {"superscript", "font-superscript"},
            {"fss", "font-superscript"},
            {"ss", "font-superscript"},
            {"bg-color", "background-color"},
            {"bg-c", "background-color"},
            {"bgc", "background-color"},
            {"align", "text-align"},
            {"wrap", "WrapText"},
            {"inden", "Indention"},
            {"in", "Indention"},
            {"v-align", "vertical-align"},
            {"b-t", "border-type"},
            {"b", "border-type"},
            {"bt", "top-border-type"},
            {"br", "right-border-type"},
            {"bb", "bottom-border-type"},
            {"bl", "left-border-type"},
            {"b-c", "border-color"},
            {"bc", "border-color"},
            {"btc", "top-border-color"},
            {"brc", "right-border-color"},
            {"bbc", "bottom-border-color"},
            {"blc", "left-border-color"},
            {"format", "data-format"},
            {"df", "data-format"}
        };

        #endregion


        #region 样式转换

        #region font-weight

        private static short FontWeight(this SortedDictionary<string, string> fontdic)
        {
            switch (fontdic["font-weight"])
            {
                case "NORMAL":
                    return 400;

                case "BOLD":
                    return 700;
                default:
                    return 0;
            }
        }

        #endregion font-weight

        #region font-name
        private static string FontName(this SortedDictionary<string, string> fontdic)
        {
            return fontdic["font-name"];
        }

        #endregion font-name

        #region font-size

        private static short FontSize(this SortedDictionary<string, string> fontdic)
        {
            return short.TryParse(fontdic["font-size"], out var value) ? value : (short)10;
        }

        #endregion font-size

        #region font-color

        private static short FontColor(this SortedDictionary<string, string> fontdic)
        {
            var color = fontdic["font-color"].ConvertToColor();
            return color;
        }

        #endregion font-color

        #region font-italic

        private static bool FontItalic(this SortedDictionary<string, string> fontdic)
        {
            return fontdic["font-italic"] == "TRUE";
        }

        #endregion font-italic

        #region font-strikeout
        /// <summary>
        /// 删除线
        /// </summary>
        /// <param name="fontdic"></param>
        /// <returns></returns>
        private static bool FontStrikeout(this SortedDictionary<string, string> fontdic)
        {
            return fontdic["font-strikeout"] == "TRUE";
        }

        #endregion font-strikeout

        #region WrapText 

        private static void TextWrap(this ICellStyle style, string v)
        {
            style.WrapText = v.ToUpper() == "TRUE";
        }

        #endregion WrapText

        #region TextIndention  
        /// <summary>
        /// 缩进
        /// </summary>
        /// <param name="style"></param>
        /// <param name="v"></param>
        private static void TextIndention(this ICellStyle style, string v)
        {
            if (short.TryParse(v, out var value))
            {
                style.Indention = value;
            }
        }

        #endregion WrapText

        #region text-align

        private static void TextAlign(this ICellStyle style, string v)
        {
            style.Alignment = v.ConvertToHorizontalAlignment();
        }

        #endregion text-align

        #region vertical-align

        private static void VerticalAlign(this ICellStyle style, string v)
        {
            style.VerticalAlignment = v.ConvertToVerticalAlignment();
        }

        #endregion vertical-align

        #region boder-type / boder-color
        internal static void BorderTypes(this ICellStyle style, string v)
        {
            if (string.IsNullOrEmpty(v))
                return;
            string[] borderTypeNames = { string.Empty, string.Empty, string.Empty, string.Empty };
            v = v.ToUpper();
            var vs = v.Split(' ');
            switch (vs.Length)
            {
                case 1:
                    borderTypeNames[0] = borderTypeNames[1] = borderTypeNames[2] = borderTypeNames[3] = vs[0];
                    break;
                case 2:
                    borderTypeNames[0] = borderTypeNames[2] = vs[0];
                    borderTypeNames[1] = borderTypeNames[3] = vs[1];
                    break;
                case 3:
                    borderTypeNames[0] = vs[0];
                    borderTypeNames[1] = borderTypeNames[3] = vs[1];
                    borderTypeNames[2] = vs[2];
                    break;
                case 4:
                    borderTypeNames[0] = vs[0];
                    borderTypeNames[1] = vs[1];
                    borderTypeNames[2] = vs[2];
                    borderTypeNames[3] = vs[3];
                    break;
            }
            var borderTopTypeName = borderTypeNames[0];
            var borderRightTypeName = borderTypeNames[1];
            var borderBottomTypeName = borderTypeNames[2];
            var borderLeftTypeName = borderTypeNames[3];

            if (!string.IsNullOrWhiteSpace(borderTopTypeName))
                style.BorderTop = borderTopTypeName.ConvertToBorderStyle();
            if (!string.IsNullOrWhiteSpace(borderRightTypeName))
                style.BorderRight = borderRightTypeName.ConvertToBorderStyle();
            if (!string.IsNullOrWhiteSpace(borderBottomTypeName))
                style.BorderBottom = borderBottomTypeName.ConvertToBorderStyle();
            if (!string.IsNullOrWhiteSpace(borderLeftTypeName))
                style.BorderLeft = borderLeftTypeName.ConvertToBorderStyle();
        }
        private static void BorderTopTypes(this ICellStyle style, string v)
        {
            if (!string.IsNullOrWhiteSpace(v))
                style.BorderTop = v.ConvertToBorderStyle();
        }
        private static void BorderBottomTypes(this ICellStyle style, string v)
        {
            if (!string.IsNullOrWhiteSpace(v))
                style.BorderBottom = v.ConvertToBorderStyle();
        }
        private static void BorderLeftTypes(this ICellStyle style, string v)
        {
            if (!string.IsNullOrWhiteSpace(v))
                style.BorderLeft = v.ConvertToBorderStyle();
        }
        private static void BorderRightTypes(this ICellStyle style, string v)
        {
            if (!string.IsNullOrWhiteSpace(v))
                style.BorderRight = v.ConvertToBorderStyle();
        }
        internal static void BorderColors(this ICellStyle style, string v)
        {
            if (string.IsNullOrEmpty(v))
                return;
            string[] borderColors = { string.Empty, string.Empty, string.Empty, string.Empty };
            v = v.ToUpper();
            var vs = v.Split(' ');
            switch (vs.Length)
            {
                case 1:
                    borderColors[0] = borderColors[1] = borderColors[2] = borderColors[3] = vs[0];
                    break;
                case 2:
                    borderColors[0] = borderColors[2] = vs[0];
                    borderColors[1] = borderColors[3] = vs[1];
                    break;
                case 3:
                    borderColors[0] = vs[0];
                    borderColors[1] = borderColors[3] = vs[1];
                    borderColors[2] = vs[2];
                    break;
                case 4:
                    borderColors[0] = vs[0];
                    borderColors[1] = vs[1];
                    borderColors[2] = vs[2];
                    borderColors[3] = vs[3];
                    break;
            }
            var borderTopColor = borderColors[0];
            var borderRightColor = borderColors[1];
            var borderBottomColor = borderColors[2];
            var borderLeftColor = borderColors[3];

            if (!string.IsNullOrWhiteSpace(borderTopColor))
                style.TopBorderColor = borderTopColor.ConvertToColor();
            if (!string.IsNullOrWhiteSpace(borderRightColor))
                style.RightBorderColor = borderRightColor.ConvertToColor();
            if (!string.IsNullOrWhiteSpace(borderBottomColor))
                style.BottomBorderColor = borderBottomColor.ConvertToColor();
            if (!string.IsNullOrWhiteSpace(borderLeftColor))
                style.LeftBorderColor = borderLeftColor.ConvertToColor();
        }

        private static void BorderTopColors(this ICellStyle style, string v)
        {
            if (!string.IsNullOrWhiteSpace(v))
                style.TopBorderColor = v.ConvertToColor();
        }
        private static void BorderBottomColors(this ICellStyle style, string v)
        {
            if (!string.IsNullOrWhiteSpace(v))
                style.BottomBorderColor = v.ConvertToColor();
        }
        private static void BorderLeftColors(this ICellStyle style, string v)
        {
            if (!string.IsNullOrWhiteSpace(v))
                style.LeftBorderColor = v.ConvertToColor();
        }
        private static void BorderRightColors(this ICellStyle style, string v)
        {
            if (!string.IsNullOrWhiteSpace(v))
                style.RightBorderColor = v.ConvertToColor();
        }
        #endregion boder-type

        #region data-format

        private static void DataFormat(this ICellStyle style, IWorkbook workbook, string v)
        {
            if (string.IsNullOrEmpty(v))
                return;
            var df = workbook.CreateDataFormat();
            style.DataFormat = df.GetFormat(v);
        }

        #endregion data-format

        #region BackgroundColor

        private static void BackgroundColor(this ICellStyle style, string v)
        {
            if (string.IsNullOrEmpty(v))
                return;
            style.FillPattern = FillPattern.SolidForeground;
            style.FillForegroundColor = v.ConvertToColor();
        }

        #endregion

        private static FontSuperScript ConvertToSuperScript(this SortedDictionary<string, string> fontdic)
        {
            var v = fontdic["font-superscript"];

            switch (v)
            {
                case "SUPER":
                    return FontSuperScript.Super;

                case "SUB":
                    return FontSuperScript.Sub;

                default:
                    return FontSuperScript.None;
            }
        }

        private static FontUnderlineType FontUnderline(this SortedDictionary<string, string> fontdic)
        {
            var v = fontdic["font-underline"];
            switch (v)
            {
                case "SINGLE":
                    return FontUnderlineType.Single;

                case "DOUBLE":
                    return FontUnderlineType.Double;

                case "SINGLEACCOUNTING":
                case "SINGLE_ACCOUNTING":
                    return FontUnderlineType.SingleAccounting;

                case "DOUBLEACCOUNTING":
                case "DOUBLE_ACCOUNTING":
                    return FontUnderlineType.DoubleAccounting;

                default:
                    return FontUnderlineType.None;
            }
        }

        public static short ConvertToColor(this string v)
        {
            if (string.IsNullOrEmpty(v))
                return 32767;
            switch (v.ToUpper())
            {
                case "AQUA":
                    return (short)ColorType.Aqua;
                case "AUTOMATIC":
                    return (short)ColorType.Automatic;
                case "BLACK":
                    return (short)ColorType.Black;
                case "BLUE":
                    return (short)ColorType.Blue;
                case "BLUE_GREY":
                case "BLUEGREY":
                    return (short)ColorType.BlueGrey;
                case "BRIGHT_GREEN":
                case "BRIGHTGREEN":
                    return (short)ColorType.BrightGreen;
                case "BROWN":
                    return (short)ColorType.Brown;
                case "CORAL":
                    return (short)ColorType.Coral;
                case "CORNFLOWER_BLUE":
                case "CORNFLOWERBLUE":
                    return (short)ColorType.CornflowerBlue;
                case "DARK_BLUE":
                case "DARKBLUE":
                    return (short)ColorType.DarkBlue;
                case "DARK_GREEN":
                case "DARKGREEN":
                    return (short)ColorType.DarkGreen;
                case "DARK_RED":
                case "DARKRED":
                    return (short)ColorType.DarkRed;
                case "DARK_TEAL":
                case "DARKTEAL":
                    return (short)ColorType.DarkTeal;
                case "DARK_YELLOW":
                case "DARKYELLOW":
                    return (short)ColorType.DarkYellow;
                case "GOLD":
                    return (short)ColorType.Gold;
                case "GREEN":
                    return (short)ColorType.Green;
                case "GREY_25_PERCENT":
                case "GREY25PERCENT":
                    return (short)ColorType.Grey25Percent;
                case "GREY_40_PERCENT":
                case "GREY40PERCENT":
                    return (short)ColorType.Grey40Percent;
                case "GREY_50_PERCENT":
                case "GREY50PERCENT":
                    return (short)ColorType.Grey50Percent;
                case "GREY_80_PERCENT":
                case "GREY80PERCENT":
                    return (short)ColorType.Grey80Percent;
                case "INDIGO":
                    return (short)ColorType.Indigo;
                case "LAVENDER":
                    return (short)ColorType.Lavender;
                case "LEMON_CHIFFON":
                case "LEMONCHIFFON":
                    return (short)ColorType.LemonChiffon;
                case "LIGHT_BLUE":
                case "LIGHTBLUE":
                    return (short)ColorType.LightBlue;
                case "LIGHT_CORNFLOWERBLUE":
                case "LIGHTCORNFLOWERBLUE":
                    return (short)ColorType.LightCornflowerBlue;
                case "LIGHT_GREEN":
                case "LIGHTGREEN":
                    return (short)ColorType.LightGreen;
                case "LIGHT_ORANGE":
                case "LIGHTORANGE":
                    return (short)ColorType.LightOrange;
                case "LIGHT_TURQUOISE":
                case "LIGHTTURQUOISE":
                    return (short)ColorType.LightTurquoise;
                case "LIGHT_YELLOW":
                case "LIGHTYELLOW":
                    return (short)ColorType.LightYellow;
                case "LIME":
                    return (short)ColorType.Lime;
                case "MAROON":
                    return (short)ColorType.Maroon;
                case "OLIVE_GREEN":
                case "OLIVEGREEN":
                    return (short)ColorType.OliveGreen;
                case "ORANGE":
                    return (short)ColorType.Orange;
                case "ORCHID":
                    return (short)ColorType.Orchid;
                case "PALE_BLUE":
                case "PALEBLUE":
                    return (short)ColorType.PaleBlue;
                case "PINK":
                    return (short)ColorType.Pink;
                case "PLUM":
                    return (short)ColorType.Plum;
                case "RED":
                    return (short)ColorType.Red;
                case "ROSE":
                    return (short)ColorType.Rose;
                case "ROYAL_BLUE":
                case "ROYALBLUE":
                    return (short)ColorType.RoyalBlue;
                case "SEA_GREEN":
                case "SEAGREEN":
                    return (short)ColorType.SeaGreen;
                case "SKY_BLUE":
                case "SKYBLUE":
                    return (short)ColorType.SkyBlue;
                case "TAN":
                    return (short)ColorType.Tan;
                case "TEAL":
                    return (short)ColorType.Teal;
                case "TURQUOISE":
                    return (short)ColorType.Turquoise;
                case "VIOLET":
                    return (short)ColorType.Violet;
                case "WHITE":
                    return (short)ColorType.White;
                case "YELLOW":
                    return (short)ColorType.Yellow;
                default:
                    return 32767;
            }
        }

        private static HorizontalAlignment ConvertToHorizontalAlignment(this string v)
        {
            if (string.IsNullOrEmpty(v))
                return HorizontalAlignment.General;
            switch (v.ToUpper())
            {
                case "LEFT":
                    return HorizontalAlignment.Left;

                case "CENTER":
                    return HorizontalAlignment.Center;

                case "CENTERSELECTION":
                case "CENTER_SELECTION":
                    return HorizontalAlignment.CenterSelection;

                case "RIGHT":
                    return HorizontalAlignment.Right;

                case "DISTRIBUTED":
                    return HorizontalAlignment.Distributed;

                case "FILL":
                    return HorizontalAlignment.Fill;

                case "JUSTIFY":
                    return HorizontalAlignment.Justify;

                default:
                    return HorizontalAlignment.General;
            }
        }

        private static VerticalAlignment ConvertToVerticalAlignment(this string v)
        {
            if (string.IsNullOrEmpty(v))
                return VerticalAlignment.Justify;
            switch (v.ToUpper())
            {
                case "TOP":
                    return VerticalAlignment.Top;

                case "CENTER":
                    return VerticalAlignment.Center;

                case "BOTTOM":
                    return VerticalAlignment.Bottom;

                case "DISTRIBUTED":
                    return VerticalAlignment.Distributed;

                default:
                    return VerticalAlignment.Justify;
            }
        }

        public static BorderStyle ConvertToBorderStyle(this string v)
        {
            if (string.IsNullOrEmpty(v))
                return BorderStyle.None;
            switch (v.ToUpper())
            {
                case "THIN":
                    return BorderStyle.Thin;

                case "MEDIUM":
                    return BorderStyle.Medium;

                case "DASHED":
                    return BorderStyle.Dashed;

                case "HAIR":
                    return BorderStyle.Hair;

                case "THICK":
                    return BorderStyle.Thick;

                case "DOUBLE":
                    return BorderStyle.Double;

                case "DOTTED":
                    return BorderStyle.Dotted;

                case "MEDIUMDASHED":
                case "MEDIUM_DASHED":
                    return BorderStyle.MediumDashed;

                case "DASHDOT":
                case "DASH_DOT":
                    return BorderStyle.DashDot;

                case "MEDIUMDASHDOT":
                case "MEDIUM_DASH_DOT":
                    return BorderStyle.MediumDashDot;

                case "DASHDOTDOT":
                case "DASH_DOT_DOT":
                    return BorderStyle.DashDotDot;

                case "MEDIUMDASHDOTDOT":
                case "MEDIUM_DASH_DOT_DOT":
                    return BorderStyle.MediumDashDotDot;

                case "SLANTEDDASHDOT":
                case "SLANTED_DASH_DOT":
                    return BorderStyle.SlantedDashDot;

                default:
                    return BorderStyle.None;
            }
        }

        #endregion

        #endregion

    }

    public enum ColorType
    {
        Black = HSSFColor.Black.Index,
        Brown = HSSFColor.Brown.Index,
        OliveGreen = HSSFColor.OliveGreen.Index,
        DarkGreen = HSSFColor.DarkGreen.Index,
        DarkTeal = HSSFColor.DarkTeal.Index,
        DarkBlue = HSSFColor.DarkBlue.Index,
        Indigo = HSSFColor.Indigo.Index,
        Grey80Percent = HSSFColor.Grey80Percent.Index,
        Orange = HSSFColor.Orange.Index,
        DarkYellow = HSSFColor.DarkYellow.Index,
        Green = HSSFColor.Green.Index,
        Teal = HSSFColor.Teal.Index,
        Blue = HSSFColor.Blue.Index,
        BlueGrey = HSSFColor.BlueGrey.Index,
        Grey50Percent = HSSFColor.Grey50Percent.Index,
        Red = HSSFColor.Red.Index,
        LightOrange = HSSFColor.LightOrange.Index,
        Lime = HSSFColor.Lime.Index,
        SeaGreen = HSSFColor.SeaGreen.Index,
        Aqua = HSSFColor.Aqua.Index,
        LightBlue = HSSFColor.LightBlue.Index,
        Violet = HSSFColor.Violet.Index,
        Grey40Percent = HSSFColor.Grey40Percent.Index,
        Pink = HSSFColor.Pink.Index,
        Gold = HSSFColor.Gold.Index,
        Yellow = HSSFColor.Yellow.Index,
        BrightGreen = HSSFColor.BrightGreen.Index,
        Turquoise = HSSFColor.Turquoise.Index,
        DarkRed = HSSFColor.DarkRed.Index,
        SkyBlue = HSSFColor.SkyBlue.Index,
        Plum = HSSFColor.Plum.Index,
        Grey25Percent = HSSFColor.Grey25Percent.Index,
        Rose = HSSFColor.Rose.Index,
        LightYellow = HSSFColor.LightYellow.Index,
        LightGreen = HSSFColor.LightGreen.Index,
        LightTurquoise = HSSFColor.LightTurquoise.Index,
        PaleBlue = HSSFColor.PaleBlue.Index,
        Lavender = HSSFColor.Lavender.Index,
        White = HSSFColor.White.Index,
        CornflowerBlue = HSSFColor.CornflowerBlue.Index,
        LemonChiffon = HSSFColor.LemonChiffon.Index,
        Maroon = HSSFColor.Maroon.Index,
        Orchid = HSSFColor.Orchid.Index,
        Coral = HSSFColor.Coral.Index,
        RoyalBlue = HSSFColor.RoyalBlue.Index,
        LightCornflowerBlue = HSSFColor.LightCornflowerBlue.Index,
        Tan = HSSFColor.Tan.Index,
        Automatic = HSSFColor.Automatic.Index
    }




    public static class TransExp<TIn, TOut>
    {

        private static readonly Func<TIn, TOut> Cache = GetFunc();
        private static Func<TIn, TOut> GetFunc()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();

            foreach (var item in typeof(TOut).GetProperties())
            {
                if (!item.CanWrite)
                    continue;

                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name) ?? throw new InvalidOperationException());
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

            return lambda.Compile();
        }

        public static TOut Trans(TIn tIn)
        {
            return Cache(tIn);
        }

    }

    public class ToExcelObj
    {
        public string ShowColumn { get; set; }

        public string MapColumn { get; set; }

        public string StyleStr { get; set; }
    }


   
}
