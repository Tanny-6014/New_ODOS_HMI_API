using System.Text;

namespace DetailingService.Repositories
{
    public class CarpetSplitingHelper
    {
        #region "Get Mesh Products"

        public List<ValidMeshProduct> SplitMeshProducts(double totalMWLength, double totalCWLength, int mwSpacing, int cwSpacing, double lap, int excessLap, out string output, out string errorMessage)
        {
            List<ValidMeshProduct> resultValidProductList = new List<ValidMeshProduct> { };
            ValidMeshProduct objValidMeshProduct = new ValidMeshProduct();
            StringBuilder builderStatus = new StringBuilder();
            errorMessage = "";
            double maxCrossLength = 0, minCrossLength = 0, meshSheetQty = 0, crossLength, preferredCrossLength, CrossExcessLap = 0, remainingCrossLength = 0;
            bool isSuccess = true;

            try
            {
                //To Write Lapping,ExcessLapping & Spacing
                builderStatus.Append("Spacing : " + mwSpacing);
                builderStatus.AppendLine();

                builderStatus.Append("Lap : " + lap);
                builderStatus.AppendLine();

                builderStatus.Append("ExcessLap : " + excessLap);
                builderStatus.AppendLine();

                // Get the max cross length
                if ((totalMWLength >= 801 && totalMWLength <= 1000) || (totalMWLength >= 1001 && totalMWLength <= 2400))
                {
                    maxCrossLength = 4000;
                    builderStatus.Append("Max Cross Length : " + maxCrossLength);
                    builderStatus.AppendLine();
                }
                else if (totalMWLength >= 501 && totalMWLength <= 800)
                {
                    maxCrossLength = 3000;
                    builderStatus.Append("Max Cross Length : " + maxCrossLength);
                    builderStatus.AppendLine();
                }
                else
                {
                    maxCrossLength = 2400;
                    builderStatus.Append("Max Cross Length : " + maxCrossLength);
                    builderStatus.AppendLine();
                }

                // Get the min cross length
                if (mwSpacing == 100 || mwSpacing == 200)
                {
                    minCrossLength = 1000;
                    builderStatus.Append("Min Cross Length : " + minCrossLength);
                    builderStatus.AppendLine();
                }

                else if (mwSpacing == 150)
                {
                    minCrossLength = 900;
                    builderStatus.Append("Min Cross Length : " + minCrossLength);
                    builderStatus.AppendLine();
                }
                else
                {
                    minCrossLength = 1080;
                    builderStatus.Append("Min Cross Length : " + minCrossLength);
                    builderStatus.AppendLine();
                }

                // To calculate MESH Quantity
                meshSheetQty = Math.Ceiling((totalCWLength - lap) / (maxCrossLength - lap));

                builderStatus.Append("Mesh Sheet Qty : " + meshSheetQty);
                builderStatus.AppendLine();

                // To calculate Cross Length
                crossLength = Math.Ceiling((totalCWLength + (meshSheetQty - 1) * lap) / meshSheetQty);
                builderStatus.Append("Cross Length : " + crossLength);
                builderStatus.AppendLine();

                // To validate Preferred Cross Length
                if ((crossLength % mwSpacing) == 0)
                {
                    preferredCrossLength = crossLength;
                    builderStatus.Append("Preferred Cross Length : " + preferredCrossLength);
                    builderStatus.AppendLine();
                }
                else
                {
                    preferredCrossLength = (crossLength - (crossLength % mwSpacing)) + mwSpacing;
                    builderStatus.Append("Preferred Cross Length : " + preferredCrossLength);
                    builderStatus.AppendLine();
                }

                // To calculate Excess Lap
                CrossExcessLap = meshSheetQty * preferredCrossLength - (meshSheetQty - 1) * lap - totalCWLength;
                builderStatus.Append("Cross Excess Lap : " + CrossExcessLap);
                builderStatus.AppendLine();

                // To verify Excess Lap
                if (CrossExcessLap <= excessLap)
                {
                    builderStatus.AppendLine();
                    builderStatus.Append("**** Cross Excess Lap(" + CrossExcessLap + ") is less than or equals to Excess Lap(" + excessLap + ") ****");
                    builderStatus.AppendLine();
                    isSuccess = true;

                    /* If Cross Excess Lap is less than Excess Lap - "Pass".*/
                    resultValidProductList.Add(new ValidMeshProduct { Quantity = Convert.ToInt32(meshSheetQty), MWLength = Convert.ToInt32(totalMWLength), CWLength = Convert.ToInt32(preferredCrossLength) });
                }
                else
                {
                    builderStatus.AppendLine();
                    builderStatus.Append("**** Cross Excess Lap(" + CrossExcessLap + ") is greater than Excess Lap(" + excessLap + ") ****");
                    builderStatus.AppendLine();
                    isSuccess = false;
                }

                while (!isSuccess)  /* Loop till sucess */
                {
                    /* Calulate remaining cross length */
                    remainingCrossLength = totalCWLength + (meshSheetQty - 1) * lap - maxCrossLength * (meshSheetQty - 1);

                    builderStatus.Append("Remaining Cross Length : " + remainingCrossLength);
                    builderStatus.AppendLine();

                    /* Validate remaining cross length is in step of main wire spacing */
                    if (remainingCrossLength % mwSpacing == 0)
                    {
                        preferredCrossLength = remainingCrossLength;
                        builderStatus.Append("Preferred Cross Length : " + preferredCrossLength);
                        builderStatus.AppendLine();
                    }
                    else
                    {
                        builderStatus.Append("--------------------------------------------------------------------------------------");
                        builderStatus.AppendLine();
                        builderStatus.Append("**** Remaining Cross Length(" + remainingCrossLength + ") is not in the step of Main Wire Spacing(" + mwSpacing + ") ****");
                        builderStatus.AppendLine();
                        builderStatus.AppendLine();

                        /* Round cross length to nearest preferred length */
                        preferredCrossLength = (remainingCrossLength - (remainingCrossLength % mwSpacing)) + mwSpacing;

                        builderStatus.Append("Preferred Cross Length : " + preferredCrossLength);
                        builderStatus.AppendLine();
                    }

                    /* Verify cross excess lap */
                    if (preferredCrossLength >= minCrossLength)
                    {
                        builderStatus.Append("--------------------------------------------------------------------------------------");
                        builderStatus.AppendLine();
                        builderStatus.Append("**** Preferred Cross Wire Length(" + preferredCrossLength + ") is greater than Min Cross Wire Length(" + minCrossLength + ") ****");
                        builderStatus.AppendLine();
                        builderStatus.AppendLine();

                        /* Calculate cross excess lap */
                        CrossExcessLap = ((meshSheetQty - 1) * maxCrossLength + preferredCrossLength) - (meshSheetQty - 1) * lap - totalCWLength;

                        builderStatus.Append("Cross Excess Lap : " + CrossExcessLap);
                        builderStatus.AppendLine();

                        if (CrossExcessLap < 0)
                        {
                            resultValidProductList = null;
                            errorMessage = "Unable to generate mesh size. Kindly check Excess Lapping.";
                            isSuccess = true;
                            break;
                        }
                        if (CrossExcessLap <= excessLap)
                        {
                            /* If Cross Excess Lap is less than Excess Lap - "Pass".*/
                            isSuccess = true;
                            resultValidProductList.Add(new ValidMeshProduct { Quantity = Convert.ToInt32(meshSheetQty) - 1, MWLength = Convert.ToInt32(totalMWLength), CWLength = Convert.ToInt32(maxCrossLength) });
                            resultValidProductList.Add(new ValidMeshProduct { Quantity = 1, MWLength = Convert.ToInt32(totalMWLength), CWLength = Convert.ToInt32(preferredCrossLength) });
                            break;
                        }
                        else
                        {
                            isSuccess = false;

                            /* Calculate next preferred max length */
                            maxCrossLength = maxCrossLength - mwSpacing;
                            /* To validate max cross length is less than min cross length */
                            if (maxCrossLength < minCrossLength)
                            {
                                resultValidProductList = null;
                                errorMessage = "Unable to generate mesh size. Kindly check Excess Lapping.";
                                isSuccess = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        isSuccess = false;
                        /* Calculate next preferred max length */
                        maxCrossLength = maxCrossLength - mwSpacing;

                        /* To validate max cross length is less than min cross length */
                        if (maxCrossLength < minCrossLength)
                        {
                            resultValidProductList = null;
                            errorMessage = "Unable to generate mesh size. Kindly check Excess Lapping.";
                            isSuccess = true;
                            break;
                        }
                    }
                }
                output = builderStatus.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultValidProductList;
        }

        #endregion
    }

}
