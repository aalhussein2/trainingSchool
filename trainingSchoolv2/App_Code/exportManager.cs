using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

namespace trainingSchoolv2.App_Code
{
    public class exportManager
    {

        public static void ExportGridToExcel(GridView myGv) // working 1
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Charset = "";
            string FileName = "Exported" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            myGv.GridLines = GridLines.Both;
            myGv.HeaderStyle.Font.Bold = true;
            myGv.RenderControl(htmltextwrtter);
            HttpContext.Current.Response.Write(strwritter.ToString());
            HttpContext.Current.Response.End();
        }
        public static void ExportGridToWord(GridView myGv) // this works 2
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Charset = "";
            string FileName = "Exported" + DateTime.Now + ".doc";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = "application/msword";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            myGv.GridLines = GridLines.Both;
            myGv.HeaderStyle.Font.Bold = true;
            myGv.RenderControl(htmltextwrtter);
            HttpContext.Current.Response.Write(strwritter.ToString());
            HttpContext.Current.Response.End();
        }
        public static void ExportGridToPdf(GridView myGv)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    myGv.AllowPaging = false;
                    //...  this.showMember();
                    myGv.RenderControl(hw);
                    StringReader sr = new StringReader(sw.ToString());
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A2, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=members_File.pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    HttpContext.Current.Response.End();
                }
            }
        }
    }
}