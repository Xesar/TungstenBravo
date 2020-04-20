using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace TungstenBravo.Controllers{
	[ApiController]
	[Route("api/[controller]")]
	public class calculateController:ControllerBase{
		[HttpGet]
		public IActionResult get(string formula="",double x=Double.NaN){
			try{
				if(string.IsNullOrEmpty(formula))
					throw new rpnException("formula empty");
				bool gotX = false;
				rpn r = new rpn(formula);
				r.divideTokens();
				List<string> infixTokens = r.getInfixTokens();
				foreach(string token in infixTokens){
					if(token=="x"){
						gotX=true;
						break;
					}
				}
				if(gotX && Double.IsNaN(x))
					throw new rpnException("specify x value");
				else if(!gotX)
					x=0;
				r.toPostfix();
				var data = new{
					status="ok",
					result=r.evaluateForX(x)
				};
				return Ok(data);
			}catch(rpnException e){
				var data = new{
					status="error",
					message=e.Message
				};
				return BadRequest(data);
			}catch(Exception){
				var data = new{
					status="error",
					message="Unknown error"
				};
				return StatusCode(500,data);
			}
		}
		[HttpGet("{xy}")]
		public IActionResult get(string formula="",double from=Double.NaN,double to=Double.NaN,int n=0){
			try{
				if(string.IsNullOrEmpty(formula))
					throw new rpnException("formula empty");
				if(Double.IsNaN(from))
					throw new rpnException("from parameter missing");
				if(Double.IsNaN(to))
					throw new rpnException("to parameter missing");
				if(n<1)
					throw new rpnException("n parameter missing or <1");
				bool gotX = false;
				rpn r = new rpn(formula);
				r.divideTokens();
				List<string> infixTokens = r.getInfixTokens();
				foreach(string token in infixTokens){
					if(token=="x"){
						gotX=true;
						break;
					}
				}
				if(!gotX)
					throw new rpnException("range calculation not needed, no x provided in formula");
				r.toPostfix();
				double[,] results = r.evaluateForRange(from,to,n);
				List<dynamic> resultObjects = new List<dynamic>();
				for(int i=0; i<results.GetLength(1); i++){
					resultObjects.Add(new{
						x=results[0,i],
						y=results[1,i]
					});
				}
				var data = new{
					status="ok",
					results=resultObjects.ToArray()
				};
				return Ok(data);
			}catch(rpnException e){
				var data = new{
					status="error",
					message=e.Message
				};
				return BadRequest(data);
			}catch(Exception){
				var data = new{
					status="error",
					message="Unknown error"
				};
				return StatusCode(500,data);
			}
		}
	}
}