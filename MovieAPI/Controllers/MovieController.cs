using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Repositories;
using MovieAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMongoRepository<Movie> _movierepository;
        private readonly ISyncService<Movie> _movieSyncService;

        public MovieController(IMongoRepository<Movie> movierepository,
            ISyncService<Movie> movieSyncService)
        {
            _movierepository = movierepository;
            _movieSyncService = movieSyncService;
        }

     
        [HttpGet]

        public List<Movie> GetAllMovies()
        {
            var records = _movierepository.GetAllRecords();

            return records;

        }
        [HttpGet("{id}")]
        public Movie GetMovieById(Guid id)
        {
            var result = _movierepository.GetRecordById(id);

            return result;
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            movie.LastChangedAt = DateTime.UtcNow;

           var result= _movierepository.InsertRecord(movie);

            _movieSyncService.Upsert(movie);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Upsert(Movie movie)
        {
            if(movie.Id==Guid.Empty)
            {
                return BadRequest("Empty Id");
 
            }

            movie.LastChangedAt = DateTime.UtcNow;
            _movierepository.DeleteRecord(movie);

            _movieSyncService.Upsert(movie);

            return Ok(movie);
        }
        [HttpPut("sync")]

        public IActionResult UpsertSync(Movie movie)
        {
            var existingMovie = _movierepository.GetRecordById(movie.Id);

            if(existingMovie== null || movie.LastChangedAt >existingMovie.LastChangedAt)
            {
                _movierepository.DeleteRecord(movie);

            }
            return Ok();
        }

        [HttpDelete("sync")]

        public IActionResult DeleteSync(Movie movie)
        {
            var existingMovie = _movierepository.GetRecordById(movie.Id);

            if (existingMovie == null || movie.LastChangedAt > existingMovie.LastChangedAt)
            {
                _movierepository.DeleteRecord(movie.Id);

            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var movie = _movierepository.GetRecordById(id);

            if (movie==null)
            {
                return BadRequest("Movie does not exist");
            }

            _movierepository.DeleteRecord(id);
            movie.LastChangedAt = DateTime.UtcNow;
            _movieSyncService.Delete(movie);

            return Ok("Deleted "+id);
        }
    }
}