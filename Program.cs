using System;
using System.Collections.Generic;

class Program{
	static void Main(string[] args){
		RPN rpn;
		if(args.Length==0 || args.Length==3 || args.Length>5){
			Console.WriteLine("Hey boss, problem:\nincorrect number of arguments");
			return;
		}else if(args.Length==1){
			rpn = new RPN(args[0]);
			if(rpn.preValidate()){
				rpn.divideTokens();
				if(rpn.postValidate()){
					rpn.printInfix();
					rpn.toPostfix();
					rpn.printPostfix();
					List<string> postfixTokens = rpn.getInfixTokens();
					foreach(string token in postfixTokens){
						if(token=="x") return;
					}
					double res = rpn.evaluateForX(0);
					if(!double.IsNaN(res)){
						Console.WriteLine(res);
					}
				}
			}
		}else if(args.Length==2){
			rpn = new RPN(args[0]);
			if(rpn.preValidate()){
				rpn.divideTokens();
				if(rpn.postValidate()){
					rpn.printInfix();
					rpn.toPostfix();
					rpn.printPostfix();
					double res = rpn.evaluateForX(double.Parse(args[1]));
					if(!double.IsNaN(res)){
						Console.WriteLine(res);
					}
				}
			}
		}else if(args.Length==4){
			rpn = new RPN(args[0]);
			if(rpn.preValidate()){
				rpn.divideTokens();
				if(rpn.postValidate()){
					rpn.printInfix();
					rpn.toPostfix();
					rpn.printPostfix();
					double[,] result = rpn.evaluateForRange(double.Parse(args[1]),double.Parse(args[2]),int.Parse(args[3]));
					for(int i=0; i<result.GetLength(1); i++){
						if(!double.IsNaN(result[1,i])){
							Console.WriteLine($"{result[0,i]} => {result[1,i]}");
						}else break;
					}
				}
			}
		}else{
			rpn = new RPN(args[0]);
			if(rpn.preValidate()){
				rpn.divideTokens();
				if(rpn.postValidate()){
					rpn.printInfix();
					rpn.toPostfix();
					rpn.printPostfix();
					double res = rpn.evaluateForX(double.Parse(args[1]));
					if(!double.IsNaN(res)){
						Console.WriteLine(res);
					}
					double[,] result = rpn.evaluateForRange(double.Parse(args[2]),double.Parse(args[3]),int.Parse(args[4]));
					for(int i=0; i<result.GetLength(1); i++){
						if(!double.IsNaN(result[1,i])){
							Console.WriteLine($"{result[0,i]} => {result[1,i]}");
						}else break;
					}
				}
			}
		}
		if(rpn.getErrorMsg()!="")
			Console.WriteLine("Hey boss, problem:\n"+rpn.getErrorMsg());
	}
}
