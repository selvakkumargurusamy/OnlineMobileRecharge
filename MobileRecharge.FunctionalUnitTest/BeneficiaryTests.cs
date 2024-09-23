using FluentAssertions;
using MobileRecharge.Domain.Dtos;
using Newtonsoft.Json;
using System.Net;

namespace MobileRecharge.FunctionalUnitTest
{
    public class BeneficiaryTests : BaseTest
    {
        [Fact]
        public async Task CreateBeneficiary_ShouldSuccess_WhenValidInput()
        {
            //arrange 

            var httpClient = CreateClient();

            BeneficiaryDto beneficiaryDto = new BeneficiaryDto()
            {
                NickName = "Test",
                PhoneNumber = "9750587945"
            };

            //act
            var response = await httpClient.PostAsJsonAsync($"/api/v1/Beneficiary/user/1/create", beneficiaryDto);
            string id = await response.Content.ReadAsStringAsync();

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var beneficiary = await httpClient.GetAsync($"/api/v1/Beneficiary/{id}");

            beneficiary.StatusCode.Should().Be(HttpStatusCode.OK);

            var beneficiaryResponse = JsonConvert.DeserializeObject<BeneficiaryDto>(await beneficiary.Content.ReadAsStringAsync());

            beneficiaryResponse!.Id.ToString().Should().Be(id);
        }
    }
}