
using Dapper;
using Microsoft.Data.SqlClient;

using System.Data;
using WBSService.Constants;

namespace WBSService.Repositories
{
    public class CapClink
    {
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=NDSWebApps;Password=NDS4DBAdmin*;Connection Timeout=36000000";
        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";



        #region Properties

        public int WBSElementId { get; set; }
        public int ProductTypeId { get; set; }
        public int StructureElementTypeId { get; set; }
        public bool Exist { get; set; }
        public Customer Customer { get; set; }
        public Contract Contract { get; set; }
        public Project Project { get; set; }
        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public StructureElement StructureElement { get; set; }
        public ProductType ProductType { get; set; }
        public ShapeCode ShapeCode { get; set; }
        public ProductCode ProductCode { get; set; }
        public int ParentId { get; set; }
        public int ProductCodeId { get; set; }
        //public string ProductCode { get; set; }
        public int Width { get; set; }
        public int Depth { get; set; }
        public int MWLength { get; set; }
        public int CWLength { get; set; }
        public int Qty { get; set; }
        public int RevNo { get; set; }
        public string AddFlag { get; set; }
        public string ShapeId { get; set; }
        public int MO1 { get; set; }
        public int MO2 { get; set; }
        public int CO1 { get; set; }
        public int CO2 { get; set; }
        public string CapProduct { get; set; }
        public int Count { get; set; }
        public string Type { get; set; }
        public int MWQty { get; set; }
        public int CWQty { get; set; }
        public int MWSpace { get; set; }
        public int CWSpace { get; set; }
        public decimal InvoiceMWWeight { get; set; }
        public decimal InvoiceCWWeight { get; set; }
        public decimal TheoriticalWeight { get; set; }
        public int Area { get; set; }
        public int SMID { get; set; }
        public int PMID { get; set; }
        public int ShapeCodes { get; set; }
        public string ShapeCodeName { get; set; }
        public string SlabParentValue { get; set; }
        //Usp_PostingInsertCLinkProductMarking
        //usp_PostingINSERTCAPPRODUCTMARKING
        #endregion

        //public bool CapClinkExist(int PostHeaderId)
        //{
        //    bool isSuccess = false;
        //    DBManager dbManager = new DBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(3);
        //        dbManager.AddParameters(0, "@INTPOSTHEADERID", PostHeaderId);
        //        dbManager.AddParameters(1, "@INTSTRUCTUREELEMENTTYPEID", this.StructureElementTypeId);
        //        dbManager.AddParameters(2, "@PRODUCTTYPEID", this.ProductTypeId);
        //        this.Exist = Convert.ToBoolean(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_PostingCapClinkExists_Get"));
        //        isSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //    return isSuccess;
        //}

        //public List<CapClink> CappingHeaderInfo_Get()
        //{
        //    DBManager dbManager = new DBManager();
        //    List<CapClink> CapClinkList = new List<CapClink>();
        //    DataSet dsCapClink = new DataSet();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);
        //        dbManager.AddParameters(0, "@WBSELEMENTID", this.WBSElementId);
        //        dbManager.AddParameters(1, "@INTPARENT", this.ParentId);
        //        dsCapClink = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_PostingCappingHeaderInfo_Get");

        //        if (dsCapClink != null && dsCapClink.Tables.Count != 0)
        //        {
        //            foreach (DataRowView drvBeam in dsCapClink.Tables[0].DefaultView)
        //            {
        //                //For Customer
        //                Customer objCustomer = new Customer();
        //                List<Customer> listCustomer = new List<Customer>();
        //                string listCustomerCache = "listCustomerCache";
        //                if (IndexusDistributionCache.SharedCache.Get(listCustomerCache) == null)
        //                {
        //                    listCustomer = objCustomer.GetCustomer();
        //                }
        //                else
        //                {
        //                    listCustomer = IndexusDistributionCache.SharedCache.Get(listCustomerCache) as List<Customer>;
        //                }
        //                listCustomer = listCustomer.FindAll(x => x.CustomerId == Convert.ToInt32((drvBeam["INTCUSTOMERCODE"])));

        //                if (listCustomer.Count != 0)
        //                {
        //                    objCustomer.CustomerId = Convert.ToInt32((drvBeam["INTCUSTOMERCODE"]));
        //                    objCustomer.CustomerName = Convert.ToString((drvBeam["VCHCUSTOMERNAME"]));
        //                }

        //                //For Contract
        //                Contract objContract = new Contract();
        //                List<Contract> listContract = new List<Contract>();
        //                string listContractCache = "listContractCache";
        //                if (IndexusDistributionCache.SharedCache.Get(listContractCache) == null)
        //                {
        //                    listContract = objContract.GetContract(objCustomer.CustomerId);
        //                }
        //                else
        //                {
        //                    listContract = IndexusDistributionCache.SharedCache.Get(listContractCache) as List<Contract>;
        //                }
        //                listContract = listContract.FindAll(x => x.ContractId == Convert.ToInt32((drvBeam["INTCONTRACTID"])));

        //                if (listContract.Count != 0)
        //                {
        //                    objContract.ContractId = Convert.ToInt32((drvBeam["INTCONTRACTID"]));
        //                    objContract.ContractName = Convert.ToString((drvBeam["VCHCONTRACTNUMBER"]));
        //                    objContract.ContractDescription = Convert.ToString((drvBeam["VCHNDSCONTRACTDESCRIPTION"]));
        //                }

        //                //For Project
        //                Project objProject = new Project();
        //                List<Project> listProject = new List<Project>();
        //                string listProjectCache = "listProjectCache";
        //                if (IndexusDistributionCache.SharedCache.Get(listProjectCache) == null)
        //                {
        //                    listProject = objProject.GetProject(objContract.ContractId);
        //                }
        //                else
        //                {
        //                    listProject = IndexusDistributionCache.SharedCache.Get(listProjectCache) as List<Project>;
        //                }
        //                listProject = listProject.FindAll(x => x.ProjectId == Convert.ToInt32((drvBeam["INTPROJECTID"])));

        //                if (listProject.Count != 0)
        //                {
        //                    objProject.ProjectId = Convert.ToInt32((drvBeam["INTPROJECTID"]));
        //                    objProject.ProjectName = Convert.ToString((drvBeam["VCHPROJECTNAME"]));
        //                }

        //                //For StructureElement
        //                StructureElement objStructureElement = new StructureElement();
        //                List<StructureElement> listStructureElement = new List<StructureElement>();
        //                string listStructureElementCache = "listStructureElementCache";
        //                if (IndexusDistributionCache.SharedCache.Get(listStructureElementCache) == null)
        //                {
        //                    listStructureElement = objStructureElement.GetStructureElement();
        //                }
        //                else
        //                {
        //                    listStructureElement = IndexusDistributionCache.SharedCache.Get(listStructureElementCache) as List<StructureElement>;
        //                }
        //                listStructureElement = listStructureElement.FindAll(x => x.StructureElementTypeId == Convert.ToInt32((drvBeam["INTSTRUCTUREELEMENTTYPEID"])));

        //                if (listStructureElement.Count != 0)
        //                {
        //                    objStructureElement.StructureElementTypeId = Convert.ToInt32((drvBeam["INTSTRUCTUREELEMENTTYPEID"]));
        //                    objStructureElement.StructureElementType = Convert.ToString((drvBeam["VCHSTRUCTUREELEMENTTYPE"]));
        //                }

        //                //For ProductType
        //                ProductType objProductType = new ProductType();
        //                List<ProductType> listProductType = new List<ProductType>();
        //                if (IndexusDistributionCache.SharedCache.Get(listStructureElementCache) == null)
        //                {
        //                    listProductType = objProductType.GetProductType();
        //                }
        //                else
        //                {
        //                    listProductType = IndexusDistributionCache.SharedCache.Get(listStructureElementCache) as List<ProductType>;
        //                }
        //                listProductType = listProductType.FindAll(x => x.ProductTypeId == Convert.ToInt32((drvBeam["SITPRODUCTTYPEID"])));

        //                if (listProductType.Count != 0)
        //                {
        //                    objProductType.ProductTypeId = Convert.ToInt32((drvBeam["SITPRODUCTTYPEID"]));
        //                    objProductType.ProductTypeName = Convert.ToString((drvBeam["VCHPRODUCTTYPEL1"]));
        //                }

        //                CapClink CapClink = new CapClink
        //                {
        //                    Customer = objCustomer,
        //                    Contract = objContract,
        //                    Project = objProject,
        //                    StructureElement = objStructureElement,
        //                    ProductType = objProductType,
        //                    WBS1 = Convert.ToString((drvBeam["VCHWBS1"])),
        //                    WBS2 = Convert.ToString((drvBeam["VCHWBS2"])),
        //                    WBS3 = Convert.ToString((drvBeam["VCHWBS3"])),
        //                    WBSElementId = Convert.ToInt32((drvBeam["INTWBSELEMENTID"]))
        //                };
        //                //fix me
        //                CapClinkList.Add(CapClink);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return CapClinkList;
        //}

        //public List<CapClink> CLinkHeaderInfo_Get()
        //{
        //    DBManager dbManager = new DBManager();
        //    List<CapClink> CapClinkList = new List<CapClink>();
        //    DataSet dsCapClink = new DataSet();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);
        //        dbManager.AddParameters(0, "@WBSELEMENTID", this.WBSElementId);
        //        dbManager.AddParameters(1, "@INTPARENT", this.ParentId);
        //        dsCapClink = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_PostingCLinkHeaderInfo_Get");

        //        if (dsCapClink != null && dsCapClink.Tables.Count != 0)
        //        {
        //            foreach (DataRowView drvBeam in dsCapClink.Tables[0].DefaultView)
        //            {
        //                //For Customer
        //                Customer objCustomer = new Customer();
        //                List<Customer> listCustomer = new List<Customer>();
        //                string listCustomerCache = "listCustomerCache";
        //                if (IndexusDistributionCache.SharedCache.Get(listCustomerCache) == null)
        //                {
        //                    listCustomer = objCustomer.GetCustomer();
        //                }
        //                else
        //                {
        //                    listCustomer = IndexusDistributionCache.SharedCache.Get(listCustomerCache) as List<Customer>;
        //                }
        //                listCustomer = listCustomer.FindAll(x => x.CustomerId == Convert.ToInt32((drvBeam["INTCUSTOMERCODE"])));

        //                if (listCustomer.Count != 0)
        //                {
        //                    objCustomer.CustomerId = Convert.ToInt32((drvBeam["INTCUSTOMERCODE"]));
        //                    objCustomer.CustomerName = Convert.ToString((drvBeam["VCHCUSTOMERNAME"]));
        //                }

        //                //For Contract
        //                Contract objContract = new Contract();
        //                List<Contract> listContract = new List<Contract>();
        //                string listContractCache = "listContractCache";
        //                if (IndexusDistributionCache.SharedCache.Get(listContractCache) == null)
        //                {
        //                    listContract = objContract.GetContract(objCustomer.CustomerId);
        //                }
        //                else
        //                {
        //                    listContract = IndexusDistributionCache.SharedCache.Get(listContractCache) as List<Contract>;
        //                }
        //                listContract = listContract.FindAll(x => x.ContractId == Convert.ToInt32((drvBeam["INTCONTRACTID"])));

        //                if (listContract.Count != 0)
        //                {
        //                    objContract.ContractId = Convert.ToInt32((drvBeam["INTCONTRACTID"]));
        //                    objContract.ContractName = Convert.ToString((drvBeam["VCHCONTRACTNUMBER"]));
        //                    objContract.ContractDescription = Convert.ToString((drvBeam["VCHNDSCONTRACTDESCRIPTION"]));
        //                }

        //                //For Project
        //                Project objProject = new Project();
        //                List<Project> listProject = new List<Project>();
        //                string listProjectCache = "listProjectCache";
        //                if (IndexusDistributionCache.SharedCache.Get(listProjectCache) == null)
        //                {
        //                    listProject = objProject.GetProject(objContract.ContractId);
        //                }
        //                else
        //                {
        //                    listProject = IndexusDistributionCache.SharedCache.Get(listProjectCache) as List<Project>;
        //                }
        //                listProject = listProject.FindAll(x => x.ProjectId == Convert.ToInt32((drvBeam["INTPROJECTID"])));

        //                if (listProject.Count != 0)
        //                {
        //                    objProject.ProjectId = Convert.ToInt32((drvBeam["INTPROJECTID"]));
        //                    objProject.ProjectName = Convert.ToString((drvBeam["VCHPROJECTNAME"]));
        //                }

        //                //For StructureElement
        //                StructureElement objStructureElement = new StructureElement();
        //                List<StructureElement> listStructureElement = new List<StructureElement>();
        //                string listStructureElementCache = "listStructureElementCache";
        //                if (IndexusDistributionCache.SharedCache.Get(listStructureElementCache) == null)
        //                {
        //                    listStructureElement = objStructureElement.GetStructureElement();
        //                }
        //                else
        //                {
        //                    listStructureElement = IndexusDistributionCache.SharedCache.Get(listStructureElementCache) as List<StructureElement>;
        //                }
        //                listStructureElement = listStructureElement.FindAll(x => x.StructureElementTypeId == Convert.ToInt32((drvBeam["INTSTRUCTUREELEMENTTYPEID"])));

        //                if (listStructureElement.Count != 0)
        //                {
        //                    objStructureElement.StructureElementTypeId = Convert.ToInt32((drvBeam["INTSTRUCTUREELEMENTTYPEID"]));
        //                    objStructureElement.StructureElementType = Convert.ToString((drvBeam["VCHSTRUCTUREELEMENTTYPE"]));
        //                }

        //                //For ProductType
        //                ProductType objProductType = new ProductType();
        //                List<ProductType> listProductType = new List<ProductType>();
        //                if (IndexusDistributionCache.SharedCache.Get(listStructureElementCache) == null)
        //                {
        //                    listProductType = objProductType.GetProductType();
        //                }
        //                else
        //                {
        //                    listProductType = IndexusDistributionCache.SharedCache.Get(listStructureElementCache) as List<ProductType>;
        //                }
        //                listProductType = listProductType.FindAll(x => x.ProductTypeId == Convert.ToInt32((drvBeam["SITPRODUCTTYPEID"])));

        //                if (listProductType.Count != 0)
        //                {
        //                    objProductType.ProductTypeId = Convert.ToInt32((drvBeam["SITPRODUCTTYPEID"]));
        //                    objProductType.ProductTypeName = Convert.ToString((drvBeam["VCHPRODUCTTYPEL1"]));
        //                }

        //                CapClink CapClink = new CapClink
        //                {
        //                    Customer = objCustomer,
        //                    Contract = objContract,
        //                    Project = objProject,
        //                    StructureElement = objStructureElement,
        //                    ProductType = objProductType,
        //                    WBS1 = Convert.ToString((drvBeam["VCHWBS1"])),
        //                    WBS2 = Convert.ToString((drvBeam["VCHWBS2"])),
        //                    WBS3 = Convert.ToString((drvBeam["VCHWBS3"])),
        //                    WBSElementId = Convert.ToInt32((drvBeam["INTWBSELEMENTID"]))
        //                };
        //                //fix me
        //                CapClinkList.Add(CapClink);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return CapClinkList;
        //}



        //public bool SaveClinkDetails(int UserId, int postHeaderId)
        //{
        //    bool isSuccess = false;
        //    DBManager dbManager = new DBManager();
        //    try
        //    {

        //        dbManager.Open();
        //        dbManager.CreateParameters(18);
        //        dbManager.AddParameters(0, "@INTWBSELEMENTID", this.WBSElementId);
        //        dbManager.AddParameters(1, "@INTPRODUCTCODEID", this.ProductCode.ProductCodeId);
        //        dbManager.AddParameters(2, "@INTWIDTH", this.Width);
        //        dbManager.AddParameters(3, "@INTDEPTH", this.Depth);
        //        dbManager.AddParameters(4, "@INTMWLENGTH", this.MWLength);
        //        dbManager.AddParameters(5, "@INTCWLENGTH", this.CWLength);
        //        dbManager.AddParameters(6, "@INTQTY", this.Qty);
        //        dbManager.AddParameters(7, "@INTREVNO", this.RevNo);
        //        dbManager.AddParameters(8, "@CHRADDFLAG", this.AddFlag);
        //        dbManager.AddParameters(9, "@STRSHAPEID", this.ShapeCode.ShapeCodeName);
        //        dbManager.AddParameters(10, "@MO1", this.MO1);
        //        dbManager.AddParameters(11, "@MO2", this.MO2);
        //        dbManager.AddParameters(12, "@CO1", this.CO1);
        //        dbManager.AddParameters(13, "@CO2", this.CO2);
        //        dbManager.AddParameters(14, "@STRUCTUREELEMENTID", this.StructureElementTypeId);
        //        dbManager.AddParameters(15, "@PRODUCTTYPEL1ID", this.ProductTypeId);
        //        dbManager.AddParameters(16, "@USERID", UserId);
        //        dbManager.AddParameters(17, "@INTPOSTHEADERID", postHeaderId);
        //        dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_PostingInsertClinkCCLMarkDetails_Insert");
        //        isSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //    return isSuccess;
        //}

        //public bool DeleteCapStructure(int PostHeaderId)
        //{
        //    bool isSuccess = false;
        //    DBManager dbManager = new DBManager();
        //    try
        //    {

        //        dbManager.Open();
        //        dbManager.CreateParameters(5);
        //        dbManager.AddParameters(0, "@POSTHEADERID", PostHeaderId);
        //        dbManager.AddParameters(1, "@VCHBEAMPRODUCTCODE", this.ProductCode.ProductCodeName);
        //        dbManager.AddParameters(2, "@INTWIDTH", this.Width);
        //        dbManager.AddParameters(3, "@STRSHAPEID", this.ShapeCode.ShapeCodeName);
        //        dbManager.AddParameters(4, "@STRSMID", this.SMID);
        //        dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_PostingCapStructure_Delete");
        //        isSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //    return isSuccess;
        //}

        //public bool DeleteClinkStructure(int PostHeaderId)
        //{
        //    bool isSuccess = false;
        //    DBManager dbManager = new DBManager();
        //    try
        //    {

        //        dbManager.Open();
        //        dbManager.CreateParameters(5);
        //        dbManager.AddParameters(0, "@INTPOSTHEADERID", PostHeaderId);
        //        dbManager.AddParameters(1, "@VCHBEAMPRODUCTCODE", this.ProductCode.ProductCodeName);
        //        dbManager.AddParameters(2, "@INTWIDTH", this.Width);
        //        dbManager.AddParameters(3, "@STRSHAPEID", this.ShapeCode.ShapeCodeName);
        //        dbManager.AddParameters(4, "@STRSMID", this.SMID);
        //        dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_PostingCLinkStructure_Delete");
        //        isSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //    return isSuccess;
        //}

        //public List<CapClink> CapMOCO_Get(int PostHeaderId)
        //{
        //    DBManager dbManager = new DBManager();
        //    List<CapClink> listMOCO = new List<CapClink> { };
        //    DataSet dsMOCO = new DataSet();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(4);
        //        dbManager.AddParameters(0, "@INTPOSTHEADERID", PostHeaderId);
        //        dbManager.AddParameters(1, "@MWLength", this.MWLength);
        //        dbManager.AddParameters(2, "@CapProduct", this.CapProduct);
        //        dbManager.AddParameters(3, "@CWlength", this.CWLength);
        //        dsMOCO = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_PostingCappingMO1MO2CO1Co2_Get");
        //        if (dsMOCO != null && dsMOCO.Tables.Count != 0)
        //        {
        //            foreach (DataRowView drvBBSDescription in dsMOCO.Tables[0].DefaultView)
        //            {
        //                CapClink capClink = new CapClink
        //                {
        //                    MO1 = Convert.ToInt32(drvBBSDescription["MO1"]),
        //                    MO2 = Convert.ToInt32(drvBBSDescription["MO2"]),
        //                    CO1 = Convert.ToInt32(drvBBSDescription["CO1"]),
        //                    CO2 = Convert.ToInt32(drvBBSDescription["CO2"])
        //                };
        //                listMOCO.Add(capClink);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //    return listMOCO;
        //}

        //public List<CapClink> ClinkMOCO_Get(int PostHeaderId)
        //{
        //    DBManager dbManager = new DBManager();
        //    List<CapClink> listMOCO = new List<CapClink> { };
        //    DataSet dsMOCO = new DataSet();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(4);
        //        dbManager.AddParameters(0, "@INTPOSTHEADERID", PostHeaderId);
        //        dbManager.AddParameters(1, "@MWLength", this.MWLength);
        //        dbManager.AddParameters(2, "@CapProduct", this.CapProduct);
        //        dbManager.AddParameters(3, "@CWlength", this.CWLength);
        //        dsMOCO = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_PostingClinkMO1MO2CO1Co2_Get");
        //        if (dsMOCO != null && dsMOCO.Tables.Count != 0)
        //        {
        //            foreach (DataRowView drvBBSDescription in dsMOCO.Tables[0].DefaultView)
        //            {
        //                CapClink capClink = new CapClink
        //                {
        //                    MO1 = Convert.ToInt32(drvBBSDescription["MO1"]),
        //                    MO2 = Convert.ToInt32(drvBBSDescription["MO2"]),
        //                    CO1 = Convert.ToInt32(drvBBSDescription["CO1"]),
        //                    CO2 = Convert.ToInt32(drvBBSDescription["CO2"])
        //                };
        //                listMOCO.Add(capClink);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //    return listMOCO;
        //}

        public List<CapClink> Capping_Get(int PostHeaderId)
        {

            List<CapClink> listCapping = new List<CapClink> { };
            DataSet dsCapping = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPOSTHEADERID", PostHeaderId);
                    listCapping = (List<CapClink>)sqlConnection.Query<CapClink>(SystemConstant.usp_PostingCappingInfo_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    //listCapping;
                }



                if (dsCapping != null && dsCapping.Tables.Count != 0)
                {
                    foreach (DataRowView drvCappingInfo in dsCapping.Tables[0].DefaultView)
                    {

                        //For StructureElement
                        StructureElement objStructureElement = new StructureElement();
                        List<StructureElement> listStructureElement = new List<StructureElement>();
                        string listStructureElementCache = "listStructureElementCache";
                        //if (IndexusDistributionCache.SharedCache.Get(listStructureElementCache) == null)
                        //{
                        //    listStructureElement = objStructureElement.GetStructureElement();
                        //}
                        //else
                        //{
                        //    listStructureElement = IndexusDistributionCache.SharedCache.Get(listStructureElementCache) as List<StructureElement>;
                        //}
                        listStructureElement = listStructureElement.FindAll(x => x.StructureElementType == Convert.ToString((drvCappingInfo["STRUCTUREELEMENT"])));

                        if (listStructureElement.Count != 0)
                        {
                            objStructureElement.StructureElementType = Convert.ToString((drvCappingInfo["StructureElement"]));
                        }
                        ShapeCode objShapeCode = new ShapeCode();
                        List<ShapeCode> listShapeCode = new List<ShapeCode>();
                        listShapeCode = objShapeCode.ShapeCodeNoFilter_Get();
                        listShapeCode = listShapeCode.FindAll(x => x.ShapeCodeName == Convert.ToString((drvCappingInfo["ShapeCode"])));
                        if (listShapeCode.Count != 0)
                        {
                            objShapeCode.ShapeCodeName = Convert.ToString((drvCappingInfo["ShapeCode"]));
                            objShapeCode.ShapeID = listShapeCode[0].ShapeID;
                        }
                        else
                        {
                            objShapeCode.ShapeCodeName = "";
                            objShapeCode.ShapeID = 0;
                        }
                        ProductCode objProductCode = new ProductCode();
                        List<ProductCode> listProductCode = new List<ProductCode>();
                        listProductCode = objProductCode.CapProductCodeNoFilter_Get();
                        listProductCode = listProductCode.FindAll(x => x.ProductCodeName == Convert.ToString((drvCappingInfo["CAPPRODUCT"])));
                        if (listProductCode.Count != 0)
                        {
                            objProductCode.ProductCodeName = Convert.ToString((drvCappingInfo["CAPPRODUCT"]));
                            objProductCode.ProductCodeId = listProductCode[0].ProductCodeId;
                        }
                        else
                        {
                            objProductCode.ProductCodeName = "";
                            objProductCode.ProductCodeId = 0;
                        }

                        CapClink capClink = new CapClink
                        {
                            Width = Convert.ToInt32(drvCappingInfo["WIDTH"]),
                            Depth = Convert.ToInt32(drvCappingInfo["DEPTH"]),
                            Count = Convert.ToInt32(drvCappingInfo["COUNT"]),
                            Type = Convert.ToString(drvCappingInfo["TYPE"]),
                            StructureElement = objStructureElement,
                            MWLength = Convert.ToInt32(drvCappingInfo["MWLENGTH"]),
                            CWQty = Convert.ToInt32(drvCappingInfo["CWQTY"]),
                            MO1 = Convert.ToInt32(drvCappingInfo["MO1"]),
                            MO2 = Convert.ToInt32(drvCappingInfo["MO2"]),
                            CWSpace = Convert.ToInt32(drvCappingInfo["CWSPACE"]),
                            MWSpace = Convert.ToInt32(drvCappingInfo["MWSPACE"]),
                            CWLength = Convert.ToInt32(drvCappingInfo["LENGTH"]),
                            CO1 = Convert.ToInt32(drvCappingInfo["CO1"]),
                            CO2 = Convert.ToInt32(drvCappingInfo["CO2"]),
                            MWQty = Convert.ToInt32(drvCappingInfo["MWQTY"]),
                            InvoiceCWWeight = Convert.ToDecimal(drvCappingInfo["INVOICECWWEIGHT"]),
                            InvoiceMWWeight = Convert.ToDecimal(drvCappingInfo["INVOICEMWWEIGHT"]),
                            TheoriticalWeight = Convert.ToDecimal(drvCappingInfo["THEORITICALWEIGHT"]),
                            Area = Convert.ToInt32(drvCappingInfo["AREA"]),
                            SMID = Convert.ToInt32(drvCappingInfo["SMID"]),
                            PMID = Convert.ToInt32(drvCappingInfo["PMID"]),
                            ShapeCode = objShapeCode,
                            ProductCode = objProductCode,
                            Qty = Convert.ToInt32(drvCappingInfo["COUNT"])
                        };
                        listCapping.Add(capClink);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{

            //}
            return listCapping;
        }

        //public List<CapClink> CLink_Get(int PostHeaderId)
        //{
        //    DBManager dbManager = new DBManager();
        //    List<CapClink> listClink = new List<CapClink> { };
        //    DataSet dsClink = new DataSet();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(1);
        //        dbManager.AddParameters(0, "@INTPOSTHEADERID", PostHeaderId);
        //        dsClink = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_PostingClinkInfo_Get");
        //        if (dsClink != null && dsClink.Tables.Count != 0)
        //        {
        //            foreach (DataRowView drvCappingInfo in dsClink.Tables[0].DefaultView)
        //            {

        //                //For StructureElement
        //                StructureElement objStructureElement = new StructureElement();
        //                List<StructureElement> listStructureElement = new List<StructureElement>();
        //                string listStructureElementCache = "listStructureElementCache";
        //                if (IndexusDistributionCache.SharedCache.Get(listStructureElementCache) == null)
        //                {
        //                    listStructureElement = objStructureElement.GetStructureElement();
        //                }
        //                else
        //                {
        //                    listStructureElement = IndexusDistributionCache.SharedCache.Get(listStructureElementCache) as List<StructureElement>;
        //                }
        //                listStructureElement = listStructureElement.FindAll(x => x.StructureElementType == Convert.ToString((drvCappingInfo["StructureElement"])));

        //                if (listStructureElement.Count != 0)
        //                {
        //                    objStructureElement.StructureElementType = Convert.ToString((drvCappingInfo["StructureElement"]));
        //                }
        //                ShapeCode objShapeCode = new ShapeCode();
        //                List<ShapeCode> listShapeCode = new List<ShapeCode>();
        //                listShapeCode = objShapeCode.ClinkShapeCodeNoFilter_Get();
        //                listShapeCode = listShapeCode.FindAll(x => x.ShapeCodeName == Convert.ToString((drvCappingInfo["ShapeCode"])));
        //                if (listShapeCode.Count != 0)
        //                {
        //                    objShapeCode.ShapeCodeName = Convert.ToString((drvCappingInfo["ShapeCode"]));
        //                    objShapeCode.ShapeID = listShapeCode[0].ShapeID;
        //                }

        //                ProductCode objProductCode = new ProductCode();
        //                List<ProductCode> listProductCode = new List<ProductCode>();
        //                listProductCode = objProductCode.CLinkProductCodeNoFilter_Get();
        //                listProductCode = listProductCode.FindAll(x => x.ProductCodeName == Convert.ToString((drvCappingInfo["CLINKPRODUCT"])));
        //                if (listProductCode.Count != 0)
        //                {
        //                    objProductCode.ProductCodeName = Convert.ToString((drvCappingInfo["CLINKPRODUCT"]));
        //                    objProductCode.ProductCodeId = listProductCode[0].ProductCodeId;
        //                }

        //                CapClink Clink = new CapClink
        //                {
        //                    Width = Convert.ToInt32(drvCappingInfo["WIDTH"]),
        //                    Depth = Convert.ToInt32(drvCappingInfo["DEPTH"]),
        //                    Count = Convert.ToInt32(drvCappingInfo["COUNT"]),
        //                    Type = Convert.ToString(drvCappingInfo["TYPE"]),
        //                    StructureElement = objStructureElement,
        //                    MWLength = Convert.ToInt32(drvCappingInfo["MWLENGTH"]),
        //                    CWQty = Convert.ToInt32(drvCappingInfo["CWQTY"]),
        //                    MO1 = Convert.ToInt32(drvCappingInfo["MO1"]),
        //                    MO2 = Convert.ToInt32(drvCappingInfo["MO2"]),
        //                    CWSpace = Convert.ToInt32(drvCappingInfo["CWSPACE"]),
        //                    MWSpace = Convert.ToInt32(drvCappingInfo["MWSPACE"]),
        //                    CWLength = Convert.ToInt32(drvCappingInfo["LENGTH"]),
        //                    CO1 = Convert.ToInt32(drvCappingInfo["CO1"]),
        //                    CO2 = Convert.ToInt32(drvCappingInfo["CO2"]),
        //                    MWQty = Convert.ToInt32(drvCappingInfo["MWQTY"]),
        //                    InvoiceCWWeight = Convert.ToDecimal(drvCappingInfo["INVOICECWWEIGHT"]),
        //                    InvoiceMWWeight = Convert.ToDecimal(drvCappingInfo["INVOICEMWWEIGHT"]),
        //                    TheoriticalWeight = Convert.ToDecimal(drvCappingInfo["THEORITICALWEIGHT"]),
        //                    Area = Convert.ToInt32(drvCappingInfo["AREA"]),
        //                    SMID = Convert.ToInt32(drvCappingInfo["SMID"]),
        //                    PMID = Convert.ToInt32(drvCappingInfo["PMID"]),
        //                    ShapeCode = objShapeCode,
        //                    ProductCode = objProductCode,
        //                    Qty = Convert.ToInt32(drvCappingInfo["COUNT"])
        //                };
        //                listClink.Add(Clink);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //    return listClink;
        //}

        //public bool SlabParent(string GroupMarkIdString, int ProjectId)
        //{
        //    bool isSuccess = false;
        //    DBManager dbManager = new DBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(3);
        //        dbManager.AddParameters(0, "@intProjectId", ProjectId);
        //        dbManager.AddParameters(1, "@intGroupMarkId", GroupMarkIdString);
        //        dbManager.AddParameters(2, "@sitProductTypeId", this.ProductTypeId);
        //        this.SlabParentValue = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_PostingSlabHasChild_GET"));
        //        isSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //    return isSuccess;
        //}

        public bool SaveCappingDetails(int UserId, int postHeaderId, out int PMID, out int SMID)
        {
            bool isSuccess = false;
            int Output = 0;
            PMID = 0;
            SMID = 0;
            try
            {
                DataSet dsCapClink = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTWBSELEMENTID", this.WBSElementId);
                    //dynamicParameters.Add("@INTPRODUCTCODEID", this.ProductCode.ProductCodeId);//by vidya
                    dynamicParameters.Add("@INTPRODUCTCODEID", this.ProductCodeId);



                    dynamicParameters.Add("@INTWIDTH", this.Width);
                    dynamicParameters.Add("@INTDEPTH", this.Depth);
                    dynamicParameters.Add("@INTMWLENGTH", this.MWLength);
                    dynamicParameters.Add("@INTCWLENGTH", this.CWLength);
                    dynamicParameters.Add("@INTQTY", this.Qty);
                    dynamicParameters.Add("@INTREVNO", this.RevNo);
                    dynamicParameters.Add("@CHRADDFLAG", this.AddFlag);
                    //dynamicParameters.Add("@STRSHAPEID", this.ShapeCode.ShapeCodeName);  //by vidya
                    dynamicParameters.Add("@STRSHAPEID", this.ShapeCodeName);



                    dynamicParameters.Add("@MO1", this.MO1);
                    dynamicParameters.Add("@MO2", this.MO2);
                    dynamicParameters.Add("@CO1", this.CO1);
                    dynamicParameters.Add("@CO2", this.CO2);
                    dynamicParameters.Add("@STRUCTUREELEMENTID", this.StructureElementTypeId);
                    dynamicParameters.Add("@PRODUCTTYPEL1ID", this.ProductTypeId);
                    dynamicParameters.Add("@USERID", UserId);
                    dynamicParameters.Add("@INTPOSTHEADERID", postHeaderId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstant.PostingInsertCCLMarkDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();

                }


                if ((dsCapClink != null) && (dsCapClink.Tables.Count != 0))
                {
                    PMID = Convert.ToInt32(dsCapClink.Tables[0].Rows[0]["ProductMarkID"]);
                    SMID = Convert.ToInt32(dsCapClink.Tables[0].Rows[0]["StructureMarkID"]);
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }
    }




}
