using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AdamURL.Models;
using AdamURL.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.IO;

namespace AdamURL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShortUrlContext _context;

        public HomeController(ShortUrlContext context)
        {
            _context = context;
        }

        //GET: /Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile theFile)
        {
            if (theFile == null)
            {
                return View();
            }
            FileUpload newFile = new FileUpload();
            //validation goes here... file extension, size, etc

            using (var reader = new StreamReader(theFile.OpenReadStream()))
            {
                while(reader.Peek() >= 0)
                {
                    newFile.listUrls.Add(reader.ReadLine());
                }
            }
            bool result = newFile.listUrls.Count > 0;
            if (!result)
            {
                ViewData["Success"] = "Upload didnt work, this is rubbish.";
                return View("Upload");
            }
            foreach (string s in newFile.listUrls)
            {
                await Submit(s);
            }
            ViewData["Success"] = "true";
            return View("Upload",newFile);
        }


        [HttpPost]
        public async Task<IActionResult> Submit(string longUrl)
        {
            string newUrl = tidyUrl(longUrl);
            if (!validateUrlInput(newUrl))
            {
                ViewData["ErrorText"] = "Invalid url " + newUrl;
                return View("UrlError");
            }

            //get existing or create
            ShortUrl urlReturn = await _context.ShortUrl
               .FirstOrDefaultAsync(m => m.longUrl == newUrl);
            if (urlReturn == null)
            { 
                urlReturn = new ShortUrl(newUrl);
                while (!keyUnique(urlReturn.key))
                {
                    urlReturn = new ShortUrl(newUrl);
                }
                if (ModelState.IsValid)
                {
                    _context.Add(urlReturn);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ViewData["ErrorText"] = "Fatal Error";
                    return View("UrlError");
                }
            }
            ViewData["UrlReturn"] = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase) + "/" + urlReturn.key;
            return View();
        }

        public async Task<IActionResult> TokenUrl(string token)
        {
            if (token.Length == 8)
            {
                ShortUrl shortUrl = await _context.ShortUrl
                .FirstOrDefaultAsync(m => m.key == token);
                if (shortUrl != null)
                {
                    Response.Redirect(shortUrl.longUrl);
                }
            } 

            //no exist
            ViewData["urlToken"] = token;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /* check the input is a real URL */
        private bool validateUrlInput(string urlIn)
        {
            Uri uriOut;
            bool result = Uri.TryCreate(urlIn, UriKind.Absolute, out uriOut)
                && (uriOut.Scheme == Uri.UriSchemeHttp || uriOut.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        /* ensure starts with http or https for consistency */
        private string tidyUrl(string rawUrl)
        {
            string tidyUrl = rawUrl.Trim();
            /*
            if (!tidyUrl.StartsWith("http"))
                tidyUrl = "http://" + tidyUrl;
            */
            return tidyUrl;
        }

        /*check unique token/key
        i know this isn't the best way - but low overhead for a demo
        */
        private bool keyUnique(string keyTest)
        {
            ShortUrl urlGet = _context.ShortUrl.FirstOrDefault(m => m.key == keyTest);
            if (urlGet == null)
                return true;
            else
                return false;
        }

    }
}
