using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using DrainService.Context;
using DrainService.Constants;
using ExpressionParser;
using static Dapper.SqlMapper;
using DrainService.Dtos;

namespace DrainService.Repositories
{
    public class SlabDetailingComponent
    {
       //private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=3600";
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        public SlabDetailingComponent()
        {

        }
        public SlabDetailingComponent(IConfiguration configuration)
        {
           // _dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);
        }

        Dictionary<string, string> dict_input_parameters = new Dictionary<string, string>();
        Dictionary<string, string> dict_result = new Dictionary<string, string>();
        List<string> tactonfunctions = new List<string> { "min", "max", "valuenum", "valuename", "ceiling", "round", "concat", "if" };

        #region "Product Marking Formula"

        #region "Method Call"
        public Dictionary<string, string> ExecuteDetailingComponent(int shapeId, string strElement)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Dictionary<string, string> validationresult = new Dictionary<string, string>();
            if (shapeId != 0 && (strElement == "" || strElement == null))
            {
                result = GetProductMarkingFormulae(shapeId);
                if (result.Count > 0 && result != null)
                {
                    validationresult = GetValidationConstraints(shapeId);
                }

                if (validationresult.Count > 0 && validationresult != null)
                {
                    validationresult.Add("ValidationFailed", "True");
                    return validationresult;
                }
                else
                {
                    result.Add("ValidationFailed", "False");
                    return result;
                }
            }
            else
            {
                result = GetStructuteMarkingFormulae(strElement);
                if (result.Count > 0 && result != null)
                {
                    validationresult = GetValidationConstraints(shapeId);
                }

                if (validationresult.Count > 0 && validationresult != null)
                {
                    validationresult.Add("ValidationFailed", "True");
                    return validationresult;
                }
                else
                {
                    result.Add("ValidationFailed", "False");
                    return result;
                }
            }
        }
        #endregion


        #region "Product Marking"

        public Dictionary<string, string> GetProductMarkingFormulae(int shapeId)
        {


            int Count = 0;
            List<SlabDetailingComponent> listStructureMarking = new List<SlabDetailingComponent>();
            DataSet dsProductMark = new DataSet();
            IEnumerable<SlabValidationFormulaDto> Formula;

            Dictionary<string, string> result_set = new Dictionary<string, string>();
            var comparer = StringComparer.OrdinalIgnoreCase;
            Dictionary<string, string> dict_product_marking_formulae = new Dictionary<string, string>(comparer);
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    string no_of_cw = "0";
                    //FillData();
                    dynamicParameters.Add("@ShapeId", shapeId);
                    if (dict_input_parameters.ContainsKey("no_cw"))
                    {
                        no_of_cw = dict_input_parameters["no_cw"];
                    }

                    // dsProductMark = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "GetProductMarkingFormulaForShape");
                    Formula = sqlConnection.Query<SlabValidationFormulaDto>(SystemConstants.GetProductMarkingFormulaForShape, dynamicParameters, commandType: CommandType.StoredProcedure);
                    if (Formula.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(Formula);
                        dsProductMark.Tables.Add(dt);
                    }



                    if (dsProductMark != null && dsProductMark.Tables.Count != 0)
                    {
                        foreach (DataRowView dr in dsProductMark.Tables[0].DefaultView)
                        {
                            if (Convert.ToString(dr["NoOfCW"]) == no_of_cw || Convert.ToString(dr["NoOfCW"]) == "")
                            {
                                dict_product_marking_formulae.Add(Convert.ToString((dr["FormulaName"])), Convert.ToString((dr["Formula"])));
                            }
                        }
                    }
                    foreach (KeyValuePair<string, string> ele in dict_product_marking_formulae)
                    {
                        
                        bool ifpresent = false;
                        bool concatpresent = false;
                        bool ifResultAlreadyCalculated = false;
                        ifResultAlreadyCalculated = result_set.ContainsKey(ele.Key);
                        if (!ifResultAlreadyCalculated)
                        {
                            ifpresent = ele.Value.Contains("if");
                            concatpresent = ele.Value.Contains("concat");
                            if (ifpresent)
                            {
                                result_set = EvaluateIfCondition(ele.Key, ele.Value, dict_product_marking_formulae);
                            }
                            else if (concatpresent)
                            {
                                result_set = EvaluateConcatCondition(ele.Key, ele.Value, dict_product_marking_formulae);
                            }
                            else
                            {
                                result_set = CalculateValueFromFormula(ele.Key, ele.Value, dict_product_marking_formulae);
                            }
                        }
                        Count++;
                    }
                }
            }
            catch (Exception ex)
            {
                Count++;
                throw ex;



            }

            return result_set;
        }
        public Dictionary<string, string> CalculateValueFromFormula(string formulaName, string formula, Dictionary<string, string> dict_product_marking_formulae)
        {
            try
            {

                //MathParser parser = new MathParser();//commented by vanita
                MathParserWrapper parser = new ExpressionParser.MathParserWrapper();//Added by vanita
                bool isKeyPresent = false;
                string output = string.Empty;

                foreach (KeyValuePair<string, string> ele in dict_product_marking_formulae)
                {
                    //isKeyPresent = formula.Contains(ele.Key.ToLower());
                    isKeyPresent = Regex.IsMatch(formula, @"\b" + ele.Key + @"\b");
                    var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.Key.ToLower());
                    //bool IsPresentInInputs = formula.IndexOf(ele.Key, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (isKeyPresent == true && IsPresentInInputs == false)
                    {
                        CalculateVaribaleValues(ele.Key, ele.Value, dict_product_marking_formulae);
                    }
                }

                foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                {
                    isKeyPresent = formula.Contains(ele.Key);
                    if (isKeyPresent)
                    {
                        output = formula.SafeReplace(ele.Key, ele.Value, true);
                        formula = output;
                    }
                }

                MathParserWrapper objParse = new MathParserWrapper();
                double result = objParse.EvaluateFormula(formula, false);
                string[] splitArray;
                splitArray = formulaName.Split(null);
                string csvString = string.Join("_", splitArray).ToLower();
                if (!dict_input_parameters.ContainsKey(csvString) && !dict_result.ContainsKey(csvString))
                {
                    dict_input_parameters.Add(csvString, result.ToString());
                    dict_result.Add(csvString, result.ToString());
                }
                return dict_result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ClearDictionary()
        {
            dict_input_parameters.Clear();
            dict_result.Clear();
        }


        public double GetMWLength(int shapeId, Dictionary<string, string> dict_ip)
        {

            DataSet ds = new DataSet();
            DataSet dsProductMark = new DataSet();
            Dictionary<string, string> result_set = new Dictionary<string, string>();
            Dictionary<string, string> dict_formula = new Dictionary<string, string>();
            IEnumerable<BeamFormulaDto> Formula;
            IEnumerable<BeamProductMarkingFormulaForShapeDto> beamproductMarkingFormulaForShapeDto;

            string mw_length_formula = string.Empty;
            try
            {
                dict_input_parameters = dict_ip;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ShapeId", shapeId);
                    //ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "GetMWLengthFormula");
                    Formula = sqlConnection.Query<BeamFormulaDto>(SystemConstants.GetMWLengthFormula, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (Formula.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(Formula);
                        ds.Tables.Add(dt);
                    }

                    if (ds != null && ds.Tables.Count != 0)
                    {
                        foreach (DataRowView dr in ds.Tables[0].DefaultView)
                        {
                            mw_length_formula = Convert.ToString((dr["Formula"]));
                        }
                    }
                    //dsProductMark = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "GetProductMarkingFormulaForShape");
                    beamproductMarkingFormulaForShapeDto = sqlConnection.Query<BeamProductMarkingFormulaForShapeDto>(SystemConstants.GetProductMarkingFormulaForShape, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (beamproductMarkingFormulaForShapeDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(beamproductMarkingFormulaForShapeDto);
                        dsProductMark.Tables.Add(dt);
                    }

                    if (dsProductMark != null && dsProductMark.Tables.Count != 0)
                    {
                        foreach (DataRowView dr in dsProductMark.Tables[0].DefaultView)
                        {
                            dict_formula.Add(Convert.ToString((dr["FormulaName"])), Convert.ToString((dr["Formula"])));
                        }
                    }
                    result_set = CalculateValueFromFormula("mw_length", mw_length_formula, dict_formula);
                    double result = Double.Parse(result_set["mw_length"]);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void dummyFunction(string abbb)
        {
            string specialSlabShapes = "1M1;1MR1;1M2;2M2;2MR12;2M1;1C1;1CR1";
            List<string> specialSlabShapesList = new List<string>(specialSlabShapes.Split(';'));
            bool a1 = specialSlabShapesList.Contains(abbb.ToUpper().Trim());

            int maxLinks = 20;
            int minLinks = 5;
            int NoOfstirrups = 0;
            int no_of_beamcage_1, no_of_beamcage_2, no_of_links_1, no_of_links_2;

            int div;
            if (NoOfstirrups <= maxLinks)
            {
                no_of_beamcage_1 = 1;
                no_of_beamcage_2 = 0;
                no_of_links_1 = NoOfstirrups;
                no_of_links_2 = 0;
            }
            else if (NoOfstirrups > maxLinks)
            {
                int factor;
                //if (NoOfstirrups % 2 == 0)
                //{
                //    factor = 2;
                //}
                //else
                //{
                //    factor = 3;
                //}
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

        public void CalculateVaribaleValues(string formulaName, string formula, Dictionary<string, string> dict_product_marking_formulae)
        {
            try
            {
                Dictionary<string, string> result_set = new Dictionary<string, string>();
                string output = string.Empty;
                bool ifpresent = false;
                bool concatpresent = false;
                ifpresent = formula.Contains("if");
                concatpresent = formula.Contains("concat");
                if (ifpresent)
                {
                    result_set = EvaluateIfCondition(formulaName, formula, dict_product_marking_formulae);
                }
                else if (concatpresent)
                {
                    result_set = EvaluateConcatCondition(formulaName, formula, dict_product_marking_formulae);
                }
                else
                {
                    result_set = CalculateValueFromFormula(formulaName, formula, dict_product_marking_formulae);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<string> ParseFormulaeFromIf(string formula)
        {
            MathParserWrapper objParse = new MathParserWrapper();
            Regex rex = new Regex("if", RegexOptions.IgnoreCase);
            string str = rex.Replace(formula, "", 1);
            string str1 = formula.Replace("if", "");
            str = str.TrimStart('(');
            str = str.Remove(str.Length - 1);

            StringBuilder formattedString = new StringBuilder();
            int balanceOfParenth = 0;
            Stack<StringBuilder> stack = new Stack<StringBuilder>();
            List<string> list = new List<string>();
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if (ch == '(')
                {
                    balanceOfParenth++;
                    formattedString.Append(ch);
                }
                else if (ch == ')')
                {
                    balanceOfParenth--;
                    formattedString.Append(ch);
                }
                else
                {
                    formattedString.Append(ch);
                }
                if (balanceOfParenth == 0)
                {
                    if (ch == ',' || i == str.Length - 1)
                    {
                        list.Add(formattedString.ToString());
                        formattedString = new StringBuilder();
                    }
                }
            }
            return list;
        }

        public double EvaluateNestedIfCondition(string formulaName, string formula, Dictionary<string, string> dict_product_marking_formulae)
        {
            MathParserWrapper objParse = new MathParserWrapper();
            bool isEqualToPresent = false;
            bool isLessThanPresent = false;
            bool isGreaterThanPresent = false;
            string EqualToSign = "=";
            string LessThanSign = "<";
            string GreaterThanSign = ">";

            List<string> list = new List<string>();
            list = ParseFormulaeFromIf(formula);
            string[] splittedEqualToList = null;
            bool isKeyPresent;
            string output = string.Empty;
            string formula1 = string.Empty;
            double ConditionResult = 0;
            List<double> resultList = new List<double>();
            double OutputValue = 0;
            //foreach (string singleformula in list)
            //{
            string singleformula = list[0];
            isEqualToPresent = singleformula.Contains(EqualToSign);
            isLessThanPresent = singleformula.Contains(LessThanSign);
            isGreaterThanPresent = singleformula.Contains(GreaterThanSign);
            if (isEqualToPresent)
            {
                splittedEqualToList = singleformula.Split('=');
            }
            else if (isLessThanPresent)
            {
                splittedEqualToList = singleformula.Split('<');
            }
            else if (isGreaterThanPresent)
            {
                splittedEqualToList = singleformula.Split('>');
            }

            if (splittedEqualToList.Length > 0 && splittedEqualToList != null)
            {
                splittedEqualToList = singleformula.Split('=');
                foreach (var splitedFormula in splittedEqualToList)
                {
                    formula1 = splitedFormula.TrimEnd(',');

                    foreach (KeyValuePair<string, string> ele in dict_product_marking_formulae)
                    {
                        isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.Key + @"\b");
                        var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.Key.ToLower());
                        if (isKeyPresent == true && IsPresentInInputs == false)
                        {
                            CalculateVaribaleValues(ele.Key, ele.Value, dict_product_marking_formulae);
                        }
                    }

                    foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                    {
                        isKeyPresent = formula1.Contains(ele.Key);
                        if (isKeyPresent)
                        {
                            output = formula1.SafeReplace(ele.Key, ele.Value, true);
                            formula1 = output;

                        }
                    }
                    ConditionResult = objParse.EvaluateFormula(formula1, false);
                    resultList.Add(ConditionResult);
                }
                bool allAreSame = false;
                if (isEqualToPresent)
                {
                    allAreSame = resultList.All(x => x == resultList.First());
                }
                else if (isLessThanPresent)
                {
                    if (Convert.ToDouble(resultList[0]) < Convert.ToDouble(resultList[1]))
                    {
                        allAreSame = true;
                    }
                }
                else if (isGreaterThanPresent)
                {
                    if (Convert.ToDouble(resultList[0]) > Convert.ToDouble(resultList[1]))
                    {
                        allAreSame = true;
                    }
                }
                if (allAreSame == true)
                {
                    formula1 = list[1].TrimEnd(',');
                    int countofif = Regex.Matches(formula1, "if").Count;
                    if (countofif > 0)
                    {
                        OutputValue = EvaluateNestedIfCondition(formulaName, formula1, dict_product_marking_formulae);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                    else
                    {

                        foreach (KeyValuePair<string, string> ele in dict_product_marking_formulae)
                        {
                            isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.Key + @"\b");
                            var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.Key.ToLower());
                            if (isKeyPresent == true && IsPresentInInputs == false)
                            {
                                CalculateVaribaleValues(ele.Key, ele.Value, dict_product_marking_formulae);
                            }
                        }

                        foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                        {
                            isKeyPresent = formula1.Contains(ele.Key);
                            if (isKeyPresent)
                            {
                                output = formula1.SafeReplace(ele.Key, ele.Value, true);
                                formula1 = output;

                            }
                        }
                        OutputValue = objParse.EvaluateFormula(formula1, false);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }

                }
                else
                {
                    formula1 = list[2].TrimEnd(',');
                    int countofif = Regex.Matches(formula1, "if").Count;
                    if (countofif > 0)
                    {
                        OutputValue = EvaluateNestedIfCondition(formulaName, formula1, dict_product_marking_formulae);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> ele in dict_product_marking_formulae)
                        {
                            isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.Key + @"\b");
                            var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.Key.ToLower());
                            if (isKeyPresent == true && IsPresentInInputs == false)
                            {
                                CalculateVaribaleValues(ele.Key, ele.Value, dict_product_marking_formulae);
                            }
                        }


                        foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                        {
                            isKeyPresent = formula1.Contains(ele.Key);
                            if (isKeyPresent)
                            {
                                output = formula1.SafeReplace(ele.Key, ele.Value, true);
                                formula1 = output;

                            }
                        }
                        OutputValue = objParse.EvaluateFormula(formula1, false);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                }
            }
            //}

            return OutputValue;
        }

        public Dictionary<string, string> EvaluateIfCondition(string formulaName, string formula, Dictionary<string, string> dict_product_marking_formulae)
        {
            MathParserWrapper objParse = new MathParserWrapper();
            bool isEqualToPresent = false;
            bool isLessThanPresent = false;
            bool isGreaterThanPresent = false;
            string EqualToSign = "=";
            string LessThanSign = "<";
            string GreaterThanSign = ">";
            Dictionary<string, StringBuilder> ifDict = new Dictionary<string, StringBuilder>();
            List<string> list = new List<string>();
            list = ParseFormulaeFromIf(formula);
            string[] splittedEqualToList = null;
            bool isKeyPresent;
            string output = string.Empty;
            string formula1 = string.Empty;
            double ConditionResult = 0;
            List<double> resultList = new List<double>();
            double OutputValue = 0;
            //foreach (string singleformula in list)
            //{
            string singleformula = list[0];
            isEqualToPresent = singleformula.Contains(EqualToSign);
            isLessThanPresent = singleformula.Contains(LessThanSign);
            isGreaterThanPresent = singleformula.Contains(GreaterThanSign);
            if (isEqualToPresent)
            {
                splittedEqualToList = singleformula.Split('=');
            }
            else if (isLessThanPresent)
            {
                splittedEqualToList = singleformula.Split('<');
            }
            else if (isGreaterThanPresent)
            {
                splittedEqualToList = singleformula.Split('>');
            }


            if (splittedEqualToList.Length > 0)
            {
                foreach (var splitedFormula in splittedEqualToList)
                {
                    formula1 = splitedFormula.TrimEnd(',');

                    foreach (KeyValuePair<string, string> ele in dict_product_marking_formulae)
                    {
                        isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.Key + @"\b");
                        var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.Key.ToLower());
                        if (isKeyPresent == true && IsPresentInInputs == false)
                        {
                            CalculateVaribaleValues(ele.Key, ele.Value, dict_product_marking_formulae);
                        }
                    }


                    foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                    {
                        isKeyPresent = formula1.Contains(ele.Key);
                        if (isKeyPresent)
                        {
                            output = formula1.SafeReplace(ele.Key, ele.Value, true);
                            formula1 = output;

                        }
                    }
                    ConditionResult = objParse.EvaluateFormula(formula1, false);
                    resultList.Add(ConditionResult);
                }
                bool allAreSame = false;
                if (isEqualToPresent)
                {
                    allAreSame = resultList.All(x => x == resultList.First());
                }
                else if (isLessThanPresent)
                {
                    if (Convert.ToDouble(resultList[0]) < Convert.ToDouble(resultList[1]))
                    {
                        allAreSame = true;
                    }
                }
                else if (isGreaterThanPresent)
                {
                    if (Convert.ToDouble(resultList[0]) > Convert.ToDouble(resultList[1]))
                    {
                        allAreSame = true;
                    }
                }
                if (allAreSame == true)
                {
                    formula1 = list[1].TrimEnd(',');
                    int countofif = Regex.Matches(formula1, "if").Count;
                    if (countofif > 0)
                    {
                        OutputValue = EvaluateNestedIfCondition(formulaName, formula1, dict_product_marking_formulae);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                    else
                    {

                        foreach (KeyValuePair<string, string> ele in dict_product_marking_formulae)
                        {
                            isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.Key + @"\b");
                            var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.Key.ToLower());
                            if (isKeyPresent == true && IsPresentInInputs == false)
                            {
                                CalculateVaribaleValues(ele.Key, ele.Value, dict_product_marking_formulae);
                            }
                        }

                        foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                        {
                            isKeyPresent = formula1.Contains(ele.Key);
                            if (isKeyPresent)
                            {
                                output = formula1.SafeReplace(ele.Key, ele.Value, true);
                                formula1 = output;

                            }
                        }
                        OutputValue = objParse.EvaluateFormula(formula1, false);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }

                }
                else
                {
                    formula1 = list[2].TrimEnd(',');
                    int countofif = Regex.Matches(formula1, "if").Count;
                    if (countofif > 0)
                    {
                        OutputValue = EvaluateNestedIfCondition(formulaName, formula1, dict_product_marking_formulae);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> ele in dict_product_marking_formulae)
                        {
                            isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.Key + @"\b");
                            var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.Key.ToLower());
                            if (isKeyPresent == true && IsPresentInInputs == false)
                            {
                                CalculateVaribaleValues(ele.Key, ele.Value, dict_product_marking_formulae);
                            }
                        }


                        foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                        {
                            isKeyPresent = formula1.Contains(ele.Key);
                            if (isKeyPresent)
                            {
                                output = formula1.SafeReplace(ele.Key, ele.Value, true);
                                formula1 = output;

                            }
                        }
                        OutputValue = objParse.EvaluateFormula(formula1, false);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                }
            }
            // }

            return dict_result;
        }

        public void AddInputParametersToDictionary(string formulaName, string OutputValue)
        {

            string[] splitArray;
            splitArray = formulaName.Split(null);
            string csvString = string.Join("_", splitArray).ToLower();
            if (dict_input_parameters.ContainsKey(csvString) == false)
            {
                dict_input_parameters.Add(csvString, OutputValue);
            }
        }

        public void AddCalculatedFormulaeToDictionary(string formulaName, string OutputValue)
        {
            string[] splitArray;
            splitArray = formulaName.Split(null);
            string csvString = string.Join("_", splitArray).ToLower();
            if (dict_result.ContainsKey(csvString) == false)
            {
                dict_result.Add(csvString, OutputValue);
            }
        }
        public Dictionary<string, string> EvaluateConcatCondition(string formulaName, string formula, Dictionary<string, string> dict_product_marking_formulae)
        {


            MathParserWrapper objParse = new MathParserWrapper();
            string str = formula.SafeReplace("concat", "", true);
            str = str.Trim();
            str = str.TrimStart('(');
            str = str.Remove(str.Length - 1);
            string[] splittedString;
            string output;
            bool isKeyPresent;
            string result;
            double OutputValue;



            foreach (KeyValuePair<string, string> ele in dict_product_marking_formulae)
            {
                isKeyPresent = formula.Contains(ele.Key.ToLower());
                bool contains = formula.IndexOf(ele.Key, StringComparison.OrdinalIgnoreCase) >= 0;
                var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.Key.ToLower());
                if (isKeyPresent == true && IsPresentInInputs == false)
                {
                    CalculateVaribaleValues(ele.Key, ele.Value, dict_product_marking_formulae);
                }
            }



            foreach (KeyValuePair<string, string> ele in dict_input_parameters)
            {
                isKeyPresent = str.Contains(ele.Key);
                if (isKeyPresent)
                {
                    output = str.SafeReplace(ele.Key, ele.Value, true);
                    str = output;
                }
            }
            bool isTactonFunction = false;
            splittedString = str.Split(',');
            string stringConcated = string.Empty;
            for (int i = 0; i < splittedString.Length; i++)
            {
                bool isPlusPresent1 = splittedString[i].Contains("+");
                bool isminusPresent1 = splittedString[i].Contains("-");
                bool IsDigitsOnlyFlag = IsDigitsOnly(splittedString[i]);
                if ((isPlusPresent1 == true || isminusPresent1 == true) && IsDigitsOnlyFlag == true)
                {
                    OutputValue = objParse.EvaluateFormula(splittedString[i], false);
                    splittedString[Array.IndexOf(splittedString, splittedString[i])] = OutputValue.ToString();
                    stringConcated += OutputValue;
                }
                else
                {
                    isTactonFunction = tactonfunctions.Any(s => splittedString[i].Contains(s));
                    if (isTactonFunction)
                    {
                        OutputValue = objParse.EvaluateFormula(splittedString[i], false);
                        splittedString[Array.IndexOf(splittedString, splittedString[i])] = OutputValue.ToString();
                        stringConcated += OutputValue;
                    }
                    else
                    {
                        stringConcated += splittedString[i];
                    }
                }
            }
            //result = string.Concat(splittedString);
            //dict_result.Add(formulaName, result);
            AddCalculatedFormulaeToDictionary(formulaName, stringConcated);
            AddInputParametersToDictionary(formulaName, stringConcated);
            return dict_result;

        }
        public void FillInputDictionary(Dictionary<string, string> inputsDict)
        {
            if (inputsDict.Count > 0)
            {
                dict_input_parameters = inputsDict;
            }
            //dict_input_parameters["b"] = "140";
            //dict_input_parameters["c"] = "140";



        }


        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    if (c == '-' || c == '+')
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
            }

            return true;
        }

        #endregion

        #region "Validations"
        public Dictionary<string, string> GetValidationConstraints(int shapeId)
        {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();

            //int ShapeId = 10; // hardcoded value needs to be changed
            DataSet dsValidations = new DataSet();
            Dictionary<string, string> result_set = new Dictionary<string, string>();
            var comparer = StringComparer.OrdinalIgnoreCase;
            bool Returnresult = false;
            string ReturnError = string.Empty;
            Dictionary<string, string> dict_product_marking_formulae = new Dictionary<string, string>(comparer);



            List<ValidationsDto> listValidations = new List<ValidationsDto>();

            IEnumerable<ValidationsDto> listValidationsDto;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    //FillInputDictionary("","");
                    dynamicParameters.Add("@ShapeId", shapeId);
                    //dsValidations = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "GetInputOutputValidationForShape");
                    listValidationsDto = sqlConnection.Query<ValidationsDto>(SystemConstants.GetInputOutputValidationForShape, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (listValidationsDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(listValidationsDto);
                        dsValidations.Tables.Add(dt);
                    }

                    Validations validation = new Validations();
                    List<Validations> validationList = new List<Validations>();
                    if (dsValidations != null && dsValidations.Tables.Count != 0)
                    {
                        validationList = dsValidations.Tables[0].AsEnumerable()
                                           .Select(dataRow => new Validations
                                           {
                                               Attribute = dataRow.Field<string>("Attribute"),
                                               ValidationConstraint = dataRow.Field<string>("ValidationConstraint")
                                           }).ToList();
                    }
                    foreach (var item in validationList)
                    {
                        Returnresult = EvaluateValidations(item.Attribute, item.ValidationConstraint);
                        resultDict.Clear();
                        if (Returnresult == false)
                        {
                            resultDict.Add(item.Attribute, item.Attribute + ":" + item.ValidationConstraint + ":" + "Validation constraint failed");
                            //ReturnError = item.Attribute + ":" + "Validation constraint failed";
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultDict;



        }
        public bool EvaluateValidations(string Attribute, string ValidationConstraint)
        {
            try
            {
                bool andsign = ValidationConstraint.Contains("&&");
                bool isEqualToPresent = Regex.IsMatch(ValidationConstraint, @"\b=\b");
                bool isLessThanPresent = Regex.IsMatch(ValidationConstraint, @"\b<\b");
                bool isGreaterThanPresent = Regex.IsMatch(ValidationConstraint, @"\b>\b");
                bool isLessThanEqualToPresent = Regex.IsMatch(ValidationConstraint, @"\b<=\b");
                bool isGreaterThanEqualToPresent = Regex.IsMatch(ValidationConstraint, @"\b>=\b");

                bool isGreaterThanEqualToPresent1 = Regex.IsMatch(ValidationConstraint, @"(^|\s)>=(\s|$)");

                string[] splitAndArray = null;
                bool isKeyPresent = false;
                string output;
                bool Returnresult = false;
                List<bool> resultList = new List<bool>();
                List<bool> SingleresultList = new List<bool>();
                foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                {
                    isKeyPresent = ValidationConstraint.Contains(ele.Key);
                    if (isKeyPresent)
                    {
                        output = ValidationConstraint.SafeReplace(ele.Key, ele.Value, true);
                        ValidationConstraint = output;
                    }
                }
                if (andsign)
                {
                    splitAndArray = Regex.Split(ValidationConstraint, @"\&&+");
                    for (int i = 0; i < splitAndArray.Length; i++)
                    {
                        string singleformula = splitAndArray[i];
                        SingleresultList = ParseValidation(singleformula);
                        resultList.Add(SingleresultList[0]);
                    }
                }
                else if (isEqualToPresent == true && andsign == false)
                {
                    string formula1;
                    double ConditionResult;
                    string[] splittedEqualToList = ValidationConstraint.Split('=');
                    MathParserWrapper objParse = new MathParserWrapper();
                    List<double> resultLst = new List<double>();
                    foreach (var splitedFormula in splittedEqualToList)
                    {
                        formula1 = splitedFormula.TrimEnd(',');
                        foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                        {
                            isKeyPresent = formula1.Contains(ele.Key);
                            if (isKeyPresent)
                            {
                                output = formula1.SafeReplace(ele.Key, ele.Value, true);
                                formula1 = output;

                            }
                        }
                        ConditionResult = objParse.EvaluateFormula(formula1, false);
                        resultLst.Add(ConditionResult);
                    }
                    bool allAreSame = false;
                    allAreSame = resultLst.All(x => x == resultLst.First());
                    if (allAreSame)
                    {
                        resultList.Add(true);
                    }
                    else
                    {
                        resultList.Add(false);
                    }
                }
                else if (isLessThanPresent || isLessThanEqualToPresent || isGreaterThanPresent || isGreaterThanEqualToPresent)
                {
                    resultList = ParseValidation(ValidationConstraint);
                }
                bool returnValue = resultList.Contains(false);
                if (returnValue == true)
                {
                    Returnresult = false;
                }
                else
                {
                    Returnresult = true;
                }
                return Returnresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        List<bool> ParseValidation(string singleformula)
        {
            MathParserWrapper objParse = new MathParserWrapper();
            List<bool> resultListValidations = new List<bool>();
            bool andsign = Regex.IsMatch(singleformula, @"\b&&\b");
            bool isEqualToPresent = Regex.IsMatch(singleformula, @"\b=\b");
            bool isLessThanPresent = Regex.IsMatch(singleformula, @"\b<\b");
            bool isGreaterThanPresent = Regex.IsMatch(singleformula, @"\b>\b");
            bool isLessThanEqualPresent = Regex.IsMatch(singleformula, @"\b<=\b");
            bool isGreaterThanEqualPresent = Regex.IsMatch(singleformula, @"\b>=\b");
            string[] splitArray;
            if (isGreaterThanEqualPresent)
            {
                splitArray = Regex.Split(singleformula, @"\>=+");
                for (int i = 0; i < splitArray.Length; i++)
                {
                    bool isPlusPresent = Regex.IsMatch(splitArray[i], @"\b+\b");
                    bool isminusPresent = Regex.IsMatch(splitArray[i], @"\b-\b");
                    if (isPlusPresent == true || isminusPresent == true)
                    {
                        double arryresult = objParse.EvaluateFormula(splitArray[i], false);
                        splitArray[i] = arryresult.ToString();
                    }
                }
                if (Convert.ToDouble(splitArray[0]) >= Convert.ToDouble(splitArray[1]))
                {
                    resultListValidations.Add(true);
                }
                else
                {
                    resultListValidations.Add(false);
                }
            }
            else if (isLessThanEqualPresent)
            {
                splitArray = Regex.Split(singleformula, @"\<=+");
                for (int i = 0; i < splitArray.Length; i++)
                {
                    bool isPlusPresent = Regex.IsMatch(splitArray[i], @"\b+\b");
                    bool isminusPresent = Regex.IsMatch(splitArray[i], @"\b-\b");
                    if (isPlusPresent == true || isminusPresent == true)
                    {
                        double arryresult = objParse.EvaluateFormula(splitArray[i], false);
                        splitArray[i] = arryresult.ToString();
                    }
                }
                if (Convert.ToDouble(splitArray[0]) <= Convert.ToDouble(splitArray[1]))
                {
                    resultListValidations.Add(true);
                }
                else
                {
                    resultListValidations.Add(false);
                }
            }
            else if (isLessThanPresent)
            {
                splitArray = Regex.Split(singleformula, @"\<+");
                for (int i = 0; i < splitArray.Length; i++)
                {
                    bool isPlusPresent = Regex.IsMatch(splitArray[i], @"\b+\b");
                    bool isminusPresent = Regex.IsMatch(splitArray[i], @"\b-\b");
                    if (isPlusPresent == true || isminusPresent == true)
                    {
                        double arryresult = objParse.EvaluateFormula(splitArray[i], false);
                        splitArray[i] = arryresult.ToString();
                    }
                }

                if (Convert.ToDouble(splitArray[0]) < Convert.ToDouble(splitArray[1]))
                {
                    resultListValidations.Add(true);
                }
                else
                {
                    resultListValidations.Add(false);
                }
            }
            else if (isGreaterThanPresent)
            {
                splitArray = Regex.Split(singleformula, @"\>+");
                for (int i = 0; i < splitArray.Length; i++)
                {
                    bool isPlusPresent = Regex.IsMatch(splitArray[i], @"\b+\b");
                    bool isminusPresent = Regex.IsMatch(splitArray[i], @"\b-\b");
                    if (isPlusPresent == true || isminusPresent == true)
                    {
                        double arryresult = objParse.EvaluateFormula(splitArray[i], false);
                        splitArray[i] = arryresult.ToString();
                    }
                }
                if (Convert.ToDouble(splitArray[0]) > Convert.ToDouble(splitArray[1]))
                {
                    resultListValidations.Add(true);
                }
                else
                {
                    resultListValidations.Add(false);
                }
            }
            else if (isEqualToPresent)
            {
                splitArray = Regex.Split(singleformula, @"\=+");
                for (int i = 0; i < splitArray.Length; i++)
                {
                    bool isPlusPresent = Regex.IsMatch(splitArray[i], @"\b+\b");
                    bool isminusPresent = Regex.IsMatch(splitArray[i], @"\b-\b");
                    if (isPlusPresent == true || isminusPresent == true)
                    {
                        double arryresult = objParse.EvaluateFormula(splitArray[i], false);
                        splitArray[i] = arryresult.ToString();
                    }
                }
                if (Convert.ToDouble(splitArray[0]) == Convert.ToDouble(splitArray[1]))
                {
                    resultListValidations.Add(true);
                }
                else
                {
                    resultListValidations.Add(false);
                }
            }
            return resultListValidations;
        }


        public List<DataRow> GetShapeParams(int ShapeId)
        {
            DataSet dsParameters = new DataSet();
            List<DataRow> parameterList = new List<DataRow>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@ShapeId", ShapeId);
                //  dsParameters = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "GetShapeParametersForShapeId");
                dsParameters = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.GetShapeParametersForShapeId, dynamicParameters, commandType: CommandType.StoredProcedure);


                if (dsParameters != null && dsParameters.Tables.Count != 0)
                {
                    parameterList = dsParameters.Tables[0].AsEnumerable().ToList();
                }
            }
            return parameterList;
        }

        #endregion

        public string GetShapeIdByShapeCode(string ShapeCode)
        {
            int ShapeId;
            string query = "SELECT intshapeId FROM shapemaster WHERE chrshapecode= '" + ShapeCode + "'";
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                // ShapeId = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.Text, query));
                ShapeId = sqlConnection.QueryFirstOrDefault<int>(query, CommandType.Text);
            }
            return ShapeId.ToString();
        }

        public string GetProductCodeDetails(string productmarkingid)
        {

            int productmarkid = Convert.ToInt32(productmarkingid);
            string ProductCode;
            string query = "SELECT DISTINCT vchproductcode FROM meshproductmarkingdetails INNER JOIN ProductCodeMaster on meshproductmarkingdetails.intproductcodeid=ProductCodeMaster.intproductcodeid where intmeshproductmarkid = " + productmarkid;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                //  ProductCode = Convert.ToString(dbManager.ExecuteScalar(CommandType.Text, query));
                ProductCode = sqlConnection.QueryFirstOrDefault<string>(query, CommandType.Text);
            }
            return ProductCode;
        }

        public string GetProductCodeParameters(string enteredText)
        {
            List<ProductCode> listSlabProductCode = new List<ProductCode>();
            string cw_weight_m_run = "", weight_per_area = "", mw_weight_m_run = "";
            ProductCode objProductCode = new ProductCode();
            try
            {
                listSlabProductCode = objProductCode.SlabProductCodeFilter(enteredText);
                if (listSlabProductCode != null)
                {
                    cw_weight_m_run = Convert.ToString(listSlabProductCode[0].CwWeightPerMeterRun);
                    weight_per_area = Convert.ToString(listSlabProductCode[0].WeightArea);
                    mw_weight_m_run = Convert.ToString(listSlabProductCode[0].WeightPerMeterRun);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductCode = null;
            }
            return cw_weight_m_run + "#" + weight_per_area + "#" + mw_weight_m_run;
        }

        public string ConvertJsonFormat(Dictionary<string, string> inputs)
        {
            //DrainJsonFormat DrainJsonObj = new DrainJsonFormat();
            string dictionaryString = "{";
            foreach (KeyValuePair<string, string> item in inputs)
            {
                dictionaryString += item.Key + " : " + item.Value + ", ";
            }
            return dictionaryString.TrimEnd(',', ' ') + "}";
        }

        public void FillData()
        {
            dict_input_parameters.Add("column_width", "200");
            dict_input_parameters.Add("column_length", "600");
            dict_input_parameters.Add("top_cover", "20");
            dict_input_parameters.Add("bottom_cover", "20");
            dict_input_parameters.Add("left_cover", "20");
            dict_input_parameters.Add("right_cover", "20");
            dict_input_parameters.Add("no_of_link", "0");
            dict_input_parameters.Add("leg", "20");
            dict_input_parameters.Add("cw_length", "0");
            dict_input_parameters.Add("mw_spacing", "100");
            dict_input_parameters.Add("mw_dia", "10");
            dict_input_parameters.Add("cw_dia", "8");
            dict_input_parameters.Add("weight_per_area", "6.6");
            dict_input_parameters.Add("mw_weight_per_m_run", "0.617");
            dict_input_parameters.Add("cw_weight_per_m_run", "0.395");
            dict_input_parameters.Add("pin", "32");
            dict_input_parameters.Add("no_cw", "3");
        }

        public Dictionary<string, string> GetStructuteMarkingFormulae(string strelement)
        {

            List<SlabDetailingComponent> listStructureMarking = new List<SlabDetailingComponent>();
            DataSet dsProductMark = new DataSet();
            Dictionary<string, string> result_set = new Dictionary<string, string>();
            var comparer = StringComparer.OrdinalIgnoreCase;


            IEnumerable<ColumnStructureMarkingFormulaeElementDto> StructureMarkingFormulaeElementDto;

            Dictionary<string, string> dict_product_marking_formulae = new Dictionary<string, string>(comparer);
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    //FillData();
                    dynamicParameters.Add("@StructuralElement", strelement);
                    // dsProductMark = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "GetStructureMarkingFormulaeElement");
                    StructureMarkingFormulaeElementDto = sqlConnection.Query<ColumnStructureMarkingFormulaeElementDto>(SystemConstants.GetStructureMarkingFormulaeElement, dynamicParameters, commandType: CommandType.StoredProcedure);
                    if (StructureMarkingFormulaeElementDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(StructureMarkingFormulaeElementDto);
                        dsProductMark.Tables.Add(dt);
                    }

                    if (dsProductMark != null && dsProductMark.Tables.Count != 0)
                    {
                        foreach (DataRowView dr in dsProductMark.Tables[0].DefaultView)
                        {
                            dict_product_marking_formulae.Add(Convert.ToString((dr["FormulaName"])), Convert.ToString((dr["Formula"])));

                        }
                    }

                    foreach (KeyValuePair<string, string> ele in dict_product_marking_formulae)
                    {
                        bool ifpresent = false;
                        bool concatpresent = false;
                        bool ifResultAlreadyCalculated = false;
                        ifResultAlreadyCalculated = result_set.ContainsKey(ele.Key);
                        if (!ifResultAlreadyCalculated)
                        {
                            ifpresent = ele.Value.Contains("if");
                            concatpresent = ele.Value.Contains("concat");
                            if (ifpresent)
                            {
                                result_set = EvaluateIfCondition(ele.Key, ele.Value, dict_product_marking_formulae);
                            }
                            else if (concatpresent)
                            {
                                result_set = EvaluateConcatCondition(ele.Key, ele.Value, dict_product_marking_formulae);
                            }
                            else
                            {
                                result_set = CalculateValueFromFormula(ele.Key, ele.Value, dict_product_marking_formulae);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

            return result_set;
        }


        #region DrainProductMarking

        public Dictionary<string, string> GetDrainProductMarkingFormulae(string strElement, string shapecode, string layername)
        {

            // dbManager.ConnectionString = "Data Source=NSPRDDB19\\MSSQL2022;Initial Catalog=NDSPRD;UID=ndswebapps;Password=DBAdmin4*NDS;Persist Security Info=True;";
            DataSet dsDrainProductMark = new DataSet();
            Dictionary<string, string> result_set = new Dictionary<string, string>();
            var comparer = StringComparer.OrdinalIgnoreCase;
            Dictionary<string, string> dict_product_marking_formulae_drain = new Dictionary<string, string>(comparer);
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@StrElement", strElement);

                    //// dsDrainProductMark = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "GetDrainProductMarkingFormuale");
                    //dsDrainProductMark = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.GetDrainProductMarkingFormuale, dynamicParameters, commandType: CommandType.StoredProcedure);

                    SqlCommand cmd = new SqlCommand(SystemConstants.GetDrainProductMarkingFormuale, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@StrElement", strElement));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsDrainProductMark);

                    DrainParameters drainparam = new DrainParameters();
                    List<DrainParameters> drainparamList = new List<DrainParameters>();
                    List<DrainParameters> FilteredDrainparamList = new List<DrainParameters>();
                    if (dsDrainProductMark != null && dsDrainProductMark.Tables.Count != 0)
                    {
                        drainparamList = dsDrainProductMark.Tables[0].AsEnumerable()
                                           .Select(dataRow => new DrainParameters
                                           {
                                               ShapeCode = dataRow.Field<string>("ShapeCode"),
                                               FormulaName = dataRow.Field<string>("FormulaName"),
                                               Formula = dataRow.Field<string>("Formula"),
                                               Layer = dataRow.Field<string>("Layer")
                                           }).ToList();
                    }
                    foreach (var item in drainparamList)
                    {
                        if (item.ShapeCode != null && item.ShapeCode != "")
                        {
                            if (item.ShapeCode.Trim() == shapecode.Trim() && item.Layer.Trim() == layername.Trim())
                            {
                                FilteredDrainparamList.Add(new DrainParameters
                                {
                                    ShapeCode = item.ShapeCode,
                                    FormulaName = item.FormulaName,
                                    Formula = item.Formula,
                                    Layer = item.Layer
                                });
                            }
                        }
                        else
                        {
                            FilteredDrainparamList.Add(new DrainParameters
                            {
                                ShapeCode = item.ShapeCode,
                                FormulaName = item.FormulaName,
                                Formula = item.Formula,
                                Layer = item.Layer
                            });
                        }
                    }

                    foreach (var item in FilteredDrainparamList)
                    {
                        if (item.ShapeCode != null && item.ShapeCode != "")
                        {

                        }
                        bool ifpresent = false;
                        bool ifResultAlreadyCalculated = false;
                        ifResultAlreadyCalculated = result_set.ContainsKey(item.FormulaName);
                        if (!ifResultAlreadyCalculated)
                        {
                            ifpresent = item.Formula.Contains("if");
                            if (ifpresent)
                            {
                                result_set = EvaluateIfConditionDrain(item.FormulaName, item.Formula, FilteredDrainparamList);
                            }

                            else
                            {
                                result_set = CalculateValueFromDrainFormula(item.FormulaName, item.Formula, FilteredDrainparamList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
             
                throw ex;

            }

            //foreach (var item in result_set.Where(kvp => kvp.Value == "0").ToList())
            //{
            //    result_set.Remove(item.Key);
            //}
            Dictionary<string, string> result_set_sorted = new Dictionary<string, string>();
            // SortedDictionary<string, string> result_set_sorted = new Dictionary<string, string>();
            result_set_sorted = result_set.Keys.OrderBy(k => k).ToDictionary(k => k, k => result_set[k]);
            return result_set_sorted;
        }

        public void CalculateVaribaleValuesForDrain(string formulaName, string formula, List<DrainParameters> listDrain)
        {
            try
            {
                Dictionary<string, string> result_set = new Dictionary<string, string>();
                string output = string.Empty;
                bool ifpresent = false;
                ifpresent = formula.Contains("if");
                if (ifpresent)
                {
                    result_set = EvaluateIfConditionDrain(formulaName, formula, listDrain);
                }
                else
                {
                    result_set = CalculateValueFromDrainFormula(formulaName, formula, listDrain);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, string> CalculateValueFromDrainFormula(string formulaName, string formula, List<DrainParameters> listDrain)
        {
            int count = 0;
            try
            {
                // MathParser parser = new MathParser();//commented by vanita
                MathParserWrapper parser = new ExpressionParser.MathParserWrapper();//Added by vanita
                bool isKeyPresent = false;
                string output = string.Empty;
                

                foreach (var ele in listDrain)
                {
                    //isKeyPresent = formula.Contains(ele.Key.ToLower());
                    isKeyPresent = Regex.IsMatch(formula, @"\b" + ele.FormulaName + @"\b");
                    var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.FormulaName.ToLower());
                    //bool IsPresentInInputs = formula.IndexOf(ele.Key, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (isKeyPresent == true && IsPresentInInputs == false)
                    {
                        CalculateVaribaleValuesForDrain(ele.FormulaName, ele.Formula, listDrain);
                    }
                    count++;
                }

                foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                {
                    isKeyPresent = formula.Contains(ele.Key);
                    if (isKeyPresent)
                    {
                        output = formula.SafeReplace(ele.Key, ele.Value, true);
                        formula = output;
                    }
                }

                formula = formula.Replace("+-", "-");
                MathParserWrapper objParse = new MathParserWrapper();
                double result = objParse.EvaluateFormula(formula, false);
                string[] splitArray;
                splitArray = formulaName.Split(null);
                string csvString = string.Join("_", splitArray).ToLower();
                if (!dict_input_parameters.ContainsKey(csvString) && !dict_result.ContainsKey(csvString))
                {
                    dict_input_parameters.Add(csvString, result.ToString());
                    dict_result.Add(csvString, result.ToString());
                }
                return dict_result;
            }
            catch (Exception ex)
            {
                var a = count;
                throw ex;
            }
        }

        public Dictionary<string, string> EvaluateIfConditionDrain(string formulaName, string formula, List<DrainParameters> drainparamList)
        {

            MathParserWrapper objParse = new MathParserWrapper();
            bool isEqualToPresent = false;
            bool isLessThanPresent = false;
            bool isGreaterThanPresent = false;
            string EqualToSign = "=";
            string LessThanSign = "<";
            string GreaterThanSign = ">";
            Dictionary<string, StringBuilder> ifDict = new Dictionary<string, StringBuilder>();
            List<string> list = new List<string>();
            list = ParseFormulaeFromIf(formula);
            string[] splittedEqualToList = null;
            bool isKeyPresent;
            string output = string.Empty;
            string formula1 = string.Empty;
            double ConditionResult = 0;
            List<double> resultList = new List<double>();
            double OutputValue = 0;
            //foreach (string singleformula in list)
            //{
            string singleformula = list[0];

            isEqualToPresent = Regex.IsMatch(singleformula, @"\b=\b");
            isLessThanPresent = Regex.IsMatch(singleformula, @"\b<\b");
            isGreaterThanPresent = Regex.IsMatch(singleformula, @"\b>\b");
            bool isGreaterThanEqualToPresent = Regex.IsMatch(singleformula, @"\b>=\b");
            if (isEqualToPresent)
            {
                splittedEqualToList = singleformula.Split('=');
            }
            else if (isLessThanPresent)
            {
                splittedEqualToList = singleformula.Split('<');
            }
            else if (isGreaterThanPresent)
            {
                splittedEqualToList = singleformula.Split('>');
            }
            else if (isGreaterThanEqualToPresent)
            {
                splittedEqualToList = Regex.Split(singleformula, @"\>=+");
            }


            if (splittedEqualToList.Length > 0)
            {
                foreach (var splitedFormula in splittedEqualToList)
                {
                    formula1 = splitedFormula.TrimEnd(',');

                    foreach (var ele in drainparamList)
                    {
                        isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.FormulaName + @"\b");
                        var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.FormulaName.ToLower());
                        if (isKeyPresent == true && IsPresentInInputs == false)
                        {
                            CalculateVaribaleValuesForDrain(ele.FormulaName, ele.Formula, drainparamList);
                        }
                    }


                    foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                    {
                        isKeyPresent = formula1.Contains(ele.Key);
                        if (isKeyPresent)
                        {
                            output = formula1.SafeReplace(ele.Key, ele.Value, true);
                            formula1 = output;

                        }
                    }
                    ConditionResult = objParse.EvaluateFormula(formula1, false);
                    resultList.Add(ConditionResult);
                }
                bool allAreSame = false;
                if (isEqualToPresent)
                {
                    allAreSame = resultList.All(x => x == resultList.First());
                }
                else if (isLessThanPresent)
                {
                    if (Convert.ToDouble(resultList[0]) < Convert.ToDouble(resultList[1]))
                    {
                        allAreSame = true;
                    }
                }
                else if (isGreaterThanPresent)
                {
                    if (Convert.ToDouble(resultList[0]) > Convert.ToDouble(resultList[1]))
                    {
                        allAreSame = true;
                    }
                }
                else if (isGreaterThanEqualToPresent)
                {
                    if (Convert.ToDouble(resultList[0]) >= Convert.ToDouble(resultList[1]))
                    {
                        allAreSame = true;
                    }
                }
                if (allAreSame == true)
                {
                    formula1 = list[1].TrimEnd(',');
                    int countofif = Regex.Matches(formula1, "if").Count;
                    if (countofif > 0)
                    {
                        OutputValue = EvaluateNestedIfConditionDrain(formulaName, formula1, drainparamList);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                    else
                    {

                        foreach (var ele in drainparamList)
                        {
                            isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.FormulaName + @"\b");
                            var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.FormulaName.ToLower());
                            if (isKeyPresent == true && IsPresentInInputs == false)
                            {
                                CalculateVaribaleValuesForDrain(ele.FormulaName, ele.Formula, drainparamList);
                            }
                        }

                        foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                        {
                            isKeyPresent = formula1.Contains(ele.Key);
                            if (isKeyPresent)
                            {
                                output = formula1.SafeReplace(ele.Key, ele.Value, true);
                                formula1 = output;

                            }
                        }
                        OutputValue = objParse.EvaluateFormula(formula1, false);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }

                }
                else
                {
                    formula1 = list[2].TrimEnd(',');
                    int countofif = Regex.Matches(formula1, "if").Count;
                    if (countofif > 0)
                    {
                        OutputValue = EvaluateNestedIfConditionDrain(formulaName, formula1, drainparamList);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                    else
                    {
                        foreach (var ele in drainparamList)
                        {
                            isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.FormulaName + @"\b");
                            var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.FormulaName.ToLower());
                            if (isKeyPresent == true && IsPresentInInputs == false)
                            {
                                CalculateVaribaleValuesForDrain(ele.FormulaName, ele.Formula, drainparamList);
                            }
                        }


                        foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                        {
                            isKeyPresent = formula1.Contains(ele.Key);
                            if (isKeyPresent)
                            {
                                output = formula1.SafeReplace(ele.Key, ele.Value, true);
                                formula1 = output;

                            }
                        }
                        OutputValue = objParse.EvaluateFormula(formula1, false);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                }
            }
            // }

            return dict_result;
        }

        public double EvaluateNestedIfConditionDrain(string formulaName, string formula, List<DrainParameters> drainparamList)
        {
            MathParserWrapper objParse = new MathParserWrapper();
            bool isEqualToPresent = false;
            bool isLessThanPresent = false;
            bool isGreaterThanPresent = false;
            string EqualToSign = "=";
            string LessThanSign = "<";
            string GreaterThanSign = ">";

            List<string> list = new List<string>();
            list = ParseFormulaeFromIf(formula);
            string[] splittedEqualToList = null;
            bool isKeyPresent;
            string output = string.Empty;
            string formula1 = string.Empty;
            double ConditionResult = 0;
            List<double> resultList = new List<double>();
            double OutputValue = 0;
            //foreach (string singleformula in list)
            //{
            string singleformula = list[0];

            isEqualToPresent = Regex.IsMatch(singleformula, @"\b=\b");
            isLessThanPresent = Regex.IsMatch(singleformula, @"\b<\b");
            isGreaterThanPresent = Regex.IsMatch(singleformula, @"\b>\b");
            bool isGreaterThanEqualToPresent = Regex.IsMatch(singleformula, @"\b>=\b");
            if (isEqualToPresent)
            {
                splittedEqualToList = singleformula.Split('=');
            }
            else if (isLessThanPresent)
            {
                splittedEqualToList = singleformula.Split('<');
            }
            else if (isGreaterThanPresent)
            {
                splittedEqualToList = singleformula.Split('>');
            }
            else if (isGreaterThanEqualToPresent)
            {
                splittedEqualToList = Regex.Split(singleformula, @"\>=+");
            }

            if (splittedEqualToList.Length > 0 && splittedEqualToList != null)
            {
                //splittedEqualToList = singleformula.Split('=');
                foreach (var splitedFormula in splittedEqualToList)
                {
                    formula1 = splitedFormula.TrimEnd(',');

                    foreach (var ele in drainparamList)
                    {
                        isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.FormulaName + @"\b");
                        var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.FormulaName.ToLower());
                        if (isKeyPresent == true && IsPresentInInputs == false)
                        {
                            CalculateVaribaleValuesForDrain(ele.FormulaName, ele.Formula, drainparamList);
                        }
                    }

                    foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                    {
                        isKeyPresent = formula1.Contains(ele.Key);
                        if (isKeyPresent)
                        {
                            output = formula1.SafeReplace(ele.Key, ele.Value, true);
                            formula1 = output;

                        }
                    }
                    ConditionResult = objParse.EvaluateFormula(formula1, false);
                    resultList.Add(ConditionResult);
                }
                bool allAreSame = false;
                if (isEqualToPresent)
                {
                    allAreSame = resultList.All(x => x == resultList.First());
                }
                else if (isLessThanPresent)
                {
                    if (Convert.ToDouble(resultList[0]) < Convert.ToDouble(resultList[1]))
                    {
                        allAreSame = true;
                    }
                }
                else if (isGreaterThanPresent)
                {
                    if (Convert.ToDouble(resultList[0]) > Convert.ToDouble(resultList[1]))
                    {
                        allAreSame = true;
                    }
                }
                else if (isGreaterThanEqualToPresent)
                {
                    if (Convert.ToDouble(resultList[0]) >= Convert.ToDouble(resultList[1]))
                    {
                        allAreSame = true;
                    }
                }
                if (allAreSame == true)
                {
                    formula1 = list[1].TrimEnd(',');
                    int countofif = Regex.Matches(formula1, "if").Count;
                    if (countofif > 0)
                    {
                        OutputValue = EvaluateNestedIfConditionDrain(formulaName, formula1, drainparamList);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                    else
                    {

                        foreach (var ele in drainparamList)
                        {
                            isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.FormulaName + @"\b");
                            var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.FormulaName.ToLower());
                            if (isKeyPresent == true && IsPresentInInputs == false)
                            {
                                CalculateVaribaleValuesForDrain(ele.FormulaName, ele.Formula, drainparamList);
                            }
                        }

                        foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                        {
                            isKeyPresent = formula1.Contains(ele.Key);
                            if (isKeyPresent)
                            {
                                output = formula1.SafeReplace(ele.Key, ele.Value, true);
                                formula1 = output;

                            }
                        }
                        OutputValue = objParse.EvaluateFormula(formula1, false);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }

                }
                else
                {
                    formula1 = list[2].TrimEnd(',');
                    int countofif = Regex.Matches(formula1, "if").Count;
                    if (countofif > 0)
                    {
                        OutputValue = EvaluateNestedIfConditionDrain(formulaName, formula1, drainparamList);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                    else
                    {
                        foreach (var ele in drainparamList)
                        {
                            isKeyPresent = Regex.IsMatch(formula1, @"\b" + ele.FormulaName + @"\b");
                            var IsPresentInInputs = dict_input_parameters.ContainsKey(ele.FormulaName.ToLower());
                            if (isKeyPresent == true && IsPresentInInputs == false)
                            {
                                CalculateVaribaleValuesForDrain(ele.FormulaName, ele.Formula, drainparamList);
                            }
                        }


                        foreach (KeyValuePair<string, string> ele in dict_input_parameters)
                        {
                            isKeyPresent = formula1.Contains(ele.Key);
                            if (isKeyPresent)
                            {
                                output = formula1.SafeReplace(ele.Key, ele.Value, true);
                                formula1 = output;

                            }
                        }
                        OutputValue = objParse.EvaluateFormula(formula1, false);
                        //dict_result.Add(formulaName, OutputValue.ToString());
                        AddCalculatedFormulaeToDictionary(formulaName, OutputValue.ToString());
                        AddInputParametersToDictionary(formulaName, OutputValue.ToString());
                    }
                }
            }
            //}

            return OutputValue;
        }

        #endregion DrainProductMarking


        #endregion


    }

    public static class StringExtensions
    {
        public static string SafeReplace(this string input, string find, string replace, bool matchWholeWord)
        {
            string textToFind = matchWholeWord ? string.Format(@"\b{0}\b", find) : find;
            return Regex.Replace(input, textToFind, replace);
        }
    }

    public class Validations
    {
        public string Attribute;
        public string ValidationConstraint;
    }

    public class DrainParameters
    {
        public string ShapeCode;
        public string Layer;
        public string FormulaName;
        public string Formula;
    }

    //class DrainJsonFormat
    //{
    //    public string ParamName;
    //    public string ParamValue;
    //}  //commented by vidya


}
