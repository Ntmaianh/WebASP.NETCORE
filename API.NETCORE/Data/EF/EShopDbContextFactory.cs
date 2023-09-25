using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF
{
    // Lớp này để kết nối Cofiguration với Sql (Tạo Migration )
    // để khi chạy Migraytion nó biết nó cần phải lấy dữ liệu ở đâu 
    // IDesignTimeDbContextFactory kế thừa từ lớp này và truyền DB muốn Migration vào trong cặp ngoặc <>
    internal class EShopDbContextFactory : IDesignTimeDbContextFactory<ESHOPDBContext>
    {
        public ESHOPDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              // lỗi ở SetBasePath thì là chưa cài nuget Microsoft.Extensions.Configuration.FileExtensions
              .SetBasePath(Directory.GetCurrentDirectory())
               // lỗi ở AddJsonFile thì là chưa cài nuget Microsoft.Extensions.Configuration.Json
               .AddJsonFile("Appsetting.json")
               .Build();

            // lấy cái connectionString từ appsetting 
            var connectionString = configuration.GetConnectionString("Database"); // tên của ConnectionSting 

            var optionsBuilder = new DbContextOptionsBuilder<ESHOPDBContext>(); // tên DB muốn migration
            optionsBuilder.UseSqlServer(connectionString); // sử dụng sql sever để kết nối 

            return new ESHOPDBContext(optionsBuilder.Options);
        }
    }
}
