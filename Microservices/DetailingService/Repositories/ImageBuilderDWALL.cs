using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Dapper;
using DetailingService.Constants;
using Microsoft.Data.SqlClient;
using DetailingService.Repositories;

public class ImageBuilderDWALL
{
    //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

    private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";


    public ImageBuilderDWALL()
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="shapeCode"></param>
    public void CreateImage(string shapeCode, int cabproductmarkid)
    {
        try
        {
            DataTable dtShapeCordinates = new DataTable();
            if (shapeCode != string.Empty && shapeCode.Trim() != null && shapeCode.Trim() != "")
            {
                //get the image from database.
                byte[] img = GetShapeImage(shapeCode.Trim());

                //load the image in byte array format to a bitmap.
                Image image = Image.FromStream(new MemoryStream(img));
                Bitmap bmp = ConvertToRGB((Bitmap)image);

                //Clear the img array
                Array.Clear(img, 0, img.Length);

                //Call a method to get the x y values from database.  
                //PLEASE MODIFY THE PROCEDURE HERE.
                //REPLACE THE DR[1] VALUES WITH SHAPE PARAMETER VALUE.
                //DO NOT MODIFY THE PROCEDURE IN THE BELOW METHOD. IT GETS CALLED FROM DIFFRENT PAGE ALSO.
                dtShapeCordinates = GetShapeCoordinates(shapeCode.Trim(), cabproductmarkid).Tables[0];
                foreach (DataRow dr in dtShapeCordinates.Rows)
                {
                    if ((!string.IsNullOrEmpty(dr["chrparamname"].ToString()) || (dr["chrparamname"].ToString() != null)) && (dr["Visible"].ToString().ToUpper().Equals("VISIBLE")))
                    {
                        if (!(Convert.ToInt32(dr["intXCoord"].ToString()) == 0) && !(Convert.ToInt32(dr["intyCoord"].ToString()) == 0))
                        {
                            //Draw string on the bitmap file.
                            Graphics g = Graphics.FromImage(bmp);
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.DrawString(dr["vchFieldValue"].ToString(), new Font("Areal Black", 12f, FontStyle.Bold), Brushes.Black, new Point(Convert.ToInt32(dr["intXCoord"].ToString()), Convert.ToInt32(dr["intyCoord"].ToString()) + 3));
                            g.Save();
                            g.Dispose();
                        }
                    }
                }

                //Load the bitmap to memory stream.
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Gif);

                //Copy the memory stream to a byte array. BVyte array to be saved to database.
                img = ms.ToArray();
                if (img.Length > 0)
                {
                    //Call method to save the byte array to database.
                    bool flag = ImportImageToDB(img, shapeCode.Trim(), cabproductmarkid);
                }

                //Clear the array.
                Array.Clear(img, 0, img.Length);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="StrShapeID"></param>
    /// <returns></returns>
    public byte[] GetShapeImage(string StrShapeID)
    {
        byte[] image = null;

        DataSet ds = new DataSet();
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                //dynamicParameters.Add("@SHAPECODE", StrShapeID);
                //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_GET_SHAPE_IMAGE, dynamicParameters, commandType: CommandType.StoredProcedure);


                

                SqlCommand cmd = new SqlCommand(SystemConstant.USP_GET_SHAPE_IMAGE, sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SHAPECODE", StrShapeID));


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);

                if (ds.Tables.Count != 0)
                {
                    foreach (DataRowView drCABProductMarkID in ds.Tables[0].DefaultView)
                    {
                        image = (byte[])drCABProductMarkID[0];
                    }
                }
            }



        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }
        return image;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="StrShapeID"></param>
    /// <returns></returns>
    public DataSet GetShapeCoordinates(string StrShapeID, int cabproductmarkid)
    {

        DataSet ds = new DataSet();
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                //var dynamicParameters = new DynamicParameters();
                //dynamicParameters.Add("@StrShapeID", StrShapeID);
                //dynamicParameters.Add("@productmarkid", cabproductmarkid);
                //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.CABShapeCoordsDWALL_get_cube, dynamicParameters, commandType: CommandType.StoredProcedure);

                SqlCommand cmd = new SqlCommand(SystemConstant.CABShapeCoordsDWALL_get_cube, sqlConnection);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StrShapeID", StrShapeID));
                cmd.Parameters.Add(new SqlParameter("@productmarkid", cabproductmarkid));



                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);


                return ds;
            }




        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }
        return ds;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="imag"></param>
    /// <param name="shapeCode"></param>
    /// <returns></returns>
    public bool ImportImageToDB(byte[] imag, string shapeCode, int cabproductmarkid)
    {

        StringBuilder sb = new StringBuilder();
        try
        {
            //sb.Append("UPDATE tbl_Dwall_Report_Cube SET Image=@Pic WHERE CABProductmarkingID='" + cabproductmarkid + "'");

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intcabproductmarkid", cabproductmarkid);
                dynamicParameters.Add("@Pic", imag);
                sqlConnection.QueryFirstOrDefault(SystemConstant.USP_UPDATE_SHAPE_IMAGE, dynamicParameters, commandType: CommandType.StoredProcedure);

            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {

            sb.Clear();
        }
    }


    public bool insertGroupMarkID(int groupmarkid)
    {

        try
        {

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@groupmarkid", groupmarkid);

                sqlConnection.QueryFirstOrDefault(SystemConstant.Usp_cubeshapeparameter_insert_forgm, dynamicParameters, commandType: CommandType.StoredProcedure);

            }


            GetDetails(groupmarkid);
            return true;


        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {


        }
    }

    public bool GetDetails(int groupmarkid)
    {

        bool isSuccuess = false;
       
        ShapeParameter objshappee = new ShapeParameter();
        ImageBuilderDWALL objimage = new ImageBuilderDWALL();
        DataSet dtcabproductmark = new DataSet();
        try
        {
            dtcabproductmark = objshappee.getcabproductmark(groupmarkid);
            foreach (DataTable dt in dtcabproductmark.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string shapecode = Convert.ToString(dr["intshapecode"]);
                    int productmark = Convert.ToInt32(dr["intcabproductmarkid"]);
                    objimage.CreateImage(shapecode, productmark);
                }
            }
            return true;
        }
        
        catch (Exception ex)
        {
            return false;
        }
        finally
        {


        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    private Bitmap ConvertToRGB(Bitmap original)
    {
        try
        {
            Bitmap newImage = new Bitmap(original.Width, original.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            newImage.SetResolution(original.HorizontalResolution, original.VerticalResolution);
            Graphics g = Graphics.FromImage(newImage);
            g.DrawImageUnscaled(original, 0, 0);
            g.Dispose();
            return newImage;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}

