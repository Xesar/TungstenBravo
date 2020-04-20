using System;
using Microsoft.AspNetCore.Mvc;

namespace TungstenBravo.Controllers{
	[ApiController]
	[Route("api/[controller]")]
	public class tokensController:ControllerBase{
		[HttpGet]
		public IActionResult get(string formula=""){
			if(formula=="coffee"){
				var data = new{
					status="I'm a teapot"
				};
				return StatusCode(418,data);
			}
			try{
				if(string.IsNullOrEmpty(formula))
					throw new rpnException("formula empty");
				rpn r = new rpn(formula);
				var data = new{
					status="ok",
					result=new{
						infix=r.getInfixTokens().ToArray(),
						rpn=r.getPostfixTokens().ToArray()
					}
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
					message="unknown error"
				};
				return StatusCode(500,data);
			}
		}
	}
}