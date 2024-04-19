using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SeminarHub.Data;
using SeminarHub.Models;
using System.Globalization;
using System.Security.Claims;
using static SeminarHub.Data.DataRequirments;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {
        private readonly SeminarHubDbContext data;

        public SeminarController(SeminarHubDbContext data)
        {
            this.data = data;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new SeminarFormViewModel();
            model.Categories = await GetCategories();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(SeminarFormViewModel model)
        {
            DateTime dateandtime = DateTime.Now;
            if (!DateTime.TryParseExact(
               model.DateAndTime,
               DateAndTimeFormat,
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dateandtime))
            {
                ModelState
                    .AddModelError(nameof(model.DateAndTime), $"Invalid date and/or time! Format must be: {DateAndTimeFormat}");
            }
            if (int.Parse(model.Duration) > 180 || int.Parse(model.Duration) < 30) 
            {
                ModelState
                    .AddModelError(nameof(model.Duration), $"Invalid duration. Duration must be between {DurationMin} and {DurationMax} minutes.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();

                return View(model);
            }

            var entity = new Seminar()
            {
                Topic = model.Topic,
                Lecturer = model.Lecturer,
                Details = model.Details,
                OrganizerId = GetUserId(),
                CategoryId = model.CategoryId,
                Duration = int.Parse(model.Duration),
                DateAndTime = dateandtime,
            };

            await data.Seminars.AddAsync(entity);
            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));

        }
        public async Task<IActionResult> All()
        {
            var models = await data.Seminars
                .AsNoTracking()
                .Select(s => new SeminarInfoViewModel(
                s.Id,
                s.Topic,
                s.Lecturer,
                s.Category.Name,
                s.Organizer.UserName,
                s.DateAndTime
                ))
                .ToListAsync();
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            var model = await data.SeminaarParticipants
                .Where(sp => sp.ParticipantId == userId)
                .AsNoTracking()
                .Select(sp => new SeminarInfoViewModel(
                    sp.SeminarId,
                    sp.Seminar.Topic,
                    sp.Seminar.Lecturer,
                    sp.Seminar.Category.Name,
                    sp.Seminar.Organizer.UserName,
                    sp.Seminar.DateAndTime
                    ))
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var s = await data.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarParticipants)
                .FirstOrDefaultAsync();

            if (s == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (!s.SeminarParticipants.Any(s => s.ParticipantId == userId))
            {
                s.SeminarParticipants.Add(new SeminarParticipant()
                {
                    SeminarId = s.Id,
                    ParticipantId = userId
                });

                await data.SaveChangesAsync();
                return RedirectToAction(nameof(Joined));
            }
            else
            {
                return RedirectToAction(nameof(All));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var s = await data.Seminars
                .FindAsync(id);

            if (s == null)
            {
                return BadRequest();
            }

            if (s.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }

            var model = new SeminarFormViewModel()
            {
                Topic = s.Topic,
                Lecturer = s.Lecturer,
                Details = s.Details,
                CategoryId = s.CategoryId,
                Duration = s.Duration.ToString(),
                DateAndTime = s.DateAndTime.ToString(DateAndTimeFormat),
            };

            model.Categories = await GetCategories();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeminarFormViewModel model, int id)
        {
            var s = await data.Seminars
                .FindAsync(id);

            if (s == null)
            {
                return BadRequest();
            }

            if (s.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }

            DateTime dateandtime = DateTime.Now;
            if (!DateTime.TryParseExact(
               model.DateAndTime,
               DateAndTimeFormat,
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dateandtime))
            {
                ModelState
                    .AddModelError(nameof(model.DateAndTime), $"Invalid date and/or time! Format must be: {DateAndTimeFormat}");
            }
            if (int.Parse(model.Duration) > 180 || int.Parse(model.Duration) < 30)
            {
                ModelState
                    .AddModelError(nameof(model.Duration), $"Invalid duration. Duration must be between {DurationMin} and {DurationMax} minutes.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();

                return View(model);
            }

            s.Topic = model.Topic;
            s.Lecturer = model.Lecturer;
            s.Details = model.Details;
            s.OrganizerId = GetUserId();
            s.CategoryId = model.CategoryId;
            s.Duration = int.Parse(model.Duration);
            s.DateAndTime = dateandtime;        

            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Leave(int id)
        {
            var s = await data.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarParticipants)
                .FirstOrDefaultAsync();

            if (s == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            var sp = s.SeminarParticipants
                .FirstOrDefault(sp => sp.ParticipantId == userId);

            if (sp == null)
            {
                return BadRequest();
            }

            s.SeminarParticipants.Remove(sp);

            await data.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }

        public async Task<IActionResult> Details(int id)
        {
            var model  = await data.Seminars
                .Where(s => s.Id == id)
                .AsNoTracking()
                .Select(s => new SeminarDetailsViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    DateAndTime = s.DateAndTime.ToString(DateAndTimeFormat),
                    Duration = s.Duration.ToString(),
                    Lecturer = s.Lecturer,
                    Category = s.Category.Name,
                    Details = s.Details,
                    Organizer = s.Organizer.UserName
                })
                .FirstOrDefaultAsync();
           

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await data.Seminars
                .Where(s => s.Id == id)
                .AsNoTracking()
                .Select(s => new SeminarDeleteViewModel()
                {   
                    Id = s.Id,
                    Topic = s.Topic,
                    DateAndTime = s.DateAndTime.ToString(DateAndTimeFormat),
                })
                .FirstOrDefaultAsync();

            return View(model);
        }

        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var s = await data.Seminars.Where(s => s.Id == id).AsNoTracking().FirstOrDefaultAsync();

            var sp = await data.SeminaarParticipants
                .Where(sp => s.Id == id)
                .AsNoTracking()
                .ToListAsync();
               

            if (sp != null)
            {
                foreach (var Sp in sp)
                {
                    data.SeminaarParticipants.Remove(Sp);
                }
            }
            if (s != null) 
            {
                data.Seminars.Remove(s);
            }
            await data.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
        private async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            return await data.Category
                .AsNoTracking()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
