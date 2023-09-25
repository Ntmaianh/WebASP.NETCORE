using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    // khi cta sử dụng IdentityDBContext rồi thì nó sẽ tự tạo sẵn cho cta bảng User và Roles. Hai bảng này sẽ có 1 số
    // thuộc tính có sẵn => kiểm tra nếu đủ thì không phải tạo class User và Role nữa. Còn không thì chúng ta tạo class
    // ra và kế thừa từ lớp tạo sẵn để thêm những thuộc tính chúng ta muốn thêm vào ngoài những thuộc tính có sẵn 
    //
    public class AppUser : IdentityUser<Guid> // Guid là kiểu dữ liệu của Khóa chính 
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Dob { get; set; }

        public List<Cart> Carts { get; set; }

        public List<Oder> Orders { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
