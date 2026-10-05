using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using Xunit;

namespace TARge25Shop.SpaceshipTest
{
    public class KindergartenTest : TestBase
    {
        // Kontrollitakse, et uue lasteaia lisamisel tagastatakse objekt ja tulemus pole tühi.
        [Fact]
        public async Task Should_CreateKindergarten_WhenDataIsCorrect()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.NotNull(result);
        }

        // Kontrollitakse, et salvestatud lasteaia nimi on täpselt sama, mis sisestati.
        [Fact]
        public async Task Should_SaveCorrectKindergartenName_WhenCreated()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.KindergartenName = "Tallinna Lasteaed";

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.Equal("Tallinna Lasteaed", result.KindergartenName);
        }

        // Kontrollitakse, et salvestatud rühma nimi on täpselt sama, mis sisestati.
        [Fact]
        public async Task Should_SaveCorrectGroupName_WhenCreated()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.GroupName = "Sipsikud";

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.Equal("Sipsikud", result.GroupName);
        }

        // Kontrollitakse, et salvestatud õpetaja nimi on täpselt sama, mis sisestati.
        [Fact]
        public async Task Should_SaveCorrectTeacherName_WhenCreated()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.TeacherName = "Juhan Puu";

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.Equal("Juhan Puu", result.TeacherName);
        }

        // Kontrollitakse, et salvestatud laste arv rühmas on täpselt sama, mis sisestati.
        [Fact]
        public async Task Should_SaveCorrectChildrenCount_WhenCreated()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.ChildrenCount = 22;

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.Equal(22, result.ChildrenCount);
        }

        // Kontrollitakse, et lasteaia rühma otsimisel õige ID-ga tagastatakse andmebaasist õige lasteaed.
        [Fact]
        public async Task Should_GetKindergartenByID_WhenGuidMatches()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            var created = await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            var result = await Svc<IKindergartenServices>().DetailAsync((Guid)created.Id);

            // Kontroll
            Assert.Equal(created.Id, result.Id);
        }

        // Kontrollitakse, et vale ID-ga otsimisel ei tohi süsteem leitud lasteaeda tagastada.
        [Fact]
        public async Task ShouldNot_GetKindergartenByID_WhenGuidIsWrong()
        {
            // Ülesseade
            Guid wrongGuid = Guid.NewGuid();
            KindergartenDto dto = MockKindergartenData();
            await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            var result = await Svc<IKindergartenServices>().DetailAsync(wrongGuid);

            // Kontroll
            Assert.Null(result);
        }

        // Kontrollitakse, et andmete muutmisel salvestub uus lasteaia nimi korrektselt.
        [Fact]
        public async Task Should_UpdateKindergartenName_WhenUpdated()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            var created = await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            KindergartenDto updateDto = new KindergartenDto
            {
                Id = created.Id,
                KindergartenName = "Uus nimi",
                GroupName = created.GroupName,
                TeacherName = created.TeacherName,
                ChildrenCount = created.ChildrenCount
            };

            var result = await Svc<IKindergartenServices>().Update(updateDto);

            // Kontroll
            Assert.Equal("Uus nimi", result.KindergartenName);
        }

        // Kontrollitakse, et andmete muutmisel salvestub uus rühma nimi korrektselt.
        [Fact]
        public async Task Should_UpdateGroupName_WhenUpdated()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            var created = await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            KindergartenDto updateDto = new KindergartenDto
            {
                Id = created.Id,
                KindergartenName = created.KindergartenName,
                GroupName = "Naksitrallid",
                TeacherName = created.TeacherName,
                ChildrenCount = created.ChildrenCount
            };

            var result = await Svc<IKindergartenServices>().Update(updateDto);

            // Kontroll
            Assert.Equal("Naksitrallid", result.GroupName);
        }

        // Kontrollitakse, et andmete muutmisel salvestub uus laste arv korrektselt.
        [Fact]
        public async Task Should_UpdateChildrenCount_WhenUpdated()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            var created = await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            KindergartenDto updateDto = new KindergartenDto
            {
                Id = created.Id,
                KindergartenName = created.KindergartenName,
                GroupName = created.GroupName,
                TeacherName = created.TeacherName,
                ChildrenCount = 18
            };

            var result = await Svc<IKindergartenServices>().Update(updateDto);

            // Kontroll
            Assert.Equal(18, result.ChildrenCount);
        }

        // Kontrollitakse, et lasteaia rühma kustutamisel tagastab süsteem kustutatud objekti ID.
        [Fact]
        public async Task Should_ReturnDeletedId_WhenKindergartenIsDeleted()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            var created = await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            var deleted = await Svc<IKindergartenServices>().Delete((Guid)created.Id);

            // Kontroll
            Assert.Equal(created.Id, deleted.Id);
        }

        // Kontrollitakse, et peale kustutamist ei ole lasteaed enam vaatest leitav.
        [Fact]
        public async Task Should_RemoveKindergartenFromDatabase_WhenDeleted()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            var created = await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            await Svc<IKindergartenServices>().Delete((Guid)created.Id);
            var result = await Svc<IKindergartenServices>().DetailAsync((Guid)created.Id);

            // Kontroll
            Assert.Null(result);
        }

        // Kontrollitakse, et ilma lasteaia nimeta uuendamise katse tagastab nulli, kui objekti ei leita.
        [Fact]
        public async Task ShouldNot_UpdateKindergarten_WhenKindergartenDoesNotExist()
        {
            // Ülesseade
            KindergartenDto emptyDto = new KindergartenDto
            {
                Id = Guid.NewGuid(),
                KindergartenName = "Tundmatu"
            };

            // Tegevus
            var result = await Svc<IKindergartenServices>().Update(emptyDto);

            // Kontroll
            Assert.Null(result);
        }

        // Kontrollitakse, et negatiivse või vigase sisendiga andmete uuendamisel andmebaasi väärtused muutuvad DTO-ga samaks.
        [Fact]
        public async Task Should_UpdateToNegativeChildrenCount_WhenDtoHasNegativeValue()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            var created = await Svc<IKindergartenServices>().Create(dto);

            KindergartenDto updateDto = new KindergartenDto
            {
                Id = created.Id,
                KindergartenName = created.KindergartenName,
                GroupName = created.GroupName,
                TeacherName = created.TeacherName,
                ChildrenCount = -5
            };

            // Tegevus
            var result = await Svc<IKindergartenServices>().Update(updateDto);

            // Kontroll
            Assert.Equal(-5, result.ChildrenCount);
        }


        // Kontrollitakse, et kahe erineva rühma loomisel genereeritakse neile erinevad ID-d.
        [Fact]
        public async Task Should_GenerateDifferentGuids_WhenTwoKindergartensAreCreated()
        {
                KindergartenDto uniqueDto1 = MockKindergartenData();
                KindergartenDto uniqueDto2 = MockKindergartenData();

            var testResult1 = await Svc<IKindergartenServices>().Create(uniqueDto1);
            var testResult2 = await Svc<IKindergartenServices>().Create(uniqueDto2);

            Assert.NotEqual(testResult1.Id, testResult2.Id);
        }

        private KindergartenDto MockKindergartenData()
        {
            return new KindergartenDto()
            {
                KindergartenName = "Päikesekiir",
                GroupName = "Lepatriinud",
                TeacherName = "Mati Auto",
                ChildrenCount = 20,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
    }
}
