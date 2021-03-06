using System.Threading.Tasks;
using PlaywrightSharp.Tests.BaseTests;
using PlaywrightSharp.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace PlaywrightSharp.Tests.Frame.JsHandle
{
    ///<playwright-file>jshandle.spec.js</playwright-file>
    ///<playwright-describe>JSHandle.asElement</playwright-describe>
    [Collection(TestConstants.TestFixtureBrowserCollectionName)]
    public class JSHandleAsElementTests : PlaywrightSharpPageBaseTest
    {
        /// <inheritdoc/>
        public JSHandleAsElementTests(ITestOutputHelper output) : base(output)
        {
        }

        [PlaywrightTest("jshandle.spec.js", "JSHandle.asElement", "should work")]
        [Fact(Timeout = TestConstants.DefaultTestTimeout)]
        public async Task ShouldWork()
        {
            var aHandle = await Page.EvaluateHandleAsync("() => document.body");
            var element = aHandle as IElementHandle;
            Assert.NotNull(element);
        }

        [PlaywrightTest("jshandle.spec.js", "JSHandle.asElement", "should return null for non-elements")]
        [Fact(Timeout = TestConstants.DefaultTestTimeout)]
        public async Task ShouldReturnNullForNonElements()
        {
            var aHandle = await Page.EvaluateHandleAsync("() => 2");
            var element = aHandle as IElementHandle;
            Assert.Null(element);
        }

        [PlaywrightTest("jshandle.spec.js", "JSHandle.asElement", "should return ElementHandle for TextNodes")]
        [Fact(Timeout = TestConstants.DefaultTestTimeout)]
        public async Task ShouldReturnElementHandleForTextNodes()
        {
            await Page.SetContentAsync("<div>ee!</div>");
            var aHandle = await Page.EvaluateHandleAsync("() => document.querySelector('div').firstChild");
            var element = aHandle as IElementHandle;
            Assert.NotNull(element);
            Assert.True(await Page.EvaluateAsync<bool>("e => e.nodeType === HTMLElement.TEXT_NODE", element));
        }

        [PlaywrightTest("jshandle.spec.js", "JSHandle.asElement", "should work with nullified Node")]
        [Fact(Timeout = TestConstants.DefaultTestTimeout)]
        public async Task ShouldWorkWithNullifiedNode()
        {
            await Page.SetContentAsync("<section>test</section>");
            await Page.EvaluateAsync("() => delete Node");
            var handle = await Page.EvaluateHandleAsync("() => document.querySelector('section')");
            var element = handle as IElementHandle;
            Assert.NotNull(element);
        }
    }
}
