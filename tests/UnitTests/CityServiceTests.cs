using Xunit;
using Application.Services;
using FluentAssertions;
using Common.Testing.Extensions.AutoFixture;
using AutoFixture.Xunit2;

namespace UnitTests
{
    public class CityServiceTests
    {
        [InlineData("LAS PALMAS DE G. CANARIA", "LAS PALMAS DE GRAN CANARIA", true)]
        [InlineData("POSADA DE VALDEON", "PRADA DE VALDEON", true)]
        [InlineData("LESŸROQUETES", "LES ROQUETES", true)]
        [InlineData("PE#ISCOLA", "PEÑISCOLA", true)]
        [InlineData("STA EULALIA DEL RIO NEGRO", "SANTA EULALIA DEL RIO NEGRO", true)]
        [InlineData("BILBAO", "BILBO", true)]
        [InlineData("BARACALDO", "BARAKALDO", true)]
        [InlineData("TERRASSA", "TRERASSA", true)]
        [InlineData("TERRASSA", "TRRASSA", true)]
        [InlineData("TERRASSA", "TARRASA", true)]
        [InlineData("TERRASSA", "TARRASSA", true)]
        [InlineData("MANRESA", "MANRESA", true)] //Exact case
        [InlineData("Huesca", "Huéscar", false)]
        [InlineData("Cádiz", "Cáceres", false)]
        [InlineData("Logroño", "Logrosán", false)]
        [Theory]
        public void IsSameCityTests(string city, string candidate, bool expected)
        {
            //When
            var result = CityService.IsSameCity(city, candidate);

            //Then
            result.Should().Be(expected);
        }

        [Theory, AutoData]
        public void IsSameCityGuardClauseTests(StringGuardClauseAssertion assertion)
        {
            var sut = typeof(CityService).GetMethod(nameof(CityService.IsSameCity));
            assertion.Verify(sut);
        }
    }
}