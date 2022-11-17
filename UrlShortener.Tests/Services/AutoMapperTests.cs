using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UrlShortener.Models.UrlModel;

namespace UrlShortener.Tests.Services
{
    public class AutoMapperTests
    {
        private readonly IMapper _sut;

        public AutoMapperTests() =>
            _sut = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UrlProfile>();
            }).CreateMapper();

        [Fact]
        public void All_mappings_should_be_setup_correctly() => _sut.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
