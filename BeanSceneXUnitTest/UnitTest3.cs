using BeanScene.ViewModels;

namespace BeanSceneXUnitTest
{
    public class UnitTest3
    {

        [Fact]

        public async void Create_Table_Test()
        {

            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "Create_Table_Test")
                .Options;

            var context = new ApplicationDBContext(options);

            // Create areas
            Area area1 = new Area() { Id = 1, Name = "Main" };
            context.Add(area1);
            //context.SaveChanges();

            Area area2 = new Area() { Id = 2, Name = "Different" };
            context.Add(area2);
            context.SaveChanges();


            // Get all areas from DB
            List<Area> areas = await context.Area.ToListAsync();


            // Create table
            Table table = new Table() {
                Name = "Test Table",
                AreaID = area1.Id,
                Areas = area1
            };



            var controller = new TableController(context);


            // Act
            var tableVM = new TableViewModel ()
            {
                 Table = table,
                 Areas = areas
            };

            // Run the method being tested
            var result = await controller.Create(tableVM);
            //var model = result.ViewData.Model;

            // Assert
            //var viewResult = Assert.IsType<ViewResult>(result);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            //var model = Assert.IsAssignableFrom<IEnumerable<Table>>(
            //    viewResult.ViewData.Model);
            //Assert.Equal(1, model.Count());

            // Get tables from DB
            List<Table> tables = await context.Table.ToListAsync();

            // Check collection size
            //Assert.Equal(2, tables.Count);
            Assert.Single(tables);
            //Assert.NotEmpty(tables);

            // Check every item of the collection one-by-one...
            //Assert.Collection<Table>(tables, )
            Assert.Contains<Table>(table, tables);
        }
    }
}