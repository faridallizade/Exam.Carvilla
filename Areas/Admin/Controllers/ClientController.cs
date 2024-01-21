using CarVilla.Areas.ViewModels;
using CarVilla.DAL;
using CarVilla.Helpers;
using CarVilla.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Drawing.Printing;

namespace CarVilla.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ClientController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async  Task<IActionResult> Index()
        {
            List<Client> client = await _context.clients.ToListAsync();
            return View(client);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateClientVm vm)
        {
            if(!ModelState.IsValid) return View();
            if (!vm.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Only Image file less than 3 Mb is accepted.");
            }
            vm.ImageUrl = vm.ImageFile.Upload(_env.WebRootPath, @"/Upload/");
            Client client = new Client()
            {
                Name = vm.Name,
                Description = vm.Description,
                CityLocation = vm.CityLocation,
                ImageUrl = vm.ImageUrl,
            };
            await _context.clients.AddAsync(client);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Client");
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(int id)
        {
            Client client = await _context.clients.Where(c=>c.Id == id).FirstOrDefaultAsync();
            if (client == null) { return View(); }
            if(!ModelState.IsValid) return View ();
            UpdateClientVm vm = new UpdateClientVm()
            {
                Name = client.Name,
                Description = client.Description,
                CityLocation = client.CityLocation,
                ImageUrl = client.ImageUrl,
            };
            return View(vm);   
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(UpdateClientVm vm)
        {
            if (!ModelState.IsValid) return View();
            if (!vm.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Only Image file less than 3 Mb is accepted.");
            }
            Client exist = await _context.clients.Where(c=>c.Id == vm.Id).FirstOrDefaultAsync();
            if(exist == null) { return NotFound(); }
            vm.ImageUrl = vm.ImageFile.Upload(_env.WebRootPath, @"/Upload/");
            exist.Name = vm.Name;
            exist.Description = vm.Description;
            exist.CityLocation = vm.CityLocation;
            if(vm.ImageUrl != null)
            {
                exist.ImageUrl = vm.ImageUrl;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Client");
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {
            Client client = await _context.clients.Where(c => c.Id == id).FirstOrDefaultAsync();
            _context.clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Client");
        }
    }
}
