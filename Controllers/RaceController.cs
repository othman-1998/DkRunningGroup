using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Interfaces;
using webapp.Models;
using webapp.Repository;
using webapp.Services;
using webapp.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly iPhotoService _photoService;

        public RaceController(IRaceRepository raceRepository, iPhotoService photoService)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;

        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var races = await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            // searching after races that matches the parameter id. and by saying include, we make sure we get the address. its like a join
            // because it will be a heavy call if you wanted to call the whole club
            // as the club have some forein keys to other tabels. thats why we make a join by saying include
            Race race = await _raceRepository.GetByIdAsync(id);
            return View(race);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVm)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVm.Image);

                var race = new Race
                {
                    Title = raceVm.Title,
                    Description = raceVm.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        City = raceVm.Address.City,
                        Street = raceVm.Address.Street,
                        State = raceVm.Address.State
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVm);

        }

        public async Task<IActionResult> Edit (int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);

            if (race == null) return View("Error");

            var raceVm = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };

            return View(raceVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceVm);
            }

            var userClub = await _raceRepository.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {


                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceVm);
                }

                var photoResult = await _photoService.AddPhotoAsync(raceVm.Image);

                var race = new Race
                {
                    Id = id,
                    Title = raceVm.Title,
                    Description = raceVm.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceVm.AddressId,
                    Address = raceVm.Address
                };

                _raceRepository.Update(race);

                return RedirectToAction("Index");
            }
            else
            {
                return View(raceVm);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var raceDetails = await _raceRepository.GetByIdAsync(id);
            if (raceDetails == null) return View("Error");
            return View(raceDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var raceDetails = await _raceRepository.GetByIdAsync(id);
            if (raceDetails == null) return View("Error");

            _raceRepository.Delete(raceDetails);
            return RedirectToAction("Index");
        }
    }

 }


