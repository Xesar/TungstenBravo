using System;

class Program{
	static void Main(string[] args){
        RPN rpn = new RPN("3,0*x+x^4.5*5");
        rpn.printFormula();
        rpn.printInfix();
        rpn.printPostfix();
	}
}
