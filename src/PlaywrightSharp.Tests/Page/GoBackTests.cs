using System.Threading.Tasks;
using PlaywrightSharp.Tests.BaseTests;
using PlaywrightSharp.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace PlaywrightSharp.Tests.Page
{
    ///<playwright-file>navigation.spec.js</playwright-file>
    ///<playwright-describe>Page.goBack</playwright-describe>
    [Collection(TestConstants.TestFixtureBrowserCollectionName)]
    public class GoBackTests : PlaywrightSharpPageBaseTest
    {
        /// <inheritdoc/>
        public GoBackTests(ITestOutputHelper output) : base(output)
        {
        }

        [PlaywrightTest("navigation.spec.js", "Page.goBack", "should work")]
        [Fact(Timeout = TestConstants.DefaultTestTimeout)]
        public async Task ShouldWork()
        {
            await Page.GoToAsync(TestConstants.EmptyPage);
            await Page.GoToAsync(TestConstants.ServerUrl + "/grid.html");

            var response = await Page.GoBackAsync();
            Assert.True(response.Ok);
            Assert.Contains(TestConstants.EmptyPage, response.Url);

            response = await Page.GoForwardAsync();
            Assert.True(response.Ok);
            Assert.Contains("/grid.html", response.Url);

            response = await Page.GoForwardAsync();
            Assert.Null(response);
        }

        [PlaywrightTest("navigation.spec.js", "Page.goBack", "should work with HistoryAPI")]
        [Fact(Timeout = TestConstants.DefaultTestTimeout)]
        public async Task ShouldWorkWithHistoryAPI()
        {
            await Page.GoToAsync(TestConstants.EmptyPage);
            await Page.EvaluateAsync(@"
              history.pushState({ }, '', '/first.html');
              history.pushState({ }, '', '/second.html');
            ");
            Assert.Equal(TestConstants.ServerUrl + "/second.html", Page.Url);

            await Page.GoBackAsync();
            Assert.Equal(TestConstants.ServerUrl + "/first.html", Page.Url);
            await Page.GoBackAsync();
            Assert.Equal(TestConstants.EmptyPage, Page.Url);
            await Page.GoForwardAsync();
            Assert.Equal(TestConstants.ServerUrl + "/first.html", Page.Url);
        }

        [PlaywrightTest("navigation.spec.js", "Page.goBack", "should work for file urls")]
        [Fact(Skip = "We need screenshots for this")]
        public void ShouldWorkForFileUrls()
        {
        }
    }
}
