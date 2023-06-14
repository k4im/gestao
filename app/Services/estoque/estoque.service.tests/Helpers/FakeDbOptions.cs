using Microsoft.EntityFrameworkCore;

namespace estoque.service.tests.Helpers
{
    public class FakeDbOptions
    {
        public static DbContextOptionsBuilder factoryDbInMemory()
        {
            return new DbContextOptionsBuilder().UseInMemoryDatabase(new Guid().ToString());
        }
    }
}