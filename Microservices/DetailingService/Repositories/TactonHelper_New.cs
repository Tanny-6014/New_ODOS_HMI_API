//
//using NatSteel.Tacton.BLL;

using System.Collections.Specialized;
using System.Collections;
using System.Data;
//using System.IO;
//using System.Xml;


using DetailingService.Interfaces;
using Tacton.Configurator.ObjectModel;
using Tacton.Configurator.Public;
using Tacton.Configurator.Helpers;
using Tacton.Configurator.Interfaces;
using Tacton.Configurator.Core;
using DataAccessor = Tacton.Configurator.Interfaces.DataAccessor;
using System.Xml;
using DetailingService.Dtos;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;

namespace DetailingService.Repositories
{
    public class TactonHelper_New
    {
        #region "Declaration"

        Configuration configuration = null;
        string tactonServer = "//172.25.1.141:9090/NDSTactonAccess1,//172.25.1.141:9090/NDSTactonAccess2,//172.25.1.141:9090/NDSTactonAccess3,//172.25.1.141:9090/NDSTactonAccess4,//172.25.1.141:9090/NDSTactonAccess5";
        string strBeamStructureTcxPath = "D:\\NDSDev_2012\\NDS_2012\\Web\\beamcage\\data\\";//System.Configuration.ConfigurationManager.AppSettings["TCX_Folder_Path_Beam"].ToString();
        string strColumnStructureTcxPath = "D:\\NDSDev_2012\\NDS_2012\\Web\\columnCage\\data\\";//System.Configuration.ConfigurationManager.AppSettings["TCX_Folder_Path_Column"].ToString();
        string strSlabStructureTcxPath = "D:\\TTL_TFS\\ODOS\\ODOS_API\\Microservices\\DetailingService\\SlabWall\\data\\";  //"D:\\NDSDev_2012\\NDS_2012\\Web\\slabwall\\data\\";//System.Configuration.ConfigurationManager.AppSettings["TCX_Folder_Path_Slab"].ToString();
        string strCarpetStructureTcxPath = "D:\\NDSDev_2012\\NDS_2012\\Web\\slabwall\\data\\";//System.Configuration.ConfigurationManager.AppSettings["TCX_Folder_Path_Slab"].ToString();


        #endregion

        #region "Methods"

        #region "Slab"

        public List<SlabProduct> GenerateSlabStructureMarking(SlabStructure structureMarking, int projectTypeId, bool isSingleMesh, int structureElementId, int projectId, int productTypeId, out string message, out string meshSplittingStatus)
        {
            message = "";
            meshSplittingStatus = "";

            List<SlabProduct> products = null;
            SlabProduct newProduct = null;
            products = new List<SlabProduct>();
            try
            {
                ShapeCodeParameterSet objShapeCode = new ShapeCodeParameterSet();
                List<ShapeCodeParameterSet> listProjectParam = objShapeCode.ProjectParamLap_Get(structureMarking.ProductCode.ProductCodeId,structureMarking.ParamSetNumber);
                ShapeCodeParameterSet projectParamFiltered;
                if (structureElementId == 13)
                {
                    projectParamFiltered = (from item in listProjectParam
                                            where item.ParameterSetNumber == structureMarking.ParamSetNumber && item.productCode.ProductCodeId == structureMarking.ProductCode.ProductCodeId && item.StructureElementType == structureElementId
                                            select item).FirstOrDefault();
                }
                else
                {
                    projectParamFiltered = (from item in listProjectParam
                                            where item.ParameterSetNumber == structureMarking.ParamSetNumber && item.productCode.ProductCodeId == structureMarking.ProductCode.ProductCodeId && item.StructureElementType != 13  //Vanita Commented
                                            select item).FirstOrDefault();
                }
                if (projectTypeId == 0)
                {
                    throw new Exception("Project Type is not mapped to the Project.Map the same and continue.");
                }
                PreferredMesh preferredMesh = new PreferredMesh();
                //structureMarking.ProductCode.ProductCodeId = 3727
                //structureMarking.ProductCode.MainWireSpacing = 100;
                List<PreferredMesh> preferredMeshList = preferredMesh.PreferredMeshData_Get(structureMarking.ProductCode.ProductCodeId, projectTypeId, structureMarking.ProductCode.MainWireSpacing, structureMarking.MainWireLength);
                PreferredMesh preferredMeshFiltered = (from item in preferredMeshList
                                                       where item.ProjectTypeId == projectTypeId 
                                                       && item.MainWireSpacing == structureMarking.ProductCode.MainWireSpacing 
                                                       && (item.MWMinLength <= structureMarking.MainWireLength && item.MWMaxLength >= structureMarking.MainWireLength)
                                                       select item).FirstOrDefault();

                //Include New Logic to split the Slab----DEC-2012------------------------------------------------------------//
                /////////////////////////Include the validation block for the above two list to avoid Null exception.////////////////
                ///Pankaj
                if (projectParamFiltered == null) throw new Exception("MWLap and CWLap not found for the selected product and parameter set.");
                // Pankaj
                if (preferredMeshList == null || preferredMeshList.Count == 0) throw new Exception("Preferred length is not defined in the Preferred Mesh Master.");

                SlabSplitingHelper objSlabSplittingHelper = new SlabSplitingHelper();
                List<ValidMeshProduct> ListMeshProduct = new List<ValidMeshProduct>();
                if (structureMarking.ProductSplitUp == true)
                {
                //    structureMarking.ProductCode.MainWireSpacing, structureMarking.ProductCode.CrossWireSpacing,Convert.ToDouble(projectParamFiltered.CWLap)
                   ListMeshProduct = objSlabSplittingHelper.SplitMeshProducts(Convert.ToDouble(structureMarking.MainWireLength), Convert.ToDouble(structureMarking.CrossWireLength), structureMarking.ProductCode.MainWireSpacing, structureMarking.ProductCode.CrossWireSpacing, Convert.ToDouble(projectParamFiltered.CWLap), preferredMeshList[0].CWExcessLap, out meshSplittingStatus, out message);
                    if (message != "")
                    {
                        throw new Exception(message);
                    }
                    foreach (ValidMeshProduct MeshProduct in ListMeshProduct)
                    {
                        newProduct = new SlabProduct();
                        newProduct.MWLength = MeshProduct.MWLength;
                        newProduct.CWLength = MeshProduct.CWLength;

                        newProduct.MemberQty = MeshProduct.Quantity;
                        products.Add(newProduct);
                    }
                }
                else
                {
                    //Commented the below line in order split the product based on new logic.DEC-2012
                    //-----------------------------------------------------------------------------------------------------------//
                    isSingleMesh = true;
                    if (!isSingleMesh)
                    {
                        //Tacton processing
                        if (projectTypeId == 0)//Handled the exception in event of no record available for the Mesh data  Surendar. S June-5-2012
                        {
                            throw new Exception("Project Type is not mapped to the Project.Map the same and continue.");
                        }

                        if (projectParamFiltered == null)
                        {
                            throw new Exception("MWLap and CWLap not found for the selected product and parameter set.");
                        }

                        DataAccessor accessor = new FileAccessor();
                        NameValueCollection properties = new NameValueCollection();
                        properties.Add("Tacton.Configurator.remoteServers", "true");
                        properties.Add("Tacton.Configurator.servers", tactonServer);

                        Factory factory = new Factory(accessor, properties);


                        // Create and use a configuration object
                        configuration = new Configuration(factory);
                        string fileName = strSlabStructureTcxPath + "Structure_SlabWall_2012.tcx";
                        configuration.StartConfiguration(fileName, null);

                        //Slab Structure
                        //program_input_group
                        if (preferredMeshList == null)//Handled the exception in event of no record available for the Mesh data  Surendar. S June-5-2012
                        {
                            throw new Exception("Mesh data is not available for the entered combination.");
                        }
                        if (preferredMeshList.Count > 0)
                        {
                            TactonCommit("slabwall_part_mw_spacing_field", structureMarking.ProductCode.MainWireSpacing.ToString());//Product Code Master
                            TactonCommit("slabwall_part_min_mw_length_field", preferredMeshList[0].MWMinLength.ToString());//PreferredMeshData_Get
                            TactonCommit("slabwall_part_max_mw_length_field", preferredMeshList[0].MWMaxLength.ToString());//PreferredMeshData_Get
                            TactonCommit("slabwall_part_mw_length_step_field", preferredMeshList[0].MWStep.ToString());//PreferredMeshData_Get
                            TactonCommit("slabwall_part_mw_lapping_field", projectParamFiltered.MWLap.ToString()); //SWProjectParamLap_Get
                            TactonCommit("slabwall_part_excess_mw_lapping_field", preferredMeshList[0].MWExcessLap.ToString());//PreferredMeshData_Get
                            TactonCommit("slabwall_part_cw_spacing_field", structureMarking.ProductCode.CrossWireSpacing.ToString());//Product Code Master
                            TactonCommit("slabwall_part_min_cw_length_field", preferredMeshList[0].CWMinLength.ToString());//PreferredMeshData_Get
                            TactonCommit("slabwall_part_max_cw_length_field", preferredMeshList[0].CWMaxLength.ToString());//PreferredMeshData_Get
                            TactonCommit("slabwall_part_cw_length_step_field", preferredMeshList[0].CWStep.ToString());//PreferredMeshData_Get
                            TactonCommit("slabwall_part_cw_lapping_field", projectParamFiltered.CWLap.ToString()); //SWProjectParamLap_Get
                            TactonCommit("slabwall_part_excess_cw_lapping_field", preferredMeshList[0].CWExcessLap.ToString());//PreferredMeshData_Get
                        }
                        else
                        {
                            throw new Exception("Mesh data is not available for the entered combination.");
                        }
                        //user_input_group
                        TactonCommit("slabwall_part_single_mesh_field", "No");
                        TactonCommit("slabwall_part_total_mw_length_field", structureMarking.MainWireLength.ToString());//UserInput
                        TactonCommit("slabwall_part_total_cw_length_field", structureMarking.CrossWireLength.ToString());//UserInput

                        //result
                        int slabwall_part_cw_length_field = 0;
                        int slabwall_part_cw_length_qty_field = 0;
                        int slabwall_part_mw_length_field = 0;
                        int slabwall_part_mw_length_qty_field = 0;




                        //result 
                        Group userInputGroup = (Tacton.Configurator.ObjectModel.Group)configuration.Result.RootGroup.SubGroups["user_input_group"];
                        if (userInputGroup == null) throw new Exception("userInputGroup is NULL");

                        products = new List<SlabProduct>();

                        //Product Split up
                        string isCWLastLengthSpecial = (userInputGroup.Parameters["slabwall_part_cw_last_length_special_status_field"] as Parameter).Value.ToString().ToUpper();
                        string isMWLastLengthSpecial = (userInputGroup.Parameters["slabwall_part_mw_last_length_special_status_field"] as Parameter).Value.ToString().ToUpper();

                        //To maximize 2400 cw length mesh
                        if (isCWLastLengthSpecial == "YES" && Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_length_qty_field"] as Parameter).Value) > 0)
                        {
                            try
                            {
                                if (preferredMeshList.Count > 0)//Handled the exception in event of no record available for the Mesh data  Surendar. S June-5-2012
                                {
                                    TactonCommit("slabwall_part_cw_length_field", preferredMeshList[0].CWMaxLength.ToString());//PreferredMeshData_Get
                                }
                                else
                                {
                                    throw new Exception("Mesh data is not available for the entered combination.");
                                }
                            }
                            catch { } //Try to set max cw length field if possible 
                        }

                        userInputGroup = (Tacton.Configurator.ObjectModel.Group)configuration.Result.RootGroup.SubGroups["user_input_group"];
                        if (userInputGroup == null) throw new Exception("userInputGroup is NULL");
                        if (isCWLastLengthSpecial == "NO" && isMWLastLengthSpecial == "NO")
                        {
                            //Single Mesh Size
                            slabwall_part_cw_length_field = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_length_field"] as Parameter).Value);
                            slabwall_part_cw_length_qty_field = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_length_qty_field"] as Parameter).Value);
                            slabwall_part_mw_length_field = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_length_field"] as Parameter).Value);
                            slabwall_part_mw_length_qty_field = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_length_qty_field"] as Parameter).Value);

                            newProduct = new SlabProduct();
                            newProduct.CWLength = slabwall_part_cw_length_field;
                            newProduct.MWLength = slabwall_part_mw_length_field;
                            newProduct.MemberQty = slabwall_part_mw_length_qty_field * slabwall_part_cw_length_qty_field;
                            products.Add(newProduct);
                        }
                        else if (isCWLastLengthSpecial == "YES" && isMWLastLengthSpecial == "NO")
                        {
                            //Double Mesh Size variying CW Length
                            newProduct = new SlabProduct();
                            newProduct.CWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_last_length_special_field"] as Parameter).Value);
                            newProduct.MWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_length_field"] as Parameter).Value);
                            newProduct.MemberQty = 1;
                            products.Add(newProduct);

                            newProduct = new SlabProduct();
                            newProduct.CWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_length_field"] as Parameter).Value);
                            newProduct.MWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_length_field"] as Parameter).Value);
                            newProduct.MemberQty = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_length_qty_field"] as Parameter).Value);
                            products.Add(newProduct);

                        }
                        else if (isCWLastLengthSpecial == "NO" && isMWLastLengthSpecial == "YES")
                        {
                            //Double Mesh Size variying MW Length
                            newProduct = new SlabProduct();
                            newProduct.CWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_length_field"] as Parameter).Value);
                            newProduct.MWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_last_length_special_field"] as Parameter).Value);
                            newProduct.MemberQty = 1;
                            products.Add(newProduct);

                            newProduct = new SlabProduct();
                            newProduct.CWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_length_field"] as Parameter).Value);
                            newProduct.MWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_length_field"] as Parameter).Value);
                            newProduct.MemberQty = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_length_qty_field"] as Parameter).Value);
                            products.Add(newProduct);

                        }
                        else if (isCWLastLengthSpecial == "YES" && isMWLastLengthSpecial == "YES")
                        {
                            //4 Mesh Size variying MW Length & CW Length
                            newProduct = new SlabProduct();
                            newProduct.CWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_last_length_special_field"] as Parameter).Value);
                            newProduct.MWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_last_length_special_field"] as Parameter).Value);
                            newProduct.MemberQty = 1;
                            products.Add(newProduct);

                            newProduct = new SlabProduct();
                            newProduct.CWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_last_length_special_field"] as Parameter).Value);
                            newProduct.MWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_length_field"] as Parameter).Value);
                            newProduct.MemberQty = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_length_qty_field"] as Parameter).Value);
                            products.Add(newProduct);

                            newProduct = new SlabProduct();
                            newProduct.CWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_length_field"] as Parameter).Value);
                            newProduct.MWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_last_length_special_field"] as Parameter).Value);
                            newProduct.MemberQty = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_length_qty_field"] as Parameter).Value);
                            products.Add(newProduct);

                            newProduct = new SlabProduct();
                            newProduct.CWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_length_field"] as Parameter).Value);
                            newProduct.MWLength = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_length_field"] as Parameter).Value);
                            newProduct.MemberQty = Convert.ToInt32((userInputGroup.Parameters["slabwall_part_cw_length_qty_field"] as Parameter).Value) * Convert.ToInt32((userInputGroup.Parameters["slabwall_part_mw_length_qty_field"] as Parameter).Value);
                            products.Add(newProduct);
                        }

                    }
                    else//Single Mesh
                    {
                        products = new List<SlabProduct>();
                        newProduct = new SlabProduct();
                        newProduct.CWLength = structureMarking.CrossWireLength;
                        newProduct.MWLength = structureMarking.MainWireLength;
                        //newProduct.MemberQty = structureMarking.MemberQty;
                        newProduct.MemberQty = 1;   


                        products.Add(newProduct);
                    }
                }
                if (products == null) throw new Exception("products is NULL");
                foreach (SlabProduct product in products)
                {
                    //Set the properties for 'F' Shape without accessing tacton.
                    ShapeParameter shapeParam = new ShapeParameter { ShapeId = 9, ParameterName = "A", ParameterValue = 0, CriticalIndiacator = "N", WireType = "M", AngleType = "S", IsVisible = true, SequenceNumber = 1, ShapeCodeImage = "F", EditFlag = true, VisibleFlag = true };
                    ShapeParameterCollection shapeParamCollection = new ShapeParameterCollection();
                    shapeParamCollection.Add(shapeParam);
                    product.shapecode = new ShapeCode { ShapeID = 9, ShapeCodeName = "F", MeshShapeGroup = "flat", CreepDeductAtCO1 = false, MOCO = "", CreepDeductAtMO1 = false, BendIndicator = false, IsCapping = false, ShapeParam = shapeParamCollection };
                    product.ProductCodeId = structureMarking.ProductCode.ProductCodeId;
                    //product.ProductCodeId = 3727;
                    product.ProductionMWLength = product.MWLength;
                    product.ProductionCWLength = product.CWLength;
                    product.MWSpacing = structureMarking.ProductCode.MainWireSpacing;
                    product.CWSpacing = structureMarking.ProductCode.CrossWireSpacing;                
                    product.PinSize = structureMarking.PinSize;
                    List<SlabProduct> ListOverHang = new List<SlabProduct>();
                    SlabProduct objSlabProduct = new SlabProduct();
                    objSlabProduct.ProductionMWLength = product.MWLength;
                    objSlabProduct.ProductionCWLength = product.CWLength;
                    objSlabProduct.MWSpacing = product.MWSpacing;
                    objSlabProduct.CWSpacing = product.CWSpacing;

                    try
                    {
                        ListOverHang = objSlabProduct.GetOverHang(structureMarking.ParamSetNumber, projectId, structureElementId, productTypeId, objSlabProduct);
                    }
                    catch
                    {
                        throw new Exception("Problem in getting the Overhang details. Please check the Parameterset.MWSpacing=" + product.MWSpacing + " CWSpacing=" + product.CWSpacing + "shapecode=" + product.shapecode + " ProductCodeId=" + product.ProductCodeId + " ProductionMWLength=" + product.ProductionMWLength + "ProductionCWLength=" + product.ProductionCWLength);
                    }
                    if (ListOverHang.Count > 0)
                    {
                        product.MO1 = ListOverHang[0].MO1;
                        product.MO2 = ListOverHang[0].MO2;
                        product.CO1 = ListOverHang[0].CO1;
                        product.CO2 = ListOverHang[0].CO2;
                    }

                    //Code to Adjust MO2 based on MO1.                    
                    product.NoOfCrossWire = (product.MWLength - product.MO1 - product.MO2) / product.CWSpacing + 1;
                    if (product.NoOfCrossWire < 1)
                    {
                        product.MO1 = (product.MWLength - product.CWSpacing) / 2;
                        product.MO2 = product.MO1;
                        product.NoOfCrossWire = (product.MWLength - product.MO1 - product.MO2) / product.CWSpacing + 1;
                    }
                    product.MO2 = product.MWLength - (product.NoOfCrossWire - 1) * product.CWSpacing - product.MO1;
                    product.NoOfMainWire = (product.CWLength - product.CO1 - product.CO2) / product.MWSpacing + 1;
                    product.CO2 = product.CWLength - (product.NoOfMainWire - 1) * product.MWSpacing - product.CO1;


                    product.TheoraticalWeight = (product.MWLength / 1000) * (product.CWLength / 1000) * structureMarking.ProductCode.WeightArea;
                    product.TheoraticalWeight = (product.MWLength / 1000) * (product.CWLength / 1000) * structureMarking.ProductCode.WeightArea;
                    product.InvoiceArea = product.MWLength * product.CWLength;

                    product.NetWeight = ((product.CWLength / 1000) * product.NoOfCrossWire * structureMarking.ProductCode.CwWeightPerMeterRun) + ((product.MWLength / 1000) * product.NoOfMainWire * structureMarking.ProductCode.WeightPerMeterRun);
                    product.ProductionWeight = product.TheoraticalWeight;
                    product.EnvelopeLength = product.MWLength > product.CWLength ? product.MWLength : product.CWLength;
                    product.EnvelopeWidth = product.EnvelopeLength == product.MWLength ? product.CWLength : product.MWLength;
                    product.EnvelopeHeight = structureMarking.ProductCode.CwDia + structureMarking.ProductCode.MainWireDia;
                    product.ProduceIndicator = structureMarking.ProduceIndicator;
                    product.BOMIndicator = "S";
                    product.ParamValues = String.Format("A:{0}", product.MWLength);
                    product.InvoiceMWWeight = (structureMarking.ProductCode.WeightPerMeterRun * product.NoOfMainWire * product.MWLength) / 1000;
                    product.InvoiceCWWeight = (structureMarking.ProductCode.CwWeightPerMeterRun * product.NoOfCrossWire * product.CWLength) / 1000;
                    product.ProductionMWWeight = (structureMarking.ProductCode.WeightPerMeterRun * product.NoOfMainWire * product.ProductionMWLength) / 1000;
                    product.ProductionCWWeight = (structureMarking.ProductCode.CwWeightPerMeterRun * product.NoOfCrossWire * product.ProductionCWLength) / 1000;

                    product.ProductionMO1 = product.MO1;
                    product.ProductionMO2 = product.MO2;
                    product.ProductionCO1 = product.CO1;
                    product.ProductionCO2 = product.CO2;
                    product.ProductionMWQty = product.NoOfMainWire;
                    product.ProductionCWQty = product.NoOfCrossWire;
                    product.MWBVBSString = "BF2D@Gl" + product.MWLength + "@w0@";
                    product.CWBVBSString = "BF2D@Gl" + product.CWLength + "@w0@";
                    product.BendingCheckInd = true; /* By default bending check is true */
                }


            }
            catch (Exception ex)
            {
                message = "Error Generating Products; Invalid Inputs: " + ex.Message + "Trace: " + ex.StackTrace;
            }

            return products;
        }

        public SlabProduct GenerateSlabProductMarking(SlabStructure structureMark, SlabProduct productMark, out bool machineCheck, out string bendingCheck, out bool transportCheck, out string message)
        {
            string errormessage = "";
            message = "";
            bendingCheck = "";
            transportCheck = false;
            machineCheck = false;
            SlabProduct newProductMark = new SlabProduct();
            DataAccessor accessor = new FileAccessor();
            NameValueCollection properties = new NameValueCollection();
            ArrayList sParamList = new ArrayList();

            //Factory factory = new Factory(accessor, properties);
            string meshGroup = "1sw2";


            try
            {
                meshGroup = productMark.shapecode.MeshShapeGroup.ToLower().Trim();
                newProductMark = productMark;
                meshGroup = meshGroup.Trim().ToLower();



                SlabDetailingComponent objInfo = new SlabDetailingComponent();
                Dictionary<string, string> inputs = new Dictionary<string, string>();
                inputs.Add("mw_length", newProductMark.MWLength.ToString());
                inputs.Add("cw_length", newProductMark.CWLength.ToString());
                inputs.Add("mw_spacing", newProductMark.MWSpacing.ToString());
                inputs.Add("cw_spacing", newProductMark.CWSpacing.ToString());
                inputs.Add("mw_dia", structureMark.ProductCode.MainWireDia.ToString());
                inputs.Add("cw_dia", structureMark.ProductCode.CwDia.ToString());
           //     inputs.Add("mw_dia", "2");
             //   inputs.Add("cw_dia", "4");
                inputs.Add("mo1", newProductMark.MO1.ToString());
                inputs.Add("mo2", newProductMark.MO2.ToString());
                inputs.Add("co1", newProductMark.CO1.ToString());
                inputs.Add("co2", newProductMark.CO2.ToString());
                inputs.Add("weight_per_area", structureMark.ProductCode.WeightArea.ToString());
                inputs.Add("mw_weight_m_run", structureMark.ProductCode.WeightPerMeterRun.ToString());
                inputs.Add("cw_weight_m_run", structureMark.ProductCode.CwWeightPerMeterRun.ToString());



                //inputs.Add("weight_per_area", "2");
                //inputs.Add("mw_weight_m_run", "3");
                //inputs.Add("cw_weight_m_run", "4");
                inputs.Add("pin_dia", newProductMark.PinSize.ToString());



                //inputs.Add("no_of_mw", Convert.ToString(newProductMark.NoOfMainWire));      //to check how to calculate
                //inputs.Add("no_of_cw", Convert.ToString(newProductMark.NoOfCrossWire));



                //ShapeParameter Loop
                List<DataRow> parameterList = new List<DataRow>();
                int shapeId = productMark.shapecode.ShapeID;
                //int shapeId = 9;



                //foreach (ShapeParameter shapeParam in newProductMark.shapecode.ShapeParam)
                //{
                //    if (shapeParam.EditFlag)
                //    {
                //        inputs.Add(shapeParam.ParameterName.ToLower().Trim(), shapeParam.ParameterValue.ToString());
                //        //TactonCommit(meshGroup + "_" + shapeParam.ParameterName.ToLower().Trim() + "_field", shapeParam.ParameterValue.ToString());
                //        //}
                //        //else
                //        //{
                //        //    parameterList = objInfo.GetShapeParams(shapeId);
                //        //    foreach (var item in parameterList)
                //        //    {
                //        //       inputs.Add(item.ItemArray[0].ToString().Trim(), item.ItemArray[1].ToString());
                //        //    }
                //    }
                //}
                foreach (var shapeParam in newProductMark.shapecode.ShapeParam)
                {
                    var options = new JsonSerializerOptions()
                    {
                        NumberHandling = JsonNumberHandling.AllowReadingFromString |
                                 JsonNumberHandling.WriteAsString
                    };
                    string denemeJson = JsonSerializer.Serialize<JsonElement>((JsonElement)shapeParam, options);



                    // deserialize

                    ShapeParameter shapeParamObj = JsonSerializer.Deserialize<ShapeParameter>(denemeJson, options);
                    if (shapeParamObj.EditFlag)
                    {
                        inputs.Add(shapeParamObj.ParameterName.ToLower().Trim(), shapeParamObj.ParameterValue.ToString());
                        //TactonCommit(meshGroup + "_" + shapeParam.ParameterName.ToLower().Trim() + "_field", shapeParam.ParameterValue.ToString());
                        //}
                        //else
                        //{
                        //    parameterList = objInfo.GetShapeParams(shapeId);
                        //    foreach (var item in parameterList)
                        //    {
                        //       inputs.Add(item.ItemArray[0].ToString().Trim(), item.ItemArray[1].ToString());
                        //    }
                    }
                }



                objInfo.FillInputDictionary(inputs);



                //result 
                string validations = string.Empty;
                string error = string.Empty;
                  Dictionary<string, string> result = objInfo.ExecuteDetailingComponent(shapeId, string.Empty);
                //Dictionary<string, string> result = objInfo.ExecuteDetailingComponent(9, string.Empty);




                if (result != null)
                {
                    validations = result["ValidationFailed"];
                    result.Remove("ValidationFailed");
                    if (validations == "False")
                    {



                        //newProductMark.InvoiceArea = Int32.Parse(result["area_post"]); //original code
                        newProductMark.NoOfMainWire = Convert.ToInt32(Double.Parse(result["no_of_mw"]));
                        newProductMark.NoOfCrossWire = Convert.ToInt32(Double.Parse(result["no_of_cw"]));
                        newProductMark.InvoiceArea = Convert.ToInt32(Math.Round(Double.Parse(result["area_post"])));
                        newProductMark.EnvelopeLength = Double.Parse(result["envelope_length"]);
                        newProductMark.EnvelopeWidth = Double.Parse(result["envelope_width"]);
                        newProductMark.EnvelopeHeight = Double.Parse(result["envelope_height"]);
                        newProductMark.TheoraticalWeight = Double.Parse(result["theoretical_tonnage"]);
                        newProductMark.NetWeight = Double.Parse(result["net_tonnage"]);
                        newProductMark.ProductionMWLength = Convert.ToInt32(Math.Round(Double.Parse(result["production_mw_length"])));
                        newProductMark.ProductionCWLength = Convert.ToInt32(Math.Round(Double.Parse(result["production_cw_length"])));



                        newProductMark.ProductionWeight = Double.Parse(result["actual_tonnage_post"]);
                        newProductMark.ProductionMO1 = Convert.ToInt32(result["production_mo1"]);
                        newProductMark.ProductionMO2 = Convert.ToInt32(Math.Round(Double.Parse(result["production_mo2"])));
                        newProductMark.ProductionCO1 = Convert.ToInt32(result["production_co1"]);
                        newProductMark.ProductionCO2 = Convert.ToInt32(Math.Round(Double.Parse(result["production_co2"])));




                        //Added by Sagarika for CHG0031300
                        newProductMark.ParamValues = string.Empty;




                        //newProductMark.shapecode.ShapeCodeName = "F";



                  //      commented by pankaj start
                        if (newProductMark.shapecode.ShapeCodeName == "3M8" || newProductMark.shapecode.ShapeCodeName == "3MR8")
                        {
                            //foreach (ShapeParameter shapeParam in newProductMark.shapecode.ShapeParam)
                            //{



                            //    newProductMark.ParamValues = newProductMark.ParamValues + ((shapeParam.ParameterName).ToUpper() + ":" + shapeParam.ParameterValue + ";");
                            //}



                            foreach (var shapeParam in newProductMark.shapecode.ShapeParam)
                            {
                                var options = new JsonSerializerOptions()
                                {
                                    NumberHandling = JsonNumberHandling.AllowReadingFromString |
                                             JsonNumberHandling.WriteAsString
                                };
                                string denemeJson = JsonSerializer.Serialize<JsonElement>((JsonElement)shapeParam, options);



                                // deserialize

                                ShapeParameter shapeParamObj = JsonSerializer.Deserialize<ShapeParameter>(denemeJson, options);
                                if (shapeParamObj.EditFlag)
                                {
                                    newProductMark.ParamValues = newProductMark.ParamValues + ((shapeParamObj.ParameterName).ToUpper() + ":" + shapeParamObj.ParameterValue + ";");
                                    //TactonCommit(meshGroup + "_" + shapeParam.ParameterName.ToLower().Trim() + "_field", shapeParam.ParameterValue.ToString());
                                    //}
                                    //else
                                    //{
                                    //    parameterList = objInfo.GetShapeParams(shapeId);
                                    //    foreach (var item in parameterList)
                                    //    {
                                    //       inputs.Add(item.ItemArray[0].ToString().Trim(), item.ItemArray[1].ToString());
                                    //    }
                                }
                            }

                        }



                        else
                        {
                            newProductMark.ParamValues = Convert.ToString((result["string_post"]));
                            //newProductMark.ParamValues = Convert.ToString("AAAA");
                        }
                        if (newProductMark.shapecode.ShapeCodeName == "3M8" || newProductMark.shapecode.ShapeCodeName == "3MR8")
                        {
                            newProductMark.ParamValues = newProductMark.ParamValues.Substring(0, newProductMark.ParamValues.LastIndexOf(';'));
                        }
                       // End CHG0031300



                        //commented by pankaj end



                        //newProductMark.ParamValues = Convert.ToString((resultGroup.Parameters[meshGroup + "_string_post"] as Parameter).Value);
                        newProductMark.MWBVBSString = Convert.ToString(result["mw_bvbs_string"]);
                        newProductMark.CWBVBSString = Convert.ToString(result["cw_bvbs_string"]);
                        newProductMark.InvoiceMWWeight = (structureMark.ProductCode.WeightPerMeterRun * newProductMark.NoOfMainWire * newProductMark.MWLength) / 1000;
                        newProductMark.InvoiceCWWeight = (structureMark.ProductCode.CwWeightPerMeterRun * newProductMark.NoOfCrossWire * newProductMark.CWLength) / 1000;
                        newProductMark.ProductionMWWeight = (structureMark.ProductCode.WeightPerMeterRun * newProductMark.NoOfMainWire * newProductMark.ProductionMWLength) / 1000;
                        newProductMark.ProductionCWWeight = (structureMark.ProductCode.CwWeightPerMeterRun * newProductMark.NoOfCrossWire * newProductMark.ProductionCWLength) / 1000;



                        newProductMark.ProductionMWQty = newProductMark.NoOfMainWire;
                        newProductMark.ProductionCWQty = newProductMark.NoOfCrossWire;



                        //Rounding production length
                        if (newProductMark.shapecode.CreepDeductAtMO1)
                        {
                            newProductMark.ProductionMO2 = newProductMark.MO2;
                            newProductMark.ProductionMWLength = RoundUpTo10Setp(newProductMark.ProductionMWLength);
                            newProductMark.ProductionMO1 = newProductMark.ProductionMWLength - (newProductMark.NoOfCrossWire - 1) * newProductMark.CWSpacing - newProductMark.ProductionMO2;
                        }
                        else
                        {
                            newProductMark.ProductionMO1 = newProductMark.MO1;
                            newProductMark.ProductionMWLength = RoundUpTo10Setp(newProductMark.ProductionMWLength);
                            newProductMark.ProductionMO2 = newProductMark.ProductionMWLength - (newProductMark.NoOfCrossWire - 1) * newProductMark.CWSpacing - newProductMark.ProductionMO1;
                        }



                        if (newProductMark.shapecode.CreepDeductAtCO1)
                        {
                            newProductMark.ProductionCO2 = newProductMark.CO2;
                            newProductMark.ProductionCWLength = RoundUpTo10Setp(newProductMark.ProductionCWLength);
                            newProductMark.ProductionCO1 = newProductMark.ProductionCWLength - (newProductMark.NoOfMainWire - 1) * newProductMark.MWSpacing - newProductMark.ProductionCO2;
                        }
                        else
                        {
                            newProductMark.ProductionCO1 = newProductMark.CO1;
                            newProductMark.ProductionCWLength = RoundUpTo10Setp(newProductMark.ProductionCWLength);
                            newProductMark.ProductionCO2 = newProductMark.ProductionCWLength - (newProductMark.NoOfMainWire - 1) * newProductMark.MWSpacing - newProductMark.ProductionCO1;
                        }



                        ///*
                        // * Commented By :   Premkumar
                        // * Reason       :   Machine check value from the tacton can be skip. Bending adjustment calculation is doning seperately.
                        // * Date         :   11 - Oct - 2013
                        // * */
                        if (newProductMark.ProductionMO1 < structureMark.ParameterSet.MinMo1 || newProductMark.ProductionMO2 < structureMark.ParameterSet.MinMo2 || newProductMark.ProductionCO1 < structureMark.ParameterSet.MinCo1 || newProductMark.ProductionCO2 < structureMark.ParameterSet.MinCo2)
                        {
                            machineCheck = false;
                            //throw new Exception("MachineCheck is false,can not able to insert");
                            return newProductMark;
                        }
                        else
                        {
                            machineCheck = true;
                        }
                        if (newProductMark.EnvelopeHeight > structureMark.ParameterSet.TransportMaxHeight || newProductMark.EnvelopeLength > structureMark.ParameterSet.TransportMaxLength || newProductMark.EnvelopeWidth > structureMark.ParameterSet.TransportMaxWidth)
                        {
                            transportCheck = false;
                            //throw new Exception("TransportCheck is false,can not able to insert");
                            return newProductMark;
                        }
                        else
                        {
                            transportCheck = true;
                        }



                        newProductMark.MWPitch = "";
                        newProductMark.CWPitch = "";
                        newProductMark.MWFlag = 0;
                        newProductMark.CWFlag = 0;



                        /*
                        machineCheck = true;
                        transportCheck = true;

 

                

 

                
                         * Commented By :   Premkumar
                         * Reason       :   Bending check validation is not required. Since we will do the bending adjustment for the failed cases.
                         * Date         :   11 - Jun - 2013
                         * 


                        ShapeParameterCollection segmentList = (ShapeParameterCollection)newProductMark.shapecode.ShapeParam;

 

                        if (productMark.shapecode.MOCO.Trim() == "M")
                        {
                            List<ShapeParameter> shapeParamColl = (from ShapeParameter sp in segmentList
                                                                   where sp.AngleType == "S" && sp.WireType == "M"
                                                                   select sp).ToList();

 

                            foreach (ShapeParameter spItem in shapeParamColl)
                            {
                                sParamList.Add(spItem.ParameterName + ";" + spItem.ParameterValue + ";" + "N");
                            }
                            string[] paramList = sParamList.ToArray(typeof(string)) as string[];
                            CheckBendingFeasibility(newProductMark.NoOfCrossWire, newProductMark.MO1, newProductMark.MO2, newProductMark.CWSpacing, paramList, out bendingCheck);
                            if (bendingCheck != "")
                            {
                                bendingCheck = "Bending check failed with main wire. Please click OK to continue saving the values or click No to change the values.";
                            }
                        }
                        else if (productMark.shapecode.MOCO.Trim() == "C")
                        {
                            List<ShapeParameter> shapeParamColl = (from ShapeParameter sp in segmentList
                                                                   where sp.AngleType == "S" && sp.WireType == "C"
                                                                   select sp).ToList();

 

                            foreach (ShapeParameter spItem in shapeParamColl)
                            {
                                sParamList.Add(spItem.ParameterName + ";" + spItem.ParameterValue + ";" + "N");
                            }
                            string[] paramList = sParamList.ToArray(typeof(string)) as string[];
                            CheckBendingFeasibility(newProductMark.NoOfMainWire, newProductMark.CO1, newProductMark.CO2, newProductMark.MWSpacing, paramList, out bendingCheck);
                            if (bendingCheck != "")
                            {
                                bendingCheck = "Bending check failed with cross wire. Please click OK to continue saving the values or click No to change the values.";
                            }
                        }
                        else if (productMark.shapecode.MOCO.Trim() == "B")
                        {
                            List<ShapeParameter> shapeParamColl = (from ShapeParameter sp in segmentList
                                                                   where sp.AngleType == "S"
                                                                   select sp).ToList();

 

                            foreach (ShapeParameter spItem in shapeParamColl)
                            {
                                sParamList.Add(spItem.ParameterName + ";" + spItem.ParameterValue + ";" + "N");
                            }
                            string[] paramList = sParamList.ToArray(typeof(string)) as string[];
                            CheckBendingFeasibility(newProductMark.NoOfCrossWire, newProductMark.MO1, newProductMark.MO2, newProductMark.CWSpacing, paramList, out bendingCheck);

 

                            if (bendingCheck == "")
                            {
                                CheckBendingFeasibility(newProductMark.NoOfMainWire, newProductMark.CO1, newProductMark.CO2, newProductMark.MWSpacing, paramList, out bendingCheck);
                                if (bendingCheck != "")
                                {
                                    bendingCheck = "Bending check failed with cross wire. Please click OK to continue saving the values or click No to change the values.";
                                }
                            }
                            else
                            {
                                bendingCheck = "Bending check failed with main wire. Please click OK to continue saving the values or click No to change the values.";
                            }
                        }
                         */
                    }
                    else
                    {
                        foreach (var item in result)
                        {
                            if (item.Key != "ValidationFailed")
                            {
                                message = item.Value;
                                errormessage = item.Value;                               
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Product marking result is NULL");
                }
            }
            catch (Exception ex)
            {
                if (errormessage != "")
                {
                    message = errormessage;
                }
                else
                {
                    message = "Error Generating Products; Invalid Inputs: " + ex.Message + "Trace: " + ex.StackTrace;
                }
            }
            return newProductMark;
        }
        #endregion

        #region "Column"
        public List<ColumnProduct> GenerateColumnStructureMarking(ColumnStructure structureMarking, out string message, int topCover, int bottomCover, int leftCover, int rightCover, int leg)
        {
            message = "";
            string errormessage = "";
            List<ColumnProduct> products = null;
            int NoOfstirrups;
            try
            {
                SlabDetailingComponent objInfo = new SlabDetailingComponent();
                Dictionary<string, string> inputsStructureMarking = new Dictionary<string, string>();
                //From ProductMaster
                inputsStructureMarking.Add("column_part_min_links_factor_field", structureMarking.ProductCode.MinLinkFactor.ToString());
                inputsStructureMarking.Add("column_part_max_links_factor_field", structureMarking.ProductCode.MaxLinkFactor.ToString());
                inputsStructureMarking.Add("column_part_mw_spacing_field", structureMarking.ProductCode.MainWireSpacing.ToString());

                //Cover from ParameterSet
                inputsStructureMarking.Add("column_part_top_cover_field", topCover.ToString());
                inputsStructureMarking.Add("column_part_bottom_cover_field", bottomCover.ToString());
                inputsStructureMarking.Add("column_part_left_cover_field", leftCover.ToString());
                inputsStructureMarking.Add("column_part_right_cover_field", rightCover.ToString());

                //Values from UI
                inputsStructureMarking.Add("column_part_column_width_field", structureMarking.ColumnWidth.ToString());
                inputsStructureMarking.Add("column_part_column_length_field", structureMarking.ColumnLength.ToString());
                inputsStructureMarking.Add("column_part_no_of_stirrups_field", structureMarking.TotalNoOfLinks.ToString());
                products = new List<ColumnProduct>();
                objInfo.FillInputDictionary(inputsStructureMarking);

                Dictionary<string, string> resultStructMarking = objInfo.ExecuteDetailingComponent(0, "Column");
                if (resultStructMarking.Count == 0 && resultStructMarking == null)
                {
                    throw new Exception("resultStructMarking is NULL");
                }
                string validations = string.Empty;
                string error = string.Empty;
                if (resultStructMarking != null)
                {
                    validations = resultStructMarking["ValidationFailed"];
                    resultStructMarking.Remove("ValidationFailed");
                    if (validations == "False")
                    {
                        structureMarking.ColumnHeight = Convert.ToInt32(Double.Parse(resultStructMarking["column_part_column_height"]));
                        ColumnProduct newProduct = new ColumnProduct();
                        newProduct.LinkWidth = structureMarking.ColumnWidth - leftCover - rightCover;
                        newProduct.LinkLength = structureMarking.ColumnLength - topCover - bottomCover;
                        newProduct.ShapeCode = structureMarking.Shape.ShapeCodeName;
                        newProduct.ShapeParam = structureMarking.Shape.ShapeParam;
                        newProduct.ProductCodeId = structureMarking.ProductCode.ProductCodeId;
                        newProduct.ShapeCodeId = structureMarking.Shape.ShapeID;

                        int no_of_links_1 = 0, no_of_links_2 = 0, no_of_columncage_1 = 0, no_of_columncage_2 = 0;
                        int maxLinks = structureMarking.ProductCode.MaxLinkFactor;
                        int minLinks = structureMarking.ProductCode.MinLinkFactor;
                        NoOfstirrups = structureMarking.TotalNoOfLinks;
                        int div;
                        if (NoOfstirrups >= minLinks)
                        {
                            if (NoOfstirrups <= maxLinks)
                            {
                                no_of_columncage_1 = 1;
                                no_of_columncage_2 = 0;
                                no_of_links_1 = NoOfstirrups;
                                no_of_links_2 = 0;
                            }
                            else if (NoOfstirrups > maxLinks)
                            {
                                int factor = 2;
                                int m = NoOfstirrups - (maxLinks * 2);
                                if (m > maxLinks)
                                {
                                    factor = 3;
                                }
                                else
                                {
                                    factor = 2;
                                }

                                int modResult = NoOfstirrups % factor;
                                if (modResult == 0)
                                {
                                    if (NoOfstirrups / factor <= maxLinks)
                                    {
                                        no_of_links_1 = NoOfstirrups / factor;
                                        no_of_columncage_1 = factor;
                                        no_of_columncage_2 = 0;
                                        no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_columncage_1);
                                    }
                                    else
                                    {
                                        div = NoOfstirrups / maxLinks;
                                        for (int i = minLinks; i <= maxLinks; i++)
                                        {
                                            int a = div * i;
                                            int rem = NoOfstirrups - a;
                                            if (rem <= maxLinks)
                                            {
                                                no_of_links_1 = i;
                                                no_of_columncage_2 = 1;
                                                no_of_columncage_1 = div;
                                                no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_columncage_1);
                                                break;
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    div = NoOfstirrups / maxLinks;
                                    int b = NoOfstirrups - maxLinks;
                                    if (b <= maxLinks && b >= minLinks)
                                    {
                                        no_of_links_1 = maxLinks;
                                        no_of_columncage_1 = div;
                                        no_of_columncage_2 = 1;
                                        no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_columncage_1);
                                    }
                                    else
                                    {
                                        for (int i = minLinks; i <= maxLinks; i++)
                                        {
                                            int a = div * i;
                                            int rem = NoOfstirrups - a;
                                            if (rem <= maxLinks)
                                            {
                                                no_of_links_1 = i;
                                                no_of_columncage_2 = 1;
                                                no_of_columncage_1 = div;
                                                no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_columncage_1);
                                                break;
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Total no. of links should be greater than min no. of links");
                        }

                        //newProduct.InvoiceCWLength = Convert.ToInt32(Double.Parse(resultStructMarking["column_part_cw_length_1_field"]));
                        //newProduct.NoofLinks = Convert.ToInt32(Double.Parse(resultStructMarking["column_part_no_of_links_1_field"]));
                        //newProduct.Quantity = Convert.ToInt32(Double.Parse(resultStructMarking["column_part_no_of_columncage_1_field"]));

                        //modified by aishwarya
                        //string cw_lenghth_1_field = "column_part_no_of_links_1_field*column_part_mw_spacing_field";
                        newProduct.InvoiceCWLength = no_of_links_1 * structureMarking.ProductCode.MainWireSpacing;
                        newProduct.NoofLinks = Convert.ToInt32(no_of_links_1);
                        newProduct.Quantity = Convert.ToInt32(no_of_columncage_1);


                        newProduct.BOMIndicator = "S";

                        products.Add(newProduct);

                        newProduct = new ColumnProduct();
                        newProduct.LinkWidth = structureMarking.ColumnWidth - leftCover - rightCover;
                        newProduct.LinkLength = structureMarking.ColumnLength - topCover - bottomCover;                       
                        newProduct.ShapeCode = structureMarking.Shape.ShapeCodeName; 
                        newProduct.ShapeParam = structureMarking.Shape.ShapeParam;
                        newProduct.ProductCodeId = structureMarking.ProductCode.ProductCodeId;
                        newProduct.ShapeCodeId = structureMarking.Shape.ShapeID;


                        //newProduct.InvoiceCWLength = Convert.ToInt32(Double.Parse(resultStructMarking["column_part_cw_length_2_field"]));
                        //newProduct.NoofLinks = Convert.ToInt32(Double.Parse(resultStructMarking["column_part_no_of_links_2_field"]));
                        //newProduct.Quantity = Convert.ToInt32(Double.Parse(resultStructMarking["column_part_no_of_columncage_2_field_1"]));

                        string cw_length_2_field = "no_of_links_2*mw_spacing";
                        newProduct.InvoiceCWLength = no_of_links_2 * structureMarking.ProductCode.MainWireSpacing;
                        newProduct.NoofLinks = no_of_links_2;
                        newProduct.Quantity = no_of_columncage_2;
                        newProduct.BOMIndicator = "S";
                        //Add if more than 2 products
                        if (newProduct.NoofLinks != 0) products.Add(newProduct);
                    }
                }

                if (products == null) throw new Exception("products is NULL");

                foreach (ColumnProduct product in products)
                {
                    //configuration.StartConfiguration(strColumnStructureTcxPath + "Product_Column.tcx", null);


                    //Product Marking Generation
                    objInfo.ClearDictionary();
                    Dictionary<string, string> inputsProductMarking = new Dictionary<string, string>();
                    //From ProductMaster
                    inputsProductMarking.Add("column_width", structureMarking.ColumnWidth.ToString());
                    inputsProductMarking.Add("column_length", structureMarking.ColumnLength.ToString());
                    inputsProductMarking.Add("top_cover", topCover.ToString());
                    inputsProductMarking.Add("bottom_cover", bottomCover.ToString());
                    inputsProductMarking.Add("left_cover", leftCover.ToString());
                    inputsProductMarking.Add("right_cover", rightCover.ToString());
                    inputsProductMarking.Add("no_of_link", product.NoofLinks.ToString());
                    int legValue = 0;                   

                    //foreach (ShapeParameter sp in product.ShapeParam)
                    //{
                    //    if (sp.ParameterName.ToUpper().Trim() == "LEG" && sp.VisibleFlag == true)
                    //    {
                    //        if (sp.ParameterValue == 0)
                    //        {
                    //            sp.ParameterValue = leg;
                    //        }
                    //        inputsProductMarking.Add("leg", sp.ParameterValue.ToString());
                    //        legValue = sp.ParameterValue;
                    //    }
                    //}

                    foreach (var sp in product.ShapeParam)
                    {
                        if (sp != null)
                        {
                            var options = new JsonSerializerOptions()
                            {
                                NumberHandling = JsonNumberHandling.AllowReadingFromString |
                                     JsonNumberHandling.WriteAsString
                            };

                            // serialize
                            string denemeJson = JsonSerializer.Serialize<JsonElement>((JsonElement)sp, options);

                            // deserialize
                           
                            ShapeParameter shapeparamObj = JsonSerializer.Deserialize<ShapeParameter>(denemeJson, options);

                            if (shapeparamObj.ParameterName.ToUpper().Trim() == "LEG" && shapeparamObj.VisibleFlag == true)
                            {
                                if (shapeparamObj.ParameterValue == 0)
                                {
                                    shapeparamObj.ParameterValue = leg;
                                }                              
                                inputsProductMarking.Add("leg", shapeparamObj.ParameterValue.ToString());
                                legValue = shapeparamObj.ParameterValue;
                            }

                        }


                    }

                    int noOfCW = 2;

                    inputsProductMarking.Add("cw_length", product.InvoiceCWLength.ToString());
                    inputsProductMarking.Add("mw_spacing", structureMarking.ProductCode.MainWireSpacing.ToString());
                    inputsProductMarking.Add("mw_dia", structureMarking.ProductCode.MainWireDia.ToString());
                    inputsProductMarking.Add("cw_dia", structureMarking.ProductCode.CwDia.ToString());
                    inputsProductMarking.Add("weight_per_area", structureMarking.ProductCode.WeightArea.ToString());
                    inputsProductMarking.Add("mw_weight_per_m_run", structureMarking.ProductCode.WeightPerMeterRun.ToString());
                    inputsProductMarking.Add("cw_weight_per_m_run", structureMarking.ProductCode.CwWeightPerMeterRun.ToString());
                    inputsProductMarking.Add("pin", "32");
                    //To chnange 
                    //inputsProductMarking.Add("no_of_cw", noOfCW.ToString());

                    objInfo.FillInputDictionary(inputsProductMarking);
                    //Result
                    int shapeId = product.ShapeCodeId;
                    Dictionary<string, string> resultProductMarking = new Dictionary<string, string>();

                    //Hardcoding to fix
                    string link_width = "column_width-top_cover-bottom_cover";
                    double link_width_value = structureMarking.ColumnWidth - topCover - bottomCover;
                    string link_length = "column_length-left_cover-right_cover";
                    double link_length_value = structureMarking.ColumnLength - leftCover - rightCover;
                    string mw_length = "(2*(link_length+link_width))+(2*leg)";
                    double mw_length_value = 0;
                    if (product.ShapeCode == "CO" || product.ShapeCode == "COI" || product.ShapeCode == "COIH" || product.ShapeCode == "COH") // SNEHAT_TTL 02092022
                    {
                        //(2*(link_length+link_width))+(2*leg)
                        mw_length_value = (2 * (link_length_value + link_width_value)) + (2 * legValue);

                    }
                    else if (product.ShapeCode == "2M1C" || product.ShapeCode == "2MR1C")
                    {
                        // link_width+link_length+link_width
                        mw_length_value = link_width_value + link_length_value + link_width_value;
                    }

                    else if (product.ShapeCode == "4M1C" || product.ShapeCode == "4MR1C")
                    {
                        //(link_length+2*(link_width))+(2*leg)
                        mw_length_value = (link_length_value + 2 * (link_width_value)) + (2 * legValue);
                    }

                    product.InvoiceMWLength = Convert.ToInt32(mw_length_value);

                    //Fix me:
                    if (product.InvoiceMWLength < 701)
                    {
                        noOfCW = 2;
                    }
                    else if (product.InvoiceMWLength > 700 && product.InvoiceMWLength <= 1200)
                    {
                        noOfCW = 2;
                    }
                    else if (product.InvoiceMWLength > 1200 && product.InvoiceMWLength <= 3000)
                    {
                        noOfCW = 3;
                    }
                    else if (product.InvoiceMWLength > 3000 && product.InvoiceMWLength <= 4000)
                    {
                        noOfCW = 4;
                    }
                    else if (product.InvoiceMWLength > 4000)
                    {
                        noOfCW = 5;
                    }

                    try
                    {
                        inputsProductMarking.Add("no_cw", noOfCW.ToString());
                    }

                    catch { }
                    string validationsProductMark = "";
                    resultProductMarking = objInfo.ExecuteDetailingComponent(shapeId, "");
                    if (resultProductMarking != null)
                    {
                        validationsProductMark = resultProductMarking["ValidationFailed"];
                        resultProductMarking.Remove("ValidationFailed");
                        if (validationsProductMark == "False")
                        {

                            product.LinkWidth = Convert.ToInt32(Double.Parse(resultProductMarking["link_width"]));
                            product.LinkLength = Convert.ToInt32(Double.Parse(resultProductMarking["link_length"]));
                            product.InvoiceMWLength = Convert.ToInt32(Double.Parse(resultProductMarking["mw_length"]));
                            product.MO1 = Convert.ToInt32(Double.Parse(resultProductMarking["mo1"]));
                            product.CO1 = Convert.ToInt32(Double.Parse(resultProductMarking["co1"]));
                            product.CO2 = Convert.ToInt32(Double.Parse(resultProductMarking["co2"]));
                            product.InvoiceArea = Double.Parse(resultProductMarking["area_post"]);
                            product.TheoraticalWeight = Double.Parse(resultProductMarking["theoretical_tonnage_post"]);
                            product.InvoiceWeight = Double.Parse(resultProductMarking["theoretical_tonnage_post"]);


                            product.ProductionWeight = Double.Parse(resultProductMarking["actual_tonnage_post"]);

                            product.ParamValues = resultProductMarking["parameters_post"];

                            product.MWBVBSString = resultProductMarking["mw_bvbs_string"];

                            product.CWBVBSString = resultProductMarking["cw_bvbs_string"];

                            product.ProductionMWLength = RoundUpBy5Setp(Convert.ToInt32(Math.Round(Convert.ToDouble((resultProductMarking["mw_production_length"])))));
                           
                            //product.MWSpacing = Convert.ToInt32(Double.Parse(resultProductMarking["mw_spacing_field"]));
                            //product.PinSize = Convert.ToInt32(Double.Parse(resultProductMarking["pin_diameter_field"]));
                            //product.NoofLinks = Convert.ToInt32(Double.Parse(resultProductMarking["no_of_links_field"]));
                            //product.InvoiceMWQty = Convert.ToInt32(Double.Parse(resultProductMarking["no_of_links_field"]));
                            //product.InvoiceCWQty = Convert.ToInt32(Double.Parse(resultProductMarking["no_of_cw_field"]));

                            //To change - code by aishwarya--------
                            product.MWSpacing = structureMarking.ProductCode.MainWireSpacing;
                            product.PinSize = 32;
                            product.NoofLinks = product.NoofLinks;
                            product.InvoiceMWQty = product.NoofLinks;
                            product.InvoiceCWQty = noOfCW;

                            //CWSpacingString
                            int cwSpacing1 = 0;
                            int cwSpacing2 = 0;
                            int cwSpacing3 = 0;
                            int cwSpacing4 = 0;
                            switch (product.InvoiceCWQty)
                            {
                                case 2:
                                    {
                                        product.MO2 = Convert.ToInt32(Double.Parse(resultProductMarking["mo2_2_post"]));
                                        cwSpacing1 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_1_2_post"]));
                                        cwSpacing2 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_2_2_post"]));
                                        cwSpacing3 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_3_2_post"]));
                                        cwSpacing4 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_4_2_post"]));
                                        break;
                                    }
                                case 3:
                                    {
                                        product.MO2 = Convert.ToInt32(Double.Parse(resultProductMarking["mo2_3_post"]));
                                        cwSpacing1 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_1_field"]));
                                        cwSpacing2 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_2_3_post"]));
                                        cwSpacing3 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_3_2_post"]));
                                        cwSpacing4 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_4_2_post"]));

                                        break;
                                    }
                                case 4:
                                    {
                                        product.MO2 = Convert.ToInt32(Double.Parse(resultProductMarking["mo2_4_post"]));
                                        cwSpacing1 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_1_field"]));
                                        cwSpacing2 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_2_4_post"]));
                                        cwSpacing3 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_3_4_post"]));
                                        cwSpacing4 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_4_2_post"]));
                                        break;
                                    }
                                case 5:
                                    {
                                        product.MO2 = Convert.ToInt32(Double.Parse(resultProductMarking["mo2_5_post"]));
                                        cwSpacing1 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_1_field"]));
                                        cwSpacing2 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_2_5_field"]));
                                        cwSpacing3 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_3_5_post"]));
                                        cwSpacing4 = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing_4_5_post"]));
                                        break;
                                    }
                            }

                            //Production MWLength, MO1 & MO2 round Calculation.
                            product.ProductionMWLength = RoundUpBy5Setp(product.ProductionMWLength);
                            product.ProductionMO1 = RoundUpBy5Setp(Convert.ToInt32(Math.Round(Convert.ToDouble(product.MO1 - (product.InvoiceMWLength - product.ProductionMWLength) / 2))));
                            product.ProductionMO2 = product.ProductionMWLength - product.ProductionMO1 - (product.InvoiceCWQty - 1) * product.CWSpacing;

                            product.ProductionCO1 = product.CO1;
                            product.ProductionCO2 = product.CO2;

                            //Form CWSpacingString
                            product.CWSpacingString = cwSpacing1 + "ì" + cwSpacing2 + "ì" + cwSpacing3 + "ì" + cwSpacing4 + "ì";
                        }
                        else
                        {
                            foreach (var item in resultProductMarking)
                            {
                                if (item.Key != "ValidationFailed")
                                {
                                    message = item.Value;
                                    errormessage = item.Value;
                                    throw new ArgumentOutOfRangeException(errormessage);
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Product marking result is NULL");
                    }
                }

            }
            catch (Exception ex)
            {

                if (errormessage != "")
                {
                    message = errormessage;
                }
                else
                {
                    message = "Error Generating Products; Invalid Inputs: " + ex.Message;// +"Trace: " + ex.StackTrace;
                }
                products = null;
            }

            return products;
        }

        #endregion

        #region "Beam"

        public List<BeamProduct> GenerateBeamStructureMarking(BeamStructure structureMarking, out string message, int gap1, int gap2, int topCover, int bottomCover, int leftCover, int rightCover, int hook, int leg, out string bendingcheck)
        {
            bendingcheck = "";
            message = "";
            string errormessage = "";
            List<BeamProduct> products = null;

            try
            {
                int NoOfstirrups = 0;
                int no_of_links_1 = 0, no_of_links_2 = 0, no_of_beamcage_1 = 0, no_of_beamcage_2 = 0;
                double slopegradientlimit = 0.06;
                SlabDetailingComponent objInfo = new SlabDetailingComponent();
                Dictionary<string, string> inputsStructureMarking = new Dictionary<string, string>();
                inputsStructureMarking.Add("min_links", structureMarking.ProductCode.MinLinkFactor.ToString());
                inputsStructureMarking.Add("max_links", structureMarking.ProductCode.MaxLinkFactor.ToString());
                inputsStructureMarking.Add("mw_spacing", structureMarking.ProductCode.MainWireSpacing.ToString());
                inputsStructureMarking.Add("gap1", gap1.ToString());
                inputsStructureMarking.Add("gap2", gap2.ToString());

                inputsStructureMarking.Add("beam_width", structureMarking.Width.ToString());
                inputsStructureMarking.Add("beam_depth", structureMarking.Depth.ToString());
                inputsStructureMarking.Add("slope", structureMarking.Slope.ToString());
                inputsStructureMarking.Add("no_of_stirrups", structureMarking.Stirupps.ToString());
                products = new List<BeamProduct>();
                //(beam_depth - slope)/(no_of_stirrups * mw_spacing) = slopegradient
                double slopegradientvalue = Convert.ToDouble(structureMarking.Depth - structureMarking.Slope) / (structureMarking.Stirupps * structureMarking.ProductCode.MainWireSpacing);
                objInfo.FillInputDictionary(inputsStructureMarking);
                if (slopegradientvalue >= slopegradientlimit)
                {
                    message = "Invalid inputs:no_of_stirrups:" + structureMarking.Stirupps.ToString();
                }

                Dictionary<string, string> resultStructMarking = objInfo.ExecuteDetailingComponent(0, "Beam");
                if (resultStructMarking.Count == 0 && resultStructMarking == null)
                {
                    throw new Exception("resultStructMarking is NULL");
                }
                string validations = string.Empty;
                string error = string.Empty;
                if (resultStructMarking != null)
                {
                    validations = resultStructMarking["ValidationFailed"];
                    resultStructMarking.Remove("ValidationFailed");
                    if (validations == "False")
                    {
                        structureMarking.Span = Convert.ToInt32(Double.Parse(resultStructMarking["clear_span"]));
                        //Slope
                        bool isSlope = false;
                        string strSlopeStatus = "YES"; // Hardcoding to fix don't know calculation
                        if (structureMarking.Slope == structureMarking.Depth)
                        {
                            strSlopeStatus = "NO";
                        }

                        if (strSlopeStatus.ToString().Trim().ToUpper() == "YES")
                            isSlope = true;
                        //Regular without Slope
                        if (!isSlope)
                        {
                            BeamProduct newProduct = new BeamProduct();
                            newProduct.BeamDepth = structureMarking.Depth;
                            newProduct.BeamWidth = structureMarking.Width;
                            newProduct.BeamSlope = structureMarking.Slope;
                            newProduct.CageSlope = RoundUpBy5Setp(newProduct.BeamSlope - topCover - bottomCover);
                            newProduct.ShapeCode = structureMarking.Shape.ShapeCodeName;
                            newProduct.ShapeParam = structureMarking.Shape.ShapeParam;
                            newProduct.ProductCodeId = structureMarking.ProductCode.ProductCodeId;
                            newProduct.ShapeCodeId = structureMarking.Shape.ShapeID;

                            int maxLinks = structureMarking.ProductCode.MaxLinkFactor;
                            int minLinks = structureMarking.ProductCode.MinLinkFactor;
                            NoOfstirrups = structureMarking.Stirupps;
                            int div;
                            if (NoOfstirrups >= minLinks)
                            {
                                if (NoOfstirrups <= maxLinks)
                                {
                                    no_of_beamcage_1 = 1;
                                    no_of_beamcage_2 = 0;
                                    no_of_links_1 = NoOfstirrups;
                                    no_of_links_2 = 0;
                                }
                                else if (NoOfstirrups > maxLinks)
                                {
                                    int factor = 2;
                                    int m = NoOfstirrups - (maxLinks * 2);
                                    if (m > maxLinks)
                                    {
                                        factor = 3;
                                    }
                                    else
                                    {
                                        factor = 2;
                                    }

                                    int modResult = NoOfstirrups % factor;
                                    if (modResult == 0)
                                    {
                                        if (NoOfstirrups / factor <= maxLinks)
                                        {
                                            no_of_links_1 = NoOfstirrups / factor;
                                            no_of_beamcage_1 = factor;
                                            no_of_beamcage_2 = 0;
                                            no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_beamcage_1);
                                        }
                                        else
                                        {
                                            div = NoOfstirrups / maxLinks;
                                            for (int i = minLinks; i <= maxLinks; i++)
                                            {
                                                int a = div * i;
                                                int rem = NoOfstirrups - a;
                                                if (rem <= maxLinks)
                                                {
                                                    no_of_links_1 = i;
                                                    no_of_beamcage_2 = 1;
                                                    no_of_beamcage_1 = div;
                                                    no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_beamcage_1);
                                                    break;
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        div = NoOfstirrups / maxLinks;
                                        int b = NoOfstirrups - maxLinks;
                                        if (b <= maxLinks && b >= minLinks)
                                        {
                                            no_of_links_1 = maxLinks;
                                            no_of_beamcage_1 = div;
                                            no_of_beamcage_2 = 1;
                                            no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_beamcage_1);
                                        }
                                        else
                                        {
                                            for (int i = minLinks; i <= maxLinks; i++)
                                            {
                                                int a = div * i;
                                                int rem = NoOfstirrups - a;
                                                if (rem <= maxLinks)
                                                {
                                                    no_of_links_1 = i;
                                                    no_of_beamcage_2 = 1;
                                                    no_of_beamcage_1 = div;
                                                    no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_beamcage_1);
                                                    break;
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("Total no. of links should be greater than min no. of links");
                            }


                            //cw_length_1=no_of_links_1*mw_spacing
                            newProduct.InvoiceCWLength = Convert.ToInt32(no_of_links_1 * structureMarking.ProductCode.MainWireSpacing);
                            newProduct.NoofLinks = Convert.ToInt32(no_of_links_1);
                            newProduct.Quantity = Convert.ToInt32(no_of_beamcage_1);
                            newProduct.BOMIndicator = "S";


                            products.Add(newProduct);

                            newProduct = new BeamProduct();
                            newProduct.BeamDepth = structureMarking.Depth;
                            newProduct.BeamWidth = structureMarking.Width;
                            newProduct.BeamSlope = structureMarking.Slope;
                            newProduct.CageSlope = RoundUpBy5Setp(newProduct.BeamSlope - topCover - bottomCover);
                            newProduct.ShapeCode = structureMarking.Shape.ShapeCodeName;
                            newProduct.ShapeParam = structureMarking.Shape.ShapeParam;
                            newProduct.ProductCodeId = structureMarking.ProductCode.ProductCodeId;
                            newProduct.ShapeCodeId = structureMarking.Shape.ShapeID;

                            //no_of_links_2*mw_spacing
                            newProduct.InvoiceCWLength = Convert.ToInt32(no_of_links_2 * structureMarking.ProductCode.MainWireSpacing);
                            newProduct.NoofLinks = Convert.ToInt32(no_of_links_2);
                            newProduct.Quantity = Convert.ToInt32(no_of_beamcage_2);
                            newProduct.BOMIndicator = "S";
                            if (newProduct.NoofLinks != 0) products.Add(newProduct);

                        }
                        //With Slope
                        else
                        {
                            int totalNumberOfCages;
                            int noOfCages1;
                            int noOfLinks1;
                            int cwLength1;
                            int noOfCages2;
                            int noOfLinks2;
                            int cwLength2;
                            double slopeVariant;
                            int maxLinks = structureMarking.ProductCode.MaxLinkFactor;
                            int minLinks = structureMarking.ProductCode.MinLinkFactor;
                            NoOfstirrups = structureMarking.Stirupps;
                            int div;
                            if (NoOfstirrups >= minLinks)
                            {
                                if (NoOfstirrups <= maxLinks)
                                {
                                    no_of_beamcage_1 = 1;
                                    no_of_beamcage_2 = 0;
                                    no_of_links_1 = NoOfstirrups;
                                    no_of_links_2 = 0;
                                }
                                else if (NoOfstirrups > maxLinks)
                                {
                                    int factor = 2;
                                    int m = NoOfstirrups - (maxLinks * 2);
                                    if (m > maxLinks)
                                    {
                                        factor = 3;
                                    }
                                    else
                                    {
                                        factor = 2;
                                    }

                                    int modResult = NoOfstirrups % factor;
                                    if (modResult == 0)
                                    {
                                        if (NoOfstirrups / factor <= maxLinks)
                                        {
                                            no_of_links_1 = NoOfstirrups / factor;
                                            no_of_beamcage_1 = factor;
                                            no_of_beamcage_2 = 0;
                                            no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_beamcage_1);
                                        }
                                        else
                                        {
                                            div = NoOfstirrups / maxLinks;
                                            for (int i = minLinks; i <= maxLinks; i++)
                                            {
                                                int a = div * i;
                                                int rem = NoOfstirrups - a;
                                                if (rem <= maxLinks)
                                                {
                                                    no_of_links_1 = i;
                                                    no_of_beamcage_2 = 1;
                                                    no_of_beamcage_1 = div;
                                                    no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_beamcage_1);
                                                    break;
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        div = NoOfstirrups / maxLinks;
                                        int b = NoOfstirrups - maxLinks;
                                        if (b <= maxLinks && b >= minLinks)
                                        {
                                            no_of_links_1 = maxLinks;
                                            no_of_beamcage_1 = div;
                                            no_of_beamcage_2 = 1;
                                            no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_beamcage_1);
                                        }
                                        else
                                        {
                                            for (int i = minLinks; i <= maxLinks; i++)
                                            {
                                                int a = div * i;
                                                int rem = NoOfstirrups - a;
                                                if (rem <= maxLinks)
                                                {
                                                    no_of_links_1 = i;
                                                    no_of_beamcage_2 = 1;
                                                    no_of_beamcage_1 = div;
                                                    no_of_links_2 = NoOfstirrups - (no_of_links_1 * no_of_beamcage_1);
                                                    break;
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("Total no. of links should be greater than min no. of links");
                            }




                            totalNumberOfCages = Convert.ToInt32(no_of_beamcage_2);
                            noOfCages1 = Convert.ToInt32(no_of_beamcage_1);
                            noOfLinks1 = Convert.ToInt32(no_of_links_1);
                            //no_of_links_1*mw_spacing
                            cwLength1 = Convert.ToInt32(no_of_links_1 * structureMarking.ProductCode.MainWireSpacing);
                            noOfCages2 = Convert.ToInt32(no_of_beamcage_2);
                            noOfLinks2 = Convert.ToInt32(no_of_links_2);
                            cwLength2 = Convert.ToInt32(no_of_links_2 * structureMarking.ProductCode.MainWireSpacing);

                            //slope_variant=(beam_depth-slope)/(no_of_stirrups*mw_spacing)
                            double slopevariantvalue = Convert.ToDouble(structureMarking.Depth - structureMarking.Slope) / (structureMarking.Stirupps * structureMarking.ProductCode.MainWireSpacing);
                            slopeVariant = slopevariantvalue;

                            int totalCages = no_of_beamcage_1 + no_of_beamcage_2;

                            //Get the group and generate products

                            for (int i = 1; i <= totalCages; i++)
                            {
                                BeamProduct newProduct = new BeamProduct();

                                newProduct.BeamWidth = structureMarking.Width;


                                //cage_part.depth=round(beam_depth-slope_variant*(cage_part.loc_inst-1)*no_of_links_1*mw_spacing)

                                double cageDepth = Math.Round(structureMarking.Depth - (slopevariantvalue * (i - 1) * no_of_links_1 * structureMarking.ProductCode.MainWireSpacing));

                                newProduct.BeamDepth = RoundUpBy5Setp(Convert.ToInt32(Math.Round(Convert.ToDecimal(cageDepth))));
                                double cagebeamslope = 0;
                                if (i == totalCages)
                                {
                                    cagebeamslope = structureMarking.Slope;
                                    newProduct.BeamSlope = RoundUpBy5Setp(Convert.ToInt32(Math.Round(Convert.ToDecimal(cagebeamslope))));
                                }
                                else
                                {
                                    //round(beam depth-slope variant*cage part.loc inst*no of links 1*mw spacing)
                                    cagebeamslope = Math.Round(structureMarking.Depth - (slopevariantvalue * i * no_of_links_1 * structureMarking.ProductCode.MainWireSpacing));
                                    newProduct.BeamSlope = RoundUpBy5Setp(Convert.ToInt32(Math.Round(Convert.ToDecimal(cagebeamslope))));
                                }

                                //Parameter slope = (Parameter)slopeDepthGruop.Parameters[i + slopeDepthGruop.Parameters.Count / 2];


                                newProduct.CageSlope = RoundUpBy5Setp(newProduct.BeamSlope - topCover - bottomCover);
                                newProduct.ShapeCode = structureMarking.Shape.ShapeCodeName;
                                newProduct.ShapeParam = structureMarking.Shape.ShapeParam;
                                newProduct.ProductCodeId = structureMarking.ProductCode.ProductCodeId;
                                newProduct.ShapeCodeId = structureMarking.Shape.ShapeID;
                                newProduct.BOMIndicator = "S";
                                if (i == (totalCages) && noOfCages2 == 1)
                                {
                                    newProduct.InvoiceCWLength = cwLength2;
                                    newProduct.NoofLinks = noOfLinks2;
                                    newProduct.Quantity = 1;
                                }
                                else
                                {
                                    newProduct.InvoiceCWLength = cwLength1;
                                    newProduct.NoofLinks = noOfLinks1;
                                    newProduct.Quantity = 1;
                                }
                                products.Add(newProduct);
                            }
                        }
                    }
                }
                if (products == null) throw new Exception("products is NULL");
                foreach (BeamProduct product in products)
                {

                    objInfo.ClearDictionary();
                    //Product Marking Generation
                    Dictionary<string, string> inputsProductMarking = new Dictionary<string, string>();
                    inputsProductMarking.Add("beam_width", product.BeamWidth.ToString());
                    inputsProductMarking.Add("beam_depth", product.BeamDepth.ToString());
                    inputsProductMarking.Add("top_cover", topCover.ToString());
                    inputsProductMarking.Add("bottom_cover", bottomCover.ToString());
                    inputsProductMarking.Add("right_cover", rightCover.ToString());
                    inputsProductMarking.Add("left_cover", leftCover.ToString());
                    inputsProductMarking.Add("no_of_link", product.NoofLinks.ToString());
                    //inputsProductMarking.Add("cw_spacing", "400");
                    //inputsProductMarking.Add("no_of_link_1", no_of_links_1.ToString());

                    //  foreach (ShapeParameter sp in product.ShapeParam)
                    //{
                    //if (sp.ParameterName.ToUpper().Trim() == "LEG" && sp.VisibleFlag == true)
                    //{
                    //    if (sp.ParameterValue == 0)
                    //    {
                    //        sp.ParameterValue = leg;
                    //    }
                    //    inputsProductMarking.Add("leg", sp.ParameterValue.ToString());
                    //}
                    //else if (sp.ParameterName.ToUpper().Trim() == "HOOK" && sp.VisibleFlag == true)
                    //{
                    //    if (sp.ParameterValue == 0)
                    //    {
                    //        sp.ParameterValue = RoundUpBy5Setp(Convert.ToInt32(Math.Round(structureMarking.PinSize + (2.5 * structureMarking.ProductCode.MainWireDia))));
                    //    }
                    //    inputsProductMarking.Add("hook", sp.ParameterValue.ToString());
                    //}

                    foreach (var sp in product.ShapeParam)
                    {
                        if (sp != null)
                        {
                            var options = new JsonSerializerOptions()
                            {
                                NumberHandling = JsonNumberHandling.AllowReadingFromString |
                                     JsonNumberHandling.WriteAsString
                            };
                            // serialize
                            string denemeJson = JsonSerializer.Serialize<JsonElement>((JsonElement)sp, options);
                            // deserialize

                            ShapeParameter shapeparamObj = JsonSerializer.Deserialize<ShapeParameter>(denemeJson, options);

                            if (shapeparamObj.ParameterName.ToUpper().Trim() == "LEG" && shapeparamObj.VisibleFlag == true)
                            {
                                if (shapeparamObj.ParameterValue == 0)
                                {
                                    shapeparamObj.ParameterValue = leg;
                                }
                                inputsProductMarking.Add("leg", shapeparamObj.ParameterValue.ToString());
                                //legValue = shapeparamObj.ParameterValue;
                            }
                            else if (shapeparamObj.ParameterName.ToUpper().Trim() == "HOOK" && shapeparamObj.VisibleFlag == true)
                            {
                                if (shapeparamObj.ParameterValue == 0)
                                {
                                    shapeparamObj.ParameterValue = RoundUpBy5Setp(Convert.ToInt32(Math.Round(structureMarking.PinSize + (2.5 * structureMarking.ProductCode.MainWireDia))));
                                }
                                inputsProductMarking.Add("hook", shapeparamObj.ParameterValue.ToString());
                            }




                            //ShapeParameters for Special Beam Shapes
                            string specialBeamShapes = "2M1B;CR1;L1;RL1;LF;LH;LH2;LH3;LH4;2M11B;2MR11B;3M13B;3MR13B;4M13B;4MR13B;C1;2MR1B;LH1;3M2B;LH5;SC5;SCR5";
                            List<string> specialBeamShapesList = new List<string>(specialBeamShapes.Split(';'));
                            if (specialBeamShapesList.Contains(product.ShapeCode.ToUpper().Trim()))
                            {
                                //shape_code_a_field
                                if (shapeparamObj.ParameterName.ToUpper().Trim() == "A" && shapeparamObj.EditFlag == true)
                                {
                                    //Changed for 2M1B & 2MR1B 26-07-2017
                                    if (product.ShapeCode.ToUpper().Trim() == "2M1B" || product.ShapeCode.ToUpper().Trim() == "2MR1B")
                                    {
                                        if (shapeparamObj.ParameterValue == 0)
                                            shapeparamObj.ParameterValue = product.BeamDepth - topCover - bottomCover;
                                    }
                                    //End of change 2M1b & 2MR1B     

                                    inputsProductMarking.Add("a", shapeparamObj.ParameterValue.ToString());
                                }
                                //shape_code_b_field
                                else if (shapeparamObj.ParameterName.ToUpper().Trim() == "B" && shapeparamObj.EditFlag == true)
                                {
                                    //Changed for 2M1B & 2MR1B 26-07-2017
                                    if (product.ShapeCode.ToUpper().Trim() == "2M1B" || product.ShapeCode.ToUpper().Trim() == "2MR1B")
                                    {
                                        if (shapeparamObj.ParameterValue == 0)
                                            shapeparamObj.ParameterValue = product.BeamWidth - leftCover - rightCover;
                                    }
                                    //End of change 2M1b & 2MR1B 

                                    inputsProductMarking.Add("b", shapeparamObj.ParameterValue.ToString());
                                }
                                //shape_code_c_field
                                else if (shapeparamObj.ParameterName.ToUpper().Trim() == "C" && shapeparamObj.EditFlag == true)
                                {
                                    //Changed for 2M1B & 2MR1B 26-07-2017
                                    if (product.ShapeCode.ToUpper().Trim() == "2M1B" || product.ShapeCode.ToUpper().Trim() == "2MR1B")
                                    {
                                        if (shapeparamObj.ParameterValue == 0)
                                            shapeparamObj.ParameterValue = product.BeamDepth - topCover - bottomCover;
                                    }
                                    //End of change 2M1b & 2MR1B 

                                    inputsProductMarking.Add("c", shapeparamObj.ParameterValue.ToString());
                                }
                                //shape_code_d_field
                                else if (shapeparamObj.ParameterName.ToUpper().Trim() == "D" && shapeparamObj.EditFlag == true)
                                {
                                    inputsProductMarking.Add("d", shapeparamObj.ParameterValue.ToString());
                                }
                                //shape_code_e_field
                                else if (shapeparamObj.ParameterName.ToUpper().Trim() == "E" && shapeparamObj.EditFlag == true)
                                {
                                    inputsProductMarking.Add("e", shapeparamObj.ParameterValue.ToString());
                                }
                                //shape_code_f_field
                                else if (shapeparamObj.ParameterName.ToUpper().Trim() == "F" && shapeparamObj.EditFlag == true)
                                {
                                    inputsProductMarking.Add("f", shapeparamObj.ParameterValue.ToString());
                                }
                                //shape_code_p_field
                                else if (shapeparamObj.ParameterName.ToUpper().Trim() == "P" && shapeparamObj.EditFlag == true)
                                {
                                    inputsProductMarking.Add("p", shapeparamObj.ParameterValue.ToString());
                                }
                                //shape_code_q_field
                                else if (shapeparamObj.ParameterName.ToUpper().Trim() == "Q" && shapeparamObj.EditFlag == true)
                                {
                                    inputsProductMarking.Add("q", shapeparamObj.ParameterValue.ToString());
                                }
                            }
                        }

                    }
                    inputsProductMarking.Add("cw_length", product.InvoiceCWLength.ToString());
                    inputsProductMarking.Add("mw_spacing", structureMarking.ProductCode.MainWireSpacing.ToString());
                    inputsProductMarking.Add("mw_dia", structureMarking.ProductCode.MainWireDia.ToString());
                    inputsProductMarking.Add("cw_dia", structureMarking.ProductCode.CwDia.ToString());
                    inputsProductMarking.Add("weight_per_area", structureMarking.ProductCode.WeightArea.ToString());
                    inputsProductMarking.Add("mw_weight_per_m_run", structureMarking.ProductCode.WeightPerMeterRun.ToString());
                    inputsProductMarking.Add("cw_weight_per_m_run", structureMarking.ProductCode.CwWeightPerMeterRun.ToString());
                    inputsProductMarking.Add("pin", "32");



                    double mw_length_value = objInfo.GetMWLength(product.ShapeCodeId, inputsProductMarking);


                    int noOfCW = 2;
                    if (mw_length_value < 1400 && mw_length_value >= 100)
                    {
                        noOfCW = 2;
                    }
                    else if (mw_length_value >= 1400 && mw_length_value <= 3000)
                    {
                        noOfCW = 3;
                    }
                    else if (mw_length_value < 4001 && mw_length_value > 3000)
                    {
                        noOfCW = 4;
                    }
                    else if (mw_length_value < 5001 && mw_length_value > 4000)
                    {
                        noOfCW = 5;
                    }
                    else if (mw_length_value <= 6000 && mw_length_value > 5000)
                    {
                        noOfCW = 6;
                    }

                    inputsProductMarking.Add("no_cw", noOfCW.ToString());
                    objInfo.FillInputDictionary(inputsProductMarking);
                
                    Dictionary<string, string> resultProductMarking = new Dictionary<string, string>();
                    //Result
                
                    resultProductMarking = objInfo.ExecuteDetailingComponent(product.ShapeCodeId, "");
                    string validationsProduct = string.Empty;
                    string errorProduct = string.Empty;
                    if (resultProductMarking != null)
                    {
                        validations = resultProductMarking["ValidationFailed"];
                        resultProductMarking.Remove("ValidationFailed");
                        if (validations == "False")
                        {
                            product.CageWidth = Convert.ToInt32(Double.Parse(resultProductMarking["cage_width"]));
                            product.CageDepth = Convert.ToInt32(Double.Parse(resultProductMarking["cage_depth"]));
                            product.InvoiceMWLength = Convert.ToInt32(mw_length_value);

                            product.MO1 = Convert.ToInt32(Double.Parse(resultProductMarking["mo1"]));
                            product.MO2 = Convert.ToInt32(Double.Parse(resultProductMarking["mo2"]));
                            product.CO1 = Convert.ToInt32(Double.Parse(resultProductMarking["co1"]));
                            product.CO2 = Convert.ToInt32(Double.Parse(resultProductMarking["co2"]));
                            product.InvoiceArea = Convert.ToDouble(Double.Parse(resultProductMarking["area_post"]));
                            product.TheoraticalWeight = Convert.ToDouble(Double.Parse(resultProductMarking["theoretical_tonnage"]));
                            product.InvoiceWeight = Convert.ToDouble(Double.Parse(resultProductMarking["theoretical_tonnage"]));

                            product.ProductionWeight = Convert.ToDouble(Double.Parse(resultProductMarking["actual_tonnage_post"]));
                            product.ParamValues = resultProductMarking["parameters_post"];
                            product.MWBVBSString = resultProductMarking["mw_bvbs_string"];
                            product.CWBVBSString = resultProductMarking["cw_bvbs_string"];
                            product.ProductionMWLength = RoundUpBy5Setp(Convert.ToInt32(Math.Round(Convert.ToDouble(Double.Parse(resultProductMarking["production_mw_length"])))));


                            product.MWSpacing = Convert.ToInt32(structureMarking.ProductCode.MainWireSpacing);
                            product.CWSpacing = Convert.ToInt32(Double.Parse(resultProductMarking["cw_spacing"]));
                            product.PinSize = Convert.ToInt32(32);
                            product.NoofLinks = Convert.ToInt32(product.NoofLinks);
                            product.InvoiceMWQty = Convert.ToInt32(product.NoofLinks);
                            product.InvoiceCWQty = Convert.ToInt32(noOfCW);

                            //Invoice MWLength, MO1 & MO2 round Calculation.
                            product.InvoiceMWLength = RoundUpBy5Setp(product.InvoiceMWLength);
                            product.MO1 = RoundUpBy5Setp(product.MO1);
                            product.MO2 = product.InvoiceMWLength - product.MO1 - (product.InvoiceCWQty - 1) * product.CWSpacing;

                            //Added by Panchatapa for Beam Cage CR

                            if (structureMarking.Shape.ShapeCodeName != "5M1B" && structureMarking.Shape.ShapeCodeName != "5MR1B" && structureMarking.Shape.ShapeCodeName != "2M1B" && structureMarking.Shape.ShapeCodeName != "2MR1B")
                            {


                                Int32 MO1balance = 0, MO2balance = 0, TotMOBalance = 0, DistributedBalance = 0, remaining = 0;

                                if (product.InvoiceMWLength <= 1400)
                                {
                                    if (product.MO1 != 300)
                                    {
                                        MO1balance = product.MO1 - 300;
                                    }

                                    if (product.MO2 != 300)
                                    {
                                        MO2balance = product.MO2 - 300;
                                    }
                                }

                                if (product.InvoiceMWLength >= 1400)
                                {
                                    if (product.MO1 != 460)
                                    {
                                        MO1balance = product.MO1 - 460;
                                    }

                                    if (product.MO2 != 460)
                                    {
                                        MO2balance = product.MO2 - 460;
                                    }
                                }




                                TotMOBalance = MO1balance + MO2balance;

                                Int32 modbalance = 0;

                                if (TotMOBalance != 0)
                                {
                                    modbalance = TotMOBalance % (product.InvoiceCWQty - 1);
                                }


                                if (product.InvoiceMWLength <= 1400)
                                {
                                    product.MO1 = 300;
                                    product.MO2 = 300;

                                }
                                else if (product.InvoiceMWLength >= 1400)
                                {
                                    product.MO1 = 460;
                                    product.MO2 = 460;
                                }

                                //if (modbalance > 0) 
                                //{
                                //    TotMOBalance = TotMOBalance - modbalance;

                                //    if (modbalance % 2 == 1)
                                //    {
                                //        product.MO1 = product.MO1 + ((modbalance - 1) / 2);
                                //        product.MO2 = product.MO2 + ((modbalance + 1) / 2);
                                //    }
                                //    else if (modbalance % 2 == 0)
                                //    {
                                //        product.MO1 = product.MO1 + (modbalance / 2);
                                //        product.MO2 = product.MO2 + (modbalance / 2);
                                //    }
                                //}

                                //if (modbalance < 0) 
                                //{
                                //    modbalance = -1 * modbalance;
                                //    TotMOBalance = TotMOBalance - modbalance;

                                //    if (modbalance % 2 == 1)
                                //    {
                                //        product.MO1 = product.MO1 + ((modbalance - 1) / 2);
                                //        product.MO2 = product.MO2 + ((modbalance + 1) / 2);
                                //    }
                                //    else if (modbalance % 2 == 0)
                                //    {
                                //        product.MO1 = product.MO1 + (modbalance / 2);
                                //        product.MO2 = product.MO2 + (modbalance / 2);
                                //    }
                                //}


                                if (TotMOBalance > 0)
                                {
                                    DistributedBalance = (TotMOBalance / (product.InvoiceCWQty - 1));
                                    product.CWSpacing = product.CWSpacing + DistributedBalance;
                                    remaining = TotMOBalance - (DistributedBalance * (product.InvoiceCWQty - 1));
                                    product.MO1 = product.MO1 + remaining;

                                }
                                if (TotMOBalance < 0)
                                {
                                    DistributedBalance = (TotMOBalance / (product.InvoiceCWQty - 1));
                                    product.CWSpacing = product.CWSpacing + DistributedBalance;
                                    remaining = TotMOBalance - (DistributedBalance * (product.InvoiceCWQty - 1));
                                    product.MO1 = product.MO1 + remaining;

                                }

                                Int32 invlength = 0, diff = 0;
                                invlength = product.MO1 + product.MO2 + product.CWSpacing * (product.InvoiceCWQty - 1);

                                if (product.InvoiceMWLength != invlength)
                                {
                                    if (product.InvoiceMWLength > invlength)
                                    {
                                        diff = product.InvoiceMWLength - invlength;
                                        if (diff % 2 == 0)
                                        {
                                            product.MO1 = product.MO1 + (diff / 2);
                                            product.MO2 = product.MO2 + (diff / 2);
                                        }
                                        if (diff % 2 == 1)
                                        {
                                            product.MO1 = product.MO1 + ((diff - 1) / 2);
                                            product.MO2 = product.MO2 + ((diff + 1) / 2);
                                        }

                                    }

                                    if (product.InvoiceMWLength < invlength)
                                    {
                                        diff = invlength - product.InvoiceMWLength;
                                        if (diff % 2 == 0)
                                        {
                                            product.MO1 = product.MO1 - (diff / 2);
                                            product.MO2 = product.MO2 - (diff / 2);
                                        }
                                        if (diff % 2 == 1)
                                        {
                                            product.MO1 = product.MO1 - ((diff - 1) / 2);
                                            product.MO2 = product.MO2 - ((diff + 1) / 2);
                                        }

                                    }
                                }


                                //Rounding to nearest 100

                                Int32 roundCWSpacing = 0, adjfactor = 0;
                                roundCWSpacing = product.CWSpacing % 100;
                                if (product.InvoiceMWLength >= 1400)
                                {
                                    if (roundCWSpacing != 0)
                                    {
                                        if (roundCWSpacing >= 50)
                                        {
                                            adjfactor = 100 - roundCWSpacing;
                                            product.CWSpacing = product.CWSpacing + adjfactor;
                                            adjfactor = adjfactor * (product.InvoiceCWQty - 1);
                                            if (adjfactor % 2 == 0)
                                            {
                                                product.MO1 = product.MO1 - (adjfactor / 2);
                                                product.MO2 = product.MO2 - (adjfactor / 2);
                                            }

                                            if (adjfactor % 2 == 1)
                                            {
                                                product.MO1 = product.MO1 - ((adjfactor + 1) / 2);
                                                product.MO2 = product.MO2 - ((adjfactor - 1) / 2);
                                            }

                                        }
                                        if (roundCWSpacing < 50)
                                        {
                                            adjfactor = roundCWSpacing;
                                            product.CWSpacing = product.CWSpacing - adjfactor;
                                            adjfactor = adjfactor * (product.InvoiceCWQty - 1);
                                            if (adjfactor % 2 == 0)
                                            {
                                                product.MO1 = product.MO1 + (adjfactor / 2);
                                                product.MO2 = product.MO2 + (adjfactor / 2);
                                            }

                                            if (adjfactor % 2 == 1)
                                            {
                                                product.MO1 = product.MO1 + ((adjfactor + 1) / 2);
                                                product.MO2 = product.MO2 + ((adjfactor - 1) / 2);
                                            }

                                        }
                                    }
                                }



                            }


                            //Added for Beam Cage CR

                            //Production MWLength, MO1 & MO2 round Calculation.
                            product.ProductionMWLength = RoundUpBy5Setp(product.ProductionMWLength);
                            product.ProductionMO1 = RoundUpBy5Setp(Convert.ToInt32(Math.Round(Convert.ToDouble(product.MO1 - (product.InvoiceMWLength - product.ProductionMWLength) / 2))));
                            product.ProductionMO2 = product.ProductionMWLength - product.ProductionMO1 - (product.InvoiceCWQty - 1) * product.CWSpacing;
                            product.ProductionCO1 = product.CO1;
                            product.ProductionCO2 = product.CO2;

                            //Added for Beam Cage CR
                            if (structureMarking.Shape.ShapeCodeName != "5M1B" && structureMarking.Shape.ShapeCodeName != "5MR1B" && structureMarking.Shape.ShapeCodeName != "2M1B" && structureMarking.Shape.ShapeCodeName != "2MR1B")
                            {
                                if (product.InvoiceMWLength < 700)
                                {
                                    message = "Invoice MW Length cannot be less than 700";
                                    products = null;
                                }
                                if (product.InvoiceMWLength > 6000)
                                {
                                    message = "Invoice MW Length cannot be more than 6000";
                                    products = null;
                                }
                                if (product.CageDepth > 1800)
                                {
                                    message = "Beam Cage max. Depth cannot be > 1800mm";
                                    products = null;
                                }
                                if (product.CageWidth > 3600)
                                {
                                    message = "Beam Cage max. Width cannot be > 3600mm";
                                    products = null;
                                }

                                Int32 BeamCageWidth = 0, BeamCageDepth = 0, BCTopCover = 0, BCBottomCover = 0, BCLeftCover = 0, BCRightCover = 0, BCDepthClearance = 0, BCWidthClearance = 0;

                                BeamCageWidth = Convert.ToInt32(product.BeamWidth.ToString());
                                BeamCageDepth = Convert.ToInt32(product.BeamDepth.ToString());
                                BCTopCover = Convert.ToInt32(topCover.ToString());
                                BCBottomCover = Convert.ToInt32(bottomCover.ToString());
                                BCLeftCover = Convert.ToInt32(leftCover.ToString());
                                BCRightCover = Convert.ToInt32(rightCover.ToString());
                                BCDepthClearance = BeamCageDepth - BCTopCover - BCBottomCover;
                                BCWidthClearance = BeamCageWidth - BCRightCover - BCLeftCover;

                                if (BCDepthClearance < 130)
                                {
                                    message = "Beam Cage depth (without cover) cannot be less than 130";
                                }

                                if (BCWidthClearance < 130)
                                {
                                    message = "Beam Cage width (without cover) cannot be less than 130";
                                }


                            }


                            //Added by Panchatapa on 14th Sep 2016 to handle changes in 2M1B and 2MR1B shapes

                            if (structureMarking.Shape.ShapeCodeName != "2M1B" || structureMarking.Shape.ShapeCodeName != "2MR1B")
                            {
                                if (product.InvoiceMWLength < 350)
                                {
                                    message = "Invoice MW Length cannot be less than 350";
                                    products = null;
                                }
                            }

                            //Added by Panchatapa on 14th Sep 2016 to handle changes in 2M1B and 2MR1B shapes 

                        }
                        else
                        {
                            foreach (var item in resultProductMarking)
                            {
                                if (item.Key != "ValidationFailed")
                                {
                                    message = item.Value;
                                    errormessage = item.Value;
                                    throw new ArgumentOutOfRangeException(errormessage);
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Product marking result is NULL");
                    }
                }
                
            }
            catch (Exception ex)
            {

                if (errormessage != "")
                {
                    message = errormessage;
                }
                else
                {
                    message = "Error Generating Products; Invalid Inputs: " + ex.Message + "Trace: " + ex.StackTrace;
                }
                products = null;
            }

            return products;
        }

        #endregion

        #region "Carpet"

        public List<CarpetProduct> GenerateCarpetStructureMarking(CarpetStructure structureMarking, int projectTypeId, bool isSingleMesh, int structureElementId, int projectId, int productTypeId, out string message, out string meshSplittingStatus)
        {
            message = "";
            meshSplittingStatus = "";

            List<CarpetProduct> products = null;
            CarpetProduct newProduct = null;
            products = new List<CarpetProduct>();
            try
            {
                ShapeCodeParameterSet objShapeCode = new ShapeCodeParameterSet();
                List<ShapeCodeParameterSet> listProjectParam = objShapeCode.ProjectParamLap_Get(structureMarking.ProductCode.ProductCodeId, structureMarking.ParamSetNumber);
                //Commented by Tanmay
                //ShapeCodeParameterSet projectParamFiltered = (from item in listProjectParam
                //                                              where item.ParameterSetNumber == structureMarking.ParamSetNumber && item.productCode.ProductCodeId == structureMarking.ProductCode.ProductCodeId && item.StructureElementType == structureElementId
                //                                              select item).FirstOrDefault();

                ShapeCodeParameterSet projectParamFiltered = (from item in listProjectParam
                                                              where item.ParameterSetNumber == structureMarking.ParamSetNumber && item.productCode.ProductCodeId == structureMarking.ProductCode.ProductCodeId && item.StructureElementType == 0
                                                              select item).FirstOrDefault();
                if (projectTypeId == 0)
                {
                    throw new Exception("Project Type is not mapped to the Project.Map the same and continue.");
                }
                
                if (projectParamFiltered == null) throw new Exception("MWLap and CWLap not found for the selected product and parameter set.");

                CarpetSplitingHelper objCarpetSplittingHelper = new CarpetSplitingHelper();
                List<ValidMeshProduct> ListMeshProduct = new List<ValidMeshProduct>();

                
                products = new List<CarpetProduct>();
                newProduct = new CarpetProduct();
                newProduct.CWLength = structureMarking.CrossWireLength;
                newProduct.MWLength = structureMarking.MainWireLength;
                newProduct.MemberQty = 1;

                products.Add(newProduct);


                if (products == null) throw new Exception("products is NULL");
                foreach (CarpetProduct product in products)
                {
                    //Set the properties for 'F' Shape without accessing tacton.
                    ShapeParameter shapeParam = new ShapeParameter { ShapeId = 9, ParameterName = "A", ParameterValue = 0, CriticalIndiacator = "N", WireType = "M", AngleType = "S", IsVisible = true, SequenceNumber = 1, ShapeCodeImage = "N", EditFlag = true, VisibleFlag = true };
                    ShapeParameterCollection shapeParamCollection = new ShapeParameterCollection();
                    shapeParamCollection.Add(shapeParam);
                    // PRODUCTION 
                    product.shapecode = new ShapeCode { ShapeID = 2394, ShapeCodeName = "N", MeshShapeGroup = "normal", CreepDeductAtCO1 = false, MOCO = "", CreepDeductAtMO1 = false, BendIndicator = false, IsCapping = false, ShapeParam = shapeParamCollection };
                    // QA
                    //product.shapecode = new ShapeCode { ShapeID = 2307, ShapeCodeName = "N", MeshShapeGroup = "normal", CreepDeductAtCO1 = false, MOCO = "", CreepDeductAtMO1 = false, BendIndicator = false, IsCapping = false, ShapeParam = shapeParamCollection };
                    product.ProductCodeId = structureMarking.ProductCode.ProductCodeId;
                    product.ProductionMWLength = product.MWLength;
                    product.ProductionCWLength = product.CWLength;
                    product.MWSpacing = structureMarking.ProductCode.MainWireSpacing;
                    product.CWSpacing = structureMarking.ProductCode.CrossWireSpacing;
                    product.PinSize = structureMarking.PinSize;
                    List<CarpetProduct> ListOverHang = new List<CarpetProduct>();
                    CarpetProduct objCarpetProduct = new CarpetProduct();
                    objCarpetProduct.ProductionMWLength = product.MWLength;
                    objCarpetProduct.ProductionCWLength = product.CWLength;
                    objCarpetProduct.MWSpacing = product.MWSpacing;
                    objCarpetProduct.CWSpacing = product.CWSpacing;

                    try
                    {
                        ListOverHang = objCarpetProduct.GetOverHang(structureMarking.ParamSetNumber, projectId, structureElementId, productTypeId);
                    }
                    catch
                    {
                        throw new Exception("Problem in getting the Overhang details. Please check the Parameterset.");
                    }
                    if (ListOverHang.Count > 0)
                    {
                        product.MO1 = ListOverHang[0].MO1;
                        product.MO2 = ListOverHang[0].MO2;
                        product.CO1 = ListOverHang[0].CO1;
                        product.CO2 = ListOverHang[0].CO2;
                    }

                    //Code to Adjust MO2 based on MO1.                    
                    product.NoOfCrossWire = (product.MWLength - product.MO1 - product.MO2) / product.CWSpacing + 1;
                    if (product.NoOfCrossWire < 1)
                    {
                        product.MO1 = (product.MWLength - product.CWSpacing) / 2;
                        product.MO2 = product.MO1;
                        product.NoOfCrossWire = (product.MWLength - product.MO1 - product.MO2) / product.CWSpacing + 1;
                    }
                    product.MO2 = product.MWLength - (product.NoOfCrossWire - 1) * product.CWSpacing - product.MO1;
                    product.NoOfMainWire = (product.CWLength - product.CO1 - product.CO2) / product.MWSpacing + 1;
                    product.CO2 = product.CWLength - (product.NoOfMainWire - 1) * product.MWSpacing - product.CO1;

                    product.TheoraticalWeight = (product.MWLength / 1000) * (product.CWLength / 1000) * structureMarking.ProductCode.WeightArea;
                    product.InvoiceArea = product.MWLength * product.CWLength;


                    product.NetWeight = ((product.CWLength / 1000) * product.NoOfCrossWire * structureMarking.ProductCode.CwWeightPerMeterRun) + ((product.MWLength / 1000) * product.NoOfMainWire * structureMarking.ProductCode.WeightPerMeterRun);
                    product.ProductionWeight = product.TheoraticalWeight;
                    product.EnvelopeLength = product.MWLength > product.CWLength ? product.MWLength : product.CWLength;
                    product.EnvelopeWidth = product.EnvelopeLength == product.MWLength ? product.CWLength : product.MWLength;
                    product.EnvelopeHeight = structureMarking.ProductCode.CwDia + structureMarking.ProductCode.MainWireDia;
                    product.ProduceIndicator = structureMarking.ProduceIndicator;
                    product.BOMIndicator = "S";
                    product.ParamValues = String.Format("A:{0}", product.MWLength);
                    product.InvoiceMWWeight = (structureMarking.ProductCode.WeightPerMeterRun * product.NoOfMainWire * product.MWLength) / 1000;
                    product.InvoiceCWWeight = (structureMarking.ProductCode.CwWeightPerMeterRun * product.NoOfCrossWire * product.CWLength) / 1000;
                    product.ProductionMWWeight = (structureMarking.ProductCode.WeightPerMeterRun * product.NoOfMainWire * product.ProductionMWLength) / 1000;
                    product.ProductionCWWeight = (structureMarking.ProductCode.CwWeightPerMeterRun * product.NoOfCrossWire * product.ProductionCWLength) / 1000;

                    product.ProductionMO1 = product.MO1;
                    product.ProductionMO2 = product.MO2;
                    product.ProductionCO1 = product.CO1;
                    product.ProductionCO2 = product.CO2;
                    product.ProductionMWQty = product.NoOfMainWire;
                    product.ProductionCWQty = product.NoOfCrossWire;
                    product.MWBVBSString = "BF2D@Gl" + product.MWLength + "@w0@";
                    product.CWBVBSString = "BF2D@Gl" + product.CWLength + "@w0@";
                    product.BendingCheckInd = true; /* By default bending check is true */
                }


            }
            catch (Exception ex)
            {
                message = "Error Generating Products; Invalid Inputs: " + ex.Message + "Trace: " + ex.StackTrace;
            }

            return products;
        }

        public CarpetProduct GenerateCarpetProductMarking(CarpetStructure structureMark,  CarpetProduct productMark, out bool machineCheck, out string bendingCheck, out bool transportCheck, out string message)
        {
            string errormessage = "";
            message = "";
            bendingCheck = "";
            transportCheck = false;
            machineCheck = false;
            CarpetProduct newProductMark = new CarpetProduct();

            string meshGroup = "";

            try
            {
                meshGroup = productMark.shapecode.MeshShapeGroup.ToLower().Trim();
                newProductMark = productMark;
                meshGroup = meshGroup.Trim().ToLower();
                SlabDetailingComponent objInfo = new SlabDetailingComponent();
                Dictionary<string, string> inputProductMarking = new Dictionary<string, string>();

                //program_input_group
                inputProductMarking.Add("mw_length", newProductMark.MWLength.ToString()); // EXISTING PRODUCT MARK
                inputProductMarking.Add("cw_length", newProductMark.CWLength.ToString());  // EXISTING PRODUCT MARK
                inputProductMarking.Add("mw_spacing", newProductMark.MWSpacing.ToString()); // EXISTING PRODUCT MARK
                inputProductMarking.Add("cw_spacing", newProductMark.CWSpacing.ToString()); // EXISTING PRODUCT MARK
                inputProductMarking.Add("mw_dia", structureMark.ProductCode.MainWireDia.ToString()); // PRODUCT CODE
                inputProductMarking.Add("cw_dia", structureMark.ProductCode.CwDia.ToString()); // PRODUCT CODE
                inputProductMarking.Add("mo1", newProductMark.MO1.ToString()); //   EXISTING PRODUCT MARK
                inputProductMarking.Add("mo2", newProductMark.MO2.ToString()); // EXISTING PRODUCT MARK
                inputProductMarking.Add("co1", newProductMark.CO1.ToString()); // EXISTING PRODUCT MARK
                inputProductMarking.Add("co2", newProductMark.CO2.ToString()); // EXISTING PRODUCT MARK
                inputProductMarking.Add("weight_per_area", structureMark.ProductCode.WeightArea.ToString());  // PRODUCT CODE
                inputProductMarking.Add("mw_weight_m_run", structureMark.ProductCode.WeightPerMeterRun.ToString()); // PRODUCT CODE
                inputProductMarking.Add("cw_weight_m_run", structureMark.ProductCode.CwWeightPerMeterRun.ToString()); // PRODUCT CODE
                inputProductMarking.Add("pin_dia", newProductMark.PinSize.ToString()); // PRODUCT CODE


                //ShapeParameter Loop
                foreach (var shapeParam in newProductMark.shapecode.ShapeParam)
                {
                    var options = new JsonSerializerOptions()
                    {
                        NumberHandling = JsonNumberHandling.AllowReadingFromString |
                                 JsonNumberHandling.WriteAsString
                    };
                    string denemeJson = JsonSerializer.Serialize<JsonElement>((JsonElement)shapeParam, options);
 
 
                    // deserialize
 
                    ShapeParameter shapeParamObj = JsonSerializer.Deserialize<ShapeParameter>(denemeJson, options);
                    if (shapeParamObj.EditFlag)
                    {
                        inputProductMarking.Add(shapeParamObj.ParameterName.ToLower().Trim(), shapeParamObj.ParameterValue.ToString());
                        //TactonCommit(meshGroup + "_" + shapeParam.ParameterName.ToLower().Trim() + "_field", shapeParam.ParameterValue.ToString());
                        //}
                        //else
                        //{
                        //    parameterList = objInfo.GetShapeParams(shapeId);
                        //    foreach (var item in parameterList)
                        //    {
                        //       inputs.Add(item.ItemArray[0].ToString().Trim(), item.ItemArray[1].ToString());
                        //    }
                    }
                }
                objInfo.FillInputDictionary(inputProductMarking);
                int shapeId = productMark.shapecode.ShapeID;
                //result 
                string validations = string.Empty;
                string error = string.Empty;
                Dictionary<string, string> resultProductMarking = objInfo.ExecuteDetailingComponent(shapeId, string.Empty);

                if (resultProductMarking != null)
                {
                    validations = resultProductMarking["ValidationFailed"];
                    resultProductMarking.Remove("ValidationFailed");
                    if (validations == "False")
                    {
                        newProductMark.NoOfMainWire = Convert.ToInt32(Double.Parse(resultProductMarking["no_of_mw"]));
                        newProductMark.NoOfCrossWire = Convert.ToInt32(Double.Parse(resultProductMarking["no_of_cw"]));
                        newProductMark.InvoiceArea = Convert.ToDouble(Double.Parse(resultProductMarking["area_post"]));
                        newProductMark.EnvelopeLength = Convert.ToDouble(Double.Parse(resultProductMarking["envelope_length"]));
                        newProductMark.EnvelopeWidth = Convert.ToDouble(resultProductMarking["envelope_width"]);

                        newProductMark.EnvelopeHeight = Convert.ToDouble(Double.Parse(resultProductMarking["envelope_height"]));
                        newProductMark.TheoraticalWeight = Convert.ToDouble(Double.Parse(resultProductMarking["theoretical_tonnage"]));
                        newProductMark.NetWeight = Convert.ToDouble(resultProductMarking["net_tonnage"]);
                        newProductMark.ProductionMWLength = Convert.ToInt32(Math.Round(Convert.ToDouble(Double.Parse(resultProductMarking["production_mw_length"]))));
                        newProductMark.ProductionCWLength = Convert.ToInt32(Math.Round(Convert.ToDouble(Double.Parse(resultProductMarking["production_cw_length"]))));

                        newProductMark.ProductionWeight = Convert.ToDouble(Double.Parse(resultProductMarking["actual_tonnage_post"]));
                        newProductMark.ProductionMO1 = Convert.ToInt32(Double.Parse(resultProductMarking["production_mo1"]));
                        newProductMark.ProductionMO2 = Convert.ToInt32(Double.Parse(resultProductMarking["production_mo2"]));
                        newProductMark.ProductionCO1 = Convert.ToInt32(Double.Parse(resultProductMarking["production_co1"]));
                        newProductMark.ProductionCO2 = Convert.ToInt32(Double.Parse(resultProductMarking["production_co2"]));

                        newProductMark.ParamValues = Convert.ToString(resultProductMarking["string_post"]);
                        newProductMark.MWBVBSString = Convert.ToString(resultProductMarking["mw_bvbs_string"]);
                        newProductMark.CWBVBSString = Convert.ToString(resultProductMarking["cw_bvbs_string"]);
                        newProductMark.InvoiceMWWeight = (structureMark.ProductCode.WeightPerMeterRun * newProductMark.NoOfMainWire * newProductMark.MWLength) / 1000;
                        newProductMark.InvoiceCWWeight = (structureMark.ProductCode.CwWeightPerMeterRun * newProductMark.NoOfCrossWire * newProductMark.CWLength) / 1000;
                        newProductMark.ProductionMWWeight = (structureMark.ProductCode.WeightPerMeterRun * newProductMark.NoOfMainWire * newProductMark.ProductionMWLength) / 1000;
                        newProductMark.ProductionCWWeight = (structureMark.ProductCode.CwWeightPerMeterRun * newProductMark.NoOfCrossWire * newProductMark.ProductionCWLength) / 1000;

                        newProductMark.ProductionMWQty = newProductMark.NoOfMainWire;
                        newProductMark.ProductionCWQty = newProductMark.NoOfCrossWire;

                        //Rounding production length
                        if (newProductMark.shapecode.CreepDeductAtMO1)
                        {
                            newProductMark.ProductionMO2 = newProductMark.MO2;
                            newProductMark.ProductionMWLength = RoundUpTo10Setp(newProductMark.ProductionMWLength);
                            newProductMark.ProductionMO1 = newProductMark.ProductionMWLength - (newProductMark.NoOfCrossWire - 1) * newProductMark.CWSpacing - newProductMark.ProductionMO2;
                        }
                        else
                        {
                            newProductMark.ProductionMO1 = newProductMark.MO1;
                            newProductMark.ProductionMWLength = RoundUpTo10Setp(newProductMark.ProductionMWLength);
                            newProductMark.ProductionMO2 = newProductMark.ProductionMWLength - (newProductMark.NoOfCrossWire - 1) * newProductMark.CWSpacing - newProductMark.ProductionMO1;
                        }

                        if (newProductMark.shapecode.CreepDeductAtCO1)
                        {
                            newProductMark.ProductionCO2 = newProductMark.CO2;
                            newProductMark.ProductionCWLength = RoundUpTo10Setp(newProductMark.ProductionCWLength);
                            newProductMark.ProductionCO1 = newProductMark.ProductionCWLength - (newProductMark.NoOfMainWire - 1) * newProductMark.MWSpacing - newProductMark.ProductionCO2;
                        }
                        else
                        {
                            newProductMark.ProductionCO1 = newProductMark.CO1;
                            newProductMark.ProductionCWLength = RoundUpTo10Setp(newProductMark.ProductionCWLength);
                            newProductMark.ProductionCO2 = newProductMark.ProductionCWLength - (newProductMark.NoOfMainWire - 1) * newProductMark.MWSpacing - newProductMark.ProductionCO1;
                        }

                        /*
                         * Commented By :   Premkumar
                         * Reason       :   Machine check value from the tacton can be skip. Bending adjustment calculation is doning seperately.
                         * Date         :   11 - Oct - 2013
                         * */
                        if (newProductMark.ProductionMO1 < structureMark.ParameterSet.MinMo1 || newProductMark.ProductionMO2 < structureMark.ParameterSet.MinMo2 || newProductMark.ProductionCO1 < structureMark.ParameterSet.MinCo1 || newProductMark.ProductionCO2 < structureMark.ParameterSet.MinCo2)
                        {
                            machineCheck = false;
                            return newProductMark;
                        }
                        else
                        {
                            machineCheck = true;
                        }
                        transportCheck = true;
                        //if (newProductMark.EnvelopeHeight > structureMark.ParameterSet.TransportMaxHeight || newProductMark.EnvelopeLength > structureMark.ParameterSet.TransportMaxLength || newProductMark.EnvelopeWidth > structureMark.ParameterSet.TransportMaxWidth)
                        //{
                        //    transportCheck = false;
                        //    return newProductMark;
                        //}

                        //else
                        //{
                        //    transportCheck = true;
                        //}

                        newProductMark.MWPitch = "";
                        newProductMark.CWPitch = "";
                        newProductMark.MWFlag = 0;
                        newProductMark.CWFlag = 0;

                        /*
                        machineCheck = true;
                        transportCheck = true;

                

                
                         * Commented By :   Premkumar
                         * Reason       :   Bending check validation is not required. Since we will do the bending adjustment for the failed cases.
                         * Date         :   11 - Jun - 2013
                         * 
                
             
                        ShapeParameterCollection segmentList = (ShapeParameterCollection)newProductMark.shapecode.ShapeParam;

                        if (productMark.shapecode.MOCO.Trim() == "M")
                        {
                            List<ShapeParameter> shapeParamColl = (from ShapeParameter sp in segmentList
                                                                   where sp.AngleType == "S" && sp.WireType == "M"
                                                                   select sp).ToList();

                            foreach (ShapeParameter spItem in shapeParamColl)
                            {
                                sParamList.Add(spItem.ParameterName + ";" + spItem.ParameterValue + ";" + "N");
                            }
                            string[] paramList = sParamList.ToArray(typeof(string)) as string[];
                            CheckBendingFeasibility(newProductMark.NoOfCrossWire, newProductMark.MO1, newProductMark.MO2, newProductMark.CWSpacing, paramList, out bendingCheck);
                            if (bendingCheck != "")
                            {
                                bendingCheck = "Bending check failed with main wire. Please click OK to continue saving the values or click No to change the values.";
                            }
                        }
                        else if (productMark.shapecode.MOCO.Trim() == "C")
                        {
                            List<ShapeParameter> shapeParamColl = (from ShapeParameter sp in segmentList
                                                                   where sp.AngleType == "S" && sp.WireType == "C"
                                                                   select sp).ToList();

                            foreach (ShapeParameter spItem in shapeParamColl)
                            {
                                sParamList.Add(spItem.ParameterName + ";" + spItem.ParameterValue + ";" + "N");
                            }
                            string[] paramList = sParamList.ToArray(typeof(string)) as string[];
                            CheckBendingFeasibility(newProductMark.NoOfMainWire, newProductMark.CO1, newProductMark.CO2, newProductMark.MWSpacing, paramList, out bendingCheck);
                            if (bendingCheck != "")
                            {
                                bendingCheck = "Bending check failed with cross wire. Please click OK to continue saving the values or click No to change the values.";
                            }
                        }
                        else if (productMark.shapecode.MOCO.Trim() == "B")
                        {
                            List<ShapeParameter> shapeParamColl = (from ShapeParameter sp in segmentList
                                                                   where sp.AngleType == "S"
                                                                   select sp).ToList();

                            foreach (ShapeParameter spItem in shapeParamColl)
                            {
                                sParamList.Add(spItem.ParameterName + ";" + spItem.ParameterValue + ";" + "N");
                            }
                            string[] paramList = sParamList.ToArray(typeof(string)) as string[];
                            CheckBendingFeasibility(newProductMark.NoOfCrossWire, newProductMark.MO1, newProductMark.MO2, newProductMark.CWSpacing, paramList, out bendingCheck);

                            if (bendingCheck == "")
                            {
                                CheckBendingFeasibility(newProductMark.NoOfMainWire, newProductMark.CO1, newProductMark.CO2, newProductMark.MWSpacing, paramList, out bendingCheck);
                                if (bendingCheck != "")
                                {
                                    bendingCheck = "Bending check failed with cross wire. Please click OK to continue saving the values or click No to change the values.";
                                }
                            }
                            else
                            {
                                bendingCheck = "Bending check failed with main wire. Please click OK to continue saving the values or click No to change the values.";
                            }
                        }
                         */
                    }
                    else
                    {
                        foreach (var item in resultProductMarking)
                        {
                            if (item.Key != "ValidationFailed")
                            {
                                message = item.Value;
                                errormessage = item.Value;
                                throw new ArgumentOutOfRangeException(errormessage);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Product marking result is NULL");
                }
            }
            catch (Exception ex)
            {
                if (errormessage != "")
                {
                    message = errormessage;
                }
                else
                {
                    message = "Error Generating Products; Invalid Inputs: " + ex.Message + "Trace: " + ex.StackTrace;
                }

            }
            return newProductMark;
        }

        #endregion

        #region "Helper funtions"

        public List<listAngle> GetAngle(string bvbsstring, ShapeParameterCollection segmentList, out string errorMessage)
        {
            List<listAngle> listAngle = new List<listAngle>();
            errorMessage = "";
            try
            {
                if (bvbsstring.Contains("lA@"))
                {
                    int space1 = bvbsstring.IndexOf("lA@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 3);


                    int minus = space2 - (space1 + 3);
                    string firstPart = bvbsstring.Substring((space1 + 3), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "A", Angle = firstPart });

                }
                if (bvbsstring.Contains("@lB@"))
                {
                    int space1 = bvbsstring.IndexOf("@lB@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "B", Angle = firstPart });
                }
                if (bvbsstring.Contains("@lC@"))
                {
                    int space1 = bvbsstring.IndexOf("@lC@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "C", Angle = firstPart });
                }
                if (bvbsstring.Contains("@lD@"))
                {
                    int space1 = bvbsstring.IndexOf("@lD@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "D", Angle = firstPart });
                }
                if (bvbsstring.Contains("@lE@"))
                {
                    int space1 = bvbsstring.IndexOf("@lE@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "E", Angle = firstPart });
                }
                if (bvbsstring.Contains("@lF@"))
                {
                    int space1 = bvbsstring.IndexOf("@lF@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "F", Angle = firstPart });
                }
                if (bvbsstring.Contains("@lG@"))
                {
                    int space1 = bvbsstring.IndexOf("@lG@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "G", Angle = firstPart });
                }
                if (bvbsstring.Contains("@lH@"))
                {
                    int space1 = bvbsstring.IndexOf("@lH@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "H", Angle = firstPart });
                }

                foreach (ShapeParameter sp in segmentList)
                {
                    if (sp.AngleType == "V")
                    {
                        foreach (listAngle la in listAngle)
                        {
                            if (sp.ParameterName.Trim().ToUpper() == la.Angle.Trim().ToUpper())
                            {
                                la.Angle = Convert.ToString(sp.ParameterValue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return listAngle;
        }

        public bool CheckBendingFeasibility(int noOfWires, int oh1, int oh2, int spacing, string[] segments, out string errorMessage)
        {
            bool isBendable = false;
            errorMessage = "";
            double wireLength = 0.0;
            try
            {
                wireLength = oh1 + ((noOfWires - 1) * spacing) + oh2;
                for (int weldPoint = oh1; weldPoint <= wireLength - oh2; weldPoint += spacing)
                {
                    string segCode = "";
                    int segLength = 0;
                    string segType = "";
                    for (int j = 0; j < segments.Length; j++)
                    {
                        string currentSegment = segments[j];
                        string[] bendParams = currentSegment.Split(';');
                        if (bendParams.Length == 3)
                        {
                            segCode = Convert.ToString(bendParams[0]);
                            segLength += Convert.ToInt32(bendParams[1]);
                            segType = bendParams[2].ToString().Trim();
                        }
                        else
                        {
                            throw new Exception("Invalid Segment Parameters - " + currentSegment);
                        }

                        if (segType == "N")
                        {
                            //Bending Failed
                            if (Math.Abs(weldPoint - segLength) < 10)//normalBendLimit)
                            {
                                throw new Exception("Normal Bending Failed");
                            }
                        }
                        else if (segType == "R")
                        {
                            //Bending Failed
                            if (Math.Abs(weldPoint - segLength) < 5)//reverseBendLimit)
                            {
                                throw new Exception("Reverse Bending Failed");
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid Segment Type - " + segType);
                        }
                    }
                }
                isBendable = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return isBendable;
        }

        private Result TactonCommit(string paramName, string paramValue)
        {
            Result tactonResult = null;
            string tcxFilename = "";

            try
            {
                tactonResult = configuration.Commit(paramName, paramValue);
                if (!tactonResult.Response.IsOk)
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(tactonResult.State)))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Text && reader.Value.EndsWith("tcx"))
                            {
                                tcxFilename = reader.Value;
                                break;
                            }
                        }
                    }
                    if (tactonResult.Response.IsResolvable)
                    {
                        throw new Exception("Tacton Commit Alert: " + paramName + " : " + paramValue + " in " + tcxFilename);
                    }
                    else
                    {
                        throw new Exception("Tacton Commit Error: " + paramName + " : " + paramValue + " in " + tcxFilename);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tactonResult;
        }

        private int RoundUpBy5Setp(int input)
        {
            if (input % 5 == 0)
                return input;
            else
                return input + (5 - (input % 5));
        }

        private int RoundUpTo10Setp(int input)
        {
            if (input % 10 == 0)
                return input;
            else
                return input + (10 - (input % 10));
        }

        #endregion

        #endregion

    }



    class FileAccessor : DataAccessor
    {
        public ModelFile GetModel(string modelkey)
        {
            return new DiskModelFile(modelkey);
        }

        public AttributeSet GetData(string datakey)
        {
            return null;
        }
    }

    public class listAngle
    {
        public string paramName;
        public string Angle;
    }


}
