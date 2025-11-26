using System;
using System.Text;
using System.Collections.Generic;

namespace ExpressionParser
{
    public class MathParserWrapper
    {
        private char[] Operands = new char[] { '+', '-', '*', '/', '^' };
        private StringBuilder mathSymbolBuilder = new StringBuilder();
        private StringBuilder parameterNameBuilder = new StringBuilder();
        private StringBuilder staticValueBuilder = new StringBuilder();
        private StringBuilder simplifiedFormulaBuilder = new StringBuilder();

        MathParser parser = new MathParser();



        public double GetCalculatedValue(string FormulaString, List<valuePair> valuePairs,bool isRadians = true )
        {
            double retVal = 0;
            int count = 1;

            string _formulaString = FormulaString.Trim().ToUpper().Replace('{', '(').Replace('[', '(').Replace('}', ')').Replace(']', ')');

            //Start:Added for testing +/- sign for Acute Angle CR 
            
            //char[] phraseAsChars = _formulaString.ToCharArray();
            //int signIndex = _formulaString.IndexOf("+SQ");
            //if (signIndex != -1)
            //{
            //    phraseAsChars[signIndex++] = '-';
                
            //}
            //_formulaString = new string(phraseAsChars);
            //End:Testing


            foreach (char thisChar in _formulaString.ToCharArray())
            {
                //Ignore space bars within the formula while processing
                if (!thisChar.Equals(' '))
                {
                    if (thisChar.Equals('('))
                    {
                        if (mathSymbolBuilder.Length > 0)
                        {
                            simplifiedFormulaBuilder.Append(mathSymbolBuilder.ToString());
                            NukeEmAll();
                        }
                        simplifiedFormulaBuilder.Append(thisChar);
                    }
                    else if (thisChar.Equals(')'))
                    {
                        if (parameterNameBuilder.Length > 0)
                        {
                            simplifiedFormulaBuilder.Append(GetParamValue(parameterNameBuilder.ToString(), ref valuePairs));
                        }
                        else if (staticValueBuilder.Length > 0)
                        {
                            simplifiedFormulaBuilder.Append(staticValueBuilder.ToString());
                        }
                        simplifiedFormulaBuilder.Append(thisChar);
                        NukeEmAll();
                    }
                    //else if (Operands.Contains(thisChar))
                    else if (Array.IndexOf(Operands,thisChar)>=0)
                    {
                        if (parameterNameBuilder.Length > 0)
                        {
                            simplifiedFormulaBuilder.Append(GetParamValue(parameterNameBuilder.ToString(), ref valuePairs));
                        }
                        else if (staticValueBuilder.Length > 0)
                        {
                            simplifiedFormulaBuilder.Append(staticValueBuilder.ToString());
                        }
                        simplifiedFormulaBuilder.Append(thisChar);
                        NukeEmAll();
                    }
                    else if (IsStringOrValue(thisChar) && !thisChar.Equals('.'))
                    {
                        mathSymbolBuilder.Append(thisChar);
                        parameterNameBuilder.Append(thisChar);
						if (count == _formulaString.ToCharArray().Length)
						{
							simplifiedFormulaBuilder.Append(GetParamValue(parameterNameBuilder.ToString(), ref valuePairs));
						}
                    }
                    else
                    {
                        staticValueBuilder.Append(thisChar);
                        if (count == _formulaString.ToCharArray().Length)
                        {
                            simplifiedFormulaBuilder.Append(staticValueBuilder.ToString());
                        }
                    }
                }
                    count++;
            }
			//if the formula contains inverse trigonometric functions set the is radian to true.
			if (simplifiedFormulaBuilder.ToString().ToUpper().Contains("ASIN") || simplifiedFormulaBuilder.ToString().ToUpper().Contains("ACOS") || simplifiedFormulaBuilder.ToString().ToUpper().Contains("ATAN"))
			{
				isRadians = true;
			}
			retVal = parser.Parse(simplifiedFormulaBuilder.ToString(), isRadians);
      
            return retVal;
        }

        //=================Test=====================================//
        public string Test_GetCalculatedValue(string FormulaString, List<valuePair> valuePairs, bool isRadians = true)
        {
            double retVal = 0;
            int count = 1;

            string _formulaString = FormulaString.Trim().ToUpper().Replace('{', '(').Replace('[', '(').Replace('}', ')').Replace(']', ')');

            foreach (char thisChar in _formulaString.ToCharArray())
            {
                //Ignore space bars within the formula while processing
                if (!thisChar.Equals(' '))
                {
                    if (thisChar.Equals('('))
                    {
                        if (mathSymbolBuilder.Length > 0)
                        {
                            simplifiedFormulaBuilder.Append(mathSymbolBuilder.ToString());
                            NukeEmAll();
                        }
                        simplifiedFormulaBuilder.Append(thisChar);
                    }
                    else if (thisChar.Equals(')'))
                    {
                        if (parameterNameBuilder.Length > 0)
                        {
                            simplifiedFormulaBuilder.Append(GetParamValue(parameterNameBuilder.ToString(), ref valuePairs));
                        }
                        else if (staticValueBuilder.Length > 0)
                        {
                            simplifiedFormulaBuilder.Append(staticValueBuilder.ToString());
                        }
                        simplifiedFormulaBuilder.Append(thisChar);
                        NukeEmAll();
                    }
                    //else if (Operands.Contains(thisChar))
                    else if (Array.IndexOf(Operands, thisChar) >= 0)
                    {
                        if (parameterNameBuilder.Length > 0)
                        {
                            simplifiedFormulaBuilder.Append(GetParamValue(parameterNameBuilder.ToString(), ref valuePairs));
                        }
                        else if (staticValueBuilder.Length > 0)
                        {
                            simplifiedFormulaBuilder.Append(staticValueBuilder.ToString());
                        }
                        simplifiedFormulaBuilder.Append(thisChar);
                        NukeEmAll();
                    }
                    else if (IsStringOrValue(thisChar) && !thisChar.Equals('.'))
                    {
                        mathSymbolBuilder.Append(thisChar);
                        parameterNameBuilder.Append(thisChar);
                        if (count == _formulaString.ToCharArray().Length)
                        {
                            simplifiedFormulaBuilder.Append(GetParamValue(parameterNameBuilder.ToString(), ref valuePairs));
                        }
                    }
                    else
                    {
                        staticValueBuilder.Append(thisChar);
                        if (count == _formulaString.ToCharArray().Length)
                        {
                            simplifiedFormulaBuilder.Append(staticValueBuilder.ToString());
                        }
                    }
                }
                count++;
            }
            //if the formula contains inverse trigonometric functions set the is radian to true.
            if (simplifiedFormulaBuilder.ToString().ToUpper().Contains("ASIN") || simplifiedFormulaBuilder.ToString().ToUpper().Contains("ACOS") || simplifiedFormulaBuilder.ToString().ToUpper().Contains("ATAN"))
            {
                isRadians = true;
            }
            retVal = parser.Parse(simplifiedFormulaBuilder.ToString(), isRadians);

            return simplifiedFormulaBuilder.ToString();
        }
        //====================================================//


        //Added by aishwarya

        public double EvaluateFormula(string FormulaString, bool isRadians = false)
        {
            double retVal = 0;
            retVal = parser.Parse(FormulaString, isRadians,true);
            return retVal;
        }

        private void NukeEmAll()
        {
            mathSymbolBuilder = new StringBuilder();
            staticValueBuilder = new StringBuilder();
            parameterNameBuilder = new StringBuilder();
        }

        private double GetParamValue(string paramKey, ref List<valuePair> valueList)
        {
            double retVal = 0;
            foreach (valuePair value in valueList)
            {
                if (value._key == paramKey)
                {
                    retVal = value._value;
                    break;
                }
            }

            return retVal;
        }

        private Boolean IsStringOrValue(char value)
        {
            Boolean retVal = false;
            //String = True, Int = false
            try
            {
                int intVal = int.Parse(value.ToString());
                retVal = false;
            }
            catch (Exception ex)
            {
                retVal = true;
            }
            return retVal;
        }
    }

    public class valuePair
    {
        public double _value { get; set; }
        public string _key { get; set; }

        public valuePair(string Key, double Value)
        {
            this._value = Value;
            this._key = Key;
        }
    }
}
