using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using DetailingService.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DetailingService.Repositories
{
    public class CABService : ICABService
    {
        List<CABItem> cabProductMarkingDetails = new List<CABItem>();
        public List<CABItem> GetCABProductMarkingDetailsBySEDetailingID(int intSEDetailingID, ref string errorMessage)
        {
            try
            {
                CABItem objCABProductMarkingDetails = new CABItem();
                cabProductMarkingDetails = objCABProductMarkingDetails.GetCABProductMarkingDetailsBySEDetailingID(intSEDetailingID);
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
            }
            return cabProductMarkingDetails;
        }



        #region Cube Implementation



        /// <summary>
        /// Method to get the sedetailingid from groupmarkid for BPC.
        /// </summary>
        /// <param name="groupMarkId"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int GetSeDetailingId(int groupMarkId, out string errorMessage)
        {
            CABItem objCab = new CABItem();
            errorMessage = "";
            int seDetailingId = 0;
            try
            {
                seDetailingId = objCab.GetSeDetailingId(groupMarkId, out errorMessage);
                return seDetailingId;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return seDetailingId;
            }
            finally
            {
                objCab = null;
            }
        }



        /// <summary>
        /// Method to copy barmark on the basis of prodmarkid.
        /// </summary>
        /// <param name="cabPodMarkId"></param>
        /// <param name="nextBarMark"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int CopyBarMark(int cabPodMarkId, string nextBarMark, out string errorMessage)
        {
            CABItem objCab = new CABItem();
            int intCABProductMarkID = 0;
            errorMessage = "";
            try
            {
                intCABProductMarkID = objCab.CopyBarMark(cabPodMarkId, nextBarMark, out errorMessage);
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
            }
            finally
            {
                objCab = null;
            }
            return intCABProductMarkID;
        }



        /// <summary>
        /// Mehod to insert new product marking.
        /// </summary>
        /// <param name="objCab"></param>
        /// <param name="SEDetailingID"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int InsertProductMark(CABItem objCabDto, int SEDetailingID, out string errorMessage)
        {
            int cabProductmarkId = 0;
            errorMessage = "";

            //CABItem objCab=new CABItem();

            try
            {
                cabProductmarkId = objCabDto.InsertUpdCABAccessories(out errorMessage);
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
            }
            finally
            {
                objCabDto = null;
            }
            return cabProductmarkId;
        }



        /// <summary>
        /// Method to insert fresh bar mark inbetween sequence
        /// </summary>
        /// <param name="objCab"></param>
        /// <param name="SEDetailingID"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<CABItem> InsertProductMarkInLine(InsertProductMarkInLineDto insertProduct, int SEDetailingID, string barMarkStart, out string errorMessage)
        {
            CABItem objCab = new CABItem();
           
            List<CABItem> cabProductMark = new List<CABItem>();
            errorMessage = "";
              
            try
            {
                //Insret the fresh bar mark.
                int cabProductmarkId = objCab.InsertUpdCABAccessories(out errorMessage);
                //Call method to handle the rest bar marks.
                bool flag = objCab.InsertProductMarkInLine(barMarkStart, objCab.CABProductMarkName.Trim(), objCab.GroupMarkId, SEDetailingID, out errorMessage);
                //get the data to bind the gridview
                cabProductMark = GetCABProductMarkingDetailsBySEDetailingID(SEDetailingID, ref errorMessage);
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
            }
            finally
            {
                //objCab = null;
            }
            return cabProductMark;
        }



        /// <summary>
        /// Method to get the pin.
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="dia"></param>
        /// <param name="groupMarkId"></param>
        /// <param name="productTypeId"></param>
        /// <param name="errorMsg"></param>
        /// <param name="pin"></param>
        /// <param name="minLength"></param>
        public void GetPin(string grade, int dia, int groupMarkId, int productTypeId, out string errorMsg, out int pin, out int minLength, out int minHookLen, out int minHookHt)
        {
            CABItem obj = new CABItem();
            try
            {
                errorMsg = "";
                pin = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;
                obj.GetPin(grade, dia, groupMarkId, productTypeId, out errorMsg, out pin, out minLength, out minHookLen, out minHookHt);
            }
            catch (Exception ex)
            {
                
                errorMsg = ex.Message;
                pin = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;
            }
            finally
            {
                obj = null;
            }
        }

        /// <summary>
        /// Method to get the pin.
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="dia"></param>
        /// <param name="formerFlag"></param>
        /// <param name="errorMsg"></param>
        /// <param name="pin"></param>
        /// <param name="minLength"></param>
        /// <param name="minHookLen"></param>
        /// <param name="minHookHt"></param>
        public void GetPinByFormerFlag(string grade, int dia, int formerFlag, out string errorMsg, out int pin, out int minLength, out int minHookLen, out int minHookHt)
        {
            CABItem obj = new CABItem();
            try
            {
                errorMsg = "";
                pin = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;
                obj.GetPin(grade, dia, formerFlag, out errorMsg, out pin, out minLength, out minHookLen, out minHookHt);
            }
            catch (Exception ex)
            {
                
                errorMsg = ex.Message;
                pin = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;
            }
            finally
            {
                obj = null;
            }
        }



        /// <summary>
        /// Method to validate the pin and get the min length, height and hook
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="dia"></param>
        /// <param name="pin"></param>
        /// <param name="errorMsg"></param>
        /// <param name="minLength"></param>
        /// <param name="minHookLen"></param>
        /// <param name="minHookHt"></param>
        /// <returns></returns>
        public bool ValidatePin(string grade, int dia, int pin, out string errorMsg, out int minLength, out int minHookLen, out int minHookHt)
        {
            CABItem obj = new CABItem();
            bool flag = false;
            try
            {
                errorMsg = "";
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;
                flag = obj.GetPin(grade, dia, pin, out errorMsg, out minLength, out minHookLen, out minHookHt);
            }
            catch (Exception ex)
            {
                
                errorMsg = ex.Message;
                pin = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;
            }
            finally
            {
                obj = null;
            }
            return flag;
        }


        /// <summary>
        /// Method to get prod and invoice length.
        /// </summary>
        /// <param name="objCab"></param>
        /// <param name="errorMsg"></param>
        /// <param name="invoiceLength"></param>
        /// <param name="prodLength"></param>
        /// <param name="r_intTotBend"></param>
        /// <param name="r_intTotArc"></param>
        /// <returns></returns>
        public List<Accessory> GetProdInvLength(CABItem objCab, out string errorMsg, out int invoiceLength, out int prodLength, out int r_intTotBend, out int r_intTotArc, out string bvbs)
        {
            try
            {
                errorMsg = "";
                invoiceLength = 0;
                prodLength = 0;
                r_intTotBend = 0;
                r_intTotArc = 0;
                bvbs = "";
                List<Accessory> list = new List<Accessory>();
                list = objCab.GetProdInvLength(out errorMsg, out invoiceLength, out prodLength, out r_intTotBend, out r_intTotArc, out bvbs);
                return list;
            }
            catch (Exception ex)
            {
                
                errorMsg = ex.Message;
                invoiceLength = 0;
                prodLength = 0;
                r_intTotBend = 0;
                r_intTotArc = 0;
                bvbs = "";
                return null;
            }
        }



        /// <summary>
        /// Method to get the produce indecator.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<CABItem> PopulateProduceInd(out string errorMessage)
        {
            List<CABItem> listProduceIndInfo = new List<CABItem>();
            errorMessage = "";
            try
            {
                CABItem obj = new CABItem
                {
                    ProduceInd = "Yes"
                };
                listProduceIndInfo.Add(obj);
                CABItem obj1 = new CABItem
                {
                    ProduceInd = "No"
                };
                listProduceIndInfo.Add(obj1);
                return listProduceIndInfo;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                listProduceIndInfo = null;
            }
        }



        /// <summary>
        /// Method to get the filtered produce ind.
        /// </summary>
        /// <param name="enteredText"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<CABItem> FilterProduceInd(string enteredText, out string errorMessage)
        {
            List<CABItem> listProduceIndInfo = new List<CABItem>();
            errorMessage = "";
            try
            {
                if (enteredText.Contains("Y"))
                {
                    CABItem obj = new CABItem
                    {
                        ProduceInd = "YES"
                    };
                    listProduceIndInfo.Add(obj);
                }
                else
                {
                    CABItem obj = new CABItem
                    {
                        ProduceInd = "NO"
                    };
                    listProduceIndInfo.Add(obj);
                }
                return listProduceIndInfo;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                listProduceIndInfo = null;
            }
        }



        /// <summary>
        /// Method to get the filtered shape code.
        /// </summary>
        /// <param name="enteredText"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<ShapeCode> FilterShapeCode(string enteredText, out string errorMessage)
        {
            List<ShapeCode> listShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            errorMessage = "";
            try
            {
                listShapeCode = objShapeCode.FilterShapeCodeForCab_Get(enteredText);
                return listShapeCode;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                listShapeCode = null;
                objShapeCode = null;
            }
        }



        //START:ADDED BY SIDDHI FOR CAB MODULE ENHANCEMENT CR
        public List<ShapeCode> AllCABShapeCodes()
        {
            List<ShapeCode> listShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();



            try
            {
                listShapeCode = objShapeCode.AllShapeCodeForCab_Get();
                return listShapeCode;
            }
            catch (Exception ex)
            {
                



                return null;
            }
            finally
            {
                listShapeCode = null;
                objShapeCode = null;
            }
        }
        //END





        /// <summary>
        /// Method to get the filtered shape code during edit mode.
        /// </summary>
        /// <param name="enteredText"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<ShapeCode> FilterShapeCodeEdit(string enteredText, int cabProdmarkId, out string errorMessage)
        {
            List<ShapeCode> listShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            errorMessage = "";
            try
            {
                listShapeCode = objShapeCode.FilterShapeCodeForCabEdit_Get(enteredText, cabProdmarkId);
                return listShapeCode;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                listShapeCode = null;
                objShapeCode = null;
            }
        }



        /// <summary>
        /// Method to get shape code and shape parameters during variable length calcullation.
        /// </summary>
        /// <param name="enteredText"></param>
        /// <param name="cabProdmarkId"></param>
        /// <param name="seDetailingId"></param>
        /// <param name="barMark"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<ShapeCode> GetShapeCodeAndParam(string enteredText, int cabProdmarkId, int seDetailingId, string barMark, out string errorMessage)
        {
            List<ShapeCode> listShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            errorMessage = "";
            try
            {
                listShapeCode = objShapeCode.GetShapeCodeAndParam(enteredText, cabProdmarkId, seDetailingId, barMark);
                return listShapeCode;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                listShapeCode = null;
                objShapeCode = null;
            }
        }



        /// <summary>
        /// Method to return shape parameters when in edit mode.
        /// </summary>
        /// <param name="shapeCode"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<ShapeCode> FilterShapeParam_Edit_Get(string shapeCode, out string errorMessage)
        {
            List<ShapeCode> listShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            errorMessage = "";
            try
            {
                listShapeCode = objShapeCode.FilterShapeCodeCabEdit_Get(shapeCode);
                return listShapeCode;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                listShapeCode = null;
                objShapeCode = null;
            }
        }



        /// <summary>
        /// Web method to get the shape code image.
        /// </summary>
        /// <param name="shapeCode"></param>
        /// <returns></returns>
        public byte[] GetImage(string shapeCode)
        {
            byte[] Ret = null;
            try
            {
                CABItem objProductCode = new CABItem();
               //Ret = objProductCode.GetImage(shapeCode);   //by vidya
            }
            catch (Exception ex)
            {
                
            }
            return Ret;
        }



        /// <summary>
        /// Method to handle cab detailing delete.
        /// </summary>
        /// <param name="objCabMarkingDetail"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool DeleteCabMarkingDetail(CABItem objCabMarkingDetail, out string errorMsg)
        {
            bool isSuccess = false;
            errorMsg = "";
            try
            {
                isSuccess = objCabMarkingDetail.DeleteCabStructureMark(out errorMsg);
            }
            catch (Exception ex)
            {
                
                errorMsg = ex.Message;
            }
            return isSuccess;
        }



        /// <summary>
        /// Method to handle cab detailing delete.
        /// </summary>
        /// <param name="deleteList"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool DeletePoductMark(List<CABItem> deleteList, out string errorMsg)
        {
            bool isSuccess = false;
            errorMsg = "";
            try
            {
                foreach (CABItem objCab in deleteList)
                {
                    isSuccess = objCab.DeleteCabStructureMark(out errorMsg);
                }
            }
            catch (Exception ex)
            {
                
                errorMsg = ex.Message;
            }
            return isSuccess;
        }



        /// <summary>
        /// Method to save data on datagrid update.
        /// </summary>
        /// <param name="objCabproductMarking"></param>
        /// <param name="errorMessage"></param>
        /// <param name="SEDetailingID"></param>
        /// <param name="minLen"></param>
        /// <param name="minHookLen"></param>
        /// <param name="minHookHt"></param>
        /// <returns></returns>
        public List<CABItem> UpdateCABProductMarking(CABItem objCabproductMarking, out string errorMessage, int SEDetailingID, out int minLen, out int minHookLen, out int minHookHt, out bool flag)
        {
            List<CABItem> cabStructureMarking = new List<CABItem>();
            errorMessage = "";
            minLen = 0;
            minHookLen = 0;
            minHookHt = 0;
            flag = false;
            try
            {
                objCabproductMarking.SEDetailingID = SEDetailingID;
                objCabproductMarking.intSEDetailingId = SEDetailingID;


                bool status = objCabproductMarking.Update(out errorMessage, out minLen, out minHookLen, out minHookHt);
                flag = status;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                minLen = 0;
                minHookLen = 0;
                minHookHt = 0;
                
            }
            finally
            {
                cabStructureMarking = GetCABProductMarkingDetailsBySEDetailingID(SEDetailingID, ref errorMessage);
            }

            return cabStructureMarking;
        }



        /// <summary>
        /// Method to save data on datagrid update during variable length quantity change.
        /// </summary>
        /// <param name="objCabproductMarking"></param>
        /// <param name="errorMessage"></param>
        /// <param name="userId"></param>
        /// <param name="SEDetailingID"></param>
        /// <returns></returns>
        public List<CABItem> UpdateCABProductMarkingVar(List<CABItem> list, out string errorMessage, int userId, int SEDetailingID, int qty)
        {
            List<CABItem> cabStructureMarking = new List<CABItem>();
            errorMessage = "";
            try
            {
                //Loop here to update the values.
                foreach (CABItem obj in list)
                {
                    obj.SEDetailingID = SEDetailingID;
                    obj.intSEDetailingId = SEDetailingID;
                    bool status = obj.UpdateVar(userId, out errorMessage, qty);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                
            }
            finally
            {
                cabStructureMarking = GetCABProductMarkingDetailsBySEDetailingID(SEDetailingID, ref errorMessage);
            }
            return cabStructureMarking;
        }



        /// <summary>
        /// Method to insert product marking, shape parameter and accesory.
        /// </summary>
        /// <param name="objcab"></param>
        /// <param name="sourceList"></param>
        /// <param name="targetList"></param>
        /// <param name="cabProdMarkId"></param>
        /// <param name="count"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool InsertVariableLength(CABItem objcab, List<ShapeParameter> sourceList, List<ShapeParameter> targetList, int cabProdMarkId, int count, out string errorMessage)
        {
            try
            {
                errorMessage = "";
                CABItem cab = new CABItem();
                bool flag = cab.InsertVariableLength(objcab, sourceList, targetList, cabProdMarkId, count, out errorMessage);
                return flag;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                
                return false;
            }
        }



        /// <summary>
        /// Method to get the height angle.
        /// </summary>
        /// <param name="shapeparam"></param>
        /// <param name="param"></param>
        /// <param name="shape"></param>
        /// <returns></returns>
        public int SetHeightAngle(List<ShapeParameter> shapeparam, string param, string shape)
        {
            try
            {
                int val = 0;
                string[,] arr = new string[shapeparam.Count, 2];
                int j = 0;
                for (int i = 0; i <= shapeparam.Count - 1; i++)
                {
                    if (shapeparam[i].ParameterName != "" && shapeparam[i].ParameterName != string.Empty)
                    {
                        arr[j, 0] = shapeparam[i].ParameterName.ToString();
                        arr[j, 1] = shapeparam[i].ParameterValueCab.ToString();
                        j++;
                    }
                }
                CABCalculation cabcalc = new CABCalculation();
                val = cabcalc.SetHeightAngle(arr, param, shape);
                cabcalc = null;
                return val;
            }
            catch (Exception ex)
            {
                
                return 0;
            }
        }



        /// <summary>
        /// Method to save trans detailing from parallel to old table.
        /// </summary>
        /// <param name="groupMarkId"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool ValidateAndSave(int groupMarkId, out string errorMsg)
        {
            bool success = false;
            errorMsg = "";
            try
            {
                CABItem objCab = new CABItem();
                success = objCab.ValidateAndSave(groupMarkId, out errorMsg);
                objCab = null;
                return success;
            }
            catch (Exception ex)
            {
                
                errorMsg = ex.Message;
                return false;
            }
        }



        #endregion


        #region OES & TCT
        /// <summary>
        /// Method for TCT and OES to calculate and save to database.
        /// </summary>
        /// <param name="lstCab"></param>
        /// <returns></returns>
        public bool InsertToDbTCT(CABItem lstCab,bool isValidated, out int  ProductMarkId)
            {
            //CABItem cABItem = new CABItem();
            bool isSuccuess = false;
            string errorMsg = string.Empty;
            ProductMarkId = 0;
            try
            {
                //foreach (CABItem objCab in lstCab)
                //{

                //    isSuccuess = objCab.InsertToDbTCT(out errorMsg);
                //}
                //var options = new JsonSerializerOptions()
                //{
                //    NumberHandling = JsonNumberHandling.AllowReadingFromString |
                //                JsonNumberHandling.WriteAsString
                //};

                //string denemeJson = JsonSerializer.Serialize<JsonElement>((JsonElement)lstCab.Shape.ShapeParam, options);
                //List<ShapeParameter> shapeParamObj = JsonSerializer.Deserialize<List<ShapeParameter>>(denemeJson, options);

                //lstCab.ShapeParametersList = lstCab.Shape.ShapeParam;

                //lstCab.ShapeParametersList = new List<ShapeParameter>();

                //foreach (var shapeParam in lstCab.Shape.ShapeParam)
                //{
                //    var options = new JsonSerializerOptions()
                //    {
                //        NumberHandling = JsonNumberHandling.AllowReadingFromString |
                //                 JsonNumberHandling.WriteAsString
                //    };
                //    string denemeJson = JsonSerializer.Serialize<JsonElement>((JsonElement)shapeParam, options);



                //    // deserialize

                //    ShapeParameter shapeParamObj = JsonSerializer.Deserialize<ShapeParameter>(denemeJson, options);



                //    lstCab.ShapeParametersList.Add(shapeParamObj);
                //}//Commented by ajit for cage CAB data entry

                if (!isValidated)
                {
                    lstCab.ValidateShapeParameters(out errorMsg);
                }

                isSuccuess = lstCab.InsertToDbTCT(out errorMsg, out ProductMarkId);

            }
            catch (Exception ex)
            {
                //return false
                throw ex;
            }
            return isSuccuess;
        }





        /// <summary>
        /// Method for TCT and OES to calculate and save to database.
        /// </summary>
        /// <param name="lstCab"></param>
        /// <param name="isValidated"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool InsertToDbTCT(CABItem lstCab, bool isValidated, out string errorMsg)
        {
            try
            {
                errorMsg = "";
                bool flag = false;

                int ProductMarkId = 0;

                //CABItem objCab=new CABItem();

                //foreach (CABItem objCab in lstCab)
                //{
                    if (!isValidated)
                    {
                    lstCab.ValidateShapeParameters(out errorMsg);
                    }
                    flag = lstCab.InsertToDbTCT(out errorMsg,out ProductMarkId);
                //}
                return flag;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
        #endregion

        //Method to fetch Grade and diameter value from database:Anamika
        //public int FetchGradeDia(char gradetype, int dia)
        //{
        //    int count = 0;

        //    CABItem objCABProductMarkingDetails = new CABItem();
        //    count = objCABProductMarkingDetails.FetchGradeDia(gradetype, dia);

        //    return count;
        //}

        //#region Generic Methods
        ///// <summary>
        ///// Method to check if group mark is released or posted
        ///// </summary>
        ///// <param name="groupMarkId"></param>
        ///// <param name="errorMessage"></param>
        ///// <returns></returns>
        //public bool GetReleasedPostedGroupMark(int groupMarkId, out string errorMessage)
        //{
        //    CABItem objCab = new CABItem();
        //    errorMessage = "";
        //    DataSet dsRelease = new DataSet();
        //    DataSet dsPost = new DataSet();
        //    bool flag = false;
        //    try
        //    {
        //        dsRelease = objCab.GetReleasedGroupMark(groupMarkId, out errorMessage);
        //        dsPost = objCab.GetPostedGroupMark(groupMarkId, out errorMessage);
        //        if (dsRelease.Tables.Count > 0 && dsRelease.Tables[0].Rows.Count > 0)
        //        {
        //            flag = true;
        //        }
        //        else if (dsPost.Tables.Count > 0 && dsPost.Tables[0].Rows.Count > 0)
        //        {
        //            flag = true;
        //        }
        //        else
        //        {
        //            flag = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.LogException(ex, UserName);
        //        errorMessage = ex.Message;
        //        flag = false;
        //    }
        //    finally
        //    {
        //        objCab = null;
        //        dsRelease.Dispose();
        //        dsPost.Dispose();
        //    }
        //    return flag;
        //}
        //#endregion

    }
}