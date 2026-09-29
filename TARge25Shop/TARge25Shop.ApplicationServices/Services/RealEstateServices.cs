using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstate
    {
        private readonly TARge25ShopContext _context;

        public RealEstateServices(TARge25ShopContext context)
        {
            _context = context;
        }

        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate realEstate = new();

            realEstate.Id = Guid.NewGuid();
            realEstate.Area = dto.Area;
            realEstate.Location = dto.Location;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreatedAt = DateTime.Now;
            realEstate.ModifiedAt = DateTime.Now;

            _context.RealEstate.Add(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            var realEstate = await _context.RealEstate
                .SingleOrDefaultAsync(x => x.Id == dto.Id);

            if (realEstate == null)
            {
                return null;
            }

            realEstate.Area = dto.Area;
            realEstate.Location = dto.Location;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.ModifiedAt = DateTime.Now;

            _context.RealEstate.Update(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> DetailAsync(Guid Id)
        {
            var realEstate = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == Id);

            return realEstate;
        }

        public async Task<RealEstate> Delete(Guid Id)
        {
            var result = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == Id);

            _context.RealEstate.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
