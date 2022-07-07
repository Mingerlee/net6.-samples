using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;

namespace Infrastructure.Utilities
{
    public class ExcelHelper
    {
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="sWebRootFolder">webRoot文件夹</param>
        /// <param name="sFileName">文件名</param>
        /// <param name="sColumnName">自定义列名（不传默认dt列名）</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] ExportExcel(DataTable dt, string sWebRootFolder, string sFileName, string[] sColumnName, ref string msg)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    msg = "没有符合条件的数据！";
                    //  return false;
                }
                //转utf-8
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] buffer = utf8.GetBytes(sFileName);
                sFileName = utf8.GetString(buffer);
                //判断文件夹
                sWebRootFolder = Path.Combine(sWebRootFolder, "ExprotExcel");
                if (!Directory.Exists(sWebRootFolder))
                    Directory.CreateDirectory(sWebRootFolder);
                //删除大于7天的文件
                string[] files = Directory.GetFiles(sWebRootFolder, "*.xlsx", SearchOption.AllDirectories);
                foreach (string item in files)
                {

                    FileInfo f = new FileInfo(item);
                    DateTime now = DateTime.Now;
                    TimeSpan t = now - f.CreationTime;
                    int day = t.Days;
                    if (day > 7)
                    {
                        File.Delete(item);
                    }
                }
                //判断同名文件
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    //判断同名文件创建时间
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }
                //指定EPPlus使用非商业证书
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    //添加worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sFileName.Split('.')[0]);
                    //添加表头
                    int column = 1;
                    if (sColumnName.Count() == dt.Columns.Count)
                    {
                        foreach (string cn in sColumnName)
                        {
                            worksheet.Cells[1, column].Value = cn.Trim();
                            worksheet.Cells[1, column].Style.Font.Bold = true;//字体为粗体
                            worksheet.Cells[1, column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;//水平居中
                            worksheet.Cells[1, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;//设置样式类型
                            worksheet.Cells[1, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(159, 197, 232));//设置单元格背景色
                            column++;
                        }
                    }
                    else
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            worksheet.Cells[1, column].Value = dc.ColumnName;
                            worksheet.Cells[1, column].Style.Font.Bold = true;//字体为粗体
                            worksheet.Cells[1, column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;//水平居中
                            worksheet.Cells[1, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;//设置样式类型
                            worksheet.Cells[1, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(159, 197, 232));//设置单元格背景色
                            column++;
                        }
                    }
                    //添加数据
                    int row = 2;
                    foreach (DataRow dr in dt.Rows)
                    {
                        int col = 1;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            worksheet.Cells[row, col].Value = dr[col - 1].ToString();
                            col++;
                        }
                        row++;
                    }
                    //自动列宽
                    worksheet.Cells.AutoFitColumns();
                    //保存
                    package.Save();
                    return package.GetAsByteArray();
                    //MemoryStream file = new MemoryStream();
                    //package.SaveAs(file);
                    //file.Seek(0, SeekOrigin.Begin);

                    //return file;
                }
            }
            catch (Exception ex)
            {
                msg = "生成Excel失败：" + ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="sWebRootFolder">webRoot文件夹</param>
        /// <param name="sFileName">文件名</param>
        /// <param name="sColumnName">自定义列名（不传默认dt列名）</param>
        /// <param name="msg"></param>
        /// <param name="lst">合并的表头参数集合</param>
        /// <returns></returns>
        public static byte[] ExportExcel(DataTable dt, string sWebRootFolder, string sFileName, string[] sColumnName, ref string msg, List<ExcelTableHeader> lst)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    msg = "没有符合条件的数据！";
                    //  return false;
                }
                //转utf-8
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] buffer = utf8.GetBytes(sFileName);
                sFileName = utf8.GetString(buffer);
                //判断文件夹
                sWebRootFolder = Path.Combine(sWebRootFolder, "ExprotExcel");
                if (!Directory.Exists(sWebRootFolder))
                    Directory.CreateDirectory(sWebRootFolder);
                //删除大于7天的文件
                string[] files = Directory.GetFiles(sWebRootFolder, "*.xlsx", SearchOption.AllDirectories);
                foreach (string item in files)
                {

                    FileInfo f = new FileInfo(item);
                    DateTime now = DateTime.Now;
                    TimeSpan t = now - f.CreationTime;
                    int day = t.Days;
                    if (day > 7)
                    {
                        File.Delete(item);
                    }
                }
                //判断同名文件
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    //判断同名文件创建时间
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }
                //指定EPPlus使用非商业证书
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    //添加worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sFileName.Split('.')[0]);
                    //添加表头
                    int column = 1;

                    #region 合并表头单元格
                    int CurrCol = 0;
                    for (int i = 1; i <= lst.Count; i++)
                    {
                        foreach (var item in lst)
                        {
                            if (i == item.TopColIndex)
                            {
                                if (item.TopColIndex == 1)
                                {
                                    worksheet.Cells[1, 1, 1, item.SecondColCount].Merge = true;
                                    worksheet.Cells[1, 1, 1, item.SecondColCount].Value = item.HeaderName;
                                }
                                else
                                {
                                    worksheet.Cells[1, CurrCol + 1, 1, item.SecondColCount + CurrCol].Merge = true;
                                    worksheet.Cells[1, CurrCol + 1, 1, item.SecondColCount + CurrCol].Value = item.HeaderName;
                                }
                                CurrCol += item.SecondColCount;
                            }
                        }
                    }
                    //设置高度
                    worksheet.Row(1).Height = 40;
                    worksheet.Cells[1, 1, 1, CurrCol].Style.Font.Bold = true;//字体为粗体
                    worksheet.Cells[1, 1, 1, CurrCol].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;//垂直居中
                    worksheet.Cells[1, 1, 1, CurrCol].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;//水平居中
                    #endregion

                    //worksheet.Cells[1, 1, 1, CurrCol].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;//设置样式类型
                    //worksheet.Cells[1, 1, 1, CurrCol].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(159, 197, 232));//设置单元格背景色
                    if (sColumnName.Count() == dt.Columns.Count)
                    {
                        foreach (string cn in sColumnName)
                        {
                            worksheet.Cells[2, column].Value = cn.Trim();
                            worksheet.Cells[2, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;//设置样式类型
                            worksheet.Cells[2, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(159, 197, 232));//设置单元格背景色
                            // worksheet.Cells[2, column].Style.Font.Bold = true;//字体为粗体
                            worksheet.Cells[2, column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;//水平居中
                            column++;
                        }
                    }
                    else
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            worksheet.Cells[2, column].Value = dc.ColumnName;
                            worksheet.Cells[2, column].Style.Font.Bold = true;//字体为粗体
                            worksheet.Cells[2, column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;//水平居中
                            worksheet.Cells[2, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;//设置样式类型
                            worksheet.Cells[2, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(159, 197, 232));//设置单元格背景色
                            column++;
                        }
                    }
                    //添加数据
                    int row = 3;
                    foreach (DataRow dr in dt.Rows)
                    {
                        int col = 1;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            worksheet.Cells[row, col].Value = dr[col - 1].ToString();
                            col++;
                        }
                        row++;
                    }
                    //自动列宽
                    worksheet.Cells.AutoFitColumns();
                    //保存
                    package.Save();
                    return package.GetAsByteArray();
                    //MemoryStream file = new MemoryStream();
                    //package.SaveAs(file);
                    //file.Seek(0, SeekOrigin.Begin);

                    //return file;
                }
            }
            catch (Exception ex)
            {
                msg = "生成Excel失败：" + ex.Message;
                return null;
            }
        }
    }
}
