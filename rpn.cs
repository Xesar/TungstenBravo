using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class RPN{
	private string formula;
	private static Dictionary<string,int> priority = new Dictionary<string,int>(){
		{"abs",4},{"cos",4},{"exp",4},{"log",4},{"sin",4},{"sqrt",4},{"tan",4},
		{"cosh",4},{"sinh",4},{"tanh",4},{"acos",4},{"asin",4},{"atan",4},
		{"^",3},
		{"*",2},{"/",2},
		{"+",1},{"-",1},
		{"(",0}
	};
	public RPN(string formula){
		this.formula = Regex.Replace(formula,@"\s+","").ToLower();
		Console.WriteLine(check());
	}
	private bool check(){
		Stack<char> bracketStack = new Stack<char>();
		foreach(char element in this.formula.ToCharArray()){
			if(element=='('){
				bracketStack.Push(element);
			}
			else if(element==')'){
				if(bracketStack.Count==0)
					return false;
				bracketStack.Pop();
			}
		}
		return true;
	} 
}