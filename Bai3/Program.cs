using System;
using System.Collections.Generic;

// Lớp đại diện cho mô hình dữ liệu Sinh viên trong ứng dụng
public class Student
{
    // Mã sinh viên duy nhất dùng làm mốc định danh và tìm kiếm (chống trùng)
    public string Id { get; set; } = "";

    // Họ và tên đầy đủ dùng cho mục đích hiển thị và tìm kiếm chuỗi
    public string FullName { get; set; } = "";

    // Điểm tổng kết thuộc khoảng [0.0 - 10.0] dùng để xét duyệt kết quả học tập
    public double Score { get; set; }
}

class Program
{
    // Phương thức phụ: Tìm vị trí index của SV theo mã bằng thuật toán duyệt tuyến tính O(n) (Không dùng LINQ)
    static int FindIndexById(List<Student> students, string id)
    {
        // Duyệt tuần tự từng phần tử từ chỉ số 0 đến Count - 1 trong danh sách List
        for (int i = 0; i < students.Count; i++)
        {
            // So sánh chuỗi Id không phân biệt chữ hoa/thường để tránh lỗi do người dùng nhập nhầm định dạng
            if (students[i].Id.Equals(id, StringComparison.OrdinalIgnoreCase))
            {
                return i; // Trả về ngay chỉ số vị trí hợp lệ đầu tiên tìm thấy
            }
        }
        return -1; // Trả về -1 làm tín hiệu cờ báo không tìm thấy đối tượng trong danh sách
    }

    // Chức năng 1: Thêm sinh viên mới (Có kiểm tra chống trùng mã và lọc miền giá trị điểm)
    static void AddStudent(List<Student> students)
    {
        Console.Write("Nhap ma SV: ");
        string id = Console.ReadLine()!.Trim();

        // Kiểm tra trùng mã trước: Nếu kết quả khác -1 nghĩa là mã SV đã tồn tại -> Từ chối thêm
        if (FindIndexById(students, id) != -1)
        {
            Console.WriteLine("Loi: Ma SV da ton tai!");
            return; // Dừng ngay phương thức để bảo toàn tính duy nhất cho mã SV
        }

        Console.Write("Nhap ho ten: ");
        string fullName = Console.ReadLine()!.Trim();

        Console.Write("Nhap diem (0-10): ");
        // Ép kiểu chuỗi sang double kết hợp kiểm tra khoảng giá trị hợp lệ từ 0.0 đến 10.0
        if (!double.TryParse(Console.ReadLine(), out double score) || score < 0 || score > 10)
        {
            Console.WriteLine("Loi: Diem khong hop le!");
            return; // Bắt lỗi dữ liệu nhập không chuẩn và dừng thao tác chèn
        }

        // Khởi tạo đối tượng Student mới và chèn vào cuối List (thao tác List.Add tốn O(1) khấu hao)
        students.Add(new Student { Id = id, FullName = fullName, Score = score });
        Console.WriteLine("Them sinh vien thanh cong!");
    }

    // Chức năng 2: Cập nhật thông tin sinh viên theo mã SV
    static void UpdateStudent(List<Student> students)
    {
        Console.Write("Nhap ma SV can cap nhat: ");
        string id = Console.ReadLine()!.Trim();

        // Tìm vị trí phần tử cần sửa trong List
        int index = FindIndexById(students, id);
        if (index == -1)
        {
            Console.WriteLine("Loi: Khong tim thay SV!");
            return; // Dừng xử lý nếu mã không tồn tại trong hệ thống
        }

        // Lấy tham chiếu đối tượng Student tại chỉ số index tìm được
        Student st = students[index];

        Console.Write("Nhap ho ten moi (de chong neu giu nguyen): ");
        string newName = Console.ReadLine()!.Trim();
        // Nếu người dùng gõ chuỗi mới thì mới tiến hành ghi đè tên
        if (!string.IsNullOrEmpty(newName))
        {
            st.FullName = newName; // Thay đổi thuộc tính FullName trực tiếp trên vùng nhớ Heap
        }

        Console.Write("Nhap diem moi (de chong neu giu nguyen): ");
        string scoreInput = Console.ReadLine()!.Trim();
        // Chỉ cập nhật điểm nếu người dùng nhập chuỗi mới VÀ chuyển đổi thành công thành số thực trong [0, 10]
        if (!string.IsNullOrEmpty(scoreInput))
        {
            if (double.TryParse(scoreInput, out double newScore) && newScore >= 0 && newScore <= 10)
            {
                st.Score = newScore;
            }
            else
            {
                Console.WriteLine("Loi: Diem moi khong hop le! Giu nguyen diem cu.");
            }
        }

        Console.WriteLine("Cap nhat thanh cong!");
    }

    // Chức năng 3: Xóa sinh viên theo mã SV
    static void RemoveStudent(List<Student> students)
    {
        Console.Write("Nhap ma SV can xoa: ");
        string id = Console.ReadLine()!.Trim();

        // Tìm chỉ số vị trí cần xóa
        int index = FindIndexById(students, id);
        if (index == -1)
        {
            Console.WriteLine("Loi: Khong tim thay SV de xoa!");
            return; // Bắt lỗi mã sinh viên không tồn tại
        }

        // Xóa phần tử tại vị trí index (List<T> sẽ tự động dịch chuyển các phần tử phía sau sang trái 1 ô để lấp chỗ)
        students.RemoveAt(index);
        Console.WriteLine("Xoa sinh vien thanh cong!");
    }

    // Chức năng 4: Tìm kiếm theo một phần họ tên (Không dùng LINQ)
    static void SearchByName(List<Student> students)
    {
        Console.Write("Nhap tu khoa ho ten: ");
        string keyword = Console.ReadLine()!.Trim();

        bool found = false; // Biến cờ cắm ghi nhận có tìm thấy kết quả nào hay không
        Console.WriteLine("\n--- KET QUA TIM KIEM ---");

        // Duyệt tuyến tính O(n) qua toàn bộ danh sách để so sánh chuỗi
        for (int i = 0; i < students.Count; i++)
        {
            // Kiểm tra từ khóa có xuất hiện trong họ tên hay không (không phân biệt chữ hoa/thường)
            if (students[i].FullName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Console.WriteLine($"MSSV: {students[i].Id,-10} | Ten: {students[i].FullName,-20} | Diem: {students[i].Score:F2}");
                found = true; // Đánh dấu đã tìm thấy ít nhất 1 sinh viên thỏa mãn
            }
        }

        // Nếu duyệt hết danh sách mà biến cờ vẫn bằng false -> Thông báo không tìm thấy
        if (!found)
        {
            Console.WriteLine("Khong tim thay sinh vien phu hop!");
        }
    }

    // Chức năng 5: Hiển thị danh sách sinh viên kèm thống kê điểm
    static void DisplayAndStats(List<Student> students)
    {
        Console.WriteLine("\n================ DANH SACH SINH VIEN ================");
        
        // Xử lý trường hợp biên: Danh sách chưa có dữ liệu
        if (students.Count == 0)
        {
            Console.WriteLine("Danh sach hien dang rong!");
            Console.WriteLine("Diem trung binh: N/A");
            return; // Ngắt hàm để tránh phép chia cho 0 (sum / 0) gây lỗi chương trình
        }

        double sum = 0; // Biến tích lũy tổng điểm
        int pass = 0;  // Biến đếm số sinh viên đạt (điểm >= 5.0)

        // Duyệt 1 vòng duy nhất Theta(n) để vừa hiển thị vừa tính tổng điểm và đếm SV đạt
        for (int i = 0; i < students.Count; i++)
        {
            Console.WriteLine($"{i + 1}. MSSV: {students[i].Id,-10} | Ten: {students[i].FullName,-20} | Diem: {students[i].Score:F2}");
            sum += students[i].Score; // Cộng dồn điểm số vào tổng
            
            if (students[i].Score >= 5.0)
            {
                pass++; // Tăng biến đếm nếu sinh viên có điểm đạt từ 5.0 trở lên
            }
        }

        // Tính điểm trung bình bằng tổng điểm chia cho tổng số phần tử thực tế
        double avg = sum / students.Count;

        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"Tong so SV: {students.Count} | DTB: {avg:F2} | So SV dat (>= 5): {pass}");
    }

    static void Main()
    {
        // Khởi tạo List chứa sẵn 5 sinh viên mẫu làm dữ liệu ban đầu
        List<Student> students = new List<Student>
        {
            new Student { Id = "SV01", FullName = "Nguyen Van A", Score = 8.5 },
            new Student { Id = "SV02", FullName = "Tran Thi B", Score = 4.0 },
            new Student { Id = "SV03", FullName = "Le Hoang C", Score = 6.0 },
            new Student { Id = "SV04", FullName = "Pham Minh D", Score = 3.5 },
            new Student { Id = "SV05", FullName = "Vo Thanh E", Score = 9.0 }
        };

        // Vòng lặp vô hạn giữ chương trình tương tác liên tục cho đến khi người dùng chọn 0
        while (true)
        {
            Console.WriteLine("\n========== QUAN LY SINH VIEN ==========");
            Console.WriteLine("1. Them sinh vien");
            Console.WriteLine("2. Cap nhat theo ma");
            Console.WriteLine("3. Xoa theo ma");
            Console.WriteLine("4. Tim kiem theo ten");
            Console.WriteLine("5. Hien thi danh sach & Thong ke");
            Console.WriteLine("0. Thoat");
            Console.Write("Chon chuc nang (0-5): ");

            string choice = Console.ReadLine()!;
            switch (choice)
            {
                case "1": AddStudent(students); break;
                case "2": UpdateStudent(students); break;
                case "3": RemoveStudent(students); break;
                case "4": SearchByName(students); break;
                case "5": DisplayAndStats(students); break;
                case "0": return; // Thoát hàm Main và kết thúc chương trình
                default: Console.WriteLine("Chuc nang khong hop le!"); break;
            }
        }
    }
}