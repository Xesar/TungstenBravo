using System;

class Program{
	static void Main(string[] args){
        // RPN rpn = new RPN("x");
        RPN rpn = new RPN("-x2+(-2)+c");
        // RPN rpn = new RPN("sin(x/2)");
        rpn.printFormula();
        if(rpn.preValidate()){
            rpn.divideTokens();
            if(rpn.postValidate()){
                rpn.printInfix();
                rpn.toPostfix();
                rpn.printPostfix();
                Console.WriteLine(rpn.evaluateForX(1));
            }
            // double[,] results = rpn.evaluateForRange(2,6,5);
            // for(int i=0; i<results.GetLength(1);i++){
            //     Console.WriteLine(results[0,i]+":\t"+results[1,i]);
            // }
        }else{
            Console.WriteLine("Hey boss, problem:");
            Console.WriteLine(rpn.getErrorMsg().ToLower()+"");
        }
	}
}
