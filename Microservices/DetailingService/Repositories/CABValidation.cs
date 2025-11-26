using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DetailingService.Repositories
{
	public class CABValidation
	{

		#region Class Properties
		public int intSEDetailingId { get; set; }
		public string ProduceInd { get; set; }
		public string Prefix { get; set; }
		public string PageNo { get; set; }
		public string BarMarkId { get; set; }
		public string ShapeId { get; set; }
		public string ShapeCode { get; set; }
		public string Grade { get; set; }
		public string Dia { get; set; }
		public string Quantity { get; set; }
		public string UnitQuantity { get; set; }
		public string Pin { get; set; }
		public string InvoiceLength { get; set; }
		public string ProdnLength { get; set; }
		public string InvoiceWeight { get; set; }
		public string ProdnWeight { get; set; }
		public string OperationType { get; set; }
		public string ShapeCategory { get; set; }
		public List<ShapeParameter> ShapeParametersList { get; set; }


        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";

        public CABValidation()
		{

		}
		#endregion

		#region Detailing entry validations before saving to db
		/// <summary>
		/// Method to validate the input fields by the user.
		/// </summary>
		/// <param name="errMsg"></param>
		/// <returns></returns>
		public bool DetailingInputDataValidation(out string errMsg)
		{
			try
			{
				#region Null field and default field checks.
				if (this.ProduceInd == "" || this.ProduceInd == string.Empty || this.ProduceInd.Trim() == null)
				{
					errMsg = "Please enter Produce Indicator.";
					return false;
				}
				if (this.PageNo == "" || this.PageNo == string.Empty || this.PageNo.Trim() == null)
				{
					errMsg = "Please enter Page Number.";
					return false;
				}
				if (this.BarMarkId == "" || this.BarMarkId == string.Empty || this.BarMarkId.Trim() == null)
				{
					errMsg = "Please enter Bar Mark Id.";
					return false;
				}
				if (this.ShapeId == "" || this.ShapeId == string.Empty || this.ShapeId.Trim() == null)
				{
					errMsg = "Please enter Shape Id.";
					return false;
				}
				if (this.Grade == "" || this.Grade == string.Empty || this.Grade.Trim() == null)
				{
					errMsg = "Please enter Grade.";
					return false;
				}
				if (this.Dia == "" || this.Dia == string.Empty || this.Dia.Trim() == null)
				{
					errMsg = "Please enter Diameter.";
					return false;
				}
				if (this.Quantity == "" || this.Quantity == string.Empty || this.Quantity.Trim() == null)
				{
					errMsg = "Please enter Quantity.";
					return false;
				}
				if (this.UnitQuantity == "" || this.UnitQuantity == string.Empty || this.UnitQuantity.Trim() == null)
				{
					errMsg = "Please enter Unit Quantity.";
					return false;
				}
				if (this.Pin == "" || this.Pin == string.Empty || this.Pin.Trim() == null)
				{
					errMsg = "Please enter Pin.";
					return false;
				}
				if (this.InvoiceLength == "" || this.InvoiceLength == string.Empty || this.InvoiceLength.Trim() == null)
				{
					errMsg = "Please enter Invoice Length.";
					return false;
				}
				if (this.ProdnLength == "" || this.ProdnLength == string.Empty || this.ProdnLength.Trim() == null)
				{
					errMsg = "Please enter Production Length.";
					return false;
				}
				if (this.InvoiceWeight == "" || this.InvoiceWeight == string.Empty || this.InvoiceWeight.Trim() == null)
				{
					errMsg = "Please enter Invoice Weight.";
					return false;
				}
				if (this.ProdnWeight == "" || this.ProdnWeight == string.Empty || this.ProdnWeight.Trim() == null)
				{
					errMsg = "Please enter Production Weight.";
					return false;
				}
				#endregion

				#region Validation for quotes
				if (this.ProduceInd.Contains("'"))
				{
					errMsg = "Produce Ind contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.Prefix.Contains("'"))
				{
					errMsg = "Prefix contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.PageNo.Contains("'"))
				{
					errMsg = "Page Number contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.BarMarkId.Contains("'"))
				{
					errMsg = "Bar mark id contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.ShapeId.Contains("'"))
				{
					errMsg = "Shape id contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.Grade.Contains("'"))
				{
					errMsg = "Grade contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.Dia.Contains("'"))
				{
					errMsg = "Dia contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.Quantity.Contains("'"))
				{
					errMsg = "Quantity contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.UnitQuantity.Contains("'"))
				{
					errMsg = "Unit Quantity contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.Pin.Contains("'"))
				{
					errMsg = "Pin contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.InvoiceLength.Contains("'"))
				{
					errMsg = "Invoice Length contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.ProdnLength.Contains("'"))
				{
					errMsg = "Production Length contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.InvoiceWeight.Contains("'"))
				{
					errMsg = "Invoice Weight contains '(Single Quote) character. Please remove.";
					return false;
				}
				if (this.ProdnWeight.Contains("'"))
				{
					errMsg = "Production Weight '(Single Quote) character. Please remove.";
					return false;
				}
				#endregion

				#region Number validation
				int res;
				double res1;
				if (int.TryParse(this.Quantity, out res) == false)
				{
					errMsg = "Please enter Quantity.";
					return false;
				}
				else if (int.TryParse(this.Quantity, out res) == true)
				{
					if (Convert.ToInt32(this.Quantity) <= 0)
					{
						errMsg = "Quantity should be greater than 0.";
						return false;
					}
				}

				if (int.TryParse(this.UnitQuantity, out res) == false)
				{
					errMsg = "Please enter Unit Quantity.";
					return false;
				}
				else if (int.TryParse(this.UnitQuantity, out res) == true)
				{
					if (Convert.ToInt32(this.UnitQuantity) <= 0)
					{
						errMsg = "Quantity should be greater than 0.";
						return false;
					}
				}

				if (int.TryParse(this.InvoiceLength, out res) == false)
				{
					errMsg = "Please enter Invoice Length.";
					return false;
				}
				else if (int.TryParse(this.InvoiceLength, out res) == true)
				{
					if (Convert.ToInt32(this.InvoiceLength) <= 0)
					{
						errMsg = "Invoice Length should be greater than 0.";
						return false;
					}
				}

				if (int.TryParse(this.ProdnLength, out res) == false)
				{
					errMsg = "Please enter Production Length.";
					return false;
				}
				else if (int.TryParse(this.ProdnLength, out res) == true)
				{
					if (Convert.ToInt32(this.ProdnLength) <= 0)
					{
						errMsg = "Production Length should be greater than 0.";
						return false;
					}
				}

				if (double.TryParse(this.InvoiceWeight, out res1) == false)
				{
					errMsg = "Please enter Invoice Weight.";
					return false;
				}
				else if (double.TryParse(this.InvoiceWeight, out res1) == true)
				{
					if (Convert.ToDouble(this.InvoiceWeight) <= 0)
					{
						errMsg = "Invoice Weight should be greater than 0.";
						return false;
					}
				}

				if (double.TryParse(this.ProdnWeight, out res1) == false)
				{
					errMsg = "Please enter Production Weight.";
					return false;
				}
				else if (double.TryParse(this.ProdnWeight, out res1) == true)
				{
					if (Convert.ToDouble(this.ProdnWeight) <= 0)
					{
						errMsg = "Production Weight should be greater than 0.";
						return false;
					}
				}
				#endregion

				errMsg = "";
				return true;
			}
			catch (Exception ex)
			{
				errMsg = ex.Message.ToString();
				return false;
			}
		}

		/// <summary>
		/// Method to validate the bar mark, production and invoice lengths.
		/// </summary>
		/// <param name="errMsg"></param>
		/// <returns></returns>
		public bool DetailingShapeDataValidation(out string errMsg)
		{
			//DBManager dbManager = new DBManager();
			DataSet ds = new DataSet();
			StringBuilder sb = new StringBuilder();
			//dbManager.Open();
			try
			{
				if (this.OperationType.ToUpper() == "ADD")
				{
					DataTable dtBarMark = new DataTable();  
                    string query2= "select intSEDetailingId from cabproductmarkingdetails where intSEDetailingId = " + this.intSEDetailingId.ToString() + " and vchCABProductMarkName = '" + this.BarMarkId + "'";
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
					{
                        sqlConnection.Open();
                        using (SqlCommand sqlCommand = new SqlCommand(query2, sqlConnection))
						{
                            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
							{		
								sqlDataAdapter.Fill(ds);
							}
						}
					}
                    dtBarMark = ds.Tables[0];
					if (dtBarMark.Rows.Count > 0)
					{
						errMsg = "Bar Mark already exist.";
						return false;
					}
				}

				if (ShapeParameter(out errMsg) == false)
				{
					return false;
				}

				DataTable dtMat = new DataTable();
				ds = new DataSet();
				sb = new StringBuilder();
				//sb = sb.Append("SELECT * FROM CABPRODUCTCODEMASTER C,SAPMATERIALMASTER S WHERE C.INTRMSAPMATERIALID = S.MATERIALCODEID AND CHRGRADETYPE = '" + this.Grade + "'  AND INTDIAMETER = " + Convert.ToInt32(this.Dia) + " AND TNTSTATUSID = 1");
				//DBManager dbManager = new DBManager();
				//ds = dbManager.ExecuteDataSet(CommandType.Text, sb.ToString());
				string query = "SELECT * FROM CABPRODUCTCODEMASTER C,SAPMATERIALMASTER S WHERE C.INTRMSAPMATERIALID = S.MATERIALCODEID AND CHRGRADETYPE = '" + this.Grade + "'  AND INTDIAMETER = " + Convert.ToInt32(this.Dia) + " AND TNTSTATUSID = 1";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
				{
					sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                        {	
                            sqlDataAdapter.Fill(ds);
                        }
                    }
                }

                dtMat = ds.Tables[0];
				if (dtMat.Rows.Count == 0)
				{
					errMsg = "Material is not available for this Grade and Diameter combination.";
					return false;
				}

				if (this.ShapeCategory != "3-D")
				{
					for (Int16 row = 0; row <= ShapeParametersList.Count - 1; row++)
					{
						if (ShapeParametersList[row].AngleType != "ESPLICE" && ShapeParametersList[row].AngleType != "DEXTRA" && ShapeParametersList[row].AngleType != "NSPLICE")
						{
							if (Convert.ToInt32(ShapeParametersList[row].ParameterValueCab) <= 0 && ShapeParametersList[row].AngleType.ToUpper() == "LENGTH")
							{
								errMsg = "Length is less than or equal to 0.";
								return false;
							}
							if (Convert.ToInt32(ShapeParametersList[row].ParameterValueCab) <= 0 && ShapeParametersList[row].AngleType.ToUpper() == "SPRING")
							{
								errMsg = "Length is less than or equal to 0.";
								return false;
							}

							if (ShapeParametersList[row].AngleType.ToUpper() == "ANGLE")
							{
								if (Convert.ToInt32(ShapeParametersList[row].ParameterValueCab) <= 0)
								{
									errMsg = "Angle is less than or equal to 0.";
									return false;
								}
								if (Convert.ToInt32(ShapeParametersList[row].ParameterValueCab) > 180)
								{
									errMsg = "Angle is greater than 180.";
									return false;
								}
							}
							else if (ShapeParametersList[row].AngleType.ToUpper() == "ARC_RADIUS")
							{
								if (Convert.ToInt32(ShapeParametersList[row].ParameterValueCab) <= 0)
								{
									errMsg = "Zero value entered for Arc Radius.";
									return false;
								}
							}
						}
					}
				}

				errMsg = "";
				return true;
			}
			catch (Exception ex)
			{
				errMsg = ex.Message.ToString();
				return false;
			}
			finally
			{
				//dbManager.Close();
				//dbManager.Dispose();
			}
		}

		/// <summary>
		/// Check if shape code is active or not. If inactive, prompt the user.
		/// </summary>
		/// <param name="errMsg"></param>
		/// <returns></returns>
		public bool ShapeParameter(out string errMsg)
		{
			DBManager dbManager = new DBManager();
			DataSet ds = new DataSet();
			StringBuilder sb = new StringBuilder();
			//dbManager.Open();
			try
			{
				DataTable dt = new DataTable();
                //sb = new StringBuilder();
                //Clarify Suni about standard id. if required include in the below query.
                //sb = sb.Append("SELECT CSM_SHAPE_ID,CSC_CAT_DESC FROM T_CAB_SHAPE_MAST INNER JOIN T_CM_SHAPE_CAT ON CSC_CAT_ID=CSM_CAT_ID INNER JOIN T_CM_SHAPE_MAPPING ON CSM_MASTER_SHAPE_ID=CSM_SHAPE_ID WHERE CSM_PLANT_SHAPE_ID= '" + this.ShapeCode + "' And CSM_ACT_INACTIVE=1 ");
                //ds = dbManager.ExecuteDataSet(CommandType.Text, sb.ToString());
                string query = "SELECT CSM_SHAPE_ID,CSC_CAT_DESC FROM T_CAB_SHAPE_MAST INNER JOIN T_CM_SHAPE_CAT ON CSC_CAT_ID=CSM_CAT_ID INNER JOIN T_CM_SHAPE_MAPPING ON CSM_MASTER_SHAPE_ID=CSM_SHAPE_ID WHERE CSM_PLANT_SHAPE_ID= '" + this.ShapeCode + "' And CSM_ACT_INACTIVE=1 ";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                        {
                            sqlDataAdapter.Fill(ds);
                        }
                    }
                }
                dt = ds.Tables[0];
				if (dt.Rows.Count == 0)
				{
					errMsg = "Incorrect Shape Id.";
					return false;
				}
				this.ShapeCategory = dt.Rows[0]["CSC_CAT_DESC"].ToString();
				errMsg = "";
				return true;
			}
			catch (Exception ex)
			{
				errMsg = ex.Message.ToString();
				return false;
			}
			finally
			{
				//dbManager.Close();
				//dbManager.Dispose();
			}
		}

        private class DBManager
        {
            internal DataSet ExecuteDataSet(CommandType text, string v)
            {
                throw new NotImplementedException();
            }

            internal void Open()
            {
                throw new NotImplementedException();
            }
        }
        #endregion

    }
}
