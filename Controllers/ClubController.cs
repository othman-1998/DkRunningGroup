using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Interfaces;
using webapp.Models;
using webapp.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly iPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController(IClubRepository clubRepository, iPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        // this is our controller
        public async Task<IActionResult> Index() 
        {
            // getting our model
            var clubs = await _clubRepository.GetAll();
            // returning the view with clubs 
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            // searching after clubs that matches the parameter id. and by saying include, we make sure we get the address. its like a join
            // because it will be a heavy call if you wanted to call the whole club
            // as the club have some forein keys to other tabels. thats why we make a join by saying include
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);

        }

        edededede

        public IActionResult Create()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel { AppUserId = currentUserId };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVm)
        {
            if(ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVm.Image);

                    var club = new Club
                    {
                        Title = clubVm.Title,
                        Description = clubVm.Description,
                        Image = result.Url.ToString(),
                        Address = new Address
                        {
                            City = clubVm.Address.City,
                            Street = clubVm.Address.Street,
                            State = clubVm.Address.State
                        }
                    };
                    _clubRepository.Add(club);
                    return RedirectToAction("Index");
            } else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVm);

        }


        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);

            if (club == null) return View("Error");

            var clubVm = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };

            return View(clubVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVm)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVm);
            }

            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {


                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVm);
                }

                var photoResult = await _photoService.AddPhotoAsync(clubVm.Image);

                var club = new Club
                {
                    Id = id,
                    Title = clubVm.Title,
                    Description = clubVm.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubVm.AddressId,
                    Address = clubVm.Address
                };

                _clubRepository.Update(club);

                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVm);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");

            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }
         
    }
}

