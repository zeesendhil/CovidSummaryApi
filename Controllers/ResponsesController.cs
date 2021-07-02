using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CovidSummaryApi.Model;

namespace CovidSummaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResponsesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Responses
        [HttpGet("GetResponse")]
        public IActionResult GetResponse()
        {
            IQueryable<Response> IQhistory = _context.Response
              .Include(x => x.cases)
              .Include(x => x.deaths)
              .Include(x => x.tests);
            return new JsonResult(IQhistory.ToList());
        }

        // GET: api/Responses/5
        [HttpGet("{id}")]

        public IActionResult GetResponse(Guid id)
        {
            IQueryable<Response> IQhistory = _context.Response
             .Include(x => x.cases)
             .Include(x => x.deaths)
             .Include(x => x.tests)
             .Where(u => u.ID == id);

            Response history = IQhistory.FirstOrDefault<Response>();

            if (history == null)
            {
                return NotFound();
            }
            return new JsonResult(history);
        }

        // PUT: api/Responses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResponse(Guid id, Response response)
        {
            if (id != response.ID)
            {
                return BadRequest();
            }

            _context.Entry(response).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Responses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Response>> PostResponse(Response response)
        {
            _context.Response.Add(response);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResponse", new { id = response.ID }, response);
        }

        // DELETE: api/Responses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> DeleteResponse(Guid id)
        {
            var response = await _context.Response.FindAsync(id);
            if (response == null)
            {
                return NotFound();
            }

            _context.Response.Remove(response);
            await _context.SaveChangesAsync();

            return response;
        }


        private bool ResponseExists(Guid id)
        {
            return _context.Response.Any(e => e.ID == id);
        }

        // DELETE: api/Summary/5
        [HttpDelete("DeleteSummary/{id}")]
        public async Task<ActionResult<Summary>> DeleteSummary(Guid id)
        {
            var Summary = await _context.Summary.FindAsync(id);
            if (Summary == null)
            {
                return NotFound();
            }

            _context.Summary.Remove((Summary)Summary);
            await _context.SaveChangesAsync();

            return (Summary)Summary;
        }


        // GET: api/Responses/5
        [HttpGet("GetSummary")]
        public IActionResult GetSummary()
        {
            IQueryable<Summary> IQSummary = _context.Summary
              .Include(u => u.response)
                .ThenInclude(x => x.cases)
              .Include(u => u.response)
                .ThenInclude(x => x.deaths)
              .Include(u => u.response)
                .ThenInclude(x => x.tests);
            return new JsonResult(IQSummary.ToList());
        }


        [HttpGet("Fetchhistory")]
        public async Task<IActionResult> FetchHistory()
        {

            using (var client = new HttpClient())
            {
                // build request
                client.BaseAddress = new Uri("https://covid-193.p.rapidapi.com");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("x-rapidapi-host", "covid-193.p.rapidapi.com");
                client.DefaultRequestHeaders.Add("x-rapidapi-key", "fce34ab26bmsh94eb74f58360794p11adeejsna38c4b96ac3a");
                var response = await client.GetAsync($"/history?country=uae");
                response.EnsureSuccessStatusCode();

                var historydetails = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                // convert request to list of Photosfd
                Summary actionRes = JsonConvert.DeserializeObject<Summary>(historydetails, settings);
                if (actionRes != null && actionRes.errors != null)
                {
                    foreach (Response searchResult in actionRes.response)
                    {
                        if (!ResponseExists(searchResult.ID))
                        {
                            _context.Add(searchResult);
                        }
                        else
                        {
                            _context.Update(searchResult);
                        }

                        // since id is being set by api call need to toggle identity on before saving
                        _context.Database.OpenConnection();

                        try
                        {
                            _context.SaveChanges();
                        }
                        finally
                        {
                            _context.Database.CloseConnection();
                        }

                        await _context.SaveChangesAsync();
                    }
                }

                return Redirect("/response");
            }
        }

        [HttpGet("FetchSummary")]
        public async Task<IActionResult> FetchSummary()
        {

            using (var client = new HttpClient())
            {
                // build request
                client.BaseAddress = new Uri("https://covid-193.p.rapidapi.com");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("x-rapidapi-host", "covid-193.p.rapidapi.com");
                client.DefaultRequestHeaders.Add("x-rapidapi-key", "fce34ab26bmsh94eb74f58360794p11adeejsna38c4b96ac3a");
                var response = await client.GetAsync($"/statistics?country=uae");
                response.EnsureSuccessStatusCode();

                var historydetails = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                // convert request to list of Photosfd
                Summary actionRes = JsonConvert.DeserializeObject<Summary>(historydetails, settings);
                if (actionRes != null && actionRes.errors != null)
                {
                    Summary searchResult = actionRes;
                    if (!ResponseExists(searchResult.ID))
                    {
                        _context.Add(searchResult);
                    }
                    else
                    {
                        _context.Update(searchResult);
                    }

                    // since id is being set by api call need to toggle identity on before saving
                    _context.Database.OpenConnection();

                    try
                    {
                        _context.SaveChanges();
                    }
                    finally
                    {
                        _context.Database.CloseConnection();
                    }

                    await _context.SaveChangesAsync();

                }

                return Redirect("/response");
            }
        }
    }
}