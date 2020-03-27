using System;
using System.Collections.Generic;

class Program{
	static void Main(string[] args){
		if(args.Length==0 || args.Length==3 || args.Length>5){
			Console.WriteLine("Hey boss, problem:\nincorrect number of arguments");
			return;
		}
		if(args.Length==1){
			RPN rpn = new RPN(args[0]);
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
                    Console.WriteLine(rpn.evaluateForX(0));
                    return;
				}
			}
			Console.WriteLine("Hey boss, problem:\n"+rpn.getErrorMsg());
		}else if(args.Length==2){
			RPN rpn = new RPN(args[0]);
			if(rpn.preValidate()){
				rpn.divideTokens();
				if(rpn.postValidate()){
					rpn.printInfix();
					rpn.toPostfix();
					rpn.printPostfix();
					Console.WriteLine(rpn.evaluateForX(double.Parse(args[1])));
					return;
				}
			}
			Console.WriteLine("Hey boss, problem:\n"+rpn.getErrorMsg());
		}else if(args.Length==4){
			RPN rpn = new RPN(args[0]);
			if(rpn.preValidate()){
				rpn.divideTokens();
				if(rpn.postValidate()){
					rpn.printInfix();
					rpn.toPostfix();
					rpn.printPostfix();
					double[,] result = rpn.evaluateForRange(double.Parse(args[1]),double.Parse(args[2]),int.Parse(args[3]));
					for(int i=0; i<result.GetLength(1); i++)
						Console.WriteLine($"{result[0,i]} => {result[1,i]}");
					return;
				}
			}
			Console.WriteLine("Hey boss, problem:\n"+rpn.getErrorMsg());
		}else{
			RPN rpn = new RPN(args[0]);
			if(rpn.preValidate()){
				rpn.divideTokens();
				if(rpn.postValidate()){
					rpn.printInfix();
					rpn.toPostfix();
					rpn.printPostfix();
					Console.WriteLine(rpn.evaluateForX(double.Parse(args[1])));
					double[,] result = rpn.evaluateForRange(double.Parse(args[2]),double.Parse(args[3]),int.Parse(args[4]));
					for(int i=0; i<result.GetLength(1); i++)
						Console.WriteLine($"{result[0,i]} => {result[1,i]}");
					return;
				}
			}
			Console.WriteLine("Hey boss, problem:\n"+rpn.getErrorMsg());
		}
	}
}
