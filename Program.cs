using System;

class Program{
	static void Main(string[] args){
        // RPN rpn = new RPN("3,000*abs(x+x^4.5)*5");
        // RPN rpn = new RPN("sin(x^2+6/x)");
        RPN rpn = new RPN("abs(2)");
        rpn.printFormula();
        if(rpn.validate()){
            rpn.divideTokens();
            rpn.printInfix();
            rpn.toPostfix();
            rpn.printPostfix();
        }else{
            Console.WriteLine("Whoopsie doopsie! Problem Boss!");
            Console.WriteLine(rpn.getErrorMsg()+"!");
        }
	}
}
