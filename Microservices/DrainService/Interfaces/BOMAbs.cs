using DrainService.Repositories;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DrainService.Interfaces
{
    public interface BOMAbs
    {
        public  int BOMCheck(BOMInfo obj);
        public  int DeleteBOMDetails(BOMInfo obj);
        public  DataSet GetBendingCheck(BOMInfo obj);
        public  DataSet GetBOM(BOMInfo obj);
        public  DataSet GetBOMDetailsForPDF(BOMInfo obj);
        public  DataSet GetBOMDrawingHeader(BOMInfo obj);
        public  SqlDataReader GetBOMEditStatus(BOMInfo obj);
        public  DataSet GetBOMHeader(BOMInfo obj);
        //public  DataSet GetBomUserid(WBSInfo objWbsInfo); by vidya
        public  DataSet GetDoubleBend(BOMInfo obj);
        public  int GetOverHang(BOMInfo obj);
        public  DataSet GetProductionMWLength(BOMInfo obj);
        public  DataSet GetRawMaterial();
        public  int GetSpaceShift(BOMInfo obj);
        public  SqlDataReader GetWireDiameter(BOMInfo obj);
        public  int GetWireRemovalFail(BOMInfo obj);
        public  int GetWireRemovalPass(BOMInfo obj);
        public  DataSet GetWireType(BOMInfo obj);
        public  int InsertBOMDetails(BOMInfo obj);
        public  int InsertBOMHeaderDetails(BOMInfo obj);
        public  int IsBendingCheckRequired(BOMInfo obj);
        public  DataSet UpdateBOMPDF(BOMInfo obj);
        public  int UpdateProductionBOMDetails(BOMInfo obj);
        public  string[] ValidateUser(BOMInfo obj);
    }
}
