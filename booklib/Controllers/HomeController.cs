﻿using AutoMapper;
using booklib.Entities;
using booklib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace booklib.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public HomeController(DatabaseContext databaseContext, IConfiguration configuration, IMapper mapper)
                {
                    _databaseContext = databaseContext;
                    _configuration = configuration;
                    _mapper = mapper;
                }

        public IActionResult Index()
        {
            List<BookSearchModel> model = GetBooks();

            return View(model);
        }

        

        //public List<BookSearchModel> GetBooks()
        //{
        //    List<BookSearchModel> mod = _databaseContext.Books.ToList().Select(x => _mapper.Map<BookSearchModel>(x)).ToList();

        //    return mod;

        //}
        //

        public List<BookSearchModel> GetBooks()
        {
            List<BookSearchModel> mod = new List<BookSearchModel> 
            { 
                new BookSearchModel {BookId = Guid.NewGuid(),BookName="Onur1",Author="Onur Doğan1",BookImageFileName=""}, 
                new BookSearchModel {BookId = Guid.NewGuid(),BookName="Onur2",Author="Onur Doğan2",BookImageFileName=""}, 
                new BookSearchModel {BookId = Guid.NewGuid(),BookName="Onur3",Author="Onur Doğan3",BookImageFileName=""}, 
                new BookSearchModel {BookId = Guid.NewGuid(),BookName="Onur4",Author="Onur Doğan4",BookImageFileName=""}, 
                new BookSearchModel {BookId = Guid.NewGuid(),BookName="Onur5",Author="Onur Doğan5",BookImageFileName=""}, 
            };
            return mod;
        }
       
        public PartialViewResult SearchBooks(string searchText)
        {
            List<BookSearchModel> model = GetBooks();
            var result = model.Where(a => a.BookName.ToLower().Contains(searchText) || a.Author.ToLower().Contains(searchText)).ToList();
            return PartialView("_PartialGridView", result);
        }

        [Authorize(Roles = "user, admin, moderator")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult DetailsList(BookModel model)
        {

            List<BookModel> books=_databaseContext.Books.ToList().Select(x=>_mapper.Map<BookModel>(x)).ToList();

            return View(books);

        }
        
        public IActionResult BookDetails(Guid id, BookModel model)
        {            
            Book book = _databaseContext.Books.Find(id);
            //model.BookImageFileName = book.BookImageFileName;
            _mapper.Map(book, model);
            return View(model);
        } 
    }
}