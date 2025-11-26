using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NatSteel.NDS.BLL
{
	class MathParser
	{
		/// <summary>
		/// Method to evaluate a string expression.
		/// </summary>
		/// <param name="formula"></param>
		/// <param name="paramVal"></param>
		/// <returns></returns>
		public double Evaluate(string formula, string[,] paramVal)
		{
			try
			{
				// Instantiate the parser
				MathParserTK.MathParser parser = new MathParserTK.MathParser();

				double result = 0;

				if (paramVal.Length > 0)
				{
					//Method to get the formula string with values.
					formula = GetFormula(formula, paramVal);

					// Parse and write the result
					if (formula != "")
					{
						if (formula.ToUpper().Contains("ASIN"))
						{
							string partormula = formula.Substring(5, formula.Length - 2);
							result = Math.Asin(Convert.ToDouble(partormula) * (Math.PI / 180));
						}
						else
						{
							result = parser.Parse(formula, false);
						}
					}
				}
				return result;
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		/// <summary>
		/// Method to get the formula by replacing parameters by value.
		/// </summary>
		/// <param name="formula"></param>
		/// <param name="paramVal"></param>
		/// <returns></returns>
		public string GetFormula(string formula, string[,] paramVal)
		{
			try
			{
				if (formula.ToUpper().Contains("ASIN("))
				{
					string partFormula = formula.Substring(5, formula.Length - 1);
					for (int i = 0; i < paramVal.GetLength(0); i++)
					{
						if (paramVal[i, 0] != null)
						{
							if (partFormula.ToUpper().Contains(paramVal[i, 0].ToString().ToUpper()))
							{
								partFormula = partFormula.Replace(paramVal[i, 0].ToString().ToUpper(), paramVal[i, 1].ToString());
							}
						}
					}
					formula = "ASIN(" + partFormula;
				}
				else if (formula.ToUpper().Contains("SIN") || formula.ToUpper().Contains("COS") || formula.ToUpper().Contains("TAN"))
				{
					StringBuilder tempFormula = new StringBuilder();
					if (formula.ToUpper().Contains("SIN"))
					{
						string[] split = formula.ToUpper().Split(new string[] { "SIN" }, StringSplitOptions.None);
						string temp = string.Empty;
						foreach (string s in split)
						{
							for (int i = 0; i < paramVal.GetLength(0); i++)
							{
								if (paramVal[i, 0] != null)
								{
									if (s.ToUpper().Contains(paramVal[i, 0].ToString().ToUpper()))
									{
										temp = s.Replace(paramVal[i, 0].ToString().ToUpper(), paramVal[i, 1].ToString());
									}
								}
							}
							tempFormula.Append(temp);
							tempFormula.Append("SIN");
						}
					}
					if (formula.ToUpper().Contains("COS"))
					{
						string[] split = formula.ToUpper().Split(new string[] { "COS" }, StringSplitOptions.None);
						string temp = string.Empty;
						foreach (string s in split)
						{
							for (int i = 0; i < paramVal.GetLength(0); i++)
							{
								if (paramVal[i, 0] != null)
								{
									if (s.ToUpper().Contains(paramVal[i, 0].ToString().ToUpper()))
									{
										temp = s.Replace(paramVal[i, 0].ToString().ToUpper(), paramVal[i, 1].ToString());
									}
								}
							}
							tempFormula.Append(temp);
							tempFormula.Append("COS");
						}
					}
					if (formula.ToUpper().Contains("TAN"))
					{
						string[] split = formula.ToUpper().Split(new string[] { "TAN" }, StringSplitOptions.None);
						string temp = string.Empty;
						foreach (string s in split)
						{
							for (int i = 0; i < paramVal.GetLength(0); i++)
							{
								if (paramVal[i, 0] != null)
								{
									if (s.ToUpper().Contains(paramVal[i, 0].ToString().ToUpper()))
									{
										temp = s.Replace(paramVal[i, 0].ToString().ToUpper(), paramVal[i, 1].ToString());
									}
								}
							}
							tempFormula.Append(temp);
							tempFormula.Append("TAN");
						}
					}
					formula = tempFormula.ToString().Remove(tempFormula.ToString().Length - 3).ToString();
				}
				else
				{
					for (int i = 0; i < paramVal.GetLength(0); i++)
					{
						if (paramVal[i, 0] != null)
						{
							if (formula.ToUpper().Contains(paramVal[i, 0].ToString().ToUpper()))
							{
								formula = formula.Replace(paramVal[i, 0].ToString().ToUpper(), paramVal[i, 1].ToString());
							}
						}
					}
				}
				return formula;
			}
			catch (Exception ex)
			{
				return "";
			}
		}
	}
}
