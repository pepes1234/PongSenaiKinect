using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using backend.Model;
using backend.Services;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace backend;
[Route("User/")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> RegisterUser(Usuario user, Color[] clr)   
    {
        byte[] byteClr;
        byteClr = clr.SelectMany(c => new byte[] { c.R, c.G, c.B }).ToArray();
        try
        {
            PongGameDbContext context = new PongGameDbContext();
            user.FaceData = byteClr;
            
            await context.AddAsync(user);
            await context.SaveChangesAsync();
            return Ok("Dados salvos com sucesso");
        }
        catch
        {     
            return BadRequest("Erro no servidor");
        } 
    }
    [HttpGet("login/{id}")]
    public async Task<IActionResult> Login(Usuario user, Color[] clr1, [FromServices]UserService service)
    {
        var context =  new PongGameDbContext();
        var queryNickname = await context.Usuarios.FirstOrDefaultAsync(u => u.Nickname == user.Nickname);
        if(queryNickname == null)
            return BadRequest("Usuario inexistente.");
        if(queryNickname.FaceData == null)
            return BadRequest("Facedata nulo.");
        
        byte[] data  = queryNickname.FaceData;
        Color[] clr2 = new Color[data.Length / 3];

        for(int i = 0; i < data.Length; i+=3)
        {
            clr2[i / 3] = Color.FromArgb(data[i], data[i + 1], data[i + 2]);
        }
        
        if(service.VerifyKmeans(clr1, clr2) == false)
            return BadRequest("Faceid incorreto.");
        else
            return BadRequest("Login realizado com sucesso");
    }
}
//     [HttpGet("test")]
//     public IActionResult Test()
//     {
//         PongGameDbContext context = new PongGameDbContext();
//         var user = new Usuario();
//         int number = 1;
//         var query = context.Usuarios.FirstOrDefault(u => u.Id == number);

//         byte[] data  = query.FaceData;
//         Color[] clr = new Color[data.Length / 3];

//         for(int i = 0; i < data.Length; i+=3)
//         {
//             clr[i / 3] = Color.FromArgb(data[i], data[i + 1], data[i + 2]);
//         }
//         string result = "";
//         for(int i = 0; i < clr.Length; i++)
//         {
//             result += clr[i].ToString();
//         }
//         return Ok(result + "    completo");
//     }
// }
/*
Usuario user = new Usuario();
        Color[] clrArray = new Color[5];
        clrArray [0] = Color.AliceBlue;
        clrArray [1] = Color.Black;
        clrArray [2] = Color.White;
        clrArray [3] = Color.Yellow;
        clrArray [4] = Color.Red;
        user.Nickname = "aaa";
        if(user.Nickname == null)
            return BadRequest("Nickname obrigatório.");
        var context = new PongGameDbContext();
        var userinDb = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == user.Id);
        
            for(int i = 0; i < clrArray.Length; i++)
            {
                var rgb = new Rgb();
                rgb.UserId = user.Id;
                rgb.ArrayIndex = i + 1;
                rgb.R = clrArray[i].R;
                rgb.G = clrArray[i].G;
                rgb.B = clrArray[i].B;
                await context.AddAsync(rgb);
            }
            await context.AddAsync(user);
            await context.SaveChangesAsync();*/