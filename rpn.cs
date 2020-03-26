using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class RPN{
	private string formula;
	private string errorMsg;
	private List<string> infixTokens = new List<string>();
	private List<string> postfixTokens = new List<string>();
	private static Dictionary<string,int> d = new Dictionary<string,int>(){
		{"abs",4},{"cos",4},{"exp",4},{"log",4},{"sin",4},{"sqrt",4},{"tan",4},
		{"cosh",4},{"sinh",4},{"tanh",4},{"acos",4},{"asin",4},{"atan",4},
		{"^",3},
		{"*",2},{"/",2},
		{"+",1},{"-",1},
		{"(",0}
	};
	public RPN(string formula){
		this.formula = Regex.Replace(formula,@"\s+","").ToLower();
	}
	public void divideTokens(){
		string shortTokens = "abs cos exp log sin tan";
		string longTokens = "sqrt cosh sinh tanh acos asin atan";
		for(int i=0; i<formula.Length; i++){
			string shortToken = "";
			string longToken = "";
			if(i+2<formula.Length){
				shortToken = formula[i].ToString()+formula[i+1].ToString()+formula[i+2].ToString();
			}
			if(i+3<formula.Length){
				longToken = formula[i].ToString()+formula[i+1].ToString()+formula[i+2].ToString()+formula[i+3].ToString();
			}
			if("()^*/+-".Contains(formula[i])){
				infixTokens.Add(formula[i].ToString());
			}else if(shortToken.Length>0 && shortTokens.Contains(shortToken)){
				infixTokens.Add(shortToken);
				i+=2;
			}else if(longToken.Length>0 && longTokens.Contains(longToken)){
				infixTokens.Add(longToken);
				i+=3;
			}else{
				string temp = formula[i].ToString();
				while(i+1<formula.Length && "1234567890,.".Contains(formula[i+1])){
					temp+=formula[i+1].ToString();
					i++;
				}
				infixTokens.Add(temp);
			}
		}
	}
	public void toPostfix(){
		Stack<string> s = new Stack<string>();
		for(int i=0; i<infixTokens.Count; i++){
			if(!("()^*/+-".Contains(infixTokens[i])) && !(d.ContainsKey(infixTokens[i])))
				postfixTokens.Add(infixTokens[i]);
			else if(infixTokens[i]=="(")
				s.Push(infixTokens[i]);
			else if(infixTokens[i]==")"){
				while(s.Count>0 && s.Peek()!="(")
					postfixTokens.Add(s.Pop());
				s.Pop();
			}else{
				while(s.Count>0 && d[infixTokens[i]]<=d[s.Peek()])
					postfixTokens.Add(s.Pop());
				s.Push(infixTokens[i]);
			}
		}
		while(s.Count>0)
			postfixTokens.Add(s.Pop());
	}
	public bool validate(){
		Regex operators = new Regex(@"[\-+*/^%]");
		int beginBracket = 0, endBracket = 0;

		for(int i=0; i<formula.Length; i++){
			if(formula[i]=='('){
				if(formula[i+1]==')'){
					errorMsg="Epmty brackets";
					return false;
				}
				beginBracket++;
			}
			if(formula[i]==')') endBracket++;
		}
		if(beginBracket!=endBracket){
			errorMsg="Wrong number of brackets";
			return false;
		}

		if(string.IsNullOrEmpty(formula)){
			errorMsg="No formula detected";
			return false;
		}

		string temp = operators.Replace(formula,".");
		string[] incorrectOperatorOrder = new string[]{"(.",".)","..",",,","(,",",)"};
		foreach(string str in incorrectOperatorOrder){
			if(temp.Contains(str)){
				errorMsg="Wrong operator order";
				return false;
			}
		}

		if(formula.StartsWith("*") || formula.StartsWith("/") || formula.StartsWith("+") || formula.StartsWith("-") || formula.StartsWith("^")){
			errorMsg="Cannot begin formula with an operator";
			return false;
		}

		if(formula.EndsWith("*") || formula.EndsWith("/") || formula.EndsWith("+") || formula.EndsWith("-") || formula.EndsWith("^")){
			errorMsg="Cannot end formula with an operator";
			return false;
		}

		return true;
	}
	public string getFormula(){
		return formula;
	}
	public void printFormula(){
		Console.WriteLine(formula);
	}
	public string getErrorMsg(){
		return errorMsg;
	}
	public void printErrorMsg(){
		Console.WriteLine(errorMsg);
	}
	public List<string> getInfixTokens(){
		return infixTokens;
	}
	public List<string> getPostfixTokens(){
		return postfixTokens;
	}
	public void printInfix(){
		foreach(string str in infixTokens)
			Console.Write(str+" ");
		Console.WriteLine();
	}
	public void printPostfix(){
		foreach(string str in postfixTokens)
			Console.Write(str+" ");
		Console.WriteLine();
	}
}