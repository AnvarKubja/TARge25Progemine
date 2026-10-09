using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Models.Kindergarten;
using TARge25Shop.Core.Dto;
using Microsoft.EntityFrameworkCore;

namespace TARge25Shop.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly IKindergartenServices _kindergartenServices;
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileServices;

        public KindergartenController(IKindergartenServices kindergartenServices, TARge25ShopContext context, IFileServices fileServices)
            {
                _kindergartenServices = kindergartenServices;
                _context = context;
                _fileServices = fileServices;
            }

        public IActionResult Index()
        {
            var result = _context.Kindergartens.Select(x => new KindergartenIndexViewModel
            {
                Id = x.Id,
                GroupName = x.GroupName,
                ChildrenCount = x.ChildrenCount,
                KindergartenName = x.KindergartenName,
                TeacherName = x.TeacherName,
                CreatedAt = x.CreatedAt,
            });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            KindergartenCreateUpdateViewModel result = new();
            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        KindergartenId = x.KindergartenId
                    }).ToArray()
            };

            await _kindergartenServices.Create(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            KindergartenImageViewModel[] images = await FileFromDatabase(id);

            var vm = new KindergartenCreateUpdateViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;
            vm.Image.AddRange(images);
            
            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        KindergartenId = x.KindergartenId
                    }).ToArray()
            };

            var result = await _kindergartenServices.Update(dto);

            var kindergartenId = result.Id;

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Details), new { id = kindergartenId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            KindergartenImageViewModel[] images = await FileFromDatabase(id);

            var vm = new KindergartenDeleteViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.Image.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var kindergarten = await _kindergartenServices.Delete(id);

            if (kindergarten == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            KindergartenImageViewModel[] images = await FileFromDatabase(id);

            var vm = new KindergartenDetailViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;
            vm.Images.AddRange(images);
            
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(KindergartenImageViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.ImageId
            };

            var image = await _fileServices.RemoveImageFromDatabase(dto);

            var kindergartenId = image.KindergartenId;

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Update), new { id = kindergartenId });
        }

        private async Task<KindergartenImageViewModel[]> FileFromDatabase(Guid Id)
        {
            var data = await _context.FileToDatabases
                .Where(x => x.KindergartenId == Id)
                .ToArrayAsync();

            return data.Select(y => new KindergartenImageViewModel
            {
                ImageId = y.Id,
                ImageTitle = y.ImageTitle,
                ImageData = y.ImageData,
                KindergartenId = y.KindergartenId,
                Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(y.ImageData))
            }).ToArray();
        }
    }   
}
