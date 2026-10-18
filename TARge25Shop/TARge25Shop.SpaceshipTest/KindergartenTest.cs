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
        [Fact]
        // Lasteaia lisamisel ei tohi tulemus olla tühi
        public async Task ShouldNot_AddEmptyKindergarten_WhenResultIsReturned()
        {
            // ülesseade
            var dto = MockKindergartenData();

            // tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // kontroll
            Assert.NotNull(result);
        }

        [Fact]
        // Vale ID-ga ei tohi andmebaasist lasteaeda leida
        public async Task ShouldNot_GetKindergartenByID_WhenIDNotEqual()
        {
            // ülesseade
            await Svc<IKindergartenServices>().Create(MockKindergartenData());
            Guid wrongGuid = Guid.NewGuid();

            // tegevus
            var result = await Svc<IKindergartenServices>().DetailAsync(wrongGuid);

            // kontroll
            Assert.Null(result);
        }

        [Fact]
        // Õige ID-ga peab andmebaasist lasteaed tagastuma
        public async Task Should_GetKindergartenByID_WhenGuidIsEqual()
        {
            // ülesseade
            var created = await Svc<IKindergartenServices>().Create(MockKindergartenData());

            // tegevus
            var result = await Svc<IKindergartenServices>().DetailAsync((Guid)created.Id);

            // kontroll
            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
        }

        [Fact]
        // Kustutamisel peab tagastatud lasteaed olema sama, mis lisati
        public async Task Should_KindergartenDeletedByID_WhenReturnedResultIsEqual()
        {
            // ülesseade
            var created = await Svc<IKindergartenServices>().Create(MockKindergartenData());

            // tegevus
            var deleted = await Svc<IKindergartenServices>().Delete((Guid)created.Id);

            // kontroll
            Assert.Equal(created.Id, deleted.Id);
        }

        [Fact]
        // Kui kustutatakse teine lasteaed, peab esimene alles jääma
        public async Task ShouldNot_DeleteKindergartenByID_WhenDidNotDeleteKindergarten()
        {
            // ülesseade
            var dto = MockKindergartenData();
            var first = await Svc<IKindergartenServices>().Create(dto);
            var second = await Svc<IKindergartenServices>().Create(dto);

            // tegevus
            var deleted = await Svc<IKindergartenServices>().Delete((Guid)second.Id);

            // kontroll
            Assert.NotEqual(first.Id, deleted.Id);
            Assert.NotNull(await Svc<IKindergartenServices>().DetailAsync((Guid)first.Id));
        }

        [Fact]
        // Uute andmetega uuendamisel peavad andmed muutuma, CreatedAt aga mitte
        public async Task Should_UpdateKindergartenByID_WhenUpdatingData()
        {
            // ülesseade
            var created = await Svc<IKindergartenServices>().Create(MockKindergartenData());

            var update = new KindergartenDto
            {
                Id = created.Id,
                GroupName = "Lalala",
                ChildrenCount = 20,
                KindergartenName = "Kokoko",
                TeacherName = "Mart",
                CreatedAt = created.CreatedAt,
                UpdatedAt = DateTime.Now
            };

            // tegevus
            await Svc<IKindergartenServices>().Update(update);
            var result = await Svc<IKindergartenServices>().DetailAsync((Guid)created.Id);

            // kontroll
            Assert.Equal(update.GroupName, result.GroupName);
            Assert.Equal(update.ChildrenCount, result.ChildrenCount);
            Assert.Equal(update.KindergartenName, result.KindergartenName);
            Assert.Equal(update.TeacherName, result.TeacherName);
            Assert.Equal(created.CreatedAt, result.CreatedAt);
        }

        [Fact]
        // Ilma ID-ta uuendamisel ei tohi tulemuseks olla lasteaed
        public async Task ShouldNot_UpdateKindergartenByID_WhenNoDataIsUpdated()
        {
            // tegevus
            var result = await Svc<IKindergartenServices>().Update(MockKindergartenNullData());

            // kontroll
            Assert.Null(result);
        }

        [Fact]
        // Laste arv ei tohi olla negatiivne
        public async Task ShouldNot_CreateKindergartenWithNegativeChildrenCount_WhenChildrenCountNegative()
        {
            // ülesseade
            var dto = MockKindergartenData();
            dto.ChildrenCount = -5;

            // tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // kontroll
            Assert.True(result.ChildrenCount >= 0);
        }

        [Fact]
        // Kustutatud lasteaed ei tohi andmebaasist enam leiduda
        public async Task Should_RemoveKindergartenFromDatabase_WhenKindergartenIsDeleted()
        {
            // ülesseade
            var created = await Svc<IKindergartenServices>().Create(MockKindergartenData());

            // tegevus
            var deleted = await Svc<IKindergartenServices>().Delete((Guid)created.Id);
            var result = await Svc<IKindergartenServices>().DetailAsync((Guid)created.Id);

            // kontroll
            Assert.Equal(created.Id, deleted.Id);
            Assert.Null(result);
        }

        [Fact]
        // Teise lasteaia kustutamine ei tohi esimest andmebaasist eemaldada
        public async Task ShouldNot_RemoveKindergartenFromDatabase_WhenKindergartenIdIsDifferent()
        {
            // ülesseade
            var dto = MockKindergartenData();
            var first = await Svc<IKindergartenServices>().Create(dto);
            var second = await Svc<IKindergartenServices>().Create(dto);

            // tegevus
            var deleteResult = await Svc<IKindergartenServices>().Delete((Guid)second.Id);
            var inDb = await Svc<IKindergartenServices>().DetailAsync((Guid)first.Id);

            // kontroll
            Assert.NotNull(inDb);
            Assert.NotEqual(deleteResult.Id, first.Id);
            Assert.Equal(first.Id, inDb.Id);
        }



        // 15 oma tehtud testi
        // 01
        // Kontrollib, et lasteaiale tekib loomisel ID.
        [Fact]
        public async Task Should_GenerateId_WhenCreated()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.NotNull(result.Id);
            Assert.NotEqual(Guid.Empty, result.Id.Value);
        }

        // 02
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

        // 03
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

        // 04
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

        // 05
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

        // 06
        // Kontrollib, et loomisel pannakse kirja loomise aeg.
        [Fact]
        public async Task Should_SetCreatedAt_WhenCreated()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.NotEqual(DateTime.MinValue, result.CreatedAt);
        }

        // 07
        // Kontrollib otsimist tühja ID-ga.
        [Fact]
        public async Task ShouldNot_GetKindergartenByID_WhenGuidIsEmpty()
        {
            // Ülesseade
            await Svc<IKindergartenServices>().Create(MockKindergartenData());

            // Tegevus
            var result = await Svc<IKindergartenServices>().DetailAsync(Guid.Empty);

            // Kontroll
            Assert.Null(result);
        }

        // 08
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

        // 09
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
                GroupName = "Gadadada",
                TeacherName = created.TeacherName,
                ChildrenCount = created.ChildrenCount
            };

            var result = await Svc<IKindergartenServices>().Update(updateDto);

            // Kontroll
            Assert.Equal("Gadadada", result.GroupName);
        }

        // 10
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

        // 11
        // Kontrollib õpetaja nime muutmist.
        [Fact]
        public async Task Should_UpdateTeacherName_WhenUpdated()
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
                TeacherName = "Mart",
                ChildrenCount = created.ChildrenCount
            };

            var result = await Svc<IKindergartenServices>().Update(updateDto);

            // Kontroll
            Assert.Equal("Mart", result.TeacherName);
        }

        // 12
        // Kontrollib, et uuendamisel ID ei muutu.
        [Fact]
        public async Task Should_KeepSameId_WhenUpdated()
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
            Assert.Equal(created.Id, result.Id);
        }

        // 13
        // Kontrollib laste arvu 0 salvestamist.
        [Fact]
        public async Task Should_SaveZeroChildren_WhenChildrenCountIsZero()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.ChildrenCount = 0;

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.Equal(0, result.ChildrenCount);
        }

        // 14
        // Kontrollib, et negatiivset laste arvu uuendamisel ei salvestata.
        [Fact]
        public async Task ShouldNot_UpdateToNegativeChildrenCount_WhenDtoHasNegativeValue()
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
            Assert.True(result.ChildrenCount >= 0);
        }

        // 15
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


        // Abimeetodid
        private KindergartenDto MockKindergartenNullData()
        {
            return new KindergartenDto
            {
                Id = null,
                GroupName = "",
                ChildrenCount = 0,
                KindergartenName = "",
                TeacherName = "",
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue
            };
        }

        private KindergartenDto MockKindergartenData()
        {
            return new KindergartenDto
            {
                GroupName = "Gsdgsdg",
                ChildrenCount = 18,
                KindergartenName = "Lasteaed",
                TeacherName = "Pets",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
    }
}