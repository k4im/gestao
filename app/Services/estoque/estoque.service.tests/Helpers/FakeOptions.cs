
using Microsoft.EntityFrameworkCore;

namespace estoque.service.tests.Helpers
{
    public class FakeOptions
    {
        public static DbContextOptionsBuilder factoryDbInMemory()
        {
            return new DbContextOptionsBuilder().UseInMemoryDatabase(new Guid().ToString());
        }
    }
}