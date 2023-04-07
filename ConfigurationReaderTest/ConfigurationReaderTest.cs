using ConfigurationLibrary;
using Xunit;

namespace ConfigurationReaderTest
{
        public class ConfigurationReaderTest
        {
            [InlineData("SERVICE-A", "Boyner.com.tr")]
            [Theory]
            public async Task Should_Get_Expected_SiteName(string applicationName, string expectedSiteName)
            {
                var configReader = new ConfigurationReader(applicationName,"mongodb://localhost:27017",5000);

                var siteName =  configReader.GetValue<string>("SiteName");

                Assert.Equal(expectedSiteName, siteName);
            }
        }
    
}