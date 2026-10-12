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
        // Kontrollib lasteaia loomist.
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

        // Kontrollib lasteaia nime salvestamist.
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

        // Kontrollib rühma nime salvestamist.
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

        // Kontrollib õpetaja nime salvestamist.
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

        // Kontrollib laste arvu salvestamist.
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

        // Kontrollib lasteaia otsimist ID järgi.
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

        // Kontrollib otsimist vale ID-ga.
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

        // Kontrollib lasteaia nime muutmist.
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

        // Kontrollib rühma nime muutmist.
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

        // Kontrollib laste arvu muutmist.
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

        // Kontrollib kustutatud lasteaia ID-d.
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

        // Kontrollib, et kustutatud lasteaeda enam ei leita.
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

        // Kontrollib uuendamist, kui lasteaeda pole olemas.
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

        // Kontrollib negatiivse laste arvu salvestamist.
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

        // Kontrollib, et lasteaedadel on erinevad ID-d.
        [Fact]
        public async Task Should_GenerateDifferentGuids_WhenTwoKindergartensAreCreated()
        {
            // Ülesseade
            KindergartenDto uniqueDto1 = MockKindergartenData();
            KindergartenDto uniqueDto2 = MockKindergartenData();

            // Tegevus
            var testResult1 = await Svc<IKindergartenServices>().Create(uniqueDto1);
            var testResult2 = await Svc<IKindergartenServices>().Create(uniqueDto2);

            // Kontroll
            Assert.NotEqual(testResult1.Id, testResult2.Id);
        }

        // Tagastab lasteaia testandmed.
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