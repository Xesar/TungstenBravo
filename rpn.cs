using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TungstenBravo{
	class rpn{
	private string formula="";
	private bool preValidated = false;
	private bool tokensDivided = false;
	private bool postValidated = false;
	private bool postfixParsed = false;
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
	public rpn(string formula){
		this.formula=Regex.Replace(formula,@"\s+","").ToLower();
		this.formula=this.formula.Replace(".",",");
		if(this.formula[0]=='-')
			this.formula=this.formula.Insert(0,"0");
		preValidate();
	}
	public void preValidate(){
		Regex operators = new Regex(@"[\+*/^%]");
		int bracketCount = 0;

		if(string.IsNullOrEmpty(formula))
			throw new rpnException("No formula detected");

		if(formula.Last()=='(')
			throw new rpnException("Wrong number of brackets");

		for(int i=0; i<formula.Length; i++){
			if(formula[i]=='('){
				if(formula[i+1]==')')
					throw new rpnException("Empty brackets");
				bracketCount++;
			}
			if(formula[i]==')')
				bracketCount--;
		}
		if(bracketCount!=0)
			throw new rpnException("Wrong number of brackets");

		string temp = operators.Replace(formula,".");
		string[] incorrectOperatorOrder = new string[]{"(.)","(.",".)","..",",,","(,",",)","(-)","-)","--"};
		foreach(string str in incorrectOperatorOrder)
			if(temp.Contains(str))
				throw new rpnException("Wrong operator order");

		if(formula.StartsWith("*") || formula.StartsWith("/") || formula.StartsWith("+") || formula.StartsWith("-") || formula.StartsWith("^"))
			throw new rpnException("Cannot begin formula with an operator");

		if(formula.EndsWith("*") || formula.EndsWith("/") || formula.EndsWith("+") || formula.EndsWith("-") || formula.EndsWith("^"))
			throw new rpnException("Cannot end formula with an operator");

		this.preValidated=true;
	}
	public void divideTokens(){
		if(!preValidated)
			preValidate();

		string shortTokens = "abs cos exp log sin tan";
		string longTokens = "sqrt cosh sinh tanh acos asin atan";
		for(int i=0; i<formula.Length; i++){
			string shortToken = "";
			string longToken = "";
			if(i+2<formula.Length)
				shortToken = formula[i].ToString()+formula[i+1].ToString()+formula[i+2].ToString();
			if(i+3<formula.Length)
				longToken = formula[i].ToString()+formula[i+1].ToString()+formula[i+2].ToString()+formula[i+3].ToString();
			if("()^*/+-".Contains(formula[i])){
				if(i>0 && formula[i-1]=='(' && formula[i]=='-')
					infixTokens.Add(0d.ToString());
				infixTokens.Add(formula[i].ToString());
				if(i+1<formula.Length && formula[i]==')' && (formula[i+1]=='x' || "1234567890".Contains(formula[i+1])))
					infixTokens.Add("*");
			}else if(longToken.Length>0 && longTokens.Contains(longToken)){
				infixTokens.Add(longToken);
				i+=3;
			}else if(shortToken.Length>0 && shortTokens.Contains(shortToken)){
				infixTokens.Add(shortToken);
				i+=2;
			}else{
				if(i+1<formula.Length && formula[i]=='x' && "1234567890".Contains(formula[i+1])){
					infixTokens.Add("x");
					infixTokens.Add("*");
					i++;
				}
				string temp = formula[i].ToString();
				while(i+1<formula.Length && "1234567890,".Contains(formula[i+1])){
					temp+=formula[i+1].ToString();
					i++;
				}
				infixTokens.Add(temp);
				if(i+1<formula.Length && (formula[i+1]=='x' || formula[i+1]=='('))
					infixTokens.Add("*");
			}
		}
		this.tokensDivided=true;
		postValidate();
	}
	public void postValidate(){
		if(!tokensDivided)
			divideTokens();

		Regex numRegex = new Regex(@"^\d*\,?\d*$");
		string[] allowedTokens = new string[]{"(",")","+","-","/","*","^","x","abs","cos","exp","log","sin","tan","sqrt","cosh","sinh","tanh","acos","asin","atan"};
		
		foreach(string token in infixTokens)
			if(!numRegex.IsMatch(token) && !allowedTokens.Contains(token))
				throw new rpnException(token+": not number/operator/math function or other unallowed character");
		
		this.postValidated=true;
	}
	public void toPostfix(){
		if(!postValidated)
			postValidate();

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

		this.postfixParsed=true;
	}
	public double evaluateForX(double x){
		if(!postfixParsed)
			toPostfix();

		Stack<string> s = new Stack<string>();
		Regex numRegex = new Regex(@"^\d*\,?\d*$");
		string mathFunctions = "abs cos exp log sin tan sqrt cosh sinh tanh acos asin atan";
		foreach(string token in postfixTokens){
			if(numRegex.IsMatch(token)){
				s.Push(token);
			}else if(token=="x"){
				s.Push(x.ToString());
			}else if("^*/+-".Contains(token)){
				double a = double.Parse(s.Pop());
				double b = double.Parse(s.Pop());
				if(token=="^")
					a=Math.Pow(b,a);
				else if(token=="*")
					a=b*a;
				else if(token=="/"){
					if(a==0)
						throw new rpnException("Division by 0");
					a=b/a;
				}
				else if(token=="+")
					a=b+a;
				else
					a=b-a;
				s.Push(a.ToString());
			}else if(mathFunctions.Contains(token)){
				string tempToken = token.Substring(0,1).ToUpper()+token.Substring(1);
				MethodInfo method = typeof(Math).GetMethod(tempToken,new[] {typeof(double)});
				s.Push(method.Invoke(null,new object[]{double.Parse(s.Pop())}).ToString());
			}
		}
		double result = double.Parse(s.Pop());
		if(Double.IsNaN(result))
			throw new rpnException("Result not computable, not a number");
		else
			return result;
	}
	public double[,] evaluateForRange(double start, double end, int N){
		double[,] results = new double[2,N];
		double h = (end-start)/(N-1.0);
		for(int i=0; i<N; i++){
			results[0,i]=start+i*h;
			results[1,i]=evaluateForX(start+i*h);
		}
		return results;
	}
	public void setFormula(string formula){
		this.formula=formula;
	}
	public string getFormula(){
		return formula;
	}
	public void printFormula(){
		Console.WriteLine(formula);
	}
	public List<string> getInfixTokens(){
		if(!tokensDivided)
			divideTokens();
		return infixTokens;
	}
	public void printInfix(){
		if(!tokensDivided)
			divideTokens();
		foreach(string str in infixTokens)
			Console.Write(str+" ");
		Console.WriteLine();
	}
	public List<string> getPostfixTokens(){
		if(!postfixParsed)
			toPostfix();
		return postfixTokens;
	}
	public void printPostfix(){
		if(!postfixParsed)
			toPostfix();
		foreach(string str in postfixTokens)
			Console.Write(str+" ");
		Console.WriteLine();
	}
}	
}