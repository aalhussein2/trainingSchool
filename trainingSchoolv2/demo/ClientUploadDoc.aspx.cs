using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using trainingSchoolv2.App_Code;
namespace trainingSchoolv2.demo
{
    public partial class ClientUploadDoc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populateGroupTypeCombo();
            }
        }
        protected void populateGroupTypeCombo()
        {
            CRUD myCrud = new CRUD();
            string mySql = @"SELECT  groupTypeId, groupType
                          FROM groupType ";
            SqlDataReader dr = myCrud.getDrPassSql(mySql);
            ddlgroupType.DataValueField = "groupTypeId";
            ddlgroupType.DataTextField = "groupType";
            ddlgroupType.DataSource = dr;
            ddlgroupType.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //clearMsg();
            ////capture inserted values from the input form
            //string strClientName = "";
            //int ddlGroupTypeId = 0;
            //int intClientId = 0;
            //int rtn = 0;

            //strClientName = txtClient.Text;
            //ddlGroupTypeId = int.Parse(ddlgroupType.SelectedItem.Value);
            //// lblOutput.Text = strClientName + " " + ddlGroupTypeId;
            //// connect to the db and insert the captured vlaues
            //CRUD myCrud = new CRUD();
            //string mySql = @"INSERT INTO client (client ,groupTypeId)
            //                VALUES  (@client, @groupTypeId)";
            //Dictionary<string, object> myPara = new Dictionary<string, object>();
            //myPara.Add("@client", strClientName);
            //myPara.Add("groupTypeId", ddlGroupTypeId);
            //rtn = myCrud.InsertUpdateDelete(mySql, myPara);
            //lblOutput.Text += "<br>Returned id : " + rtn;

            //// save in a file system
            //ProcessUpload(FileUpload);
        }
        protected void btnSubmitMultiFiles_Click(object sender, EventArgs e)
        {
            clearMsg();
            ProcessMultiUploads();

            //string clientId = string.IsNullOrEmpty(txtClient.Text)? "000" : txtClient.Text ;  // move to common
            //ProcessMultiUploads(FileUpload, clientId);
        }
        protected void btnSubmitMultiFilesToDb_Click(object sender, EventArgs e)
        {
            clearMsg();
            //System.Threading.Thread.Sleep(5000);
            //return;
            try
            {
                //capture inserted values from the input form
                string strClient = "";
                int ddlGroupTypeId = 0;
                int intClientId = 0;
                strClient = txtClient.Text;
                ddlGroupTypeId = int.Parse(ddlgroupType.SelectedItem.Value);
                lblOutput.Text = strClient + " " + ddlGroupTypeId;
                // connect to the db and insert the captured vlaues
                CRUD myCrud = new CRUD();
                string mySql = @"INSERT INTO client (client ,groupTypeId)
                                VALUES  (@client, @groupTypeId)" +
                                "SELECT CAST(scope_identity() AS int)";
                Dictionary<string, object> myPara = new Dictionary<string, object>();
                myPara.Add("@client", strClient);
                myPara.Add("groupTypeId", ddlGroupTypeId);
                intClientId = myCrud.InsertUpdateDeleteViaSqlDicRtnIdentity(mySql, myPara);
               int rtn =  InsertDocuments(intClientId);
                if (rtn >=1)
                {
                    lblOutput.Text = "You successfully uploaded " + rtn + " files ";
                }
                else
                {
                    lblOutput.Text = "Failed to upload!";
                }
                
            }

            catch (Exception ex)
            {
                lblOutput.Text = ex.Message.ToString();
            }
        }
        protected int InsertDocuments(int myClientId)  // upload doc to db
        {
            int intFilesUploaded = 0;
            foreach (HttpPostedFile postedFile in FileUpload.PostedFiles)
            {
                string filename = Path.GetFileName(postedFile.FileName);
                string contentType = postedFile.ContentType;
                using (Stream fs = postedFile.InputStream)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] bytes = br.ReadBytes((Int32)fs.Length);
                        CRUD DocInsert = new CRUD();  // added Nov 2017 
                        string mySql = @"insert into clientDoc(clientId,DocName,ContentType,DocData)
                                    values (@clientId,@DocName,@ContentType,@DocData)";
                        Dictionary<string, Object> p = new Dictionary<string, object>();
                        //p.Add("@TaskId", "get the value ");
                        p.Add("@clientId", myClientId);  // added Nov 2017
                        p.Add("@DocName", filename);
                        p.Add("@ContentType", contentType);
                        p.Add("@DocData", bytes);
                        DocInsert.InsertUpdateDelete(mySql, p);
                        intFilesUploaded += 1;
                    }
                }
            }
            return intFilesUploaded;  // return number of files uploaded
        }
        protected void ProcessUpload(FileUpload upload)  // works 2021
        {
            string getMyID = "";
            // capture id 
            string myContactId = txtClient.Text;
            myContactId = string.IsNullOrEmpty(myContactId) ? "000" : myContactId;
            if (upload.HasFile)
            {
                 string fileName = Path.Combine(Server.MapPath("~/Uploads"), myContactId + "_" + upload.FileName);
                if (File.Exists(fileName))
                    File.Delete(fileName);
                upload.SaveAs(fileName);
                lblOutput.Text = "Uploaded to Db";

                ////use index of and substring 
                //getMyID = fileName;
                //int index1 = getMyID.IndexOf("Uploads");
                //int indexEnd = getMyID.IndexOf("_");
                //int x = (index1 + 8) - indexEnd;
                //getMyID = fileName.Substring(index1 + 8, x);
                //lblOutput.Text = getMyID.ToString();
            }
        }
        protected void ProcessUpload(FileUpload upload, int myCientId)  // works 2021
        {
            myCientId = string.IsNullOrEmpty(myCientId.ToString()) ? 000 : myCientId;
            if (upload.HasFile)
            {
                 string fileName = Path.Combine(Server.MapPath("~/Uploads"), myCientId + "_" + upload.FileName);
                if (File.Exists(fileName))
                    File.Delete(fileName);
                upload.SaveAs(fileName);
                lblOutput.Text = "Uploaded to Folder!";

                ////use index of and substring 
                //getMyID = fileName;
                //int index1 = getMyID.IndexOf("Uploads");
                //int indexEnd = getMyID.IndexOf("_");
                //int x = (index1 + 8) - indexEnd;
                //getMyID = fileName.Substring(index1 + 8, x);
                //lblOutput.Text = getMyID.ToString();
            }
        }
        protected void ProcessMultiUploads()
        {
            int rtn = 0;
            string myContactId = txtClient.Text;
            myContactId = string.IsNullOrEmpty(myContactId) ? "000" : myContactId;
            string myPath = Path.Combine(Server.MapPath("~/Uploads"));
            int uploadIndex = 0;
            string fileName = "";
            if (FileUpload.HasFiles)
            {
                foreach (HttpPostedFile postedFile in FileUpload.PostedFiles)
                {
                    fileName = Path.Combine(Server.MapPath("~/Uploads"), myContactId + "_" + postedFile.FileName);//postedFile.FileName;
                    // uploadIndex = fileName.IndexOf("Uploads");
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    FileUpload.SaveAs(fileName);
                }
                lblOutput.Text = "Your files has been uploaded Successfully!";
            }
        }
        protected void ProcessMultiUploads(FileUpload upload, string myClientId)
        {
            int rtn = 0;
            myClientId = string.IsNullOrEmpty(myClientId) ? "000" : myClientId;
            string myPath = Path.Combine(Server.MapPath("~/Uploads"));
            int uploadIndex = 0;
            string fileName = "";
            if (FileUpload.HasFiles)
            {
                foreach (HttpPostedFile postedFile in FileUpload.PostedFiles)
                {
                    fileName = Path.Combine(Server.MapPath("~/Uploads"), myClientId + "_" + postedFile.FileName);
                    // uploadIndex = fileName.IndexOf("Uploads");
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    FileUpload.SaveAs(fileName);
                }
                lblOutput.Text = "Client files has been uploaded Successfully!";
            }
        }

        protected void btnShowFiles_Click(object sender, EventArgs e)
        {
            clearMsg();
            //gvClientFiles.DataSource = null;
            //gvClientFiles.DataBind();

            gvClientFiles.DataSource = GetUploadList();  // new 
            gvClientFiles.DataBind();
          
        }
        protected IEnumerable GetUploadList()
        {
            string folder = Server.MapPath("~/Uploads");// get uploaded folder
            string[] files = Directory.GetFiles(folder); // get all files in folder 
            Array.Sort(files);
            foreach (string file in files)
                //// here I can specify the id of the file
                //if (file.Contains("_thm"))
                //{
                //    int position = file.IndexOf("_thm");
                //    yield return Path.GetFileName(file.Substring(0,position));
                //}
                //////if (file.Contains("500")) // check if files contains a value
                //////{
                //////    yield return Path.GetFileName(file);
                //////    int position = file.IndexOf("_");
                //////    yield return Path.GetFileName(file.Substring(0, position));
                //////}
                //////else
                //////{
                //////    yield return Path.GetFileName(file);
                //////}
             yield return Path.GetFileName(file);
        }
        protected IEnumerable GetUploadList(string clientId)
        {
            string folder = Server.MapPath("~/Uploads");// get uploaded folder
            string[] files = Directory.GetFiles(folder); // get all files in folder 
            Array.Sort(files);
            int intFileCount = 0;
            foreach (string file in files)
            //// here I can specify the id of the file
            //if (file.Contains("_thm"))
            //{
            //    int position = file.IndexOf("_thm");
            //    yield return Path.GetFileName(file.Substring(0,position));
            //}
            {
                if (file.Contains(clientId)) // check if files contains a value
                {
                    intFileCount += 1;
                    yield return Path.GetFileName(file);
                    //int position = file.IndexOf("_");
                    //yield return Path.GetFileName(file.Substring(0, position));
                }
            }
            if (intFileCount == 0)
            {
                lblOutput.Text = "No File Found !";
            }
            //.. yield return Path.GetFileName(file);
        }

        protected void ShowClientfiles_Click(object sender, EventArgs e)
        {
            clearMsg();
            // clear gridview from previous data 
            //gvClientFiles.DataSource = null;
            //gvClientFiles.DataBind();

            string myClientId = txtClient.Text;
            hiddenClientId.Value = myClientId;
            if (string.IsNullOrEmpty(myClientId))
            {
                gvClientFiles.DataSource = null;
                gvClientFiles.DataBind();
                lblOutput.Text = "Not Authorized!";
                return;
            }
            else
            {
                gvClientFiles.DataSource = GetUploadList(hiddenClientId.Value); // new 
                gvClientFiles.DataBind();
            }
        }

        protected void clearMsg()
        {
            lblOutput.Text = "";
        }

        protected void txtClient_TextChanged(object sender, EventArgs e)
        {
            clearMsg();
        }
    }
}