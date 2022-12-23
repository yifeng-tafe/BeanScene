namespace BeanSceneMSUnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async void TestMethod1()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "BeanScene")
                .Options;

            var context = new ApplicationDBContext(options);

            var c = new Category();

            c.Name = "Lunch";
            c.Id = 10;

            context.Add(c);
            context.SaveChanges();

            var controller = new CategoryController(context);

            // Act
            var result = await controller.Index() as ViewResult;
            var model = result.ViewData.Model;

            // Assert
            Assert.Equals(model, c);
        }
    }
}