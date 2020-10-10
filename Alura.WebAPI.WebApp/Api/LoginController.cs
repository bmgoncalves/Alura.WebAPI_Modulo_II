using Alura.ListaLeitura.Seguranca;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Alura.WebAPI.WebApp.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<Usuario> _signInManager;

        public LoginController(SignInManager<Usuario> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Token(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, true, true);
                if (result.Succeeded)
                {
                    //Padrão de referencia https://jwt.io/

                    //Cria token (header + payload (direitos) + signature) 
                    var direitos = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, model.Login),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() )

                    };

                    var chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("alura-webapi-authetication-valid"));
                    var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

                    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                        issuer: "Alura.WebApp",
                        audience: "Postman",
                        claims: direitos,
                        signingCredentials: credenciais,
                        expires: DateTime.Now.AddMinutes(30)
                    );

                    var tokenString = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(tokenString); //200
                }
                return Unauthorized(); //401
            }
            return BadRequest(); //400
        }
    }
}
