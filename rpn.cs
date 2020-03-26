using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class RPN{
	private string formula;
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
		Console.WriteLine(this.formula);
		divideTokens();
		toPostfix();
        //-----------------------------------
        foreach(string str in postfixTokens){
            Console.Write(str+" ");
        }
        Console.WriteLine();
	}
	private void divideTokens(){
		for(int i=0; i<this.formula.Length; i++){
			if("()^*/+-".Contains(this.formula[i])){
				this.infixTokens.Add(this.formula[i].ToString());
			}else{
				string temp = this.formula[i].ToString();
				while(i+1<this.formula.Length && "1234567890".Contains(this.formula[i+1])){
					temp+=this.formula[i+1].ToString();
					i++;
				}
				this.infixTokens.Add(temp);
			}
		}
		foreach(string str in infixTokens){
			Console.Write(str+" ");
		}
		Console.WriteLine();
	}
	private void toPostfix(){
        Stack<string> s = new Stack<string>();
        for(int i=0; i<this.infixTokens.Count; i++){
            if(!("()^*/+-".Contains(infixTokens[i])))
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
	private bool check(){
		return true;
	}
}