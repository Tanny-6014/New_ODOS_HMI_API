namespace DetailingService.Repositories
{
    public class BendingAdjusment
    {
        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";

        #region "Variables"
        public string ParamName { get; set; }
        public int ParamValue { get; set; }
        public string AngleType { get; set; }
        public int AngleDir { get; set; }
        public string WireType { get; set; }
        public int Bend1 { get; set; }
        public int Bend2 { get; set; }
        public int WireLength1 { get; set; }
        //public int WireLength2 { get; set; }
        public int Quantity1 { get; set; }
        //public int Quantity2 { get; set; }
        public bool BendStatus1 { get; set; }
        public bool BendStatus2 { get; set; }
        public int BendLimit { get; set; }
        public bool OHStatus { get; set; }
        public int OH1 { get; set; }
        public int OH2 { get; set; }
        public bool OH1Flag { get; set; }
        public bool OH2Flag { get; set; }
        public int BendType { get; set; }
        public int angle { get; set; }
        public int MinOH { get; set; }
        public int MaxOH { get; set; }
        public int Angle { get; set; }
        public int CreepLength { get; set; }
        public int Quantity { get; set; }
        public int Spacing { get; set; }
        public bool IsSegmentUsed { get; set; }
        public bool BendFlag { get; set; }
        public bool CanRemove { get; set; }
        public bool IsSpecialCase { get; set; }
        public bool IsSegmentLessthanSpace { get; set; }
        public bool SegmentNoWire { get; set; }
        public int AdjustableSpacing { get; set; }
        public bool IsSegmentAdjusted { get; set; }
        public bool IsSegmentHit { get; set; }
        public bool Shape_5M1B_5MR1B { get; set; }
        #endregion

        #region "Methods..."

        /* Update Production BOM Details */
        public int UpdateProductionBOMDetails(int ProductMarkId, string StructureElementType, int UserId, out string message)
        {
            message = "";
            int output = 0;
            ShapeCode objShapeCode = new ShapeCode();
            ShapeCode Shape = new ShapeCode();
            BendingAdjustmentClass objBendingAdjustmentClass = new BendingAdjustmentClass();
            List<ShapeParameter> segmentList = new List<ShapeParameter>();
            int StructureElementTypeId, ProductionMO1 = 0, ProductionMO2 = 0, PinSize = 0, ProductionCWLength = 0, ProductionMWLength = 0, MWSpacing = 0, CWSpacing = 0;
            double MWDiameter = 0, CWDiameter = 0;
            //LogManager.log.Info(" * * * * Production BOM Bending Adjustment Starts * * * * ");
            //LogManager.log.Info("Product Mark Id : " + ProductMarkId);
            //LogManager.log.Info("Structure Element Type : " + StructureElementType);
            //LogManager.log.Info("User Id : " + UserId);
            string bomType = "R";
            try
            {
                /* To get the shape param details */
                var shapeParam = objShapeCode.ShapeCodeDetailsforIrregularSpacing_Get(ProductMarkId, StructureElementType);
                if (shapeParam != null && shapeParam.Count != 0)
                {
                    segmentList = shapeParam[0].ShapeParameterList;
                    Shape = shapeParam[0];
                }

                /* added by anuran CHG0031334>> */
                Shape_5M1B_5MR1B = false;
                if ((shapeParam[0].ShapeCodeName == "5M1B") || (shapeParam[0].ShapeCodeName == "5MR1B"))
                {
                    Shape_5M1B_5MR1B = true;
                }
                if (Shape_5M1B_5MR1B == false)
                {
                    if (StructureElementType == "Beam")
                    {
                        return 0;
                    }
                }
                /* added by anuran CHG0031334<< */

                /* To get the param values details */
                var paramValues = objBendingAdjustmentClass.ParamValuesByProductMarkId_Get(ProductMarkId, StructureElementType);
                //LogManager.log.Info("Shape Param Values : " + paramValues);
                //LogManager.log.Info("Shape : " + shapeParam[0].ShapeCodeName);
                //LogManager.log.Info("Shape Group : " + shapeParam[0].MeshShapeGroup);
                // paramValues = "A:460;B:970;C:300";

                StructureElementTypeId = objBendingAdjustmentClass.StructureElementTypeIdByStructureElementType_Get(StructureElementType);

                /* To get the productmarking details (Prod MO1, MO2, CWSpace, PinSize, MainWireDia) */
                var productMarkindDetails = objBendingAdjustmentClass.ProductMarkingDetailsForIrregularBOMByProductMarkId_Get(ProductMarkId, StructureElementType);
                if (productMarkindDetails != null)
                {
                    BendingAdjustmentClass objProductDetails = (BendingAdjustmentClass)productMarkindDetails;
                    ProductionMO1 = objProductDetails.ProductionMO1;
                    ProductionMO2 = objProductDetails.ProductionMO2;
                    PinSize = objProductDetails.PinSize;
                    ProductionMWLength = objProductDetails.ProductionMWLength;
                    ProductionCWLength = objProductDetails.ProductionCWLength;
                    MWDiameter = objProductDetails.MWDiameter;
                    CWDiameter = objProductDetails.CWDiameter;
                    MWSpacing = objProductDetails.MWSpacing;
                    CWSpacing = objProductDetails.CWSpacing;
                }

                /* To get the Wire Type and Wire Spacing */
                var WireDetails = objBendingAdjustmentClass.WireTypeAndWireSpacingforIrregularBOM_Get(ProductMarkId, StructureElementType);

                string ParameterValue = paramValues;
                string[] ListParamValue = ParameterValue.Split(';');
                int LHCounter = 0;
                bool LegFlag = false;
                int LegIndex = 0;

                /* Assign the Segment values into Shape Segment */
                foreach (ShapeParameter shapeParamNames in segmentList)
                {
                    foreach (string segmentParamDetails in ListParamValue)
                    {
                        if (segmentParamDetails.Contains(":"))
                        {
                            string[] arraySegmentParameters = segmentParamDetails.Split(':');
                            if (shapeParamNames.ParameterName.ToUpper() == arraySegmentParameters[0].ToUpper())
                            {
                                shapeParamNames.ParameterValue = Convert.ToInt32(arraySegmentParameters[1]);
                            }
                        }
                    }
                }

                /* Validate Leg values for Column */
                if (StructureElementType == "Column")
                {
                    foreach (ShapeParameter param in segmentList)
                    {
                        if (param.ParameterName.ToUpper().Trim() == "LEG")
                        {
                            LegFlag = true;
                            LegIndex = LHCounter;
                        }
                        LHCounter = LHCounter + 1;
                    }
                    if (LegFlag == true)
                    {
                        segmentList.Insert(0, new ShapeParameter { Angle = segmentList[LegIndex].Angle, AngleDir = segmentList[LegIndex].AngleDir, AngleType = segmentList[LegIndex].AngleType, ParameterName = segmentList[LegIndex].ParameterName, ParameterValue = segmentList[LegIndex].ParameterValue, WireType = segmentList[LegIndex].WireType, ShapeId = segmentList[LegIndex].ShapeId, SequenceNumber = 0 });
                    }
                }


                /* Validate Leg values for Column (added by Anuran CHG0031334)*/
                if (StructureElementType == "Beam")
                {
                    foreach (ShapeParameter param in segmentList)
                    {
                        if (param.ParameterName.ToUpper().Trim() == "LEG")
                        {
                            LegFlag = true;
                            LegIndex = LHCounter;
                        }
                        LHCounter = LHCounter + 1;
                    }
                    if (LegFlag == true)
                    {
                        segmentList.Insert(0, new ShapeParameter { Angle = segmentList[LegIndex].Angle, AngleDir = segmentList[LegIndex].AngleDir, AngleType = segmentList[LegIndex].AngleType, ParameterName = segmentList[LegIndex].ParameterName, ParameterValue = segmentList[LegIndex].ParameterValue, WireType = segmentList[LegIndex].WireType, ShapeId = segmentList[LegIndex].ShapeId, SequenceNumber = 0 });
                    }
                }

                List<listAngle> listAngle = new List<listAngle>();
                BendingAdjusment objBendingAdjustment = new BendingAdjusment();
                List<BendingAdjusment> listbendingAdjustment = new List<BendingAdjusment>();

                //Include code for bending logic
                //Validate if bends are present.
                //If bends present validate for which side bend present
                //validate for position
                //get the bend angle
                //calculate the creep length for all the bend
                //Calculate the calculated bend

                string MWPitch = "";
                string CWPitch = "";
                int MWFlag = 0;
                int CWFlag = 0;


                if (Shape.NoOfBends > 0)
                {
                    if (Shape.NoOfMWBend > 0)
                    {
                        if (Shape.MWBendPosition == 1) // start direction from MO1
                        {
                            string PitchValue = "";
                            listAngle = GetAngle(Shape.MWbvbsString, segmentList, out message);
                            List<ShapeParameter> shapeParamColl = (from ShapeParameter sp in segmentList
                                                                   where sp.AngleType == "S" && sp.WireType == "M"
                                                                   select sp).ToList();

                            List<BendingAdjustmentClass> mainWireDetails = (from BendingAdjustmentClass bendAdj in WireDetails
                                                                            where bendAdj.WireType == "C"
                                                                            select bendAdj).ToList();

                            foreach (ShapeParameter sp in segmentList)
                            {
                                foreach (listAngle la in listAngle)
                                {
                                    if (sp.ParameterName == la.paramName)
                                    {
                                        sp.Angle = la.Angle;
                                    }
                                }
                            }

                            if (StructureElementType == "Column")
                            {
                                segmentList[segmentList.Count - 1].Angle = "0";
                            }

                            if (StructureElementType == "Beam")
                            {
                                segmentList[segmentList.Count - 1].Angle = "0";
                            }

                            /* (added by Anuran CHG0031334)*/

                            foreach (BendingAdjustmentClass bs in mainWireDetails)
                            {
                                string WireSpacing = bs.WireSpacing;
                                string WireType = bs.WireType;
                                string[] ArrayBomvalue = WireSpacing.Split(',');
                                //LogManager.log.Info("Bend Details: Bend in MW @ OH1");
                                if (StructureElementType == "Column")
                                {
                                    PitchValue = objBendingAdjustment.BendAdjustmentforColumn(shapeParamColl, ProductionMO1, ProductionMO2, CWSpacing, PinSize, MWDiameter, Shape.MWBendPosition, mainWireDetails, out message, ref bomType, StructureElementType);
                                    if (PitchValue != "")
                                    {
                                        MWPitch = PitchValue;
                                        MWFlag = 1;
                                    }
                                }
                                /* (added by Anuran CHG0031334)*/
                                else if (StructureElementType == "Beam")
                                {
                                    PitchValue = objBendingAdjustment.BendAdjustmentforColumn(shapeParamColl, ProductionMO1, ProductionMO2, CWSpacing, PinSize, MWDiameter, Shape.MWBendPosition, mainWireDetails, out message, ref bomType, StructureElementType);
                                    if (PitchValue != "")
                                    {
                                        MWPitch = PitchValue;
                                        MWFlag = 1;
                                    }
                                }

                                else
                                {
                                    PitchValue = objBendingAdjustment.BendAdjustmentforMesh(shapeParamColl, ProductionMO1, ProductionMO2, CWSpacing, PinSize, MWDiameter, Shape.MWBendPosition, mainWireDetails, out message, ref bomType, StructureElementType);
                                    if (PitchValue != "")
                                    {
                                        MWPitch = PitchValue;
                                        MWFlag = 1;
                                    }
                                }
                            }
                        }
                        else if (Shape.MWBendPosition == 2)// start direction from MO2
                        {
                            string PitchValue = "";
                            listAngle = GetAngle(Shape.MWbvbsString, segmentList, out message);
                            List<ShapeParameter> shapeParamColl = (from ShapeParameter sp in segmentList
                                                                   where sp.AngleType == "S" && sp.WireType == "M"
                                                                   select sp).ToList();
                            shapeParamColl = shapeParamColl.OrderByDescending(o => o.ParameterName).ToList();   //Sort the list by descending

                            List<BendingAdjustmentClass> mainWireDetails = (from BendingAdjustmentClass bendAdj in WireDetails
                                                                            where bendAdj.WireType == "C"
                                                                            select bendAdj).ToList();


                            foreach (ShapeParameter sp in segmentList)
                            {
                                foreach (listAngle la in listAngle)
                                {
                                    if (sp.ParameterName == la.paramName)
                                    {
                                        sp.Angle = la.Angle;
                                    }
                                }
                            }
                            if (StructureElementType == "Column")
                            {
                                segmentList[segmentList.Count - 1].Angle = "0";
                            }
                            foreach (BendingAdjustmentClass bs in mainWireDetails)
                            {
                                string WireSpacing = bs.WireSpacing;
                                string WireType = bs.WireType;
                                string[] ArrayBomvalue = WireSpacing.Split(',');

                                //LogManager.log.Info("Bend Details: Bend in MW @ OH2");

                                if (StructureElementType == "Column")
                                {
                                    PitchValue = objBendingAdjustment.BendAdjustmentforColumn(shapeParamColl, ProductionMO1, ProductionMO2, CWSpacing, PinSize, MWDiameter, Shape.MWBendPosition, mainWireDetails, out message, ref bomType, StructureElementType);
                                    if (PitchValue != "")
                                    {
                                        MWPitch = PitchValue;
                                        MWFlag = 1;
                                    }
                                }
                                else
                                {
                                    PitchValue = objBendingAdjustment.BendAdjustmentforMesh(shapeParamColl, ProductionMO1, ProductionMO2, CWSpacing, PinSize, MWDiameter, Shape.MWBendPosition, mainWireDetails, out message, ref bomType, StructureElementType);
                                    if (PitchValue != "")
                                    {
                                        MWPitch = PitchValue;
                                        MWFlag = 1;
                                    }
                                }
                            }
                        }
                    }
                    if (Shape.NoOfCWBend > 0)
                    {
                        if (Shape.CWBendPosition == 1)  // start direction from CO1
                        {
                            string PitchValue = "";
                            listAngle = GetAngle(Shape.CWbvbsString, segmentList, out message);
                            List<ShapeParameter> shapeParamColl = (from ShapeParameter sp in segmentList
                                                                   where sp.AngleType == "S" && sp.WireType == "C"
                                                                   select sp).ToList();
                            List<BendingAdjustmentClass> crossWireDetails = (from BendingAdjustmentClass bendAdj in WireDetails
                                                                             where bendAdj.WireType == "M"
                                                                             select bendAdj).ToList();
                            foreach (ShapeParameter sp in segmentList)
                            {
                                foreach (listAngle la in listAngle)
                                {
                                    if (sp.ParameterName == la.paramName)
                                    {
                                        sp.Angle = la.Angle;
                                    }
                                }
                            }
                            if (StructureElementType == "Column")
                            {
                                segmentList[segmentList.Count - 1].Angle = "0";
                            }

                            foreach (BendingAdjustmentClass bs in crossWireDetails)
                            {
                                string WireSpacing = bs.WireSpacing;
                                string WireType = bs.WireType;
                                string[] ArrayBomvalue = WireSpacing.Split(',');
                                //LogManager.log.Info("Bend Details: Bend in CW @ OH1");
                                if (StructureElementType == "Column")
                                {
                                    PitchValue = objBendingAdjustment.BendAdjustmentforColumn(shapeParamColl, ProductionMO1, ProductionMO2, CWSpacing, PinSize, MWDiameter, Shape.MWBendPosition, crossWireDetails, out message, ref bomType, StructureElementType);
                                    if (PitchValue != "")
                                    {
                                        CWPitch = PitchValue;
                                        CWFlag = 1;
                                    }
                                }
                                else
                                {
                                    PitchValue = objBendingAdjustment.BendAdjustmentforMesh(shapeParamColl, ProductionMO1, ProductionMO2, CWSpacing, PinSize, MWDiameter, Shape.MWBendPosition, crossWireDetails, out message, ref bomType, StructureElementType);
                                    if (PitchValue != "")
                                    {
                                        CWPitch = PitchValue;
                                        CWFlag = 1;
                                    }
                                }
                            }
                        }
                        else if (Shape.CWBendPosition == 2)     // start direction from CO2
                        {
                            string PitchValue = "";
                            listAngle = GetAngle(Shape.CWbvbsString, segmentList, out message);
                            List<ShapeParameter> shapeParamColl = (from ShapeParameter sp in segmentList
                                                                   where sp.AngleType == "S" && sp.WireType == "C"
                                                                   select sp).ToList();
                            shapeParamColl = shapeParamColl.OrderByDescending(o => o.ParameterName).ToList();   //Sort the list by descending

                            List<BendingAdjustmentClass> crossWireDetails = (from BendingAdjustmentClass bendAdj in WireDetails
                                                                             where bendAdj.WireType == "M"
                                                                             select bendAdj).ToList();
                            foreach (ShapeParameter sp in segmentList)
                            {
                                foreach (listAngle la in listAngle)
                                {
                                    if (sp.ParameterName == la.paramName)
                                    {
                                        sp.Angle = la.Angle;
                                    }
                                }
                            }
                            if (StructureElementType == "Column")
                            {
                                segmentList[segmentList.Count - 1].Angle = "0";
                            }
                            foreach (BendingAdjustmentClass bs in crossWireDetails)
                            {
                                string WireSpacing = bs.WireSpacing;
                                string WireType = bs.WireType;
                                string[] ArrayBomvalue = WireSpacing.Split(',');
                                //LogManager.log.Info("Bend Details: Bend in CW @ OH2");
                                if (StructureElementType == "Column")
                                {
                                    PitchValue = objBendingAdjustment.BendAdjustmentforColumn(shapeParamColl, ProductionMO1, ProductionMO2, CWSpacing, PinSize, MWDiameter, Shape.MWBendPosition, crossWireDetails, out message, ref bomType, StructureElementType);
                                    if (PitchValue != "")
                                    {
                                        CWPitch = PitchValue;
                                        CWFlag = 1;
                                    }
                                }
                                else
                                {
                                    PitchValue = objBendingAdjustment.BendAdjustmentforMesh(shapeParamColl, ProductionMO1, ProductionMO2, CWSpacing, PinSize, MWDiameter, Shape.MWBendPosition, crossWireDetails, out message, ref bomType, StructureElementType);
                                    if (PitchValue != "")
                                    {
                                        CWPitch = PitchValue;
                                        CWFlag = 1;
                                    }
                                }
                            }
                        }
                    }
                }

                /* Save ProductionBOMDetails */
                if (message == "")
                {
                    output = objBendingAdjustmentClass.BOMDetailsforIrregularSpacing_Insert(ProductMarkId, StructureElementTypeId, MWPitch, MWFlag, CWPitch, CWFlag, UserId, bomType);
                }
                else
                {
                    output = objBendingAdjustmentClass.BendingCheckFailed_Insert(ProductMarkId, StructureElementTypeId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return output;
        }

        /* Bending Adjustment for Mesh */
        public string BendAdjustmentforMesh(List<ShapeParameter> shapeParam, int oH1, int oH2, int wireSpacing, int pinSize, double barDia, int bendPosition, List<BendingAdjustmentClass> WireDetails, out string errorMessage, ref string bomType, string structureElementType)
        {
            string PitchValue = "";
            List<BendingAdjusment> listBendingAdjustment = new List<BendingAdjusment>();
            errorMessage = "";
            try
            {
                int ParamCountLast = shapeParam.Count;

                List<BendConstraint> listBendConstraint = new List<BendConstraint>();
                BendConstraint objBendConstraint = new BendConstraint();
                listBendConstraint = objBendConstraint.GetBendConstraint(barDia);    /* Get the bend limit value */
                List<BendingAdjusment> bomList = new List<BendingAdjusment>();
                foreach (BendingAdjustmentClass bs in WireDetails)
                {
                    int Paramcount = 1
                        , CountCreep = 0;
                    string WireSpacing = bs.WireSpacing;
                    string WireType = bs.WireType;
                    string[] ArrayBomvalue = WireSpacing.Split(',');
                    int countBomValue = 1
                        , countMaxBomValue = ArrayBomvalue.Length;

                    bool overHangFlowFlag = false;



                    foreach (string paramvalue in ArrayBomvalue)
                    {
                        //LogManager.log.Info("Detailing BOM : " + paramvalue);
                        if (paramvalue.Contains("x"))
                        {
                            string[] ArrayWireDetails = paramvalue.Split('x');
                            if (Convert.ToInt32(ArrayWireDetails[0]) > 1)
                            {
                                for (int quantity = 1; quantity <= Convert.ToInt32(ArrayWireDetails[0]); quantity++)
                                {
                                    bomList.Add(new BendingAdjusment { Quantity = 1, Spacing = Convert.ToInt32(ArrayWireDetails[1]) });
                                }
                            }
                            else
                            {
                                bomList.Add(new BendingAdjusment { Quantity = Convert.ToInt32(ArrayWireDetails[0]), Spacing = Convert.ToInt32(ArrayWireDetails[1]) });
                            }
                        }
                        countBomValue++;
                    }

                    int currentSegment = 0, currentSegmentforOH = 0;
                    int currentWire = 0, currentWireCount = 0, totalWireCount = 0;
                    bool oh2CalcDone = false;
                    foreach (ShapeParameter segment in shapeParam)   /*Iterate the param details to fetch the param value for bending logic*/
                    {
                        string AngleDir = "";
                        if (segment.AngleDir == 1)
                        {
                            AngleDir = "N";
                        }
                        else
                        {
                            AngleDir = "R";
                        }
                        int CreepLengthValue = 0;
                        int BendType = 0;
                        if (Paramcount % 2 == 0)
                        {
                            BendType = 2;
                        }
                        else
                        {
                            BendType = 1;
                        }
                        CreepLengthValue = Math.Abs(CalculateCreepLength(Convert.ToDouble(shapeParam[CountCreep].Angle), pinSize, barDia, out errorMessage)); /* Creep Calculation */
                        CreepLengthValue = Convert.ToInt32(Math.Round(CreepLengthValue / 10.0) * 10);
                        int tempBOM = 0, bend2 = 0, bend1 = 0;
                        currentSegment = segment.ParameterValue;
                        currentSegmentforOH += segment.ParameterValue;
                        bool calculateFirstBend = true, calculateSecondBend = true;

                        foreach (BendingAdjusment bomitem in bomList)
                        {
                            if (bomList[bomList.Count - 1].Spacing > shapeParam[shapeParam.Count - 1].ParameterValue && oh2CalcDone == false)
                            {
                                int segmentSpace = 0, i = 0, bendOH = 0;
                                for (i = shapeParam.Count; i > 0; i--)
                                {
                                    segmentSpace += shapeParam[i - 1].ParameterValue;
                                    if (bomList[bomList.Count - 1].Spacing > segmentSpace)
                                    {
                                        shapeParam[i - 1].IsParamUsed = true;
                                        continue;
                                    }
                                    else
                                    {
                                        int oh1CreepLength = 0;
                                        for (int creepcount = shapeParam.Count; creepcount >= i; creepcount--)
                                        {
                                            CreepLengthValue = Math.Abs(CalculateCreepLength(Convert.ToDouble(shapeParam[creepcount - 1].Angle), pinSize, barDia, out errorMessage)); /* Creep Calculation */
                                            CreepLengthValue = Convert.ToInt32(Math.Round(CreepLengthValue / 10.0) * 10);
                                            oh1CreepLength += CreepLengthValue;
                                        }
                                        CreepLengthValue = oh1CreepLength;

                                        if ((i) % 2 == 0)
                                        {
                                            List<BendConstraint> listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                                 where bc.BendType == AngleDir && bc.Sequence == 2
                                                                                                 select bc).ToList();
                                            int bomValue = segmentSpace - shapeParam[i - 1].ParameterValue;

                                            bendOH = bomList[bomList.Count - 1].Spacing - bomValue;

                                            if (bendOH < listBendConstraintLimitation[0].BendLength)
                                            {
                                                int requiredSpacing = listBendConstraintLimitation[0].BendLength - bendOH;
                                                bomList[bomList.Count - 2].Spacing -= requiredSpacing;
                                                bomList[bomList.Count - 2].IsSegmentUsed = true;

                                                bomList[bomList.Count - 1].Spacing += (requiredSpacing - CreepLengthValue);
                                                bomList[bomList.Count - 1].IsSegmentUsed = true;
                                                bomType = "E";
                                            }
                                            else
                                            {
                                                bomList[bomList.Count - 1].Spacing += -CreepLengthValue;
                                                bomList[bomList.Count - 1].IsSegmentUsed = true;
                                                bomType = "E";
                                            }
                                        }
                                        else
                                        {
                                            List<BendConstraint> listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                                 where bc.BendType == AngleDir && bc.Sequence == 1
                                                                                                 select bc).ToList();
                                            int bomValue = segmentSpace;

                                            bendOH = bomValue - bomList[bomList.Count - 1].Spacing;

                                            if (bendOH < listBendConstraintLimitation[0].BendLength)
                                            {
                                                int requiredSpacing = listBendConstraintLimitation[0].BendLength - bendOH;
                                                bomList[bomList.Count - 1].Spacing -= (requiredSpacing + CreepLengthValue);
                                                bomList[bomList.Count - 1].IsSegmentUsed = true;

                                                bomList[bomList.Count - 2].Spacing += requiredSpacing;
                                                bomList[bomList.Count - 2].IsSegmentUsed = true;
                                                bomType = "E";
                                            }
                                            else
                                            {
                                                bomList[bomList.Count - 1].Spacing += -CreepLengthValue;
                                                bomList[bomList.Count - 1].IsSegmentUsed = true;
                                                bomType = "E";
                                            }
                                        }
                                        break;
                                    }
                                }
                                oh2CalcDone = true;
                            }
                            else
                            {
                                if (Paramcount == shapeParam.Count)
                                {
                                    if ((shapeParam.Count % 2) == 0)
                                    {
                                        calculateFirstBend = false;
                                    }
                                    else
                                    {
                                        calculateSecondBend = false;
                                    }
                                }
                            }

                            if (bomList[0].Spacing < shapeParam[0].ParameterValue || overHangFlowFlag == true) // Overhang < First Segment 
                            {
                                if (bomitem.IsSegmentUsed == false)
                                {
                                    currentWire += bomitem.Spacing;

                                    if (currentWire <= currentSegment)
                                    {
                                        bomList[currentWireCount].IsSegmentUsed = true;
                                        currentWireCount += 1;
                                        continue;
                                    }
                                    else
                                    {
                                        CreepLengthValue = Math.Abs(CalculateCreepLength(Convert.ToDouble(shapeParam[CountCreep].Angle), pinSize, barDia, out errorMessage)); /* Creep Calculation */
                                        CreepLengthValue = Convert.ToInt32(Math.Round(CreepLengthValue / 10.0) * 10);

                                        if (BendType == 1)
                                        {
                                            /* Bend 2 limit constraint */
                                            List<BendConstraint> listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint  /* Filter Normal/Reverse bend */
                                                                                                 where bc.BendType == AngleDir && bc.Sequence == 2
                                                                                                 select bc).ToList();

                                            if (calculateSecondBend)
                                            {
                                                bend2 = currentWire - currentSegment;

                                                if (bend2 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    tempBOM = bomList[currentWireCount + 1].Spacing;
                                                    bomList[currentWireCount + 1].Spacing -= (listBendConstraintLimitation[0].BendLength - bend2);
                                                    bomList[currentWireCount + 1].IsSegmentUsed = true;
                                                    bomType = "E";
                                                }
                                            }


                                            if (calculateFirstBend)
                                            {
                                                /* Bend 1 limit constraint */
                                                listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                where bc.BendType == AngleDir && bc.Sequence == 1
                                                                                select bc).ToList();

                                                if (currentWire == currentSegment)
                                                {
                                                    bend1 = listBendConstraintLimitation[0].BendLength;
                                                    bomList[currentWireCount].AdjustableSpacing = bomList[currentWireCount].Spacing - bend1;
                                                    bomList[currentWireCount].IsSegmentAdjusted = true;
                                                }
                                                else
                                                {
                                                    bend1 = bomList[currentWireCount].Spacing - bend2;
                                                }

                                                if (bend1 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    bomList[currentWireCount - 1].Spacing = bomList[currentWireCount - 1].Spacing - (listBendConstraintLimitation[0].BendLength - bend1);
                                                    bend1 = listBendConstraintLimitation[0].BendLength;
                                                    bomType = "E";
                                                }
                                            }
                                            listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint  /* Filter Normal/Reverse bend */
                                                                            where bc.BendType == AngleDir && bc.Sequence == 2
                                                                            select bc).ToList();

                                            if (bend2 == 0)
                                            {
                                                bend2 = bomList[currentWireCount + 1].Spacing;
                                                bomList[currentWireCount + 1].IsSegmentUsed = true;

                                                if (bend2 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    int requiredSpacing = listBendConstraintLimitation[0].BendLength - bend2;
                                                    if (bomList.Count > (currentWireCount + 2))
                                                    {
                                                        bomList[currentWireCount + 2].Spacing -= requiredSpacing;
                                                    }
                                                }
                                                bomList[currentWireCount].Spacing = bend1 + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                bomList[currentWireCount].BendFlag = true;
                                            }
                                            else if (bend2 < listBendConstraintLimitation[0].BendLength)
                                            {
                                                bomList[currentWireCount].Spacing = bend1 + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                bomList[currentWireCount].BendFlag = true;
                                            }
                                            else
                                            {
                                                bomList[currentWireCount].Spacing = bend1 + bend2 - CreepLengthValue;
                                                bomList[currentWireCount].BendFlag = true;
                                            }

                                            bomList[currentWireCount].IsSegmentUsed = true;

                                            if (bomList[currentWireCount].IsSegmentAdjusted == true)
                                            {
                                                currentWireCount++;
                                            }

                                            if (tempBOM != 0)
                                            {
                                                currentWireCount++;
                                            }
                                            currentWireCount++;
                                        }
                                        else
                                        {
                                            /* Bend 1 limit constraint */
                                            List<BendConstraint> listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint  /* Filter Normal/Reverse bend */
                                                                                                 where bc.BendType == AngleDir && bc.Sequence == 1
                                                                                                 select bc).ToList();
                                            //FB
                                            if (calculateSecondBend)
                                            {
                                                bend2 = currentWire - currentSegment;

                                                if (bend2 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    tempBOM = bomList[currentWireCount + 1].Spacing;
                                                    bomList[currentWireCount + 1].Spacing -= (listBendConstraintLimitation[0].BendLength - bend2);
                                                    bomList[currentWireCount + 1].IsSegmentUsed = true;
                                                    bomType = "E";
                                                }
                                            }
                                            // SB
                                            if (calculateFirstBend)
                                            {
                                                /* Bend 2 limit constraint */
                                                listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                where bc.BendType == AngleDir && bc.Sequence == 2
                                                                                select bc).ToList();

                                                if (currentWire == currentSegment)
                                                {
                                                    bend1 = listBendConstraintLimitation[0].BendLength;
                                                    bomList[currentWireCount].AdjustableSpacing = bomList[currentWireCount].Spacing - bend1;
                                                    bomList[currentWireCount].IsSegmentAdjusted = true;
                                                }
                                                else
                                                {
                                                    bend1 = bomList[currentWireCount].Spacing - bend2;
                                                }

                                                if (bend1 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    bomList[currentWireCount - 1].Spacing = bomList[currentWireCount - 1].Spacing - (listBendConstraintLimitation[0].BendLength - bend1);
                                                    bend1 = listBendConstraintLimitation[0].BendLength;
                                                    bomType = "E";
                                                }
                                            }

                                            listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint  /* Filter Normal/Reverse bend */
                                                                            where bc.BendType == AngleDir && bc.Sequence == 1
                                                                            select bc).ToList();


                                            if (bend2 == 0)
                                            {
                                                bend2 = bomList[currentWireCount + 1].Spacing;
                                                bomList[currentWireCount + 1].IsSegmentUsed = true;

                                                if (bend2 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    int requiredSpacing = listBendConstraintLimitation[0].BendLength - bend2;
                                                    if (bomList.Count > (currentWireCount + 2))
                                                    {
                                                        bomList[currentWireCount + 2].Spacing -= requiredSpacing;
                                                    }
                                                    bomType = "E";
                                                }
                                                bomList[currentWireCount].Spacing = bend1 + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                bomList[currentWireCount].BendFlag = true;
                                            }
                                            else if (bend2 < listBendConstraintLimitation[0].BendLength)
                                            {
                                                bomList[currentWireCount].Spacing = bend1 + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                bomList[currentWireCount].BendFlag = true;
                                            }
                                            else
                                            {
                                                bomList[currentWireCount].Spacing = bend1 + bend2 - CreepLengthValue;
                                                bomList[currentWireCount].BendFlag = true;
                                            }

                                            bomList[currentWireCount].IsSegmentUsed = true;

                                            if (bomList[currentWireCount].IsSegmentAdjusted == true)
                                            {
                                                currentWireCount++;
                                            }
                                            currentWireCount++;
                                        }

                                        currentWire = bend2 + tempBOM;

                                        break;
                                    }
                                }
                            }
                            else  // Overhang > First Segment
                            {
                                if (bomList[0].Spacing > currentSegmentforOH)
                                {
                                    break;
                                }
                                else
                                {
                                    bomList[0].IsSegmentUsed = true;  // OH1 is used in the below calculation

                                    int ohCreepLength = 0;
                                    for (int creepcount = 0; creepcount < Paramcount - 1; creepcount++)
                                    {
                                        CreepLengthValue = Math.Abs(CalculateCreepLength(Convert.ToDouble(shapeParam[creepcount].Angle), pinSize, barDia, out errorMessage)); /* Creep Calculation */
                                        CreepLengthValue = Convert.ToInt32(Math.Round(CreepLengthValue / 10.0) * 10);
                                        ohCreepLength += CreepLengthValue;
                                    }
                                    CreepLengthValue = ohCreepLength;

                                    List<BendConstraint> listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                         where bc.BendType == AngleDir && bc.Sequence == 2
                                                                                         select bc).ToList();
                                    if ((Paramcount % 2) == 0)  // Identify FB or SB. If even, check SB else FB
                                    {
                                        bend2 = bomList[0].Spacing - (currentSegmentforOH - shapeParam[Paramcount - 1].ParameterValue);

                                        if (bend2 < listBendConstraintLimitation[0].BendLength)
                                        {
                                            int requiredSpacing = listBendConstraintLimitation[0].BendLength - bend2; // Constraint at SB is 70 and FB is 35

                                            tempBOM = bomList[currentWireCount + 1].Spacing;
                                            bomList[currentWireCount + 1].Spacing -= requiredSpacing;
                                            bomList[currentWireCount + 1].IsSegmentUsed = true;
                                            currentWireCount += 2;
                                            bomList[0].Spacing += requiredSpacing;
                                            bomType = "E";
                                        }
                                        else { currentWireCount += 1; }
                                        bomList[0].Spacing -= CreepLengthValue;
                                        currentWire = bend2 + tempBOM;
                                    }
                                    else
                                    {
                                        listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                        where bc.BendType == AngleDir && bc.Sequence == 1
                                                                        select bc).ToList();

                                        bend1 = bomList[0].Spacing - (currentSegmentforOH - shapeParam[Paramcount - 1].ParameterValue);

                                        if (bend1 < listBendConstraintLimitation[0].BendLength)
                                        {
                                            int requiredSpacing = listBendConstraintLimitation[0].BendLength - bend1;

                                            tempBOM = bomList[currentWireCount + 1].Spacing;
                                            bomList[currentWireCount + 1].Spacing -= requiredSpacing;
                                            bomList[currentWireCount + 1].IsSegmentUsed = true;

                                            currentWireCount += 2;
                                            bomList[0].Spacing += requiredSpacing;
                                            bomType = "E";
                                        }
                                        else { currentWireCount += 1; }
                                        bomList[0].Spacing -= CreepLengthValue;
                                        currentWire = bend1 + tempBOM;
                                    }

                                    overHangFlowFlag = true;
                                }
                            }
                            totalWireCount++;
                        }
                        //        /* Increment the count value to get the next parameter value */
                        CountCreep++;
                        Paramcount++;
                    }
                }

                if (errorMessage == "")
                {
                    /* To get the bend spacing constraint master table data */
                    List<BendSpacingConstraint> listBendSpacingConstraint = new List<BendSpacingConstraint>();
                    BendSpacingConstraint objBendSpacingConstraint = new BendSpacingConstraint();
                    int bendSpacingConstraint = 0;
                    if (structureElementType != "Column")
                    {
                        objBendSpacingConstraint.WireSpace = wireSpacing;
                        listBendSpacingConstraint = objBendSpacingConstraint.GetBendSpacingConstraint(barDia);  /* Get the bend spacing value */

                        if (listBendSpacingConstraint.Count > 0)
                        {
                            bendSpacingConstraint = listBendSpacingConstraint[0].WireShiftedSpace;
                        }
                        else
                        {
                            throw new Exception("Bend Spacing constraint is not defined."); /* It should come out of the application only if the bend fails. */
                        }
                    }

                    /* To get the machine constraint master table data */
                    List<MachineConstraint> listMachineConstriant = new List<MachineConstraint>();
                    MachineConstraint objMachineConstraint = new MachineConstraint();
                    listMachineConstriant = objMachineConstraint.GetMachineConstraint();    /*  Get the OH limit value  */
                    int minWireSpace = 0, maxWireSpace = 0;

                    if (listMachineConstriant.Count != 0)
                    {
                        foreach (BendingAdjustmentClass bs in WireDetails)
                        {
                            if (bs.WireType == "M")
                            {
                                minWireSpace = listMachineConstriant[0].MinMWSpace;
                                maxWireSpace = listMachineConstriant[0].MaxMWSpace;
                                break;
                            }
                            else if (bs.WireType == "C")
                            {
                                minWireSpace = listMachineConstriant[0].MinCWSpace;
                                maxWireSpace = listMachineConstriant[0].MaxCWSpace;
                                break;
                            }
                        }

                    }
                    else
                    {
                        throw new Exception("Min & Max Wire Spacing is not defined in MachineConstraintMaster.");
                    }

                    List<AdjustableBend> adjustableBendList = new List<AdjustableBend>();
                    int loopCount = 0;
                    foreach (BendingAdjusment item in bomList)
                    {
                        if (item.IsSegmentAdjusted == true)
                        {
                            adjustableBendList.Add(new AdjustableBend { Quantity = item.Quantity, Position = loopCount, Spacing = item.AdjustableSpacing });
                        }
                        loopCount++;
                    }

                    foreach (AdjustableBend item in adjustableBendList)
                    {
                        bomList.Insert(item.Position, new BendingAdjusment { Quantity = item.Quantity, Spacing = item.Spacing });
                    }


                    /* To remove wire if it has min spacing */
                    loopCount = 0;
                    foreach (BendingAdjusment item in bomList)
                    {
                        if (item.BendFlag == false)
                        {
                            if (loopCount != 0 && loopCount != bomList.Count - 1)
                            {
                                if (item.Spacing < minWireSpace && loopCount > 0)
                                {
                                    bomList[loopCount - 1].Spacing += bomList[loopCount].Spacing;
                                    bomList[loopCount].CanRemove = true;
                                }
                            }
                        }
                        loopCount++;
                    }
                    bomList.RemoveAll(item => item.CanRemove == true);


                    /* Get the output as no of pitch and wire pitch */
                    loopCount = 0;
                    foreach (BendingAdjusment bendItem in bomList)
                    {
                        if (bendItem.BendFlag == true && loopCount != 0)
                        {
                            bomList[loopCount].Spacing = GetRoundValue(bomList[loopCount].Spacing);
                        }

                        if (loopCount == 0 && bendItem.BendFlag != true)
                        {
                            if (bendItem.Spacing % 10 == 0)
                            {
                                if (bomList[bomList.Count - 1].Spacing % 10 != 0)
                                {
                                    bomList[bomList.Count - 1].Spacing = RoundUpTo10Setp(bomList[bomList.Count - 1].Spacing);
                                }
                            }
                            else if (bendItem.Spacing % 5 == 0)
                            {
                                if (bomList[bomList.Count - 1].Spacing % 5 != 0 || (bomList[bomList.Count - 1].Spacing % 10 == 0 && bendItem.Spacing % 5 == 0))
                                {
                                    bomList[bomList.Count - 1].Spacing = RoundUpBy5Setp(bomList[bomList.Count - 1].Spacing);
                                }
                            }

                        }
                        else
                        {
                            if (loopCount != bomList.Count - 1)
                            {
                                bomList[loopCount].Spacing = GetRoundValue(bomList[loopCount].Spacing);
                            }
                        }
                        loopCount++;
                    }
                    bomList.RemoveAll(item => item.CanRemove == true);

                    if (structureElementType != "Column")
                    {
                        loopCount = 0;
                        /* Validate & Adjust Bending spacing against machine constraint */
                        foreach (BendingAdjusment bendAdjust in bomList)
                        {
                            if (bendAdjust.BendFlag == true)
                            {
                                if (bendAdjust.Spacing > bendSpacingConstraint)
                                {
                                    errorMessage = "Bending spacing exceeded the limitation.";
                                    //LogManager.log.Info(errorMessage + "at " + bendAdjust.ParamName + "; " + bendAdjust.ParamValue);
                                }
                            }
                            loopCount = loopCount + 1;
                        }
                    }

                    /* Sum up the equal amount of wire spacing */
                    int itemCount = 1;
                    foreach (BendingAdjusment item in bomList)
                    {
                        if (itemCount < bomList.Count - 1)
                        {
                            if (itemCount != 1 && itemCount != bomList.Count)
                            {
                                if (item.Spacing == bomList[itemCount].Spacing)
                                {
                                    bomList[itemCount].Quantity = bomList[itemCount].Quantity + item.Quantity;
                                    bomList[itemCount - 1].CanRemove = true;
                                }
                            }
                            itemCount++;
                        }
                    }
                    bomList.RemoveAll(item => item.CanRemove == true);

                    foreach (BendingAdjusment item in bomList)
                    {
                        PitchValue = PitchValue + item.Spacing + "-" + item.Quantity + ",";
                    }
                    //LogManager.log.Info("Pitch value : " + PitchValue);
                    //LogManager.log.Info("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * ");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PitchValue;
        }

        /* Bending Adjustment for Column */
        public string BendAdjustmentforColumn(List<ShapeParameter> shapeParam, int oH1, int oH2, int wireSpacing, int pinSize, double barDia, int bendPosition, List<BendingAdjustmentClass> WireDetails, out string errorMessage, ref string bomType, string structureElementType)
        {
            string PitchValue = "";
            List<BendingAdjusment> listBendingAdjustment = new List<BendingAdjusment>();
            errorMessage = "";
            try
            {
                int ParamCountLast = shapeParam.Count;

                List<BendConstraint> listBendConstraint = new List<BendConstraint>();
                BendConstraint objBendConstraint = new BendConstraint();
                listBendConstraint = objBendConstraint.GetBendConstraint(barDia);    /* Get the bend limit value */
                List<BendingAdjusment> bomList = new List<BendingAdjusment>();
                foreach (BendingAdjustmentClass bs in WireDetails)
                {
                    int Paramcount = 1
                        , CountCreep = 0;
                    string WireSpacing = bs.WireSpacing;
                    string WireType = bs.WireType;
                    string[] ArrayBomvalue = WireSpacing.Split(',');
                    int countBomValue = 1
                        , countMaxBomValue = ArrayBomvalue.Length;

                    bool overHangFlowFlag = false;

                    foreach (string paramvalue in ArrayBomvalue)
                    {
                        //LogManager.log.Info("Detailing BOM : " + paramvalue);
                        if (paramvalue.Contains("x"))
                        {
                            string[] ArrayWireDetails = paramvalue.Split('x');
                            if (Convert.ToInt32(ArrayWireDetails[0]) > 1)
                            {
                                for (int quantity = 1; quantity <= Convert.ToInt32(ArrayWireDetails[0]); quantity++)
                                {
                                    bomList.Add(new BendingAdjusment { Quantity = 1, Spacing = Convert.ToInt32(ArrayWireDetails[1]) });
                                }
                            }
                            else
                            {
                                bomList.Add(new BendingAdjusment { Quantity = Convert.ToInt32(ArrayWireDetails[0]), Spacing = Convert.ToInt32(ArrayWireDetails[1]) });
                            }
                        }
                        countBomValue++;
                    }

                    int currentSegment = 0, currentSegmentforOH = 0, currentSegmentOverFlowValue = 0;
                    int currentWire = 0, currentWireCount = 0, totalWireCount = 0;
                    bool oh2CalcDone = false, IsOtherthanOverhangSegmentVisited = false;
                    foreach (ShapeParameter segment in shapeParam)   /*Iterate the param details to fetch the param value for bending logic*/
                    {
                        if (segment.IsParamUsed == true)
                        {
                            Paramcount++;
                            continue;
                        }
                        string AngleDir = "";
                        if (segment.AngleDir == 1)
                        {
                            AngleDir = "N";
                        }
                        else
                        {
                            AngleDir = "R";
                        }
                        int CreepLengthValue = 0;
                        int BendType = 0;
                        if (Paramcount % 2 == 0)
                        {
                            BendType = 2;
                        }
                        else
                        {
                            BendType = 1;
                        }
                        CreepLengthValue = Math.Abs(CalculateCreepLength(Convert.ToDouble(shapeParam[CountCreep].Angle), pinSize, barDia, out errorMessage)); /* Creep Calculation */
                        CreepLengthValue = Convert.ToInt32(Math.Round(CreepLengthValue / 10.0) * 10);
                        int tempBOM = 0, bend2 = 0, bend1 = 0;
                        if (currentSegmentOverFlowValue != 0)
                        {
                            currentSegment = segment.ParameterValue + currentSegmentOverFlowValue;
                        }
                        else
                        {
                            currentSegment = segment.ParameterValue;
                        }
                        currentSegmentforOH += segment.ParameterValue;
                        bool calculateFirstBend = true, calculateSecondBend = true, isSegmentValidated = false;

                        foreach (BendingAdjusment bomitem in bomList)
                        {
                            /* OH2 Calculation */
                            if (bomList[bomList.Count - 1].Spacing > shapeParam[shapeParam.Count - 1].ParameterValue && oh2CalcDone == false)
                            {
                                int segmentSpace = 0, i = 0, bendOH = 0;
                                for (i = shapeParam.Count; i > 0; i--)
                                {
                                    segmentSpace += shapeParam[i - 1].ParameterValue;
                                    if (bomList[bomList.Count - 1].Spacing > segmentSpace)
                                    {
                                        shapeParam[i - 1].IsParamUsed = true;
                                        continue;
                                    }
                                    else
                                    {
                                        int oh1CreepLength = 0;
                                        for (int creepcount = shapeParam.Count; creepcount >= i; creepcount--)
                                        {
                                            CreepLengthValue = Math.Abs(CalculateCreepLength(Convert.ToDouble(shapeParam[creepcount - 1].Angle), pinSize, barDia, out errorMessage)); /* Creep Calculation */
                                            CreepLengthValue = Convert.ToInt32(Math.Round(CreepLengthValue / 10.0) * 10);
                                            oh1CreepLength += CreepLengthValue;
                                        }
                                        CreepLengthValue = oh1CreepLength;

                                        if ((i) % 2 == 0)
                                        {
                                            List<BendConstraint> listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                                 where bc.BendType == AngleDir && bc.Sequence == 2
                                                                                                 select bc).ToList();
                                            int bomValue = segmentSpace - shapeParam[i - 1].ParameterValue;

                                            bendOH = bomList[bomList.Count - 1].Spacing - bomValue;

                                            //check here
                                            if (listBendConstraintLimitation.Count>0 && bendOH < listBendConstraintLimitation[0].BendLength)
                                            {
                                                int requiredSpacing = listBendConstraintLimitation[0].BendLength - bendOH;
                                                bomList[bomList.Count - 2].Spacing -= requiredSpacing;

                                                bomList[bomList.Count - 1].Spacing += (requiredSpacing - CreepLengthValue);
                                                bomList[bomList.Count - 1].IsSegmentUsed = true;
                                                bomType = "E";
                                            }
                                            else
                                            {
                                                bomList[bomList.Count - 1].Spacing += -CreepLengthValue;
                                                bomList[bomList.Count - 1].IsSegmentUsed = true;
                                                bomType = "E";
                                            }
                                        }
                                        else
                                        {
                                            List<BendConstraint> listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                                 where bc.BendType == AngleDir && bc.Sequence == 1
                                                                                                 select bc).ToList();
                                            int bomValue = segmentSpace;

                                            bendOH = bomValue - bomList[bomList.Count - 1].Spacing;

                                            if (listBendConstraintLimitation.Count>0 && bendOH < listBendConstraintLimitation[0].BendLength)
                                            {
                                                int requiredSpacing = listBendConstraintLimitation[0].BendLength - bendOH;
                                                bomList[bomList.Count - 1].Spacing -= (requiredSpacing + CreepLengthValue);
                                                bomList[bomList.Count - 1].IsSegmentUsed = true;

                                                bomList[bomList.Count - 2].Spacing += requiredSpacing;
                                                bomType = "E";
                                            }
                                            else
                                            {
                                                bomList[bomList.Count - 1].Spacing += -CreepLengthValue;
                                                bomList[bomList.Count - 1].IsSegmentUsed = true;
                                                bomType = "E";
                                            }
                                        }
                                        break;
                                    }
                                }
                                oh2CalcDone = true;
                            }
                            else
                            {
                                if (Paramcount == shapeParam.Count)
                                {
                                    if ((shapeParam.Count % 2) == 0)
                                    {
                                        calculateFirstBend = false;
                                    }
                                    else
                                    {
                                        calculateSecondBend = false;
                                    }
                                }
                            }
                            /* OH2 Calculation Ends */

                            /* Other than OH segment */
                            if (bomList[0].Spacing < shapeParam[0].ParameterValue || overHangFlowFlag == true) // Overhang < First Segment 
                            {
                                int countOverFlowSegment = Paramcount;
                                int bomCount = 0;
                                if (bomitem.IsSegmentUsed == false)
                                {
                                    currentWire += bomitem.Spacing;


                                    if (currentSegment < currentWire && isSegmentValidated == false)
                                    {
                                        //if (bomitem.IsSegmentHit == true)
                                        //{
                                        //    currentSegment = currentWire - bomitem.Spacing;
                                        //}
                                        //else
                                        //{
                                        currentSegment -= currentWire - bomitem.Spacing;
                                        //}

                                        int segmentSpace = 0, i = 0;
                                        segmentSpace = currentSegment;
                                        for (i = Paramcount; i < shapeParam.Count - 1; i++)
                                        {
                                            segmentSpace += shapeParam[i].ParameterValue;

                                            if (bomList[currentWireCount].Spacing > segmentSpace)
                                            {
                                                shapeParam[i].IsParamUsed = true;
                                                countOverFlowSegment++;
                                                continue;
                                            }
                                            else
                                            {
                                                currentWire = bomitem.Spacing;
                                                break;
                                            }
                                        }
                                        currentSegment = segmentSpace - shapeParam[i].ParameterValue;
                                    }

                                    if (currentWire <= currentSegment)
                                    {
                                        bomList[currentWireCount].IsSegmentUsed = true;
                                        currentWireCount += 1;
                                        continue;
                                    }
                                    else
                                    {
                                        currentSegmentOverFlowValue = 0; tempBOM = 0; IsOtherthanOverhangSegmentVisited = true;
                                        int currentSegmentCreepLengthValue = 0;
                                        for (int i = Paramcount; i <= countOverFlowSegment; i++)
                                        {
                                            CreepLengthValue = Math.Abs(CalculateCreepLength(Convert.ToDouble(shapeParam[i].Angle), pinSize, barDia, out errorMessage)); /* Creep Calculation */
                                            CreepLengthValue = Convert.ToInt32(Math.Round(CreepLengthValue / 10.0) * 10);
                                            currentSegmentCreepLengthValue += CreepLengthValue;
                                        }
                                        CreepLengthValue = currentSegmentCreepLengthValue;

                                        if (Paramcount < countOverFlowSegment)
                                        {
                                            if (countOverFlowSegment % 2 == 0)
                                            {
                                                BendType = 2;
                                            }
                                            else
                                            {
                                                BendType = 1;
                                            }
                                        }
                                        if (BendType == 1)
                                        {
                                            int bend1Value = currentSegment;
                                            /* Bend 2 limit constraint */
                                            List<BendConstraint> listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint  /* Filter Normal/Reverse bend */
                                                                                                 where bc.BendType == AngleDir && bc.Sequence == 2
                                                                                                 select bc).ToList();

                                            if (calculateSecondBend)
                                            {
                                                bend2 = currentWire - currentSegment;

                                                if ( listBendConstraintLimitation.Count > 0 && bend2 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    tempBOM = bomList[currentWireCount + 1].Spacing;
                                                    bomList[currentWireCount + 1].Spacing -= (listBendConstraintLimitation[0].BendLength - bend2);
                                                    bomList[currentWireCount + 1].IsSegmentUsed = true;
                                                    bomType = "E";
                                                }
                                            }


                                            if (calculateFirstBend)
                                            {
                                                if (Paramcount % 2 == 0)
                                                {
                                                    /* Bend 1 limit constraint */
                                                    listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                    where bc.BendType == AngleDir && bc.Sequence == 2
                                                                                    select bc).ToList();
                                                }
                                                else
                                                {
                                                    listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                    where bc.BendType == AngleDir && bc.Sequence == 1
                                                                                    select bc).ToList();
                                                }

                                                if (currentWire == currentSegment )
                                                {
                                                    bend1 = listBendConstraintLimitation[0].BendLength;
                                                    bomList[currentWireCount].AdjustableSpacing = bomList[currentWireCount].Spacing - bend1;
                                                    bomList[currentWireCount].IsSegmentAdjusted = true;
                                                }
                                                else
                                                {
                                                    if (Paramcount < countOverFlowSegment)
                                                    {
                                                        for (int i = countOverFlowSegment - 1; i >= Paramcount; i--)
                                                        {
                                                            bend1Value = currentSegment - shapeParam[i].ParameterValue;
                                                        }
                                                        bend1 = bend1Value;
                                                    }
                                                    else
                                                    {
                                                        bend1 = bomList[currentWireCount].Spacing - bend2;
                                                    }
                                                }

                                                if (listBendConstraintLimitation.Count > 0 && bend1 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    bomList[currentWireCount - 1].Spacing = bomList[currentWireCount - 1].Spacing - (listBendConstraintLimitation[0].BendLength - bend1);
                                                    bend1 = listBendConstraintLimitation[0].BendLength;
                                                    bomType = "E";
                                                }
                                            }
                                            listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint  /* Filter Normal/Reverse bend */
                                                                            where bc.BendType == AngleDir && bc.Sequence == 2
                                                                            select bc).ToList();

                                            if (bend2 == 0)
                                            {
                                                bend2 = bomList[currentWireCount + 1].Spacing;
                                                bomList[currentWireCount + 1].IsSegmentUsed = true;

                                                if (listBendConstraintLimitation.Count > 0 && bend2 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    int requiredSpacing = listBendConstraintLimitation[0].BendLength - bend2;
                                                    if (bomList.Count > (currentWireCount + 2))
                                                    {
                                                        bomList[currentWireCount + 2].Spacing -= requiredSpacing;
                                                    }
                                                }
                                                if (Paramcount < countOverFlowSegment)
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + (currentSegment - bend1Value) + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                }
                                                else
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                }
                                                bomList[currentWireCount].BendFlag = true;
                                            }
                                            else if (listBendConstraintLimitation.Count > 0 && bend2 < listBendConstraintLimitation[0].BendLength)
                                            {
                                                if (Paramcount < countOverFlowSegment)
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + (currentSegment - bend1Value) + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                }
                                                else
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                }
                                                bomList[currentWireCount].BendFlag = true;
                                            }
                                            else
                                            {
                                                if (Paramcount < countOverFlowSegment)
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + (currentSegment - bend1Value) + bend2 - CreepLengthValue;
                                                }
                                                else
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + bend2 - CreepLengthValue;
                                                }
                                                bomList[currentWireCount].BendFlag = true;
                                            }

                                            bomList[currentWireCount].IsSegmentUsed = true;

                                            if (bomList[currentWireCount].IsSegmentAdjusted == true)
                                            {
                                                currentWireCount++;
                                            }

                                            if (tempBOM != 0)
                                            {
                                                currentWireCount++;
                                            }
                                            currentWireCount++;
                                        }
                                        else
                                        {
                                            int bend1Value = currentSegment; bool bend2ZeroFlag = false;
                                            /* Bend 1 limit constraint */
                                            List<BendConstraint> listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint  /* Filter Normal/Reverse bend */
                                                                                                 where bc.BendType == AngleDir && bc.Sequence == 1
                                                                                                 select bc).ToList();
                                            //FB
                                            if (calculateSecondBend)
                                            {
                                                bend2 = currentWire - currentSegment;
                                                if (bend2 == 0)
                                                {
                                                    bend2ZeroFlag = true;
                                                }
                                                else if (listBendConstraintLimitation.Count > 0 && bend2 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    tempBOM = bomList[currentWireCount + 1].Spacing;
                                                    bomList[currentWireCount + 1].Spacing -= (listBendConstraintLimitation[0].BendLength - bend2);
                                                    bomList[currentWireCount + 1].IsSegmentUsed = true;
                                                }
                                                bomType = "E";
                                            }
                                            // SB
                                            if (calculateFirstBend)
                                            {
                                                if (Paramcount % 2 == 0)
                                                {
                                                    /* Bend 2 limit constraint */
                                                    listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                    where bc.BendType == AngleDir && bc.Sequence == 2
                                                                                    select bc).ToList();
                                                }
                                                else
                                                {
                                                    listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                    where bc.BendType == AngleDir && bc.Sequence == 1
                                                                                    select bc).ToList();
                                                }



                                                if (currentWire == currentSegment )
                                                {
                                                    if (bend2ZeroFlag == true)
                                                    {
                                                        bend1 = listBendConstraintLimitation[0].BendLength;
                                                        bomList[currentWireCount].Spacing = bomList[currentWireCount].Spacing - bend1;
                                                        bomList[currentWireCount + 1].Spacing += bend1;
                                                        bomList[currentWireCount].IsSegmentAdjusted = true;
                                                    }
                                                    else
                                                    {
                                                        bend1 = listBendConstraintLimitation[0].BendLength;
                                                        bomList[currentWireCount].AdjustableSpacing = bomList[currentWireCount].Spacing - bend1;
                                                        bomList[currentWireCount].IsSegmentAdjusted = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Paramcount < countOverFlowSegment)
                                                    {
                                                        for (int i = countOverFlowSegment - 1; i >= Paramcount; i--)
                                                        {
                                                            bend1Value -= shapeParam[i].ParameterValue;
                                                        }
                                                        bend1 = bend1Value;
                                                    }
                                                    else
                                                    {
                                                        bend1 = bomList[currentWireCount].Spacing - bend2;
                                                    }
                                                }


                                                if (listBendConstraintLimitation.Count > 0 && bend1 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    bomList[currentWireCount - 1].Spacing = bomList[currentWireCount - 1].Spacing - (listBendConstraintLimitation[0].BendLength - bend1);
                                                    bend1 = listBendConstraintLimitation[0].BendLength;
                                                    bomType = "E";
                                                }
                                                if (bend2ZeroFlag == true)
                                                {
                                                    tempBOM = bend1;
                                                }
                                            }

                                            listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint  /* Filter Normal/Reverse bend */
                                                                            where bc.BendType == AngleDir && bc.Sequence == 1
                                                                            select bc).ToList();

                                            int currentBOMValue = bomList[currentWireCount].Spacing;
                                            if (bend2 == 0)
                                            {
                                                bend2 = bomList[currentWireCount + 1].Spacing;
                                                //bomList[currentWireCount + 1].IsSegmentUsed = true;

                                                if (listBendConstraintLimitation.Count > 0 && bend2 < listBendConstraintLimitation[0].BendLength)
                                                {
                                                    int requiredSpacing = listBendConstraintLimitation[0].BendLength - bend2;
                                                    if (bomList.Count > (currentWireCount + 2))
                                                    {
                                                        bomList[currentWireCount + 2].Spacing -= requiredSpacing;
                                                    }
                                                    bomType = "E";
                                                }
                                                if (Paramcount < countOverFlowSegment)
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + (currentSegment - bend1Value) + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                }
                                                else if (bend2ZeroFlag == true)
                                                {
                                                    // No need to calculate bend value
                                                }
                                                else
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                }
                                                if (bend2ZeroFlag == true)
                                                {
                                                    bomList[currentWireCount].BendFlag = false;
                                                }
                                                else
                                                {
                                                    bomList[currentWireCount].BendFlag = true;
                                                }
                                            }
                                            else if (listBendConstraintLimitation.Count > 0 && bend2 < listBendConstraintLimitation[0].BendLength)
                                            {
                                                if (Paramcount < countOverFlowSegment)
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + (currentSegment - bend1Value) + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                }
                                                else
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + listBendConstraintLimitation[0].BendLength - CreepLengthValue;
                                                }
                                                bomList[currentWireCount].BendFlag = true;
                                            }
                                            else
                                            {
                                                if (Paramcount < countOverFlowSegment)
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + (currentSegment - bend1Value) + bend2 - CreepLengthValue;
                                                }
                                                else
                                                {
                                                    bomList[currentWireCount].Spacing = bend1 + bend2 - CreepLengthValue;
                                                }
                                                bomList[currentWireCount].BendFlag = true;
                                            }

                                            bomList[currentWireCount].IsSegmentUsed = true;

                                            if (bomList[currentWireCount].IsSegmentAdjusted == true)
                                            {
                                                currentWireCount++;
                                            }
                                            currentWireCount++;
                                            if (bend2ZeroFlag == true)
                                            {
                                                bend2 = 0;
                                            }
                                        }

                                        currentWire = bend2 + tempBOM;

                                        break;
                                    }
                                }
                                bomCount++;
                            }
                            /* Other than OH segment Ends */

                            /* OH1 calculation */
                            else  // Overhang > First Segment
                            {
                                if (bomList[0].Spacing > currentSegmentforOH)
                                {
                                    break;
                                }
                                else
                                {
                                    bomList[0].IsSegmentUsed = true;  // OH1 is used in the below calculation

                                    int ohCreepLength = 0;
                                    for (int creepcount = 0; creepcount < Paramcount - 1; creepcount++)
                                    {
                                        CreepLengthValue = Math.Abs(CalculateCreepLength(Convert.ToDouble(shapeParam[creepcount].Angle), pinSize, barDia, out errorMessage)); /* Creep Calculation */
                                        CreepLengthValue = Convert.ToInt32(Math.Round(CreepLengthValue / 10.0) * 10);
                                        ohCreepLength += CreepLengthValue;
                                    }
                                    CreepLengthValue = ohCreepLength;

                                    List<BendConstraint> listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                                         where bc.BendType == AngleDir && bc.Sequence == 2
                                                                                         select bc).ToList();
                                    if ((Paramcount % 2) == 0)  // Identify FB or SB. If even, check SB else FB
                                    {
                                        bend2 = bomList[0].Spacing - (currentSegmentforOH - shapeParam[Paramcount - 1].ParameterValue);

                                        if (listBendConstraintLimitation.Count>0 && bend2 < listBendConstraintLimitation[0].BendLength)
                                        {
                                            int requiredSpacing = listBendConstraintLimitation[0].BendLength - bend2; // Constraint at SB is 70 and FB is 35

                                            tempBOM = bomList[currentWireCount + 1].Spacing;
                                            bomList[currentWireCount + 1].Spacing -= requiredSpacing;
                                            bomList[currentWireCount + 1].IsSegmentUsed = true;
                                            currentWireCount += 2;
                                            bomList[0].Spacing += requiredSpacing;
                                            bomType = "E";
                                        }
                                        else { currentWireCount += 1; }
                                        bomList[0].Spacing -= CreepLengthValue;
                                        currentWire = bend2 + tempBOM;
                                    }
                                    else
                                    {
                                        listBendConstraintLimitation = (from BendConstraint bc in listBendConstraint
                                                                        where bc.BendType == AngleDir && bc.Sequence == 1
                                                                        select bc).ToList();

                                        bend1 = bomList[0].Spacing - (currentSegmentforOH - shapeParam[Paramcount - 1].ParameterValue);

                                        if (listBendConstraintLimitation.Count > 0 && bend1 < listBendConstraintLimitation[0].BendLength)
                                        {
                                            int requiredSpacing = listBendConstraintLimitation[0].BendLength - bend1;

                                            tempBOM = bomList[currentWireCount + 1].Spacing;
                                            bomList[currentWireCount + 1].Spacing -= requiredSpacing;
                                            bomList[currentWireCount + 1].IsSegmentUsed = true;

                                            currentWireCount += 2;
                                            bomList[0].Spacing += requiredSpacing;
                                            bomType = "E";
                                        }
                                        else { currentWireCount += 1; }
                                        bomList[0].Spacing -= CreepLengthValue;
                                        currentWire = bend1 + tempBOM;
                                    }

                                    overHangFlowFlag = true;
                                }
                            }
                            /* OH1 calculation Ends */
                            totalWireCount++;
                        }
                        //        /* Increment the count value to get the next parameter value */
                        CountCreep++;
                        Paramcount++;
                    }

                    if (IsOtherthanOverhangSegmentVisited == false)
                    {
                        if (bomList.Count == 3)
                        {
                            /* Modified by anuran CHG0031334  */
                            if ((structureElementType == "Column") || (structureElementType == "Beam"))
                            {
                                CreepLength = Math.Abs(CalculateCreepLength(Convert.ToDouble("90"), pinSize, barDia, out errorMessage)); /* Creep Calculation */
                                CreepLength = Convert.ToInt32(Math.Round(CreepLength / 10.0) * 10);
                                bomList[1].Spacing -= CreepLength;
                            }
                        }
                    }
                }

                if (errorMessage == "")
                {
                    /* To get the bend spacing constraint master table data */
                    List<BendSpacingConstraint> listBendSpacingConstraint = new List<BendSpacingConstraint>();
                    BendSpacingConstraint objBendSpacingConstraint = new BendSpacingConstraint();
                    int bendSpacingConstraint = 0;

                    /* Modified by anuran  CHG0031334 */
                    if ((structureElementType != "Column") && (structureElementType != "Beam"))
                    {
                        objBendSpacingConstraint.WireSpace = wireSpacing;
                        listBendSpacingConstraint = objBendSpacingConstraint.GetBendSpacingConstraint(barDia);  /* Get the bend spacing value */

                        if (listBendSpacingConstraint.Count > 0)
                        {
                            bendSpacingConstraint = listBendSpacingConstraint[0].WireShiftedSpace;
                        }
                        else
                        {
                            throw new Exception("Bend Spacing constraint is not defined."); /* It should come out of the application only if the bend fails. */
                        }
                    }

                    /* To get the machine constraint master table data */
                    List<MachineConstraint> listMachineConstriant = new List<MachineConstraint>();
                    MachineConstraint objMachineConstraint = new MachineConstraint();
                    listMachineConstriant = objMachineConstraint.GetMachineConstraint();    /*  Get the OH limit value  */
                    int minWireSpace = 0, maxWireSpace = 0;

                    if (listMachineConstriant.Count != 0)
                    {
                        foreach (BendingAdjustmentClass bs in WireDetails)
                        {
                            if (bs.WireType == "M")
                            {
                                minWireSpace = listMachineConstriant[0].MinMWSpace;
                                maxWireSpace = listMachineConstriant[0].MaxMWSpace;
                                break;
                            }
                            else if (bs.WireType == "C")
                            {
                                minWireSpace = listMachineConstriant[0].MinCWSpace;
                                maxWireSpace = listMachineConstriant[0].MaxCWSpace;
                                break;
                            }
                        }

                    }
                    else
                    {
                        throw new Exception("Min & Max Wire Spacing is not defined in MachineConstraintMaster.");
                    }

                    List<AdjustableBend> adjustableBendList = new List<AdjustableBend>();
                    int loopCount = 0;
                    foreach (BendingAdjusment item in bomList)
                    {
                        if (item.IsSegmentAdjusted == true)
                        {
                            adjustableBendList.Add(new AdjustableBend { Quantity = item.Quantity, Position = loopCount, Spacing = item.AdjustableSpacing });
                        }
                        loopCount++;
                    }

                    foreach (AdjustableBend item in adjustableBendList)
                    {
                        bomList.Insert(item.Position, new BendingAdjusment { Quantity = item.Quantity, Spacing = item.Spacing });
                    }


                    /* To remove wire if it has min spacing */
                    loopCount = 0;
                    foreach (BendingAdjusment item in bomList)
                    {
                        if (item.BendFlag == false)
                        {
                            if (loopCount != 0 && loopCount != bomList.Count - 1)
                            {
                                if (item.Spacing < minWireSpace && loopCount > 0)
                                {
                                    bomList[loopCount - 1].Spacing += bomList[loopCount].Spacing;
                                    bomList[loopCount].CanRemove = true;
                                }
                            }
                        }
                        loopCount++;
                    }
                    bomList.RemoveAll(item => item.CanRemove == true);


                    /* Get the output as no of pitch and wire pitch */
                    loopCount = 0;
                    foreach (BendingAdjusment bendItem in bomList)
                    {
                        if (bendItem.BendFlag == true && loopCount != 0)
                        {
                            bomList[loopCount].Spacing = GetRoundValue(bomList[loopCount].Spacing);
                        }

                        if (loopCount == 0 && bendItem.BendFlag != true)
                        {
                            if (bendItem.Spacing % 10 == 0)
                            {
                                if (bomList[bomList.Count - 1].Spacing % 10 != 0)
                                {
                                    bomList[bomList.Count - 1].Spacing = RoundUpTo10Setp(bomList[bomList.Count - 1].Spacing);
                                }
                            }
                            else if (bendItem.Spacing % 5 == 0)
                            {
                                if (bomList[bomList.Count - 1].Spacing % 5 != 0 || (bomList[bomList.Count - 1].Spacing % 10 == 0 && bendItem.Spacing % 5 == 0))
                                {
                                    bomList[bomList.Count - 1].Spacing = RoundUpBy5Setp(bomList[bomList.Count - 1].Spacing);
                                }
                            }

                        }
                        else
                        {
                            if (loopCount != bomList.Count - 1)
                            {
                                bomList[loopCount].Spacing = GetRoundValue(bomList[loopCount].Spacing);
                            }
                        }
                        loopCount++;
                    }
                    bomList.RemoveAll(item => item.CanRemove == true);

                    /* modified by Anuran CHG0031334*/
                    if ((structureElementType != "Column") && (structureElementType != "Beam"))
                    {
                        loopCount = 0;
                        /* Validate & Adjust Bending spacing against machine constraint */
                        foreach (BendingAdjusment bendAdjust in bomList)
                        {
                            //int SpacingConstraint = 0;

                            if (bendAdjust.BendFlag == true)
                            {
                                if (bendAdjust.Spacing > bendSpacingConstraint)
                                {
                                    errorMessage = "Bending spacing exceeded the limitation.";
                                    //LogManager.log.Info(errorMessage + "at " + bendAdjust.ParamName + "; " + bendAdjust.ParamValue);
                                }
                            }
                            loopCount = loopCount + 1;
                        }
                    }

                    /* Sum up the equal amount of wire spacing */
                    int itemCount = 1;
                    foreach (BendingAdjusment item in bomList)
                    {
                        if (itemCount < bomList.Count - 1)
                        {
                            if (itemCount != 1 && itemCount != bomList.Count)
                            {
                                if (item.Spacing == bomList[itemCount].Spacing)
                                {
                                    bomList[itemCount].Quantity = bomList[itemCount].Quantity + item.Quantity;
                                    bomList[itemCount - 1].CanRemove = true;
                                }
                            }
                            itemCount++;
                        }
                    }
                    bomList.RemoveAll(item => item.CanRemove == true);

                    foreach (BendingAdjusment item in bomList)
                    {
                        PitchValue = PitchValue + item.Spacing + "-" + item.Quantity + ",";
                    }
                    //LogManager.log.Info("Pitch value : " + PitchValue);
                    //LogManager.log.Info("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * ");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PitchValue;
        }

        /* Calculate Angle */
        private List<listAngle> GetAngle(string bvbsstring, List<ShapeParameter> segmentList, out string errorMessage)
        {
            List<listAngle> listAngle = new List<listAngle>();
            errorMessage = "";
            try
            {
                //string bvbsstring = productMark.shapecode.CWbvbsString; //"BF2D@GlA@w90@lB@w90@lC@w0@";                

                if (bvbsstring.Contains("lA@"))
                {
                    int space1 = bvbsstring.IndexOf("lA@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 3);


                    int minus = space2 - (space1 + 3);
                    string firstPart = bvbsstring.Substring((space1 + 3), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "A", Angle = firstPart });

                    //listAngle.Add(firstPart);
                    //sbString.AppendLine(firstPart).Replace('w', ' ');
                }
                else if (bvbsstring.Contains("GlA@"))
                {
                    int space1 = bvbsstring.IndexOf("GlA@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "A", Angle = firstPart });

                    //listAngle.Add(firstPart);
                    //sbString.AppendLine(firstPart).Replace('w', ' ');
                }
                if (bvbsstring.Contains("@lB@"))
                {
                    int space1 = bvbsstring.IndexOf("@lB@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "B", Angle = firstPart });
                }
                else if (bvbsstring.Contains("GlB@"))
                {
                    int space1 = bvbsstring.IndexOf("GlB@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "B", Angle = firstPart });

                    //listAngle.Add(firstPart);
                    //sbString.AppendLine(firstPart).Replace('w', ' ');
                }

                if (bvbsstring.Contains("@lC@"))
                {
                    int space1 = bvbsstring.IndexOf("@lC@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "C", Angle = firstPart });
                }
                else if (bvbsstring.Contains("GlC@"))
                {
                    int space1 = bvbsstring.IndexOf("GlC@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "C", Angle = firstPart });

                    //listAngle.Add(firstPart);
                    //sbString.AppendLine(firstPart).Replace('w', ' ');
                }

                if (bvbsstring.Contains("@lD@"))
                {
                    int space1 = bvbsstring.IndexOf("@lD@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "D", Angle = firstPart });
                }
                else if (bvbsstring.Contains("GlD@"))
                {
                    int space1 = bvbsstring.IndexOf("GlD@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "D", Angle = firstPart });

                    //listAngle.Add(firstPart);
                    //sbString.AppendLine(firstPart).Replace('w', ' ');
                }

                if (bvbsstring.Contains("@lE@"))
                {
                    int space1 = bvbsstring.IndexOf("@lE@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "E", Angle = firstPart });
                }
                else if (bvbsstring.Contains("GlE@"))
                {
                    int space1 = bvbsstring.IndexOf("GlE@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "E", Angle = firstPart });

                    //listAngle.Add(firstPart);
                    //sbString.AppendLine(firstPart).Replace('w', ' ');
                }

                if (bvbsstring.Contains("@lF@"))
                {
                    int space1 = bvbsstring.IndexOf("@lF@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "F", Angle = firstPart });
                }
                else if (bvbsstring.Contains("GlF@"))
                {
                    int space1 = bvbsstring.IndexOf("GlF@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "F", Angle = firstPart });

                    //listAngle.Add(firstPart);
                    //sbString.AppendLine(firstPart).Replace('w', ' ');
                }

                if (bvbsstring.Contains("@lG@"))
                {
                    int space1 = bvbsstring.IndexOf("@lG@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "G", Angle = firstPart });
                }
                else if (bvbsstring.Contains("GlG@"))
                {
                    int space1 = bvbsstring.IndexOf("GlG@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "G", Angle = firstPart });

                    //listAngle.Add(firstPart);
                    //sbString.AppendLine(firstPart).Replace('w', ' ');
                }


                if (bvbsstring.Contains("@lH@"))
                {
                    int space1 = bvbsstring.IndexOf("@lH@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "H", Angle = firstPart });
                }
                else if (bvbsstring.Contains("GlH@"))
                {
                    int space1 = bvbsstring.IndexOf("GlH@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 4);


                    int minus = space2 - (space1 + 4);
                    string firstPart = bvbsstring.Substring((space1 + 4), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "H", Angle = firstPart });

                    //listAngle.Add(firstPart);
                    //sbString.AppendLine(firstPart).Replace('w', ' ');
                }

                if (bvbsstring.Contains("GlLeg@"))
                {
                    int space1 = bvbsstring.IndexOf("GlLeg@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 6);


                    int minus = space2 - (space1 + 6);
                    string firstPart = bvbsstring.Substring((space1 + 6), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "Leg", Angle = firstPart });
                }
                else if (bvbsstring.Contains("@lLeg@"))
                {
                    int space1 = bvbsstring.IndexOf("@lLeg@");
                    int space2 = bvbsstring.IndexOf("@", space1 + 6);


                    int minus = space2 - (space1 + 6);
                    string firstPart = bvbsstring.Substring((space1 + 6), minus).Replace('w', ' ');

                    listAngle.Add(new listAngle { paramName = "Leg", Angle = firstPart });
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

        /* Calculate Creep length */
        private int CalculateCreepLength(double angle, int pinSize, double barDia, out string errorMessage)
        {
            int creeplength = 0;
            double calculatedCreepLength = 0;
            errorMessage = "";
            try
            {
                double Radiant;
                double Part1;
                double Part2;
                Radiant = Convert.ToDouble((angle / 180)) * 3.14;
                Part1 = (2 * (Convert.ToDouble((pinSize / 2)) + barDia) * Math.Sin(Radiant / 2)) / Math.Cos(Radiant / 2);
                Part2 = Radiant * (Convert.ToDouble((pinSize / 2)) + (barDia / 2));
                calculatedCreepLength = Part1 - Part2;
                creeplength = Convert.ToInt32(Math.Round(calculatedCreepLength, 0));
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return creeplength;
        }

        /* Round up to 10 step. */
        private int RoundUpTo10Step(int input)
        {
            if (input % 10 == 0)
                return input;
            else
                return input + (10 - (input % 10));
        }

        /* Rounding the values */
        public static int GetRoundValue(int Intvalue)
        {
            string IntCount = Intvalue.ToString();
            string IntCountlastvalue = IntCount.Substring(IntCount.Length - 1, 1);
            if (@"01234".Any(IntCountlastvalue.Contains))
            {
                return Convert.ToInt32(IntCount.Substring(0, (IntCount.Length - 1)) + "0");
            }
            else
            {
                return Convert.ToInt32(IntCount.Substring(0, (IntCount.Length - 1)) + "0") + 10;
            }
        }

        /* Rounding up to 5 */
        private static int RoundUpBy5Setp(int input)
        {
            return input + (5 - (input % 5));
        }

        /* Rounding up to 10 */
        private static int RoundUpTo10Setp(int input)
        {
            return input + (10 - (input % 10));
        }

        #endregion
    }

    #region "Class..."

    public class AdjustableBend
    {
        public int Quantity { get; set; }
        public int Spacing { get; set; }
        public int Position { get; set; }
    }

    #endregion


}
