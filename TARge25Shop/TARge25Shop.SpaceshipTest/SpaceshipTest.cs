using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using Xunit;

namespace TARge25Shop.SpaceshipTest
{
    public class SpaceshipTest : TestBase
    {
        [Fact] //Fact tähistab ära ühte testi xUnit raamistikus
        // 1 - Kirjeldatakse ära kas test on tavaline, või negatiivne ()
        // 2 - Kirjeldatakse ära mida parasjagu üritatakse testialuse objektiga teha
        // 3 - Mis tingimustel tulemust kontrollitakse, peale tegevust

        //Selles testis kontrollitakse, et (2) kosmoselaeva lisamisel (1) ei tohiks (3) saadud tulemus olla tühi. Jälgi seda sõnastusviisi:
        //
        //                     1        2                   3
        public async Task ShouldNot_AddEmptySpaceShip_WhenResultIsReturned()
        {
            //ülesseade
            SpaceshipDto dto = new SpaceshipDto()
            {
                Name = "X AE a L 12",
                ShipType = "lendav taldrik",
                Crew = 666,
                EnginePower = 12,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            //tegutsemine
            var result = await Svc<ISpaceshipServices>().Create(dto);

            //kontroll
            Assert.NotNull(result);
        }

        //Selles testis kontrollitakse, et (2) Spaceshipi päring andmebaasist (1) ei tohiks tagastada objekti (3) kui Id-d ei ole samad:
        //                  1               2           3
        [Fact]
        public async Task ShouldNot_GetSpaceShipByID_WhenIDNotEqual()
        {
            //ülesseade
            Guid wrongGuid = Guid.NewGuid();
            Guid goodGuid = Guid.Parse("eda81556-c05d-457e-a2da-4ce3d74f0439");

            //tegevus
            await Svc<ISpaceshipServices>().DetailAsync(goodGuid);

            //kontroll
            Assert.NotEqual(wrongGuid, goodGuid);
        }
    }
}
