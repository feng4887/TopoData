using Flee.PublicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//***************************************************************************************
//Description: Flee expression evaluation library.
//Author: 杜金旺 Jim DU
//Data: 2026.11.16
//****************************************************************************************
namespace auDASLib
{
    public class FleeOperator
    {
        /// <summary>
        /// 根据表达式获取条件是否满足， etc "(a = 100 OR b > 0) AND c <> 2"
        /// </summary>
        /// <param name="exString"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public static bool ConditionIsOK(string exString, Dictionary<string, dynamic> paramters)
        {
            ExpressionContext context = new ExpressionContext();
            VariableCollection variables = context.Variables;
            //variables.Add("a", 100);
            //variables.Add("b", 1);
            //variables.Add("c", 24);
            foreach (var para in paramters)
            {
                variables.Add(para.Key, para.Value);
            }
            IGenericExpression<bool> e = context.CompileGeneric<bool>(exString);
            bool result = e.Evaluate();
            return result;
        }
    }
}
