using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class SpaceshipServices : ISpaceshipServices
    {
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileServices;

        public SpaceshipServices(TARge25ShopContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        //see meetod on vaja controlleris esile kutsuda
        //peab lisama interface, et kutsuda see meetod välja
            public async Task<Spaceship> Create(SpaceshipDto dto)
            {
                //siin peab tegema vaheinstansi dto ja domain vahel,
                //et andmed liiguvad dto-st domain objekt
                Spaceship spaceShip = new();

                spaceShip.Id = Guid.NewGuid();
                spaceShip.Name = dto.Name;
                spaceShip.ShipType = dto.ShipType;
                spaceShip.Crew = dto.Crew;
                spaceShip.EnginePower = dto.EnginePower;
                spaceShip.CreatedAt = DateTime.Now;
                spaceShip.UpdatedAt = DateTime.Now;
            //kui uus ankeet on loodud, siis
            // toimub ka faili salvestamine
            _fileServices.FilesToApi(dto, spaceShip);

            //andmete salvestamine andmebaasi
            _context.Spaceships.Add(spaceShip);
            await _context.SaveChangesAsync();

            return spaceShip;
            }

        //teha update meetod, mis võtab vastu dto ja uuendab olemasolevat kosmoselaeva
            public async Task<Spaceship> Update(SpaceshipDto dto)
        {
            var spaceShip = await _context.Spaceships
                .SingleOrDefaultAsync(x => x.Id == dto.Id);

            if (spaceShip == null)
            {
                return null;
            }

            spaceShip.Name = dto.Name;
            spaceShip.ShipType = dto.ShipType;
            spaceShip.Crew = dto.Crew;
            spaceShip.EnginePower = dto.EnginePower;
            spaceShip.UpdatedAt = DateTime.Now;

            _context.Spaceships.Update(spaceShip);
            await _context.SaveChangesAsync();

            return spaceShip;
        }

            public async Task<Spaceship> DetailAsync(Guid id)
        {
            var spaceship = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            return spaceship;
        }

            public async Task<Spaceship> Delete(Guid Id)
        {
            var result = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == Id);

            _context.Spaceships.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }

    }
}
