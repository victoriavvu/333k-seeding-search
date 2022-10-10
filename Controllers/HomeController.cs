using Microsoft.AspNetCore.Mvc;
using Vu_Victoria_HW3.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Vu_Victoria_HW3.DAL;
using System.Linq;

namespace Vu_Victoria_HW3.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;
        public HomeController(AppDbContext dbContext)
        {
            _context = dbContext;
        }
        // GET: Home
        public IActionResult Index(string SearchString)
        {
            var query = from b in _context.Movies
                        select b;

            List<Movie> SelectedMovies = query.Include(b => b.Genre).ToList();

            if (SearchString != null)
            {
                ViewBag.SelectedMovieCount = SelectedMovies.Count();
                ViewBag.AllMovieCount = _context.Movies.Count();

                query = query.Where(b => b.Title.Contains(SearchString) ||
                b.Description.Contains(SearchString));
            }

            ViewBag.SelectedMovieCount = SelectedMovies.Count();
            ViewBag.AllMovieCount = _context.Movies.Count();

            return View(SelectedMovies.OrderBy(s => s.Title));
        }
        public IActionResult Details(int? id)//id is the id of the movie you want to see
        {
            if (id == null) 
            {
                return View("Error", new String[] { "MovieID not specified - which movie doyou want to view?" });
            }

            Movie movie = _context.Movies.Include(b => b.Genre)
            .FirstOrDefault(b => b.MovieID == id);
            if (movie == null) 
            {
                return View("Error", new String[] { "Movie not found in database" });
            }
            return View(movie);
        }
        private SelectList GetAllGenres()
        {
            List<Genre> genreList = _context.Genres.ToList();

            Genre AllGenres = new Genre() { GenreID = 0, GenreName = "All Genres" };
            genreList.Add(AllGenres);

            SelectList genreSelectList = new SelectList(genreList.OrderBy(m => m.GenreID), "GenreID", "GenreName");

            return genreSelectList;
        }
        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();

            SearchViewModel svm = new SearchViewModel();
            svm.GenreID = 0;

            return View(svm);
        }
        public IActionResult DisplaySearchResults(SearchViewModel svm)
        {
            TryValidateModel(svm);
            if (ModelState.IsValid == false)
            {
                return View("DetailedSearch", svm);
            }


            var query = from m in _context.Movies select m;

            if (svm.SearchTitle != null && svm.SearchTitle != "")
            {
                query = query.Where(m => m.Title.Contains(svm.SearchTitle) || m.Description.Contains(svm.SearchDescription));
            }

            if (svm.SelectedRating != null)
            {
                query = query.Where(m => m.Rating == svm.SelectedRating);
            }

            if (svm.GenreID != 0)
            {
                query = query.Where(m => m.Genre.GenreID == svm.GenreID);
            }

            if (svm.SelectedVoteAverage != null)
            {
                if (svm.SelectedVote == VoteSearch.GreaterThan)
                {
                    query = query.Where(m => m.VoteAverage >= svm.SelectedVoteAverage);
                }
                else
                {
                    query = query.Where(m => m.VoteAverage <= svm.SelectedVoteAverage);
                }
            }

            if (svm.SelectedDate != null)
            {
                query = query.Where(m => m.ReleaseDate >= svm.SelectedDate);
            }

            List<Movie> SelectedMovies = query.Include(m => m.Genre).ToList();

            ViewBag.SelectedMovieCount = SelectedMovies.Count();
            ViewBag.AllMovieCount = _context.Movies.Count();

            return View("Index", SelectedMovies.OrderByDescending(m => m.VoteAverage));
        }
    }
}



