using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Web;
using DrainService.Constants;

namespace DrainService.Repositories
{
    public class BendingCheck
    {

    }

    //public class BendingCheck
    //{
    //    BOMInfo objBOMInfo = new BOMInfo();
    //    BOMDal objBOMDal = new BOMDal();
    //    private DataSet dsBendingCheck;
    //    //private DataSet dsBendingCheck;
    //    private int k = 0;
    //    private int intInterval;
    //    private int intInterval2B;
    //    private int intInterval3B;
    //    private int intInterval4B;
    //    private int intInterval5B;
    //    private int intInterval6B;


    //    private string strInterval;
    //    private int A;
    //    private int B;
    //    private int C;
    //    private int D;
    //    private int E;
    //    private int F;
    //    private int G;
    //    private int H;
    //    private int I;
    //    private int J;
    //    private int L;
    //    private int M;
    //    private int N;
    //    private int O;
    //    private int P;
    //    private int Q;
    //    private int R;
    //    private int S;
    //    private int T;




    //    private decimal FB;
    //    private decimal FB1;
    //    private decimal FB2;
    //    private decimal FB3;
    //    private decimal FB4;
    //    private decimal FB5;

    //    private string[] strBendLengths = new string[51];
    //    private int intNoWire = 0;
    //    private int mw_dia;
    //    private int cw_dia;
    //    private string mw_spacing;
    //    private string cw_spacing;
    //    private int intMWSpacing;
    //    private int intCWSpacing;
    //    private string[] strSegment;
    //    private int A1;
    //    private int B2;
    //    private int C1;
    //    private int C2;
    //    private int D1;
    //    private int D2;
    //    private int E1;
    //    private int E2;
    //    private int F1;
    //    private int F2;
    //    private int G1;



    //    private int intSegmentCount;
    //    private int intBendLengthCount;

    //    private decimal intFactor;
    //    private string StrStatus = "";
    //    private string StrStatusCo1 = "";
    //    private string StrStatusMo1 = "";

    //    private string FBStrStatus = "";
    //    private string SBStrStatus = "";

    //    private string SBStrStatus2B = "";
    //    private string SBStrStatus3B = "";
    //    private string SBStrStatus4B = "";
    //    private string SBStrStatus5B = "";
    //    private string SBStrStatus6B = "";



    //    private string FBStrStatus2B = "";
    //    private string FBStrStatus3B = "";
    //    private string FBStrStatus4B = "";
    //    private string FBStrStatus5B = "";
    //    private string FBStrStatus6B = "";

    //    private int B1 = 0;
    //    private decimal SB;
    //    private decimal SB1;
    //    private decimal SB2;
    //    private decimal SB3;
    //    private decimal SB4;
    //    private decimal SB5;

    //    private string StrMethod = "";
    //    private int ResMo1;
    //    private int ResMo2;
    //    private int ResCo1;
    //    private int ResCo2;
    //    private string segment;
    //    private string Shapecode;

    //    private int Mo1;
    //    private int Co1;
    //    private int CW_A = 0;
    //    private int CW_B = 0;
    //    private int CW_C = 0;
    //    private int CW_D = 0;
    //    private int CW_E = 0;
    //    private int CW_F = 0;
    //    private int CW_G = 0;
    //    private string StrStatus1;
    //    private int Mo2;
    //    private int Co2;
    //    private int intNumWire;
    //    private string strSpaceLength = "";
    //    private string StrSpaceLengthCo1 = "";
    //    private string strspace = "";
    //    private string strspaceCo1 = "";
    //    private string strWireRemove;
    //    private string[] strSpaceShift;
    //    private string[] strSpaceShiftCo1;
    //    private int intfailcount = 0;
    //    private int intNoWireRemoved = 0;
    //    private string[] strRemoveSplit;
    //    private string OHStatus1 = "";
    //    private string OHStatus2 = "";
    //    private string OHStatus3 = "";
    //    private string OHStatus4 = "";
    //    private decimal intFactorCW;
    //    private string intFactorMW;

    //    private string[] StrStatusMo1Split;
    //    private string[] StrStatusCo1Split;

    //    private DataSet dsProdMWLen;
    //    private int intProdMWLen;


    //    public void BendingCheck(ArrayList AL)
    //    {
    //        string StrConcatenate;

    //        try
    //        {
    //            objBOMInfo.ProductMarkingId = Convert.ToInt32(AL[0]); // intproductmarkid
    //            objBOMInfo.ShapeId = Convert.ToInt32(AL[1]); // intShapeid
    //            objBOMInfo.StructureElement = AL[8].ToString();
    //            dsBendingCheck = objBOMDal.GetBendingCheck(objBOMInfo);

    //            if ((dsBendingCheck.Tables.Count > 0))
    //            {
    //                if ((dsBendingCheck.Tables[0].Rows.Count > 0))
    //                {
    //                    mw_dia = Convert.ToInt32(dsBendingCheck.Tables[0].Rows[0]["intMWDia"].ToString());
    //                    cw_dia = Convert.ToInt32(dsBendingCheck.Tables[0].Rows[0]["intCWDia"].ToString());


    //                    mw_spacing = dsBendingCheck.Tables[0].Rows[0]["intMWSpace"].ToString();

    //                    if ((mw_spacing == ""))
    //                        intMWSpacing = 0;
    //                    else
    //                        intMWSpacing = Convert.ToInt32(mw_spacing);
    //                    cw_spacing = dsBendingCheck.Tables[0].Rows[0]["intCWSpace"].ToString();
    //                    if ((cw_spacing == ""))
    //                        intCWSpacing = 0;
    //                    else
    //                        intCWSpacing = Convert.ToInt32(cw_spacing);
    //                }

    //                if (dsBendingCheck.Tables[6].Rows.Count > 0)
    //                {
    //                    intSegmentCount = Convert.ToInt32(Interaction.IIf(dsBendingCheck.Tables[6].Rows[0].IsNull("intNoOfSegments"), 0, Convert.ToInt32(dsBendingCheck.Tables[6].Rows[0]["intNoOfSegments"])));
    //                    intBendLengthCount = Convert.ToInt32(Interaction.IIf(dsBendingCheck.Tables[6].Rows[0].IsNull("intBendLengthCount"), 0, Convert.ToInt32(dsBendingCheck.Tables[6].Rows[0]["intBendLengthCount"])));
    //                }

    //                if ((dsBendingCheck.Tables[3].Rows.Count > 0))
    //                {
    //                    for (k = 0; k <= intBendLengthCount - 1; k++)

    //                        strBendLengths[k] = dsBendingCheck.Tables[3].Rows[k]["sitBendLength"].ToString();
    //                }
    //            }



    //            objBOMInfo.ProductMarkingId = Convert.ToInt32(AL[0]);
    //            dsProdMWLen = objBOMDal.GetProductionMWLength(objBOMInfo);
    //            if ((dsProdMWLen.Tables.Count > 0))
    //            {
    //                if ((dsProdMWLen.Tables[0].Rows.Count > 0))
    //                    intProdMWLen = Convert.ToInt32(dsProdMWLen.Tables[0].Rows[0]["numProductionMWLength"]);
    //            }

    //            segment = AL[5].ToString();
    //            segment = segment.Replace(":", "");
    //            strSegment = segment.Split(";");


    //            if (strSegment.Length < 3)
    //            {
    //                A = Convert.ToInt32(Convert.ToInt32(strSegment[0].Substring(1)));
    //                B = Convert.ToInt32(Convert.ToInt32(strSegment[1].Substring(1)));
    //            }
    //            else if (strSegment.Length > 2 & strSegment.Length < 4)
    //            {
    //                A = Convert.ToInt32(Convert.ToInt32(strSegment[0].Substring(1)));
    //                B = Convert.ToInt32(Convert.ToInt32(strSegment[1].Substring(1)));
    //                C = Convert.ToInt32(Convert.ToInt32(strSegment[2].Substring(1)));
    //            }
    //            else if (strSegment.Length > 3 & strSegment.Length < 5)
    //            {
    //                A = Convert.ToInt32(Convert.ToInt32(strSegment[0].Substring(1)));
    //                B = Convert.ToInt32(Convert.ToInt32(strSegment[1].Substring(1)));
    //                C = Convert.ToInt32(Convert.ToInt32(strSegment[2].Substring(1)));
    //                D = Convert.ToInt32(Convert.ToInt32(strSegment[3].Substring(1)));
    //            }
    //            else if (strSegment.Length > 4 & strSegment.Length < 6)
    //            {
    //                A = Convert.ToInt32(Convert.ToInt32(strSegment[0].Substring(1)));
    //                B = Convert.ToInt32(Convert.ToInt32(strSegment[1].Substring(1)));
    //                C = Convert.ToInt32(Convert.ToInt32(strSegment[2].Substring(1)));
    //                D = Convert.ToInt32(Convert.ToInt32(strSegment[3].Substring(1)));
    //                E = Convert.ToInt32(Convert.ToInt32(strSegment[4].Substring(1)));
    //            }
    //            else if (strSegment.Length > 5 & strSegment.Length < 7)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //            }
    //            else if (strSegment.Length > 6 & strSegment.Length < 8)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //            }
    //            else if (strSegment.Length > 7 & strSegment.Length < 9)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //                H = Convert.ToInt32(strSegment[7].Substring(1));
    //            }
    //            else if (strSegment.Length > 8 & strSegment.Length < 10)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //                H = Convert.ToInt32(strSegment[7].Substring(1));
    //                I = Convert.ToInt32(strSegment[8].Substring(1));
    //            }
    //            else if (strSegment.Length > 9 & strSegment.Length < 11)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //                H = Convert.ToInt32(strSegment[7].Substring(1));
    //                I = Convert.ToInt32(strSegment[8].Substring(1));
    //                J = Convert.ToInt32(strSegment[9].Substring(1));
    //            }
    //            else if (strSegment.Length > 10 & strSegment.Length < 12)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //                H = Convert.ToInt32(strSegment[7].Substring(1));
    //                I = Convert.ToInt32(strSegment[8].Substring(1));
    //                J = Convert.ToInt32(strSegment[9].Substring(1));
    //                L = Convert.ToInt32(strSegment[10].Substring(1));
    //            }
    //            else if (strSegment.Length > 11 & strSegment.Length < 13)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //                H = Convert.ToInt32(strSegment[7].Substring(1));
    //                I = Convert.ToInt32(strSegment[8].Substring(1));
    //                J = Convert.ToInt32(strSegment[9].Substring(1));
    //                L = Convert.ToInt32(strSegment[10].Substring(1));
    //                M = Convert.ToInt32(strSegment[11].Substring(1));
    //            }
    //            else if (strSegment.Length > 12 & strSegment.Length < 14)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //                H = Convert.ToInt32(strSegment[7].Substring(1));
    //                I = Convert.ToInt32(strSegment[8].Substring(1));
    //                J = Convert.ToInt32(strSegment[9].Substring(1));
    //                L = Convert.ToInt32(strSegment[10].Substring(1));
    //                M = Convert.ToInt32(strSegment[11].Substring(1));
    //                N = Convert.ToInt32(strSegment[12].Substring(1));
    //            }
    //            else if (strSegment.Length > 13 & strSegment.Length < 15)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //                H = Convert.ToInt32(strSegment[7].Substring(1));
    //                I = Convert.ToInt32(strSegment[8].Substring(1));
    //                J = Convert.ToInt32(strSegment[9].Substring(1));
    //                L = Convert.ToInt32(strSegment[10].Substring(1));
    //                M = Convert.ToInt32(strSegment[11].Substring(1));
    //                N = Convert.ToInt32(strSegment[12].Substring(1));
    //                O = Convert.ToInt32(strSegment[13].Substring(1));
    //            }
    //            else if (strSegment.Length > 14 & strSegment.Length < 16)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //                H = Convert.ToInt32(strSegment[7].Substring(1));
    //                I = Convert.ToInt32(strSegment[8].Substring(1));
    //                J = Convert.ToInt32(strSegment[9].Substring(1));
    //                L = Convert.ToInt32(strSegment[10].Substring(1));
    //                M = Convert.ToInt32(strSegment[11].Substring(1));
    //                N = Convert.ToInt32(strSegment[12].Substring(1));
    //                O = Convert.ToInt32(strSegment[13].Substring(1));
    //                P = Convert.ToInt32(strSegment[14].Substring(1));
    //            }
    //            else if (strSegment.Length > 15 & strSegment.Length < 17)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //                H = Convert.ToInt32(strSegment[7].Substring(1));
    //                I = Convert.ToInt32(strSegment[8].Substring(1));
    //                J = Convert.ToInt32(strSegment[9].Substring(1));
    //                L = Convert.ToInt32(strSegment[10].Substring(1));
    //                M = Convert.ToInt32(strSegment[11].Substring(1));
    //                N = Convert.ToInt32(strSegment[12].Substring(1));
    //                O = Convert.ToInt32(strSegment[13].Substring(1));
    //                P = Convert.ToInt32(strSegment[14].Substring(1));
    //                Q = Convert.ToInt32(strSegment[15].Substring(1));
    //            }
    //            else if (strSegment.Length > 16 & strSegment.Length < 18)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //                H = Convert.ToInt32(strSegment[7].Substring(1));
    //                I = Convert.ToInt32(strSegment[8].Substring(1));
    //                J = Convert.ToInt32(strSegment[9].Substring(1));
    //                L = Convert.ToInt32(strSegment[10].Substring(1));
    //                M = Convert.ToInt32(strSegment[11].Substring(1));
    //                N = Convert.ToInt32(strSegment[12].Substring(1));
    //                O = Convert.ToInt32(strSegment[13].Substring(1));
    //                P = Convert.ToInt32(strSegment[14].Substring(1));
    //                Q = Convert.ToInt32(strSegment[15].Substring(1));
    //                R = Convert.ToInt32(strSegment[16].Substring(1));
    //            }
    //            else if (strSegment.Length > 17 & strSegment.Length < 19)
    //            {
    //                A = Convert.ToInt32(strSegment[0].Substring(1));
    //                B = Convert.ToInt32(strSegment[1].Substring(1));
    //                C = Convert.ToInt32(strSegment[2].Substring(1));
    //                D = Convert.ToInt32(strSegment[3].Substring(1));
    //                E = Convert.ToInt32(strSegment[4].Substring(1));
    //                F = Convert.ToInt32(strSegment[5].Substring(1));
    //                G = Convert.ToInt32(strSegment[6].Substring(1));
    //                H = Convert.ToInt32(strSegment[7].Substring(1));
    //                I = Convert.ToInt32(strSegment[8].Substring(1));
    //                J = Convert.ToInt32(strSegment[9].Substring(1));
    //                L = Convert.ToInt32(strSegment[10].Substring(1));
    //                M = Convert.ToInt32(strSegment[11].Substring(1));
    //                N = Convert.ToInt32(strSegment[12].Substring(1));
    //                O = Convert.ToInt32(strSegment[13].Substring(1));
    //                P = Convert.ToInt32(strSegment[14].Substring(1));
    //                Q = Convert.ToInt32(strSegment[15].Substring(1));
    //                R = Convert.ToInt32(strSegment[16].Substring(1));
    //                S = Convert.ToInt32(strSegment[17].Substring(1));
    //            }

    //            string[] strsplit = new string[21];
    //            for (k = 0; k <= strSegment.Length - 1; k++)
    //                strsplit[k] = strSegment[k].Substring(1);

    //            if (strSegment.Length < 3)
    //            {
    //                A = Convert.ToInt32(Convert.ToInt32(strsplit[0]));
    //                B = Convert.ToInt32(Convert.ToInt32(strsplit[1]));
    //            }
    //            else if (strSegment.Length > 2 & strSegment.Length < 4)
    //            {
    //                A = Convert.ToInt32(Convert.ToInt32(strsplit[0]));
    //                B = Convert.ToInt32(Convert.ToInt32(strsplit[1]));
    //                C = Convert.ToInt32(Convert.ToInt32(strsplit[2]));
    //            }
    //            else if (strSegment.Length > 3 & strSegment.Length < 5)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //            }
    //            else if (strSegment.Length > 4 & strSegment.Length < 6)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //            }
    //            else if (strSegment.Length > 5 & strSegment.Length < 7)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //            }
    //            else if (strSegment.Length > 6 & strSegment.Length < 8)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //            }
    //            else if (strSegment.Length > 7 & strSegment.Length < 9)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //                H = Convert.ToInt32(strsplit[7]);
    //            }
    //            else if (strSegment.Length > 8 & strSegment.Length < 10)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //                H = Convert.ToInt32(strsplit[7]);
    //                I = Convert.ToInt32(strsplit[8]);
    //            }
    //            else if (strSegment.Length > 9 & strSegment.Length < 11)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //                H = Convert.ToInt32(strsplit[7]);
    //                I = Convert.ToInt32(strsplit[8]);
    //                J = Convert.ToInt32(strsplit[9]);
    //            }
    //            else if (strSegment.Length > 10 & strSegment.Length < 12)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //                H = Convert.ToInt32(strsplit[7]);
    //                I = Convert.ToInt32(strsplit[8]);
    //                J = Convert.ToInt32(strsplit[9]);
    //                L = Convert.ToInt32(strsplit[10]);
    //            }
    //            else if (strSegment.Length > 11 & strSegment.Length < 13)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //                H = Convert.ToInt32(strsplit[7]);
    //                I = Convert.ToInt32(strsplit[8]);
    //                J = Convert.ToInt32(strsplit[9]);
    //                L = Convert.ToInt32(strsplit[10]);
    //                M = Convert.ToInt32(strsplit[11]);
    //            }
    //            else if (strSegment.Length > 12 & strSegment.Length < 14)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //                H = Convert.ToInt32(strsplit[7]);
    //                I = Convert.ToInt32(strsplit[8]);
    //                J = Convert.ToInt32(strsplit[9]);
    //                L = Convert.ToInt32(strsplit[10]);
    //                M = Convert.ToInt32(strsplit[11]);
    //                N = Convert.ToInt32(strsplit[12]);
    //            }
    //            else if (strSegment.Length > 13 & strSegment.Length < 15)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //                H = Convert.ToInt32(strsplit[7]);
    //                I = Convert.ToInt32(strsplit[8]);
    //                J = Convert.ToInt32(strsplit[9]);
    //                L = Convert.ToInt32(strsplit[10]);
    //                M = Convert.ToInt32(strsplit[11]);
    //                N = Convert.ToInt32(strsplit[12]);
    //                O = Convert.ToInt32(strsplit[13]);
    //            }
    //            else if (strSegment.Length > 14 & strSegment.Length < 16)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //                H = Convert.ToInt32(strsplit[7]);
    //                I = Convert.ToInt32(strsplit[8]);
    //                J = Convert.ToInt32(strsplit[9]);
    //                L = Convert.ToInt32(strsplit[10]);
    //                M = Convert.ToInt32(strsplit[11]);
    //                N = Convert.ToInt32(strsplit[12]);
    //                O = Convert.ToInt32(strsplit[13]);
    //                P = Convert.ToInt32(strsplit[14]);
    //            }
    //            else if (strSegment.Length > 15 & strSegment.Length < 17)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //                H = Convert.ToInt32(strsplit[7]);
    //                I = Convert.ToInt32(strsplit[8]);
    //                J = Convert.ToInt32(strsplit[9]);
    //                L = Convert.ToInt32(strsplit[10]);
    //                M = Convert.ToInt32(strsplit[11]);
    //                N = Convert.ToInt32(strsplit[12]);
    //                O = Convert.ToInt32(strsplit[13]);
    //                P = Convert.ToInt32(strsplit[14]);
    //                Q = Convert.ToInt32(strsplit[15]);
    //            }
    //            else if (strSegment.Length > 16 & strSegment.Length < 18)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //                H = Convert.ToInt32(strsplit[7]);
    //                I = Convert.ToInt32(strsplit[8]);
    //                J = Convert.ToInt32(strsplit[9]);
    //                L = Convert.ToInt32(strsplit[10]);
    //                M = Convert.ToInt32(strsplit[11]);
    //                N = Convert.ToInt32(strsplit[12]);
    //                O = Convert.ToInt32(strsplit[13]);
    //                P = Convert.ToInt32(strsplit[14]);
    //                Q = Convert.ToInt32(strsplit[15]);
    //                R = Convert.ToInt32(strsplit[16]);
    //            }
    //            else if (strSegment.Length > 17 & strSegment.Length < 19)
    //            {
    //                A = Convert.ToInt32(strsplit[0]);
    //                B = Convert.ToInt32(strsplit[1]);
    //                C = Convert.ToInt32(strsplit[2]);
    //                D = Convert.ToInt32(strsplit[3]);
    //                E = Convert.ToInt32(strsplit[4]);
    //                F = Convert.ToInt32(strsplit[5]);
    //                G = Convert.ToInt32(strsplit[6]);
    //                H = Convert.ToInt32(strsplit[7]);
    //                I = Convert.ToInt32(strsplit[8]);
    //                J = Convert.ToInt32(strsplit[9]);
    //                L = Convert.ToInt32(strsplit[10]);
    //                M = Convert.ToInt32(strsplit[11]);
    //                N = Convert.ToInt32(strsplit[12]);
    //                O = Convert.ToInt32(strsplit[13]);
    //                P = Convert.ToInt32(strsplit[14]);
    //                Q = Convert.ToInt32(strsplit[15]);
    //                R = Convert.ToInt32(strsplit[16]);
    //                S = Convert.ToInt32(strsplit[17]);
    //            }

    //            Shapecode = AL[2].ToString();
    //            Mo1 = Convert.ToInt32(Convert.ToInt32(AL[3]));
    //            Mo2 = Convert.ToInt32(Convert.ToInt32(AL[6]));
    //            Co1 = Convert.ToInt32(Convert.ToInt32(AL[4]));
    //            Co2 = Convert.ToInt32(Convert.ToInt32(AL[7]));


    //            if (((Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("HM1")) & (Shapecode != "2M13" & Shapecode != "3M4" & Shapecode != "3M5" & Shapecode != "3M10" & Shapecode != "3M12" & Shapecode != "4M2" & Shapecode != "4M3" & Shapecode != "4M4" & Shapecode != "4M5" & Shapecode != "4M6" & Shapecode != "4M7" & Shapecode != "4M10" & Shapecode != "4M14" & Shapecode != "4M15")))
    //            {

    //                // 'Normal Metohd'

    //                StrStatus = FnShapeCheck(Mo1, Mo2, Co1, Co2, A, B, C, D, E, F, G, H, I, intCWSpacing, Shapecode, intFactor, AL);


    //                // 'ENd Normal Method


    //                // Start OverHang Method'
    //                if (StrStatus == "Fail")
    //                {
    //                    StrMethod = "OH";
    //                    ResMo1 = Mo1 - 25;
    //                    ResMo2 = Mo2 + 25;
    //                    // ResMo2 = intCWSpacing - ResMo1
    //                    ResCo1 = Co1 - 25;
    //                    ResCo2 = Co2 + 25;
    //                    if (ResMo1 > 10)
    //                        OHStatus1 = FnShapeCheck(ResMo1, ResMo2, ResCo1, ResCo2, A, B, C, D, E, F, G, H, I, intCWSpacing, Shapecode, intFactor, AL);
    //                    else
    //                        OHStatus1 = "Fail";
    //                    if (OHStatus1 == "Fail")
    //                    {
    //                        ResMo1 = Mo1 - 50;
    //                        ResMo2 = Mo2 + 50;
    //                        // ResMo2 = intCWSpacing - ResMo1
    //                        ResCo1 = Co1 - 50;
    //                        ResCo2 = Co2 + 50;
    //                        if (ResMo1 > 10)
    //                            OHStatus2 = FnShapeCheck(ResMo1, ResMo2, ResCo1, ResCo2, A, B, C, D, E, F, G, H, I, intCWSpacing, Shapecode, intFactor, AL);
    //                        else
    //                            OHStatus2 = "Fail";
    //                        if (OHStatus2 == "Fail")
    //                        {
    //                            ResMo1 = Mo1 + 25;
    //                            ResMo2 = Mo2 - 25;

    //                            ResCo1 = Co1 + 25;
    //                            ResCo2 = Co2 - 25;
    //                            if (ResMo1 > 10)
    //                                OHStatus3 = FnShapeCheck(ResMo1, ResMo2, ResCo1, ResCo2, A, B, C, D, E, F, G, H, I, intCWSpacing, Shapecode, intFactor, AL);
    //                            else
    //                                OHStatus3 = "Fail";

    //                            if (OHStatus3 == "Fail")
    //                            {
    //                                ResMo1 = Mo1 + 50;
    //                                ResMo2 = Mo2 - 50;
    //                                // ResMo2 = intCWSpacing - ResMo1
    //                                ResCo1 = Co1 + 50;
    //                                ResCo2 = Co2 - 50;
    //                                if (ResMo1 > 10)
    //                                    OHStatus4 = FnShapeCheck(ResMo1, ResMo2, ResCo1, ResCo2, A, B, C, D, E, F, G, H, I, intCWSpacing, Shapecode, intFactor, AL);
    //                                else
    //                                    OHStatus4 = "Fail";
    //                            }
    //                            else
    //                            {
    //                                StrStatus = "Pass";
    //                                StrStatus1 = "Pass";
    //                            }
    //                        }
    //                        else
    //                        {
    //                            StrStatus = "Pass";
    //                            StrStatus1 = "Pass";
    //                        }
    //                    }
    //                    else
    //                    {
    //                        StrStatus = "Pass";
    //                        StrStatus1 = "Pass";
    //                    }
    //                }
    //                else
    //                {
    //                    StrStatus = "Pass";
    //                    StrStatus1 = "Pass";
    //                    ResMo1 = Mo1;
    //                    ResMo2 = Mo2;
    //                    StrMethod = "Normal";
    //                }

    //                if (OHStatus4 == "Fail")
    //                    StrStatus1 = "Fail";
    //                else
    //                    StrStatus1 = "Pass";
    //                // End OverHang Method'

    //                // Start Space Shift Method'
    //                if (StrStatus1 == "Fail")
    //                {
    //                    StrMethod = "SS";
    //                    Mo1 = Convert.ToInt32(Convert.ToInt32(AL[3]));
    //                    Mo2 = Convert.ToInt32(Convert.ToInt32(AL[6]));
    //                    intFactorMW = Convert.ToDecimal((mw_dia + (cw_dia / 2)));
    //                    intFactorCW = Convert.ToDecimal((cw_dia + (mw_dia / 2)));


    //                    if (Shapecode.Contains("DF1") | Shapecode.Contains("DF5"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, G, H, I, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);


    //                        StrStatusMo1Split = StrStatusMo1.Split(",");
    //                        StrStatusCo1Split = StrStatusCo1.Split(",");
    //                        if (StrStatusMo1Split[0] == "Pass" & StrStatusCo1Split[0] == "Pass")
    //                        {
    //                            ResMo1 = Convert.ToInt32(Convert.ToInt32(AL[3]));
    //                            ResMo2 = ResMo2;
    //                            if (ResMo2 < 20)
    //                                StrStatus = "Fail";
    //                            else
    //                                StrStatus = "Pass";
    //                        }
    //                        else
    //                        {
    //                            StrStatus = "Fail";
    //                            ResMo1 = Convert.ToInt32(Convert.ToInt32(AL[3]));
    //                            ResMo2 = Convert.ToInt32(Convert.ToInt32(AL[6]));
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("DF3") | Shapecode.Contains("DF4"))
    //                    {
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        StrStatusCo1Split = StrStatusCo1.Split(",");

    //                        if (StrStatusCo1Split[0] == "Pass")
    //                        {
    //                            ResMo1 = Convert.ToInt32(Convert.ToInt32(AL[3]));
    //                            ResMo2 = ResMo2;
    //                            if (ResMo2 < 20)
    //                                StrStatus = "Fail";
    //                            else
    //                                StrStatus = "Pass";
    //                        }
    //                        else
    //                        {
    //                            StrStatus = "Fail";
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = Convert.ToInt32(AL[6]);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2"))
    //                    {
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        StrStatusCo1Split = StrStatusCo1.Split(",");

    //                        if (StrStatusCo1Split[0] == "Pass")
    //                        {
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = ResMo2;
    //                            if (ResMo2 < 20)
    //                                StrStatus = "Fail";
    //                            else
    //                                StrStatus = "Pass";
    //                        }
    //                        else
    //                        {
    //                            StrStatus = "Fail";
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = Convert.ToInt32(AL[6]);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, C, D, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        StrStatusMo1Split = StrStatusMo1.Split(",");
    //                        StrStatusCo1Split = StrStatusCo1.Split(",");
    //                        if (StrStatusMo1Split[0] == "Pass" & StrStatusCo1Split[0] == "Pass")
    //                        {
    //                            StrStatus = "Pass";
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = ResMo2;
    //                        }
    //                        else
    //                        {
    //                            StrStatus = "Fail";
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = Convert.ToInt32(AL[6]);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ES10") | Shapecode.Contains("ENR6") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("HM1") | Shapecode.Contains("HM1"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, D, E, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        StrStatusMo1Split = StrStatusMo1.Split(",");
    //                        StrStatusCo1Split = StrStatusCo1.Split(",");
    //                        if (StrStatusMo1Split[0] == "Pass" & StrStatusCo1Split[0] == "Pass")
    //                        {
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = ResMo2;
    //                            if (ResMo2 < 20)
    //                                StrStatus = "Fail";
    //                            else
    //                                StrStatus = "Pass";
    //                        }
    //                        else
    //                        {
    //                            StrStatus = "Fail";
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = Convert.ToInt32(AL[6]);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("TB1") | Shapecode.Contains("TN5") | Shapecode.Contains("TS2") | Shapecode.Contains("TS5"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, F, G, H, I, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        StrStatusMo1Split = StrStatusMo1.Split(",");
    //                        StrStatusCo1Split = StrStatusCo1.Split(",");
    //                        if (StrStatusMo1Split[0] == "Pass" & StrStatusCo1Split[0] == "Pass")
    //                        {
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = ResMo2;
    //                            if (ResMo2 < 20)
    //                                StrStatus = "Fail";
    //                            else
    //                                StrStatus = "Pass";
    //                        }
    //                        else
    //                        {
    //                            StrStatus = "Fail";
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = Convert.ToInt32(AL[6]);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("TB11") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TB24"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, E, F, G, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        StrStatusMo1Split = StrStatusMo1.Split(",");
    //                        StrStatusCo1Split = StrStatusCo1.Split(",");

    //                        if (StrStatusMo1Split[0] == "Pass" & StrStatusCo1Split[0] == "Pass")
    //                        {
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = ResMo2;
    //                            if (ResMo2 < 20)
    //                                StrStatus = "Fail";
    //                            else
    //                                StrStatus = "Pass";
    //                        }
    //                        else
    //                        {
    //                            StrStatus = "Fail";
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = Convert.ToInt32(AL[6]);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("TN7") | Shapecode.Contains("TB14") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        StrStatusMo1Split = StrStatusMo1.Split(",");
    //                        StrStatusCo1Split = StrStatusCo1.Split(",");

    //                        if (StrStatusMo1Split[0] == "Pass" & StrStatusCo1Split[0] == "Pass")
    //                        {
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = ResMo2;
    //                            if (ResMo2 < 20)
    //                                StrStatus = "Fail";
    //                            else
    //                                StrStatus = "Pass";
    //                        }
    //                        else
    //                        {
    //                            StrStatus = "Fail";
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = Convert.ToInt32(AL[6]);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("EN14") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("HM1"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        StrStatusMo1Split = StrStatusMo1.Split(",");
    //                        StrStatusCo1Split = StrStatusCo1.Split(",");

    //                        if (StrStatusMo1Split[0] == "Pass" & StrStatusCo1Split[0] == "Pass")
    //                        {
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = ResMo2;
    //                            if (ResMo2 < 20)
    //                                StrStatus = "Fail";
    //                            else
    //                                StrStatus = "Pass";
    //                        }
    //                        else
    //                        {
    //                            StrStatus = "Fail";
    //                            ResMo1 = Convert.ToInt32(AL[3]);
    //                            ResMo2 = Convert.ToInt32(AL[6]);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("TS6"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, H, I, J, D, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        // StrStatusMo1 = "Fail"
    //                        // StrStatusCo1 = "Fail"

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (((Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20")) & (Shapecode != "2M13" & Shapecode != "3M4" & Shapecode != "3M5" & Shapecode != "3M10" & Shapecode != "3M12" & Shapecode != "4M2" & Shapecode != "4M3" & Shapecode != "4M4" & Shapecode != "4M5" & Shapecode != "4M6" & Shapecode != "4M7" & Shapecode != "4M10" & Shapecode != "4M14" & Shapecode != "4M15")))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusMo1Split = StrStatusMo1.Split(",");

    //                        ResMo1 = Convert.ToInt32(AL[3]);
    //                        if (StrStatusMo1Split[0] == "Pass")
    //                            ResMo2 = ResMo2;
    //                        else
    //                            ResMo2 = Convert.ToInt32(AL[6]);
    //                        strSpaceShift = StrStatusMo1.Split(",");
    //                        StrStatus = strSpaceShift[0];
    //                        StrMethod = "SS";
    //                        if (ResMo2 < 20)
    //                            StrStatus = "Fail";
    //                    }

    //                    if (StrStatus == "Pass")
    //                        StrConcatenate = StrStatus + "," + System.Convert.ToString(ResMo1) + "," + System.Convert.ToString(ResMo2) + "," + Convert.ToInt32(AL[4]) + "," + Convert.ToInt32(AL[7]) + "," + StrMethod + "," + StrStatusMo1Split[1] + "," + StrStatusMo1Split[2];
    //                    else
    //                        // StrStatusMo1Split(1) = "0"
    //                        // StrStatusMo1Split(2) = "0"

    //                        StrConcatenate = StrStatus + "," + Convert.ToInt32(AL[3]) + "," + Convert.ToInt32(AL[6]) + "," + Convert.ToInt32(AL[4]) + "," + Convert.ToInt32(AL[7]) + "," + StrMethod;
    //                }

    //                // End Space Shift Method'

    //                // Start Wire Removal Method'

    //                if (StrStatus == "Fail")
    //                {
    //                    StrMethod = "WR";
    //                    Mo1 = Convert.ToInt32(AL[3]);
    //                    Mo2 = Convert.ToInt32(AL[6]);
    //                    intFactorMW = (mw_dia + (cw_dia / 2));
    //                    intFactorCW = (cw_dia + (mw_dia / 2));
    //                    if (Shapecode.Contains("DF1") | Shapecode.Contains("DF5"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, G, H, I, D, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("DF3") | Shapecode.Contains("DF4"))
    //                    {
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        if (StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2"))
    //                    {
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, D, E, F, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        if (StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, I, F, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, C, D, I, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ES10") | Shapecode.Contains("ENR6") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("HM1"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, F, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, D, E, F, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("TB1") | Shapecode.Contains("TN5") | Shapecode.Contains("TS2") | Shapecode.Contains("TS5"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, F, G, H, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("TB11") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TB24"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, E, F, G, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("TN7") | Shapecode.Contains("TB14") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, G, H, I, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("EN14") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("HM1"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, F, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, C, D, E, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("TS6"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, F, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, C, D, E, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        // StrStatusMo1 = "Fail"
    //                        // StrStatusCo1 = "Fail"

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (((Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20")) & (Shapecode != "2M13" & Shapecode != "3M4" & Shapecode != "3M5" & Shapecode != "3M10" & Shapecode != "3M12" & Shapecode != "4M2" & Shapecode != "4M3" & Shapecode != "4M4" & Shapecode != "4M5" & Shapecode != "4M6" & Shapecode != "4M7" & Shapecode != "4M10" & Shapecode != "4M14" & Shapecode != "4M15")))
    //                    {
    //                        StrStatus = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        ResMo1 = Convert.ToInt32(AL[3]);
    //                        ResMo2 = ResMo2;
    //                        strSpaceShift = StrStatus.Split(",");
    //                        StrStatus = strSpaceShift[0];
    //                        StrMethod = "WR";
    //                    }

    //                    if (StrStatus == "Pass")
    //                        StrConcatenate = StrStatus + "," + System.Convert.ToString(ResMo1) + "," + System.Convert.ToString(ResMo2) + "," + Convert.ToInt32(AL[4]) + "," + Convert.ToInt32(AL[7]) + "," + StrMethod + "," + strSpaceShift[1] + "," + strSpaceShift[2] + "," + strSpaceShift[3];
    //                    else
    //                        StrConcatenate = StrStatus + "," + Convert.ToInt32(AL[3]) + "," + Convert.ToInt32(AL[6]) + "," + Convert.ToInt32(AL[4]) + "," + Convert.ToInt32(AL[7]) + "," + StrMethod;
    //                }  // If status Fails
    //            }
    //            else if ((Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C")) & (Shapecode != "2C9" & Shapecode != "3C9" & Shapecode != "3C10" & Shapecode != "3C4" & Shapecode != "3C5"))
    //            {
    //                intFactorMW = (mw_dia + (cw_dia / 2));
    //                intFactorCW = (cw_dia + (mw_dia / 2));

    //                Co1 = Convert.ToInt32(Convert.ToInt32(AL[4]));
    //                Co2 = Convert.ToInt32(AL[7]);
    //                StrStatus = FnStatusCo1(Co1, Co2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                StrMethod = "Normal";
    //                // Start OverHang Method

    //                if (StrStatus == "Fail")
    //                {
    //                    ResCo1 = Co1 - 25;
    //                    ResCo2 = Co2 + 25;
    //                    // ResCo2 = intMWSpacing - ResCo1
    //                    if (Co1 > 10)
    //                    {
    //                        OHStatus1 = FnStatusCo1(ResCo1, ResCo2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        StrMethod = "OH";
    //                    }
    //                    else
    //                        OHStatus1 = "Fail";
    //                    if (OHStatus1 == "Fail")
    //                    {
    //                        ResCo1 = Co1 - 50;
    //                        ResCo2 = Co2 + 50;
    //                        // ResCo2 = intMWSpacing - ResCo1
    //                        if (Co1 > 10)
    //                        {
    //                            OHStatus2 = FnStatusCo1(ResCo1, ResCo2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                            StrMethod = "OH";
    //                        }
    //                        else
    //                            OHStatus2 = "Fail";
    //                        if (OHStatus2 == "Fail")
    //                        {
    //                            ResCo1 = Co1 + 25;
    //                            ResCo2 = Co2 - 25;
    //                            // ResCo2 = intMWSpacing - ResCo1
    //                            if (Co1 > 10)
    //                            {
    //                                OHStatus3 = FnStatusCo1(ResCo1, ResCo2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                                StrMethod = "OH";
    //                            }
    //                            else
    //                                OHStatus3 = "Fail";

    //                            if (OHStatus3 == "Fail")
    //                            {
    //                                ResCo1 = Co1 + 50;
    //                                ResCo2 = Co2 - 50;
    //                                // ResCo2 = intMWSpacing - ResCo1
    //                                if (Co1 > 10)
    //                                {
    //                                    OHStatus4 = FnStatusCo1(ResCo1, ResCo2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                                    StrMethod = "OH";
    //                                }
    //                                else
    //                                    OHStatus4 = "Fail";
    //                            }
    //                            else
    //                                StrStatus = "Pass";
    //                        }
    //                        else
    //                            StrStatus = "Pass";
    //                    }
    //                    else
    //                        StrStatus = "Pass";
    //                }
    //                else
    //                {
    //                    StrStatus = "Pass";
    //                    ResCo1 = Co1;
    //                }
    //                // End OverHang Method '

    //                // Start Space Shift Method'
    //                if (StrStatus == "Fail")
    //                {
    //                    Mo1 = Convert.ToInt32(AL[3]);
    //                    Mo2 = Convert.ToInt32(AL[6]);

    //                    // 'Start Paste


    //                    intFactorMW = (mw_dia + (cw_dia / 2));
    //                    intFactorCW = (cw_dia + (mw_dia / 2));

    //                    if (Shapecode.Contains("DF1") | Shapecode.Contains("DF5"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, G, H, I, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("DF3") | Shapecode.Contains("DF4"))
    //                    {
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        if (StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2"))
    //                    {
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, D, E, F, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        if (StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, I, F, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, C, D, I, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ES10") | Shapecode.Contains("ENR6") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("HM1"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, F, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, D, E, F, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("TB1") | Shapecode.Contains("TN5") | Shapecode.Contains("TS2") | Shapecode.Contains("TS5"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, F, G, H, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("TB11") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TB24"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, E, F, G, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("TN7") | Shapecode.Contains("TB14") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, G, H, I, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("EN14") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("HM1"))
    //                    {
    //                        StrStatusMo1 = FnStatusSpaceshiftMo1(Mo1, Mo2, A, B, C, F, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusSpaceShiftCo1(Co1, C, D, E, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if ((Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C")) & (Shapecode != "2C9" & Shapecode != "3C9" & Shapecode != "3C10" & Shapecode != "3C4" & Shapecode != "3C5"))
    //                    {
    //                        StrStatus = FnStatusSpaceShiftCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        ResMo1 = Convert.ToInt32(AL[3]);
    //                        ResMo2 = Convert.ToInt32(AL[6]);
    //                        strSpaceShift = StrStatus.Split(",");
    //                        StrStatus = strSpaceShift[0];
    //                        StrMethod = "SS";
    //                    }
    //                }

    //                // End Space Shift Method'
    //                if (StrStatus == "Fail")
    //                {
    //                    StrMethod = "WR";
    //                    Mo1 = Convert.ToInt32(AL[3]);
    //                    Mo2 = Convert.ToInt32(AL[6]);
    //                    // Start Wire Removal Method'
    //                    if (Shapecode.Contains("DF1") | Shapecode.Contains("DF5"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, G, H, I, D, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("DF3") | Shapecode.Contains("DF4"))
    //                    {
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        if (StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2"))
    //                    {
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, D, E, F, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        if (StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, I, F, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, C, D, I, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ES10") | Shapecode.Contains("ENR6") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("HM1"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, F, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, D, E, F, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("TB1") | Shapecode.Contains("TN5") | Shapecode.Contains("TS2") | Shapecode.Contains("TS5"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, F, G, H, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("TB11") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TB24"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, E, F, G, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("TN7") | Shapecode.Contains("TB14") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, G, H, I, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("EN14") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("HM1"))
    //                    {
    //                        StrStatusMo1 = FnStatusWireRemovalMo1(Mo1, Mo2, A, B, C, F, E, F, intCWSpacing, Shapecode, intFactorMW);
    //                        StrStatusCo1 = FnStatusWireRemovalCo1(Co1, C, D, E, H, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                        if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else if ((Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C")) & (Shapecode != "2C9" & Shapecode != "3C9" & Shapecode != "3C10" & Shapecode != "3C4" & Shapecode != "3C5"))
    //                    {
    //                        StrStatus = FnStatusWireRemovalCo1(Co1, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                        if (StrStatus == "Pass" & StrStatusCo1 == "Pass")
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                }
    //            }
    //            else if (!Shapecode.Contains("C") & !Shapecode.Contains("M") & !Shapecode.Contains("DF1") & !Shapecode.Contains("DN") & !Shapecode.Contains("DS"))
    //            {
    //                StrStatus = "Fail";
    //                StrMethod = "Normal";
    //                StrConcatenate = StrStatus + "," + Convert.ToInt32(AL[3]) + "," + Convert.ToInt32(AL[6]) + "," + Convert.ToInt32(AL[4]) + "," + Convert.ToInt32(AL[7]) + "," + StrMethod;
    //            } // Shape IC1 1M1 End if


    //            if (StrMethod == "Normal")
    //            {
    //                if (StrStatus == "Pass")
    //                    StrConcatenate = StrStatus + "," + Convert.ToInt32(AL[3]) + "," + Convert.ToInt32(AL[6]) + "," + Convert.ToInt32(AL[4]) + "," + Convert.ToInt32(AL[7]) + "," + StrMethod;
    //                else
    //                    StrConcatenate = StrStatus + "," + Convert.ToInt32(AL[3]) + "," + Convert.ToInt32(AL[6]) + "," + Convert.ToInt32(AL[4]) + "," + Convert.ToInt32(AL[7]) + "," + StrMethod;
    //            }
    //            else if (StrMethod == "OH")
    //            {
    //                if (StrStatus == "Pass")
    //                    StrConcatenate = StrStatus + "," + System.Convert.ToString(ResMo1) + "," + System.Convert.ToString(ResMo2) + "," + System.Convert.ToString(ResCo1) + "," + System.Convert.ToString(ResCo2) + "," + StrMethod;
    //                else
    //                    StrConcatenate = StrStatus + "," + Convert.ToInt32(AL[3]) + "," + Convert.ToInt32(AL[6]) + "," + Convert.ToInt32(AL[4]) + "," + Convert.ToInt32(AL[7]) + "," + StrMethod;
    //            }
    //            else if (StrMethod == "SS")
    //            {
    //                if (StrStatus == "Pass")
    //                    StrConcatenate = StrStatus + "," + System.Convert.ToString(ResMo1) + "," + System.Convert.ToString(ResMo2) + "," + System.Convert.ToString(ResCo1) + "," + System.Convert.ToString(ResCo2) + "," + StrMethod + "," + strSpaceShift[1] + "," + strSpaceShift[2] + "," + strSpaceShift[3];
    //                else
    //                    StrConcatenate = StrStatus + "," + Convert.ToInt32(AL[3]) + "," + Convert.ToInt32(AL[6]) + "," + Convert.ToInt32(AL[4]) + "," + Convert.ToInt32(AL[7]) + "," + StrMethod;
    //            }
    //            else if (StrStatus == "Pass")
    //                StrConcatenate = StrStatus + "," + System.Convert.ToString(ResMo1) + "," + System.Convert.ToString(ResMo2) + "," + System.Convert.ToString(ResCo1) + "," + System.Convert.ToString(ResCo2) + "," + StrMethod + "," + strSpaceShift[1] + "," + strSpaceShift[2] + "," + strSpaceShift[3];
    //            else
    //            {
    //                StrStatus = "Fail";
    //                StrMethod = "WR";
    //                StrConcatenate = StrStatus + "," + Convert.ToInt32(AL[3]) + "," + Convert.ToInt32(AL[6]) + "," + (string)Co1 + "," + Convert.ToInt32(AL[7]) + "," + StrMethod + "," + "0" + "," + "0" + "," + "0";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //        return StrConcatenate;
    //    }

    //    public void FnShapeCheck(int Mo1, int Mo2, int Co1, int Co2, int A, int B, int C, int D, int E, int F, int G, int H, int I, int intCWSpacing, string Shapecode, int intFactor, ArrayList AL)
    //    {
    //        try
    //        {
    //            intFactorMW = (mw_dia + (cw_dia / 2));
    //            intFactorCW = (cw_dia + (mw_dia / 2));

    //            if (Shapecode.Contains("DF1") | Shapecode.Contains("DF5"))
    //            {
    //                StrStatusMo1 = FnStatusMo1(Mo1, Mo2, G, H, I, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                StrStatusCo1 = FnStatusCo1(Co1, Co2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);

    //                if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                    StrStatus = "Pass";
    //                else
    //                    StrStatus = "Fail";
    //            }
    //            else if (Shapecode.Contains("DF3") | Shapecode.Contains("DF4"))
    //            {
    //                StrStatusCo1 = FnStatusCo1(Co1, Co2, A, B, C, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                if (StrStatusCo1 == "Pass")
    //                    StrStatus = "Pass";
    //                else
    //                    StrStatus = "Fail";
    //            }
    //            else if (Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2"))
    //            {
    //                StrStatusCo1 = FnStatusCo1(Co1, Co2, D, E, F, D, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                if (StrStatusCo1 == "Pass")
    //                    StrStatus = "Pass";
    //                else
    //                    StrStatus = "Fail";
    //            }
    //            else if (Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4"))
    //            {
    //                StrStatusMo1 = FnStatusMo1(Mo1, Mo2, A, B, I, F, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                StrStatusCo1 = FnStatusCo1(Co1, Co2, C, D, I, H, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                    StrStatus = "Pass";
    //                else
    //                    StrStatus = "Fail";
    //            }
    //            else if (Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ES10") | Shapecode.Contains("ENR6") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("FS1") | Shapecode.Contains("FN1") | Shapecode.Contains("HM1"))
    //            {
    //                StrStatusMo1 = FnStatusMo1(Mo1, Mo2, A, B, C, F, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                StrStatusCo1 = FnStatusCo1(Co1, Co2, D, E, F, H, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                    StrStatus = "Pass";
    //                else
    //                    StrStatus = "Fail";
    //            }
    //            else if (Shapecode.Contains("TB1") | Shapecode.Contains("TN5") | Shapecode.Contains("TS2") | Shapecode.Contains("TS5") | Shapecode.Contains("FN10"))
    //            {
    //                StrStatusMo1 = FnStatusMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                StrStatusCo1 = FnStatusCo1(Co1, Co2, F, G, H, H, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                    StrStatus = "Pass";
    //                else
    //                    StrStatus = "Fail";
    //            }
    //            else if (Shapecode.Contains("TB11") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TB24") | Shapecode.Contains("FN10"))
    //            {
    //                StrStatusMo1 = FnStatusMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                StrStatusCo1 = FnStatusCo1(Co1, Co2, E, F, G, H, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                    StrStatus = "Pass";
    //                else
    //                    StrStatus = "Fail";
    //            }
    //            else if (Shapecode.Contains("TN7") | Shapecode.Contains("TB14") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7"))
    //            {
    //                StrStatusMo1 = FnStatusMo1(Mo1, Mo2, A, B, C, D, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                StrStatusCo1 = FnStatusCo1(Co1, Co2, G, H, I, H, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                    StrStatus = "Pass";
    //                else
    //                    StrStatus = "Fail";
    //            }
    //            else if (Shapecode.Contains("EN14") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("HM1"))
    //            {
    //                StrStatusMo1 = FnStatusMo1(Mo1, Mo2, A, B, C, F, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                StrStatusCo1 = FnStatusCo1(Co1, Co2, C, D, E, H, E, F, intCWSpacing, Shapecode, intFactorCW);
    //                if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                    StrStatus = "Pass";
    //                else
    //                    StrStatus = "Fail";
    //            }
    //            else if (Shapecode.Contains("TS6"))
    //            {
    //                StrStatusMo1 = FnStatusMo1(Mo1, Mo2, A, B, C, F, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //                StrStatusCo1 = FnStatusCo1(Co1, Co2, H, I, J, H, I, J, intCWSpacing, Shapecode, intFactorCW);
    //                if (StrStatusMo1 == "Pass" & StrStatusCo1 == "Pass")
    //                    StrStatus = "Pass";
    //                else
    //                    StrStatus = "Fail";
    //            }
    //            else if (((Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20")) & (Shapecode != "2M13" & Shapecode != "3M4" & Shapecode != "3M5" & Shapecode != "3M10" & Shapecode != "3M12" & Shapecode != "4M2" & Shapecode != "4M3" & Shapecode != "4M4" & Shapecode != "4M5" & Shapecode != "4M6" & Shapecode != "4M7" & Shapecode != "4M10" & Shapecode != "4M14" & Shapecode != "4M15")))
    //            {
    //                intFactorMW = (mw_dia + (cw_dia / 2));
    //                // Mo1 = AL(3)
    //                // Mo2 = AL(6)

    //                StrStatus = FnStatusMo1(Mo1, Mo2, A, B, C, F, E, F, G, intCWSpacing, Shapecode, intFactorMW);
    //            }
    //        }






    //        catch (Exception ex)
    //        {
    //        }

    //        return StrStatus;
    //    }

    //    public void FnStatusMo1(int Mo1, int Mo2, int A, int B, int C, int D, int E, int F, int G, int intCWSpacing, string Shapecode, int intFactor)
    //    {
    //        try
    //        {
    //            if (Shapecode.Contains("DF"))
    //            {
    //                A = G;
    //                B = H;
    //                C = I;
    //            }
    //            FBStrStatus = "";
    //            FBStrStatus2B = "";
    //            FBStrStatus3B = "";
    //            FBStrStatus4B = "";
    //            FBStrStatus5B = "";
    //            FBStrStatus6B = "";
    //            SBStrStatus = "";
    //            SBStrStatus2B = "";
    //            SBStrStatus3B = "";
    //            SBStrStatus4B = "";
    //            SBStrStatus5B = "";
    //            SBStrStatus6B = "";


    //            if (Mo1 < A)
    //            {
    //                intInterval = Math.Floor((A - Mo1) / intCWSpacing);
    //                // intInterval = (intInterval)

    //                A1 = A - ((intInterval * intCWSpacing) + Mo1);
    //                FB = A1 - intFactor;

    //                if (Shapecode.Contains("1MR") | Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("FBR1") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("JB1") | Shapecode.Contains("HM1"))
    //                {
    //                    if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                        FBStrStatus = "Pass";
    //                    else if (FB > strBendLengths[2])
    //                        FBStrStatus = "Pass";
    //                    else
    //                        FBStrStatus = "Fail";
    //                }
    //                else if (Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DF1") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4M9") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("FN10") | Shapecode.Contains("TS6") | Shapecode.Contains("HM1"))
    //                {
    //                    if (FB > strBendLengths[0])
    //                        FBStrStatus = "Pass";
    //                    else
    //                        FBStrStatus = "Fail";
    //                }

    //                B1 = intCWSpacing - A1;
    //                SB = B1 + intFactor;

    //                if (Shapecode.Contains("1MR") | Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("FBR1") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("JB1") | Shapecode.Contains("HM1"))
    //                {
    //                    if (SB > strBendLengths[3])
    //                        SBStrStatus = "Pass";
    //                    else
    //                        SBStrStatus = "Fail";
    //                }
    //                else if (Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DF1") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4M9") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("FN10") | Shapecode.Contains("TS6") | Shapecode.Contains("HM1"))
    //                {
    //                    if (SB > strBendLengths[1])
    //                        SBStrStatus = "Pass";
    //                    else
    //                        SBStrStatus = "Fail";
    //                }

    //                if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("DF5") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2M6") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("4M9") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("JB1") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("FN10") | Shapecode.Contains("TS6"))
    //                {
    //                    intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                    B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                    FB1 = B2 - intFactor;

    //                    if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("FBR1") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("4M9") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("FN10"))
    //                    {
    //                        if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            FBStrStatus2B = "Pass";
    //                        else if (FB1 > strBendLengths[2])
    //                            FBStrStatus2B = "Pass";
    //                        else
    //                            FBStrStatus2B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("JB1") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6") | Shapecode.Contains("HM1"))
    //                    {
    //                        if (FB1 > strBendLengths[0])
    //                            FBStrStatus2B = "Pass";
    //                        else
    //                            FBStrStatus2B = "Fail";
    //                    }

    //                    C1 = intCWSpacing - B2;
    //                    SB1 = C1 + intFactor;

    //                    if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("FBR1") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("4M9") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("FN10"))
    //                    {
    //                        if (SB1 > strBendLengths[3])
    //                            SBStrStatus2B = "Pass";
    //                        else
    //                            SBStrStatus2B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("JB1") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("FN10") | Shapecode.Contains("TS6") | Shapecode.Contains("HM1"))
    //                    {
    //                        if (SB1 > strBendLengths[1])
    //                            SBStrStatus2B = "Pass";
    //                        else
    //                            SBStrStatus2B = "Fail";
    //                    }
    //                }

    //                if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("4M9") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("JB1") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("FN10") | Shapecode.Contains("TS6"))
    //                {
    //                    intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                    C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                    FB2 = C2 - intFactor;
    //                    if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("4M9") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("FN10"))
    //                    {
    //                        if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            FBStrStatus3B = "Pass";
    //                        else if (FB2 > strBendLengths[2])
    //                            FBStrStatus3B = "Pass";
    //                        else
    //                            FBStrStatus3B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("JB1") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (FB2 > strBendLengths[0])
    //                            FBStrStatus3B = "Pass";
    //                        else
    //                            FBStrStatus3B = "Fail";
    //                    }

    //                    D1 = intCWSpacing - C2;
    //                    SB2 = D1 + intFactor;

    //                    if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("4M9") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("FN10"))
    //                    {
    //                        if (SB2 > strBendLengths[3])
    //                            SBStrStatus3B = "Pass";
    //                        else
    //                            SBStrStatus3B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("JB1") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (SB2 > strBendLengths[1])
    //                            SBStrStatus3B = "Pass";
    //                        else
    //                            SBStrStatus3B = "Fail";
    //                    }
    //                }




    //                if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("4M9") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("JB1") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TS6"))
    //                {
    //                    intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                    D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                    FB3 = D2 - intFactor;
    //                    if (Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("JB1") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26"))
    //                    {
    //                        if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                            FBStrStatus4B = "Pass";
    //                        else if (FB3 > strBendLengths[2])
    //                            FBStrStatus4B = "Pass";
    //                        else
    //                            FBStrStatus4B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("4M9") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (FB3 > strBendLengths[0])
    //                            FBStrStatus4B = "Pass";
    //                        else
    //                            FBStrStatus4B = "Fail";
    //                    }

    //                    E1 = intCWSpacing - D2;
    //                    SB3 = E1 + intFactor;
    //                    if (Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("JB1") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26"))
    //                    {
    //                        if (SB3 > strBendLengths[3])
    //                            SBStrStatus4B = "Pass";
    //                        else
    //                            SBStrStatus4B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("4M9") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (SB3 > strBendLengths[1])
    //                            SBStrStatus4B = "Pass";
    //                        else
    //                            SBStrStatus4B = "Fail";
    //                    }
    //                }

    //                if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7") | Shapecode.Contains("TS6"))
    //                {
    //                    intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                    E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                    FB4 = E2 - intFactor;
    //                    if (Shapecode.Contains("5MR"))
    //                    {
    //                        if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            FBStrStatus5B = "Pass";
    //                        else if (FB4 > strBendLengths[2])
    //                            FBStrStatus5B = "Pass";
    //                        else
    //                            FBStrStatus5B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (FB4 > strBendLengths[0])
    //                            FBStrStatus5B = "Pass";
    //                        else
    //                            FBStrStatus5B = "Fail";
    //                    }

    //                    F1 = intCWSpacing - E2;
    //                    SB4 = F1 + intFactor;
    //                    if (Shapecode.Contains("5MR"))
    //                    {
    //                        if (SB4 > strBendLengths[3])
    //                            SBStrStatus5B = "Pass";
    //                        else
    //                            SBStrStatus5B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (SB4 > strBendLengths[1])
    //                            SBStrStatus5B = "Pass";
    //                        else
    //                            SBStrStatus5B = "Fail";
    //                    }
    //                }

    //                if (Shapecode.Contains("TS6"))
    //                {
    //                    intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                    F2 = F - ((intInterval6B * intCWSpacing) + G1);
    //                    FB5 = F2 - intFactor;

    //                    if (FB4 > strBendLengths[0])
    //                        FBStrStatus6B = "Pass";
    //                    else
    //                        FBStrStatus6B = "Fail";


    //                    G1 = intCWSpacing - E2;
    //                    SB4 = G1 + intFactor;
    //                    if (Shapecode.Contains("TS6"))
    //                    {
    //                        if (SB5 > strBendLengths[1])
    //                            SBStrStatus6B = "Pass";
    //                        else
    //                            SBStrStatus6B = "Fail";
    //                    }
    //                }
    //            }
    //            else if (Mo1 > A)
    //            {
    //                B1 = Mo1 - A;
    //                SB = B1 + intFactor;

    //                if (Shapecode.Contains("1MR") | Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("FBR1") | Shapecode.Contains("DS10") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("JB1"))
    //                {
    //                    if (SB > strBendLengths[3])
    //                        SBStrStatus = "Pass";
    //                    else
    //                        SBStrStatus = "Fail";
    //                }
    //                else if (Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4M9") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("FN10"))
    //                {
    //                    if (SB > strBendLengths[1])
    //                        SBStrStatus = "Pass";
    //                    else
    //                        SBStrStatus = "Fail";
    //                }


    //                if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("FBR1") | Shapecode.Contains("4M9") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("FN10"))
    //                {
    //                    intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                    B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                    FB1 = B2 - intFactor;

    //                    if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        FBStrStatus2B = "Pass";
    //                    else if (FB1 > strBendLengths[2])
    //                        FBStrStatus2B = "Pass";
    //                    else
    //                        FBStrStatus2B = "Fail";
    //                }
    //                else if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("JB1") | Shapecode.Contains("HM1"))
    //                {
    //                    if (FB1 > strBendLengths[0])
    //                        FBStrStatus2B = "Pass";
    //                    else
    //                        FBStrStatus2B = "Fail";
    //                }


    //                C1 = intCWSpacing - B2;
    //                SB1 = C1 + intFactor;

    //                if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("FBR1") | Shapecode.Contains("4M9") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("FN10"))
    //                {
    //                    if (SB1 > strBendLengths[3])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                        SBStrStatus2B = "Fail";
    //                }
    //                else if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("JB1") | Shapecode.Contains("HM1"))
    //                {
    //                    if (SB1 > strBendLengths[1])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                        SBStrStatus2B = "Fail";
    //                }

    //                if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("4M9") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("JB1") | Shapecode.Contains("FN10"))
    //                {
    //                    intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                    C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                    FB2 = C2 - intFactor;


    //                    if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                        FBStrStatus3B = "Pass";
    //                    else if (FB2 > strBendLengths[2])
    //                        FBStrStatus3B = "Pass";
    //                    else
    //                        FBStrStatus3B = "Fail";
    //                }
    //                else if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("JB1"))
    //                {
    //                    if (FB2 > strBendLengths[0])
    //                        FBStrStatus3B = "Pass";
    //                    else
    //                        FBStrStatus3B = "Fail";
    //                }

    //                D1 = intCWSpacing - C2;
    //                SB2 = D1 + intFactor;

    //                if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("4M9") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("FN10"))
    //                {
    //                    if (SB2 > strBendLengths[3])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                        SBStrStatus3B = "Fail";
    //                }
    //                else if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("JB1"))
    //                {
    //                    if (SB2 > strBendLengths[1])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                        SBStrStatus3B = "Fail";
    //                }


    //                if (Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("JB1"))
    //                {
    //                    intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                    D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                    FB3 = D2 - intFactor;
    //                    if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                        FBStrStatus4B = "Pass";
    //                    else if (FB3 > strBendLengths[2])
    //                        FBStrStatus4B = "Pass";
    //                    else
    //                        FBStrStatus4B = "Fail";
    //                }
    //                else if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4M9") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2"))
    //                {
    //                    if (FB3 > strBendLengths[0])
    //                        FBStrStatus4B = "Pass";
    //                    else
    //                        FBStrStatus4B = "Fail";
    //                }

    //                E1 = intCWSpacing - D2;
    //                SB3 = E1 + intFactor;
    //                if (Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("4MR3") | Shapecode.Contains("4C2") | Shapecode.Contains("JB1"))
    //                {
    //                    if (SB3 > strBendLengths[3])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                        SBStrStatus4B = "Fail";
    //                }
    //                else if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4M9") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2"))
    //                {
    //                    if (SB3 > strBendLengths[1])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                        SBStrStatus4B = "Fail";
    //                }

    //                if (Shapecode.Contains("5MR"))
    //                {
    //                    intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                    E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                    FB4 = E2 - intFactor;
    //                    if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                        FBStrStatus4B = "Pass";
    //                    else if (FB4 > strBendLengths[2])
    //                        FBStrStatus5B = "Pass";
    //                    else
    //                        FBStrStatus5B = "Fail";
    //                }
    //                else if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7"))
    //                {
    //                    if (FB4 > strBendLengths[0])
    //                        FBStrStatus5B = "Pass";
    //                    else
    //                        FBStrStatus5B = "Fail";
    //                }

    //                F1 = intCWSpacing - E2;
    //                SB4 = F1 + intFactor;
    //                if (Shapecode.Contains("5MR"))
    //                {
    //                    if (SB4 > strBendLengths[3])
    //                        SBStrStatus5B = "Pass";
    //                    else
    //                        SBStrStatus5B = "Fail";
    //                }
    //                else if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7"))
    //                {
    //                    if (SB4 > strBendLengths[1])
    //                        SBStrStatus5B = "Pass";
    //                    else
    //                        SBStrStatus5B = "Fail";
    //                }
    //            }
    //            else if (((intInterval * intCWSpacing) + Mo1) == A)
    //            {
    //                FBStrStatus = "Fail";
    //                SBStrStatus = "Fail";
    //                FBStrStatus2B = "Fail";
    //                SBStrStatus2B = "Fail";
    //                FBStrStatus3B = "Fail";
    //                SBStrStatus3B = "Fail";
    //                FBStrStatus4B = "Fail";
    //                SBStrStatus4B = "Fail";
    //                FBStrStatus5B = "Fail";
    //                SBStrStatus5B = "Fail";
    //                FBStrStatus6B = "Fail";
    //                SBStrStatus6B = "Fail";
    //            }

    //            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;




    //            if (Shapecode.Contains("TS6"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;
    //                CW_G = Math.Floor((G - (G1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F + CW_G;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));

    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "" & FBStrStatus4B != "" & SBStrStatus4B != "" & FBStrStatus5B != "" & SBStrStatus5B != "" & FBStrStatus6B != "" & SBStrStatus6B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass" & FBStrStatus5B == "Pass" & SBStrStatus5B == "Pass" & FBStrStatus6B != "" & SBStrStatus6B != "")
    //                    {
    //                        if (ResMo2 > 20)
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("5M") | Shapecode.Contains("TS7"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;

    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "" & FBStrStatus4B != "" & SBStrStatus4B != "" & FBStrStatus5B != "" & SBStrStatus5B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass" & FBStrStatus5B == "Pass" & SBStrStatus5B == "Pass")
    //                    {
    //                        if (ResMo2 > 20)
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("4M"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));

    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "" & FBStrStatus4B != "" & SBStrStatus4B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass")
    //                    {
    //                        if (ResMo2 > 20)
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("3M") | Shapecode.Contains("FN10"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));

    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass")
    //                    {
    //                        if (ResMo2 > 20)
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("2M") | Shapecode.Contains("DF1"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));

    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass")
    //                    {
    //                        if (ResMo2 > 20)
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("1M") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));

    //                if (FBStrStatus != "" & SBStrStatus != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass")
    //                    {
    //                        if (ResMo2 > 20)
    //                            StrStatus = "Pass";
    //                        else
    //                            StrStatus = "Fail";
    //                    }
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //                else if (SBStrStatus != "")
    //                {
    //                    if (SBStrStatus == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //        return StrStatus;
    //    }

    //    public void FnStatusCo1(int Co1, int Co2, int A, int B, int C, int D, int E, int F, int intCWSpacing, string Shapecode, int intFactor)
    //    {
    //        try
    //        {
    //            if (Co1 < A)
    //            {
    //                intInterval = Math.Floor((A - Co1) / intCWSpacing);
    //                // intInterval = (intInterval)

    //                A1 = A - ((intInterval * intCWSpacing) + Co1);
    //                FB = A1 - intFactor;

    //                if (Shapecode.Contains("1CR") | Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2CR6") | Shapecode.Contains("2CR8") | Shapecode.Contains("2CR9") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("ES1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB24") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN6") | Shapecode.Contains("TS6") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("FN10") | Shapecode.Contains("TS6") | Shapecode.Contains("HM1"))
    //                {
    //                    if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                        FBStrStatus = "Pass";
    //                    else if (FB > strBendLengths[2])
    //                        FBStrStatus = "Pass";
    //                    else
    //                        FBStrStatus = "Fail";
    //                }
    //                else if (Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("FBR1") | Shapecode.Contains("2C6") | Shapecode.Contains("2C8") | Shapecode.Contains("2C9") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2"))
    //                {
    //                    if (FB > strBendLengths[0])
    //                        FBStrStatus = "Pass";
    //                    else
    //                        FBStrStatus = "Fail";
    //                }

    //                B1 = intCWSpacing - A1;
    //                SB = B1 + intFactor;

    //                if (Shapecode.Contains("1CR") | Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2CR6") | Shapecode.Contains("2CR8") | Shapecode.Contains("2CR9") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("ES1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN6") | Shapecode.Contains("TS6") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("FN10") | Shapecode.Contains("TS6") | Shapecode.Contains("HM1"))
    //                {
    //                    if (SB > strBendLengths[3])
    //                        SBStrStatus = "Pass";
    //                    else
    //                        SBStrStatus = "Fail";
    //                }
    //                else if (Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR7") | Shapecode.Contains("FBR1") | Shapecode.Contains("2C6") | Shapecode.Contains("2C8") | Shapecode.Contains("2C9") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2"))
    //                {
    //                    if (SB > strBendLengths[1])
    //                        SBStrStatus = "Pass";
    //                    else
    //                        SBStrStatus = "Fail";
    //                }

    //                if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2C6") | Shapecode.Contains("2C8") | Shapecode.Contains("2C9") | Shapecode.Contains("2CR6") | Shapecode.Contains("2CR8") | Shapecode.Contains("2CR9") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                {
    //                    intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                    B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                    FB1 = B2 - intFactor;

    //                    if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2C6") | Shapecode.Contains("2C8") | Shapecode.Contains("2C9") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                    {
    //                        if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            FBStrStatus2B = "Pass";
    //                        else if (FB1 > strBendLengths[2])
    //                            FBStrStatus2B = "Pass";
    //                        else
    //                            FBStrStatus2B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("FBR1") | Shapecode.Contains("2CR6") | Shapecode.Contains("2CR8") | Shapecode.Contains("2CR9") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("HM1"))
    //                    {
    //                        if (FB1 > strBendLengths[0])
    //                            FBStrStatus2B = "Pass";
    //                        else
    //                            FBStrStatus2B = "Fail";
    //                    }

    //                    C1 = intCWSpacing - B2;
    //                    SB1 = C1 + intFactor;

    //                    if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2C6") | Shapecode.Contains("2C8") | Shapecode.Contains("2C9") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                    {
    //                        if (SB1 > strBendLengths[3])
    //                            SBStrStatus2B = "Pass";
    //                        else
    //                            SBStrStatus2B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("FBR1") | Shapecode.Contains("2CR6") | Shapecode.Contains("2CR8") | Shapecode.Contains("2CR9") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("HM1"))
    //                    {
    //                        if (SB1 > strBendLengths[1])
    //                            SBStrStatus2B = "Pass";
    //                        else
    //                            SBStrStatus2B = "Fail";
    //                    }
    //                }

    //                if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("TS7") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9"))
    //                {
    //                    intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                    C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                    FB2 = C2 - intFactor;
    //                    if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10"))
    //                    {
    //                        if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            FBStrStatus3B = "Pass";
    //                        else if (FB2 > strBendLengths[2])
    //                            FBStrStatus3B = "Pass";
    //                        else
    //                            FBStrStatus3B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9"))
    //                    {
    //                        if (FB2 > strBendLengths[0])
    //                            FBStrStatus3B = "Pass";
    //                        else
    //                            FBStrStatus3B = "Fail";
    //                    }

    //                    D1 = intCWSpacing - C2;
    //                    SB2 = D1 + intFactor;

    //                    if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10"))
    //                    {
    //                        if (SB2 > strBendLengths[3])
    //                            SBStrStatus3B = "Pass";
    //                        else
    //                            SBStrStatus3B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9"))
    //                    {
    //                        if (SB2 > strBendLengths[1])
    //                            SBStrStatus3B = "Pass";
    //                        else
    //                            SBStrStatus3B = "Fail";
    //                    }
    //                }




    //                if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                    D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                    FB3 = D2 - intFactor;
    //                    if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                    {
    //                        if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                            FBStrStatus4B = "Pass";
    //                        else if (FB3 > strBendLengths[2])
    //                            FBStrStatus4B = "Pass";
    //                        else
    //                            FBStrStatus4B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                    {
    //                        if (FB3 > strBendLengths[0])
    //                            FBStrStatus4B = "Pass";
    //                        else
    //                            FBStrStatus4B = "Fail";
    //                    }

    //                    E1 = intCWSpacing - D2;
    //                    SB3 = E1 + intFactor;
    //                    if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                    {
    //                        if (SB3 > strBendLengths[3])
    //                            SBStrStatus4B = "Pass";
    //                        else
    //                            SBStrStatus4B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                    {
    //                        if (SB3 > strBendLengths[1])
    //                            SBStrStatus4B = "Pass";
    //                        else
    //                            SBStrStatus4B = "Fail";
    //                    }
    //                }

    //                if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                    E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                    FB4 = E2 - intFactor;
    //                    if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                    {
    //                        if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            FBStrStatus4B = "Pass";
    //                        else if (FB4 > strBendLengths[2])
    //                            FBStrStatus5B = "Pass";
    //                        else
    //                            FBStrStatus5B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                    {
    //                        if (FB4 > strBendLengths[0])
    //                            FBStrStatus5B = "Pass";
    //                        else
    //                            FBStrStatus5B = "Fail";
    //                    }

    //                    F1 = intCWSpacing - E2;
    //                    SB4 = F1 + intFactor;
    //                    if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                    {
    //                        if (SB4 > strBendLengths[3])
    //                            SBStrStatus5B = "Pass";
    //                        else
    //                            SBStrStatus5B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                    {
    //                        if (SB4 > strBendLengths[1])
    //                            SBStrStatus5B = "Pass";
    //                        else
    //                            SBStrStatus5B = "Fail";
    //                    }
    //                }
    //            }
    //            else if (Co1 > A)
    //            {
    //                B1 = Co1 - A;
    //                FB = B1 - intFactor;

    //                if (Shapecode.Contains("1CR") | Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("ES1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN6") | Shapecode.Contains("TS6") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6") | Shapecode.Contains("HM1"))
    //                {
    //                    if (SB > strBendLengths[3])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        FBStrStatus = "Fail";
    //                        SBStrStatus = "Fail";
    //                    }
    //                }
    //                else if (Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("FBR1") | Shapecode.Contains("HM1"))
    //                {
    //                    if (SB > strBendLengths[1])
    //                        SBStrStatus = "Pass";
    //                    else
    //                        SBStrStatus = "Fail";
    //                }


    //                if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("HM1"))
    //                {
    //                    intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                    B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                    FB1 = B2 - intFactor;

    //                    if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        FBStrStatus2B = "Pass";
    //                    else if (FB1 > strBendLengths[2])
    //                        FBStrStatus2B = "Pass";
    //                    else
    //                        FBStrStatus2B = "Fail";
    //                }
    //                else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("HM1"))
    //                {
    //                    if (FB1 > strBendLengths[0])
    //                        FBStrStatus2B = "Pass";
    //                    else
    //                        FBStrStatus2B = "Fail";
    //                }


    //                C1 = intCWSpacing - B2;
    //                SB1 = C1 + intFactor;

    //                if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("HM1"))
    //                {
    //                    if (SB1 > strBendLengths[3])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                        SBStrStatus2B = "Fail";
    //                }
    //                else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("HM1"))
    //                {
    //                    if (SB1 > strBendLengths[1])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                        SBStrStatus2B = "Fail";
    //                }

    //                if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                {
    //                    intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                    C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                    FB2 = C2 - intFactor;


    //                    if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                        FBStrStatus3B = "Pass";
    //                    else if (FB2 > strBendLengths[2])
    //                        FBStrStatus3B = "Pass";
    //                    else
    //                        FBStrStatus3B = "Fail";
    //                }
    //                else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("TS7"))
    //                {
    //                    if (FB2 > strBendLengths[0])
    //                        FBStrStatus3B = "Pass";
    //                    else
    //                        FBStrStatus3B = "Fail";
    //                }

    //                D1 = intCWSpacing - C2;
    //                SB2 = D1 + intFactor;

    //                if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                {
    //                    if (SB2 > strBendLengths[3])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                        SBStrStatus3B = "Fail";
    //                }
    //                else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB2 > strBendLengths[1])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                        SBStrStatus3B = "Fail";
    //                }


    //                if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                    D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                    FB3 = D2 - intFactor;
    //                    if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                        FBStrStatus4B = "Pass";
    //                    else if (FB3 > strBendLengths[2])
    //                        FBStrStatus4B = "Pass";
    //                    else
    //                        FBStrStatus4B = "Fail";
    //                }
    //                else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (FB3 > strBendLengths[0])
    //                        FBStrStatus4B = "Pass";
    //                    else
    //                        FBStrStatus4B = "Fail";
    //                }

    //                E1 = intCWSpacing - D2;
    //                SB3 = E1 + intFactor;
    //                if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB3 > strBendLengths[3])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                        SBStrStatus4B = "Fail";
    //                }
    //                else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB3 > strBendLengths[1])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                        SBStrStatus4B = "Fail";
    //                }

    //                if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                    E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                    FB4 = E2 - intFactor;
    //                    if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                        FBStrStatus5B = "Pass";
    //                    else if (FB4 > strBendLengths[2])
    //                        FBStrStatus5B = "Pass";
    //                    else
    //                        FBStrStatus5B = "Fail";
    //                }
    //                else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (FB4 > strBendLengths[0])
    //                        FBStrStatus5B = "Pass";
    //                    else
    //                        FBStrStatus5B = "Fail";
    //                }

    //                F1 = intCWSpacing - E2;
    //                SB4 = F1 + intFactor;
    //                if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB4 > strBendLengths[3])
    //                        SBStrStatus5B = "Pass";
    //                    else
    //                        SBStrStatus5B = "Fail";
    //                }
    //                else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB4 > strBendLengths[1])
    //                        SBStrStatus5B = "Pass";
    //                    else
    //                        SBStrStatus5B = "Fail";
    //                }
    //            }
    //            else if (((intInterval * intCWSpacing) + Co1) == A)
    //            {
    //                FBStrStatus = "Fail";
    //                SBStrStatus = "Fail";
    //                FBStrStatus2B = "Fail";
    //                SBStrStatus2B = "Fail";
    //                FBStrStatus3B = "Fail";
    //                SBStrStatus3B = "Fail";
    //                FBStrStatus4B = "Fail";
    //                SBStrStatus4B = "Fail";
    //                FBStrStatus5B = "Fail";
    //                SBStrStatus5B = "Fail";
    //            }

    //            if (Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus4B != "" & FBStrStatus4B != "" & SBStrStatus5B != "" & FBStrStatus5B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass" & FBStrStatus5B == "Pass" & SBStrStatus5B == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("4C"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "" & FBStrStatus4B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("3C"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("2C"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("1C") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("TS6"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //                else if (SBStrStatus == "")
    //                {
    //                    if (FBStrStatus == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //        return StrStatus;
    //    }

    //    public void FnStatusSpaceshiftMo1(int Mo1, int Mo2, int A, int B, int C, int D, int E, int F, int G, int intCWSpacing, string Shapecode, int intFactor)
    //    {
    //        try
    //        {
    //            string strSpace1M = "";
    //            string strSpace2M = "";
    //            string strSpace3M = "";
    //            string strSpace4M = "";
    //            string strSpace5M = "";
    //            string strSpace6M = "";
    //            string strSpace7M = "";




    //            strspaceCo1 = "0";
    //            if (Mo1 < A)
    //            {
    //                intInterval = Math.Floor((A - Mo1) / intCWSpacing);
    //                A1 = A - ((intInterval * intCWSpacing) + Mo1);
    //                B1 = intCWSpacing - A1;
    //                intInterval2B = System.Convert.ToInt32(Math.Floor((B - B1) / intCWSpacing));
    //                B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                C1 = intCWSpacing - B2;
    //                intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                D1 = intCWSpacing - C2;
    //                intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                E1 = intCWSpacing - D2;
    //                intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                F1 = intCWSpacing - E2;
    //                F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                G1 = intCWSpacing - F2;

    //                // intInterval = (intInterval)

    //                if (Shapecode.Contains("1MR") | Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("FBR1") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("TS10") | Shapecode.Contains("HM1"))
    //                {
    //                    A1 = A - ((intInterval * intCWSpacing) + Mo1);
    //                    if (A1 == 0)
    //                    {
    //                        A1 = intCWSpacing;
    //                        B1 = B1 + 20;
    //                    }
    //                    else
    //                    {
    //                        A1 = A1;
    //                        B1 = intCWSpacing - A1;
    //                    }


    //                    FB = A1 - intFactor;
    //                    SB = B1 + intFactor;

    //                    if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                        FBStrStatus = "Pass";
    //                    else if (FB > strBendLengths[2])
    //                        FBStrStatus = "Pass";
    //                    else if (intCWSpacing > 300)
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1M"))
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;


    //                        intNumWire = CW_A + CW_B;
    //                        // If intfailcount = 0 Then
    //                        A1 = A1 + 20;
    //                        FB = A1 - intFactor;
    //                        if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                            FBStrStatus = "Pass";
    //                        else if (FB > strBendLengths[2])
    //                        {
    //                            FBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1M"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            strspace = strspace + ";" + System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1M"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B;
    //                            A1 = A1 + 50;
    //                            FB = A1 - intFactor;
    //                            if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                                FBStrStatus = "Pass";
    //                            else if (FB > strBendLengths[2])
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("1M"))
    //                                    CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus = "Fail";
    //                        }
    //                    }
    //                    else
    //                        FBStrStatus = "Fail";
    //                }
    //                else if (Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DF1") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6") | Shapecode.Contains("HM1"))
    //                {
    //                    A1 = A - ((intInterval * intCWSpacing) + Mo1);
    //                    if (A1 == 0)
    //                        A1 = intCWSpacing;
    //                    else
    //                        A1 = A1;
    //                    B1 = intCWSpacing - A1;
    //                    if (B1 == 0)
    //                        B1 = B1 + 20;
    //                    FB = A1 - intFactor;
    //                    SB = B1 + intFactor;
    //                    if (FB > strBendLengths[0])
    //                        FBStrStatus = "Pass";
    //                    else if (intCWSpacing > 300)
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1M"))
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B;

    //                        A1 = A1 + 20;
    //                        FB = A1 - intFactor;
    //                        if (FB > strBendLengths[0])
    //                        {
    //                            FBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1M"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            intfailcount = intfailcount + 1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1M"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1M"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            A1 = A1 + 50;
    //                            B1 = B1;
    //                            FB = A1 - intFactor;
    //                            if (FB > strBendLengths[0])
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("1M"))
    //                                    CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus = "Fail";
    //                        }
    //                    }
    //                    else
    //                        FBStrStatus = "Fail";
    //                }

    //                SB = B1 + intFactor;

    //                if (Shapecode.Contains("1MR") | Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("FBR1") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("HM1"))
    //                {
    //                    B1 = intCWSpacing - A1;
    //                    if (B1 == 0)
    //                        B1 = B1 + 20;
    //                    intInterval2B = System.Convert.ToInt32(Math.Floor((B - B1) / intCWSpacing));

    //                    if (SB > strBendLengths[3])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1M"))
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B;
    //                        // If intfailcount = 0 Then
    //                        B1 = B1 + 20;
    //                        SB = B1 + intFactor;
    //                        if (SB > strBendLengths[3])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1M"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            intfailcount = intfailcount + 1;
    //                            B1 = B1 + 50;
    //                            SB = B1 + intFactor;
    //                            if (SB > strBendLengths[3])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("1M"))
    //                                    CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DF1") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6") | Shapecode.Contains("HM1"))
    //                {
    //                    B1 = intCWSpacing - A1;

    //                    intInterval2B = System.Convert.ToInt32(Math.Floor((B - B1) / intCWSpacing));

    //                    if (SB > strBendLengths[1])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1M"))
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B;
    //                        // If intfailcount = 0 Then
    //                        B1 = B1 + 20;
    //                        intInterval2B = System.Convert.ToInt32(Math.Floor((B - B1) / intCWSpacing));
    //                        SB = B1 + intFactor;
    //                        if (SB > strBendLengths[1])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1M"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            intfailcount = intfailcount + 1;
    //                            B1 = B1 + 50;
    //                            intInterval2B = System.Convert.ToInt32(Math.Floor((B - B1) / intCWSpacing));
    //                            SB = B1 + intFactor;
    //                            if (SB > strBendLengths[1])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("1M"))
    //                                    CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus = "Fail";
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("DF5") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2M6") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6"))
    //                {
    //                    B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                    C1 = intCWSpacing - B2;

    //                    FB1 = B2 - intFactor;

    //                    if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("FBR1") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8"))
    //                    {
    //                        if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            FBStrStatus2B = "Pass";
    //                        else if (FB1 > strBendLengths[2])
    //                            FBStrStatus2B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            // If intfailcount = 0 Then
    //                            B2 = B2 + 20;
    //                            FB1 = B2 - intFactor;
    //                            if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB1 > strBendLengths[2])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                B2 = B2 + 50;
    //                                FB1 = B2 - intFactor;
    //                                if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB1 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus2B = "Fail";
    //                            }
    //                        }
    //                        else
    //                            FBStrStatus2B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6"))
    //                    {
    //                        B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                        C1 = intCWSpacing - B2;

    //                        if (FB1 > strBendLengths[0])
    //                            FBStrStatus2B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C;

    //                            // If intfailcount = 0 Then
    //                            B2 = B2 + 20;
    //                            FB1 = B2 - intFactor;
    //                            if (FB1 > strBendLengths[0])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                B2 = B2 + 50;
    //                                C1 = C1;
    //                                FB1 = B2 - intFactor;
    //                                if (FB1 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus2B = "Fail";
    //                            }
    //                        }
    //                        else
    //                            FBStrStatus2B = "Fail";
    //                    }

    //                    C1 = intCWSpacing - B2;
    //                    SB1 = C1 + intFactor;

    //                    if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("FBR1") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8"))
    //                    {
    //                        if (SB1 > strBendLengths[3])
    //                            SBStrStatus2B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            // If intfailcount = 0 Then
    //                            C1 = C1 + 20;
    //                            SB1 = C1 + intFactor;
    //                            if (SB1 > strBendLengths[3])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                C1 = C1 + 50;
    //                                SB1 = C1 + intFactor;
    //                                if (SB1 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    SBStrStatus2B = "Fail";
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6"))
    //                    {
    //                        C1 = intCWSpacing - B2;

    //                        if (SB1 > strBendLengths[1])
    //                            SBStrStatus2B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            // If intfailcount = 0 Then
    //                            C1 = C1 + 20;
    //                            SB1 = C1 + intFactor;
    //                            if (SB1 > strBendLengths[1])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                C1 = C1 + 50;
    //                                SB1 = C1 + intFactor;
    //                                if (SB1 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    SBStrStatus2B = "Fail";
    //                            }
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6"))
    //                {
    //                    intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                    C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                    FB2 = C2 - intFactor;
    //                    if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25"))
    //                    {
    //                        if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            FBStrStatus3B = "Pass";
    //                        else if (FB2 > strBendLengths[2])
    //                            FBStrStatus3B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3M"))
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            C2 = C2 + 20;
    //                            FB2 = C2 - intFactor;
    //                            if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3M"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB2 > strBendLengths[2])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3M"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                C2 = C2 + 50;
    //                                FB2 = C2 - intFactor;
    //                                if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("3M"))
    //                                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB2 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("3M"))
    //                                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus3B = "Fail";
    //                            }
    //                        }
    //                        else
    //                            FBStrStatus3B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (FB2 > strBendLengths[0])
    //                            FBStrStatus3B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3M"))
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            C2 = C2 + 20;
    //                            FB2 = C2 - intFactor;
    //                            if (FB2 > strBendLengths[0])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3M"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                C2 = C2 + 50;
    //                                FB2 = C2 - intFactor;
    //                                if (FB2 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("3M"))
    //                                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus3B = "Fail";
    //                            }
    //                        }
    //                        else
    //                            FBStrStatus3B = "Fail";
    //                    }

    //                    D1 = intCWSpacing - C2;
    //                    SB2 = D1 + intFactor;


    //                    if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25"))
    //                    {
    //                        if (SB2 > strBendLengths[3])
    //                            SBStrStatus3B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            C2 = C2 + 20;
    //                            SB2 = C2 + intFactor;
    //                            if (SB2 > strBendLengths[3])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3M"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                C2 = C2 + 50;
    //                                SB2 = C2 + intFactor;
    //                                if (SB2 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("3M"))
    //                                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    SBStrStatus3B = "Fail";
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (SB2 > strBendLengths[1])
    //                            SBStrStatus3B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3M"))
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            C2 = C2 + 20;
    //                            SB2 = C2 + intFactor;
    //                            if (SB2 > strBendLengths[1])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3M"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                C2 = C2 + 50;
    //                                SB2 = C2 + intFactor;
    //                                if (SB2 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("3M"))
    //                                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    SBStrStatus3B = "Fail";
    //                            }
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TS6"))
    //                {
    //                    intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                    D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                    FB3 = D2 - intFactor;

    //                    if (Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26"))
    //                    {
    //                        if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                            FBStrStatus4B = "Pass";
    //                        else if (FB3 > strBendLengths[2])
    //                            FBStrStatus4B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4M"))
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            D2 = D2 + 20;
    //                            FB3 = D2 - intFactor;
    //                            if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4M"))
    //                                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB3 > strBendLengths[2])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4M"))
    //                                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                D2 = D2 + 50;
    //                                FB3 = D2 - intFactor;
    //                                if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("4M"))
    //                                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB3 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("4M"))
    //                                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus4B = "Fail";
    //                            }
    //                        }
    //                        else
    //                            FBStrStatus4B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (FB3 > strBendLengths[0])
    //                            FBStrStatus4B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4M"))
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            // If intfailcount = 0 Then
    //                            D2 = D2 + 20;
    //                            FB3 = D2 - intFactor;
    //                            if (FB3 > strBendLengths[0])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4M"))
    //                                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                D2 = D2 + 50;
    //                                FB3 = D2 - intFactor;
    //                                if (FB3 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("4M"))
    //                                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus4B = "Fail";
    //                            }
    //                        }
    //                        else
    //                            FBStrStatus4B = "Fail";
    //                    }

    //                    E1 = intCWSpacing - D2;
    //                    SB3 = E1 + intFactor;
    //                    if (Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26"))
    //                    {
    //                        if (SB3 > strBendLengths[3])
    //                            SBStrStatus4B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4M"))
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            // If intfailcount = 0 Then
    //                            D2 = D2 + 20;
    //                            SB3 = D2 + intFactor;
    //                            if (SB3 > strBendLengths[3])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4M"))
    //                                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                D2 = D2 + 20;
    //                                SB3 = D2 + intFactor;
    //                                if (SB3 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("4M"))
    //                                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    SBStrStatus4B = "Fail";
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (SB3 > strBendLengths[1])
    //                            SBStrStatus4B = "Pass";
    //                        else
    //                        {
    //                            CW_A = System.Convert.ToInt32(Math.Round((A - Mo1) / intCWSpacing) + 1);
    //                            CW_B = System.Convert.ToInt32(Math.Round((B - (B1 + B2)) / intCWSpacing) + 1);
    //                            CW_C = System.Convert.ToInt32(Math.Round((C - (C1 + C2)) / intCWSpacing) + 1);
    //                            CW_D = System.Convert.ToInt32(Math.Round((D - (D1 + D2)) / intCWSpacing) + 1);
    //                            if (Shapecode.Contains("4M"))
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            // If intfailcount = 0 Then
    //                            D2 = D2 + 20;
    //                            SB3 = D2 + intFactor;
    //                            if (SB3 > strBendLengths[1])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4M"))
    //                                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                D2 = D2 + 50;
    //                                SB3 = D2 + intFactor;
    //                                if (SB3 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("4M"))
    //                                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    SBStrStatus4B = "Fail";
    //                            }
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7") | Shapecode.Contains("TS6"))
    //                {
    //                    intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                    E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                    FB4 = E2 - intFactor;
    //                    if (Shapecode.Contains("5MR"))
    //                    {
    //                        if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            FBStrStatus5B = "Pass";
    //                        else if (FB4 > strBendLengths[2])
    //                            FBStrStatus5B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;


    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            // If intfailcount = 0 Then
    //                            E2 = E2 + 20;
    //                            FB4 = E2 - intFactor;
    //                            if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB4 > strBendLengths[2])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                E2 = E2 + 50;
    //                                FB4 = E2 - intFactor;
    //                                if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB4 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus5B = "Fail";
    //                            }
    //                        }
    //                        else
    //                            FBStrStatus5B = "Fail";
    //                    }
    //                    else if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (FB4 > strBendLengths[0])
    //                            FBStrStatus5B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            // If intfailcount = 0 Then
    //                            E2 = E2 + 20;
    //                            FB4 = E2 - intFactor;
    //                            if (FB4 > strBendLengths[0])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB4 > strBendLengths[0])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                E2 = E2 + 50;
    //                                FB4 = E2 - intFactor;
    //                                if (FB4 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB4 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus5B = "Fail";
    //                            }
    //                        }
    //                        else
    //                            FBStrStatus5B = "Fail";
    //                    }

    //                    F1 = intCWSpacing - E2;
    //                    SB4 = F1 + intFactor;
    //                    if (Shapecode.Contains("5MR"))
    //                    {
    //                        if (SB4 > strBendLengths[3])
    //                            SBStrStatus5B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            // If intfailcount = 0 Then
    //                            F1 = F1 + 20;
    //                            SB4 = F1 + intFactor;
    //                            if (SB4 > strBendLengths[3])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                F1 = F1 + 50;
    //                                SB4 = F1 + intFactor;
    //                                if (SB4 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    SBStrStatus5B = "Fail";
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7"))
    //                    {
    //                        if (SB4 > strBendLengths[1])
    //                            SBStrStatus5B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            // If intfailcount = 0 Then
    //                            F1 = F1 + 20;
    //                            SB4 = F1 + intFactor;
    //                            if (SB4 > strBendLengths[1])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                F1 = F1 + 50;
    //                                SB4 = F1 + intFactor;
    //                                if (SB4 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    SBStrStatus5B = "Fail";
    //                            }
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("TS6"))
    //                {
    //                    if (FB5 > strBendLengths[0])
    //                        FBStrStatus6B = "Pass";
    //                    else if (intCWSpacing > 300)
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                        CW_G = Math.Floor((G - (G1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F + CW_G;
    //                        // If intfailcount = 0 Then
    //                        F2 = F2 + 20;
    //                        FB5 = F2 + intFactor;
    //                        if (FB5 > strBendLengths[0])
    //                        {
    //                            FBStrStatus6B = "Pass";
    //                            strSpaceLength = F2 + G1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            CW_G = Math.Floor((G - (G1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F + CW_G;
    //                            strSpace6M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E + CW_F) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB5 > strBendLengths[0])
    //                        {
    //                            FBStrStatus6B = "Pass";
    //                            strSpaceLength = F2 + G1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            CW_G = Math.Floor((G - (G1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F + CW_G;
    //                            strSpace6M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E + CW_F) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            F2 = F2 + 50;
    //                            FB5 = F2 + intFactor;
    //                            if (FB5 > strBendLengths[0])
    //                            {
    //                                FBStrStatus6B = "Pass";
    //                                strSpaceLength = F2 + G1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_G = Math.Floor((G - (G1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F + CW_G;
    //                                strSpace6M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E + CW_F) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB5 > strBendLengths[0])
    //                            {
    //                                FBStrStatus6B = "Pass";
    //                                strSpaceLength = F2 + G1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F + CW_G;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E + CW_F) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus5B = "Fail";
    //                        }
    //                    }
    //                    else
    //                        FBStrStatus5B = "Fail";
    //                }
    //            }
    //            else if (Mo1 > A)
    //            {
    //                B1 = Mo1 - A;
    //                SB = B1 + intFactor;

    //                if (Shapecode.Contains("1MR") | Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("FBR1") | Shapecode.Contains("DS10"))
    //                {
    //                    if (SB > strBendLengths[3])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B;
    //                        B1 = B1 + 20;
    //                        SB = SB == B1 + intFactor;
    //                        if (SB > strBendLengths[3])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            B1 = B1 + 50;
    //                            SB = SB == B1 + intFactor;
    //                            if (SB > strBendLengths[3])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7"))
    //                {
    //                    if (SB > strBendLengths[1])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B;
    //                        B1 = B1 + 20;
    //                        SB = SB == B1 + intFactor;
    //                        if (SB > strBendLengths[1])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            B1 = B1 + 50;
    //                            SB = SB == B1 + intFactor;
    //                            if (SB > strBendLengths[1])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus = "Fail";
    //                        }
    //                    }
    //                }


    //                if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("FBR1"))
    //                {
    //                    intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                    B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                    FB1 = B2 - intFactor;

    //                    if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        FBStrStatus2B = "Pass";
    //                    else if (FB1 > strBendLengths[2])
    //                        FBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2M"))
    //                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B + CW_C;
    //                        B2 = B2 + 20;
    //                        FB1 = B2 - intFactor;
    //                        if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB1 > strBendLengths[2])
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            B2 = B2 + 50;
    //                            FB1 = B2 - intFactor;
    //                            if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB1 > strBendLengths[2])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus2B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7"))
    //                {
    //                    if (FB1 > strBendLengths[0])
    //                        FBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2M"))
    //                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B + CW_C;
    //                        B2 = B2 + 20;
    //                        FB1 = B2 - intFactor;
    //                        if (FB1 > strBendLengths[0])
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            B2 = B2 + 50;
    //                            FB1 = B2 - intFactor;
    //                            if (FB1 > strBendLengths[0])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus2B = "Fail";
    //                        }
    //                    }
    //                }


    //                C1 = intCWSpacing - B2;
    //                SB1 = C1 + intFactor;

    //                if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("FBR1"))
    //                {
    //                    if (SB1 > strBendLengths[3])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2M"))
    //                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B + CW_C;
    //                        C1 = C1 + 20;
    //                        SB1 = C1 + intFactor;
    //                        if (SB1 > strBendLengths[3])
    //                        {
    //                            SBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C1 = C1 + 50;
    //                            SB1 = C1 + intFactor;
    //                            if (SB1 > strBendLengths[3])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus2B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7"))
    //                {
    //                    if (SB1 > strBendLengths[1])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2M"))
    //                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B + CW_C;
    //                        C1 = C1 + 20;
    //                        SB1 = C1 + intFactor;
    //                        if (SB1 > strBendLengths[1])
    //                        {
    //                            SBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C1 = C1 + 50;
    //                            SB1 = C1 + intFactor;
    //                            if (SB1 > strBendLengths[1])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus2B = "Fail";
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR"))
    //                {
    //                    intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                    C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                    FB2 = C2 - intFactor;


    //                    if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                        FBStrStatus3B = "Pass";
    //                    else if (FB2 > strBendLengths[2])
    //                        FBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3M"))
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        C2 = C2 + 20;
    //                        FB2 = C2 - intFactor;

    //                        if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB2 > strBendLengths[2])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C2 = C2 + 50;
    //                            FB2 = C2 - intFactor;

    //                            if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB2 > strBendLengths[2])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7"))
    //                {
    //                    if (FB2 > strBendLengths[0])
    //                        FBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3M"))
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        C2 = C2 + 20;
    //                        FB2 = C2 - intFactor;
    //                        if (FB2 > strBendLengths[0])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C2 = C2 + 50;
    //                            FB2 = C2 - intFactor;
    //                            if (FB2 > strBendLengths[0])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                }

    //                D1 = intCWSpacing - C2;
    //                SB2 = D1 + intFactor;

    //                if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR"))
    //                {
    //                    if (SB2 > strBendLengths[3])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3M"))
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        D1 = D1 + 20;
    //                        SB2 = D1 + intFactor;
    //                        if (SB2 > strBendLengths[3])
    //                        {
    //                            SBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D1 = D1 + 50;
    //                            SB2 = D1 + intFactor;
    //                            if (SB2 > strBendLengths[3])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7"))
    //                {
    //                    if (SB2 > strBendLengths[1])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3M"))
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;


    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        D1 = D1 + 20;
    //                        SB2 = D1 + intFactor;
    //                        if (SB2 > strBendLengths[1])
    //                        {
    //                            SBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D1 = D1 + 50;
    //                            SB2 = D1 + intFactor;
    //                            if (SB2 > strBendLengths[1])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                }


    //                if (Shapecode.Contains("4MR") | Shapecode.Contains("5MR"))
    //                {
    //                    intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                    D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                    FB3 = D2 - intFactor;
    //                    if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        FBStrStatus4B = "Pass";
    //                    else if (FB1 > strBendLengths[2])
    //                        FBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                        D2 = D2 + 20;
    //                        FB3 = D2 - intFactor;
    //                        if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB1 > strBendLengths[2])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D2 = D2 + 50;
    //                            FB3 = D2 - intFactor;
    //                            if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB1 > strBendLengths[2])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7"))
    //                {
    //                    if (FB2 > strBendLengths[0])
    //                        FBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                        D2 = D2 + 20;
    //                        FB3 = D2 - intFactor;
    //                        if (FB2 > strBendLengths[0])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D2 = D2 + 50;
    //                            FB3 = D2 - intFactor;
    //                            if (FB2 > strBendLengths[0])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }

    //                E1 = intCWSpacing - D2;
    //                SB3 = E1 + intFactor;
    //                if (Shapecode.Contains("4MR") | Shapecode.Contains("5MR"))
    //                {
    //                    if (SB3 > strBendLengths[3])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                        E1 = E1 + 20;
    //                        SB3 = E1 + intFactor;
    //                        if (SB3 > strBendLengths[3])
    //                        {
    //                            SBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E1 = E1 + 50;
    //                            SB3 = E1 + intFactor;
    //                            if (SB3 > strBendLengths[3])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7"))
    //                {
    //                    if (SB3 > strBendLengths[1])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                        E1 = E1 + 20;
    //                        SB3 = E1 + intFactor;
    //                        if (SB3 > strBendLengths[1])
    //                        {
    //                            SBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E1 = E1 + 50;
    //                            SB3 = E1 + intFactor;
    //                            if (SB3 > strBendLengths[1])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("5MR"))
    //                {
    //                    intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                    E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                    FB4 = E2 - intFactor;
    //                    if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                        FBStrStatus5B = "Pass";
    //                    else if (FB4 > strBendLengths[2])
    //                        FBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                        E2 = E2 + 20;
    //                        FB4 = E2 - intFactor;
    //                        if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB4 > strBendLengths[2])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E2 = E2 + 50;
    //                            FB4 = E2 - intFactor;
    //                            if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB1 > strBendLengths[2])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7"))
    //                {
    //                    if (FB4 > strBendLengths[0])
    //                        FBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                        E2 = E2 + 20;
    //                        FB4 = E2 - intFactor;
    //                        if (FB4 > strBendLengths[0])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E2 = E2 + 50;
    //                            FB4 = E2 - intFactor;
    //                            if (FB4 > strBendLengths[0])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }

    //                F1 = intCWSpacing - E2;
    //                SB4 = F1 + intFactor;
    //                if (Shapecode.Contains("5MR"))
    //                {
    //                    if (SB3 > strBendLengths[3])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                        F1 = F1 + 20;
    //                        SB4 = F1 + intFactor;
    //                        if (SB4 > strBendLengths[3])
    //                        {
    //                            SBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            F1 = F1 + 50;
    //                            SB4 = F1 + intFactor;
    //                            if (SB4 > strBendLengths[3])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus5B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7"))
    //                {
    //                    if (SB3 > strBendLengths[1])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                        F1 = F1 + 20;
    //                        SB4 = F1 + intFactor;
    //                        if (SB4 > strBendLengths[1])
    //                        {
    //                            SBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            F1 = F1 + 50;
    //                            SB4 = F1 + intFactor;
    //                            if (SB4 > strBendLengths[1])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }
    //            }
    //            else if (((intInterval * intCWSpacing) + Mo1) == A)
    //            {
    //                FBStrStatus = "Fail" + "," + strspace;
    //                SBStrStatus = "Fail" + "," + strspace;
    //                FBStrStatus2B = "Fail" + "," + strspace;
    //                SBStrStatus2B = "Fail" + "," + strspace;
    //                FBStrStatus3B = "Fail" + "," + strspace;
    //                SBStrStatus3B = "Fail" + "," + strspace;
    //                FBStrStatus4B = "Fail" + "," + strspace;
    //                SBStrStatus4B = "Fail" + "," + strspace;
    //                FBStrStatus5B = "Fail" + "," + strspace;
    //                SBStrStatus5B = "Fail" + "," + strspace;
    //                FBStrStatus6B = "Fail" + "," + strspace;
    //                SBStrStatus6B = "Fail" + "," + strspace;
    //            }
    //            else
    //            {
    //                FBStrStatus = "Fail" + "," + strspace;
    //                SBStrStatus = "Fail" + "," + strspace;
    //                FBStrStatus2B = "Fail" + "," + strspace;
    //                SBStrStatus2B = "Fail" + "," + strspace;
    //                FBStrStatus3B = "Fail" + "," + strspace;
    //                SBStrStatus3B = "Fail" + "," + strspace;
    //                FBStrStatus4B = "Fail" + "," + strspace;
    //                SBStrStatus4B = "Fail" + "," + strspace;
    //                FBStrStatus5B = "Fail" + "," + strspace;
    //                SBStrStatus5B = "Fail" + "," + strspace;
    //                FBStrStatus6B = "Fail" + "," + strspace;
    //                SBStrStatus6B = "Fail" + "," + strspace;
    //            }

    //            strspace = Interaction.IIf(strSpace1M != string.Empty, strSpace1M + ";", "") + Interaction.IIf(strSpace2M != string.Empty, strSpace2M + ";", "") + Interaction.IIf(strSpace3M != string.Empty, strSpace3M + ";", "") + Interaction.IIf(strSpace4M != string.Empty, strSpace4M + ";", "") + Interaction.IIf(strSpace5M != string.Empty, strSpace5M + ";", "") + Interaction.IIf(strSpace6M != string.Empty, strSpace6M + ";", "");
    //            strspace = strspace.Trim(";");


    //            if (Shapecode.Contains("TS6"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;
    //                CW_G = Math.Floor((G - (G1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F + CW_G;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));

    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "" & FBStrStatus4B != "" & SBStrStatus4B != "" & FBStrStatus5B != "" & SBStrStatus5B != "" & FBStrStatus6B != "" & SBStrStatus6B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass" & FBStrStatus5B == "Pass" & SBStrStatus5B == "Pass" & FBStrStatus6B == "Pass" & SBStrStatus6B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //            }
    //            else if (Shapecode.Contains("5M") | Shapecode.Contains("TS7"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;

    //                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));

    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "" & FBStrStatus4B != "" & SBStrStatus4B != "" & FBStrStatus4B != "" & SBStrStatus4B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass" & FBStrStatus5B == "Pass" & SBStrStatus5B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //            }
    //            else if (Shapecode.Contains("4M"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;

    //                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "" & FBStrStatus4B != "" & SBStrStatus4B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //            }
    //            else if (Shapecode.Contains("3M"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));

    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //            }
    //            else if (Shapecode.Contains("2M") | Shapecode.Contains("DF1") | Shapecode.Contains("DF5"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));

    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //            }
    //            else if (Shapecode.Contains("1M") | Shapecode.Contains("DS1") | Shapecode.Contains("DS4"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));

    //                if (FBStrStatus != "" & SBStrStatus != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //                else if (SBStrStatus != "")
    //                {
    //                    if (SBStrStatus == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //        return StrStatus;
    //    }

    //    public void FnStatusSpaceShiftCo1(int Co1, int A, int B, int C, int D, int E, int F, int intCWSpacing, string Shapecode, int intFactor)
    //    {
    //        try
    //        {
    //            string strSpaceC1M = "";
    //            string strSpaceC2M = "";
    //            string strSpaceC3M = "";
    //            string strSpaceC4M = "";
    //            string strSpaceC5M = "";
    //            string strSpaceC6M = "";
    //            string strSpaceC7M = "";
    //            FBStrStatus = "";
    //            SBStrStatus = "";
    //            if (Co1 < A)
    //            {
    //                intInterval = Math.Floor((A - Co1) / intCWSpacing);
    //                // intInterval = (intInterval)

    //                A1 = A - ((intInterval * intCWSpacing) + Co1);
    //                FB = A1 - intFactor;

    //                if (Shapecode.Contains("1CR") | Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2CR6") | Shapecode.Contains("2CR8") | Shapecode.Contains("2CR9") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("ES1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN6") | Shapecode.Contains("TS6") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25"))
    //                {
    //                    if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                        FBStrStatus = "Pass";
    //                    else if (FB > strBendLengths[2])
    //                        FBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        B1 = intCWSpacing - A1;
    //                        intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                        B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'

    //                        intNumWire = CW_A + CW_B;
    //                        A1 = A1 + 20;
    //                        FB = A1 - intFactor;
    //                        // Calculation start'
    //                        B1 = intCWSpacing - A1;
    //                        intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                        B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                   // Calculation End'
    //                        if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                        {
    //                            FBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB > strBendLengths[2])
    //                        {
    //                            FBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            A1 = A1 + 50;
    //                            FB = A1 - intFactor;
    //                            // Calculation start'
    //                            B1 = intCWSpacing - A1;
    //                            intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                            B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                       // Calculation End '
    //                            if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB > strBendLengths[2])
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus = "Fail"; // End If A1+ 50 
    //                        } // End If A1+ 20 
    //                    } // End If FB>
    //                }
    //                else if (Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("FBR1") | Shapecode.Contains("2C6") | Shapecode.Contains("2C8") | Shapecode.Contains("2C9") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2"))
    //                {
    //                    if (FB > strBendLengths[0])
    //                        FBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B;
    //                        A1 = A1 + 20;
    //                        FB = A1 - intFactor;
    //                        // -------Calculation start--------'
    //                        B1 = intCWSpacing - A1;
    //                        intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                        B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                   // -------Calculation End----------'
    //                        if (FB > strBendLengths[0])
    //                        {
    //                            FBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            A1 = A1 + 50;
    //                            FB = A1 - intFactor;
    //                            // -------Calculation start--------'
    //                            B1 = intCWSpacing - A1;
    //                            intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                            B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                       // -------Calculation End----------'
    //                            if (FB > strBendLengths[0])
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus = "Fail"; // END if A1 + 50 
    //                        } // END if A1 + 20 
    //                    } // End if FB > 
    //                } // END If Shape 1CR 

    //                B1 = intCWSpacing - A1;
    //                SB = B1 + intFactor;

    //                if (Shapecode.Contains("1CR") | Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2CR6") | Shapecode.Contains("2CR8") | Shapecode.Contains("2CR9") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("ES1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN6") | Shapecode.Contains("TS6") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25"))
    //                {
    //                    if (SB > strBendLengths[3])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B;
    //                        B1 = B1 + 20;
    //                        SB = B1 + intFactor;
    //                        // -------Calculation start--------'

    //                        intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                        B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                   // -------Calculation End----------'  
    //                        if (SB > strBendLengths[3])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            B1 = B1 + 20;
    //                            SB = B1 + intFactor;
    //                            // -------Calculation start--------'
    //                            intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                            B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                       // -------Calculation End----------'
    //                            if (SB > strBendLengths[3])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR7") | Shapecode.Contains("FBR1") | Shapecode.Contains("2C6") | Shapecode.Contains("2C8") | Shapecode.Contains("2C9") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2"))
    //                {
    //                    if (SB > strBendLengths[1])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B;
    //                        B1 = B1 + 20;
    //                        SB = B1 + intFactor;
    //                        // -------Calculation start--------'
    //                        intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                        B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                   // -------Calculation End----------'
    //                        if (SB > strBendLengths[1])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            B1 = B1 + 50;
    //                            SB = B1 + intFactor;
    //                            // -------Calculation start--------'
    //                            intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                            B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                       // -------Calculation End----------'
    //                            if (SB > strBendLengths[1])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus = "Fail";
    //                        }
    //                    }
    //                } // END If Shape '1cR


    //                if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2C6") | Shapecode.Contains("2C8") | Shapecode.Contains("2C9") | Shapecode.Contains("2CR6") | Shapecode.Contains("2CR8") | Shapecode.Contains("2CR9") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                {
    //                    intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                    B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                    FB1 = B2 - intFactor;

    //                    if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2C6") | Shapecode.Contains("2C8") | Shapecode.Contains("2C9") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("HM1") | Shapecode.Contains("HM2") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                    {
    //                        if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            FBStrStatus2B = "Pass";
    //                        else if (FB1 > strBendLengths[2])
    //                            FBStrStatus2B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2C"))
    //                                CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;


    //                            B2 = B2 + 20;
    //                            FB1 = B2 - intFactor;
    //                            // -------Calculation start--------'
    //                            C1 = intCWSpacing - B2;
    //                            intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                            C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2C"))
    //                                CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            // -------Calculation End----------'
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB1 > strBendLengths[2])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                B2 = B2 + 50;
    //                                FB1 = B2 - intFactor;
    //                                // -------Calculation start--------'
    //                                C1 = intCWSpacing - B2;
    //                                intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                                C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                                CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2C"))
    //                                    CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                // -------Calculation End----------'
    //                                if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB1 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus2B = "Fail";
    //                            }
    //                        } // END FB >
    //                    }
    //                    else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("FBR1") | Shapecode.Contains("2CR6") | Shapecode.Contains("2CR8") | Shapecode.Contains("2CR9") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                    {
    //                        if (FB1 > strBendLengths[0])
    //                            FBStrStatus2B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2C"))
    //                                CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;


    //                            B2 = B2 + 20;
    //                            FB1 = B2 - intFactor;
    //                            // -------Calculation start--------'
    //                            C1 = intCWSpacing - B2;
    //                            intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                            C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2C"))
    //                                CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            // -------Calculation End----------'
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            if (FB1 > strBendLengths[0])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                B2 = B2 + 50;
    //                                FB1 = B2 - intFactor;
    //                                // -------Calculation start--------'
    //                                C1 = intCWSpacing - B2;
    //                                intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                                C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                                CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2C"))
    //                                    CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                // -------Calculation End----------'

    //                                if (FB1 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus2B = "Fail";
    //                            }
    //                        } // END If FB > 
    //                    } // END if Shape 2cr
    //                } // END if Shape 2c

    //                C1 = intCWSpacing - B2;
    //                SB1 = C1 + intFactor;

    //                if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                {
    //                    if (SB1 > strBendLengths[3])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2C"))
    //                            CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;


    //                        C1 = C1 + 20;
    //                        SB1 = C1 + intFactor;
    //                        // -------Calculation start--------'

    //                        intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                        C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2C"))
    //                            CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        // -------Calculation End----------'


    //                        if (SB1 > strBendLengths[3])
    //                        {
    //                            SBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C1 = C1 + 50;
    //                            SB1 = C1 + intFactor;
    //                            // -------Calculation start--------'
    //                            intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                            C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2C"))
    //                                CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            // -------Calculation End----------'
    //                            if (SB1 > strBendLengths[3])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus2B = "Fail";
    //                        }
    //                    } // END if FB>
    //                }
    //                else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("FBR1") | Shapecode.Contains("2CR6") | Shapecode.Contains("2CR8") | Shapecode.Contains("2CR9") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                {
    //                    if (SB1 > strBendLengths[1])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2C"))
    //                            CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;


    //                        C1 = C1 + 20;
    //                        SB1 = C1 + intFactor;
    //                        // -------Calculation start--------'

    //                        intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                        C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2C"))
    //                            CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        // -------Calculation End----------'

    //                        if (SB1 > strBendLengths[1])
    //                        {
    //                            SBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C1 = C1 + 50;
    //                            SB1 = C1 + intFactor;
    //                            // -------Calculation start--------'
    //                            C1 = intCWSpacing - B2;
    //                            intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                            C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2C"))
    //                                CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            // -------Calculation End----------'
    //                            if (SB1 > strBendLengths[1])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus2B = "Fail";
    //                        }
    //                    } // 'END if SB1 >
    //                } // END if Shape 2cr


    //                if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("TS7") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9"))
    //                {
    //                    intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                    C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                    FB2 = C2 - intFactor;
    //                    if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10"))
    //                    {
    //                        if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            FBStrStatus3B = "Pass";
    //                        else if (FB2 > strBendLengths[2])
    //                            FBStrStatus3B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            C2 = C2 + 20;
    //                            FB2 = C2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            D1 = intCWSpacing - C2;
    //                            intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                            D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 


    //                            if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB2 > strBendLengths[2])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                C2 = C2 + 50;
    //                                FB2 = C2 - intFactor;
    //                                // ------Calculation Start-----------'
    //                                D1 = intCWSpacing - C2;
    //                                intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                                D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                                CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3C"))
    //                                    CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                                // ------Calculation End 
    //                                if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB2 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus3B = "Fail";
    //                            }
    //                        } // END if FB2 > 
    //                    }
    //                    else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9"))
    //                    {
    //                        if (FB2 > strBendLengths[0])
    //                            FBStrStatus3B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            C2 = C2 + 20;
    //                            FB2 = C2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            D1 = intCWSpacing - C2;
    //                            intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                            D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 

    //                            if (FB2 > strBendLengths[0])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                C2 = C2 + 50;
    //                                FB2 = C2 - intFactor;
    //                                // ------Calculation Start-----------'
    //                                D1 = intCWSpacing - C2;
    //                                intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                                D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                                CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3C"))
    //                                    CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                                // ------Calculation End 
    //                                if (FB2 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus3B = "Fail";
    //                            }
    //                        } // IF FB2 > 
    //                    } // END IF Shape 3Cr
    //                } // END IF Shape 3C


    //                D1 = intCWSpacing - C2;
    //                SB2 = D1 + intFactor;

    //                if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("3C4") | Shapecode.Contains("3C5") | Shapecode.Contains("3C9") | Shapecode.Contains("3C10"))
    //                {
    //                    if (SB2 > strBendLengths[3])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3C"))
    //                            CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        D1 = D1 + 20;
    //                        SB2 = D1 + intFactor;
    //                        // ------Calculation Start-----------'

    //                        intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                        D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3C"))
    //                            CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        // ------Calculation End 

    //                        if (SB2 > strBendLengths[3])
    //                        {
    //                            SBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;

    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D1 = D1 + 50;
    //                            SB2 = D1 + intFactor;
    //                            // ------Calculation Start-----------'

    //                            intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                            D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 

    //                            if (SB2 > strBendLengths[3])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus3B = "Fail";
    //                        }
    //                    }  // IF SB2 > 
    //                }
    //                else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("3CR4") | Shapecode.Contains("3CR5") | Shapecode.Contains("3CR9"))
    //                {
    //                    if (SB2 > strBendLengths[1])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3C"))
    //                            CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        D1 = D1 + 20;
    //                        SB2 = D1 + intFactor;
    //                        // ------Calculation Start-----------'

    //                        intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                        D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3C"))
    //                            CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        // ------Calculation End 

    //                        if (SB2 > strBendLengths[1])
    //                        {
    //                            SBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D1 = D1 + 50;
    //                            SB2 = D1 + intFactor;
    //                            // ------Calculation Start-----------'

    //                            intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                            D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 
    //                            if (SB2 > strBendLengths[1])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus3B = "Fail";
    //                        }
    //                    } // IF SB2 > 
    //                } // END IF Shape 3CR

    //                if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                    D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                    FB3 = D2 - intFactor;
    //                    if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                    {
    //                        if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                            FBStrStatus4B = "Pass";
    //                        else if (FB3 > strBendLengths[2])
    //                            FBStrStatus4B = "Pass";
    //                        else
    //                        {
    //                            D2 = D2 + 20;
    //                            FB3 = D2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            E1 = intCWSpacing - D2;
    //                            intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                            E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 
    //                            if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB3 > strBendLengths[2])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                D2 = D2 + 50;
    //                                FB3 = D2 - intFactor;
    //                                // ------Calculation Start-----------'
    //                                E1 = intCWSpacing - D2;
    //                                intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                                E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                                CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                                if (Shapecode.Contains("4C"))
    //                                    CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                // ------Calculation End 


    //                                if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                    strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB3 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                    strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus4B = "Fail";
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                    {
    //                        if (FB3 > strBendLengths[0])
    //                            FBStrStatus4B = "Pass";
    //                        else
    //                        {
    //                            D2 = D2 + 20;
    //                            FB3 = D2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            E1 = intCWSpacing - D2;
    //                            intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                            E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 
    //                            if (FB3 > strBendLengths[0])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                D2 = D2 + 50;
    //                                FB3 = D2 - intFactor;
    //                                // ------Calculation Start-----------'
    //                                E1 = intCWSpacing - D2;
    //                                intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                                E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                                CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                                if (Shapecode.Contains("4C"))
    //                                    CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                // ------Calculation End 
    //                                if (FB3 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                    strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus4B = "Fail";
    //                            }
    //                        }
    //                    } // Shape 4c or 4CR
    //                } // Shape 4CR main



    //                E1 = intCWSpacing - D2;
    //                SB3 = E1 + intFactor;
    //                if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                {
    //                    if (SB3 > strBendLengths[3])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        E1 = E1 + 20;
    //                        SB3 = E1 + intFactor;
    //                        // ------Calculation Start-----------'

    //                        intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                        E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        if (Shapecode.Contains("4C"))
    //                            CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                        // ------Calculation End 
    //                        if (SB3 > strBendLengths[3])
    //                        {
    //                            SBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E1 = E1 + 50;
    //                            SB3 = E1 + intFactor;
    //                            // ------Calculation Start-----------'

    //                            intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                            E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 
    //                            if (SB3 > strBendLengths[3])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB3 > strBendLengths[1])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        E1 = E1 + 20;
    //                        SB3 = E1 + intFactor;
    //                        // ------Calculation Start-----------'

    //                        intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                        E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        if (Shapecode.Contains("4C"))
    //                            CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                        // ------Calculation End 

    //                        if (SB3 > strBendLengths[1])
    //                        {
    //                            SBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E1 = E1 + 50;
    //                            SB3 = E1 + intFactor;
    //                            // ------Calculation Start-----------'

    //                            intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                            E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 
    //                            if (SB3 > strBendLengths[1])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                } // IF SHAPE 4CR else 4C


    //                if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                    E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                    FB4 = E2 - intFactor;
    //                    if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                    {
    //                        if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            FBStrStatus4B = "Pass";
    //                        else if (FB4 > strBendLengths[2])
    //                            FBStrStatus5B = "Pass";
    //                        else
    //                        {
    //                            E2 = E2 + 20;
    //                            FB4 = E2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            F1 = intCWSpacing - E2;
    //                            intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                            F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4C"))
    //                                CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                            // ------Calculation End---------------' 
    //                            if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB4 > strBendLengths[2])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                strspace = CW_E + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                E2 = E2 + 50;
    //                                FB4 = E2 - intFactor;
    //                                // ------Calculation Start-----------'
    //                                F1 = intCWSpacing - E2;
    //                                intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                                F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                                CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4C"))
    //                                    CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                                // ------Calculation End---------------' 
    //                                if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB4 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus5B = "Fail";
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                    {
    //                        if (FB4 > strBendLengths[0])
    //                            FBStrStatus5B = "Pass";
    //                        else
    //                        {
    //                            E2 = E2 + 20;
    //                            FB4 = E2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            F1 = intCWSpacing - E2;
    //                            intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                            F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4C"))
    //                                CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                            // ------Calculation End---------------' 
    //                            if (FB4 > strBendLengths[0])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                E2 = E2 + 50;
    //                                FB4 = E2 - intFactor;
    //                                // ------Calculation Start-----------'
    //                                F1 = intCWSpacing - E2;
    //                                intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                                F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                                CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4C"))
    //                                    CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                                // ------Calculation End---------------' 
    //                                if (FB4 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    FBStrStatus5B = "Fail";
    //                            }
    //                        }
    //                    } // If Shape 5c or 5CR

    //                    F1 = intCWSpacing - E2;
    //                    SB4 = F1 + intFactor;
    //                    if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                    {
    //                        if (SB4 > strBendLengths[3])
    //                            SBStrStatus5B = "Pass";
    //                        else
    //                        {
    //                            F1 = F1 + 20;
    //                            SB4 = F1 + intFactor;
    //                            // ------Calculation Start-----------'

    //                            intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                            F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4C"))
    //                                CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                            // ------Calculation End---------------' 
    //                            if (SB4 > strBendLengths[3])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                F1 = F1 + 50;
    //                                SB4 = F1 + intFactor;
    //                                // ------Calculation Start-----------'

    //                                intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                                F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                                CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4C"))
    //                                    CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                                // ------Calculation End---------------' 
    //                                if (SB4 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    SBStrStatus5B = "Fail";
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                    {
    //                        if (SB4 > strBendLengths[1])
    //                            SBStrStatus5B = "Pass";
    //                        else
    //                        {
    //                            F1 = F1 + 20;
    //                            SB4 = F1 + intFactor;
    //                            // ------Calculation Start-----------'

    //                            intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                            F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4C"))
    //                                CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                            // ------Calculation End---------------' 
    //                            if (SB4 > strBendLengths[1])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                F1 = F1 + 50;
    //                                SB4 = F1 + intFactor;
    //                                // ------Calculation Start-----------'

    //                                intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                                F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                                CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4C"))
    //                                    CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                                // ------Calculation End---------------' 
    //                                if (SB4 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                    SBStrStatus5B = "Fail";
    //                            }
    //                        }
    //                    }  // IF Shape 5c or 5CR
    //                } // If Shpae 5CR Main
    //            }
    //            else if (Co1 > A)
    //            {
    //                B1 = Co1 - A;
    //                FB = B1 - intFactor;

    //                if (Shapecode.Contains("1CR") | Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("ES1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN6") | Shapecode.Contains("TS6") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25"))
    //                {
    //                    if (SB > strBendLengths[3])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        B1 = B1 + 20;
    //                        FB = B1 - intFactor;
    //                        // Calculation start'

    //                        intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                        B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                   // Calculation End'

    //                        if (SB > strBendLengths[3])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            B1 = B1 + 50;
    //                            FB = B1 - intFactor;
    //                            // Calculation start'

    //                            intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                            B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                       // Calculation End'
    //                            if (SB > strBendLengths[3])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus = "Fail";
    //                                FBStrStatus = "Fail";
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("FBR1"))
    //                {
    //                    if (SB > strBendLengths[1])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        B1 = B1 + 20;
    //                        FB = B1 - intFactor;
    //                        // Calculation start'

    //                        intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                        B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                   // Calculation End'
    //                        if (SB > strBendLengths[1])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            B1 = B1 + 50;
    //                            FB = B1 - intFactor;
    //                            // Calculation start'
    //                            intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                            B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1; // End if '1M'
    //                                                                                       // Calculation End'
    //                            if (SB > strBendLengths[1])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpaceC1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus = "Fail";
    //                        }
    //                    }
    //                } // IF shape 1c or 1CR


    //                if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                {
    //                    intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                    B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                    FB1 = B2 - intFactor;

    //                    if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        FBStrStatus2B = "Pass";
    //                    else if (FB1 > strBendLengths[2])
    //                        FBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        B2 = B2 + 20;
    //                        FB1 = B2 - intFactor;
    //                        // -------Calculation start--------'
    //                        C1 = intCWSpacing - B2;
    //                        intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                        C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2C"))
    //                            CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        // -------Calculation End----------'
    //                        if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB1 > strBendLengths[2])
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            B2 = B2 + 50;
    //                            FB1 = B2 - intFactor;
    //                            // -------Calculation start--------'
    //                            C1 = intCWSpacing - B2;
    //                            intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                            C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2C"))
    //                                CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            // -------Calculation End----------'
    //                            if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB1 > strBendLengths[2])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus2B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5"))
    //                {
    //                    if (FB1 > strBendLengths[0])
    //                        FBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        B2 = B2 + 20;
    //                        FB1 = B2 - intFactor;
    //                        // -------Calculation start--------'
    //                        C1 = intCWSpacing - B2;
    //                        intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                        C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2C"))
    //                            CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        // -------Calculation End----------'
    //                        if (FB1 > strBendLengths[0])
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            B2 = B2 + 50;
    //                            FB1 = B2 - intFactor;
    //                            // -------Calculation start--------'
    //                            C1 = intCWSpacing - B2;
    //                            intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                            C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2C"))
    //                                CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            // -------Calculation End----------'
    //                            if (FB1 > strBendLengths[0])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus2B = "Fail";
    //                        }
    //                    }
    //                } // IF shape 2C or 2CR


    //                C1 = intCWSpacing - B2;
    //                SB1 = C1 + intFactor;

    //                if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                {
    //                    if (SB1 > strBendLengths[3])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        C1 = C1 + 20;
    //                        SB1 = C1 + intFactor;
    //                        // -------Calculation start--------'

    //                        intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                        C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2C"))
    //                            CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        // -------Calculation End----------'
    //                        if (SB1 > strBendLengths[3])
    //                        {
    //                            SBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C1 = C1 + 50;
    //                            SB1 = C1 + intFactor;
    //                            // -------Calculation start--------'
    //                            intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                            C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2C"))
    //                                CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            // -------Calculation End----------'
    //                            if (SB1 > strBendLengths[3])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus2B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5"))
    //                {
    //                    if (SB1 > strBendLengths[1])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        C1 = C1 + 20;
    //                        SB1 = C1 + intFactor;
    //                        // -------Calculation start--------'

    //                        intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                        C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2C"))
    //                            CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        // -------Calculation End----------'
    //                        if (SB1 > strBendLengths[1])
    //                        {
    //                            SBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C1 = C1 + 20;
    //                            SB1 = C1 + intFactor;
    //                            // -------Calculation start--------'

    //                            intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                            C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2C"))
    //                                CW_C = Math.Floor((C - (C1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            // -------Calculation End----------'
    //                            if (SB1 > strBendLengths[1])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpaceC2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus2B = "Fail";
    //                        }
    //                    }
    //                } // If shape 2CR or 2C

    //                if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                {
    //                    intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                    C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                    FB2 = C2 - intFactor;

    //                    if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                        FBStrStatus3B = "Pass";
    //                    else if (FB2 > strBendLengths[2])
    //                        FBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        C2 = C2 + 20;
    //                        FB2 = C2 - intFactor;
    //                        // ------Calculation Start-----------'
    //                        D1 = intCWSpacing - C2;
    //                        intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                        D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3C"))
    //                            CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        // ------Calculation End 
    //                        if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB2 > strBendLengths[2])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C2 = C2 + 50;
    //                            FB2 = C2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            D1 = intCWSpacing - C2;
    //                            intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                            D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 
    //                            if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB2 > strBendLengths[2])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (FB2 > strBendLengths[0])
    //                        FBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        C2 = C2 + 20;
    //                        FB2 = C2 - intFactor;
    //                        // ------Calculation Start-----------'
    //                        D1 = intCWSpacing - C2;
    //                        intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                        D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3C"))
    //                            CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        // ------Calculation End 
    //                        if (FB2 > strBendLengths[0])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C2 = C2 + 50;
    //                            FB2 = C2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            D1 = intCWSpacing - C2;
    //                            intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                            D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 
    //                            if (FB2 > strBendLengths[0])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                } // IF shape 3CR or 3C

    //                D1 = intCWSpacing - C2;
    //                SB2 = D1 + intFactor;

    //                if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                {
    //                    if (SB2 > strBendLengths[3])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        D1 = D1 + 20;
    //                        SB2 = D1 + intFactor;
    //                        // ------Calculation Start-----------'

    //                        intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                        D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3C"))
    //                            CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        // ------Calculation End 
    //                        if (SB2 > strBendLengths[3])
    //                        {
    //                            SBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D1 = D1 + 50;
    //                            SB2 = D1 + intFactor;
    //                            // ------Calculation Start-----------'
    //                            intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                            D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 
    //                            if (SB2 > strBendLengths[3])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB2 > strBendLengths[1])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        D1 = D1 + 20;
    //                        SB2 = D1 + intFactor;
    //                        // ------Calculation Start-----------'

    //                        intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                        D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3C"))
    //                            CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        // ------Calculation End 
    //                        if (SB2 > strBendLengths[1])
    //                        {
    //                            SBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D1 = D1 + 50;
    //                            SB2 = D1 + intFactor;
    //                            // ------Calculation Start-----------'

    //                            intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                            D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 
    //                            if (SB2 > strBendLengths[1])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpaceC3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                } // IF shape 3cR or 3C


    //                if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                    D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                    FB3 = D2 - intFactor;
    //                    if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                        FBStrStatus4B = "Pass";
    //                    else if (FB3 > strBendLengths[2])
    //                        FBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        D2 = D2 + 20;
    //                        FB3 = D2 - intFactor;
    //                        // ------Calculation Start-----------'
    //                        E1 = intCWSpacing - D2;
    //                        intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                        E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        if (Shapecode.Contains("4C"))
    //                            CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                        // ------Calculation End 
    //                        if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB3 > strBendLengths[2])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D2 = D2 + 20;
    //                            FB3 = D2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            E1 = intCWSpacing - D2;
    //                            intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                            E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                            // ------Calculation End 
    //                            if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB3 > strBendLengths[2])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (FB3 > strBendLengths[0])
    //                        FBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        D2 = D2 + 20;
    //                        FB3 = D2 - intFactor;
    //                        // ------Calculation Start-----------'
    //                        E1 = intCWSpacing - D2;
    //                        intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                        E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        if (Shapecode.Contains("4C"))
    //                            CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                        // ------Calculation End 
    //                        if (FB3 > strBendLengths[0])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D2 = D2 + 50;
    //                            FB3 = D2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            E1 = intCWSpacing - D2;
    //                            intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                            E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            // ------Calculation End 
    //                            if (FB3 > strBendLengths[0])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                } // IF END 4CR or 4C

    //                E1 = intCWSpacing - D2;
    //                SB3 = E1 + intFactor;
    //                if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB3 > strBendLengths[3])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        E1 = E1 + 20;
    //                        SB3 = E1 + intFactor;
    //                        // ------Calculation Start-----------'
    //                        intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                        E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        if (Shapecode.Contains("4C"))
    //                            CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        // ------Calculation End 
    //                        if (SB3 > strBendLengths[3])
    //                        {
    //                            SBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E1 = E1 + 20;
    //                            SB3 = E1 + intFactor;
    //                            // ------Calculation Start-----------'
    //                            intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                            E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            // ------Calculation End 
    //                            if (SB3 > strBendLengths[3])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB3 > strBendLengths[1])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        E1 = E1 + 20;
    //                        SB3 = E1 + intFactor;
    //                        // ------Calculation Start-----------'
    //                        intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                        E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        if (Shapecode.Contains("4C"))
    //                            CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        // ------Calculation End 
    //                        if (SB3 > strBendLengths[1])
    //                        {
    //                            SBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E1 = E1 + 20;
    //                            SB3 = E1 + intFactor;
    //                            // ------Calculation Start-----------'
    //                            intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                            E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            // ------Calculation End 
    //                            if (SB3 > strBendLengths[1])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpaceC4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                } // IF Shape 4CR or 4C

    //                if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                    E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                    FB4 = E2 - intFactor;
    //                    if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                        FBStrStatus5B = "Pass";
    //                    else if (FB4 > strBendLengths[2])
    //                        FBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        E2 = E2 + 20;
    //                        FB4 = E2 - intFactor;
    //                        // ------Calculation Start-----------'
    //                        F1 = intCWSpacing - E2;
    //                        intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                        F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                        if (Shapecode.Contains("4C"))
    //                            CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                        // ------Calculation End---------------' 
    //                        if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;

    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB4 > strBendLengths[2])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E2 = E2 + 50;
    //                            FB4 = E2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            F1 = intCWSpacing - E2;
    //                            intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                            F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                            // ------Calculation End---------------' 
    //                            if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB4 > strBendLengths[2])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus5B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (FB4 > strBendLengths[0])
    //                        FBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        E2 = E2 + 20;
    //                        FB4 = E2 - intFactor;
    //                        // ------Calculation Start-----------'
    //                        F1 = intCWSpacing - E2;
    //                        intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                        F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                        if (Shapecode.Contains("4C"))
    //                            CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                        // ------Calculation End---------------' 
    //                        if (FB4 > strBendLengths[0])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E2 = E2 + 50;
    //                            FB4 = E2 - intFactor;
    //                            // ------Calculation Start-----------'
    //                            F1 = intCWSpacing - E2;
    //                            intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                            F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                            // ------Calculation End---------------' 
    //                            if (FB4 > strBendLengths[0])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                FBStrStatus5B = "Fail";
    //                        }
    //                    } // IF FB4 > 
    //                } // IF Shape  5CR or 5C

    //                F1 = intCWSpacing - E2;
    //                SB4 = F1 + intFactor;
    //                if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB4 > strBendLengths[3])
    //                        SBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        F1 = F1 + 20;
    //                        SB4 = F1 + intFactor;
    //                        // ------Calculation Start-----------'
    //                        intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                        F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                        if (Shapecode.Contains("4C"))
    //                            CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                        // ------Calculation End---------------' 
    //                        if (SB4 > strBendLengths[3])
    //                        {
    //                            SBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            F1 = F1 + 50;
    //                            SB4 = F1 + intFactor;
    //                            // ------Calculation Start-----------'
    //                            intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                            F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                            // ------Calculation End---------------' 
    //                            if (SB4 > strBendLengths[3])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus5B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB4 > strBendLengths[1])
    //                        SBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        F1 = F1 + 20;
    //                        SB4 = F1 + intFactor;
    //                        // ------Calculation Start-----------'
    //                        intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                        F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                        CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                        if (Shapecode.Contains("4C"))
    //                            CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                        // ------Calculation End---------------' 
    //                        if (SB4 > strBendLengths[1])
    //                        {
    //                            SBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            F1 = F1 + 50;
    //                            SB4 = F1 + intFactor;
    //                            // ------Calculation Start-----------'
    //                            intInterval6B = Math.Floor((F - F1) / intCWSpacing);
    //                            F2 = F - ((intInterval6B * intCWSpacing) + F1);
    //                            CW_A = Math.Floor((A - Co1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("4C"))
    //                                CW_F = Math.Floor((F - (F1 + Co2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_F = Math.Floor((F - (F1 + F2)) / intCWSpacing) + 1;

    //                            // ------Calculation End---------------' 
    //                            if (SB4 > strBendLengths[1])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpaceC5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                                SBStrStatus5B = "Fail"; // End A1 + 50
    //                        } // SB4 > A1+ 20 
    //                    } // IF SB4 > 
    //                } // IF shape 5cR else 5C
    //            }
    //            else if (((intInterval * intCWSpacing) + Co1) == A)
    //            {
    //                FBStrStatus = "Fail";
    //                SBStrStatus = "Fail";
    //                FBStrStatus2B = "Fail";
    //                SBStrStatus2B = "Fail";
    //                FBStrStatus3B = "Fail";
    //                SBStrStatus3B = "Fail";
    //                FBStrStatus4B = "Fail";
    //                SBStrStatus4B = "Fail";
    //                FBStrStatus5B = "Fail";
    //                SBStrStatus5B = "Fail";
    //            }

    //            strspace = "0";
    //            strspaceCo1 = strSpaceC1M + ";" + strSpaceC2M + ";" + strSpaceC3M + ";" + strSpaceC4M + ";" + strSpaceC5M;
    //            strspaceCo1 = Interaction.IIf(strSpaceC1M != string.Empty, strSpaceC1M + ";", "") + Interaction.IIf(strSpaceC2M != string.Empty, strSpaceC2M + ";", "") + Interaction.IIf(strSpaceC3M != string.Empty, strSpaceC3M + ";", "") + Interaction.IIf(strSpaceC4M != string.Empty, strSpaceC4M + ";", "") + Interaction.IIf(strSpaceC5M != string.Empty, strSpaceC5M + ";", "");
    //            strspaceCo1 = strspaceCo1.Trim(";");



    //            if (Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus4B != "" & FBStrStatus4B != "" & SBStrStatus5B != "" & FBStrStatus5B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass" & FBStrStatus5B == "Pass" & SBStrStatus5B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //            }
    //            else if (Shapecode.Contains("4C"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "" & FBStrStatus4B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //            }
    //            else if (Shapecode.Contains("3C"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //            }
    //            else if (Shapecode.Contains("2C"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //            }
    //            else if (Shapecode.Contains("1C") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("TS7"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //                else if (SBStrStatus == "")
    //                {
    //                    if (FBStrStatus == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + System.Convert.ToString(intNumWire);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //        return StrStatus;
    //    }

    //    public void FnStatusWireRemovalMo1(int Mo1, int Mo2, int A, int B, int C, int D, int E, int F, int intCWSpacing, string Shapecode, int intFactor)
    //    {
    //        try
    //        {
    //            string strSpace1M = "";
    //            string strSpace2M = "";
    //            string strSpace3M = "";
    //            string strSpace4M = "";
    //            string strSpace5M = "";
    //            string strSpace6M = "";
    //            string strSpace7M = "";
    //            string strWireRemove1 = "";
    //            string strWireRemove2 = "";
    //            string strWireRemove3 = "";
    //            string strWireRemove4 = "";
    //            string strWireRemove5 = "";
    //            string strWireRemove6 = "";
    //            string strWireRemove7 = "";


    //            strspaceCo1 = "0";
    //            if (Mo1 < A)
    //            {
    //                intInterval = Math.Floor((A - Mo1) / intCWSpacing);
    //                A1 = A - ((intInterval * intCWSpacing) + Mo1);
    //                B1 = intCWSpacing - A1;
    //                intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                C1 = intCWSpacing - B2;
    //                intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                D1 = intCWSpacing - C2;
    //                intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                E1 = intCWSpacing - D2;
    //                intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                F1 = intCWSpacing - E2;


    //                // intInterval = (intInterval)

    //                if (Shapecode.Contains("1MR") | Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("FBR1"))
    //                {
    //                    A1 = A - ((intInterval * intCWSpacing) + Mo1);
    //                    B1 = intCWSpacing - A1;

    //                    FB = A1 - intFactor;
    //                    SB = B1 + intFactor;

    //                    if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                        FBStrStatus = "Pass";
    //                    else if (FB > strBendLengths[2])
    //                        FBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1M"))
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;


    //                        intNumWire = CW_A + CW_B;
    //                        // If intfailcount = 0 Then
    //                        A1 = A1 + 20;
    //                        FB = A1 - intFactor;
    //                        if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                            FBStrStatus = "Pass";
    //                        else if (FB > strBendLengths[2])
    //                        {
    //                            FBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1M"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B;
    //                            A1 = A1 + 50;
    //                            FB = A1 - intFactor;
    //                            if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                                FBStrStatus = "Pass";
    //                            else if (FB > strBendLengths[2])
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                strSpaceLength = A1 + intCWSpacing + B1;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                strWireRemove1 = System.Convert.ToString(CW_A);
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DF1") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6"))
    //                {
    //                    A1 = A - ((intInterval * intCWSpacing) + Mo1);
    //                    B1 = intCWSpacing - A1;
    //                    FB = A1 - intFactor;
    //                    SB = B1 + intFactor;
    //                    if (FB > strBendLengths[0])
    //                        FBStrStatus = "Pass";
    //                    else if (intCWSpacing > 300)
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1M"))
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B;
    //                        // If intfailcount = 0 Then
    //                        A1 = A1 + 20;
    //                        FB = A1 - intFactor;
    //                        if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                        {
    //                            FBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB > strBendLengths[2])
    //                        {
    //                            FBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            intfailcount = intfailcount + 1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1M"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            A1 = A1 + 50;
    //                            B1 = B1;
    //                            FB = A1 - intFactor;
    //                            if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB > strBendLengths[2])
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + intCWSpacing + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                strWireRemove1 = System.Convert.ToString(CW_A);
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        FBStrStatus = "Pass";
    //                        strSpaceLength = A1 + intCWSpacing + B1;
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        strWireRemove1 = System.Convert.ToString(CW_A);
    //                    }
    //                }

    //                SB = B1 + intFactor;

    //                if (Shapecode.Contains("1MR") | Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR"))
    //                {
    //                    B1 = intCWSpacing - A1;
    //                    intInterval2B = Math.Floor((B - B1) / intCWSpacing);

    //                    if (SB > strBendLengths[3])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1M"))
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B;
    //                        // If intfailcount = 0 Then
    //                        B1 = B1 + 20;
    //                        SB = B1 + intFactor;
    //                        if (SB > strBendLengths[3])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            B1 = B1 + 50;
    //                            SB = B1 + intFactor;
    //                            if (SB > strBendLengths[3])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + intCWSpacing + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                strWireRemove1 = System.Convert.ToString(CW_A);
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("1M") | Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DF1") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6"))
    //                {
    //                    B1 = intCWSpacing - A1;
    //                    intInterval2B = Math.Floor((B - B1) / intCWSpacing);

    //                    if (SB > strBendLengths[1])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1M"))
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B;
    //                        // If intfailcount = 0 Then
    //                        B1 = B1 + 20;

    //                        SB = B1 + intFactor;
    //                        if (SB > strBendLengths[1])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            intfailcount = intfailcount + 1;
    //                            B1 = B1 + 50;
    //                            SB = B1 + intFactor;
    //                            if (SB > strBendLengths[1])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + intCWSpacing + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                strWireRemove1 = System.Convert.ToString(CW_A);
    //                            }
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("DF5") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2M6") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6"))
    //                {
    //                    B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                    C1 = intCWSpacing - B2;

    //                    FB1 = B2 - intFactor;


    //                    if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("FBR1") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8"))
    //                    {
    //                        if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            FBStrStatus2B = "Pass";
    //                        else if (FB1 > strBendLengths[2])
    //                            FBStrStatus2B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;



    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            // If intfailcount = 0 Then
    //                            B2 = B2 + 20;
    //                            FB1 = B2 - intFactor;
    //                            if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB1 > strBendLengths[2])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                B2 = B2 + 50;
    //                                FB1 = B2 - intFactor;
    //                                if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB1 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + intCWSpacing + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                    strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + intCWSpacing + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6"))
    //                    {
    //                        B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                        C1 = intCWSpacing - B2;

    //                        if (FB1 > strBendLengths[0])
    //                            FBStrStatus2B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B + CW_C;

    //                            // If intfailcount = 0 Then
    //                            B2 = B2 + 20;
    //                            FB1 = B2 - intFactor;
    //                            if (FB1 > strBendLengths[0])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                B2 = B2 + 50;
    //                                C1 = C1;
    //                                FB1 = B2 - intFactor;
    //                                if (FB1 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + intCWSpacing + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                    strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + intCWSpacing + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                        }
    //                    }

    //                    C1 = intCWSpacing - B2;
    //                    SB1 = C1 + intFactor;

    //                    if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("DF5") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR6") | Shapecode.Contains("FBR1") | Shapecode.Contains("2M6") | Shapecode.Contains("2M8") | Shapecode.Contains("2M9") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("FN4") | Shapecode.Contains("FS4") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8"))
    //                    {
    //                        if (SB1 > strBendLengths[3])
    //                            SBStrStatus2B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            // If intfailcount = 0 Then
    //                            C1 = C1 + 20;
    //                            SB1 = C1 + intFactor;
    //                            if (SB1 > strBendLengths[3])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                C1 = C1 + 50;
    //                                SB1 = C1 + intFactor;
    //                                if (SB1 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    SBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + intCWSpacing + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                    strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("DF1") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("ES10") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("2MR6") | Shapecode.Contains("2MR8") | Shapecode.Contains("2MR9") | Shapecode.Contains("2MR12") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("ES1") | Shapecode.Contains("FN1") | Shapecode.Contains("FS1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6"))
    //                    {
    //                        C1 = intCWSpacing - B2;

    //                        if (SB1 > strBendLengths[1])
    //                            SBStrStatus2B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            // If intfailcount = 0 Then
    //                            C1 = C1 + 20;
    //                            SB1 = C1 + intFactor;
    //                            if (SB1 > strBendLengths[1])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                C1 = C1 + 50;
    //                                SB1 = C1 + intFactor;
    //                                if (SB1 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    SBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + intCWSpacing + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                    strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                }
    //                            }
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25") | Shapecode.Contains("TS6"))
    //                {
    //                    intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                    C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                    FB2 = C2 - intFactor;
    //                    if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25"))
    //                    {
    //                        if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            FBStrStatus3B = "Pass";
    //                        else if (FB2 > strBendLengths[2])
    //                            FBStrStatus3B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            C2 = C2 + 20;
    //                            FB2 = C2 - intFactor;
    //                            if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB2 > strBendLengths[2])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                C2 = C2 + 50;
    //                                FB2 = C2 - intFactor;
    //                                if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB2 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + intCWSpacing + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                    strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + intCWSpacing + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (FB2 > strBendLengths[0])
    //                            FBStrStatus3B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;

    //                            C2 = C2 + 20;
    //                            FB2 = C2 - intFactor;
    //                            if (FB2 > strBendLengths[0])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                C2 = C2 + 50;
    //                                FB2 = C2 - intFactor;
    //                                if (FB2 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + intCWSpacing + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                    strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + intCWSpacing + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                        }
    //                    }

    //                    D1 = intCWSpacing - C2;
    //                    SB2 = D1 + intFactor;

    //                    if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("3M4") | Shapecode.Contains("3M5") | Shapecode.Contains("3M9") | Shapecode.Contains("3M10") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25"))
    //                    {
    //                        if (SB2 > strBendLengths[3])
    //                            SBStrStatus3B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            D1 = D1 + 20;
    //                            SB2 = D1 + intFactor;
    //                            if (SB2 > strBendLengths[3])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                D1 = D1 + 50;
    //                                SB2 = D1 + intFactor;
    //                                if (SB2 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    SBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + intCWSpacing + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                    strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("3M") | Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB21") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("3MR4") | Shapecode.Contains("3MR5") | Shapecode.Contains("3MR9") | Shapecode.Contains("3MR10") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (SB2 > strBendLengths[1])
    //                            SBStrStatus3B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            D1 = D1 + 20;
    //                            SB2 = D1 + intFactor;
    //                            if (SB2 > strBendLengths[1])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                D1 = D1 + 50;
    //                                SB2 = D1 + intFactor;
    //                                if (SB2 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    SBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + intCWSpacing + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                    strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                }
    //                            }
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26") | Shapecode.Contains("TS6"))
    //                {
    //                    intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                    D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                    FB3 = D2 - intFactor;
    //                    if (Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26"))
    //                    {
    //                        if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                            FBStrStatus4B = "Pass";
    //                        else if (FB3 > strBendLengths[2])
    //                            FBStrStatus4B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            D2 = D2 + 20;
    //                            FB3 = D2 - intFactor;
    //                            if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB3 > strBendLengths[2])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                D2 = D2 + 50;
    //                                FB3 = D2 - intFactor;
    //                                if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB3 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + intCWSpacing + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                    strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + intCWSpacing + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (FB3 > strBendLengths[0])
    //                            FBStrStatus4B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            D2 = D2 + 20;
    //                            FB3 = D2 - intFactor;
    //                            if (FB3 > strBendLengths[0])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                D2 = D2 + 50;
    //                                FB3 = D2 - intFactor;
    //                                if (FB3 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + intCWSpacing + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                    strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + intCWSpacing + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                        }
    //                    }

    //                    E1 = intCWSpacing - D2;
    //                    SB3 = E1 + intFactor;
    //                    if (Shapecode.Contains("4MR") | Shapecode.Contains("5MR") | Shapecode.Contains("4M2") | Shapecode.Contains("4M4") | Shapecode.Contains("4M5") | Shapecode.Contains("4M6") | Shapecode.Contains("TB24") | Shapecode.Contains("TB26"))
    //                    {
    //                        if (SB3 > strBendLengths[3])
    //                            SBStrStatus4B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            D2 = D2 + 20;
    //                            SB3 = D2 + intFactor;
    //                            if (SB3 > strBendLengths[3])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                intfailcount = intfailcount + 1;
    //                                D2 = D2 + 50;
    //                                SB3 = D2 + intFactor;
    //                                if (SB3 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    SBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + intCWSpacing + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                    strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("4M") | Shapecode.Contains("5M") | Shapecode.Contains("TB2") | Shapecode.Contains("TB20") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("4MR2") | Shapecode.Contains("4MR4") | Shapecode.Contains("4MR5") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (SB3 > strBendLengths[1])
    //                            SBStrStatus4B = "Pass";
    //                        else
    //                        {
    //                            CW_A = System.Convert.ToInt32(Math.Round((A - Mo1) / intCWSpacing) + 1);
    //                            CW_B = System.Convert.ToInt32(Math.Round((B - (B1 + B2)) / intCWSpacing) + 1);
    //                            CW_C = System.Convert.ToInt32(Math.Round((C - (C1 + C2)) / intCWSpacing) + 1);
    //                            CW_D = System.Convert.ToInt32(Math.Round((D - (D1 + D2)) / intCWSpacing) + 1);
    //                            CW_E = System.Convert.ToInt32(Math.Round((E - (E1 + Mo2)) / intCWSpacing) + 1);
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            // If intfailcount = 0 Then
    //                            D2 = D2 + 20;
    //                            SB3 = D2 + intFactor;
    //                            if (SB3 > strBendLengths[1])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = System.Convert.ToInt32(Math.Round((A - Mo1) / intCWSpacing) + 1);
    //                                CW_B = System.Convert.ToInt32(Math.Round((B - (B1 + B2)) / intCWSpacing) + 1);
    //                                CW_C = System.Convert.ToInt32(Math.Round((C - (C1 + C2)) / intCWSpacing) + 1);
    //                                CW_D = System.Convert.ToInt32(Math.Round((D - (D1 + D2)) / intCWSpacing) + 1);
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                D2 = D2 + 50;
    //                                SB3 = D2 + intFactor;
    //                                if (SB3 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = System.Convert.ToInt32(Math.Round((A - Mo1) / intCWSpacing) + 1);
    //                                    CW_B = System.Convert.ToInt32(Math.Round((B - (B1 + B2)) / intCWSpacing) + 1);
    //                                    CW_C = System.Convert.ToInt32(Math.Round((C - (C1 + C2)) / intCWSpacing) + 1);
    //                                    CW_D = System.Convert.ToInt32(Math.Round((D - (D1 + D2)) / intCWSpacing) + 1);
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    SBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + intCWSpacing + E1;
    //                                    CW_A = System.Convert.ToInt32(Math.Round((A - Mo1) / intCWSpacing) + 1);
    //                                    CW_B = System.Convert.ToInt32(Math.Round((B - (B1 + B2)) / intCWSpacing) + 1);
    //                                    CW_C = System.Convert.ToInt32(Math.Round((C - (C1 + C2)) / intCWSpacing) + 1);
    //                                    CW_D = System.Convert.ToInt32(Math.Round((D - (D1 + D2)) / intCWSpacing) + 1);
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                    strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                }
    //                            }
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7"))
    //                {
    //                    intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                    E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                    FB4 = E2 - intFactor;
    //                    if (Shapecode.Contains("5MR"))
    //                    {
    //                        if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            FBStrStatus5B = "Pass";
    //                        else if (FB4 > strBendLengths[2])
    //                            FBStrStatus5B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_F;
    //                            // If intfailcount = 0 Then
    //                            E2 = E2 + 20;
    //                            FB4 = E2 - intFactor;
    //                            if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB4 > strBendLengths[2])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                E2 = E2 + 50;
    //                                FB4 = E2 - intFactor;
    //                                if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB4 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + intCWSpacing + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                    strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + intCWSpacing + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (FB4 > strBendLengths[0])
    //                            FBStrStatus5B = "Pass";
    //                        else if (intCWSpacing > 300)
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_F;
    //                            // If intfailcount = 0 Then
    //                            E2 = E2 + 20;
    //                            FB4 = E2 - intFactor;
    //                            if (FB4 > strBendLengths[0])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB4 > strBendLengths[0])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                E2 = E2 + 50;
    //                                FB4 = E2 - intFactor;
    //                                if (FB4 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else if (FB4 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + intCWSpacing + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                    strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + intCWSpacing + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                        }
    //                    }

    //                    F1 = intCWSpacing - E2;
    //                    SB4 = F1 + intFactor;
    //                    if (Shapecode.Contains("5MR"))
    //                    {
    //                        if (SB4 > strBendLengths[3])
    //                            SBStrStatus5B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_F;
    //                            // If intfailcount = 0 Then
    //                            F1 = F1 + 20;
    //                            SB4 = F1 + intFactor;
    //                            if (SB4 > strBendLengths[3])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                F1 = F1 + 50;
    //                                SB4 = F1 + intFactor;
    //                                if (SB4 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    SBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + intCWSpacing + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                    strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("5M") | Shapecode.Contains("TB14") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS7") | Shapecode.Contains("TS6"))
    //                    {
    //                        if (SB4 > strBendLengths[1])
    //                            SBStrStatus5B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_F;
    //                            // If intfailcount = 0 Then
    //                            F1 = F1 + 20;
    //                            SB4 = F1 + intFactor;
    //                            if (SB4 > strBendLengths[1])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                F1 = F1 + 50;
    //                                SB4 = F1 + intFactor;
    //                                if (SB4 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                }
    //                                else
    //                                {
    //                                    SBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + intCWSpacing + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                    strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            else if (Mo1 > A)
    //            {
    //                B1 = Mo1 - A;
    //                SB = B1 + intFactor;

    //                if (Shapecode.Contains("1MR") | Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR"))
    //                {
    //                    if (SB > strBendLengths[3])
    //                        SBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B;
    //                        B1 = B1 + 20;
    //                        SB = SB == B1 + intFactor;
    //                        if (SB > strBendLengths[3])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            B1 = B1 + 50;
    //                            SB = SB == B1 + intFactor;
    //                            if (SB > strBendLengths[3])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + intCWSpacing + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                strWireRemove1 = System.Convert.ToString(CW_A);
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (SB > strBendLengths[1])
    //                    SBStrStatus = "Pass";
    //                else
    //                {
    //                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                    CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                    intNumWire = CW_A + CW_B;
    //                    B1 = B1 + 20;
    //                    SB = SB == B1 + intFactor;
    //                    if (SB > strBendLengths[1])
    //                    {
    //                        SBStrStatus = "Pass";
    //                        strSpaceLength = A1 + B1;
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                    }
    //                    else
    //                    {
    //                        B1 = B1 + 50;
    //                        SB = SB == B1 + intFactor;
    //                        if (SB > strBendLengths[1])
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            SBStrStatus = "Pass";
    //                            strSpaceLength = A1 + intCWSpacing + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            strWireRemove1 = System.Convert.ToString(CW_A);
    //                        }
    //                    }
    //                }


    //                if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR"))
    //                {
    //                    intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                    B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                    FB1 = B2 - intFactor;

    //                    if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        FBStrStatus2B = "Pass";
    //                    else if (FB1 > strBendLengths[2])
    //                        FBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2M"))
    //                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B + CW_C;
    //                        B2 = B2 + 20;
    //                        FB1 = B2 - intFactor;
    //                        if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB1 > strBendLengths[2])
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            B2 = B2 + 50;
    //                            FB1 = B2 - intFactor;
    //                            if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB1 > strBendLengths[2])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + intCWSpacing + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (FB1 > strBendLengths[0])
    //                    FBStrStatus2B = "Pass";
    //                else
    //                {
    //                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                    if (Shapecode.Contains("2M"))
    //                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                    else
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                    intNumWire = CW_A + CW_B + CW_C;
    //                    B2 = B2 + 20;
    //                    FB1 = B2 - intFactor;
    //                    if (FB1 > strBendLengths[0])
    //                    {
    //                        FBStrStatus2B = "Pass";
    //                        strSpaceLength = B2 + C1;
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                    }
    //                    else
    //                    {
    //                        B2 = B2 + 50;
    //                        FB1 = B2 - intFactor;
    //                        if (FB1 > strBendLengths[0])
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + intCWSpacing + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                    }
    //                }


    //                C1 = intCWSpacing - B2;
    //                SB1 = C1 + intFactor;

    //                if (Shapecode.Contains("2MR") | Shapecode.Contains("3MR") | Shapecode.Contains("4MR"))
    //                {
    //                    if (SB1 > strBendLengths[3])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2M"))
    //                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B + CW_C;
    //                        C1 = C1 + 20;
    //                        SB1 = C1 + intFactor;
    //                        if (SB1 > strBendLengths[3])
    //                        {
    //                            SBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C1 = C1 + 50;
    //                            SB1 = C1 + intFactor;
    //                            if (SB1 > strBendLengths[3])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + intCWSpacing + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                strWireRemove2 = System.Convert.ToString(CW_A);
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("2M") | Shapecode.Contains("3M") | Shapecode.Contains("4M"))
    //                {
    //                    if (SB1 > strBendLengths[1])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2M"))
    //                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B + CW_C;
    //                        C1 = C1 + 20;
    //                        SB1 = C1 + intFactor;
    //                        if (SB1 > strBendLengths[1])
    //                        {
    //                            SBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C1 = C1 + 50;
    //                            SB1 = C1 + intFactor;
    //                            if (SB1 > strBendLengths[1])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + intCWSpacing + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR"))
    //                {
    //                    intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                    C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                    FB2 = C2 - intFactor;


    //                    if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                        FBStrStatus3B = "Pass";
    //                    else if (FB2 > strBendLengths[2])
    //                        FBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3M"))
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        C2 = C2 + 20;
    //                        FB2 = C2 - intFactor;

    //                        if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB2 > strBendLengths[2])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            C2 = C2 + 50;
    //                            FB2 = C2 - intFactor;

    //                            if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB2 > strBendLengths[2])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + intCWSpacing + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (FB2 > strBendLengths[0])
    //                    FBStrStatus3B = "Pass";
    //                else
    //                {
    //                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                    if (Shapecode.Contains("3M"))
    //                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                    else
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                    C2 = C2 + 20;
    //                    FB2 = C2 - intFactor;
    //                    if (FB2 > strBendLengths[0])
    //                    {
    //                        FBStrStatus3B = "Pass";
    //                        strSpaceLength = C2 + D1;
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                    }
    //                    else
    //                    {
    //                        C2 = C2 + 50;
    //                        FB2 = C2 - intFactor;
    //                        if (FB2 > strBendLengths[0])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + intCWSpacing + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                        }
    //                    }
    //                }

    //                D1 = intCWSpacing - C2;
    //                SB2 = D1 + intFactor;

    //                if (Shapecode.Contains("3MR") | Shapecode.Contains("4MR"))
    //                {
    //                    if (SB2 > strBendLengths[3])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3M"))
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        D1 = D1 + 20;
    //                        SB2 = D1 + intFactor;
    //                        if (SB2 > strBendLengths[3])
    //                        {
    //                            SBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D1 = D1 + 50;
    //                            SB2 = D1 + intFactor;
    //                            if (SB2 > strBendLengths[3])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + intCWSpacing + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("3M") | Shapecode.Contains("4M"))
    //                {
    //                    if (SB2 > strBendLengths[1])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3M"))
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;


    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        D1 = D1 + 20;
    //                        SB2 = D1 + intFactor;
    //                        if (SB2 > strBendLengths[1])
    //                        {
    //                            SBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D1 = D1 + 50;
    //                            SB2 = D1 + intFactor;
    //                            if (SB2 > strBendLengths[1])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + intCWSpacing + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                            }
    //                        }
    //                    }
    //                }


    //                if (Shapecode.Contains("4MR"))
    //                {
    //                    intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                    D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                    FB3 = D2 - intFactor;
    //                    if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        FBStrStatus4B = "Pass";
    //                    else if (FB1 > strBendLengths[2])
    //                        FBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                        D2 = D2 + 20;
    //                        FB3 = D2 - intFactor;
    //                        if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB1 > strBendLengths[2])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            D2 = D2 + 50;
    //                            FB3 = D2 - intFactor;
    //                            if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB1 > strBendLengths[2])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + intCWSpacing + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (FB2 > strBendLengths[0])
    //                    FBStrStatus4B = "Pass";
    //                else
    //                {
    //                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                    D2 = D2 + 20;
    //                    FB3 = D2 - intFactor;
    //                    if (FB2 > strBendLengths[0])
    //                    {
    //                        FBStrStatus4B = "Pass";
    //                        strSpaceLength = D2 + E1;
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                    }
    //                    else
    //                    {
    //                        D2 = D2 + 50;
    //                        FB3 = D2 - intFactor;
    //                        if (FB2 > strBendLengths[0])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + intCWSpacing + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                        }
    //                    }
    //                }

    //                E1 = intCWSpacing - D2;
    //                SB3 = E1 + intFactor;
    //                if (Shapecode.Contains("4MR"))
    //                {
    //                    if (SB3 > strBendLengths[3])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                        E1 = E1 + 20;
    //                        SB3 = E1 + intFactor;
    //                        if (SB3 > strBendLengths[3])
    //                        {
    //                            SBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E1 = E1 + 50;
    //                            SB3 = E1 + intFactor;
    //                            if (SB3 > strBendLengths[3])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + intCWSpacing + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("4M"))
    //                {
    //                    if (SB3 > strBendLengths[1])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                        E1 = E1 + 20;
    //                        SB3 = E1 + intFactor;
    //                        if (SB3 > strBendLengths[1])
    //                        {
    //                            SBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E1 = E1 + 50;
    //                            SB3 = E1 + intFactor;
    //                            if (SB3 > strBendLengths[1])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + intCWSpacing + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                            }
    //                        }
    //                    }
    //                }

    //                // '''''
    //                if (Shapecode.Contains("5MR"))
    //                {
    //                    intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                    E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                    FB4 = E2 - intFactor;
    //                    if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                        FBStrStatus5B = "Pass";
    //                    else if (FB4 > strBendLengths[2])
    //                        FBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                        E2 = E2 + 20;
    //                        FB4 = D2 + intFactor;
    //                        if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else if (FB4 > strBendLengths[2])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            E2 = E2 + 50;
    //                            FB4 = E2 - intFactor;
    //                            if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB4 > strBendLengths[2])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + intCWSpacing + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (FB4 > strBendLengths[0])
    //                    FBStrStatus5B = "Pass";
    //                else
    //                {
    //                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                    E2 = E2 + 20;
    //                    FB4 = E2 - intFactor;
    //                    if (FB4 > strBendLengths[0])
    //                    {
    //                        FBStrStatus4B = "Pass";
    //                        strSpaceLength = E2 + F1;
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                    }
    //                    else
    //                    {
    //                        E2 = E2 + 50;
    //                        FB4 = E2 - intFactor;
    //                        if (FB4 > strBendLengths[0])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + intCWSpacing + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                        }
    //                    }
    //                }

    //                F1 = intCWSpacing - E2;
    //                SB4 = F1 + intFactor;
    //                if (Shapecode.Contains("5MR"))
    //                {
    //                    if (SB4 > strBendLengths[3])
    //                        SBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                        E1 = E1 + 20;
    //                        SB4 = E1 + intFactor;
    //                        if (SB4 > strBendLengths[3])
    //                        {
    //                            SBStrStatus4B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            F1 = F1 + 50;
    //                            SB4 = F1 + intFactor;
    //                            if (SB4 > strBendLengths[3])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + intCWSpacing + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("5M"))
    //                {
    //                    if (SB4 > strBendLengths[1])
    //                        SBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        CW_F = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                        F1 = F1 + 20;
    //                        SB4 = E1 + intFactor;
    //                        if (SB4 > strBendLengths[1])
    //                        {
    //                            SBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                        }
    //                        else
    //                        {
    //                            F1 = F1 + 50;
    //                            SB3 = F1 + intFactor;
    //                            if (SB3 > strBendLengths[1])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + intCWSpacing + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            else if (((intInterval * intCWSpacing) + Mo1) == A)
    //            {
    //                FBStrStatus = "Fail" + "," + strspace;
    //                SBStrStatus = "Fail" + "," + strspace;
    //                FBStrStatus2B = "Fail" + "," + strspace;
    //                SBStrStatus2B = "Fail" + "," + strspace;
    //                FBStrStatus3B = "Fail" + "," + strspace;
    //                SBStrStatus3B = "Fail" + "," + strspace;
    //                FBStrStatus4B = "Fail" + "," + strspace;
    //                SBStrStatus4B = "Fail" + "," + strspace;
    //            }

    //            strspace = Interaction.IIf(strSpace1M != string.Empty, strSpace1M + ";", "") + Interaction.IIf(strSpace2M != string.Empty, strSpace2M + ";", "") + Interaction.IIf(strSpace3M != string.Empty, strSpace3M + ";", "") + Interaction.IIf(strSpace4M != string.Empty, strSpace4M + ";", "") + Interaction.IIf(strSpace5M != string.Empty, strSpace5M + ";", "");
    //            strspace = strspace.Trim(";");



    //            // strWireRemove = strWireRemove1 + ";" + strWireRemove2 + ";" + strWireRemove3 + ";" + strWireRemove4 + ";" + strWireRemove5
    //            strWireRemove = Interaction.IIf(strWireRemove1 != string.Empty, strWireRemove1 + ";", "") + Interaction.IIf(strWireRemove2 != string.Empty, strWireRemove2 + ";", "") + Interaction.IIf(strWireRemove3 != string.Empty, strWireRemove3 + ";", "") + Interaction.IIf(strWireRemove4 != string.Empty, strWireRemove4 + ";", "") + Interaction.IIf(strWireRemove5 != string.Empty, strWireRemove5 + ";", "");
    //            strWireRemove = strWireRemove.Trim(";");
    //            strRemoveSplit = strWireRemove.Split(";");
    //            intNoWireRemoved = strRemoveSplit.Length;

    //            if (Shapecode.Contains("5M") | Shapecode.Contains("TS7"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));

    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "" & FBStrStatus4B != "" & SBStrStatus4B != "" & FBStrStatus4B != "" & SBStrStatus4B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass" & FBStrStatus5B == "Pass" & SBStrStatus5B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                }
    //            }
    //            else if (Shapecode.Contains("4M"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "" & FBStrStatus4B != "" & SBStrStatus4B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                }
    //            }
    //            else if (Shapecode.Contains("3M"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                }
    //            }
    //            else if (Shapecode.Contains("2M"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B + CW_C;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                }
    //            }
    //            else if (Shapecode.Contains("1M") | Shapecode.Contains("DS1"))
    //            {
    //                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                intNumWire = CW_A + CW_B;
    //                ResMo2 = intProdMWLen - (Mo1 + ((intNumWire - 1) * intCWSpacing));
    //                if (FBStrStatus != "" & SBStrStatus != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                }
    //                else if (SBStrStatus != "")
    //                {
    //                    if (SBStrStatus == "Pass")
    //                        StrStatus = "Pass" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                    else
    //                        StrStatus = "Fail" + "," + strspace + "," + strspaceCo1 + "," + strWireRemove + "," + System.Convert.ToString(intNoWireRemoved);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //        return StrStatus;
    //    }

    //    public void FnStatusWireRemovalCo1(int Co1, int A, int B, int C, int D, int E, int F, int intCWSpacing, string Shapecode, int intFactor)
    //    {
    //        try
    //        {
    //            string strSpace1M = "";
    //            string strSpace2M = "";
    //            string strSpace3M = "";
    //            string strSpace4M = "";
    //            string strSpace5M = "";
    //            string strSpace6M = "";
    //            string strSpace7M = "";
    //            string strWireRemove1 = "";
    //            string strWireRemove2 = "";
    //            string strWireRemove3 = "";
    //            string strWireRemove4 = "";
    //            string strWireRemove5 = "";
    //            string strWireRemove6 = "";
    //            string strWireRemove7 = "";


    //            if (Co1 < A)
    //            {
    //                intInterval = Math.Floor((A - Co1) / intCWSpacing);
    //                // intInterval = (intInterval)

    //                A1 = A - ((intInterval * intCWSpacing) + Co1);
    //                FB = A1 - intFactor;

    //                if (Shapecode.Contains("1CR") | Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("ES1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN6") | Shapecode.Contains("TS6") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25"))
    //                {
    //                    if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                        FBStrStatus = "Pass";
    //                    else if (FB > strBendLengths[2])
    //                        FBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B;
    //                        A1 = A1 + 20;
    //                        FB = A1 - intFactor;
    //                        if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                        {
    //                            FBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            strWireRemove1 = System.Convert.ToString(CW_A);
    //                        }
    //                        else if (FB > strBendLengths[2])
    //                        {
    //                            FBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            strWireRemove1 = System.Convert.ToString(CW_A);
    //                        }
    //                        else
    //                        {
    //                            A1 = A1 + 50;
    //                            FB = A1 - intFactor;
    //                            if (FB > strBendLengths[0] & FB < strBendLengths[1])
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("1C"))
    //                                    CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                strWireRemove1 = System.Convert.ToString(CW_A);
    //                            }
    //                            else if (FB > strBendLengths[2])
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("1C"))
    //                                    CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                strWireRemove1 = System.Convert.ToString(CW_A);
    //                            }
    //                            else
    //                                FBStrStatus = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("FBR1"))
    //                {
    //                    if (FB > strBendLengths[0])
    //                        FBStrStatus = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("1C"))
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B;
    //                        A1 = A1 + 20;
    //                        FB = A1 - intFactor;

    //                        if (FB > strBendLengths[0])
    //                        {
    //                            FBStrStatus = "Pass";
    //                            strSpaceLength = A1 + B1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            strWireRemove1 = System.Convert.ToString(CW_A);
    //                        }
    //                        else
    //                        {
    //                            A1 = A1 + 50;
    //                            FB = A1 - intFactor;
    //                            if (FB > strBendLengths[0])
    //                            {
    //                                FBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("1C"))
    //                                    CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                strWireRemove1 = System.Convert.ToString(CW_A);
    //                            }
    //                            else
    //                                FBStrStatus = "Fail";
    //                        }
    //                    }

    //                    B1 = intCWSpacing - A1;
    //                    SB = B1 + intFactor;

    //                    if (Shapecode.Contains("1CR") | Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("ES1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN6") | Shapecode.Contains("TS6") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25"))
    //                    {
    //                        if (SB > strBendLengths[3])
    //                            SBStrStatus = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            B1 = B1 + 20;
    //                            SB = B1 + intFactor;
    //                            if (SB > strBendLengths[3])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("1C"))
    //                                    CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                strWireRemove1 = System.Convert.ToString(CW_A);
    //                            }
    //                            else
    //                            {
    //                                B1 = B1 + 20;
    //                                SB = B1 + intFactor;
    //                                if (SB > strBendLengths[3])
    //                                {
    //                                    SBStrStatus = "Pass";
    //                                    strSpaceLength = A1 + B1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("1C"))
    //                                        CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B;
    //                                    strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                    strWireRemove1 = System.Convert.ToString(CW_A);
    //                                }
    //                                else
    //                                    SBStrStatus = "Fail";
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("ENR7") | Shapecode.Contains("FBR1"))
    //                    {
    //                        if (SB > strBendLengths[1])
    //                            SBStrStatus = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            B1 = B1 + 20;
    //                            SB = B1 + intFactor;
    //                            if (SB > strBendLengths[1])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                strspace = strspace + System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                            }
    //                            else
    //                            {
    //                                B1 = B1 + 50;
    //                                SB = B1 + intFactor;
    //                                if (SB > strBendLengths[1])
    //                                {
    //                                    SBStrStatus = "Pass";
    //                                    strSpaceLength = A1 + B1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("1C"))
    //                                        CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B;
    //                                    strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                    strWireRemove1 = System.Convert.ToString(CW_A);
    //                                }
    //                                else
    //                                    SBStrStatus = "Fail";
    //                            }
    //                        }
    //                    }

    //                    if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7"))
    //                    {
    //                        intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                        B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                        FB1 = B2 - intFactor;

    //                        if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                        {
    //                            if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                                FBStrStatus2B = "Pass";
    //                            else if (FB1 > strBendLengths[2])
    //                                FBStrStatus2B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                B2 = B2 + 20;
    //                                FB1 = B2 - intFactor;
    //                                if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                    strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                }
    //                                else if (FB1 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                    strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                }
    //                                else
    //                                {
    //                                    B2 = B2 + 50;
    //                                    FB1 = B2 - intFactor;
    //                                    if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                                    {
    //                                        FBStrStatus2B = "Pass";
    //                                        strSpaceLength = B2 + C1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("2M"))
    //                                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        intNumWire = CW_A + CW_B + CW_C;
    //                                        strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                        strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                    }
    //                                    else if (FB1 > strBendLengths[2])
    //                                    {
    //                                        FBStrStatus2B = "Pass";
    //                                        strSpaceLength = B2 + C1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("2M"))
    //                                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        intNumWire = CW_A + CW_B + CW_C;
    //                                        strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                        strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                    }
    //                                    else
    //                                        FBStrStatus2B = "Fail";
    //                                }
    //                            }
    //                        }
    //                        else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("FBR1"))
    //                        {
    //                            if (FB1 > strBendLengths[0])
    //                                FBStrStatus2B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                B2 = B2 + 20;
    //                                FB1 = B2 - intFactor;

    //                                if (FB1 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                    strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                }
    //                                else
    //                                {
    //                                    B2 = B2 + 50;
    //                                    FB1 = B2 - intFactor;
    //                                    if (FB1 > strBendLengths[0])
    //                                    {
    //                                        FBStrStatus2B = "Pass";
    //                                        strSpaceLength = B2 + C1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("2M"))
    //                                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        intNumWire = CW_A + CW_B + CW_C;
    //                                        strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                        strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                    }
    //                                    else
    //                                        FBStrStatus2B = "Fail";
    //                                }
    //                            }
    //                        }

    //                        C1 = intCWSpacing - B2;
    //                        SB1 = C1 + intFactor;

    //                        if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                        {
    //                            if (SB1 > strBendLengths[3])
    //                                SBStrStatus2B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                C1 = C1 + 20;
    //                                SB1 = C1 + intFactor;
    //                                if (SB1 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                    strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                }
    //                                else
    //                                {
    //                                    C1 = C1 + 50;
    //                                    SB1 = C1 + intFactor;
    //                                    if (SB1 > strBendLengths[3])
    //                                    {
    //                                        SBStrStatus2B = "Pass";
    //                                        strSpaceLength = B2 + C1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("2M"))
    //                                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        intNumWire = CW_A + CW_B + CW_C;
    //                                        strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                        strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                    }
    //                                    else
    //                                        SBStrStatus2B = "Fail";
    //                                }
    //                            }
    //                        }
    //                        else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("FBR1"))
    //                        {
    //                            if (SB1 > strBendLengths[1])
    //                                SBStrStatus2B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                C1 = C1 + 20;
    //                                SB1 = C1 + intFactor;
    //                                if (SB1 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                    strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                }
    //                                else
    //                                {
    //                                    C1 = C1 + 50;
    //                                    SB1 = C1 + intFactor;
    //                                    if (SB1 > strBendLengths[1])
    //                                    {
    //                                        SBStrStatus2B = "Pass";
    //                                        strSpaceLength = B2 + C1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("2M"))
    //                                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        intNumWire = CW_A + CW_B + CW_C;
    //                                        strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                        strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                    }
    //                                    else
    //                                        SBStrStatus2B = "Fail";
    //                                }
    //                            }
    //                        }
    //                    }

    //                    if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("TS7"))
    //                    {
    //                        intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                        C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                        FB2 = C2 - intFactor;
    //                        if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                        {
    //                            if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                                FBStrStatus3B = "Pass";
    //                            else if (FB2 > strBendLengths[2])
    //                                FBStrStatus3B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;

    //                                C2 = C2 + 20;
    //                                FB2 = C2 - intFactor;
    //                                if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                    strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                }
    //                                else if (FB2 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                    strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                }
    //                                else
    //                                {
    //                                    C2 = C2 + 50;
    //                                    FB2 = C2 - intFactor;
    //                                    if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                                    {
    //                                        FBStrStatus3B = "Pass";
    //                                        strSpaceLength = C2 + D1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("2M"))
    //                                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                        strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                        strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                    }
    //                                    else if (FB2 > strBendLengths[2])
    //                                    {
    //                                        FBStrStatus3B = "Pass";
    //                                        strSpaceLength = C2 + D1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("2M"))
    //                                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                        strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                        strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                    }
    //                                    else
    //                                        FBStrStatus3B = "Fail";
    //                                }
    //                            }
    //                        }
    //                        else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                        {
    //                            if (FB2 > strBendLengths[0])
    //                                FBStrStatus3B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3M"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                C2 = C2 + 20;
    //                                FB2 = C2 - intFactor;
    //                                if (FB2 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("3M"))
    //                                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                    strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                }
    //                                else
    //                                {
    //                                    C2 = C2 + 50;
    //                                    FB2 = C2 - intFactor;
    //                                    if (FB2 > strBendLengths[0])
    //                                    {
    //                                        FBStrStatus3B = "Pass";
    //                                        strSpaceLength = C2 + D1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("3M"))
    //                                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                        strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                        strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                    }
    //                                    else
    //                                        FBStrStatus3B = "Fail";
    //                                }
    //                            }
    //                        }

    //                        D1 = intCWSpacing - C2;
    //                        SB2 = D1 + intFactor;

    //                        if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                        {
    //                            if (SB2 > strBendLengths[3])
    //                                SBStrStatus3B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3M"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                D1 = D1 + 20;
    //                                SB2 = D1 + intFactor;
    //                                if (SB2 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("3M"))
    //                                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                    strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                }
    //                                else
    //                                {
    //                                    D1 = D1 + 20;
    //                                    SB2 = D1 + intFactor;
    //                                    if (SB2 > strBendLengths[3])
    //                                    {
    //                                        SBStrStatus3B = "Pass";
    //                                        strSpaceLength = C2 + D1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("3M"))
    //                                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                        strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                        strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                    }
    //                                    else
    //                                        SBStrStatus3B = "Fail";
    //                                }
    //                            }
    //                        }
    //                        else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                        {
    //                            if (SB2 > strBendLengths[1])
    //                                SBStrStatus3B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3M"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                D1 = D1 + 20;
    //                                SB2 = D1 + intFactor;
    //                                if (SB2 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus3B = "Pass";
    //                                    strSpaceLength = C2 + D1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("3M"))
    //                                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                    strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                    strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                }
    //                                else
    //                                {
    //                                    D1 = D1 + 50;
    //                                    SB2 = D1 + intFactor;
    //                                    if (SB2 > strBendLengths[1])
    //                                    {
    //                                        SBStrStatus3B = "Pass";
    //                                        strSpaceLength = C2 + D1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("3M"))
    //                                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                        strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                        strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                                    }
    //                                    else
    //                                        SBStrStatus3B = "Fail";
    //                                }
    //                            }
    //                        }
    //                    }




    //                    if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                    {
    //                        intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                        D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                        FB3 = D2 - intFactor;
    //                        if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                        {
    //                            if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                                FBStrStatus4B = "Pass";
    //                            else if (FB3 > strBendLengths[2])
    //                                FBStrStatus4B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4C"))
    //                                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;


    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                D2 = D2 + 20;
    //                                FB3 = D2 - intFactor;
    //                                if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("4C"))
    //                                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                    strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                }
    //                                else if (FB3 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("4C"))
    //                                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                    strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                }
    //                                else
    //                                {
    //                                    D2 = D2 + 50;
    //                                    FB3 = D2 - intFactor;
    //                                    if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                                    {
    //                                        FBStrStatus4B = "Pass";
    //                                        strSpaceLength = D2 + E1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("4C"))
    //                                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                        strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                        strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                    }
    //                                    else if (FB3 > strBendLengths[2])
    //                                    {
    //                                        FBStrStatus4B = "Pass";
    //                                        strSpaceLength = D2 + E1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("4C"))
    //                                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                        strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                        strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                    }
    //                                    else
    //                                        FBStrStatus4B = "Fail";
    //                                }
    //                            }
    //                        }
    //                        else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                        {
    //                            if (FB3 > strBendLengths[0])
    //                                FBStrStatus4B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                D2 = D2 + 20;
    //                                FB3 = D2 - intFactor;
    //                                if (FB3 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("4C"))
    //                                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                    strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                }
    //                                else
    //                                {
    //                                    D2 = D2 + 50;
    //                                    FB3 = D2 - intFactor;
    //                                    if (FB3 > strBendLengths[0])
    //                                    {
    //                                        FBStrStatus4B = "Pass";
    //                                        strSpaceLength = D2 + E1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("4C"))
    //                                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                        strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                        strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                    }
    //                                    else
    //                                        FBStrStatus4B = "Fail";
    //                                }
    //                            }
    //                        }

    //                        E1 = intCWSpacing - D2;
    //                        SB3 = E1 + intFactor;
    //                        if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                        {
    //                            if (SB3 > strBendLengths[3])
    //                                SBStrStatus4B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                E1 = E1 + 20;
    //                                SB3 = E1 + intFactor;
    //                                if (SB3 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("4C"))
    //                                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                    strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                }
    //                                else
    //                                {
    //                                    E1 = E1 + 50;
    //                                    SB3 = E1 + intFactor;
    //                                    if (SB3 > strBendLengths[3])
    //                                    {
    //                                        SBStrStatus4B = "Pass";
    //                                        strSpaceLength = D2 + E1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("4C"))
    //                                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                        strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                        strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                    }
    //                                    else
    //                                        SBStrStatus4B = "Fail";
    //                                }
    //                            }
    //                        }
    //                        else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                        {
    //                            if (SB3 > strBendLengths[1])
    //                                SBStrStatus4B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                E1 = E1 + 20;
    //                                SB3 = E1 + intFactor;
    //                                if (SB3 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus4B = "Pass";
    //                                    strSpaceLength = D2 + E1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("4C"))
    //                                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                    strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                    strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                }
    //                                else
    //                                {
    //                                    E1 = E1 + 50;
    //                                    SB3 = E1 + intFactor;
    //                                    if (SB3 > strBendLengths[1])
    //                                    {
    //                                        SBStrStatus4B = "Pass";
    //                                        strSpaceLength = D2 + E1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        if (Shapecode.Contains("4C"))
    //                                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                        else
    //                                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;

    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                        strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                        strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                                    }
    //                                    else
    //                                        SBStrStatus4B = "Fail";
    //                                }
    //                            }
    //                        }
    //                    }

    //                    if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                    {
    //                        intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                        E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                        FB4 = E2 - intFactor;
    //                        if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                        {
    //                            if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                                FBStrStatus4B = "Pass";
    //                            else if (FB4 > strBendLengths[2])
    //                                FBStrStatus5B = "Pass";
    //                            else
    //                            {
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                E2 = E2 + 20;
    //                                FB4 = E2 - intFactor;
    //                                if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus4B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;

    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                    strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                }
    //                                else if (FB4 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;

    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                    strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                }
    //                                else
    //                                {
    //                                    E2 = E2 + 50;
    //                                    FB4 = E2 - intFactor;
    //                                    if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                                    {
    //                                        FBStrStatus4B = "Pass";
    //                                        strSpaceLength = E2 + F1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;

    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                        strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                        strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                    }
    //                                    else if (FB4 > strBendLengths[2])
    //                                    {
    //                                        FBStrStatus5B = "Pass";
    //                                        strSpaceLength = E2 + F1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;

    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                        strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                        strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                    }
    //                                    else
    //                                        FBStrStatus5B = "Fail";
    //                                }
    //                            }
    //                        }
    //                        else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                        {
    //                            if (FB4 > strBendLengths[0])
    //                                FBStrStatus5B = "Pass";
    //                            else
    //                            {
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                E2 = E2 + 20;
    //                                FB4 = E2 - intFactor;
    //                                if (FB4 > strBendLengths[0])
    //                                {
    //                                    FBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;

    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                    strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                }
    //                                else
    //                                {
    //                                    E2 = E2 + 50;
    //                                    FB4 = E2 - intFactor;
    //                                    if (FB4 > strBendLengths[0])
    //                                    {
    //                                        FBStrStatus5B = "Pass";
    //                                        strSpaceLength = E2 + F1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;

    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                        strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                        strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                    }
    //                                    else
    //                                        FBStrStatus5B = "Fail";
    //                                }
    //                            }
    //                        }

    //                        F1 = intCWSpacing - E2;
    //                        SB4 = F1 + intFactor;
    //                        if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                        {
    //                            if (SB4 > strBendLengths[3])
    //                                SBStrStatus5B = "Pass";
    //                            else
    //                            {
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                F1 = F1 + 20;
    //                                SB4 = F1 + intFactor;
    //                                if (SB4 > strBendLengths[3])
    //                                {
    //                                    SBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;

    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                    strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                }
    //                                else
    //                                {
    //                                    F1 = F1 + 50;
    //                                    SB4 = F1 + intFactor;
    //                                    if (SB4 > strBendLengths[3])
    //                                    {
    //                                        SBStrStatus5B = "Pass";
    //                                        strSpaceLength = E2 + F1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;

    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                        strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                        strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                    }
    //                                    else
    //                                        SBStrStatus5B = "Fail";
    //                                }
    //                            }
    //                        }
    //                        else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                        {
    //                            if (SB4 > strBendLengths[1])
    //                                SBStrStatus5B = "Pass";
    //                            else
    //                            {
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                F1 = F1 + 20;
    //                                SB4 = F1 + intFactor;
    //                                if (SB4 > strBendLengths[1])
    //                                {
    //                                    SBStrStatus5B = "Pass";
    //                                    strSpaceLength = E2 + F1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                    CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;

    //                                    intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                    strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                    strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                }
    //                                else
    //                                {
    //                                    F1 = F1 + 50;
    //                                    SB4 = F1 + intFactor;
    //                                    if (SB4 > strBendLengths[1])
    //                                    {
    //                                        SBStrStatus5B = "Pass";
    //                                        strSpaceLength = E2 + F1;
    //                                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;

    //                                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                        strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                        strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                                    }
    //                                    else
    //                                        SBStrStatus5B = "Fail";
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (Co1 > A)
    //                {
    //                    B1 = Co1 - A;
    //                    FB = B1 - intFactor;

    //                    if (Shapecode.Contains("1CR") | Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4") | Shapecode.Contains("EC1") | Shapecode.Contains("EC1N") | Shapecode.Contains("EC1S") | Shapecode.Contains("EC2") | Shapecode.Contains("EN1") | Shapecode.Contains("EN5") | Shapecode.Contains("EN10") | Shapecode.Contains("ENR12") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR6") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES10") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TN5") | Shapecode.Contains("TN7") | Shapecode.Contains("TS1") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("EN17") | Shapecode.Contains("ES17") | Shapecode.Contains("EN16") | Shapecode.Contains("ES16") | Shapecode.Contains("ES1") | Shapecode.Contains("FN10") | Shapecode.Contains("FS10") | Shapecode.Contains("EN8") | Shapecode.Contains("ES8") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24") | Shapecode.Contains("TN26") | Shapecode.Contains("TS26") | Shapecode.Contains("TN6") | Shapecode.Contains("TS6") | Shapecode.Contains("TN25") | Shapecode.Contains("TS25"))
    //                    {
    //                        if (SB > strBendLengths[3])
    //                            SBStrStatus = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("1C"))
    //                                CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B;
    //                            B1 = B1 + 20;
    //                            FB = B1 - intFactor;
    //                            if (SB > strBendLengths[3])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("1C"))
    //                                    CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                strWireRemove1 = System.Convert.ToString(CW_A);
    //                            }
    //                            else
    //                            {
    //                                B1 = B1 + 50;
    //                                FB = B1 - intFactor;
    //                                if (SB > strBendLengths[3])
    //                                {
    //                                    SBStrStatus = "Pass";
    //                                    strSpaceLength = A1 + B1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("1C"))
    //                                        CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B;
    //                                    strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                    strWireRemove1 = System.Convert.ToString(CW_A);
    //                                }
    //                                else
    //                                    SBStrStatus = "Fail";
    //                            }
    //                        }
    //                    }
    //                    else if (Shapecode.Contains("1C") | Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("EN12") | Shapecode.Contains("EN6") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR5") | Shapecode.Contains("FBR1"))
    //                    {
    //                        if (SB > strBendLengths[1])
    //                            SBStrStatus = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B;
    //                            B1 = B1 + 20;
    //                            FB = B1 - intFactor;
    //                            if (SB > strBendLengths[1])
    //                            {
    //                                SBStrStatus = "Pass";
    //                                strSpaceLength = A1 + B1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("1C"))
    //                                    CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B;
    //                                strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                strWireRemove1 = System.Convert.ToString(CW_A);
    //                            }
    //                            else
    //                            {
    //                                B1 = B1 + 50;
    //                                FB = B1 - intFactor;
    //                                if (SB > strBendLengths[1])
    //                                {
    //                                    SBStrStatus = "Pass";
    //                                    strSpaceLength = A1 + B1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("1C"))
    //                                        CW_B = Math.Floor((B - (B1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B;
    //                                    strSpace1M = System.Convert.ToString(CW_A) + ":" + strSpaceLength;
    //                                    strWireRemove1 = System.Convert.ToString(CW_A);
    //                                }
    //                                else
    //                                    SBStrStatus = "Fail";
    //                            }
    //                        }
    //                    }


    //                    if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                    {
    //                        intInterval2B = Math.Floor((B - B1) / intCWSpacing);
    //                        B2 = B - ((intInterval2B * intCWSpacing) + B1);
    //                        FB1 = B2 - intFactor;

    //                        if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            FBStrStatus2B = "Pass";
    //                        else if (FB1 > strBendLengths[2])
    //                            FBStrStatus2B = "Pass";
    //                        else
    //                        {
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            B2 = B2 + 20;
    //                            FB1 = B2 - intFactor;
    //                            if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                strspace = strspace + ";" + System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            }
    //                            else if (FB1 > strBendLengths[2])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                            }
    //                            else
    //                            {
    //                                B2 = B2 + 50;
    //                                FB1 = B2 - intFactor;
    //                                if (FB1 > strBendLengths[0] & FB1 < strBendLengths[1])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                    strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                }
    //                                else if (FB1 > strBendLengths[2])
    //                                {
    //                                    FBStrStatus2B = "Pass";
    //                                    strSpaceLength = B2 + C1;
    //                                    CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                    CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                    if (Shapecode.Contains("2M"))
    //                                        CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                    else
    //                                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                    intNumWire = CW_A + CW_B + CW_C;
    //                                    strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                    strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                                }
    //                                else
    //                                    FBStrStatus2B = "Fail";
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7"))
    //                {
    //                    if (FB1 > strBendLengths[0])
    //                        FBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2M"))
    //                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B + CW_C;
    //                        B2 = B2 + 20;
    //                        FB1 = B2 - intFactor;
    //                        if (FB1 > strBendLengths[0])
    //                        {
    //                            FBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                        }
    //                        else
    //                        {
    //                            B2 = B2 + 50;
    //                            FB1 = B2 - intFactor;
    //                            if (FB1 > strBendLengths[0])
    //                            {
    //                                FBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                            }
    //                            else
    //                                FBStrStatus2B = "Fail";
    //                        }
    //                    }
    //                }


    //                C1 = intCWSpacing - B2;
    //                SB1 = C1 + intFactor;

    //                if (Shapecode.Contains("2CR") | Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7") | Shapecode.Contains("TB11") | Shapecode.Contains("TB24"))
    //                {
    //                    if (SB1 > strBendLengths[3])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2M"))
    //                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B + CW_C;
    //                        C1 = C1 + 20;
    //                        SB1 = C1 + intFactor;
    //                        if (SB1 > strBendLengths[3])
    //                        {
    //                            SBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                        }
    //                        else
    //                        {
    //                            C1 = C1 + 50;
    //                            SB1 = C1 + intFactor;
    //                            if (SB1 > strBendLengths[3])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                            }
    //                            else
    //                                SBStrStatus2B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("2C") | Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("EC1") | Shapecode.Contains("EC2") | Shapecode.Contains("EN14") | Shapecode.Contains("EN7") | Shapecode.Contains("EN15") | Shapecode.Contains("ENR15") | Shapecode.Contains("ENR7") | Shapecode.Contains("ES14") | Shapecode.Contains("FB1") | Shapecode.Contains("FBR1") | Shapecode.Contains("TB1") | Shapecode.Contains("TB14") | Shapecode.Contains("TB17") | Shapecode.Contains("TS25") | Shapecode.Contains("TS5") | Shapecode.Contains("TS7"))
    //                {
    //                    if (SB1 > strBendLengths[1])
    //                        SBStrStatus2B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("2M"))
    //                            CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B + CW_C;
    //                        C1 = C1 + 20;
    //                        SB1 = C1 + intFactor;
    //                        if (SB1 > strBendLengths[1])
    //                        {
    //                            SBStrStatus2B = "Pass";
    //                            strSpaceLength = B2 + C1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("2M"))
    //                                CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C;
    //                            strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                            strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                        }
    //                        else
    //                        {
    //                            C1 = C1 + 20;
    //                            SB1 = C1 + intFactor;
    //                            if (SB1 > strBendLengths[1])
    //                            {
    //                                SBStrStatus2B = "Pass";
    //                                strSpaceLength = B2 + C1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("2M"))
    //                                    CW_C = Math.Floor((C - (C1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C;
    //                                strSpace2M = System.Convert.ToString(CW_A + CW_B) + ":" + strSpaceLength;
    //                                strWireRemove2 = System.Convert.ToString(CW_A + CW_B);
    //                            }
    //                            else
    //                                SBStrStatus2B = "Fail";
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                {
    //                    intInterval3B = Math.Floor((C - C1) / intCWSpacing);
    //                    C2 = C - ((intInterval3B * intCWSpacing) + C1);
    //                    FB2 = C2 - intFactor;

    //                    if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                        FBStrStatus3B = "Pass";
    //                    else if (FB2 > strBendLengths[2])
    //                        FBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("3C"))
    //                            CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;

    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        C2 = C2 + 20;
    //                        FB2 = C2 - intFactor;
    //                        if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                        }
    //                        else if (FB2 > strBendLengths[2])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                        }
    //                        else
    //                        {
    //                            C2 = C2 + 50;
    //                            FB2 = C2 - intFactor;
    //                            if (FB2 > strBendLengths[0] & FB2 < strBendLengths[1])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3C"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                            }
    //                            else if (FB2 > strBendLengths[2])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3C"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                            }
    //                            else
    //                                FBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5") | Shapecode.Contains("TS7"))
    //                {
    //                    if (FB2 > strBendLengths[0])
    //                        FBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        C2 = C2 + 20;
    //                        FB2 = C2 - intFactor;
    //                        if (FB2 > strBendLengths[0])
    //                        {
    //                            FBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                        }
    //                        else
    //                        {
    //                            C2 = C2 + 50;
    //                            FB2 = C2 - intFactor;
    //                            if (FB2 > strBendLengths[0])
    //                            {
    //                                FBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3C"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                            }
    //                            else
    //                                FBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                }

    //                D1 = intCWSpacing - C2;
    //                SB2 = D1 + intFactor;

    //                if (Shapecode.Contains("3CR") | Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3"))
    //                {
    //                    if (SB2 > strBendLengths[3])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        D1 = D1 + 20;
    //                        SB2 = D1 + intFactor;
    //                        if (SB2 > strBendLengths[3])
    //                        {
    //                            SBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                        }
    //                        else
    //                        {
    //                            D1 = D1 + 50;
    //                            SB2 = D1 + intFactor;
    //                            if (SB2 > strBendLengths[3])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3C"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                            }
    //                            else
    //                                SBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("3C") | Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB2 > strBendLengths[1])
    //                        SBStrStatus3B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        D1 = D1 + 20;
    //                        SB2 = D1 + intFactor;
    //                        if (SB2 > strBendLengths[1])
    //                        {
    //                            SBStrStatus3B = "Pass";
    //                            strSpaceLength = C2 + D1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("3C"))
    //                                CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                            strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                            strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                        }
    //                        else
    //                        {
    //                            D1 = D1 + 50;
    //                            SB2 = D1 + intFactor;
    //                            if (SB2 > strBendLengths[1])
    //                            {
    //                                SBStrStatus3B = "Pass";
    //                                strSpaceLength = C2 + D1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("3C"))
    //                                    CW_D = Math.Floor((D - (D1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                                strSpace3M = System.Convert.ToString(CW_A + CW_B + CW_C) + ":" + strSpaceLength;
    //                                strWireRemove3 = System.Convert.ToString(CW_A + CW_B + CW_C);
    //                            }
    //                            else
    //                                SBStrStatus3B = "Fail";
    //                        }
    //                    }
    //                }


    //                if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    intInterval4B = Math.Floor((D - D1) / intCWSpacing);
    //                    D2 = D - ((intInterval4B * intCWSpacing) + D1);
    //                    FB3 = D2 - intFactor;
    //                    if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                        FBStrStatus4B = "Pass";
    //                    else if (FB3 > strBendLengths[2])
    //                        FBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        if (Shapecode.Contains("4C"))
    //                            CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        else
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;

    //                        D2 = D2 + 20;
    //                        FB3 = D2 - intFactor;
    //                        if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                        }
    //                        else if (FB3 > strBendLengths[2])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                        }
    //                        else
    //                        {
    //                            D2 = D2 + 20;
    //                            FB3 = D2 - intFactor;
    //                            if (FB3 > strBendLengths[0] & FB3 < strBendLengths[1])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4C"))
    //                                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                            }
    //                            else if (FB3 > strBendLengths[2])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4C"))
    //                                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                            }
    //                            else
    //                                FBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (FB3 > strBendLengths[0])
    //                        FBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        D2 = D2 + 20;
    //                        FB3 = D2 - intFactor;
    //                        if (FB3 > strBendLengths[0])
    //                        {
    //                            FBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                        }
    //                        else
    //                        {
    //                            D2 = D2 + 50;
    //                            FB3 = D2 - intFactor;
    //                            if (FB3 > strBendLengths[0])
    //                            {
    //                                FBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4C"))
    //                                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                            }
    //                            else
    //                                FBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }

    //                E1 = intCWSpacing - D2;
    //                SB3 = E1 + intFactor;
    //                if (Shapecode.Contains("4CR") | Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB3 > strBendLengths[3])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        E1 = E1 + 20;
    //                        SB3 = E1 + intFactor;
    //                        if (SB3 > strBendLengths[3])
    //                        {
    //                            SBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                        }
    //                        else
    //                        {
    //                            E1 = E1 + 20;
    //                            SB3 = E1 + intFactor;
    //                            if (SB3 > strBendLengths[3])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4C"))
    //                                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                            }
    //                            else
    //                                SBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("4C") | Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB3 > strBendLengths[1])
    //                        SBStrStatus4B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D;
    //                        E1 = E1 + 20;
    //                        SB3 = E1 + intFactor;
    //                        if (SB3 > strBendLengths[1])
    //                        {
    //                            SBStrStatus4B = "Pass";
    //                            strSpaceLength = D2 + E1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            if (Shapecode.Contains("4C"))
    //                                CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                            else
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                            strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                            strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                        }
    //                        else
    //                        {
    //                            E1 = E1 + 20;
    //                            SB3 = E1 + intFactor;
    //                            if (SB3 > strBendLengths[1])
    //                            {
    //                                SBStrStatus4B = "Pass";
    //                                strSpaceLength = D2 + E1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                if (Shapecode.Contains("4C"))
    //                                    CW_E = Math.Floor((E - (E1 + Mo2)) / intCWSpacing) + 1;
    //                                else
    //                                    CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E;
    //                                strSpace4M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D) + ":" + strSpaceLength;
    //                                strWireRemove4 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D);
    //                            }
    //                            else
    //                                SBStrStatus4B = "Fail";
    //                        }
    //                    }
    //                }

    //                if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    intInterval5B = Math.Floor((E - E1) / intCWSpacing);
    //                    E2 = E - ((intInterval5B * intCWSpacing) + E1);
    //                    FB4 = E2 - intFactor;
    //                    if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                        FBStrStatus5B = "Pass";
    //                    else if (FB4 > strBendLengths[2])
    //                        FBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                        E2 = E2 + 20;
    //                        FB4 = E2 - intFactor;
    //                        if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                        }
    //                        else if (FB4 > strBendLengths[2])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                        }
    //                        else
    //                        {
    //                            E2 = E2 + 50;
    //                            FB4 = E2 - intFactor;
    //                            if (FB4 > strBendLengths[0] & FB4 < strBendLengths[1])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                            }
    //                            else if (FB4 > strBendLengths[2])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                            }
    //                            else
    //                                FBStrStatus5B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (FB4 > strBendLengths[0])
    //                        FBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_F;
    //                        E2 = E2 + 20;
    //                        FB4 = E2 - intFactor;
    //                        if (FB4 > strBendLengths[0])
    //                        {
    //                            FBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                        }
    //                        else
    //                        {
    //                            E2 = E2 + 50;
    //                            FB4 = E2 - intFactor;
    //                            if (FB4 > strBendLengths[0])
    //                            {
    //                                FBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                            }
    //                            else
    //                                FBStrStatus5B = "Fail";
    //                        }
    //                    }
    //                }

    //                F1 = intCWSpacing - E2;
    //                SB4 = F1 + intFactor;
    //                if (Shapecode.Contains("5CR") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB4 > strBendLengths[3])
    //                        SBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_F;
    //                        F1 = F1 + 20;
    //                        SB4 = F1 + intFactor;
    //                        if (SB4 > strBendLengths[3])
    //                        {
    //                            SBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                        }
    //                        else
    //                        {
    //                            F1 = F1 + 50;
    //                            SB4 = F1 + intFactor;
    //                            if (SB4 > strBendLengths[3])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                            }
    //                            else
    //                                SBStrStatus5B = "Fail";
    //                        }
    //                    }
    //                }
    //                else if (Shapecode.Contains("5C") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //                {
    //                    if (SB4 > strBendLengths[1])
    //                        SBStrStatus5B = "Pass";
    //                    else
    //                    {
    //                        CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                        CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                        CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                        CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                        CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                        CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                        intNumWire = CW_A + CW_B + CW_C + CW_D + CW_F;
    //                        F1 = F1 + 20;
    //                        SB4 = F1 + intFactor;
    //                        if (SB4 > strBendLengths[1])
    //                        {
    //                            SBStrStatus5B = "Pass";
    //                            strSpaceLength = E2 + F1;
    //                            CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                            CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                            CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                            CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                            CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                            CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                            intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                            strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                            strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                        }
    //                        else
    //                        {
    //                            F1 = F1 + 50;
    //                            SB4 = F1 + intFactor;
    //                            if (SB4 > strBendLengths[1])
    //                            {
    //                                SBStrStatus5B = "Pass";
    //                                strSpaceLength = E2 + F1;
    //                                CW_A = Math.Floor((A - Mo1) / intCWSpacing) + 1;
    //                                CW_B = Math.Floor((B - (B1 + B2)) / intCWSpacing) + 1;
    //                                CW_C = Math.Floor((C - (C1 + C2)) / intCWSpacing) + 1;
    //                                CW_D = Math.Floor((D - (D1 + D2)) / intCWSpacing) + 1;
    //                                CW_E = Math.Floor((E - (E1 + E2)) / intCWSpacing) + 1;
    //                                CW_F = Math.Floor((F - (F1 + Mo2)) / intCWSpacing) + 1;
    //                                intNumWire = CW_A + CW_B + CW_C + CW_D + CW_E + CW_F;
    //                                strSpace5M = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E) + ":" + strSpaceLength;
    //                                strWireRemove5 = System.Convert.ToString(CW_A + CW_B + CW_C + CW_D + CW_E);
    //                            }
    //                            else
    //                                SBStrStatus5B = "Fail";
    //                        }
    //                    }
    //                }
    //            }
    //            else if (((intInterval * intCWSpacing) + Co1) == A)
    //            {
    //                FBStrStatus = "Fail";
    //                SBStrStatus = "Fail";
    //                FBStrStatus2B = "Fail";
    //                SBStrStatus2B = "Fail";
    //                FBStrStatus3B = "Fail";
    //                SBStrStatus3B = "Fail";
    //                FBStrStatus4B = "Fail";
    //                SBStrStatus4B = "Fail";
    //                FBStrStatus5B = "Fail";
    //                SBStrStatus5B = "Fail";
    //            }

    //            if (Shapecode.Contains("5C") | Shapecode.Contains("DF1") | Shapecode.Contains("DF3") | Shapecode.Contains("DF4") | Shapecode.Contains("DF5"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus4B != "" & FBStrStatus4B != "" & SBStrStatus5B != "" & FBStrStatus5B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass" & FBStrStatus5B == "Pass" & SBStrStatus5B == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("4C"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "" & FBStrStatus4B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass" & FBStrStatus4B == "Pass" & SBStrStatus4B == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("3C"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "" & FBStrStatus3B != "" & SBStrStatus3B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass" & FBStrStatus3B == "Pass" & SBStrStatus3B == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("2C"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "" & FBStrStatus2B != "" & SBStrStatus2B != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass" & FBStrStatus2B == "Pass" & SBStrStatus2B == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //            else if (Shapecode.Contains("1C") | Shapecode.Contains("DN5") | Shapecode.Contains("DN9") | Shapecode.Contains("DN12") | Shapecode.Contains("DS5") | Shapecode.Contains("DS9") | Shapecode.Contains("DS12") | Shapecode.Contains("DN6") | Shapecode.Contains("DN8") | Shapecode.Contains("DN13") | Shapecode.Contains("DS6") | Shapecode.Contains("DS8") | Shapecode.Contains("DS13") | Shapecode.Contains("DN10") | Shapecode.Contains("DS10") | Shapecode.Contains("DN1") | Shapecode.Contains("DN2") | Shapecode.Contains("DN3") | Shapecode.Contains("DN5") | Shapecode.Contains("DS1") | Shapecode.Contains("DS2") | Shapecode.Contains("DS3") | Shapecode.Contains("DS4"))
    //            {
    //                if (FBStrStatus != "" & SBStrStatus != "")
    //                {
    //                    if (FBStrStatus == "Pass" & SBStrStatus == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //                else if (SBStrStatus == "")
    //                {
    //                    if (FBStrStatus == "Pass")
    //                        StrStatus = "Pass";
    //                    else
    //                        StrStatus = "Fail";
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //        return StrStatus;
    //    }
    //}
}
