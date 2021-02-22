
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using trainingSchoolv2.App_Code;

namespace trainingSchoolv2.school
{
    public partial class registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                populateGenderCombo();
                showMember();
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the run time error "  
            //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // get the values from the input form
            string strFName = txtFName.Text;
            string strMName = txtMName.Text;
            string strLName = txtLName.Text;
            string strCell = txtCell.Text;
            string strEmail = txtEmail.Text;
            int intGenderId = int.Parse(ddlGender.SelectedValue);
            //pass the values to a method to insert to the db
            CRUD myCrud = new CRUD();
            string mySql = @"INSERT INTO dbo.member
                        (fName, mName, lName, cell, email, genderId)
                        VALUES
                        (@fName, @mName, @lName, @cell, @email, @genderId)";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@fName", strFName);
            myPara.Add("@mName", strMName);
            myPara.Add("@lName", strLName);
            myPara.Add("@cell", strCell);
            myPara.Add("@email", strEmail);
            myPara.Add("@genderId", intGenderId);
             int rtn =  myCrud.InsertUpdateDelete(mySql, myPara);

            if (rtn >=1)
            { lblOutput.Text = "Success!"; }
            else
            { lblOutput.Text = "Failed!"; }
            // show fresh member information
            showMember();

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // write code to update record
            // get the values from the input form
            int memberId = int.Parse(txtMemberId.Text); 
            string strFName = txtFName.Text;
            string strMName = txtMName.Text;
            string strLName = txtLName.Text;
            string strCell = txtCell.Text;
            string strEmail = txtEmail.Text;
            int intGenderId = int.Parse(ddlGender.SelectedValue);
            //pass the values to a method to insert to the db
            CRUD myCrud = new CRUD();
            string mySql = @"update member set fName = @fName, mName = @mName, lName = @lName, cell=@cell, email = @email, 
                        genderid = @genderId
                        where memberid = @memberId";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@memberId", memberId);
            myPara.Add("@fName", strFName);
            myPara.Add("@mName", strMName);
            myPara.Add("@lName", strLName);
            myPara.Add("@cell", strCell);
            myPara.Add("@email", strEmail);
            myPara.Add("@genderId", intGenderId);
            int rtn = myCrud.InsertUpdateDelete(mySql, myPara);

            if (rtn >= 1)
            { lblOutput.Text = "Success!"; }
            else
            { lblOutput.Text = "Failed!"; }
            showMember();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int intMemberId = int.Parse(txtMemberId.Text);
            CRUD myCrud = new CRUD();
            string mySql = @"delete member where memberid = @memberId ";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@memberId", intMemberId);
           int rtn =  myCrud.InsertUpdateDelete(mySql, myPara);

            if (rtn >= 1)
            { lblOutput.Text = "Success!"; }
            else
            { lblOutput.Text = "Failed!"; }

            // call to update the gridview 
            showMember();
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            showMember();
        }

        protected void populateGenderCombo()
        {
            CRUD myCrud = new CRUD();
            string mySql = @"select genderid, gender from gender ";
            SqlDataReader dr =        myCrud.getDrPassSql(mySql);
            ddlGender.DataValueField = "genderid";
            ddlGender.DataTextField = "gender";
            ddlGender.DataSource = dr;
            ddlGender.DataBind();
        }

        protected void showMember()
        {
            CRUD myCrud = new CRUD();
            string mySql = @"  select memberId, fname,mName,cell, email , m.genderid, gender
                                from member  m inner join gender g on m.genderId = g.genderId ";
            SqlDataReader dr = myCrud.getDrPassSql(mySql);
            gvMember.DataSource = dr;
            gvMember.DataBind();
        }

        protected void populateForm_Click(object sender, EventArgs e)
        {
            int PK = int.Parse((sender as LinkButton).CommandArgument);
            //lblOuput.Text = PK.ToString();

            string mySql = @" select memberId, fname,mName,lname,cell, email , genderid
                            from member 
                             where memberId=@memberId";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@memberId", PK);
            CRUD myCrud = new CRUD();
            SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    // capture data from datarader object into a variable
                    int intEmpId = int.Parse(dr["memberId"].ToString());
                    String strFName = dr["fname"].ToString();
                    String strMName = dr["mName"].ToString();
                    String strLName = dr["lname"].ToString();
                    String strCell = dr["cell"].ToString();
                    String strEmail = dr["email"].ToString();
                    string strGenderId = dr["genderid"].ToString();

                    txtMemberId.Text = intEmpId.ToString();
                    txtFName.Text = strFName;
                    txtMName.Text = strMName;
                    txtLName.Text = strLName;
                    txtCell.Text = strCell;
                    txtEmail.Text = strEmail;
                    //lblOuput.Text = empId + employee+ depId;
                    ddlGender.SelectedValue = strGenderId;
                }
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            //https://www.c-sharpcorner.com/UploadFile/0c1bb2/export-gridview-to-excel/
            // issues with validation to check https://www.aspsnippets.com/Articles/RegisterForEventValidation-can-only-be-called-during-Render.aspx
            // another article on how to export  in .net 2.0 https://www.c-sharpcorner.com/article/Asp-Net-2-0-export-gridview-to-excel/
            // write code to export to excel
            ExportGridToExcel();
        }

        protected void ExportGridToExcel() // working 1
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Vithal" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gvMember.GridLines = GridLines.Both;
            gvMember.HeaderStyle.Font.Bold = true;
            gvMember.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }

        /**
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Customers.doc"));
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/ms-word";
        **/
        protected void ExportGridToWord() // this works 2
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Vithal" + DateTime.Now + ".doc";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/msword";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gvMember.GridLines = GridLines.Both;
            gvMember.HeaderStyle.Font.Bold = true;
            gvMember.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
        protected void ExportGridToPdf() // not working
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Vithal" + DateTime.Now + ".pdf";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
           // Response.ContentType = "application/vnd.ms-pdf";  // changed from excel to pdf
            Response.ContentType = "application/pdf";
            //  Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            Response.AddHeader("content-disposition", "attachment;filename=Employees.pdf");
            gvMember.GridLines = GridLines.Both;
            gvMember.HeaderStyle.Font.Bold = true;
            gvMember.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();


        }

        //public void btnExportToExcel(GridView myGv, Page Page)
        //{    ///This export GV data with no format
        //     ///a very simple code.

        //    HttpContext.Current.Response.ClearContent();
        //    HttpContext.Current.Response.Buffer = true;
        //    HttpContext.Current.Response.ContentType = "application/ms-excel";
        //    HttpContext.Current.Response.Charset = "";
        //    Page.EnableViewState = false;  // not working
        //    HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=report.xls");
        //    HttpContext.Current.Response.ContentType = "application/ms-excel";
        //    System.IO.StringWriter tw = new System.IO.StringWriter();
        //    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        //    myGv.AllowPaging = false;   // not working
        //    myGv.AllowSorting = false; // not working
        //    myGv.DataBind();   // not working
        //    myGv.RenderControl(hw); // not working
        //    HttpContext.Current.Response.Write(tw.ToString());
        //    HttpContext.Current.Response.End();
        //}
        //public void btnExportToWord(GridView GridView1, Page Page)
        //{   // working
        //    // this must  disallow paging and sorting so all rows are exported
        //    GridView1.AllowPaging = false;
        //    GridView1.AllowSorting = false;
        //    GridView1.DataBind();
        //    HttpContext.Current.Response.ClearContent();
        //    HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Customers.doc"));
        //    HttpContext.Current.Response.Charset = "";
        //    HttpContext.Current.Response.ContentType = "application/ms-word";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);
        //    GridView1.RenderControl(htw);
        //    HttpContext.Current.Response.Write(sw.ToString());
        //    HttpContext.Current.Response.End();
        //}

 

        protected void btnExportToWord_Click(object sender, EventArgs e)
        {
            ExportGridToWord();  // working 2
        }

        protected void btnExportToPdf_Click(object sender, EventArgs e)
        {
            ExportGridToPdf();
        }

        protected void btnExportToExcelCls_Click(object sender, EventArgs e)
        {
            exportManager.ExportGridToExcel(gvMember);  // working 3
        }

        protected void btnExportToWordCls_Click(object sender, EventArgs e)
        {
            exportManager.ExportGridToWord(gvMember); // working 3
        }

        protected void btnExportToPdfCls_Click(object sender, EventArgs e)
        {

        }
    }
}