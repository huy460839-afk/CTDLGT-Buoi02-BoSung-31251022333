using System;
using System.Text;
class Program
{
    static void Insert(int[] data, ref int count, int index, int value)
    {
        // 1. Kiểm tra vị trí chèn có hợp lệ không (chỉ chấp nhận từ 0 đến count)
       if (data == null)
       {
           throw new ArgumentNullException("data","Mang khong duoc null");
       }
       if (index < 0 || index > count)
       {
           throw new ArgumentOutOfRangeException("index","Vi tri khong hop le");
       }
       // 2. Kiểm tra mảng còn chỗ trống để chèn không
       if ( count >= data.Length)
       {
           throw new InvalidOperationException("Mang da day");
       }
       // 3. Dịch các phần tử từ phải sang trái (từ count-1 lùi về index)
        // Mục đích: Tạo vị trí trống tại index mà không làm đè/mất dữ liệu cũ
       for (int i = count; i > index; i--)
       {
           data[i] = data[i - 1];// Gán giá trị của ô phía trước cho ô phía sau
       }
       // 4. Gán giá trị mới vào vị trí index đã được làm trống
       data[index] = value;
       // 5. Tăng số lượng phần tử thực tế lên 1 SAU KHI đã dịch chuyển xong
       count++;
    }
    static void RemoveAt(int[] data, ref int count, int index)
    {
       // 1. Kiểm tra vị trí xóa có hợp lệ không (chỉ chấp nhận từ 0 đến count - 1)
       if (data == null)
       {
           throw new ArgumentNullException("data","Mang khong duoc null");
       }
       if (index < 0 || index >= count)
       {
           throw new ArgumentOutOfRangeException("index","Vi tri khong hop le");
       }
       // 2. Dịch các phần tử từ trái sang phải (từ index đến count - 2)
        // Mục đích: Lấp trống vị trí bị xóa bằng cách kéo các phần tử phía sau lên trước
       for (int i = index; i < count - 1; i++)
       {
           data[i] = data[i + 1];// Gán giá trị của ô phía sau cho ô phía trước
       }
       // 3. Giảm số lượng phần tử thực tế đi 1 SAU KHI đã lấp xong dữ liệu
       count--;
     // Phương thức hỗ trợ in mảng hợp lệ ra màn hình
    static void PrintArray(int[] data, int count)
    {
        Console.Write("[ ");
        for (int i = 0; i < count; i++)
        {
            Console.Write(data[i] + (i == count - 1 ? "" : ", "));
        }
        Console.WriteLine($" ] (count = {count}, Capacity = {data.Length})");
    }

    static void Main()
    {
        // Khởi tạo mảng ban đầu có sức chứa 10 ô, chứa sẵn 5 phần tử [4, 7, 2, 9, 5]
        int[] data = new int[10] { 4, 7, 2, 9, 5, 0, 0, 0, 0, 0 };
        int count = 5;

        Console.WriteLine("TRẠNG THÁI BAN ĐẦU CỦA MẢNG:");
        PrintArray(data, count);

        // 1. Ca kiểm thử theo đề bài: Chèn 99 tại index 2, sau đó xóa tại index 1
        Console.WriteLine(" CA KIỂM THỬ THƯỜNG");
        Insert(data, ref count, 2, 99);
        Console.Write("Sau khi chèn 99 tại index 2: ");
        PrintArray(data, count);

        RemoveAt(data, ref count, 1);
        Console.Write("Sau khi xóa phần tử tại index 1: ");
        PrintArray(data, count);

        // 2. Ca kiểm thử chèn/xóa tại đầu và cuối
        Console.WriteLine("CA KIỂM THỬ ĐẦU VÀ CUỐI MẢNG");
        Insert(data, ref count, 0, 100); // Chèn tại đầu
        Console.Write("Chèn 100 tại đầu (index 0): ");
        PrintArray(data, count);

        Insert(data, ref count, count, 200); // Chèn tại cuối
        Console.Write("Chèn 200 tại cuối (index = count): ");
        PrintArray(data, count);

        RemoveAt(data, ref count, 0); // Xóa tại đầu
        Console.Write("Xóa phần tử tại đầu (index 0): ");
        PrintArray(data, count);

        RemoveAt(data, ref count, count - 1); // Xóa tại cuối
        Console.Write("Xóa phần tử tại cuối (index = count - 1): ");
        PrintArray(data, count);

        // 3. Ca kiểm thử các trường hợp ngoại lệ (Ném lỗi)
        Console.WriteLine("\n--- CA KIỂM THỬ NGOẠI LỆ ---");
        try
        {
            Console.Write("Thử chèn chỉ số âm (index = -1): ");
            Insert(data, ref count, -1, 50);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"[Bắt lỗi thành công] {ex.ParamName}: {ex.Message}");
        }

        try
        {
            Console.Write("Thử xóa tại index = count: ");
            RemoveAt(data, ref count, count);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"[Bắt lỗi thành công] {ex.ParamName}: {ex.Message}");
        }

        try
        {
            Console.Write("Thử chèn vào mảng đã đầy: ");
            int[] fullArr = new int[3] { 1, 2, 3 };
            int fullCount = 3;
            Insert(fullArr, ref fullCount, 1, 99);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"[Bắt lỗi thành công] {ex.Message}");
        }
    }
}
}