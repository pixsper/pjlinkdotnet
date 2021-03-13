using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pixsper.PJLinkDotNet.Tests
{
    [TestClass]
    public class CommandBodyValueTests
    {
        [TestMethod]
        public void CanGetCommandMetadata()
        {
            var commands = CommandBodyExtensions.CommandBodies.Any();
        }
    }
}
