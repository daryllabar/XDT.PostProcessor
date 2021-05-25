using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XDT.PostProcessor.Test
{
    [TestClass]
    public class XrmQueryUpdateLogicTests
    {
        public XrmQueryUpdateLogic Sut { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Sut = new XrmQueryUpdateLogic(new Settings{
                XrmQueryMakeEs6Compatible = false,
                XrmQueryMakeWebpackCompatible = false
            });
        }

        [TestMethod]
        public void MakeEs6Compatible_Should_ReplaceRegEx()
        {
            var settings = Sut.Settings;
            settings.XrmQueryMakeEs6Compatible = true;
            Assert.IsNotNull(settings.Es5RegExParser);
            Assert.IsNotNull(settings.Es6RegExParser);
            var result = Sut.ProcessFile($"<START> {settings.Es5RegExParser} <END>");
            result.ShouldEqualWithDiff($"<START> {settings.Es6RegExParser} <END>");
        }

        [TestMethod]
        public void MakeWebpackCompatible_Should_AddLoad()
        {
            var settings = Sut.Settings;
            settings.XrmQueryMakeWebpackCompatible = true;
            var result = Sut.ProcessFile("<START><END>");
            result.ShouldEqualWithDiff(@"<START><END>" + settings.WebpackPostfix);
        }
    }
}