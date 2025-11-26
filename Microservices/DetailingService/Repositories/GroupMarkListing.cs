using System.Text;
using Microsoft.VisualBasic;
using System.Data;
using System.Collections;
using System.Security.Principal;
using DetailingService.Context;
using DetailingService.Interfaces;
using Microsoft.EntityFrameworkCore;
using DetailingService.Constants;
using Microsoft.Data.SqlClient;
using DetailingService.Repositories;
using Dapper;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient.DataClassification;

partial class GroupMarkListing 
{
    public string[] strResult;
     GroupMarkInfo objGMLInfo = new GroupMarkInfo();
     GroupMarkDAL objGMLDal = new GroupMarkDAL();
  
    public DataView dvGML = new DataView();
    public DataTable dtGML = new DataTable();
   
    #region Declaration
    public  string strFileType;
    public string strFile;
    public string strFileName;
    public string strPath;
    public string strDirPath;
    public string strExcelColumn;
    public string strExcelDataArray = "";
    public string strSMXML;
    public string strPMXML;
    private string strPSDXML;
    private string strCABXML;
    private string strCABSDXML;
    private string strPage;
    private string strAccessoriesXML;
    private int intOutput;
    private int hdnGroupMarkId;
    private int hdnProductype;
    private Label lblGroupMarkName;
    private int hdnParameter;
    private bool IsFromEdit;
    private string strErrorMsg;
    private int errorLogNo;
    private string strExport;
    private string filterExpression = "";
    private Label lblSideForCode;

    public int hdnSEID;
    #endregion


    //protected void Page_Load(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        btnY.Attributes.Add("onclick", " this.disabled = true; " + System.Web.UI.Page.ClientScript.GetPostBackEventReference(btnY, null/* TODO Change to default(_) if this is not a reference type */) + ";");

    //        AccessRights();
    //        int i = 0;
    //        int j = RadGrid1.Columns.Count;
    //        for (i = 0; i <= j - 1; i++)
    //        {
    //            if (j == 3)
    //                RadGrid1.ClientSettings.ClientMessages.DragToGroupOrReorder = "drag to reorder";
    //        }
    //        if (System.Web.UI.Page.Request.QueryString["StructElement"].ToString() == "Beam" | System.Web.UI.Page.Request.QueryString["StructElement"].ToString() == "Slab")
    //            btnExport.Enabled = true;
    //        btnExport.Attributes.Add("OnClick", "return fnSelectValidation()");
    //        if (!System.Web.UI.Page.IsPostBack)
    //            System.Web.UI.Page.Session["UserId"] = strResult[2];
    //    }
    //    // BindGML()
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}

    //private void AccessRights()
    //{
    //    try
    //    {
    //        strPage = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
    //        WindowsPrincipal struser = new WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent());
    //        objAdminDal = new NDSDAL.AdminDal();
    //        objAdminInfo = new NDSBLL.AdminInfo();
    //        objAdminInfo.LoginId = ExtractUserName(struser.Identity.Name);
    //        int intLen;
    //        intLen = strPage.IndexOf(".");
    //        objAdminInfo.FormName = strPage.Substring(0, intLen);
    //        strResult = objAdminDal.ValidateUser(objAdminInfo);
    //        string strStructElement;
    //        strStructElement = System.Web.UI.Page.Request.QueryString["StructElement"];
    //    }
    //    // '''''''''''''''
    //    // If (strResult(0) = True And strResult(1) = True) Then
    //    // gvGML.Columns(8).Visible = True
    //    // btnImport.Enabled = True
    //    // ElseIf (strResult(0) = False And strResult(1) = False) Then
    //    // Response.Redirect("~/UnAuthorization.aspx?file=" + strStructElement + " " + "Detailing", False)
    //    // Exit Sub
    //    // ElseIf (strResult(0) = True And strResult(1) = False) Then
    //    // gvGML.Columns(8).Visible = False
    //    // btnImport.Enabled = False
    //    // ElseIf (strResult(0) = False And strResult(1) = True) Then
    //    // gvGML.Columns(8).Visible = True
    //    // btnImport.Enabled = True
    //    // End If
    //    // '''''''''''''''
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}
    //private void BindGML()
    //{
    //    try
    //    {
    //        if (objGMLInfo.intProjectId == null)
    //            objGMLInfo.intProjectId = Convert.ToInt32("ProjectId");

    //        if ("StructElementId"== null)
    //        {
    //            objGMLInfo.intStructureElementTypeId = Convert.ToInt32("StructElementId");
    //            //System.Web.UI.Page.Session["StructureElementId"] = hdnSEID.Text;
    //        }
    //        if ((System.Web.UI.Control.ViewState["sortExpr"]) != null)
    //        {
    //            dvGML.Table = objGMLDal.GetGroupMarkListing(objGMLInfo).Tables(0);
    //            dvGML.Sort = (string)System.Web.UI.Control.ViewState["sortExpr"] + " " + System.Web.UI.Control.ViewState["SortDirection"];
    //        }
    //        else
    //            dvGML = objGMLDal.GetGroupMarkListing(objGMLInfo).Tables(0).DefaultView;
    //        // dtGML = dvGML
    //        // dvGML.ToTable("dtGML")
    //        // ViewState("GridData") = dtGML

    //        RadGrid1.DataSource = dvGML;
    //    }

    //    // RadGrid1.DataBind()


    //    // gvGML.DataSource = dvGML
    //    // gvGML.DataBind()
    //    // gvGML.Visible = False
    //    catch (Exception ex)
    //    {

    //    }
    //}


    //private void CopyGM()
    //{
    //    try
    //    {
    //        string strSM = "";
    //        int i;
    //        int intlstcount = 0;
    //        int intOutput;
    //        int tntRev = 0;
    //        string strRem = null;
    //        DataSet dsSM;

    //        //objAdminInfo = new NDSBLL.AdminInfo();
    //        //objAdminDal = new NDSDAL.AdminDal();
    //        //objAdminInfo.GroupMarkingId = System.Web.UI.Page.Session["hdnGroupMarkId"];
    //        //objAdminInfo.CopyStructureElement = System.Web.UI.Page.Request.QueryString["StructElementId"];

    //        // ''dsSM = objAdminDal.GetCopyStructureMarking(objAdminInfo)
    //        // ''If (dsSM.Tables.Count > 0) Then
    //        // ''    If dsSM.Tables(0).Rows.Count > 0 Then
    //        // ''        For i = 0 To dsSM.Tables(0).Rows.Count - 1
    //        // ''            strSM += dsSM.Tables(0).Rows(i).Item("intStructureMarkId").ToString() + "ì"
    //        // ''        Next
    //        // ''    End If
    //        // ''End If
    //        // 'dsSM = objAdminDal.GetCopyStructureMarking(objAdminInfo)
    //        // 'If (dsSM.Tables.Count > 0) Then
    //        // '    If dsSM.Tables(0).Rows.Count > 0 Then
    //        // '        For i = 0 To dsSM.Tables(0).Rows.Count - 1
    //        // '            If (i = 0) Then
    //        // '                strSM = dsSM.Tables(0).Rows(i).Item("intStructureMarkId").ToString()
    //        // '            Else
    //        // '                strSM += "ì" + dsSM.Tables(0).Rows(i).Item("intStructureMarkId").ToString()
    //        // '            End If
    //        // '        Next
    //        // '    End If
    //        // 'End If
    //        // ------------------
    //        //dsSM = objAdminDal.GetCopyStructureMarking(objAdminInfo);
    //        //if ((dsSM.Tables.Count > 0))
    //        //{
    //        //    if (dsSM.Tables[0].Rows.Count > 0)
    //        //    {
    //        //        for (i = 0; i <= dsSM.Tables[0].Rows.Count - 1; i++)
    //        //            strSM += dsSM.Tables[0].Rows[i].Item["intStructureMarkId"].ToString() + "ì";
    //        //    }
    //        //}
    //        // dsSM = objAdminDal.GetCopyStructureMarking(objAdminInfo)
    //        // If (dsSM.Tables.Count > 0) Then
    //        // If dsSM.Tables(0).Rows.Count > 0 Then
    //        // For i = 0 To dsSM.Tables(0).Rows.Count - 1
    //        // If (i = 0) Then
    //        // strSM = dsSM.Tables(0).Rows(i).Item("intStructureMarkId").ToString()
    //        // Else
    //        // strSM += "ì" + dsSM.Tables(0).Rows(i).Item("intStructureMarkId").ToString()
    //        // End If
    //        // Next
    //        // End If
    //        // End If

    //        // ---------------

    //        DataSet dsGroup;
    //        GroupMarkInfo objGMLInfo = new GroupMarkInfo();



    //        objGMLInfo.GroupMarkId = Convert.ToInt32("hdnGroupMarkId");
    //        objGMLInfo.intProjectId = Convert.ToInt32("ProjectId");
    //        //objAdminInfo.CopyStructureElement = System.Web.UI.Page.Request.QueryString["StructElementId"];
    //        //objAdminInfo.ProductType = System.Web.UI.Page.Session["hdnProductype"];
    //        dsGroup = objAdminDal.GetGroupmarkingRevQuan(objAdminInfo);
    //        if ((dsGroup.Tables.Count > 0))
    //        {
    //            if (dsGroup.Tables[0].Rows.Count > 0)
    //            {
    //                tntRev = dsGroup.Tables[0].Rows[0].Item["tntGroupRevNo"].ToString();
    //                strRem = dsGroup.Tables[0].Rows[0].Item["vchRemarks"].ToString();
    //            }
    //        }

    //        objAdminInfo = new NDSBLL.AdminInfo();
    //        objAdminDal = new NDSDAL.AdminDal();

    //        objAdminInfo.GroupMarkingId = System.Web.UI.Page.Session["hdnGroupMarkId"];
    //        objAdminInfo.SourceGroupMarking = System.Web.UI.Page.Session["lblGroupMarkName"].ToString();
    //        objAdminInfo.SourceStructureMarking = strSM;
    //        objAdminInfo.ProjectId = System.Web.UI.Page.Request.QueryString["ProjectId"];
    //        objAdminInfo.ParameterSetNo = System.Web.UI.Page.Session["lblParamsetNo"];
    //        objAdminInfo.DestGroupRevNo = tntRev + 1;
    //        objAdminInfo.DestProjectId = System.Web.UI.Page.Request.QueryString["ProjectId"];
    //        objAdminInfo.DestStructureElementTypeId = System.Web.UI.Page.Request.QueryString["StructElementId"];
    //        objAdminInfo.DestProductTypeId = System.Web.UI.Page.Session["hdnProductype"];
    //        objAdminInfo.DestGroupMarkingName = System.Web.UI.Page.Session["lblGroupMarkName"].ToString();
    //        objAdminInfo.DestParamSetNumber = System.Web.UI.Page.Session["lblParamsetNo"];
    //        objAdminInfo.DestRemarks = strRem;
    //        objAdminInfo.DestStatusId = "1";
    //        // objAdminInfo.DestCreatedUId = "12"
    //        objAdminInfo.DestCreatedUId = System.Web.UI.Page.Session["UserId"];
    //        objAdminInfo.CopyFrom = System.Web.UI.Page.Session["lblGroupMarkName"].ToString();
    //        objAdminInfo.ElementId = "ì";
    //        // ddlContract.SelectedItem.Text + "," + lblSapContactNoVal.Text + "," + ddlGroupMarking.SelectedItem.Text + "," + "R" + lblRevisionNoVal.Text + "," + "P" + lblParameterSetVal.Text"

    //        // intOutput = objAdminDal.InsertCopyGroupMarking(objAdminInfo)
    //        // Begin Tran----
    //        objCommanUtilities = new APlusUtilities.ARMATools();
    //        using (System.Transactions.TransactionScope objTS = new System.Transactions.TransactionScope())
    //        {
    //            DataSet dsCopyGM;
    //            if (objCommanUtilities.checkAplusconnection() == 1)
    //            {
    //                dsCopyGM = objAdminDal.InsertCopyGroupMarking(objAdminInfo);
    //                Hashtable htArmaId = new Hashtable();
    //                Int32 intRow = 0;

    //                objCommanUtilities = new APlusUtilities.ARMATools();
    //                if ((dsCopyGM.Tables.Count > 0))
    //                {
    //                    if ((dsCopyGM.Tables.Count > 1))
    //                    {
    //                        if (dsCopyGM.Tables[0].Rows.Count > 0 & dsCopyGM.Tables[1].Rows.Count > 0)
    //                        {
    //                            for (intRow = 0; intRow <= dsCopyGM.Tables[0].Rows.Count - 1; intRow++)
    //                                htArmaId.Add(dsCopyGM.Tables[0].Rows[intRow].Item["intArmaid"], dsCopyGM.Tables[1].Rows[intRow].Item["intArmaid"]);
    //                        }
    //                    }
    //                    objCommanUtilities.CopyBBSInArma(htArmaId, System.Web.UI.Page.Server.MapPath(@"~\XML Files\CopyBBS.xml"));
    //                }

    //                switch (objCommanUtilities.ErrorID)
    //                {
    //                    case 3:
    //                        {
    //                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "<script>alert('Aplus Not responding!, Kindly restart Aplus');</script>", false);
    //                            return;
    //                        }

    //                    case 4:
    //                        {
    //                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "<script>alert('Connection to Aplus failed , Kindly check if the Aplus process is running, if the problem persist try restarting Aplus');</script>", false);
    //                            return;
    //                        }

    //                    case 6:
    //                        {
    //                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "<script>alert('BBS is not copied completely. Kindly restart Aplus and try again');</script>", false);
    //                            return;
    //                        }
    //                }
    //                if ((dsCopyGM.Tables.Count > 0))
    //                {
    //                    if ((dsCopyGM.Tables.Count > 1))
    //                    {
    //                        if (dsCopyGM.Tables[3].Rows.Count > 0)
    //                        {
    //                            objGMLInfo.GroupMarkId = System.Web.UI.Page.Session["hdnGroupMarkId"];
    //                            objGMLInfo.IdentityGMId = dsCopyGM.Tables[3].Rows[0].Item["IDENTITYGM"];
    //                            objGMLInfo.GMRevNo = dsCopyGM.Tables[3].Rows[0].Item["GMRevNo"];
    //                            objGMLDal.UpdateGMINPostGM(objGMLInfo);
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "<script>alert('Connection to Aplus failed , Kindly check if the Aplus process is running, if the problem persist try restarting Aplus');</script>", false);

    //            // ----END TRAN----
    //            objTS.Complete();
    //        }
    //    }
    //    // If intOutput = 1 Then
    //    // ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Msg", "<script>alert('The Groupmarking details copied successfully');</script>", False)
    //    // End If
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}


    //protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    //{
    //    if (e.CommandName == "Delete")
    //    {
    //        try
    //        {
    //            int intGMid;
    //            intGMid = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("intGroupMarkId");
    //            // intGMid=(CInt)(tableView.DataKeyValues(editedItem.ItemIndex)("intGroupMarkId"))

    //            // intGMid = gvGML.DataKeys(e.Item.RowIndex()).Value.ToString()
    //            objGMLInfo.GroupMarkId = intGMid;
    //            intOutput = objGMLDal.DelGroupMark(objGMLInfo);
    //            BindGML();
    //            if (intOutput == 1)
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "<script>alert('The selected groupmarking has been deleted successfully');</script>", false);
    //            else
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "<script>alert('Cannot delete the groupmarking.It has been posted already');</script>", false);
    //        }
    //        catch (Exception ex)
    //        {
    //            errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //            strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //            ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //        }
    //    }
    //}

    // lnkEdit_Click
    // Protected Sub lnkEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    //protected void RadGrid1_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    //{
    //    try
    //    {
    //        IsFromEdit = true;
    //        if (e.CommandName == "Edit")
    //        {
    //            GridDataItem item = (GridDataItem)e.Item;
    //            item.Edit = false;

    //            // Dim objEdit As LinkButton = DirectCast(e.CommandSource, LinkButton)
    //            Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
    //            // Dim editedItem As Telerik.Web.UI.GridEditableItem = radgrid1.
    //            DataSet dsGMList;
    //            DataSet dsProductType;
    //            DataSet dsGMPost = null;
    //            // Code Added in order to accomodate both Mesh and PRC detailing Page----26-MAR-2012---Surendar Selvaraj
    //            string strProductType;
    //            Label lblProdType = new Label();
    //            lblProdType = (Label)editedItem.FindControl("lblProdType");
    //            strProductType = lblProdType.Text;
    //            hdnGroupMarkId = (HiddenField)editedItem.FindControl("hdnGroupMarkId");
    //            System.Web.UI.Control.ViewState["hdnGroupMarkId"] = hdnGroupMarkId.Value;
    //            hdnProductype = (HiddenField)editedItem.FindControl("hdnProductype");
    //            System.Web.UI.Page.Session["hdnGroupMarkId"] = hdnGroupMarkId.Value;
    //            System.Web.UI.Page.Session["hdnProductype"] = hdnProductype.Value;
    //            lblGroupMarkName = (Label)editedItem.FindControl("lblGroup");
    //            System.Web.UI.Page.Session["lblGroupMarkName"] = lblGroupMarkName.Text;
    //            hdnParameter = (HiddenField)editedItem.FindControl("hdnParameter");
    //            System.Web.UI.Page.Session["lblParamsetNo"] = hdnParameter.Value;
    //            objGMLInfo.GroupMarkId = Convert.ToInt32(hdnGroupMarkId.Value);
    //            objGMLInfo.ProductId = Convert.ToInt32(hdnProductype.Value);
    //            dsGMList = objGMLDal.GetReleasedGroupMark(objGMLInfo);
    //            dsProductType = objGMLDal.GetProductType(objGMLInfo);
    //            System.Web.UI.Page.Session["hdnProductTypeName"] = dsProductType.Tables[0].Rows[0].Item["ProductType"].ToString().ToLower().Trim();
    //            System.Web.UI.Control.ViewState["IsCabOnly"] = System.Convert.ToBoolean(dsProductType.Tables[0].Rows[0].Item["Indicator"]);
    //            lblSideForCode = (Label)editedItem.FindControl("lblSideForCode");
    //            objGMLInfo.GroupMarkId = Convert.ToInt32(hdnGroupMarkId.Value);
    //            dsGMPost = objGMLDal.GetPostedGroupMark(objGMLInfo);
    //            if ((dsGMList.Tables.Count > 0) && (dsGMList.Tables[0].Rows.Count > 0))
    //                ModalPopupExtender2.Show();
    //            else if ((dsGMPost.Tables.Count > 0) & (dsGMPost.Tables[0].Rows.Count > 0))
    //            {
    //                if ((lblSideForCode.Text != "0"))
    //                    System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlSideForMesh") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //                else if (((System.Web.UI.Page.Session["hdnProductTypeName"] == "cab")))
    //                    System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlCab") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //                else if ((System.Web.UI.Page.Session["hdnProductTypeName"] == "acs"))
    //                    System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlMesh") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //                else if (hdnSEID.Text == 5)
    //                    System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlDrain") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //                else if (hdnSEID.Text == 8)
    //                    System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlBore") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //                else if (strProductType.ToUpper() == "MSH")
    //                    System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlMesh") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //                else if (strProductType.ToUpper() == "PRC")
    //                    System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlPRC") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //                else if (strProductType.ToUpper() == "CAR")
    //                    System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlCAR") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //            }
    //            else if ((lblSideForCode.Text != "0"))
    //                System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlSideForMesh") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //            else if (((System.Web.UI.Page.Session["hdnProductTypeName"] == "cab")))
    //                System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlCab") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //            else if ((System.Web.UI.Page.Session["hdnProductTypeName"] == "acs"))
    //                System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlMesh") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //            else if (hdnSEID.Text == 5)
    //                System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlDrain") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //            else if (hdnSEID.Text == 8)
    //                System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlBore") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //            else if (strProductType.ToUpper() == "MSH")
    //                System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlMesh") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //            else if (strProductType.ToUpper() == "PRC")
    //                System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlPRC") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //            else if (strProductType.ToUpper() == "CAR")
    //                System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlCAR") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), false);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}


    //protected void btnImport_Click(object sender, System.EventArgs e)
    //{
    //    UploadData();
    //}

    //private void UploadData()
    //{
    //    try
    //    {
    //        if ((fupdImport.HasFile))
    //        {
    //            strFileType = Trim(fupdImport.PostedFile.FileName.Substring(fupdImport.PostedFile.FileName.LastIndexOf(".") + 1));
    //            strFile = fupdImport.FileName.Substring(0, fupdImport.FileName.LastIndexOf("."));
    //            strFileName = Strings.Replace(Strings.Trim(strFile), " ", "", 1, -1, CompareMethod.Text);
    //        }
    //        strPath = System.Web.UI.Page.Request.PhysicalApplicationPath + @"UploadedFiles\Import\";
    //        strDirPath = strPath + strFileName + "." + strFileType;
    //        System.IO.DirectoryInfo dirFilePath = new System.IO.DirectoryInfo(strPath);
    //        if (dirFilePath.Exists == false)
    //            dirFilePath.Create();
    //        if (System.IO.File.Exists(strDirPath))
    //            System.IO.File.Delete(strDirPath);
    //        fupdImport.SaveAs(strDirPath);

    //        SaveData(strFileName + "." + strFileType);
    //    }
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}

    //public string ExtractUserName(string path)
    //{
    //    string[] userPath = path.Split(new char[] { '\\' });
    //    return userPath[userPath.Length - 1];
    //}

    //private void SaveData(string strFileName)
    //{
    //}

    //protected void btnY_Click(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        // CopyGM()
    //        CopyGroupMarking();
    //        BindGML();
    //        objAdminInfo = new NDSBLL.AdminInfo();
    //        objAdminDal = new NDSDAL.AdminDal();
    //        objAdminInfo.GMName = System.Web.UI.Page.Session["lblGroupMarkName"].ToString();
    //        SqlDataReader drGMId;
    //        drGMId = objAdminDal.GetGMId(objAdminInfo);
    //        if ((drGMId.HasRows))
    //        {
    //            while (drGMId.Read())
    //                System.Web.UI.Control.ViewState["hdnGroupMarkId"] = drGMId.Item["intgroupmarkid"];
    //        }

    //        if (((System.Web.UI.Page.Session["hdnProductTypeName"] == "cab")))
    //            System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlCab") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);
    //        else if ((System.Web.UI.Page.Session["hdnProductTypeName"] == "acs"))
    //            System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlMesh") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);
    //        else if (hdnSEID.Text == 5)
    //            System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlDrain") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);
    //        else if (hdnSEID.Text == 8)
    //            System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlBore") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);
    //        else if (System.Web.UI.Page.Session["hdnProductTypeName"].ToString().ToUpper() == "MSH")
    //            System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlMesh") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);
    //        else if (System.Web.UI.Page.Session["hdnProductTypeName"].ToString().ToUpper() == "PRC")
    //            System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlPRC") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);
    //        else if (System.Web.UI.Page.Session["hdnProductTypeName"].ToString().ToUpper() == "CAR")
    //            System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlCAR") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);
    //        ModalPopupExtender2.Hide();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    //protected void btnN_Click(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        if ("hdnProductTypeName" == "cab")
    //        {
    //            //Response.Redirect(TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlCab") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);

    //        }

    //        else if ("hdnProductTypeName"== "acs")
    //        {
    //            //System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlMesh") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);

    //        }

    //        else if (hdnSEID== 5)
    //        {
    //            //System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlDrain") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);

    //        }

    //        else if (hdnSEID == 8)
    //        {
    //           // System.Web.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlBore") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);

    //        }

    //        else if ("hdnProductTypeName".ToString().ToUpper() == "MSH")
    //        {
    //            //System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlMesh") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);

    //        }

    //        else if ("hdnProductTypeName".ToString().ToUpper() == "PRC")
    //        {
    //          //  System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlPRC") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);

    //        }

    //        else if ("hdnProductTypeName".ToString().ToUpper() == "CAR")
    //        {
    //            //System.Web.UI.Page.Response.Redirect(System.Web.UI.TemplateControl.GetGlobalResourceObject("GlobalResource", "UrlCAR") + "?GroupId=" + System.Web.UI.Page.Server.UrlEncode(Convert.ToInt32(System.Web.UI.Control.ViewState["hdnGroupMarkId"])), false);

    //        }

    //        ModalPopupExtender2.Hide();
    //    }
    //    catch (Exception ex)
    //    {
           
    //    }
    //}

    //private ArrayList Export()
    //{
    //    DataSet dsExport = new DataSet();
    //    Export = new ArrayList();
    //    int i;
    //    string strGroupMarkId = "";
    //    string columnName;
    //    string FromPage;
    //    int count = 0;
    //    int intStart;
    //    int intEnd;

    //    try
    //    {
    //        FromPage = System.Web.UI.Page.Request.QueryString["StructElement"].ToString();
    //        for (i = 0; i <= RadGrid1.Items.Count - 1; i++)
    //        {
    //            if ((CheckBox)RadGrid1.Items(i).FindControl("chkImport").Checked == true)
    //                strGroupMarkId = strGroupMarkId + (Label)RadGrid1.Items(i).FindControl("ID").Text + "ì";
    //        }
    //        if (strGroupMarkId != "")
    //        {
    //            objGMLInfo.strGroupMarkId = strGroupMarkId;

    //            if (FromPage != null)
    //            {
    //                if (FromPage == "Beam")
    //                {
    //                    dsExport = objGMLDal.GetBeamExportToTxt(objGMLInfo);
    //                    intStart = 1;
    //                    intEnd = 3;
    //                }
    //                else if (FromPage == "Slab")
    //                {
    //                    dsExport = objGMLDal.GetSlabExportToTxt(objGMLInfo);
    //                    intStart = 2;
    //                    intEnd = 3;
    //                }
    //            }
    //            // ' Delete all the files in Export Folder
    //            string s;
    //            if (System.IO.Directory.Exists(ExportPath) == false)
    //                System.IO.Directory.CreateDirectory(ExportPath);
    //            foreach (var s in System.IO.Directory.GetFiles(ExportPath))
    //                System.IO.File.Delete(s);
    //            // ' Export to text 
    //            // System.IO.Directory.CreateDirectory("C:\MyDirectory")
    //            if (dsExport != null)
    //            {
    //                if (dsExport.Tables[0].Rows.Count != 0)
    //                {
    //                    string txtFilename = "";
    //                    if (strExport == "Table")
    //                    {
    //                        for (i = intStart; i <= dsExport.Tables.Count - 1; i++)
    //                        {
    //                            columnName = dsExport.Tables[i].Columns[0].ColumnName;
    //                            txtFilename = ExportPath + columnName.ToString() + ".txt";
    //                            ExportToTxt(txtFilename, dsExport.Tables[i]);
    //                            Export.Add(txtFilename);
    //                        }
    //                    }
    //                    else
    //                        for (i = 0; i <= dsExport.Tables.Count - intEnd; i++)
    //                        {
    //                            columnName = dsExport.Tables[i].Columns[0].ColumnName;
    //                            txtFilename = ExportPath + columnName.ToString() + ".txt";
    //                            ExportToTxt(txtFilename, dsExport.Tables[i]);
    //                            Export.Add(txtFilename);
    //                        }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}

    //public void ExportToTxt(string Directory, System.Data.DataTable Table)
    //{
    //    StreamWriter output = new StreamWriter(Directory, false, UnicodeEncoding.Default);
    //    string delim;
    //    int rowCount;
    //    int ColumnCount;
    //    string newLine;
    //    newLine = "";
    //    // Write out the header row
    //    delim = "";
    //    try
    //    {
    //        foreach (DataColumn col in Table.Columns)
    //        {
    //            output.Write(delim);
    //            delim = Strings.Space(20);
    //        }
    //        // output.Write(ControlChars.CrLf)
    //        for (rowCount = 0; rowCount <= Table.Rows.Count - 1; rowCount++)
    //        {
    //            for (ColumnCount = 0; ColumnCount <= Table.Columns.Count - 1; ColumnCount++)
    //            {
    //                // output.Write(""""c)
    //                output.Write(Table.Rows[rowCount].Item[ColumnCount]);
    //                // output.Write(""""c)
    //                delim = Strings.Space(20);

    //                output.Write(delim.Trim());
    //            }
    //            output.Write(ControlChars.CrLf);
    //            newLine = ControlChars.CrLf;
    //        }
    //        // output.WriteLine()
    //        output.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}

    //protected void btnExport_Click(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        ArrayList exportedTxtFilesList;
    //        strExport = "Table";
    //        exportedTxtFilesList = Export();
    //        if (exportedTxtFilesList.Count == 0)
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "<script>alert('Please select a Groupmarking.');</script>", false);
    //            return;
    //        }
    //        string DirectoryPath;
    //        DirectoryPath = ExportPath;
    //        ZipFile z;
    //        // Dim i As Integer = 0
    //        string directory;
    //        directory = ExportPath + System.Guid.NewGuid().ToString() + ".zip";
    //        z = ZipFile.Create(directory);
    //        z.BeginUpdate();
    //        foreach (string item in exportedTxtFilesList)
    //            z.Add(item);
    //        z.CommitUpdate();
    //        z.Close();
    //        FileInfo file = new FileInfo(directory);

    //        // DirectoryPath = "C:\ExportedTextFiles\"
    //        // DirectoryPath = ExportPath
    //        // obj.CreateZipFile(DirectoryPath)
    //        // Dim Directory As String
    //        // Directory = "C:\ExportedTextFiles\.zip"
    //        // Directory = ExportPath & ".zip"
    //        // Dim file As New IO.FileInfo(Directory)
    //        // System.IO.File.Delete("C:\ExportedTextFiles\.zip")
    //        // For Each extraFile In System.IO.Directory.GetFiles("C:\ExportedTextFiles\.zip\.zip")
    //        // System.IO.File.Delete(extraFile)
    //        // Next extraFile

    //        System.Web.UI.Page.Response.ClearHeaders();
    //        System.Web.UI.Page.Response.Clear();
    //        System.Web.UI.Page.Response.ClearContent();
    //        System.Web.UI.Page.Response.ContentType = "text/xml";
    //        System.Web.UI.Page.Response.Charset = "";
    //        System.Web.UI.Page.Response.AddHeader("Content-Disposition", "attachment; filename=ExportedFiles_Table_" + System.Guid.NewGuid().ToString() + ".zip");
    //        // Add the file size into the response header
    //        System.Web.UI.Page.Response.AddHeader("Content-Length", File.Length.ToString());
    //        // Set the ContentType
    //        // Write the file into the response
    //        System.Web.UI.Page.Response.WriteFile(File.FullName);
    //        // End the response
    //        System.Web.UI.Page.Response.Flush();
    //        System.Web.UI.Page.Response.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //    finally
    //    {
    //        Export.Clear();
    //    }
    //}

    //protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    //{
    //    try
    //    {
    //        HiddenField hdnPosted = (HiddenField)e.Item.FindControl("hdnPosted");
    //        if (e.Item is GridDataItem)
    //        {
    //            if (hdnPosted.Value == 1)
    //                (LinkButton)e.Item.FindControl("lnkDelete").Enabled = false;
    //            else
    //                (LinkButton)e.Item.FindControl("lnkDelete").Enabled = true;
    //            if ((e.Item.Cells.Count != 0))
    //            {
    //                LinkButton Delete;
    //                Delete = (LinkButton)e.Item.FindControl("lnkDelete");
    //                if (Delete != null && (LinkButton)e.Item.FindControl("lnkDelete").Enabled == true)
    //                    Delete.OnClientClick = string.Format("return confirm('Do you want to delete?');");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}

    //protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    //{
    //    try
    //    {
    //        BindGML();
    //    }
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}

    //protected void RadGrid1_PreRender(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        GridItem commandItem = RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)(0);
    //        TextBox txt = (TextBox)commandItem.FindControl("txtSearch");
    //        if (txt.Text != "")
    //        {
    //            string option;

    //            int itm = 0;
    //            string itm_val;

    //            for (itm = 0; itm <= RadGrid1.MasterTableView.Columns.Count - 1; itm++)
    //            {
    //                GridColumn griditem;
    //                griditem = RadGrid1.MasterTableView.Columns.Item(itm);
    //                if (griditem.ColumnType == "GridTemplateColumn" && griditem.Visible)
    //                {
    //                    option = " LIKE ";
    //                    itm_val = RadGrid1.MasterTableView.Columns.Item(itm).UniqueName;
    //                    if (griditem.DataTypeName == "System.String" && griditem.ShowFilterIcon && (!(itm_val == "Edit/Update" || itm_val == "vchStatus" || itm_val == "tntGroupRevNo" || itm_val == "sitProductTypeId" || itm_val == "datCreatedDate" || itm_val == "tntParamSetNumber")))
    //                    {
    //                        filterExpression = Interaction.IIf((itm > 1), filterExpression + " OR ", filterExpression + "");
    //                        filterExpression = filterExpression + "([" + itm_val + "]" + option + "'%" + txt.Text + "%'" + ")";
    //                    }
    //                }
    //            }
    //            RadGrid1.EnableLinqExpressions = false;
    //            RadGrid1.MasterTableView.FilterExpression = filterExpression;
    //            RadGrid1.MasterTableView.Rebind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}

    //protected void RadGrid1_SortCommand(object source, Telerik.Web.UI.GridSortCommandEventArgs e)
    //{
    //    BindGML();
    //}

    //protected void txtSearch_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        TextBox txt = (TextBox)sender as TextBox;
    //        string option;
    //        string filterExpression = "";
    //        int itm = 0;
    //        string itm_val;
    //        // '''Search For product Marking Report
    //        for (itm = 0; itm <= RadGrid1.MasterTableView.Columns.Count - 1; itm++)
    //        {
    //            GridColumn griditem;
    //            griditem = RadGrid1.MasterTableView.Columns.Item(itm);
    //            if (griditem.ColumnType == "GridTemplateColumn" && griditem.Visible)
    //            {
    //                option = " LIKE ";
    //                itm_val = RadGrid1.MasterTableView.Columns.Item(itm).UniqueName;
    //                if (griditem.DataTypeName == "System.String")
    //                {
    //                    if (itm > 1)
    //                        filterExpression = filterExpression + " OR ";
    //                    filterExpression = filterExpression + "([" + itm_val + "]" + option + "'%" + txt.Text + "%'" + ")";
    //                }
    //            }
    //        }
    //        RadGrid1.EnableLinqExpressions = false;
    //        RadGrid1.MasterTableView.FilterExpression = filterExpression;
    //        RadGrid1.MasterTableView.Rebind();
    //    }
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}

    //protected void btnShowAll_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        RadGrid1.MasterTableView.FilterExpression = "";
    //        RadGrid1.MasterTableView.Rebind();
    //    }
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //}

    // Protected Sub gvGML_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvGML.RowEditing
    // Try

    // Dim gvrGML As GridViewRow = Me.gvGML.Rows(e.NewEditIndex)
    // gvGML.EditIndex = e.NewEditIndex
    // 'Dim objEdit As LinkButton = DirectCast(e.CommandSource, LinkButton)
    // ' Dim gvrGML As GridViewRow = DirectCast(objEdit.NamingContainer, GridViewRow)
    // Dim dsGMList As DataSet
    // Dim dsGMPost As DataSet
    // hdnGroupMarkId = DirectCast(gvrGML.FindControl("hdnGroupMarkId"), HiddenField)
    // hdnProductype = DirectCast(gvrGML.FindControl("hdnProductype"), HiddenField)
    // Session("hdnGroupMarkId") = hdnGroupMarkId.Value
    // Session("hdnProductype") = hdnProductype.Value
    // lblGroupMarkName = DirectCast(gvrGML.FindControl("lblGroup"), Label)
    // Session("lblGroupMarkName") = lblGroupMarkName.Text
    // hdnParameter = DirectCast(gvrGML.FindControl("hdnParameter"), HiddenField)
    // Session("lblParamsetNo") = hdnParameter.Value
    // objGMLInfo.GroupMarkId = Convert.ToInt32(hdnGroupMarkId.Value)
    // dsGMList = objGMLDal.GetReleasedGroupMark(objGMLInfo)
    // objGMLInfo.GroupMarkId = Convert.ToInt32(hdnGroupMarkId.Value)
    // dsGMPost = objGMLDal.GetPostedGroupMark(objGMLInfo)
    // If dsGMList.Tables(0).Rows.Count > 0 Then
    // ModalPopupExtender2.Show()
    // ElseIf dsGMPost.Tables(0).Rows.Count > 0 Then
    // 'If ((Session("hdnProductTypeName") = "cab") Or (Session("hdnProductTypeName") = "acs") Or ((Session("hdnProductTypeName") = "prc") And (ViewState("IsCabOnly") = True))) Then
    // '    Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlCab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)) & "&P=Y", False)
    // 'Else
    // '    If hdnSEID.Text = 1 Then
    // '        Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBeam") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)) & "&P=Y", False)
    // '    ElseIf hdnSEID.Text = 2 Then
    // '        Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlColumn") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)) & "&P=Y", False)
    // '    ElseIf hdnSEID.Text = 4 Or hdnSEID.Text = 13 Then
    // '        Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlSlab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)) & "&P=Y", False)
    // '    ElseIf hdnSEID.Text = 5 Then
    // '        Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlDrain") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)) & "&P=Y", False)
    // '    ElseIf hdnSEID.Text = 8 Then
    // '        Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBore") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)) & "&P=Y", False)
    // '    End If
    // 'End If
    // If ((Session("hdnProductTypeName") = "cab") Or (Session("hdnProductTypeName") = "acs") Or ((Session("hdnProductTypeName") = "prc") And (ViewState("IsCabOnly") = True))) Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlCab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // Else
    // If hdnSEID.Text = 1 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBeam") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // ElseIf hdnSEID.Text = 2 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlColumn") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // 'ElseIf hdnSEID.Text = 4 Then
    // '    Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlSlab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // ElseIf hdnSEID.Text = 8 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBore") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // Else
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlSlab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // End If
    // End If
    // Else
    // If ((Session("hdnProductTypeName") = "cab") Or (Session("hdnProductTypeName") = "acs") Or ((Session("hdnProductTypeName") = "prc") And (ViewState("IsCabOnly") = True))) Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlCab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // Else
    // If hdnSEID.Text = 1 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBeam") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // ElseIf hdnSEID.Text = 2 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlColumn") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // 'ElseIf hdnSEID.Text = 4 Or hdnSEID.Text = 13 Then
    // '    Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlSlab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // ElseIf hdnSEID.Text = 5 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlDrain") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // ElseIf hdnSEID.Text = 8 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBore") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // Else
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlSlab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // End If
    // End If
    // End If
    // Catch ex As Exception
    // ErrorHandler.RaiseError(ex, strLogPath)
    // End Try
    // End Sub

    // Protected Sub gvGML_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvGML.RowDeleting
    // 'Try

    // '    Dim intGMid As Integer
    // '    intGMid = gvGML.DataKeys(e.RowIndex).Value.ToString()
    // '    objGMLInfo.GroupMarkId = intGMid
    // '    intOutput = objGMLDal.DelGroupMark(objGMLInfo)
    // '    BindGML()
    // '    If intOutput = 1 Then
    // '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Msg", "<script>alert('The selected Groupmarking has been deleted Successfully');</script>", False)
    // '    Else
    // '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Msg", "<script>alert('Cannot delete the groupmarking.It has been Posted already');</script>", False)
    // '    End If
    // 'Catch ex As Exception
    // '    ErrorHandler.RaiseError(ex, strLogPath)
    // 'End Try
    // End Sub
    // Protected Sub gvGML_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGML.RowCommand
    // Try
    // If e.CommandName = "Edit" Then
    // Dim objEdit As LinkButton = DirectCast(e.CommandSource, LinkButton)
    // Dim gvrGML As GridViewRow = DirectCast(objEdit.NamingContainer, GridViewRow)
    // Dim dsGMList As DataSet
    // Dim dsProductType As DataSet
    // Dim dsGMPost As DataSet = Nothing
    // hdnGroupMarkId = DirectCast(gvrGML.FindControl("hdnGroupMarkId"), HiddenField)
    // hdnProductype = DirectCast(gvrGML.FindControl("hdnProductype"), HiddenField)
    // Session("hdnGroupMarkId") = hdnGroupMarkId.Value
    // Session("hdnProductype") = hdnProductype.Value
    // lblGroupMarkName = DirectCast(gvrGML.FindControl("lblGroup"), Label)
    // Session("lblGroupMarkName") = lblGroupMarkName.Text
    // hdnParameter = DirectCast(gvrGML.FindControl("hdnParameter"), HiddenField)
    // Session("lblParamsetNo") = hdnParameter.Value
    // objGMLInfo.GroupMarkId = Convert.ToInt32(hdnGroupMarkId.Value)
    // objGMLInfo.ProductId = Convert.ToInt32(hdnProductype.Value)
    // dsGMList = objGMLDal.GetReleasedGroupMark(objGMLInfo)
    // dsProductType = objGMLDal.GetProductType(objGMLInfo)
    // Session("hdnProductTypeName") = dsProductType.Tables(0).Rows(0).Item("ProductType").ToString().ToLower().Trim()
    // ViewState("IsCabOnly") = CType(dsProductType.Tables(0).Rows(0).Item("Indicator"), Boolean)
    // objGMLInfo.GroupMarkId = Convert.ToInt32(hdnGroupMarkId.Value)
    // dsGMPost = objGMLDal.GetPostedGroupMark(objGMLInfo)
    // If (dsGMList.Tables.Count > 0) AndAlso (dsGMList.Tables(0).Rows.Count > 0) Then
    // ModalPopupExtender2.Show()
    // 'If hdnSEID.Text = 1 Then
    // '    Response.Redirect("~/BeamCage/Detail.aspx?GroupId=" & Convert.ToInt32(hdnGroupMarkId.Value))
    // 'ElseIf hdnSEID.Text = 2 Then
    // '    Response.Redirect("~/ColumnCage/ColumCage.aspx?GroupId=" & Convert.ToInt32(hdnGroupMarkId.Value))
    // 'ElseIf hdnSEID.Text = 4 Then
    // '    Response.Redirect("~/SlabWall/SlabWall.aspx?GroupId=" & Convert.ToInt32(hdnGroupMarkId.Value))
    // 'ElseIf hdnSEID.Text = 8 Then
    // '    Response.Redirect("~/BorePile/Detail.aspx?GroupId=" & Convert.ToInt32(hdnGroupMarkId.Value))
    // 'End If
    // 'If hdnProductype.Value = 4 Or hdnProductype.Value = 10 Then
    // '    Response.Redirect("~/CAB/Detail.aspx?GroupId=" & Convert.ToInt32(hdnGroupMarkId.Value))
    // 'End If
    // 'ElseIf dsGMPost.Tables(0).Rows.Count > 0 Then
    // '    ModalPopupExtender1.Show()
    // ElseIf (dsGMPost.Tables.Count > 0) And (dsGMPost.Tables(0).Rows.Count > 0) Then
    // '            ModalPopupExtender1.Show()
    // If ((Session("hdnProductTypeName") = "cab") Or (Session("hdnProductTypeName") = "acs") Or ((Session("hdnProductTypeName") = "prc") And (ViewState("IsCabOnly") = True))) Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlCab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // Else
    // If hdnSEID.Text = 1 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBeam") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // ElseIf hdnSEID.Text = 2 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlColumn") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // ' ''ElseIf hdnSEID.Text = 4 Then
    // ' ''    Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlSlab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // ElseIf hdnSEID.Text = 8 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBore") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)

    // Else
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlSlab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // End If
    // End If
    // 'If ((Session("hdnProductTypeName") = "cab") Or (Session("hdnProductTypeName") = "acs") Or ((Session("hdnProductTypeName") = "prc") And (ViewState("IsCabOnly") = True))) Then
    // '    Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlCab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)) & "&P=Y", False)
    // 'Else
    // '    If hdnSEID.Text = 1 Then
    // '        Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBeam") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)) & "&P=Y", False)
    // '    ElseIf hdnSEID.Text = 2 Then
    // '        Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlColumn") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)) & "&P=Y", False)
    // '    ElseIf hdnSEID.Text = 4 Then
    // '        Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlSlab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)) & "&P=Y", False)
    // '    ElseIf hdnSEID.Text = 8 Then
    // '        Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBore") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)) & "&P=Y", False)
    // '    End If
    // 'End If
    // Else
    // If ((Session("hdnProductTypeName") = "cab") Or (Session("hdnProductTypeName") = "acs") Or ((Session("hdnProductTypeName") = "prc") And (ViewState("IsCabOnly") = True))) Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlCab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // Else
    // If hdnSEID.Text = 1 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBeam") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // ElseIf hdnSEID.Text = 2 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlColumn") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // 'ElseIf hdnSEID.Text = 4 Then
    // '    Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlSlab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // ElseIf hdnSEID.Text = 8 Then
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlBore") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)
    // Else
    // Response.Redirect(GetGlobalResourceObject("GlobalResource", "UrlSlab") & "?GroupId=" & Server.UrlEncode(Convert.ToInt32(hdnGroupMarkId.Value)), False)

    // End If
    // End If
    // End If
    // End If
    // Catch ex As Exception
    // ErrorHandler.RaiseError(ex, strLogPath)
    // End Try

    // End Sub


    // Protected Sub gvGML_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvGML.Sorting
    // Try
    // ViewState("sortExpr") = e.SortExpression
    // If ViewState("SortDirection") = "ASC" Then
    // ViewState("SortDirection") = "DESC"
    // Else
    // ViewState("SortDirection") = "ASC"
    // End If
    // BindGML()
    // Catch ex As Exception
    // ErrorHandler.RaiseError(ex, strLogPath)
    // End Try

    // End Sub

    // Protected Sub gvGML_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGML.PageIndexChanging
    // Try
    // gvGML.PageIndex = e.NewPageIndex
    // BindGML()
    // Catch ex As Exception
    // ErrorHandler.RaiseError(ex, strLogPath)
    // End Try

    // End Sub
    // Protected Sub gvGML_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGML.RowDataBound
    // Try
    // Dim lnkDelete As LinkButton
    // Dim hdnPosted As HiddenField
    // If (e.Row.RowType = DataControlRowType.DataRow) Then
    // Dim chkSelection As CheckBox
    // Dim ProductType As Label
    // chkSelection = DirectCast(e.Row.FindControl("chkImport"), CheckBox)
    // ProductType = DirectCast(e.Row.FindControl("lblProdType"), Label)
    // lnkDelete = DirectCast(e.Row.FindControl("lnkDelete"), LinkButton)
    // hdnPosted = DirectCast(e.Row.FindControl("hdnPosted"), HiddenField)
    // If hdnPosted.Value = 1 Then
    // lnkDelete.Enabled = False
    // End If
    // chkSelection.Attributes.Add("OnClick", "ProductTypeCheck('" + ProductType.ClientID + "','" + chkSelection.ClientID + "')")
    // End If
    // 'For Each i As GridViewRow In gvGML.Rows
    // '    Dim chkImport As CheckBox = DirectCast(i.FindControl("chkImport"), CheckBox)
    // '    chkImport.Attributes.Add("onClick", "ClickchkImport(this);")
    // 'Next
    // Catch ex As Exception
    // ErrorHandler.RaiseError(ex, strLogPath)
    // End Try
    // End Sub

    //protected void Page_PreRender(object sender, System.EventArgs e)
    //{
    //    GridItem commandItem = RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)(0);
    //    CheckBox chkFilter = (CheckBox)commandItem.FindControl("chkFilter");
    //    if (chkFilter.Checked == true & System.Web.UI.Control.Page.IsPostBack == true & IsFromEdit == false)
    //        chkFilter.Checked = false;
    //}

    //protected void btnExportB_Click(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        ArrayList exportedTxtFilesList;
    //        strExport = "Batch";
    //        exportedTxtFilesList = Export();
    //        if (exportedTxtFilesList.Count == 0)
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "<script>alert('Please select a Groupmarking.');</script>", false);
    //            return;
    //        }
    //        string DirectoryPath;
    //        DirectoryPath = ExportPath;
    //        ZipFile z;
    //        string directory;
    //        directory = ExportPath + System.Guid.NewGuid().ToString() + ".zip";
    //        z = ZipFile.Create(directory);
    //        z.BeginUpdate();
    //        foreach (string item in exportedTxtFilesList)
    //            z.Add(item);
    //        z.CommitUpdate();
    //        z.Close();
    //        FileInfo file = new FileInfo(directory);

    //        System.Web.UI.Page.Response.ClearHeaders();
    //        System.Web.UI.Page.Response.Clear();
    //        System.Web.UI.Page.Response.ClearContent();
    //        System.Web.UI.Page.Response.ContentType = "text/xml";
    //        System.Web.UI.Page.Response.Charset = "";
    //        System.Web.UI.Page.Response.AddHeader("Content-Disposition", "attachment; filename=ExportedFiles_Batch_" + System.Guid.NewGuid().ToString() + ".zip");
    //        System.Web.UI.Page.Response.AddHeader("Content-Length", file.Length.ToString());
    //        System.Web.UI.Page.Response.WriteFile(file.FullName);
    //        System.Web.UI.Page.Response.Flush();
    //        System.Web.UI.Page.Response.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        errorLogNo = ErrorHandler.Publish(ex, System.Web.UI.Page.Request.Url.ToString());
    //        strErrorMsg = "alert('Please contact NDS Administrator with Error Log Ref Number:" + errorLogNo.ToString() + "' );";
    //        ScriptManager.RegisterStartupScript(this, System.Web.UI.Control.Page.GetType(), "ShowInfo", strErrorMsg, true);
    //    }
    //    finally
    //    {
    //        Export.Clear();
    //    }
    //}

    public int CopyGroupMarking(ReleaseGroupMarkDto groupMarkDto)
    {
        GroupMark groupMark = new GroupMark();
        int projectId;
        int sourceGroupMarkId;
        int destGroupMarkId = 0;
        int destRevisonNo = 0;
        string groupMarkName;
        string wbsElementIds;
        string copyFrom;
        int parameterSetId = 0;
        string copyGMResult;
        int copyPostedGM=0;
        string errorMessage = "";



            try
        {

           
            sourceGroupMarkId = groupMarkDto.INTGROUPMARKID;
            groupMarkName = groupMarkDto.VCHGROUPMARKINGNAME;//("GroupMarkName").ToString();
            projectId = groupMarkDto.INTPROJECTID;
            wbsElementIds = "ì";
            copyFrom = groupMarkDto.VCHGROUPMARKINGNAME;
            groupMark.StructureElementTypeId = groupMarkDto.INTSTRUCTUREELEMENTTYPEID;
            parameterSetId = groupMarkDto.TNTPARAMSETNUMBER;
            groupMark.SitProductTypeId = groupMarkDto.SITPRODUCTTYPEID;// Convert.ToInt32("Productype");
            groupMark.CreatedUserId = 1;

            // Using objTS As System.Transactions.TransactionScope = New System.Transactions.TransactionScope

            copyGMResult = groupMark.CopyGM_CopyGroupMarking(projectId, projectId, sourceGroupMarkId, groupMarkName, parameterSetId, parameterSetId, copyFrom, wbsElementIds, 1);
           
            
            if (copyGMResult.Contains(","))
            {
                string[] Output = copyGMResult.Split(",");
                destGroupMarkId = Convert.ToInt32(Output[0]);
                destRevisonNo = Convert.ToInt32(Output[1]);
            }
            
             // If the GM's posted, Update the Poste Header Details to latest Group Mark details. 
             copyPostedGM=groupMark.CopyGM_UpdatePostedGMforRevisedGM(sourceGroupMarkId, destGroupMarkId, destRevisonNo);

            
            }


        // End Using

        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
        return destGroupMarkId;


    }
}
