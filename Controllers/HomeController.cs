using BrsBlogWeb.Data;
using BrsBlogWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace BrsBlogWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BrsBlogWebContext _context;
        private readonly BrsProjectsContext _projectContext;
        private readonly IConfiguration _configuration;


        public HomeController(ILogger<HomeController> logger, BrsBlogWebContext context, BrsProjectsContext projectContext,IConfiguration configuration )
        {
            _logger = logger;
            _context = context;
            _projectContext = projectContext;
            _configuration = configuration;
            
        }
        [AllowAnonymous]
        public IActionResult İletişim()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Anasayfa()
        {
            return View(await _context.BlogPost.ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> Projeler()
        {
            return View(await _projectContext.Projects.ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> Postlar()
        {
            return View(await _context.BlogPost.ToListAsync());
        }
        [AllowAnonymous]
        [HttpGet("PostPage/{PostUrl}")]
        public async Task<IActionResult> PostPage(string PostUrl = "Post")
        {

            using (var context = _context)
            {
                
                
              
                
                

                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BrsBlogWebContext"));
                
                    if(connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                string sqlCommand = "Select id from BlogPost where '" + PostUrl + "'= post_url";
                SqlCommand command = new SqlCommand(sqlCommand, connection);
                SqlDataReader dataReader = command.ExecuteReader();
                int FoundedId = 0;
                while (dataReader.Read())
                {
                    FoundedId = dataReader.GetInt32(0);
                }
                
                var url = HttpContext.Request.GetDisplayUrl();
                string newPostUrl = url.Remove(33);

                var  user = await context.FindAsync<BlogPost>(FoundedId);
                return View(user);
            }
            


        } 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
