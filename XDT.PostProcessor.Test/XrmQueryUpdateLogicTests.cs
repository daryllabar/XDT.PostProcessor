//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace XDT.PostProcessor.Test
//{
//    [TestClass]
//    public class XrmQueryUpdateLogicTests
//    {
//        public XrmQueryUpdateLogic Sut { get; set; }

//        [TestInitialize]
//        public void Initialize()
//        {
//            Sut = new XrmQueryUpdateLogic(new Settings{
//                XrmQueryMakeWebpackCompatible = false
//            });
//        }

//        [TestMethod]
//        public void MakeWebpackCompatible_Should_AddLoad()
//        {
//            var settings = Sut.Settings;
//            settings.XrmQueryMakeWebpackCompatible = true;
//            var result = Sut.ProcessFile("<START><END>");
//            result.ShouldEqualWithDiff(@"<START><END>" + settings.WebpackPostfix);
//        }
//    }
//}